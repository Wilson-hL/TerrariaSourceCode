// Decompiled with JetBrains decompiler
// Type: Terraria.Chest
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Terraria
{
    public class Chest
    {
        public static int[] chestTypeToIcon = new int[52];
        public static int[] chestItemSpawn = new int[52];
        public static int[] chestTypeToIcon2 = new int[2];
        public static int[] chestItemSpawn2 = new int[2];
        public static int[] dresserTypeToIcon = new int[32];
        public static int[] dresserItemSpawn = new int[32];
        public const int maxChestTypes = 52;
        public const int maxChestTypes2 = 2;
        public const int maxDresserTypes = 32;
        public const int maxItems = 40;
        public const int MaxNameLength = 20;
        public Item[] item;
        public int x;
        public int y;
        public bool bankChest;
        public string name;
        public int frameCounter;
        public int frame;

        public Chest(bool bank = false)
        {
            this.item = new Item[40];
            this.bankChest = bank;
            this.name = string.Empty;
        }

        public override string ToString()
        {
            var num = 0;
            for (var index = 0; index < this.item.Length; ++index)
            {
                if (this.item[index].stack > 0)
                    ++num;
            }

            return string.Format("{{X: {0}, Y: {1}, Count: {2}}}", (object) this.x, (object) this.y, (object) num);
        }

        public static void Initialize()
        {
            var chestItemSpawn = Chest.chestItemSpawn;
            var chestTypeToIcon = Chest.chestTypeToIcon;
            chestTypeToIcon[0] = chestItemSpawn[0] = 48;
            chestTypeToIcon[1] = chestItemSpawn[1] = 306;
            chestTypeToIcon[2] = 327;
            chestItemSpawn[2] = 306;
            chestTypeToIcon[3] = chestItemSpawn[3] = 328;
            chestTypeToIcon[4] = 329;
            chestItemSpawn[4] = 328;
            chestTypeToIcon[5] = chestItemSpawn[5] = 343;
            chestTypeToIcon[6] = chestItemSpawn[6] = 348;
            chestTypeToIcon[7] = chestItemSpawn[7] = 625;
            chestTypeToIcon[8] = chestItemSpawn[8] = 626;
            chestTypeToIcon[9] = chestItemSpawn[9] = 627;
            chestTypeToIcon[10] = chestItemSpawn[10] = 680;
            chestTypeToIcon[11] = chestItemSpawn[11] = 681;
            chestTypeToIcon[12] = chestItemSpawn[12] = 831;
            chestTypeToIcon[13] = chestItemSpawn[13] = 838;
            chestTypeToIcon[14] = chestItemSpawn[14] = 914;
            chestTypeToIcon[15] = chestItemSpawn[15] = 952;
            chestTypeToIcon[16] = chestItemSpawn[16] = 1142;
            chestTypeToIcon[17] = chestItemSpawn[17] = 1298;
            chestTypeToIcon[18] = chestItemSpawn[18] = 1528;
            chestTypeToIcon[19] = chestItemSpawn[19] = 1529;
            chestTypeToIcon[20] = chestItemSpawn[20] = 1530;
            chestTypeToIcon[21] = chestItemSpawn[21] = 1531;
            chestTypeToIcon[22] = chestItemSpawn[22] = 1532;
            chestTypeToIcon[23] = 1533;
            chestItemSpawn[23] = 1528;
            chestTypeToIcon[24] = 1534;
            chestItemSpawn[24] = 1529;
            chestTypeToIcon[25] = 1535;
            chestItemSpawn[25] = 1530;
            chestTypeToIcon[26] = 1536;
            chestItemSpawn[26] = 1531;
            chestTypeToIcon[27] = 1537;
            chestItemSpawn[27] = 1532;
            chestTypeToIcon[28] = chestItemSpawn[28] = 2230;
            chestTypeToIcon[29] = chestItemSpawn[29] = 2249;
            chestTypeToIcon[30] = chestItemSpawn[30] = 2250;
            chestTypeToIcon[31] = chestItemSpawn[31] = 2526;
            chestTypeToIcon[32] = chestItemSpawn[32] = 2544;
            chestTypeToIcon[33] = chestItemSpawn[33] = 2559;
            chestTypeToIcon[34] = chestItemSpawn[34] = 2574;
            chestTypeToIcon[35] = chestItemSpawn[35] = 2612;
            chestTypeToIcon[36] = 327;
            chestItemSpawn[36] = 2612;
            chestTypeToIcon[37] = chestItemSpawn[37] = 2613;
            chestTypeToIcon[38] = 327;
            chestItemSpawn[38] = 2613;
            chestTypeToIcon[39] = chestItemSpawn[39] = 2614;
            chestTypeToIcon[40] = 327;
            chestItemSpawn[40] = 2614;
            chestTypeToIcon[41] = chestItemSpawn[41] = 2615;
            chestTypeToIcon[42] = chestItemSpawn[42] = 2616;
            chestTypeToIcon[43] = chestItemSpawn[43] = 2617;
            chestTypeToIcon[44] = chestItemSpawn[44] = 2618;
            chestTypeToIcon[45] = chestItemSpawn[45] = 2619;
            chestTypeToIcon[46] = chestItemSpawn[46] = 2620;
            chestTypeToIcon[47] = chestItemSpawn[47] = 2748;
            chestTypeToIcon[48] = chestItemSpawn[48] = 2814;
            chestTypeToIcon[49] = chestItemSpawn[49] = 3180;
            chestTypeToIcon[50] = chestItemSpawn[50] = 3125;
            chestTypeToIcon[51] = chestItemSpawn[51] = 3181;
            var chestItemSpawn2 = Chest.chestItemSpawn2;
            var chestTypeToIcon2 = Chest.chestTypeToIcon2;
            chestTypeToIcon2[0] = chestItemSpawn2[0] = 3884;
            chestTypeToIcon2[1] = chestItemSpawn2[1] = 3885;
            Chest.dresserTypeToIcon[0] = Chest.dresserItemSpawn[0] = 334;
            Chest.dresserTypeToIcon[1] = Chest.dresserItemSpawn[1] = 647;
            Chest.dresserTypeToIcon[2] = Chest.dresserItemSpawn[2] = 648;
            Chest.dresserTypeToIcon[3] = Chest.dresserItemSpawn[3] = 649;
            Chest.dresserTypeToIcon[4] = Chest.dresserItemSpawn[4] = 918;
            Chest.dresserTypeToIcon[5] = Chest.dresserItemSpawn[5] = 2386;
            Chest.dresserTypeToIcon[6] = Chest.dresserItemSpawn[6] = 2387;
            Chest.dresserTypeToIcon[7] = Chest.dresserItemSpawn[7] = 2388;
            Chest.dresserTypeToIcon[8] = Chest.dresserItemSpawn[8] = 2389;
            Chest.dresserTypeToIcon[9] = Chest.dresserItemSpawn[9] = 2390;
            Chest.dresserTypeToIcon[10] = Chest.dresserItemSpawn[10] = 2391;
            Chest.dresserTypeToIcon[11] = Chest.dresserItemSpawn[11] = 2392;
            Chest.dresserTypeToIcon[12] = Chest.dresserItemSpawn[12] = 2393;
            Chest.dresserTypeToIcon[13] = Chest.dresserItemSpawn[13] = 2394;
            Chest.dresserTypeToIcon[14] = Chest.dresserItemSpawn[14] = 2395;
            Chest.dresserTypeToIcon[15] = Chest.dresserItemSpawn[15] = 2396;
            Chest.dresserTypeToIcon[16] = Chest.dresserItemSpawn[16] = 2529;
            Chest.dresserTypeToIcon[17] = Chest.dresserItemSpawn[17] = 2545;
            Chest.dresserTypeToIcon[18] = Chest.dresserItemSpawn[18] = 2562;
            Chest.dresserTypeToIcon[19] = Chest.dresserItemSpawn[19] = 2577;
            Chest.dresserTypeToIcon[20] = Chest.dresserItemSpawn[20] = 2637;
            Chest.dresserTypeToIcon[21] = Chest.dresserItemSpawn[21] = 2638;
            Chest.dresserTypeToIcon[22] = Chest.dresserItemSpawn[22] = 2639;
            Chest.dresserTypeToIcon[23] = Chest.dresserItemSpawn[23] = 2640;
            Chest.dresserTypeToIcon[24] = Chest.dresserItemSpawn[24] = 2816;
            Chest.dresserTypeToIcon[25] = Chest.dresserItemSpawn[25] = 3132;
            Chest.dresserTypeToIcon[26] = Chest.dresserItemSpawn[26] = 3134;
            Chest.dresserTypeToIcon[27] = Chest.dresserItemSpawn[27] = 3133;
            Chest.dresserTypeToIcon[28] = Chest.dresserItemSpawn[28] = 3911;
            Chest.dresserTypeToIcon[29] = Chest.dresserItemSpawn[29] = 3912;
            Chest.dresserTypeToIcon[30] = Chest.dresserItemSpawn[30] = 3913;
            Chest.dresserTypeToIcon[31] = Chest.dresserItemSpawn[31] = 3914;
        }

        private static bool IsPlayerInChest(int i)
        {
            for (var index = 0; index < (int) byte.MaxValue; ++index)
            {
                if (Main.player[index].chest == i)
                    return true;
            }

            return false;
        }

        public static bool isLocked(int x, int y)
        {
            return Main.tile[x, y] == null ||
                   Main.tile[x, y].frameX >= (short) 72 && Main.tile[x, y].frameX <= (short) 106 ||
                   (Main.tile[x, y].frameX >= (short) 144 && Main.tile[x, y].frameX <= (short) 178 ||
                    Main.tile[x, y].frameX >= (short) 828 && Main.tile[x, y].frameX <= (short) 1006) ||
                   (Main.tile[x, y].frameX >= (short) 1296 && Main.tile[x, y].frameX <= (short) 1330 ||
                    Main.tile[x, y].frameX >= (short) 1368 && Main.tile[x, y].frameX <= (short) 1402 ||
                    Main.tile[x, y].frameX >= (short) 1440 && Main.tile[x, y].frameX <= (short) 1474);
        }

        public static void ServerPlaceItem(int plr, int slot)
        {
            Main.player[plr].inventory[slot] =
                Chest.PutItemInNearbyChest(Main.player[plr].inventory[slot], Main.player[plr].Center);
            NetMessage.SendData(5, -1, -1, (NetworkText) null, plr, (float) slot,
                (float) Main.player[plr].inventory[slot].prefix, 0.0f, 0, 0, 0);
        }

        public static Item PutItemInNearbyChest(Item item, Vector2 position)
        {
            if (Main.netMode == 1)
                return item;
            for (var i = 0; i < 1000; ++i)
            {
                var flag1 = false;
                var flag2 = false;
                if (Main.chest[i] != null && !Chest.IsPlayerInChest(i) &&
                    !Chest.isLocked(Main.chest[i].x, Main.chest[i].y) &&
                    (double) (new Vector2((float) (Main.chest[i].x * 16 + 16), (float) (Main.chest[i].y * 16 + 16)) -
                              position).Length() < 200.0)
                {
                    for (var index = 0; index < Main.chest[i].item.Length; ++index)
                    {
                        if (Main.chest[i].item[index].type > 0 && Main.chest[i].item[index].stack > 0)
                        {
                            if (item.IsTheSameAs(Main.chest[i].item[index]))
                            {
                                flag1 = true;
                                var num = Main.chest[i].item[index].maxStack - Main.chest[i].item[index].stack;
                                if (num > 0)
                                {
                                    if (num > item.stack)
                                        num = item.stack;
                                    item.stack -= num;
                                    Main.chest[i].item[index].stack += num;
                                    if (item.stack <= 0)
                                    {
                                        item.SetDefaults(0, false);
                                        return item;
                                    }
                                }
                            }
                        }
                        else
                            flag2 = true;
                    }

                    if (flag1 && flag2 && item.stack > 0)
                    {
                        for (var index = 0; index < Main.chest[i].item.Length; ++index)
                        {
                            if (Main.chest[i].item[index].type == 0 || Main.chest[i].item[index].stack == 0)
                            {
                                Main.chest[i].item[index] = item.Clone();
                                item.SetDefaults(0, false);
                                return item;
                            }
                        }
                    }
                }
            }

            return item;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public static bool Unlock(int X, int Y)
        {
            if (Main.tile[X, Y] == null)
                return false;
            short num;
            int Type;
            switch ((int) Main.tile[X, Y].frameX / 36)
            {
                case 2:
                    num = (short) 36;
                    Type = 11;
                    AchievementsHelper.NotifyProgressionEvent(19);
                    break;
                case 4:
                    num = (short) 36;
                    Type = 11;
                    break;
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                    if (!NPC.downedPlantBoss)
                        return false;
                    num = (short) 180;
                    Type = 11;
                    AchievementsHelper.NotifyProgressionEvent(20);
                    break;
                case 36:
                case 38:
                case 40:
                    num = (short) 36;
                    Type = 11;
                    break;
                default:
                    return false;
            }

            Main.PlaySound(22, X * 16, Y * 16, 1, 1f, 0.0f);
            for (var index1 = X; index1 <= X + 1; ++index1)
            {
                for (var index2 = Y; index2 <= Y + 1; ++index2)
                {
                    Main.tile[index1, index2].frameX -= num;
                    for (var index3 = 0; index3 < 4; ++index3)
                        Dust.NewDust(new Vector2((float) (index1 * 16), (float) (index2 * 16)), 16, 16, Type, 0.0f,
                            0.0f, 0, new Color(), 1f);
                }
            }

            return true;
        }

        public static int UsingChest(int i)
        {
            if (Main.chest[i] != null)
            {
                for (var index = 0; index < (int) byte.MaxValue; ++index)
                {
                    if (Main.player[index].active && Main.player[index].chest == i)
                        return index;
                }
            }

            return -1;
        }

        public static int FindChest(int X, int Y)
        {
            for (var index = 0; index < 1000; ++index)
            {
                if (Main.chest[index] != null && Main.chest[index].x == X && Main.chest[index].y == Y)
                    return index;
            }

            return -1;
        }

        public static int FindChestByGuessing(int X, int Y)
        {
            for (var index = 0; index < 1000; ++index)
            {
                if (Main.chest[index] != null && Main.chest[index].x >= X &&
                    (Main.chest[index].x < X + 2 && Main.chest[index].y >= Y) && Main.chest[index].y < Y + 2)
                    return index;
            }

            return -1;
        }

        public static int FindEmptyChest(int x, int y, int type = 21, int style = 0, int direction = 1)
        {
            var num = -1;
            for (var index = 0; index < 1000; ++index)
            {
                var chest = Main.chest[index];
                if (chest != null)
                {
                    if (chest.x == x && chest.y == y)
                        return -1;
                }
                else if (num == -1)
                    num = index;
            }

            return num;
        }

        public static bool NearOtherChests(int x, int y)
        {
            for (var i = x - 25; i < x + 25; ++i)
            {
                for (var j = y - 8; j < y + 8; ++j)
                {
                    var tileSafely = Framing.GetTileSafely(i, j);
                    if (tileSafely.active() && TileID.Sets.BasicChest[(int) tileSafely.type])
                        return true;
                }
            }

            return false;
        }

        public static int AfterPlacement_Hook(int x, int y, int type = 21, int style = 0, int direction = 1)
        {
            var baseCoords = new Point16(x, y);
            TileObjectData.OriginToTopLeft(type, style, ref baseCoords);
            var emptyChest = Chest.FindEmptyChest((int) baseCoords.X, (int) baseCoords.Y, 21, 0, 1);
            if (emptyChest == -1)
                return -1;
            if (Main.netMode != 1)
            {
                var chest = new Chest(false);
                chest.x = (int) baseCoords.X;
                chest.y = (int) baseCoords.Y;
                for (var index = 0; index < 40; ++index)
                    chest.item[index] = new Item();
                Main.chest[emptyChest] = chest;
            }
            else
            {
                switch (type)
                {
                    case 21:
                        NetMessage.SendData(34, -1, -1, (NetworkText) null, 0, (float) x, (float) y, (float) style, 0,
                            0, 0);
                        break;
                    case 467:
                        NetMessage.SendData(34, -1, -1, (NetworkText) null, 4, (float) x, (float) y, (float) style, 0,
                            0, 0);
                        break;
                    default:
                        NetMessage.SendData(34, -1, -1, (NetworkText) null, 2, (float) x, (float) y, (float) style, 0,
                            0, 0);
                        break;
                }
            }

            return emptyChest;
        }

        public static int CreateChest(int X, int Y, int id = -1)
        {
            var index1 = id;
            if (index1 == -1)
            {
                index1 = Chest.FindEmptyChest(X, Y, 21, 0, 1);
                if (index1 == -1)
                    return -1;
                if (Main.netMode == 1)
                    return index1;
            }

            Main.chest[index1] = new Chest(false);
            Main.chest[index1].x = X;
            Main.chest[index1].y = Y;
            for (var index2 = 0; index2 < 40; ++index2)
                Main.chest[index1].item[index2] = new Item();
            return index1;
        }

        public static bool CanDestroyChest(int X, int Y)
        {
            for (var index1 = 0; index1 < 1000; ++index1)
            {
                var chest = Main.chest[index1];
                if (chest != null && chest.x == X && chest.y == Y)
                {
                    for (var index2 = 0; index2 < 40; ++index2)
                    {
                        if (chest.item[index2] != null && chest.item[index2].type > 0 && chest.item[index2].stack > 0)
                            return false;
                    }

                    return true;
                }
            }

            return true;
        }

        public static bool DestroyChest(int X, int Y)
        {
            for (var index1 = 0; index1 < 1000; ++index1)
            {
                var chest = Main.chest[index1];
                if (chest != null && chest.x == X && chest.y == Y)
                {
                    for (var index2 = 0; index2 < 40; ++index2)
                    {
                        if (chest.item[index2] != null && chest.item[index2].type > 0 && chest.item[index2].stack > 0)
                            return false;
                    }

                    Main.chest[index1] = (Chest) null;
                    if (Main.player[Main.myPlayer].chest == index1)
                        Main.player[Main.myPlayer].chest = -1;
                    Recipe.FindRecipes();
                    return true;
                }
            }

            return true;
        }

        public static void DestroyChestDirect(int X, int Y, int id)
        {
            if (id < 0)
                return;
            if (id >= Main.chest.Length)
                return;
            try
            {
                var chest = Main.chest[id];
                if (chest == null || chest.x != X || chest.y != Y)
                    return;
                Main.chest[id] = (Chest) null;
                if (Main.player[Main.myPlayer].chest == id)
                    Main.player[Main.myPlayer].chest = -1;
                Recipe.FindRecipes();
            }
            catch
            {
            }
        }

        public void AddShop(Item newItem)
        {
            for (var index = 0; index < 39; ++index)
            {
                if (this.item[index] == null || this.item[index].type == 0)
                {
                    this.item[index] = newItem.Clone();
                    this.item[index].favorited = false;
                    this.item[index].buyOnce = true;
                    if (this.item[index].value <= 0)
                        break;
                    this.item[index].value /= 5;
                    if (this.item[index].value >= 1)
                        break;
                    this.item[index].value = 1;
                    break;
                }
            }
        }

        public static void SetupTravelShop()
        {
            for (var index = 0; index < 40; ++index)
                Main.travelShop[index] = 0;
            var num1 = Main.rand.Next(4, 7);
            if (Main.rand.Next(4) == 0)
                ++num1;
            if (Main.rand.Next(8) == 0)
                ++num1;
            if (Main.rand.Next(16) == 0)
                ++num1;
            if (Main.rand.Next(32) == 0)
                ++num1;
            if (Main.expertMode && Main.rand.Next(2) == 0)
                ++num1;
            var index1 = 0;
            var num2 = 0;
            var numArray = new int[6]
            {
                100,
                200,
                300,
                400,
                500,
                600
            };
            while (num2 < num1)
            {
                var num3 = 0;
                if (Main.rand.Next(numArray[4]) == 0)
                    num3 = 3309;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 3314;
                if (Main.rand.Next(numArray[5]) == 0)
                    num3 = 1987;
                if (Main.rand.Next(numArray[4]) == 0 && Main.hardMode)
                    num3 = 2270;
                if (Main.rand.Next(numArray[4]) == 0)
                    num3 = 2278;
                if (Main.rand.Next(numArray[4]) == 0)
                    num3 = 2271;
                if (Main.rand.Next(numArray[3]) == 0 && Main.hardMode && NPC.downedPlantBoss)
                    num3 = 2223;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2272;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2219;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2276;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2284;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2285;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2286;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2287;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2296;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 3628;
                if (Main.rand.Next(numArray[2]) == 0 && WorldGen.shadowOrbSmashed)
                    num3 = 2269;
                if (Main.rand.Next(numArray[2]) == 0)
                    num3 = 2177;
                if (Main.rand.Next(numArray[2]) == 0)
                    num3 = 1988;
                if (Main.rand.Next(numArray[2]) == 0)
                    num3 = 2275;
                if (Main.rand.Next(numArray[2]) == 0)
                    num3 = 2279;
                if (Main.rand.Next(numArray[2]) == 0)
                    num3 = 2277;
                if (Main.rand.Next(numArray[2]) == 0 && NPC.downedBoss1)
                    num3 = 3262;
                if (Main.rand.Next(numArray[2]) == 0 && NPC.downedMechBossAny)
                    num3 = 3284;
                if (Main.rand.Next(numArray[2]) == 0 && Main.hardMode && NPC.downedMoonlord)
                    num3 = 3596;
                if (Main.rand.Next(numArray[2]) == 0 && Main.hardMode && NPC.downedMartians)
                    num3 = 2865;
                if (Main.rand.Next(numArray[2]) == 0 && Main.hardMode && NPC.downedMartians)
                    num3 = 2866;
                if (Main.rand.Next(numArray[2]) == 0 && Main.hardMode && NPC.downedMartians)
                    num3 = 2867;
                if (Main.rand.Next(numArray[2]) == 0 && Main.xMas)
                    num3 = 3055;
                if (Main.rand.Next(numArray[2]) == 0 && Main.xMas)
                    num3 = 3056;
                if (Main.rand.Next(numArray[2]) == 0 && Main.xMas)
                    num3 = 3057;
                if (Main.rand.Next(numArray[2]) == 0 && Main.xMas)
                    num3 = 3058;
                if (Main.rand.Next(numArray[2]) == 0 && Main.xMas)
                    num3 = 3059;
                if (Main.rand.Next(numArray[1]) == 0)
                    num3 = 2214;
                if (Main.rand.Next(numArray[1]) == 0)
                    num3 = 2215;
                if (Main.rand.Next(numArray[1]) == 0)
                    num3 = 2216;
                if (Main.rand.Next(numArray[1]) == 0)
                    num3 = 2217;
                if (Main.rand.Next(numArray[1]) == 0)
                    num3 = 3624;
                if (Main.rand.Next(numArray[1]) == 0)
                    num3 = 2273;
                if (Main.rand.Next(numArray[1]) == 0)
                    num3 = 2274;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2266;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2267;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2268;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2281 + Main.rand.Next(3);
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2258;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2242;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2260;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 3637;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 3119;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 3118;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 3099;
                if (num3 != 0)
                {
                    for (var index2 = 0; index2 < 40; ++index2)
                    {
                        if (Main.travelShop[index2] == num3)
                        {
                            num3 = 0;
                            break;
                        }

                        if (num3 == 3637)
                        {
                            switch (Main.travelShop[index2])
                            {
                                case 3621:
                                case 3622:
                                case 3633:
                                case 3634:
                                case 3635:
                                case 3636:
                                case 3637:
                                case 3638:
                                case 3639:
                                case 3640:
                                case 3641:
                                case 3642:
                                    num3 = 0;
                                    break;
                            }

                            if (num3 == 0)
                                break;
                        }
                    }
                }

                if (num3 != 0)
                {
                    ++num2;
                    Main.travelShop[index1] = num3;
                    ++index1;
                    if (num3 == 2260)
                    {
                        Main.travelShop[index1] = 2261;
                        var index2 = index1 + 1;
                        Main.travelShop[index2] = 2262;
                        index1 = index2 + 1;
                    }

                    if (num3 == 3637)
                    {
                        --index1;
                        switch (Main.rand.Next(6))
                        {
                            case 0:
                                var travelShop1 = Main.travelShop;
                                var index2 = index1;
                                var num4 = index2 + 1;
                                var num5 = 3637;
                                travelShop1[index2] = num5;
                                var travelShop2 = Main.travelShop;
                                var index3 = num4;
                                index1 = index3 + 1;
                                var num6 = 3642;
                                travelShop2[index3] = num6;
                                continue;
                            case 1:
                                var travelShop3 = Main.travelShop;
                                var index4 = index1;
                                var num7 = index4 + 1;
                                var num8 = 3621;
                                travelShop3[index4] = num8;
                                var travelShop4 = Main.travelShop;
                                var index5 = num7;
                                index1 = index5 + 1;
                                var num9 = 3622;
                                travelShop4[index5] = num9;
                                continue;
                            case 2:
                                var travelShop5 = Main.travelShop;
                                var index6 = index1;
                                var num10 = index6 + 1;
                                var num11 = 3634;
                                travelShop5[index6] = num11;
                                var travelShop6 = Main.travelShop;
                                var index7 = num10;
                                index1 = index7 + 1;
                                var num12 = 3639;
                                travelShop6[index7] = num12;
                                continue;
                            case 3:
                                var travelShop7 = Main.travelShop;
                                var index8 = index1;
                                var num13 = index8 + 1;
                                var num14 = 3633;
                                travelShop7[index8] = num14;
                                var travelShop8 = Main.travelShop;
                                var index9 = num13;
                                index1 = index9 + 1;
                                var num15 = 3638;
                                travelShop8[index9] = num15;
                                continue;
                            case 4:
                                var travelShop9 = Main.travelShop;
                                var index10 = index1;
                                var num16 = index10 + 1;
                                var num17 = 3635;
                                travelShop9[index10] = num17;
                                var travelShop10 = Main.travelShop;
                                var index11 = num16;
                                index1 = index11 + 1;
                                var num18 = 3640;
                                travelShop10[index11] = num18;
                                continue;
                            case 5:
                                var travelShop11 = Main.travelShop;
                                var index12 = index1;
                                var num19 = index12 + 1;
                                var num20 = 3636;
                                travelShop11[index12] = num20;
                                var travelShop12 = Main.travelShop;
                                var index13 = num19;
                                index1 = index13 + 1;
                                var num21 = 3641;
                                travelShop12[index13] = num21;
                                continue;
                            default:
                                continue;
                        }
                    }
                }
            }
        }

        public void SetupShop(int type)
        {
            for (var index = 0; index < 40; ++index)
                this.item[index] = new Item();
            var index1 = 0;
            switch (type)
            {
                case 1:
                    this.item[index1].SetDefaults(88, false);
                    var index2 = index1 + 1;
                    this.item[index2].SetDefaults(87, false);
                    var index3 = index2 + 1;
                    this.item[index3].SetDefaults(35, false);
                    var index4 = index3 + 1;
                    this.item[index4].SetDefaults(1991, false);
                    var index5 = index4 + 1;
                    this.item[index5].SetDefaults(3509, false);
                    var index6 = index5 + 1;
                    this.item[index6].SetDefaults(3506, false);
                    var index7 = index6 + 1;
                    this.item[index7].SetDefaults(8, false);
                    var index8 = index7 + 1;
                    this.item[index8].SetDefaults(28, false);
                    var index9 = index8 + 1;
                    this.item[index9].SetDefaults(110, false);
                    var index10 = index9 + 1;
                    this.item[index10].SetDefaults(40, false);
                    var index11 = index10 + 1;
                    this.item[index11].SetDefaults(42, false);
                    var index12 = index11 + 1;
                    this.item[index12].SetDefaults(965, false);
                    var index13 = index12 + 1;
                    if (Main.player[Main.myPlayer].ZoneSnow)
                    {
                        this.item[index13].SetDefaults(967, false);
                        ++index13;
                    }

                    if (Main.bloodMoon)
                    {
                        this.item[index13].SetDefaults(279, false);
                        ++index13;
                    }

                    if (!Main.dayTime)
                    {
                        this.item[index13].SetDefaults(282, false);
                        ++index13;
                    }

                    if (NPC.downedBoss3)
                    {
                        this.item[index13].SetDefaults(346, false);
                        ++index13;
                    }

                    if (Main.hardMode)
                    {
                        this.item[index13].SetDefaults(488, false);
                        ++index13;
                    }

                    for (var index14 = 0; index14 < 58; ++index14)
                    {
                        if (Main.player[Main.myPlayer].inventory[index14].type == 930)
                        {
                            this.item[index13].SetDefaults(931, false);
                            var index15 = index13 + 1;
                            this.item[index15].SetDefaults(1614, false);
                            index13 = index15 + 1;
                            break;
                        }
                    }

                    this.item[index13].SetDefaults(1786, false);
                    index1 = index13 + 1;
                    if (Main.hardMode)
                    {
                        this.item[index1].SetDefaults(1348, false);
                        ++index1;
                    }

                    if (Main.player[Main.myPlayer].HasItem(3107))
                    {
                        this.item[index1].SetDefaults(3108, false);
                        ++index1;
                    }

                    if (Main.halloween)
                    {
                        var objArray1 = this.item;
                        var index14 = index1;
                        var num1 = index14 + 1;
                        objArray1[index14].SetDefaults(3242, false);
                        var objArray2 = this.item;
                        var index15 = num1;
                        var num2 = index15 + 1;
                        objArray2[index15].SetDefaults(3243, false);
                        var objArray3 = this.item;
                        var index16 = num2;
                        index1 = index16 + 1;
                        objArray3[index16].SetDefaults(3244, false);
                        break;
                    }

                    break;
                case 2:
                    this.item[index1].SetDefaults(97, false);
                    var index17 = index1 + 1;
                    if (Main.bloodMoon || Main.hardMode)
                    {
                        this.item[index17].SetDefaults(278, false);
                        ++index17;
                    }

                    if (NPC.downedBoss2 && !Main.dayTime || Main.hardMode)
                    {
                        this.item[index17].SetDefaults(47, false);
                        ++index17;
                    }

                    this.item[index17].SetDefaults(95, false);
                    var index18 = index17 + 1;
                    this.item[index18].SetDefaults(98, false);
                    index1 = index18 + 1;
                    if (!Main.dayTime)
                    {
                        this.item[index1].SetDefaults(324, false);
                        ++index1;
                    }

                    if (Main.hardMode)
                    {
                        this.item[index1].SetDefaults(534, false);
                        ++index1;
                    }

                    if (Main.hardMode)
                    {
                        this.item[index1].SetDefaults(1432, false);
                        ++index1;
                    }

                    if (Main.player[Main.myPlayer].HasItem(1258))
                    {
                        this.item[index1].SetDefaults(1261, false);
                        ++index1;
                    }

                    if (Main.player[Main.myPlayer].HasItem(1835))
                    {
                        this.item[index1].SetDefaults(1836, false);
                        ++index1;
                    }

                    if (Main.player[Main.myPlayer].HasItem(3107))
                    {
                        this.item[index1].SetDefaults(3108, false);
                        ++index1;
                    }

                    if (Main.player[Main.myPlayer].HasItem(1782))
                    {
                        this.item[index1].SetDefaults(1783, false);
                        ++index1;
                    }

                    if (Main.player[Main.myPlayer].HasItem(1784))
                    {
                        this.item[index1].SetDefaults(1785, false);
                        ++index1;
                    }

                    if (Main.halloween)
                    {
                        this.item[index1].SetDefaults(1736, false);
                        var index14 = index1 + 1;
                        this.item[index14].SetDefaults(1737, false);
                        var index15 = index14 + 1;
                        this.item[index15].SetDefaults(1738, false);
                        index1 = index15 + 1;
                        break;
                    }

                    break;
                case 3:
                    int index19;
                    if (Main.bloodMoon)
                    {
                        if (WorldGen.crimson)
                        {
                            this.item[index1].SetDefaults(2886, false);
                            var index14 = index1 + 1;
                            this.item[index14].SetDefaults(2171, false);
                            index19 = index14 + 1;
                        }
                        else
                        {
                            this.item[index1].SetDefaults(67, false);
                            var index14 = index1 + 1;
                            this.item[index14].SetDefaults(59, false);
                            index19 = index14 + 1;
                        }
                    }
                    else
                    {
                        this.item[index1].SetDefaults(66, false);
                        var index14 = index1 + 1;
                        this.item[index14].SetDefaults(62, false);
                        var index15 = index14 + 1;
                        this.item[index15].SetDefaults(63, false);
                        index19 = index15 + 1;
                    }

                    this.item[index19].SetDefaults(27, false);
                    var index20 = index19 + 1;
                    this.item[index20].SetDefaults(114, false);
                    var index21 = index20 + 1;
                    this.item[index21].SetDefaults(1828, false);
                    var index22 = index21 + 1;
                    this.item[index22].SetDefaults(745, false);
                    var index23 = index22 + 1;
                    this.item[index23].SetDefaults(747, false);
                    index1 = index23 + 1;
                    if (Main.hardMode)
                    {
                        this.item[index1].SetDefaults(746, false);
                        ++index1;
                    }

                    if (Main.hardMode)
                    {
                        this.item[index1].SetDefaults(369, false);
                        ++index1;
                    }

                    if (Main.shroomTiles > 50)
                    {
                        this.item[index1].SetDefaults(194, false);
                        ++index1;
                    }

                    if (Main.halloween)
                    {
                        this.item[index1].SetDefaults(1853, false);
                        var index14 = index1 + 1;
                        this.item[index14].SetDefaults(1854, false);
                        index1 = index14 + 1;
                    }

                    if (NPC.downedSlimeKing)
                    {
                        this.item[index1].SetDefaults(3215, false);
                        ++index1;
                    }

                    if (NPC.downedQueenBee)
                    {
                        this.item[index1].SetDefaults(3216, false);
                        ++index1;
                    }

                    if (NPC.downedBoss1)
                    {
                        this.item[index1].SetDefaults(3219, false);
                        ++index1;
                    }

                    if (NPC.downedBoss2)
                    {
                        if (WorldGen.crimson)
                        {
                            this.item[index1].SetDefaults(3218, false);
                            ++index1;
                        }
                        else
                        {
                            this.item[index1].SetDefaults(3217, false);
                            ++index1;
                        }
                    }

                    if (NPC.downedBoss3)
                    {
                        this.item[index1].SetDefaults(3220, false);
                        var index14 = index1 + 1;
                        this.item[index14].SetDefaults(3221, false);
                        index1 = index14 + 1;
                    }

                    if (Main.hardMode)
                    {
                        this.item[index1].SetDefaults(3222, false);
                        ++index1;
                        break;
                    }

                    break;
                case 4:
                    this.item[index1].SetDefaults(168, false);
                    var index24 = index1 + 1;
                    this.item[index24].SetDefaults(166, false);
                    var index25 = index24 + 1;
                    this.item[index25].SetDefaults(167, false);
                    index1 = index25 + 1;
                    if (Main.hardMode)
                    {
                        this.item[index1].SetDefaults(265, false);
                        ++index1;
                    }

                    if (Main.hardMode && NPC.downedPlantBoss && NPC.downedPirates)
                    {
                        this.item[index1].SetDefaults(937, false);
                        ++index1;
                    }

                    if (Main.hardMode)
                    {
                        this.item[index1].SetDefaults(1347, false);
                        ++index1;
                        break;
                    }

                    break;
                case 5:
                    this.item[index1].SetDefaults(254, false);
                    var index26 = index1 + 1;
                    this.item[index26].SetDefaults(981, false);
                    var index27 = index26 + 1;
                    if (Main.dayTime)
                    {
                        this.item[index27].SetDefaults(242, false);
                        ++index27;
                    }

                    switch (Main.moonPhase)
                    {
                        case 0:
                            this.item[index27].SetDefaults(245, false);
                            var index28 = index27 + 1;
                            this.item[index28].SetDefaults(246, false);
                            index27 = index28 + 1;
                            if (!Main.dayTime)
                            {
                                var objArray1 = this.item;
                                var index14 = index27;
                                var num = index14 + 1;
                                objArray1[index14].SetDefaults(1288, false);
                                var objArray2 = this.item;
                                var index15 = num;
                                index27 = index15 + 1;
                                objArray2[index15].SetDefaults(1289, false);
                                break;
                            }

                            break;
                        case 1:
                            this.item[index27].SetDefaults(325, false);
                            var index29 = index27 + 1;
                            this.item[index29].SetDefaults(326, false);
                            index27 = index29 + 1;
                            break;
                    }

                    this.item[index27].SetDefaults(269, false);
                    var index30 = index27 + 1;
                    this.item[index30].SetDefaults(270, false);
                    var index31 = index30 + 1;
                    this.item[index31].SetDefaults(271, false);
                    index1 = index31 + 1;
                    if (NPC.downedClown)
                    {
                        this.item[index1].SetDefaults(503, false);
                        var index14 = index1 + 1;
                        this.item[index14].SetDefaults(504, false);
                        var index15 = index14 + 1;
                        this.item[index15].SetDefaults(505, false);
                        index1 = index15 + 1;
                    }

                    if (Main.bloodMoon)
                    {
                        this.item[index1].SetDefaults(322, false);
                        ++index1;
                        if (!Main.dayTime)
                        {
                            var objArray1 = this.item;
                            var index14 = index1;
                            var num = index14 + 1;
                            objArray1[index14].SetDefaults(3362, false);
                            var objArray2 = this.item;
                            var index15 = num;
                            index1 = index15 + 1;
                            objArray2[index15].SetDefaults(3363, false);
                        }
                    }

                    if (NPC.downedAncientCultist)
                    {
                        if (Main.dayTime)
                        {
                            var objArray1 = this.item;
                            var index14 = index1;
                            var num = index14 + 1;
                            objArray1[index14].SetDefaults(2856, false);
                            var objArray2 = this.item;
                            var index15 = num;
                            index1 = index15 + 1;
                            objArray2[index15].SetDefaults(2858, false);
                        }
                        else
                        {
                            var objArray1 = this.item;
                            var index14 = index1;
                            var num = index14 + 1;
                            objArray1[index14].SetDefaults(2857, false);
                            var objArray2 = this.item;
                            var index15 = num;
                            index1 = index15 + 1;
                            objArray2[index15].SetDefaults(2859, false);
                        }
                    }

                    if (NPC.AnyNPCs(441))
                    {
                        var objArray1 = this.item;
                        var index14 = index1;
                        var num1 = index14 + 1;
                        objArray1[index14].SetDefaults(3242, false);
                        var objArray2 = this.item;
                        var index15 = num1;
                        var num2 = index15 + 1;
                        objArray2[index15].SetDefaults(3243, false);
                        var objArray3 = this.item;
                        var index16 = num2;
                        index1 = index16 + 1;
                        objArray3[index16].SetDefaults(3244, false);
                    }

                    if (Main.player[Main.myPlayer].ZoneSnow)
                    {
                        this.item[index1].SetDefaults(1429, false);
                        ++index1;
                    }

                    if (Main.halloween)
                    {
                        this.item[index1].SetDefaults(1740, false);
                        ++index1;
                    }

                    if (Main.hardMode)
                    {
                        if (Main.moonPhase == 2)
                        {
                            this.item[index1].SetDefaults(869, false);
                            ++index1;
                        }

                        if (Main.moonPhase == 4)
                        {
                            this.item[index1].SetDefaults(864, false);
                            var index14 = index1 + 1;
                            this.item[index14].SetDefaults(865, false);
                            index1 = index14 + 1;
                        }

                        if (Main.moonPhase == 6)
                        {
                            this.item[index1].SetDefaults(873, false);
                            var index14 = index1 + 1;
                            this.item[index14].SetDefaults(874, false);
                            var index15 = index14 + 1;
                            this.item[index15].SetDefaults(875, false);
                            index1 = index15 + 1;
                        }
                    }

                    if (NPC.downedFrost)
                    {
                        this.item[index1].SetDefaults(1275, false);
                        var index14 = index1 + 1;
                        this.item[index14].SetDefaults(1276, false);
                        index1 = index14 + 1;
                    }

                    if (Main.halloween)
                    {
                        var objArray1 = this.item;
                        var index14 = index1;
                        var num = index14 + 1;
                        objArray1[index14].SetDefaults(3246, false);
                        var objArray2 = this.item;
                        var index15 = num;
                        index1 = index15 + 1;
                        objArray2[index15].SetDefaults(3247, false);
                    }

                    if (BirthdayParty.PartyIsUp)
                    {
                        var objArray1 = this.item;
                        var index14 = index1;
                        var num1 = index14 + 1;
                        objArray1[index14].SetDefaults(3730, false);
                        var objArray2 = this.item;
                        var index15 = num1;
                        var num2 = index15 + 1;
                        objArray2[index15].SetDefaults(3731, false);
                        var objArray3 = this.item;
                        var index16 = num2;
                        var num3 = index16 + 1;
                        objArray3[index16].SetDefaults(3733, false);
                        var objArray4 = this.item;
                        var index32 = num3;
                        var num4 = index32 + 1;
                        objArray4[index32].SetDefaults(3734, false);
                        var objArray5 = this.item;
                        var index33 = num4;
                        index1 = index33 + 1;
                        objArray5[index33].SetDefaults(3735, false);
                        break;
                    }

                    break;
                case 6:
                    this.item[index1].SetDefaults(128, false);
                    var index34 = index1 + 1;
                    this.item[index34].SetDefaults(486, false);
                    var index35 = index34 + 1;
                    this.item[index35].SetDefaults(398, false);
                    var index36 = index35 + 1;
                    this.item[index36].SetDefaults(84, false);
                    var index37 = index36 + 1;
                    this.item[index37].SetDefaults(407, false);
                    var index38 = index37 + 1;
                    this.item[index38].SetDefaults(161, false);
                    index1 = index38 + 1;
                    break;
                case 7:
                    this.item[index1].SetDefaults(487, false);
                    var index39 = index1 + 1;
                    this.item[index39].SetDefaults(496, false);
                    var index40 = index39 + 1;
                    this.item[index40].SetDefaults(500, false);
                    var index41 = index40 + 1;
                    this.item[index41].SetDefaults(507, false);
                    var index42 = index41 + 1;
                    this.item[index42].SetDefaults(508, false);
                    var index43 = index42 + 1;
                    this.item[index43].SetDefaults(531, false);
                    var index44 = index43 + 1;
                    this.item[index44].SetDefaults(576, false);
                    var index45 = index44 + 1;
                    this.item[index45].SetDefaults(3186, false);
                    index1 = index45 + 1;
                    if (Main.halloween)
                    {
                        this.item[index1].SetDefaults(1739, false);
                        ++index1;
                        break;
                    }

                    break;
                case 8:
                    this.item[index1].SetDefaults(509, false);
                    var index46 = index1 + 1;
                    this.item[index46].SetDefaults(850, false);
                    var index47 = index46 + 1;
                    this.item[index47].SetDefaults(851, false);
                    var index48 = index47 + 1;
                    this.item[index48].SetDefaults(3612, false);
                    var index49 = index48 + 1;
                    this.item[index49].SetDefaults(510, false);
                    var index50 = index49 + 1;
                    this.item[index50].SetDefaults(530, false);
                    var index51 = index50 + 1;
                    this.item[index51].SetDefaults(513, false);
                    var index52 = index51 + 1;
                    this.item[index52].SetDefaults(538, false);
                    var index53 = index52 + 1;
                    this.item[index53].SetDefaults(529, false);
                    var index54 = index53 + 1;
                    this.item[index54].SetDefaults(541, false);
                    var index55 = index54 + 1;
                    this.item[index55].SetDefaults(542, false);
                    var index56 = index55 + 1;
                    this.item[index56].SetDefaults(543, false);
                    var index57 = index56 + 1;
                    this.item[index57].SetDefaults(852, false);
                    var index58 = index57 + 1;
                    this.item[index58].SetDefaults(853, false);
                    var num5 = index58 + 1;
                    var objArray6 = this.item;
                    var index59 = num5;
                    var index60 = index59 + 1;
                    objArray6[index59].SetDefaults(3707, false);
                    this.item[index60].SetDefaults(2739, false);
                    var index61 = index60 + 1;
                    this.item[index61].SetDefaults(849, false);
                    var num6 = index61 + 1;
                    var objArray7 = this.item;
                    var index62 = num6;
                    var num7 = index62 + 1;
                    objArray7[index62].SetDefaults(3616, false);
                    var objArray8 = this.item;
                    var index63 = num7;
                    var num8 = index63 + 1;
                    objArray8[index63].SetDefaults(2799, false);
                    var objArray9 = this.item;
                    var index64 = num8;
                    var num9 = index64 + 1;
                    objArray9[index64].SetDefaults(3619, false);
                    var objArray10 = this.item;
                    var index65 = num9;
                    var num10 = index65 + 1;
                    objArray10[index65].SetDefaults(3627, false);
                    var objArray11 = this.item;
                    var index66 = num10;
                    index1 = index66 + 1;
                    objArray11[index66].SetDefaults(3629, false);
                    if (NPC.AnyNPCs(369) && Main.hardMode && Main.moonPhase == 3)
                    {
                        this.item[index1].SetDefaults(2295, false);
                        ++index1;
                        break;
                    }

                    break;
                case 9:
                    this.item[index1].SetDefaults(588, false);
                    var index67 = index1 + 1;
                    this.item[index67].SetDefaults(589, false);
                    var index68 = index67 + 1;
                    this.item[index68].SetDefaults(590, false);
                    var index69 = index68 + 1;
                    this.item[index69].SetDefaults(597, false);
                    var index70 = index69 + 1;
                    this.item[index70].SetDefaults(598, false);
                    var index71 = index70 + 1;
                    this.item[index71].SetDefaults(596, false);
                    index1 = index71 + 1;
                    for (var Type = 1873; Type < 1906; ++Type)
                    {
                        this.item[index1].SetDefaults(Type, false);
                        ++index1;
                    }

                    break;
                case 10:
                    if (NPC.downedMechBossAny)
                    {
                        this.item[index1].SetDefaults(756, false);
                        var index14 = index1 + 1;
                        this.item[index14].SetDefaults(787, false);
                        index1 = index14 + 1;
                    }

                    this.item[index1].SetDefaults(868, false);
                    var index72 = index1 + 1;
                    if (NPC.downedPlantBoss)
                    {
                        this.item[index72].SetDefaults(1551, false);
                        ++index72;
                    }

                    this.item[index72].SetDefaults(1181, false);
                    var index73 = index72 + 1;
                    this.item[index73].SetDefaults(783, false);
                    index1 = index73 + 1;
                    break;
                case 11:
                    this.item[index1].SetDefaults(779, false);
                    var index74 = index1 + 1;
                    int index75;
                    if (Main.moonPhase >= 4)
                    {
                        this.item[index74].SetDefaults(748, false);
                        index75 = index74 + 1;
                    }
                    else
                    {
                        this.item[index74].SetDefaults(839, false);
                        var index14 = index74 + 1;
                        this.item[index14].SetDefaults(840, false);
                        var index15 = index14 + 1;
                        this.item[index15].SetDefaults(841, false);
                        index75 = index15 + 1;
                    }

                    if (NPC.downedGolemBoss)
                    {
                        this.item[index75].SetDefaults(948, false);
                        ++index75;
                    }

                    var objArray12 = this.item;
                    var index76 = index75;
                    var num11 = index76 + 1;
                    objArray12[index76].SetDefaults(3623, false);
                    var objArray13 = this.item;
                    var index77 = num11;
                    var num12 = index77 + 1;
                    objArray13[index77].SetDefaults(3603, false);
                    var objArray14 = this.item;
                    var index78 = num12;
                    var num13 = index78 + 1;
                    objArray14[index78].SetDefaults(3604, false);
                    var objArray15 = this.item;
                    var index79 = num13;
                    var num14 = index79 + 1;
                    objArray15[index79].SetDefaults(3607, false);
                    var objArray16 = this.item;
                    var index80 = num14;
                    var num15 = index80 + 1;
                    objArray16[index80].SetDefaults(3605, false);
                    var objArray17 = this.item;
                    var index81 = num15;
                    var num16 = index81 + 1;
                    objArray17[index81].SetDefaults(3606, false);
                    var objArray18 = this.item;
                    var index82 = num16;
                    var num17 = index82 + 1;
                    objArray18[index82].SetDefaults(3608, false);
                    var objArray19 = this.item;
                    var index83 = num17;
                    var num18 = index83 + 1;
                    objArray19[index83].SetDefaults(3618, false);
                    var objArray20 = this.item;
                    var index84 = num18;
                    var num19 = index84 + 1;
                    objArray20[index84].SetDefaults(3602, false);
                    var objArray21 = this.item;
                    var index85 = num19;
                    var num20 = index85 + 1;
                    objArray21[index85].SetDefaults(3663, false);
                    var objArray22 = this.item;
                    var index86 = num20;
                    var num21 = index86 + 1;
                    objArray22[index86].SetDefaults(3609, false);
                    var objArray23 = this.item;
                    var index87 = num21;
                    var index88 = index87 + 1;
                    objArray23[index87].SetDefaults(3610, false);
                    this.item[index88].SetDefaults(995, false);
                    var index89 = index88 + 1;
                    if (NPC.downedBoss1 && NPC.downedBoss2 && NPC.downedBoss3)
                    {
                        this.item[index89].SetDefaults(2203, false);
                        ++index89;
                    }

                    if (WorldGen.crimson)
                    {
                        this.item[index89].SetDefaults(2193, false);
                        ++index89;
                    }

                    this.item[index89].SetDefaults(1263, false);
                    var index90 = index89 + 1;
                    if (Main.eclipse || Main.bloodMoon)
                    {
                        if (WorldGen.crimson)
                        {
                            this.item[index90].SetDefaults(784, false);
                            index1 = index90 + 1;
                        }
                        else
                        {
                            this.item[index90].SetDefaults(782, false);
                            index1 = index90 + 1;
                        }
                    }
                    else if (Main.player[Main.myPlayer].ZoneHoly)
                    {
                        this.item[index90].SetDefaults(781, false);
                        index1 = index90 + 1;
                    }
                    else
                    {
                        this.item[index90].SetDefaults(780, false);
                        index1 = index90 + 1;
                    }

                    if (Main.hardMode)
                    {
                        this.item[index1].SetDefaults(1344, false);
                        ++index1;
                    }

                    if (Main.halloween)
                    {
                        this.item[index1].SetDefaults(1742, false);
                        ++index1;
                        break;
                    }

                    break;
                case 12:
                    this.item[index1].SetDefaults(1037, false);
                    var index91 = index1 + 1;
                    this.item[index91].SetDefaults(2874, false);
                    var index92 = index91 + 1;
                    this.item[index92].SetDefaults(1120, false);
                    index1 = index92 + 1;
                    if (Main.netMode == 1)
                    {
                        this.item[index1].SetDefaults(1969, false);
                        ++index1;
                    }

                    if (Main.halloween)
                    {
                        this.item[index1].SetDefaults(3248, false);
                        var index14 = index1 + 1;
                        this.item[index14].SetDefaults(1741, false);
                        index1 = index14 + 1;
                    }

                    if (Main.moonPhase == 0)
                    {
                        this.item[index1].SetDefaults(2871, false);
                        var index14 = index1 + 1;
                        this.item[index14].SetDefaults(2872, false);
                        index1 = index14 + 1;
                        break;
                    }

                    break;
                case 13:
                    this.item[index1].SetDefaults(859, false);
                    var index93 = index1 + 1;
                    this.item[index93].SetDefaults(1000, false);
                    var index94 = index93 + 1;
                    this.item[index94].SetDefaults(1168, false);
                    var index95 = index94 + 1;
                    this.item[index95].SetDefaults(1449, false);
                    var index96 = index95 + 1;
                    this.item[index96].SetDefaults(1345, false);
                    var index97 = index96 + 1;
                    this.item[index97].SetDefaults(1450, false);
                    var num22 = index97 + 1;
                    var objArray24 = this.item;
                    var index98 = num22;
                    var num23 = index98 + 1;
                    objArray24[index98].SetDefaults(3253, false);
                    var objArray25 = this.item;
                    var index99 = num23;
                    var num24 = index99 + 1;
                    objArray25[index99].SetDefaults(2700, false);
                    var objArray26 = this.item;
                    var index100 = num24;
                    var index101 = index100 + 1;
                    objArray26[index100].SetDefaults(2738, false);
                    if (Main.player[Main.myPlayer].HasItem(3548))
                    {
                        this.item[index101].SetDefaults(3548, false);
                        ++index101;
                    }

                    if (NPC.AnyNPCs(229))
                        this.item[index101++].SetDefaults(3369, false);
                    if (Main.hardMode)
                    {
                        this.item[index101].SetDefaults(3214, false);
                        var index14 = index101 + 1;
                        this.item[index14].SetDefaults(2868, false);
                        var index15 = index14 + 1;
                        this.item[index15].SetDefaults(970, false);
                        var index16 = index15 + 1;
                        this.item[index16].SetDefaults(971, false);
                        var index32 = index16 + 1;
                        this.item[index32].SetDefaults(972, false);
                        var index33 = index32 + 1;
                        this.item[index33].SetDefaults(973, false);
                        index101 = index33 + 1;
                    }

                    var objArray27 = this.item;
                    var index102 = index101;
                    var num25 = index102 + 1;
                    objArray27[index102].SetDefaults(3747, false);
                    var objArray28 = this.item;
                    var index103 = num25;
                    var num26 = index103 + 1;
                    objArray28[index103].SetDefaults(3732, false);
                    var objArray29 = this.item;
                    var index104 = num26;
                    index1 = index104 + 1;
                    objArray29[index104].SetDefaults(3742, false);
                    if (BirthdayParty.PartyIsUp)
                    {
                        var objArray1 = this.item;
                        var index14 = index1;
                        var num1 = index14 + 1;
                        objArray1[index14].SetDefaults(3749, false);
                        var objArray2 = this.item;
                        var index15 = num1;
                        var num2 = index15 + 1;
                        objArray2[index15].SetDefaults(3746, false);
                        var objArray3 = this.item;
                        var index16 = num2;
                        var num3 = index16 + 1;
                        objArray3[index16].SetDefaults(3739, false);
                        var objArray4 = this.item;
                        var index32 = num3;
                        var num4 = index32 + 1;
                        objArray4[index32].SetDefaults(3740, false);
                        var objArray5 = this.item;
                        var index33 = num4;
                        var num27 = index33 + 1;
                        objArray5[index33].SetDefaults(3741, false);
                        var objArray30 = this.item;
                        var index105 = num27;
                        var num28 = index105 + 1;
                        objArray30[index105].SetDefaults(3737, false);
                        var objArray31 = this.item;
                        var index106 = num28;
                        var num29 = index106 + 1;
                        objArray31[index106].SetDefaults(3738, false);
                        var objArray32 = this.item;
                        var index107 = num29;
                        var num30 = index107 + 1;
                        objArray32[index107].SetDefaults(3736, false);
                        var objArray33 = this.item;
                        var index108 = num30;
                        var num31 = index108 + 1;
                        objArray33[index108].SetDefaults(3745, false);
                        var objArray34 = this.item;
                        var index109 = num31;
                        var num32 = index109 + 1;
                        objArray34[index109].SetDefaults(3744, false);
                        var objArray35 = this.item;
                        var index110 = num32;
                        index1 = index110 + 1;
                        objArray35[index110].SetDefaults(3743, false);
                        break;
                    }

                    break;
                case 14:
                    this.item[index1].SetDefaults(771, false);
                    ++index1;
                    if (Main.bloodMoon)
                    {
                        this.item[index1].SetDefaults(772, false);
                        ++index1;
                    }

                    if (!Main.dayTime || Main.eclipse)
                    {
                        this.item[index1].SetDefaults(773, false);
                        ++index1;
                    }

                    if (Main.eclipse)
                    {
                        this.item[index1].SetDefaults(774, false);
                        ++index1;
                    }

                    if (Main.hardMode)
                    {
                        this.item[index1].SetDefaults(760, false);
                        ++index1;
                    }

                    if (Main.hardMode)
                    {
                        this.item[index1].SetDefaults(1346, false);
                        ++index1;
                    }

                    if (Main.halloween)
                    {
                        this.item[index1].SetDefaults(1743, false);
                        var index14 = index1 + 1;
                        this.item[index14].SetDefaults(1744, false);
                        var index15 = index14 + 1;
                        this.item[index15].SetDefaults(1745, false);
                        index1 = index15 + 1;
                    }

                    if (NPC.downedMartians)
                    {
                        var objArray1 = this.item;
                        var index14 = index1;
                        var num1 = index14 + 1;
                        objArray1[index14].SetDefaults(2862, false);
                        var objArray2 = this.item;
                        var index15 = num1;
                        index1 = index15 + 1;
                        objArray2[index15].SetDefaults(3109, false);
                    }

                    if (Main.player[Main.myPlayer].HasItem(3384) || Main.player[Main.myPlayer].HasItem(3664))
                    {
                        this.item[index1].SetDefaults(3664, false);
                        ++index1;
                        break;
                    }

                    break;
                case 15:
                    this.item[index1].SetDefaults(1071, false);
                    var index111 = index1 + 1;
                    this.item[index111].SetDefaults(1072, false);
                    var index112 = index111 + 1;
                    this.item[index112].SetDefaults(1100, false);
                    var index113 = index112 + 1;
                    for (var Type = 1073; Type <= 1084; ++Type)
                    {
                        this.item[index113].SetDefaults(Type, false);
                        ++index113;
                    }

                    this.item[index113].SetDefaults(1097, false);
                    var index114 = index113 + 1;
                    this.item[index114].SetDefaults(1099, false);
                    var index115 = index114 + 1;
                    this.item[index115].SetDefaults(1098, false);
                    var index116 = index115 + 1;
                    this.item[index116].SetDefaults(1966, false);
                    var index117 = index116 + 1;
                    if (Main.hardMode)
                    {
                        this.item[index117].SetDefaults(1967, false);
                        var index14 = index117 + 1;
                        this.item[index14].SetDefaults(1968, false);
                        index117 = index14 + 1;
                    }

                    this.item[index117].SetDefaults(1490, false);
                    var index118 = index117 + 1;
                    if (Main.moonPhase <= 1)
                    {
                        this.item[index118].SetDefaults(1481, false);
                        index1 = index118 + 1;
                    }
                    else if (Main.moonPhase <= 3)
                    {
                        this.item[index118].SetDefaults(1482, false);
                        index1 = index118 + 1;
                    }
                    else if (Main.moonPhase <= 5)
                    {
                        this.item[index118].SetDefaults(1483, false);
                        index1 = index118 + 1;
                    }
                    else
                    {
                        this.item[index118].SetDefaults(1484, false);
                        index1 = index118 + 1;
                    }

                    if (Main.player[Main.myPlayer].ZoneCrimson)
                    {
                        this.item[index1].SetDefaults(1492, false);
                        ++index1;
                    }

                    if (Main.player[Main.myPlayer].ZoneCorrupt)
                    {
                        this.item[index1].SetDefaults(1488, false);
                        ++index1;
                    }

                    if (Main.player[Main.myPlayer].ZoneHoly)
                    {
                        this.item[index1].SetDefaults(1489, false);
                        ++index1;
                    }

                    if (Main.player[Main.myPlayer].ZoneJungle)
                    {
                        this.item[index1].SetDefaults(1486, false);
                        ++index1;
                    }

                    if (Main.player[Main.myPlayer].ZoneSnow)
                    {
                        this.item[index1].SetDefaults(1487, false);
                        ++index1;
                    }

                    if (Main.sandTiles > 1000)
                    {
                        this.item[index1].SetDefaults(1491, false);
                        ++index1;
                    }

                    if (Main.bloodMoon)
                    {
                        this.item[index1].SetDefaults(1493, false);
                        ++index1;
                    }

                    if ((double) Main.player[Main.myPlayer].position.Y / 16.0 < Main.worldSurface * 0.349999994039536)
                    {
                        this.item[index1].SetDefaults(1485, false);
                        ++index1;
                    }

                    if ((double) Main.player[Main.myPlayer].position.Y / 16.0 < Main.worldSurface * 0.349999994039536 &&
                        Main.hardMode)
                    {
                        this.item[index1].SetDefaults(1494, false);
                        ++index1;
                    }

                    if (Main.xMas)
                    {
                        for (var Type = 1948; Type <= 1957; ++Type)
                        {
                            this.item[index1].SetDefaults(Type, false);
                            ++index1;
                        }
                    }

                    for (var Type = 2158; Type <= 2160; ++Type)
                    {
                        if (index1 < 39)
                            this.item[index1].SetDefaults(Type, false);
                        ++index1;
                    }

                    for (var Type = 2008; Type <= 2014; ++Type)
                    {
                        if (index1 < 39)
                            this.item[index1].SetDefaults(Type, false);
                        ++index1;
                    }

                    break;
                case 16:
                    this.item[index1].SetDefaults(1430, false);
                    var index119 = index1 + 1;
                    this.item[index119].SetDefaults(986, false);
                    var index120 = index119 + 1;
                    if (NPC.AnyNPCs(108))
                        this.item[index120++].SetDefaults(2999, false);
                    if (Main.hardMode && NPC.downedPlantBoss)
                    {
                        if (Main.player[Main.myPlayer].HasItem(1157))
                        {
                            this.item[index120].SetDefaults(1159, false);
                            var index14 = index120 + 1;
                            this.item[index14].SetDefaults(1160, false);
                            var index15 = index14 + 1;
                            this.item[index15].SetDefaults(1161, false);
                            index120 = index15 + 1;
                            if (!Main.dayTime)
                            {
                                this.item[index120].SetDefaults(1158, false);
                                ++index120;
                            }

                            if (Main.player[Main.myPlayer].ZoneJungle)
                            {
                                this.item[index120].SetDefaults(1167, false);
                                ++index120;
                            }
                        }

                        this.item[index120].SetDefaults(1339, false);
                        ++index120;
                    }

                    if (Main.hardMode && Main.player[Main.myPlayer].ZoneJungle)
                    {
                        this.item[index120].SetDefaults(1171, false);
                        ++index120;
                        if (!Main.dayTime)
                        {
                            this.item[index120].SetDefaults(1162, false);
                            ++index120;
                        }
                    }

                    this.item[index120].SetDefaults(909, false);
                    var index121 = index120 + 1;
                    this.item[index121].SetDefaults(910, false);
                    var index122 = index121 + 1;
                    this.item[index122].SetDefaults(940, false);
                    var index123 = index122 + 1;
                    this.item[index123].SetDefaults(941, false);
                    var index124 = index123 + 1;
                    this.item[index124].SetDefaults(942, false);
                    var index125 = index124 + 1;
                    this.item[index125].SetDefaults(943, false);
                    var index126 = index125 + 1;
                    this.item[index126].SetDefaults(944, false);
                    var index127 = index126 + 1;
                    this.item[index127].SetDefaults(945, false);
                    index1 = index127 + 1;
                    if (Main.player[Main.myPlayer].HasItem(1835))
                    {
                        this.item[index1].SetDefaults(1836, false);
                        ++index1;
                    }

                    if (Main.player[Main.myPlayer].HasItem(1258))
                    {
                        this.item[index1].SetDefaults(1261, false);
                        ++index1;
                    }

                    if (Main.halloween)
                    {
                        this.item[index1].SetDefaults(1791, false);
                        ++index1;
                        break;
                    }

                    break;
                case 17:
                    this.item[index1].SetDefaults(928, false);
                    var index128 = index1 + 1;
                    this.item[index128].SetDefaults(929, false);
                    var index129 = index128 + 1;
                    this.item[index129].SetDefaults(876, false);
                    var index130 = index129 + 1;
                    this.item[index130].SetDefaults(877, false);
                    var index131 = index130 + 1;
                    this.item[index131].SetDefaults(878, false);
                    var index132 = index131 + 1;
                    this.item[index132].SetDefaults(2434, false);
                    index1 = index132 + 1;
                    var num33 = (int) (((double) Main.screenPosition.X + (double) (Main.screenWidth / 2)) / 16.0);
                    if ((double) Main.screenPosition.Y / 16.0 < Main.worldSurface + 10.0 &&
                        (num33 < 380 || num33 > Main.maxTilesX - 380))
                    {
                        this.item[index1].SetDefaults(1180, false);
                        ++index1;
                    }

                    if (Main.hardMode && NPC.downedMechBossAny && NPC.AnyNPCs(208))
                    {
                        this.item[index1].SetDefaults(1337, false);
                        ++index1;
                        break;
                    }

                    break;
                case 18:
                    this.item[index1].SetDefaults(1990, false);
                    var index133 = index1 + 1;
                    this.item[index133].SetDefaults(1979, false);
                    index1 = index133 + 1;
                    if (Main.player[Main.myPlayer].statLifeMax >= 400)
                    {
                        this.item[index1].SetDefaults(1977, false);
                        ++index1;
                    }

                    if (Main.player[Main.myPlayer].statManaMax >= 200)
                    {
                        this.item[index1].SetDefaults(1978, false);
                        ++index1;
                    }

                    long num34 = 0;
                    for (var index14 = 0; index14 < 54; ++index14)
                    {
                        if (Main.player[Main.myPlayer].inventory[index14].type == 71)
                            num34 += (long) Main.player[Main.myPlayer].inventory[index14].stack;
                        if (Main.player[Main.myPlayer].inventory[index14].type == 72)
                            num34 += (long) (Main.player[Main.myPlayer].inventory[index14].stack * 100);
                        if (Main.player[Main.myPlayer].inventory[index14].type == 73)
                            num34 += (long) (Main.player[Main.myPlayer].inventory[index14].stack * 10000);
                        if (Main.player[Main.myPlayer].inventory[index14].type == 74)
                            num34 += (long) (Main.player[Main.myPlayer].inventory[index14].stack * 1000000);
                    }

                    if (num34 >= 1000000L)
                    {
                        this.item[index1].SetDefaults(1980, false);
                        ++index1;
                    }

                    if (Main.moonPhase % 2 == 0 && Main.dayTime || Main.moonPhase % 2 == 1 && !Main.dayTime)
                    {
                        this.item[index1].SetDefaults(1981, false);
                        ++index1;
                    }

                    if (Main.player[Main.myPlayer].team != 0)
                    {
                        this.item[index1].SetDefaults(1982, false);
                        ++index1;
                    }

                    if (Main.hardMode)
                    {
                        this.item[index1].SetDefaults(1983, false);
                        ++index1;
                    }

                    if (NPC.AnyNPCs(208))
                    {
                        this.item[index1].SetDefaults(1984, false);
                        ++index1;
                    }

                    if (Main.hardMode && NPC.downedMechBoss1 && (NPC.downedMechBoss2 && NPC.downedMechBoss3))
                    {
                        this.item[index1].SetDefaults(1985, false);
                        ++index1;
                    }

                    if (Main.hardMode && NPC.downedMechBossAny)
                    {
                        this.item[index1].SetDefaults(1986, false);
                        ++index1;
                    }

                    if (Main.hardMode && NPC.downedMartians)
                    {
                        this.item[index1].SetDefaults(2863, false);
                        var index14 = index1 + 1;
                        this.item[index14].SetDefaults(3259, false);
                        index1 = index14 + 1;
                        break;
                    }

                    break;
                case 19:
                    for (var index14 = 0; index14 < 40; ++index14)
                    {
                        if (Main.travelShop[index14] != 0)
                        {
                            this.item[index1].netDefaults(Main.travelShop[index14]);
                            ++index1;
                        }
                    }

                    break;
                case 20:
                    if (Main.moonPhase % 2 == 0)
                        this.item[index1].SetDefaults(3001, false);
                    else
                        this.item[index1].SetDefaults(28, false);
                    var index134 = index1 + 1;
                    if (!Main.dayTime || Main.moonPhase == 0)
                        this.item[index134].SetDefaults(3002, false);
                    else
                        this.item[index134].SetDefaults(282, false);
                    var index135 = index134 + 1;
                    if (Main.time % 60.0 * 60.0 * 6.0 <= 10800.0)
                        this.item[index135].SetDefaults(3004, false);
                    else
                        this.item[index135].SetDefaults(8, false);
                    var index136 = index135 + 1;
                    if (Main.moonPhase == 0 || Main.moonPhase == 1 || (Main.moonPhase == 4 || Main.moonPhase == 5))
                        this.item[index136].SetDefaults(3003, false);
                    else
                        this.item[index136].SetDefaults(40, false);
                    var index137 = index136 + 1;
                    if (Main.moonPhase % 4 == 0)
                        this.item[index137].SetDefaults(3310, false);
                    else if (Main.moonPhase % 4 == 1)
                        this.item[index137].SetDefaults(3313, false);
                    else if (Main.moonPhase % 4 == 2)
                        this.item[index137].SetDefaults(3312, false);
                    else
                        this.item[index137].SetDefaults(3311, false);
                    var index138 = index137 + 1;
                    this.item[index138].SetDefaults(166, false);
                    var index139 = index138 + 1;
                    this.item[index139].SetDefaults(965, false);
                    index1 = index139 + 1;
                    if (Main.hardMode)
                    {
                        if (Main.moonPhase < 4)
                            this.item[index1].SetDefaults(3316, false);
                        else
                            this.item[index1].SetDefaults(3315, false);
                        var index14 = index1 + 1;
                        this.item[index14].SetDefaults(3334, false);
                        index1 = index14 + 1;
                        if (Main.bloodMoon)
                        {
                            this.item[index1].SetDefaults(3258, false);
                            ++index1;
                        }
                    }

                    if (Main.moonPhase == 0 && !Main.dayTime)
                    {
                        this.item[index1].SetDefaults(3043, false);
                        ++index1;
                        break;
                    }

                    break;
                case 21:
                    var flag1 = Main.hardMode && NPC.downedMechBossAny;
                    var flag2 = Main.hardMode && NPC.downedGolemBoss;
                    this.item[index1].SetDefaults(353, false);
                    var index140 = index1 + 1;
                    this.item[index140].SetDefaults(3828, false);
                    this.item[index140].shopCustomPrice = !flag2
                        ? (!flag1 ? new int?(Item.buyPrice(0, 0, 25, 0)) : new int?(Item.buyPrice(0, 1, 0, 0)))
                        : new int?(Item.buyPrice(0, 4, 0, 0));
                    var index141 = index140 + 1;
                    this.item[index141].SetDefaults(3816, false);
                    var index142 = index141 + 1;
                    this.item[index142].SetDefaults(3813, false);
                    this.item[index142].shopCustomPrice = new int?(75);
                    this.item[index142].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                    var num35 = index142 + 1;
                    var index143 = 10;
                    this.item[index143].SetDefaults(3818, false);
                    this.item[index143].shopCustomPrice = new int?(5);
                    this.item[index143].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                    var index144 = index143 + 1;
                    this.item[index144].SetDefaults(3824, false);
                    this.item[index144].shopCustomPrice = new int?(5);
                    this.item[index144].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                    var index145 = index144 + 1;
                    this.item[index145].SetDefaults(3832, false);
                    this.item[index145].shopCustomPrice = new int?(5);
                    this.item[index145].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                    index1 = index145 + 1;
                    this.item[index1].SetDefaults(3829, false);
                    this.item[index1].shopCustomPrice = new int?(5);
                    this.item[index1].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                    if (flag1)
                    {
                        var index14 = 20;
                        this.item[index14].SetDefaults(3819, false);
                        this.item[index14].shopCustomPrice = new int?(25);
                        this.item[index14].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index15 = index14 + 1;
                        this.item[index15].SetDefaults(3825, false);
                        this.item[index15].shopCustomPrice = new int?(25);
                        this.item[index15].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index16 = index15 + 1;
                        this.item[index16].SetDefaults(3833, false);
                        this.item[index16].shopCustomPrice = new int?(25);
                        this.item[index16].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        index1 = index16 + 1;
                        this.item[index1].SetDefaults(3830, false);
                        this.item[index1].shopCustomPrice = new int?(25);
                        this.item[index1].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                    }

                    if (flag2)
                    {
                        var index14 = 30;
                        this.item[index14].SetDefaults(3820, false);
                        this.item[index14].shopCustomPrice = new int?(100);
                        this.item[index14].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index15 = index14 + 1;
                        this.item[index15].SetDefaults(3826, false);
                        this.item[index15].shopCustomPrice = new int?(100);
                        this.item[index15].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index16 = index15 + 1;
                        this.item[index16].SetDefaults(3834, false);
                        this.item[index16].shopCustomPrice = new int?(100);
                        this.item[index16].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        index1 = index16 + 1;
                        this.item[index1].SetDefaults(3831, false);
                        this.item[index1].shopCustomPrice = new int?(100);
                        this.item[index1].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                    }

                    if (flag1)
                    {
                        var index14 = 4;
                        this.item[index14].SetDefaults(3800, false);
                        this.item[index14].shopCustomPrice = new int?(25);
                        this.item[index14].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index15 = index14 + 1;
                        this.item[index15].SetDefaults(3801, false);
                        this.item[index15].shopCustomPrice = new int?(25);
                        this.item[index15].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index16 = index15 + 1;
                        this.item[index16].SetDefaults(3802, false);
                        this.item[index16].shopCustomPrice = new int?(25);
                        this.item[index16].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        num35 = index16 + 1;
                        var index32 = 14;
                        this.item[index32].SetDefaults(3797, false);
                        this.item[index32].shopCustomPrice = new int?(25);
                        this.item[index32].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index33 = index32 + 1;
                        this.item[index33].SetDefaults(3798, false);
                        this.item[index33].shopCustomPrice = new int?(25);
                        this.item[index33].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index105 = index33 + 1;
                        this.item[index105].SetDefaults(3799, false);
                        this.item[index105].shopCustomPrice = new int?(25);
                        this.item[index105].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        num35 = index105 + 1;
                        var index106 = 24;
                        this.item[index106].SetDefaults(3803, false);
                        this.item[index106].shopCustomPrice = new int?(25);
                        this.item[index106].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index107 = index106 + 1;
                        this.item[index107].SetDefaults(3804, false);
                        this.item[index107].shopCustomPrice = new int?(25);
                        this.item[index107].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index108 = index107 + 1;
                        this.item[index108].SetDefaults(3805, false);
                        this.item[index108].shopCustomPrice = new int?(25);
                        this.item[index108].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        num35 = index108 + 1;
                        var index109 = 34;
                        this.item[index109].SetDefaults(3806, false);
                        this.item[index109].shopCustomPrice = new int?(25);
                        this.item[index109].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index110 = index109 + 1;
                        this.item[index110].SetDefaults(3807, false);
                        this.item[index110].shopCustomPrice = new int?(25);
                        this.item[index110].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index146 = index110 + 1;
                        this.item[index146].SetDefaults(3808, false);
                        this.item[index146].shopCustomPrice = new int?(25);
                        this.item[index146].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        index1 = index146 + 1;
                    }

                    if (flag2)
                    {
                        var index14 = 7;
                        this.item[index14].SetDefaults(3871, false);
                        this.item[index14].shopCustomPrice = new int?(75);
                        this.item[index14].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index15 = index14 + 1;
                        this.item[index15].SetDefaults(3872, false);
                        this.item[index15].shopCustomPrice = new int?(75);
                        this.item[index15].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index16 = index15 + 1;
                        this.item[index16].SetDefaults(3873, false);
                        this.item[index16].shopCustomPrice = new int?(75);
                        this.item[index16].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        num35 = index16 + 1;
                        var index32 = 17;
                        this.item[index32].SetDefaults(3874, false);
                        this.item[index32].shopCustomPrice = new int?(75);
                        this.item[index32].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index33 = index32 + 1;
                        this.item[index33].SetDefaults(3875, false);
                        this.item[index33].shopCustomPrice = new int?(75);
                        this.item[index33].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index105 = index33 + 1;
                        this.item[index105].SetDefaults(3876, false);
                        this.item[index105].shopCustomPrice = new int?(75);
                        this.item[index105].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        num35 = index105 + 1;
                        var index106 = 27;
                        this.item[index106].SetDefaults(3877, false);
                        this.item[index106].shopCustomPrice = new int?(75);
                        this.item[index106].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index107 = index106 + 1;
                        this.item[index107].SetDefaults(3878, false);
                        this.item[index107].shopCustomPrice = new int?(75);
                        this.item[index107].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index108 = index107 + 1;
                        this.item[index108].SetDefaults(3879, false);
                        this.item[index108].shopCustomPrice = new int?(75);
                        this.item[index108].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        num35 = index108 + 1;
                        var index109 = 37;
                        this.item[index109].SetDefaults(3880, false);
                        this.item[index109].shopCustomPrice = new int?(75);
                        this.item[index109].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index110 = index109 + 1;
                        this.item[index110].SetDefaults(3881, false);
                        this.item[index110].shopCustomPrice = new int?(75);
                        this.item[index110].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        var index146 = index110 + 1;
                        this.item[index146].SetDefaults(3882, false);
                        this.item[index146].shopCustomPrice = new int?(75);
                        this.item[index146].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
                        index1 = index146 + 1;
                        break;
                    }

                    break;
            }

            if (!Main.player[Main.myPlayer].discount)
                return;
            for (var index14 = 0; index14 < index1; ++index14)
                this.item[index14].value = (int) ((double) this.item[index14].value * 0.800000011920929);
        }

        public static void UpdateChestFrames()
        {
            var flagArray = new bool[1000];
            for (var index = 0; index < (int) byte.MaxValue; ++index)
            {
                if (Main.player[index].active && Main.player[index].chest >= 0 && Main.player[index].chest < 1000)
                    flagArray[Main.player[index].chest] = true;
            }

            for (var index = 0; index < 1000; ++index)
            {
                var chest = Main.chest[index];
                if (chest != null)
                {
                    if (flagArray[index])
                        ++chest.frameCounter;
                    else
                        --chest.frameCounter;
                    if (chest.frameCounter < 0)
                        chest.frameCounter = 0;
                    if (chest.frameCounter > 10)
                        chest.frameCounter = 10;
                    chest.frame = chest.frameCounter != 0 ? (chest.frameCounter != 10 ? 1 : 2) : 0;
                }
            }
        }
    }
}