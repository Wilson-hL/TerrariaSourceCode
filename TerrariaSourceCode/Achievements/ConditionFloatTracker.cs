// Decompiled with JetBrains decompiler
// Type: Terraria.Achievements.ConditionFloatTracker
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Terraria.Social;

namespace Terraria.Achievements
{
    public class ConditionFloatTracker : AchievementTracker<float>
    {
        public ConditionFloatTracker(float maxValue)
            : base(TrackerType.Float)
        {
            _maxValue = maxValue;
        }

        public ConditionFloatTracker()
            : base(TrackerType.Float)
        {
        }

        public override void ReportUpdate()
        {
            if (SocialAPI.Achievements == null || _name == null)
                return;
            SocialAPI.Achievements.UpdateFloatStat(_name, _value);
        }

        protected override void Load()
        {
        }
    }
}