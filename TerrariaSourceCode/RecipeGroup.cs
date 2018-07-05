﻿// Decompiled with JetBrains decompiler
// Type: Terraria.RecipeGroup
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.Collections.Generic;

namespace Terraria
{
    public class RecipeGroup
    {
        public static Dictionary<int, RecipeGroup> recipeGroups = new Dictionary<int, RecipeGroup>();
        public static Dictionary<string, int> recipeGroupIDs = new Dictionary<string, int>();
        public static int nextRecipeGroupIndex = 0;
        public Func<string> GetText;
        public List<int> ValidItems;
        public int IconicItemIndex;

        public RecipeGroup(Func<string> getName, params int[] validItems)
        {
            this.GetText = getName;
            this.ValidItems = new List<int>((IEnumerable<int>) validItems);
        }

        public static int RegisterGroup(string name, RecipeGroup rec)
        {
            var key = RecipeGroup.nextRecipeGroupIndex++;
            RecipeGroup.recipeGroups.Add(key, rec);
            RecipeGroup.recipeGroupIDs.Add(name, key);
            return key;
        }
    }
}