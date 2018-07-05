﻿// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.PlacementHook
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;

namespace Terraria.DataStructures
{
    public struct PlacementHook
    {
        public static PlacementHook Empty = new PlacementHook(null, 0, 0, false);
        public const int Response_AllInvalid = 0;
        public Func<int, int, int, int, int, int> hook;
        public int badReturn;
        public int badResponse;
        public bool processedCoordinates;

        public PlacementHook(Func<int, int, int, int, int, int> hook, int badReturn, int badResponse,
            bool processedCoordinates)
        {
            this.hook = hook;
            this.badResponse = badResponse;
            this.badReturn = badReturn;
            this.processedCoordinates = processedCoordinates;
        }

        public static bool operator ==(PlacementHook first, PlacementHook second)
        {
            if (first.hook == second.hook && first.badResponse == second.badResponse &&
                first.badReturn == second.badReturn)
                return first.processedCoordinates == second.processedCoordinates;
            return false;
        }

        public static bool operator !=(PlacementHook first, PlacementHook second)
        {
            if (!(first.hook != second.hook) && first.badResponse == second.badResponse &&
                first.badReturn == second.badReturn)
                return first.processedCoordinates != second.processedCoordinates;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is PlacementHook)
                return this == (PlacementHook) obj;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}