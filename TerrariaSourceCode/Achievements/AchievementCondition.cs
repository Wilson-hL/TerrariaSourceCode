// Decompiled with JetBrains decompiler
// Type: Terraria.Achievements.AchievementCondition
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Terraria.Achievements
{
    [JsonObject]
    public abstract class AchievementCondition
    {
        public delegate void AchievementUpdate(AchievementCondition condition);

        public readonly string Name;
        [JsonProperty("Completed")] private bool _isCompleted;
        protected IAchievementTracker _tracker;

        protected AchievementCondition(string name)
        {
            Name = name;
        }

        public bool IsCompleted => _isCompleted;

        public event AchievementUpdate OnComplete;

        public virtual void Load(JObject state)
        {
            //Fix By GScience
            _isCompleted = (bool) state["Completed"];
        }

        public virtual void Clear()
        {
            _isCompleted = false;
        }

        public virtual void Complete()
        {
            if (_isCompleted)
                return;
            _isCompleted = true;
            if (OnComplete == null)
                return;
            OnComplete(this);
        }

        protected virtual IAchievementTracker CreateAchievementTracker()
        {
            return null;
        }

        public IAchievementTracker GetAchievementTracker()
        {
            if (_tracker == null)
                _tracker = CreateAchievementTracker();
            return _tracker;
        }
    }
}