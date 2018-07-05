﻿// Decompiled with JetBrains decompiler
// Type: Terraria.IO.FavoritesFile
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using Terraria.Utilities;

namespace Terraria.IO
{
    public class FavoritesFile
    {
        private Dictionary<string, Dictionary<string, bool>> _data = new Dictionary<string, Dictionary<string, bool>>();
        public readonly string Path;
        public readonly bool IsCloudSave;

        public FavoritesFile(string path, bool isCloud)
        {
            this.Path = path;
            this.IsCloudSave = isCloud;
        }

        public void SaveFavorite(FileData fileData)
        {
            if (!this._data.ContainsKey(fileData.Type))
                this._data.Add(fileData.Type, new Dictionary<string, bool>());
            this._data[fileData.Type][fileData.GetFileName(true)] = fileData.IsFavorite;
            this.Save();
        }

        public void ClearEntry(FileData fileData)
        {
            if (!this._data.ContainsKey(fileData.Type))
                return;
            this._data[fileData.Type].Remove(fileData.GetFileName(true));
            this.Save();
        }

        public bool IsFavorite(FileData fileData)
        {
            if (!this._data.ContainsKey(fileData.Type))
                return false;
            var fileName = fileData.GetFileName(true);
            bool flag;
            if (this._data[fileData.Type].TryGetValue(fileName, out flag))
                return flag;
            return false;
        }

        public void Save()
        {
            FileUtilities.WriteAllBytes(this.Path,
                Encoding.ASCII.GetBytes(JsonConvert.SerializeObject((object) this._data, (Formatting) 1)),
                this.IsCloudSave);
        }

        public void Load()
        {
            if (!FileUtilities.Exists(this.Path, this.IsCloudSave))
            {
                this._data.Clear();
            }
            else
            {
                this._data =
                    (Dictionary<string, Dictionary<string, bool>>) JsonConvert
                        .DeserializeObject<Dictionary<string, Dictionary<string, bool>>>(
                            Encoding.ASCII.GetString(FileUtilities.ReadAllBytes(this.Path, this.IsCloudSave)));
                if (this._data != null)
                    return;
                this._data = new Dictionary<string, Dictionary<string, bool>>();
            }
        }
    }
}