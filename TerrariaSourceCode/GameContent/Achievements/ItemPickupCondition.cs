// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Achievements.ItemPickupCondition
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
    public class ItemPickupCondition : AchievementCondition
    {
        private const string Identifier = "ITEM_PICKUP";

        private static readonly Dictionary<short, List<ItemPickupCondition>> _listeners =
            new Dictionary<short, List<ItemPickupCondition>>();

        private static bool _isListenerHooked;
        private readonly short[] _itemIds;

        private ItemPickupCondition(short itemId)
            : base("ITEM_PICKUP_" + itemId)
        {
            _itemIds = new short[1] {itemId};
            ListenForPickup(this);
        }

        private ItemPickupCondition(short[] itemIds)
            : base("ITEM_PICKUP_" + itemIds[0])
        {
            _itemIds = itemIds;
            ListenForPickup(this);
        }

        private static void ListenForPickup(ItemPickupCondition condition)
        {
            if (!_isListenerHooked)
            {
                AchievementsHelper.OnItemPickup +=
                    ItemPickupListener;
                _isListenerHooked = true;
            }

            for (var index = 0; index < condition._itemIds.Length; ++index)
            {
                if (!_listeners.ContainsKey(condition._itemIds[index]))
                    _listeners[condition._itemIds[index]] = new List<ItemPickupCondition>();
                _listeners[condition._itemIds[index]].Add(condition);
            }
        }

        private static void ItemPickupListener(Player player, short itemId, int count)
        {
            if (player.whoAmI != Main.myPlayer || !_listeners.ContainsKey(itemId))
                return;
            foreach (AchievementCondition achievementCondition in _listeners[itemId])
                achievementCondition.Complete();
        }

        public static AchievementCondition Create(params short[] items)
        {
            return new ItemPickupCondition(items);
        }

        public static AchievementCondition Create(short item)
        {
            return new ItemPickupCondition(item);
        }

        public static AchievementCondition[] CreateMany(params short[] items)
        {
            var achievementConditionArray = new AchievementCondition[items.Length];
            for (var index = 0; index < items.Length; ++index)
                achievementConditionArray[index] = new ItemPickupCondition(items[index]);
            return achievementConditionArray;
        }
    }
}