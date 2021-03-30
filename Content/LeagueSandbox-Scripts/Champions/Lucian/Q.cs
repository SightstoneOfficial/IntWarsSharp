using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;

namespace Spells
{
    public class LucianQ : IGameScript
    {
        public void OnActivate(IObjAiBase owner)
        {
        }

        public void OnDeactivate(IObjAiBase owner)
        {
        }

        public void OnStartCasting(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            var range = to * 1100;
            var trueCoords = current + range;

            spell.AddLaser("LucianQ", trueCoords);
            spell.SpellAnimation("SPELL1", owner);
            AddParticle(owner, "Lucian_Q_laser.troy", trueCoords);
            AddParticleTarget(owner, "Lucian_Q_cas.troy", owner);
        }

        public void OnFinishCasting(IObjAiBase owner, ISpell spell, IAttackableUnit target)
        {
        }

        public void ApplyEffects(IObjAiBase owner, IAttackableUnit target, ISpell spell, ISpellMissile projectile)
        {
            if (spell is null)
            {
                throw new System.ArgumentNullException(nameof(spell));
            }

            var damage = owner.Stats.AttackDamage.Total * (0.45f + spell.Level * 0.15f) + (50 + spell.Level * 30);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL,
                false);
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
