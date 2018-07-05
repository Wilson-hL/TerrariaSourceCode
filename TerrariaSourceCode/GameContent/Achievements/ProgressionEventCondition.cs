// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Achievements.ProgressionEventCondition
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
    public class ProgressionEventCondition : AchievementCondition
    {
        private const string Identifier = "PROGRESSION_EVENT";

        private static readonly Dictionary<int, List<ProgressionEventCondition>> _listeners =
            new Dictionary<int, List<ProgressionEventCondition>>();

        private static bool _isListenerHooked;
        private readonly int[] _eventIDs;

        private ProgressionEventCondition(int eventID)
            : base("PROGRESSION_EVENT_" + eventID)
        {
            _eventIDs = new int[1] {eventID};
            ListenForPickup(this);
        }

        private ProgressionEventCondition(int[] eventIDs)
            : base("PROGRESSION_EVENT_" + eventIDs[0])
        {
            _eventIDs = eventIDs;
            ListenForPickup(this);
        }

        private static void ListenForPickup(ProgressionEventCondition condition)
        {
            if (!_isListenerHooked)
            {
                AchievementsHelper.OnProgressionEvent +=
                    ProgressionEventListener;
                _isListenerHooked = true;
            }

            for (var index = 0; index < condition._eventIDs.Length; ++index)
            {
                if (!_listeners.ContainsKey(condition._eventIDs[index]))
                    _listeners[condition._eventIDs[index]] =
                        new List<ProgressionEventCondition>();
                _listeners[condition._eventIDs[index]].Add(condition);
            }
        }

        private static void ProgressionEventListener(int eventID)
        {
            if (!_listeners.ContainsKey(eventID))
                return;
            foreach (AchievementCondition achievementCondition in _listeners[eventID])
                achievementCondition.Complete();
        }

        public static ProgressionEventCondition Create(params int[] eventIDs)
        {
            return new ProgressionEventCondition(eventIDs);
        }

        public static ProgressionEventCondition Create(int eventID)
        {
            return new ProgressionEventCondition(eventID);
        }

        public static ProgressionEventCondition[] CreateMany(params int[] eventIDs)
        {
            var progressionEventConditionArray = new ProgressionEventCondition[eventIDs.Length];
            for (var index = 0; index < eventIDs.Length; ++index)
                progressionEventConditionArray[index] = new ProgressionEventCondition(eventIDs[index]);
            return progressionEventConditionArray;
        }
    }
}