// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Achievements.NPCKilledCondition
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
    public class NPCKilledCondition : AchievementCondition
    {
        private const string Identifier = "NPC_KILLED";

        private static readonly Dictionary<short, List<NPCKilledCondition>> _listeners =
            new Dictionary<short, List<NPCKilledCondition>>();

        private static bool _isListenerHooked;
        private readonly short[] _npcIds;

        private NPCKilledCondition(short npcId)
            : base("NPC_KILLED_" + npcId)
        {
            _npcIds = new short[1] {npcId};
            ListenForPickup(this);
        }

        private NPCKilledCondition(short[] npcIds)
            : base("NPC_KILLED_" + npcIds[0])
        {
            _npcIds = npcIds;
            ListenForPickup(this);
        }

        private static void ListenForPickup(NPCKilledCondition condition)
        {
            if (!_isListenerHooked)
            {
                AchievementsHelper.OnNPCKilled +=
                    NPCKilledListener;
                _isListenerHooked = true;
            }

            for (var index = 0; index < condition._npcIds.Length; ++index)
            {
                if (!_listeners.ContainsKey(condition._npcIds[index]))
                    _listeners[condition._npcIds[index]] = new List<NPCKilledCondition>();
                _listeners[condition._npcIds[index]].Add(condition);
            }
        }

        private static void NPCKilledListener(Player player, short npcId)
        {
            if (player.whoAmI != Main.myPlayer || !_listeners.ContainsKey(npcId))
                return;
            foreach (AchievementCondition achievementCondition in _listeners[npcId])
                achievementCondition.Complete();
        }

        public static AchievementCondition Create(params short[] npcIds)
        {
            return new NPCKilledCondition(npcIds);
        }

        public static AchievementCondition Create(short npcId)
        {
            return new NPCKilledCondition(npcId);
        }

        public static AchievementCondition[] CreateMany(params short[] npcs)
        {
            var achievementConditionArray = new AchievementCondition[npcs.Length];
            for (var index = 0; index < npcs.Length; ++index)
                achievementConditionArray[index] = new NPCKilledCondition(npcs[index]);
            return achievementConditionArray;
        }
    }
}