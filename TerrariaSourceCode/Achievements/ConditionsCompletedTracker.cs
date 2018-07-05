// Decompiled with JetBrains decompiler
// Type: Terraria.Achievements.ConditionsCompletedTracker
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.Collections.Generic;

namespace Terraria.Achievements
{
    public class ConditionsCompletedTracker : ConditionIntTracker
    {
        private readonly List<AchievementCondition> _conditions = new List<AchievementCondition>();

        public void AddCondition(AchievementCondition condition)
        {
            ++_maxValue;
            condition.OnComplete += OnConditionCompleted;
            _conditions.Add(condition);
        }

        private void OnConditionCompleted(AchievementCondition condition)
        {
            SetValue(Math.Min(_value + 1, _maxValue), true);
        }

        protected override void Load()
        {
            for (var index = 0; index < _conditions.Count; ++index)
                if (_conditions[index].IsCompleted)
                    ++_value;
        }
    }
}