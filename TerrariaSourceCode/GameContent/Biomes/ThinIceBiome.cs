// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Biomes.ThinIceBiome
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    public class ThinIceBiome : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            var resultsOutput = new Dictionary<ushort, int>();
            WorldUtils.Gen(new Point(origin.X - 25, origin.Y - 25), new Shapes.Rectangle(50, 50),
                new Actions.TileScanner((ushort) 0, (ushort) 59, (ushort) 147, (ushort) 1).Output(resultsOutput));
            var num1 = resultsOutput[0] + resultsOutput[1];
            var num2 = resultsOutput[59];
            var num3 = resultsOutput[147];
            if (num3 <= num2 || num3 <= num1)
                return false;
            var num4 = 0;
            for (var radius = _random.Next(10, 15); radius > 5; --radius)
            {
                var num5 = _random.Next(-5, 5);
                WorldUtils.Gen(new Point(origin.X + num5, origin.Y + num4), new Shapes.Circle(radius),
                    Actions.Chain((GenAction) new Modifiers.Blotches(4, 0.3),
                        (GenAction) new Modifiers.OnlyTiles((ushort) 147, (ushort) 161, (ushort) 224, (ushort) 0,
                            (ushort) 1), (GenAction) new Actions.SetTile(162, true, true)));
                WorldUtils.Gen(new Point(origin.X + num5, origin.Y + num4), new Shapes.Circle(radius),
                    Actions.Chain((GenAction) new Modifiers.Blotches(4, 0.3),
                        (GenAction) new Modifiers.HasLiquid(-1, -1),
                        (GenAction) new Actions.SetTile(162, true, true),
                        (GenAction) new Actions.SetLiquid(0, 0)));
                num4 += radius - 2;
            }

            return true;
        }
    }
}