﻿using ENet;
using LeagueSandbox.GameServer.Logic.Logging;
using LeagueSandbox.GameServer.Logic.Packets.PacketDefinitions.S2C;
using LeagueSandbox.GameServer.Logic.Players;

namespace LeagueSandbox.GameServer.Logic.Packets.PacketHandlers
{
    public class HandleScoreboard : PacketHandlerBase
    {
        private readonly PlayerManager _playerManager;
        private readonly ILogger _logger;
        private readonly Game _game;

        public override PacketCmd PacketType => PacketCmd.PKT_C2S_SCOREBOARD;
        public override Channel PacketChannel => Channel.CHL_C2S;

        public HandleScoreboard(Game game)
        {
            _game = game;
            _playerManager = game.PlayerManager;
            _logger = LoggerProvider.GetLogger();
        }

        public override bool HandlePacket(Peer peer, byte[] data)
        {
            _logger.Info($"Player {_playerManager.GetPeerInfo(peer).Name} has looked at the scoreboard.");
            // Send to that player stats packet
            var response = new PlayerStats(_game, _playerManager.GetPeerInfo(peer).Champion);
            // TODO: research how to send the packet
            return _game.PacketHandlerManager.BroadcastPacket(response, Channel.CHL_S2C);
        }
    }
}
