﻿using GameServerCore;
using GameServerCore.Packets.Enums;
using GameServerCore.Packets.Handlers;
using LeagueSandbox.GameServer.Chatbox;
using GameServerCore.Packets.PacketDefinitions.Requests;

namespace LeagueSandbox.GameServer.Packets.PacketHandlers
{
    public class HandleBlueTipClicked : PacketHandlerBase<BlueTipClickedRequest>
    {
        private readonly Game _game;
        private readonly ChatCommandManager _chatCommandManager;
        private readonly IPlayerManager _playerManager;

        public override PacketCmd PacketType => PacketCmd.PKT_C2S_BLUE_TIP_CLICKED;
        public override Channel PacketChannel => Channel.CHL_C2S;

        public HandleBlueTipClicked(Game game)
        {
            _game = game;
            _chatCommandManager = game.ChatCommandManager;
            _playerManager = game.PlayerManager;
        }

        public override bool HandlePacket(int userId, BlueTipClickedRequest req)
        {
            // TODO: can we use player net id from request?
            var playerNetId = _playerManager.GetPeerInfo(userId).Champion.NetId;
             _game.PacketNotifier.NotifyBlueTip(userId, "", "", "", 0, playerNetId, req.NetId);

            var msg = $"Clicked blue tip with netid: {req.NetId}";
            _chatCommandManager.SendDebugMsgFormatted(DebugMsgType.NORMAL, msg);
            return true;
        }
    }
}
