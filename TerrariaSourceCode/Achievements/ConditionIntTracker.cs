// Decompiled with JetBrains decompiler
// Type: Terraria.Achievements.ConditionIntTracker
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Terraria.Social;

namespace Terraria.Achievements
{
    public class ConditionIntTracker : AchievementTracker<int>
    {
        public ConditionIntTracker()
            : base(TrackerType.Int)
        {
        }

        public ConditionIntTracker(int maxValue)
            : base(TrackerType.Int)
        {
            _maxValue = maxValue;
        }

        public override void ReportUpdate()
        {
            if (SocialAPI.Achievements == null || _name == null)
                return;
            SocialAPI.Achievements.UpdateIntStat(_name, _value);
        }

        protected override void Load()
        {
        }
    }
}