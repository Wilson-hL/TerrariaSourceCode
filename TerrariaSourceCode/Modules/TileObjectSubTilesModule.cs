﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Modules.TileObjectSubTilesModule
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.ObjectData;

namespace Terraria.Modules
{
    public class TileObjectSubTilesModule
    {
        public List<TileObjectData> data;

        public TileObjectSubTilesModule(TileObjectSubTilesModule copyFrom = null, List<TileObjectData> newData = null)
        {
            if (copyFrom == null)
                this.data = (List<TileObjectData>) null;
            else if (copyFrom.data == null)
            {
                this.data = (List<TileObjectData>) null;
            }
            else
            {
                this.data = new List<TileObjectData>(copyFrom.data.Count);
                for (var index = 0; index < this.data.Count; ++index)
                    this.data.Add(new TileObjectData(copyFrom.data[index]));
            }
        }
    }
}