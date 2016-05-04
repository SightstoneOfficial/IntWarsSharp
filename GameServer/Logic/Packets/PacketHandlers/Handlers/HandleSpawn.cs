﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENet;
using LeagueSandbox.GameServer.Logic.Packets;
using LeagueSandbox.GameServer.Logic.Enet;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.Items;
using LeagueSandbox.GameServer.Logic.Content;

namespace LeagueSandbox.GameServer.Core.Logic.PacketHandlers.Packets
{
    class HandleSpawn : IPacketHandler
    {
        public  bool HandlePacket(Peer peer, byte[] data, Game game)
        {
            var start = new StatePacket2(PacketCmdS2C.PKT_S2C_StartSpawn);
            PacketHandlerManager.getInstace().sendPacket(peer, start, Channel.CHL_S2C);
            Logger.LogCoreInfo("Spawning map");

            int playerId = 0;
            foreach (var p in game.getPlayers())
            {
                var spawn = new HeroSpawn(p.Item2, playerId++);
                PacketHandlerManager.getInstace().sendPacket(peer, spawn, Channel.CHL_S2C);

                var info = new PlayerInfo(p.Item2);
                PacketHandlerManager.getInstace().sendPacket(peer, info, Channel.CHL_S2C);
            }

            var peerInfo = game.getPeerInfo(peer);
            var bluePill = game.ItemManager.GetItemType(game.getMap().getBluePillId());
            var itemInstance = peerInfo.getChampion().Inventory.SetExtraItem(7, bluePill);
            var buyItem = new BuyItemAns(peerInfo.getChampion(), itemInstance);
            PacketHandlerManager.getInstace().sendPacket(peer, buyItem, Channel.CHL_S2C);

            // Not sure why both 7 and 14 skill slot, but it does not seem to work without it
            var skillUp = new SkillUpPacket(peerInfo.getChampion().getNetId(), 7, 1, (byte)peerInfo.getChampion().getSkillPoints());
            PacketHandlerManager.getInstace().sendPacket(peer, skillUp, Channel.CHL_GAMEPLAY);
            skillUp = new SkillUpPacket(peerInfo.getChampion().getNetId(), 14, 1, (byte)peerInfo.getChampion().getSkillPoints());
            PacketHandlerManager.getInstace().sendPacket(peer, skillUp, Channel.CHL_GAMEPLAY);

            peerInfo.getChampion().getStats().setSpellEnabled(7, true);
            peerInfo.getChampion().getStats().setSpellEnabled(14, true);
            peerInfo.getChampion().getStats().setSummonerSpellEnabled(0, true);
            peerInfo.getChampion().getStats().setSummonerSpellEnabled(1, true);

            var objects = game.getMap().GetObjects();
            foreach (var kv in objects)
            {
                if (kv.Value is Turret)
                {
                    var t = kv.Value as Turret;
                    var turretSpawn = new TurretSpawn(t);
                    PacketHandlerManager.getInstace().sendPacket(peer, turretSpawn, Channel.CHL_S2C);

                    // To suppress game HP-related errors for enemy turrets out of vision
                    var sh = new SetHealth(t);
                    PacketHandlerManager.getInstace().sendPacket(peer, sh, Channel.CHL_S2C);

                    var u = kv.Value as Unit;
                    if (u != null)
                    {
                        PacketNotifier.notifyUpdatedStats(u, false);
                    }

                    continue;
                }
                else if (kv.Value is LevelProp)
                {
                    var lp = kv.Value as LevelProp;

                    var lpsPacket = new LevelPropSpawn(lp);
                    PacketHandlerManager.getInstace().sendPacket(peer, lpsPacket, Channel.CHL_S2C);
                    continue;
                }
                else if (kv.Value is Inhibitor || kv.Value is Nexus)
                {
                    var inhib = kv.Value as Unit;

                    var ms = new MinionSpawn2(inhib.getNetId());
                    PacketHandlerManager.getInstace().sendPacket(peer, ms, Channel.CHL_S2C);
                    var sh = new SetHealth(inhib.getNetId());
                    PacketHandlerManager.getInstace().sendPacket(peer, sh, Channel.CHL_S2C);

                    var u = kv.Value as Unit;
                    if (u != null)
                    {
                        PacketNotifier.notifyUpdatedStats(u, false);
                    }

                    continue;
                }
                else if (kv.Value is Minion)
                {
                    var m = kv.Value as Minion;
                    if (m.isVisibleByTeam(game.getPeerInfo(peer).getTeam()))
                    {
                        var ms = new MinionSpawn(m);
                        PacketHandlerManager.getInstace().sendPacket(peer, ms, Channel.CHL_S2C);
                        var sh = new SetHealth(m);
                        PacketHandlerManager.getInstace().sendPacket(peer, sh, Channel.CHL_S2C);

                        var u = kv.Value as Unit;
                        if (u != null)
                        {
                            PacketNotifier.notifyUpdatedStats(u, false);
                        }
                    }
                    continue;
                }
                else if (kv.Value is Champion)
                {
                    var c = kv.Value as Champion;
                    if (c.isVisibleByTeam(game.getPeerInfo(peer).getTeam()))
                    {
                        var cs = new ChampionRespawn(c);
                        PacketHandlerManager.getInstace().sendPacket(peer, cs, Channel.CHL_S2C);
                        var sh = new SetHealth(c);
                        PacketHandlerManager.getInstace().sendPacket(peer, sh, Channel.CHL_S2C);

                        var u = kv.Value as Unit;
                        if (u != null)
                        {
                            PacketNotifier.notifyUpdatedStats(u, false);
                        }
                    }
                    continue;
                }
                else if (kv.Value is Projectile)
                {
                    var p = kv.Value as Projectile;
                    if (p.isVisibleByTeam(game.getPeerInfo(peer).getTeam()))
                    {
                        var sp = new SpawnProjectile(p);
                        PacketHandlerManager.getInstace().sendPacket(peer, sp, Channel.CHL_S2C);
                    }
                    continue;
                }
                else
                {
                    Logger.LogCoreWarning("Object of type: " + kv.Value.GetType() + " not handled in HandleSpawn.");
                }
            }

            // TODO shop map specific?
            // Level props are just models, we need button-object minions to allow the client to interact with it
            if (peerInfo != null && peerInfo.getTeam() == TeamId.TEAM_BLUE)
            {
                // Shop (blue team)
                var ms1 = new MinionSpawn2(0xff10c6db);
                PacketHandlerManager.getInstace().sendPacket(peer, ms1, Channel.CHL_S2C);
                var sh1 = new SetHealth(0xff10c6db);
                PacketHandlerManager.getInstace().sendPacket(peer, sh1, Channel.CHL_S2C);
            }
            else if (peerInfo != null && peerInfo.getTeam() == TeamId.TEAM_PURPLE)
            {
                // Shop (purple team)
                var ms1 = new MinionSpawn2(0xffa6170e);
                PacketHandlerManager.getInstace().sendPacket(peer, ms1, Channel.CHL_S2C);
                var sh1 = new SetHealth(0xffa6170e);
                PacketHandlerManager.getInstace().sendPacket(peer, sh1, Channel.CHL_S2C);
            }

            var end = new StatePacket(PacketCmdS2C.PKT_S2C_EndSpawn);
            return PacketHandlerManager.getInstace().sendPacket(peer, end, Channel.CHL_S2C);
        }
    }
}