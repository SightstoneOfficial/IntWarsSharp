using System;
using LeagueSandbox.GameServer.Logic.Packets.PacketHandlers;

namespace LeagueSandbox.GameServer.Logic.Packets.PacketDefinitions.S2C
{
    public class TeleportRequest : BasePacket
    {
        private static short _a = 0x01;
        public TeleportRequest(Game game, uint netId, float x, float y, bool first)
            : base(game, PacketCmd.PKT_S2C_MOVE_ANS)
        {
            Write(Environment.TickCount); // syncID
            Write((byte)0x01); // Unk
            Write((byte)0x00); // Unk
            if (first) //seems to be id, 02 = before teleporting, 03 = do teleport
                Write((byte)0x02);
            else
                Write((byte)0x03);
            Write((int)netId);
            if (first == false)
            {
                Write((byte)_a); // if it is the second part, send 0x01 before coords
                _a++;
            }
            Write((short)x);
            Write((short)y);
        }

    }
}