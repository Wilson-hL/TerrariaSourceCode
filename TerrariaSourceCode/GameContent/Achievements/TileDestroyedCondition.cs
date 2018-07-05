// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Achievements.TileDestroyedCondition
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
    public class TileDestroyedCondition : AchievementCondition
    {
        private const string Identifier = "TILE_DESTROYED";

        private static readonly Dictionary<ushort, List<TileDestroyedCondition>> _listeners =
            new Dictionary<ushort, List<TileDestroyedCondition>>();

        private static bool _isListenerHooked;
        private readonly ushort[] _tileIds;

        private TileDestroyedCondition(ushort[] tileIds)
            : base("TILE_DESTROYED_" + tileIds[0])
        {
            _tileIds = tileIds;
            ListenForDestruction(this);
        }

        private static void ListenForDestruction(TileDestroyedCondition condition)
        {
            if (!_isListenerHooked)
            {
                AchievementsHelper.OnTileDestroyed +=
                    TileDestroyedListener;
                _isListenerHooked = true;
            }

            for (var index = 0; index < condition._tileIds.Length; ++index)
            {
                if (!_listeners.ContainsKey(condition._tileIds[index]))
                    _listeners[condition._tileIds[index]] = new List<TileDestroyedCondition>();
                _listeners[condition._tileIds[index]].Add(condition);
            }
        }

        private static void TileDestroyedListener(Player player, ushort tileId)
        {
            if (player.whoAmI != Main.myPlayer || !_listeners.ContainsKey(tileId))
                return;
            foreach (AchievementCondition achievementCondition in _listeners[tileId])
                achievementCondition.Complete();
        }

        public static AchievementCondition Create(params ushort[] tileIds)
        {
            return new TileDestroyedCondition(tileIds);
        }
    }
}