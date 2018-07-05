// Decompiled with JetBrains decompiler
// Type: Terraria.Achievements.AchievementManager
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria.Achievements
{
    public class AchievementManager
    {
        private static readonly object _ioLock = new object();
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings();
        private readonly Dictionary<string, int> _achievementIconIndexes = new Dictionary<string, int>();
        private readonly Dictionary<string, Achievement> _achievements = new Dictionary<string, Achievement>();
        private readonly byte[] _cryptoKey;
        private readonly bool _isCloudSave;
        private readonly string _savePath;

        public AchievementManager()
        {
            if (SocialAPI.Achievements != null)
            {
                _savePath = SocialAPI.Achievements.GetSavePath();
                _isCloudSave = true;
                _cryptoKey = SocialAPI.Achievements.GetEncryptionKey();
            }
            else
            {
                _savePath = Main.SavePath + Path.DirectorySeparatorChar + "achievements.dat";
                _isCloudSave = false;
                _cryptoKey = Encoding.ASCII.GetBytes("RELOGIC-TERRARIA");
            }
        }

        public event Achievement.AchievementCompleted OnAchievementCompleted;

        public void Save()
        {
            Save(_savePath, _isCloudSave);
        }

        private void Save(string path, bool cloud)
        {
            lock (_ioLock)
            {
                if (SocialAPI.Achievements != null)
                    SocialAPI.Achievements.StoreStats();
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream,
                            new RijndaelManaged().CreateEncryptor(_cryptoKey, _cryptoKey),
                            CryptoStreamMode.Write))
                        {
                            using (var bsonWriter = new BsonWriter(cryptoStream))
                            {
                                JsonSerializer.Create(_serializerSettings).Serialize(bsonWriter,
                                    _achievements);
                                bsonWriter.Flush();
                                cryptoStream.FlushFinalBlock();
                                FileUtilities.WriteAllBytes(path, memoryStream.ToArray(), cloud);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public List<Achievement> CreateAchievementsList()
        {
            return _achievements.Values.ToList();
        }

        public void Load()
        {
            Load(_savePath, _isCloudSave);
        }

        private void Load(string path, bool cloud)
        {
            var flag = false;
            lock (_ioLock)
            {
                if (!FileUtilities.Exists(path, cloud))
                    return;
                var buffer = FileUtilities.ReadAllBytes(path, cloud);
                var dictionary =
                    (Dictionary<string, StoredAchievement>) null;
                try
                {
                    using (var memoryStream = new MemoryStream(buffer))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream,
                            new RijndaelManaged().CreateDecryptor(_cryptoKey, _cryptoKey),
                            CryptoStreamMode.Read))
                        {
                            using (var bsonReader = new BsonReader(cryptoStream))
                            {
                                dictionary = JsonSerializer
                                    .Create(_serializerSettings)
                                    .Deserialize<Dictionary<string, StoredAchievement>>(
                                        bsonReader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileUtilities.Delete(path, cloud);
                    return;
                }

                if (dictionary == null)
                    return;
                foreach (var keyValuePair in dictionary)
                    if (_achievements.ContainsKey(keyValuePair.Key))
                        _achievements[keyValuePair.Key].Load(keyValuePair.Value.Conditions);

                if (SocialAPI.Achievements != null)
                    foreach (var achievement in _achievements)
                        if (achievement.Value.IsCompleted &&
                            !SocialAPI.Achievements.IsAchievementCompleted(achievement.Key))
                        {
                            flag = true;
                            achievement.Value.ClearProgress();
                        }
            }

            if (!flag)
                return;
            Save();
        }

        private void AchievementCompleted(Achievement achievement)
        {
            Save();
            if (OnAchievementCompleted == null)
                return;
            OnAchievementCompleted(achievement);
        }

        public void Register(Achievement achievement)
        {
            _achievements.Add(achievement.Name, achievement);
            achievement.OnCompleted += AchievementCompleted;
        }

        public void RegisterIconIndex(string achievementName, int iconIndex)
        {
            _achievementIconIndexes.Add(achievementName, iconIndex);
        }

        public void RegisterAchievementCategory(string achievementName, AchievementCategory category)
        {
            _achievements[achievementName].SetCategory(category);
        }

        public Achievement GetAchievement(string achievementName)
        {
            Achievement achievement;
            if (_achievements.TryGetValue(achievementName, out achievement))
                return achievement;
            return null;
        }

        public T GetCondition<T>(string achievementName, string conditionName) where T : AchievementCondition
        {
            return GetCondition(achievementName, conditionName) as T;
        }

        public AchievementCondition GetCondition(string achievementName, string conditionName)
        {
            Achievement achievement;
            if (_achievements.TryGetValue(achievementName, out achievement))
                return achievement.GetCondition(conditionName);
            return null;
        }

        public int GetIconIndex(string achievementName)
        {
            int num;
            if (_achievementIconIndexes.TryGetValue(achievementName, out num))
                return num;
            return 0;
        }

        private class StoredAchievement
        {
            public Dictionary<string, JObject> Conditions;
        }
    }
}