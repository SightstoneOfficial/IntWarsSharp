using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Collections.Generic;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace GangplankE
{
    internal class GangplankE : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private List<IParticle> Particles => new List<IParticle>();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            StatsModifier.AttackSpeed.PercentBonus = StatsModifier.AttackSpeed.PercentBonus + (10f + 20f * ownerSpell.CastInfo.SpellLevel) / 100f;
            StatsModifier.MoveSpeed.PercentBonus = StatsModifier.MoveSpeed.PercentBonus + (10f + 5f * ownerSpell.CastInfo.SpellLevel) / 100f;
            StatsModifier.AttackDamage.PercentBonus = StatsModifier.AttackDamage.PercentBonus + (10f + 10f * ownerSpell.CastInfo.SpellLevel) / 100f;
            unit.AddStatModifier(StatsModifier);

            var time = 7.0f;

            //_hudvisual = AddBuffHUDVisual("RaiseMorale", time, 1, unit);

            Particles.Add(AddParticleTarget(ownerSpell.CastInfo.Owner, "pirate_raiseMorale_cas.troy", unit, 1));
            Particles.Add(AddParticleTarget(ownerSpell.CastInfo.Owner, "pirate_raiseMorale_mis.troy", unit, 1));
            Particles.Add(AddParticleTarget(ownerSpell.CastInfo.Owner, "pirate_raiseMorale_tar.troy", unit, 1));
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //RemoveBuffHudVisual(_hudvisual);
            Particles.ForEach(particle => RemoveParticle(particle));
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
