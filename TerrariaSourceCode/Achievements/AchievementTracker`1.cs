// Decompiled with JetBrains decompiler
// Type: Terraria.Achievements.AchievementTracker`1
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Terraria.Social;

namespace Terraria.Achievements
{
    public abstract class AchievementTracker<T> : IAchievementTracker
    {
        protected T _maxValue;
        protected string _name;
        private readonly TrackerType _type;
        protected T _value;

        protected AchievementTracker(TrackerType type)
        {
            _type = type;
        }

        public T Value => _value;

        public T MaxValue => _maxValue;

        void IAchievementTracker.ReportAs(string name)
        {
            _name = name;
        }

        TrackerType IAchievementTracker.GetTrackerType()
        {
            return _type;
        }

        void IAchievementTracker.Clear()
        {
            SetValue(default(T), true);
        }

        void IAchievementTracker.Load()
        {
            Load();
        }

        public void SetValue(T newValue, bool reportUpdate = true)
        {
            if (newValue.Equals(_value))
                return;
            _value = newValue;
            if (!reportUpdate)
                return;
            ReportUpdate();
            if (!_value.Equals(_maxValue))
                return;
            OnComplete();
        }

        public abstract void ReportUpdate();

        protected abstract void Load();

        protected void OnComplete()
        {
            if (SocialAPI.Achievements == null)
                return;
            SocialAPI.Achievements.StoreStats();
        }
    }
}