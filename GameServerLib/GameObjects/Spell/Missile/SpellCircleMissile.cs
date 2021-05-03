﻿using System;
using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Content;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Content;

namespace LeagueSandbox.GameServer.GameObjects.Spell.Missile
{
    public class SpellCircleMissile : SpellMissile, ISpellCircleMissile
    {
        // Function Vars.
        private bool _atDestination;

        public override MissileType Type { get; protected set; } = MissileType.Circle;
        /// <summary>
        /// Number of objects this projectile has hit since it was created.
        /// </summary>
        public List<IGameObject> ObjectsHit { get; }
        /// <summary>
        /// Position this projectile is moving towards. Projectile is destroyed once it reaches this destination. Equals Vector2.Zero if TargetUnit is not null.
        /// </summary>
        public Vector2 Destination { get; protected set; }

        public SpellCircleMissile(
            Game game,
            int collisionRadius,
            ISpell originSpell,
            ICastInfo castInfo,
            float moveSpeed,
            SpellDataFlags overrideFlags = 0, // TODO: Find a use for these
            uint netId = 0,
            bool serverOnly = false
        ) : base(game, collisionRadius, originSpell, castInfo, moveSpeed,  overrideFlags, netId, serverOnly)
        {
            // TODO: Verify if there is a case which contradicts this.
            // Line and Circle Missiles are location targeted only.
            TargetUnit = null;

            Position = new Vector2(castInfo.SpellCastLaunchPosition.X, castInfo.SpellCastLaunchPosition.Z);

            var goingTo = new Vector2(castInfo.TargetPositionEnd.X, castInfo.TargetPositionEnd.Z) - Position;
            var dirTemp = Vector2.Normalize(goingTo);
            var endPos = Position + (dirTemp * SpellOrigin.GetCurrentCastRange());

            // usually doesn't happen
            if (float.IsNaN(dirTemp.X) || float.IsNaN(dirTemp.Y))
            {
                if (float.IsNaN(CastInfo.Owner.Direction.X) || float.IsNaN(CastInfo.Owner.Direction.Y))
                {
                    dirTemp = new Vector2(1, 0);
                }
                else
                {
                    dirTemp = new Vector2(CastInfo.Owner.Direction.X, CastInfo.Owner.Direction.Z);
                }

                endPos = Position + (dirTemp * SpellOrigin.GetCurrentCastRange());
                CastInfo.TargetPositionEnd = new Vector3(endPos.X, 0, endPos.Y);
            }

            // TODO: Verify if CastRangeDisplayOverride is the correct variable to use.
            Destination = endPos;

            ObjectsHit = new List<IGameObject>();
        }

        public override void Update(float diff)
        {
            if (!HasDestination() || _atDestination)
            {
                SetToRemove();
                return;
            }

            Move(diff);
        }

        public override void OnCollision(IGameObject collider, bool isTerrain = false)
        {
            if (IsToRemove() || (Destination != Vector2.Zero && collider is IObjBuilding))
            {
                return;
            }

            if (isTerrain)
            {
                // TODO: Implement methods for isTerrain for projectiles such as Nautilus Q, ShyvanaDragon Q, or Ziggs Q.
                return;
            }

            if (Destination != Vector2.Zero)
            {
                CheckFlagsForUnit(collider as IAttackableUnit);
            }
        }

        /// <summary>
        /// Moves this projectile to either its target unit, or its destination, and updates its coordinates along the way.
        /// </summary>
        /// <param name="diff">The amount of milliseconds the AI is supposed to move</param>
        public override void Move(float diff)
        {
            // current position
            var cur = new Vector2(Position.X, Position.Y);

            var next = Destination;

            var goingTo = new Vector3(next.X, _game.Map.NavigationGrid.GetHeightAtLocation(next.X, next.Y), next.Y) - new Vector3(cur.X, _game.Map.NavigationGrid.GetHeightAtLocation(cur.X, cur.Y), cur.Y);
            var dirTemp = Vector3.Normalize(goingTo);

            // usually doesn't happen
            if (float.IsNaN(dirTemp.X) || float.IsNaN(dirTemp.Y) || float.IsNaN(dirTemp.Z))
            {
                dirTemp = new Vector3(0, 0, 0);
            }

            Direction = dirTemp;

            var moveSpeed = GetSpeed();

            var dist = MathF.Abs(Vector2.Distance(cur, next));

            var deltaMovement = moveSpeed * 0.001f * diff;

            // Prevent moving past the next waypoint.
            if (deltaMovement > dist)
            {
                deltaMovement = dist;
            }

            var xx = Direction.X * deltaMovement;
            var yy = Direction.Z * deltaMovement;

            Position = new Vector2(Position.X + xx, Position.Y + yy);

            // (X, Y) has moved to the next position
            cur = new Vector2(Position.X, Position.Y);

            // Check if we reached the target position/destination.
            // REVIEW (of previous code): (deltaMovement * 2) being used here is problematic; if the server lags, the diff will be much greater than the usual values
            if ((cur - next).LengthSquared() < MOVEMENT_EPSILON * MOVEMENT_EPSILON)
            {
                // remove this projectile because it has reached its destination
                if (Position == Destination)
                {
                    _atDestination = true;
                }
            }
        }

        public override void CheckFlagsForUnit(IAttackableUnit unit)
        {
            if (!HasDestination() || !IsValidTarget(unit))
            {
                return;
            }

            ObjectsHit.Add(unit);

            if (SpellOrigin != null)
            {
                SpellOrigin.ApplyEffects(unit, this);
            }

            if (CastInfo.Owner is IObjAiBase ai && SpellOrigin.CastInfo.IsAutoAttack)
            {
                ai.AutoAttackHit(TargetUnit);
            }
        }

        public override void SetToRemove()
        {
            base.SetToRemove();

            _game.PacketNotifier.NotifyDestroyClientMissile(this);
        }

        protected override bool IsValidTarget(IAttackableUnit unit)
        {
            if (unit == null || ObjectsHit.Contains(unit))
            {
                return false;
            }

            bool valid = false;

            if (unit.Team == CastInfo.Owner.Team && !SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectFriends))
            {
                valid = false;
            }

            if (unit.Team != CastInfo.Owner.Team && unit.Team != TeamId.TEAM_NEUTRAL && !SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectEnemies))
            {
                valid = false;
            }

            if (unit.IsDead && !SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectDead))
            {
                valid = false;
            }

            if (SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectAllUnitTypes))
            {
                valid = true;
            }
            else
            {
                switch (unit)
                {
                    // TODO: Verify all
                    // Order is important
                    case ILaneMinion _ when SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectMinions)
                                    && !SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.IgnoreLaneMinion):
                        valid = true;
                        break;
                    case IMinion m when (!m.IsPet && SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectNotPet))
                                || (m.IsPet && SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectUseable))
                                || (m.IsWard && SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectWards))
                                || (!m.IsClone && SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.IgnoreClones))
                                || (unit.Team == CastInfo.Owner.Team && !SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.IgnoreAllyMinion))
                                || (unit.Team != CastInfo.Owner.Team && unit.Team != TeamId.TEAM_NEUTRAL && !SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.IgnoreEnemyMinion))
                                || SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectMinions):
                        if (!(unit is ILaneMinion))
                        {
                            valid = true;
                            break;
                        }
                        // already got checked in ILaneMinion
                        valid = false;
                        break;
                    case IBaseTurret _ when SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectTurrets):
                        valid = true;
                        break;
                    case IInhibitor _ when SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectBuildings):
                        valid = true;
                        break;
                    case INexus _ when SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectBuildings):
                        valid = true;
                        break;
                    case IChampion _ when SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectHeroes):
                        valid = true;
                        break;
                    default:
                        valid = false;
                        break;
                }

                // TODO: Verify if placing this here is okay.
                if (unit.Team == TeamId.TEAM_NEUTRAL && !SpellOrigin.SpellData.Flags.HasFlag(SpellDataFlags.AffectNeutral))
                {
                    valid = false;
                }
            }

            return valid;
        }

        /// <summary>
        /// Whether or not this projectile has a destination; if it is a valid projectile.
        /// </summary>
        /// <returns>True/False.</returns>
        public bool HasDestination()
        {
            return Destination != Vector2.Zero && Destination.X != float.NaN && Destination.Y != float.NaN;
        }
    }
}
