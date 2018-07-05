// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Achievements.ItemCraftCondition
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
    public class ItemCraftCondition : AchievementCondition
    {
        private const string Identifier = "ITEM_PICKUP";

        private static readonly Dictionary<short, List<ItemCraftCondition>> _listeners =
            new Dictionary<short, List<ItemCraftCondition>>();

        private static bool _isListenerHooked;
        private readonly short[] _itemIds;

        private ItemCraftCondition(short itemId)
            : base("ITEM_PICKUP_" + itemId)
        {
            _itemIds = new short[1] {itemId};
            ListenForCraft(this);
        }

        private ItemCraftCondition(short[] itemIds)
            : base("ITEM_PICKUP_" + itemIds[0])
        {
            _itemIds = itemIds;
            ListenForCraft(this);
        }

        private static void ListenForCraft(ItemCraftCondition condition)
        {
            if (!_isListenerHooked)
            {
                AchievementsHelper.OnItemCraft +=
                    ItemCraftListener;
                _isListenerHooked = true;
            }

            for (var index = 0; index < condition._itemIds.Length; ++index)
            {
                if (!_listeners.ContainsKey(condition._itemIds[index]))
                    _listeners[condition._itemIds[index]] = new List<ItemCraftCondition>();
                _listeners[condition._itemIds[index]].Add(condition);
            }
        }

        private static void ItemCraftListener(short itemId, int count)
        {
            if (!_listeners.ContainsKey(itemId))
                return;
            foreach (AchievementCondition achievementCondition in _listeners[itemId])
                achievementCondition.Complete();
        }

        public static AchievementCondition Create(params short[] items)
        {
            return new ItemCraftCondition(items);
        }

        public static AchievementCondition Create(short item)
        {
            return new ItemCraftCondition(item);
        }

        public static AchievementCondition[] CreateMany(params short[] items)
        {
            var achievementConditionArray = new AchievementCondition[items.Length];
            for (var index = 0; index < items.Length; ++index)
                achievementConditionArray[index] = new ItemCraftCondition(items[index]);
            return achievementConditionArray;
        }
    }
}