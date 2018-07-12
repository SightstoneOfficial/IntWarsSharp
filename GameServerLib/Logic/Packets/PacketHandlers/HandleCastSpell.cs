﻿using ENet;
using LeagueSandbox.GameServer.Logic.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Logic.Packets.PacketDefinitions.C2S;
using LeagueSandbox.GameServer.Logic.Players;

namespace LeagueSandbox.GameServer.Logic.Packets.PacketHandlers
{
    public class HandleCastSpell : PacketHandlerBase
    {
        public override PacketCmd PacketType => PacketCmd.PKT_C2S_CAST_SPELL;
        public override Channel PacketChannel => Channel.CHL_C2_S;

        public override bool HandlePacket(Peer peer, byte[] data)
        {
            var spell = new CastSpellRequest(data);

            var targetObj = Game.ObjectManager.GetObjectById(spell.TargetNetId);
            var targetUnit = targetObj as AttackableUnit;
            var owner = PlayerManager.GetPeerInfo(peer).Champion;
            if (owner == null || !owner.CanCast())
            {
                return false;
            }

            var s = owner.GetSpell(spell.SpellSlot);
            if (s == null)
            {
                return false;
            }

            return s.Cast(spell.X, spell.Y, spell.X2, spell.Y2, targetUnit);
        }
    }
}
