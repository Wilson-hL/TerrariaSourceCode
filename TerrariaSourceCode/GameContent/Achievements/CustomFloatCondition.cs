// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Achievements.CustomFloatCondition
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
    public class CustomFloatCondition : AchievementCondition
    {
        private readonly float _maxValue;
        [JsonProperty("Value")] private float _value;

        private CustomFloatCondition(string name, float maxValue)
            : base(name)
        {
            _maxValue = maxValue;
            _value = 0.0f;
        }

        public float Value
        {
            get => _value;
            set
            {
                var newValue = Utils.Clamp(value, 0.0f, _maxValue);
                if (_tracker != null)
                    ((AchievementTracker<float>) _tracker).SetValue(newValue, true);
                _value = newValue;
                if (_value != (double) _maxValue)
                    return;
                Complete();
            }
        }

        public override void Clear()
        {
            _value = 0.0f;
            base.Clear();
        }

        public override void Load(JObject state)
        {
            base.Load(state);
            //Fix By GScience
            _value = (float) state["Value"];
            if (_tracker == null)
                return;
            ((AchievementTracker<float>) _tracker).SetValue(_value, false);
        }

        protected override IAchievementTracker CreateAchievementTracker()
        {
            return new ConditionFloatTracker(_maxValue);
        }

        public static AchievementCondition Create(string name, float maxValue)
        {
            return new CustomFloatCondition(name, maxValue);
        }
    }
}