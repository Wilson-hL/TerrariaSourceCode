// Decompiled with JetBrains decompiler
// Type: Terraria.Map.WorldMap
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.IO;
using Terraria.IO;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria.Map
{
    public class WorldMap
    {
        public readonly int MaxWidth;
        public readonly int MaxHeight;
        private MapTile[,] _tiles;

        public MapTile this[int x, int y]
        {
            get { return this._tiles[x, y]; }
        }

        public WorldMap(int maxWidth, int maxHeight)
        {
            this.MaxWidth = maxWidth;
            this.MaxHeight = maxHeight;
            this._tiles = new MapTile[this.MaxWidth, this.MaxHeight];
        }

        public void ConsumeUpdate(int x, int y)
        {
            this._tiles[x, y].IsChanged = false;
        }

        public void Update(int x, int y, byte light)
        {
            this._tiles[x, y] = MapHelper.CreateMapTile(x, y, light);
        }

        public void SetTile(int x, int y, ref MapTile tile)
        {
            this._tiles[x, y] = tile;
        }

        public bool IsRevealed(int x, int y)
        {
            return this._tiles[x, y].Light > (byte) 0;
        }

        public bool UpdateLighting(int x, int y, byte light)
        {
            var tile = this._tiles[x, y];
            var mapTile = MapHelper.CreateMapTile(x, y, Math.Max(tile.Light, light));
            if (mapTile.Equals(ref tile))
                return false;
            this._tiles[x, y] = mapTile;
            return true;
        }

        public bool UpdateType(int x, int y)
        {
            var mapTile = MapHelper.CreateMapTile(x, y, this._tiles[x, y].Light);
            if (mapTile.Equals(ref this._tiles[x, y]))
                return false;
            this._tiles[x, y] = mapTile;
            return true;
        }

        public void UnlockMapSection(int sectionX, int sectionY)
        {
        }

        public void Load()
        {
            var isCloudSave = Main.ActivePlayerFileData.IsCloudSave;
            if (isCloudSave && SocialAPI.Cloud == null || !Main.mapEnabled)
                return;
            var str1 = Main.playerPathName.Substring(0, Main.playerPathName.Length - 4) +
                          (object) Path.DirectorySeparatorChar;
            string str2;
            if (Main.ActiveWorldFileData.UseGuidAsMapName)
            {
                var str3 = str1;
                str2 = str1 + Main.ActiveWorldFileData.UniqueId.ToString() + ".map";
                if (!FileUtilities.Exists(str2, isCloudSave))
                    str2 = str3 + (object) Main.worldID + ".map";
            }
            else
                str2 = str1 + (object) Main.worldID + ".map";

            if (!FileUtilities.Exists(str2, isCloudSave))
            {
                Main.MapFileMetadata = FileMetadata.FromCurrentSettings(FileType.Map);
            }
            else
            {
                using (var memoryStream = new MemoryStream(FileUtilities.ReadAllBytes(str2, isCloudSave)))
                {
                    using (var fileIO = new BinaryReader((Stream) memoryStream))
                    {
                        try
                        {
                            var release = fileIO.ReadInt32();
                            if (release > 193)
                                return;
                            if (release <= 91)
                                MapHelper.LoadMapVersion1(fileIO, release);
                            else
                                MapHelper.LoadMapVersion2(fileIO, release);
                            Main.clearMap = true;
                            Main.loadMap = true;
                            Main.loadMapLock = true;
                            Main.refreshMap = false;
                        }
                        catch (Exception ex)
                        {
                            using (var streamWriter = new StreamWriter("client-crashlog.txt", true))
                            {
                                streamWriter.WriteLine((object) DateTime.Now);
                                streamWriter.WriteLine((object) ex);
                                streamWriter.WriteLine("");
                            }

                            if (!isCloudSave)
                                File.Copy(str2, str2 + ".bad", true);
                            this.Clear();
                        }
                    }
                }
            }
        }

        public void Save()
        {
            MapHelper.SaveMap();
        }

        public void Clear()
        {
            for (var index1 = 0; index1 < this.MaxWidth; ++index1)
            {
                for (var index2 = 0; index2 < this.MaxHeight; ++index2)
                    this._tiles[index1, index2].Clear();
            }
        }
    }
}