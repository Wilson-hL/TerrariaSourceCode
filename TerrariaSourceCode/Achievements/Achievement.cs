// Decompiled with JetBrains decompiler
// Type: Terraria.Achievements.Achievement
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.Localization;
using Terraria.Social;

namespace Terraria.Achievements
{
    [JsonObject]
    public class Achievement
    {
        public delegate void AchievementCompleted(Achievement achievement);

        private static int _totalAchievements;
        public readonly LocalizedText Description;
        public readonly LocalizedText FriendlyName;
        public readonly int Id = _totalAchievements++;
        public readonly string Name;
        private int _completedCount;

        [JsonProperty("Conditions")]
        private Dictionary<string, AchievementCondition> _conditions = new Dictionary<string, AchievementCondition>();

        private IAchievementTracker _tracker;

        public Achievement(string name)
        {
            Name = name;
            FriendlyName = Language.GetText("Achievements." + name + "_Name");
            Description = Language.GetText("Achievements." + name + "_Description");
        }

        public AchievementCategory Category { get; private set; }

        public bool HasTracker => _tracker != null;

        public bool IsCompleted => _completedCount == _conditions.Count;

        public event AchievementCompleted OnCompleted;

        public IAchievementTracker GetTracker()
        {
            return _tracker;
        }

        public void ClearProgress()
        {
            _completedCount = 0;
            foreach (var condition in _conditions)
                condition.Value.Clear();
            if (_tracker == null)
                return;
            _tracker.Clear();
        }

        public void Load(Dictionary<string, JObject> conditions)
        {
            using (var enumerator = conditions.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    AchievementCondition achievementCondition;
                    if (_conditions.TryGetValue(current.Key, out achievementCondition))
                    {
                        achievementCondition.Load(current.Value);
                        if (achievementCondition.IsCompleted)
                            ++_completedCount;
                    }
                }
            }

            if (_tracker == null)
                return;
            _tracker.Load();
        }

        public void AddCondition(AchievementCondition condition)
        {
            _conditions[condition.Name] = condition;
            condition.OnComplete += OnConditionComplete;
        }

        private void OnConditionComplete(AchievementCondition condition)
        {
            ++_completedCount;
            if (_completedCount != _conditions.Count)
                return;
            if (_tracker == null && SocialAPI.Achievements != null)
                SocialAPI.Achievements.CompleteAchievement(Name);
            if (OnCompleted == null)
                return;
            OnCompleted(this);
        }

        private void UseTracker(IAchievementTracker tracker)
        {
            tracker.ReportAs("STAT_" + Name);
            _tracker = tracker;
        }

        public void UseTrackerFromCondition(string conditionName)
        {
            UseTracker(GetConditionTracker(conditionName));
        }

        public void UseConditionsCompletedTracker()
        {
            var completedTracker = new ConditionsCompletedTracker();
            foreach (var condition in _conditions)
                completedTracker.AddCondition(condition.Value);
            UseTracker(completedTracker);
        }

        public void UseConditionsCompletedTracker(params string[] conditions)
        {
            var completedTracker = new ConditionsCompletedTracker();
            for (var index = 0; index < conditions.Length; ++index)
            {
                var condition = conditions[index];
                completedTracker.AddCondition(_conditions[condition]);
            }

            UseTracker(completedTracker);
        }

        public void ClearTracker()
        {
            _tracker = null;
        }

        private IAchievementTracker GetConditionTracker(string name)
        {
            return _conditions[name].GetAchievementTracker();
        }

        public void AddConditions(params AchievementCondition[] conditions)
        {
            for (var index = 0; index < conditions.Length; ++index)
                AddCondition(conditions[index]);
        }

        public AchievementCondition GetCondition(string conditionName)
        {
            AchievementCondition achievementCondition;
            if (_conditions.TryGetValue(conditionName, out achievementCondition))
                return achievementCondition;
            return null;
        }

        public void SetCategory(AchievementCategory category)
        {
            Category = category;
        }
    }
}