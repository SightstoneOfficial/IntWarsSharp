﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.Missiles;
using LeagueSandbox.GameServer.GameObjects.Other;
using LeagueSandbox.GameServer.Packets;

namespace LeagueSandbox.GameServer.GameObjects
{ 
    public class GameObject : Target, IGameObject
    {
        protected bool _movementUpdated;
        protected bool _toRemove;
        public uint NetId { get; }
        public uint SyncId { get; }

        private static uint MOVEMENT_EPSILON = 5;
        private int _waypointIndex;

        public List<Vector2> Waypoints { get; private set; }
        
        public TeamId Team { get; protected set; }
        public void SetTeam(TeamId team)
        {
            _visibleByTeam[Team] = false;
            Team = team;
            _visibleByTeam[Team] = true;
            if (_game.IsRunning)
            {
                _game.PacketNotifier.NotifySetTeam(this as IAttackableUnit, team);
            }
        }

        public float CollisionRadius { get; set; }
        public float VisionRadius { get; protected set; }
        public override bool IsSimpleTarget => false;
        protected Vector2 _direction;
        private Dictionary<TeamId, bool> _visibleByTeam;
        protected Game _game;
        protected NetworkIdManager _networkIdManager;

        /// <summary>
        /// Current target the object running to (can be coordinates or an object)
        /// </summary>
        public ITarget Target { get; set; }

        public GameObject(Game game, float x, float y, int collisionRadius, int visionRadius = 0, uint netId = 0) : base(x, y)
        {
            _game = game;
            _networkIdManager = game.NetworkIdManager;
            if (netId != 0)
            {
                NetId = netId; // Custom netId
            }
            else
            {
                NetId = _networkIdManager.GetNewNetId(); // Let the base class (this one) asign a netId
            }
            SyncId = (uint) Environment.TickCount; // TODO: use movement manager to generate those
            Target = null;
            CollisionRadius = collisionRadius;
            VisionRadius = visionRadius;
            Waypoints = new List<Vector2>();
            _waypointIndex = 0;

            _visibleByTeam = new Dictionary<TeamId, bool>();
            var teams = Enum.GetValues(typeof(TeamId)).Cast<TeamId>();
            foreach (var team in teams)
            {
                _visibleByTeam.Add(team, false);
            }

            Team = TeamId.TEAM_NEUTRAL;
            _movementUpdated = false;
            _toRemove = false;
        }

        public virtual void OnAdded()
        {
            _game.Map.CollisionHandler.AddObject(this);
        }

        public virtual void OnRemoved()
        {
            _game.Map.CollisionHandler.RemoveObject(this);
        }

        public virtual void OnCollision(IGameObject collider)
        {
        }

        public void Move(float diff)
        {
            // no waypoints remained - return
            if (_waypointIndex>=Waypoints.Count)
            {
                _direction = new Vector2();
                return;
            }
            var next = Waypoints[_waypointIndex];

            // current position
            var cur = new Vector2(X, Y);
            var goingTo = next - cur;
            _direction = Vector2.Normalize(goingTo);
            // shouldn't happen usually
            if (float.IsNaN(_direction.X) || float.IsNaN(_direction.Y))
            {
                _direction = new Vector2(0, 0);
            }

            var moveSpeed = GetMoveSpeed();
            var deltaMovement = moveSpeed * 0.001f * diff;
            var xx = _direction.X * deltaMovement;
            var yy = _direction.Y * deltaMovement;
            X += xx;
            Y += yy;
            // now (X,Y) moved towards next position
            cur = new Vector2(X, Y);

            // Checks if we reached the next waypoint
            // REVIEW: (deltaMovement * 2) used here is problematic cause if the server lagged, diff is much greater than usual values
            if ((cur-next).LengthSquared() < MOVEMENT_EPSILON* MOVEMENT_EPSILON)
            {
                // remove this waypoint cause we reached it
                _waypointIndex++;
            }
        }

        /// <summary>
        /// Moves the object depending on its target, updating its coordinate.
        /// </summary>
        /// <param name="diff">The amount of milliseconds the object is supposed to move</param>
        public void Move(float diff, Vector2 to)
        {
            
        }

        public virtual void Update(float diff)
        {
            Move(diff);
        }

        public virtual float GetMoveSpeed()
        {
            return 0;
        }

        public void SetWaypoints(List<Vector2> newWaypoints)
        {
            Waypoints = newWaypoints;
            _waypointIndex = 1;
            _movementUpdated = true;
        }

        public bool IsMovementUpdated()
        {
            return _movementUpdated;
        }

        public void ClearMovementUpdated()
        {
            _movementUpdated = false;
        }

        public bool IsToRemove()
        {
            return _toRemove;
        }

        public virtual void SetToRemove()
        {
            _toRemove = true;
        }

        public virtual void SetPosition(float x, float y)
        {
            Console.WriteLine("Float Position changed: " + x + "," + y);
            X = x;
            Y = y;

            Target = null;
        }

        public virtual void SetPosition(Vector2 vec)
        {
            Console.WriteLine("Vector Position changed: " + vec.X + "," + vec.Y);
            X = vec.X;
            Y = vec.Y;
            Target = null;
        }

        public virtual float GetZ()
        {
            return _game.Map.NavGrid.GetHeightAtLocation(X, Y);
        }

        public bool IsCollidingWith(IGameObject o)
        {
            return GetDistanceToSqr(o) < (CollisionRadius + o.CollisionRadius) * (CollisionRadius + o.CollisionRadius);
        }

        public bool IsVisibleByTeam(TeamId team)
        {
            return team == Team || _visibleByTeam[team];
        }

        public void SetVisibleByTeam(TeamId team, bool visible)
        {
            _visibleByTeam[team] = visible;

            if (this is IAttackableUnit)
            {
                // TODO: send this in one place only
                _game.PacketNotifier.NotifyUpdatedStats(this as IAttackableUnit, false);
            }
        }
    }
}
