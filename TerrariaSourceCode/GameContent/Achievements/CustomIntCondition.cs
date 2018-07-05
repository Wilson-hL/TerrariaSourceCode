// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Achievements.CustomIntCondition
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
    public class CustomIntCondition : AchievementCondition
    {
        private readonly int _maxValue;
        [JsonProperty("Value")] private int _value;

        private CustomIntCondition(string name, int maxValue)
            : base(name)
        {
            _maxValue = maxValue;
            _value = 0;
        }

        public int Value
        {
            get => _value;
            set
            {
                var newValue = Utils.Clamp(value, 0, _maxValue);
                if (_tracker != null)
                    ((AchievementTracker<int>) _tracker).SetValue(newValue, true);
                _value = newValue;
                if (_value != _maxValue)
                    return;
                Complete();
            }
        }

        public override void Clear()
        {
            _value = 0;
            base.Clear();
        }

        public override void Load(JObject state)
        {
            base.Load(state);
            //Fix By GScience
            _value = (int) state["Value"];
            if (_tracker == null)
                return;
            ((AchievementTracker<int>) _tracker).SetValue(_value, false);
        }

        protected override IAchievementTracker CreateAchievementTracker()
        {
            return new ConditionIntTracker(_maxValue);
        }

        public static AchievementCondition Create(string name, int maxValue)
        {
            return new CustomIntCondition(name, maxValue);
        }
    }
}