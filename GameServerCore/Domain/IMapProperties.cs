using System;
using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;

namespace GameServerCore.Domain
{
    public interface IMapProperties: IUpdate
    {
        float GoldPerSecond { get; set; }
        float StartingGold { get; set; }
        bool HasFirstBloodHappened { get; set; }
        bool IsKillGoldRewardReductionActive { get; set; }
        int BluePillId { get; set; }
        long FirstGoldTime { get; set; }
        bool SpawnEnabled { get; set; }

        void Init();

        Tuple<TeamId, Vector2> GetMinionSpawnPosition(string barracksName);

        void SetMinionStats(ILaneMinion m);

        string GetMinionModel(TeamId team, MinionSpawnType type);

        float GetGoldFor(IAttackableUnit u);

        float GetExperienceFor(IAttackableUnit u);

        void HandleSurrender(int userId, IChampion who, bool vote);
    }
}