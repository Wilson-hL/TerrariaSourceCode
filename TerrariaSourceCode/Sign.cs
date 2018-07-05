﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Sign
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

namespace Terraria
{
    public class Sign
    {
        public const int maxSigns = 1000;
        public int x;
        public int y;
        public string text;

        public static void KillSign(int x, int y)
        {
            for (var index = 0; index < 1000; ++index)
            {
                if (Main.sign[index] != null && Main.sign[index].x == x && Main.sign[index].y == y)
                    Main.sign[index] = (Sign) null;
            }
        }

        public static int ReadSign(int i, int j, bool CreateIfMissing = true)
        {
            var num1 = (int) Main.tile[i, j].frameX / 18;
            var num2 = (int) Main.tile[i, j].frameY / 18;
            var num3 = num1 % 2;
            var x = i - num3;
            var y = j - num2;
            if (!Main.tileSign[(int) Main.tile[x, y].type])
            {
                Sign.KillSign(x, y);
                return -1;
            }

            var num4 = -1;
            for (var index = 0; index < 1000; ++index)
            {
                if (Main.sign[index] != null && Main.sign[index].x == x && Main.sign[index].y == y)
                {
                    num4 = index;
                    break;
                }
            }

            if (num4 < 0 && CreateIfMissing)
            {
                for (var index = 0; index < 1000; ++index)
                {
                    if (Main.sign[index] == null)
                    {
                        num4 = index;
                        Main.sign[index] = new Sign();
                        Main.sign[index].x = x;
                        Main.sign[index].y = y;
                        Main.sign[index].text = "";
                        break;
                    }
                }
            }

            return num4;
        }

        public static void TextSign(int i, string text)
        {
            if (Main.tile[Main.sign[i].x, Main.sign[i].y] == null ||
                !Main.tile[Main.sign[i].x, Main.sign[i].y].active() ||
                !Main.tileSign[(int) Main.tile[Main.sign[i].x, Main.sign[i].y].type])
                Main.sign[i] = (Sign) null;
            else
                Main.sign[i].text = text;
        }

        public override string ToString()
        {
            return "x" + (object) this.x + "\ty" + (object) this.y + "\t" + this.text;
        }
    }
}