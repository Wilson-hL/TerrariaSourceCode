﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Map.MapHelper
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Ionic.Zlib;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.IO;
using Terraria.IO;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria.Map
{
    public static class MapHelper
    {
        public static int maxUpdateTile = 1000;
        public static int numUpdateTile = 0;
        public static short[] updateTileX = new short[MapHelper.maxUpdateTile];
        public static short[] updateTileY = new short[MapHelper.maxUpdateTile];
        private static bool saveLock = false;
        private static object padlock = new object();
        public const int drawLoopMilliseconds = 5;
        private const int HeaderEmpty = 0;
        private const int HeaderTile = 1;
        private const int HeaderWall = 2;
        private const int HeaderWater = 3;
        private const int HeaderLava = 4;
        private const int HeaderHoney = 5;
        private const int HeaderHeavenAndHell = 6;
        private const int HeaderBackground = 7;
        private const int maxTileOptions = 12;
        private const int maxWallOptions = 2;
        private const int maxLiquidTypes = 3;
        private const int maxSkyGradients = 256;
        private const int maxDirtGradients = 256;
        private const int maxRockGradients = 256;
        public static int[] tileOptionCounts;
        public static int[] wallOptionCounts;
        public static ushort[] tileLookup;
        public static ushort[] wallLookup;
        private static ushort tilePosition;
        private static ushort wallPosition;
        private static ushort liquidPosition;
        private static ushort skyPosition;
        private static ushort dirtPosition;
        private static ushort rockPosition;
        private static ushort hellPosition;
        private static Color[] colorLookup;
        private static ushort[] snowTypes;
        private static ushort wallRangeStart;
        private static ushort wallRangeEnd;

        public static void Initialize()
        {
            var colorArray1 = new Color[470][];
            for (var index = 0; index < 470; ++index)
                colorArray1[index] = new Color[12];
            var color1 = new Color(151, 107, 75);
            colorArray1[0][0] = color1;
            colorArray1[5][0] = color1;
            colorArray1[30][0] = color1;
            colorArray1[191][0] = color1;
            colorArray1[272][0] = new Color(121, 119, 101);
            color1 = new Color(128, 128, 128);
            colorArray1[1][0] = color1;
            colorArray1[38][0] = color1;
            colorArray1[48][0] = color1;
            colorArray1[130][0] = color1;
            colorArray1[138][0] = color1;
            colorArray1[273][0] = color1;
            colorArray1[283][0] = color1;
            colorArray1[2][0] = new Color(28, 216, 94);
            color1 = new Color(26, 196, 84);
            colorArray1[3][0] = color1;
            colorArray1[192][0] = color1;
            colorArray1[73][0] = new Color(27, 197, 109);
            colorArray1[52][0] = new Color(23, 177, 76);
            colorArray1[353][0] = new Color(28, 216, 94);
            colorArray1[20][0] = new Color(163, 116, 81);
            colorArray1[6][0] = new Color(140, 101, 80);
            color1 = new Color(150, 67, 22);
            colorArray1[7][0] = color1;
            colorArray1[47][0] = color1;
            colorArray1[284][0] = color1;
            color1 = new Color(185, 164, 23);
            colorArray1[8][0] = color1;
            colorArray1[45][0] = color1;
            color1 = new Color(185, 194, 195);
            colorArray1[9][0] = color1;
            colorArray1[46][0] = color1;
            color1 = new Color(98, 95, 167);
            colorArray1[22][0] = color1;
            colorArray1[140][0] = color1;
            colorArray1[23][0] = new Color(141, 137, 223);
            colorArray1[24][0] = new Color(122, 116, 218);
            colorArray1[25][0] = new Color(109, 90, 128);
            colorArray1[37][0] = new Color(104, 86, 84);
            colorArray1[39][0] = new Color(181, 62, 59);
            colorArray1[40][0] = new Color(146, 81, 68);
            colorArray1[41][0] = new Color(66, 84, 109);
            colorArray1[43][0] = new Color(84, 100, 63);
            colorArray1[44][0] = new Color(107, 68, 99);
            colorArray1[53][0] = new Color(186, 168, 84);
            color1 = new Color(190, 171, 94);
            colorArray1[151][0] = color1;
            colorArray1[154][0] = color1;
            colorArray1[274][0] = color1;
            colorArray1[328][0] = new Color(200, 246, 254);
            colorArray1[329][0] = new Color(15, 15, 15);
            colorArray1[54][0] = new Color(200, 246, 254);
            colorArray1[56][0] = new Color(43, 40, 84);
            colorArray1[75][0] = new Color(26, 26, 26);
            colorArray1[57][0] = new Color(68, 68, 76);
            color1 = new Color(142, 66, 66);
            colorArray1[58][0] = color1;
            colorArray1[76][0] = color1;
            color1 = new Color(92, 68, 73);
            colorArray1[59][0] = color1;
            colorArray1[120][0] = color1;
            colorArray1[60][0] = new Color(143, 215, 29);
            colorArray1[61][0] = new Color(135, 196, 26);
            colorArray1[74][0] = new Color(96, 197, 27);
            colorArray1[62][0] = new Color(121, 176, 24);
            colorArray1[233][0] = new Color(107, 182, 29);
            colorArray1[63][0] = new Color(110, 140, 182);
            colorArray1[64][0] = new Color(196, 96, 114);
            colorArray1[65][0] = new Color(56, 150, 97);
            colorArray1[66][0] = new Color(160, 118, 58);
            colorArray1[67][0] = new Color(140, 58, 166);
            colorArray1[68][0] = new Color(125, 191, 197);
            colorArray1[70][0] = new Color(93, (int) sbyte.MaxValue, (int) byte.MaxValue);
            color1 = new Color(182, 175, 130);
            colorArray1[71][0] = color1;
            colorArray1[72][0] = color1;
            colorArray1[190][0] = color1;
            color1 = new Color(73, 120, 17);
            colorArray1[80][0] = color1;
            colorArray1[188][0] = color1;
            color1 = new Color(11, 80, 143);
            colorArray1[107][0] = color1;
            colorArray1[121][0] = color1;
            color1 = new Color(91, 169, 169);
            colorArray1[108][0] = color1;
            colorArray1[122][0] = color1;
            color1 = new Color(128, 26, 52);
            colorArray1[111][0] = color1;
            colorArray1[150][0] = color1;
            colorArray1[109][0] = new Color(78, 193, 227);
            colorArray1[110][0] = new Color(48, 186, 135);
            colorArray1[113][0] = new Color(48, 208, 234);
            colorArray1[115][0] = new Color(33, 171, 207);
            colorArray1[112][0] = new Color(103, 98, 122);
            color1 = new Color(238, 225, 218);
            colorArray1[116][0] = color1;
            colorArray1[118][0] = color1;
            colorArray1[117][0] = new Color(181, 172, 190);
            colorArray1[119][0] = new Color(107, 92, 108);
            colorArray1[123][0] = new Color(106, 107, 118);
            colorArray1[124][0] = new Color(73, 51, 36);
            colorArray1[131][0] = new Color(52, 52, 52);
            colorArray1[145][0] = new Color(192, 30, 30);
            colorArray1[146][0] = new Color(43, 192, 30);
            color1 = new Color(211, 236, 241);
            colorArray1[147][0] = color1;
            colorArray1[148][0] = color1;
            colorArray1[152][0] = new Color(128, 133, 184);
            colorArray1[153][0] = new Color(239, 141, 126);
            colorArray1[155][0] = new Color(131, 162, 161);
            colorArray1[156][0] = new Color(170, 171, 157);
            colorArray1[157][0] = new Color(104, 100, 126);
            color1 = new Color(145, 81, 85);
            colorArray1[158][0] = color1;
            colorArray1[232][0] = color1;
            colorArray1[159][0] = new Color(148, 133, 98);
            colorArray1[160][0] = new Color(200, 0, 0);
            colorArray1[160][1] = new Color(0, 200, 0);
            colorArray1[160][2] = new Color(0, 0, 200);
            colorArray1[161][0] = new Color(144, 195, 232);
            colorArray1[162][0] = new Color(184, 219, 240);
            colorArray1[163][0] = new Color(174, 145, 214);
            colorArray1[164][0] = new Color(218, 182, 204);
            colorArray1[170][0] = new Color(27, 109, 69);
            colorArray1[171][0] = new Color(33, 135, 85);
            color1 = new Color(129, 125, 93);
            colorArray1[166][0] = color1;
            colorArray1[175][0] = color1;
            colorArray1[167][0] = new Color(62, 82, 114);
            color1 = new Color(132, 157, (int) sbyte.MaxValue);
            colorArray1[168][0] = color1;
            colorArray1[176][0] = color1;
            color1 = new Color(152, 171, 198);
            colorArray1[169][0] = color1;
            colorArray1[177][0] = color1;
            colorArray1[179][0] = new Color(49, 134, 114);
            colorArray1[180][0] = new Color(126, 134, 49);
            colorArray1[181][0] = new Color(134, 59, 49);
            colorArray1[182][0] = new Color(43, 86, 140);
            colorArray1[183][0] = new Color(121, 49, 134);
            colorArray1[381][0] = new Color(254, 121, 2);
            colorArray1[189][0] = new Color(223, (int) byte.MaxValue, (int) byte.MaxValue);
            colorArray1[193][0] = new Color(56, 121, (int) byte.MaxValue);
            colorArray1[194][0] = new Color(157, 157, 107);
            colorArray1[195][0] = new Color(134, 22, 34);
            colorArray1[196][0] = new Color(147, 144, 178);
            colorArray1[197][0] = new Color(97, 200, 225);
            colorArray1[198][0] = new Color(62, 61, 52);
            colorArray1[199][0] = new Color(208, 80, 80);
            colorArray1[201][0] = new Color(203, 61, 64);
            colorArray1[205][0] = new Color(186, 50, 52);
            colorArray1[200][0] = new Color(216, 152, 144);
            colorArray1[202][0] = new Color(213, 178, 28);
            colorArray1[203][0] = new Color(128, 44, 45);
            colorArray1[204][0] = new Color(125, 55, 65);
            colorArray1[206][0] = new Color(124, 175, 201);
            colorArray1[208][0] = new Color(88, 105, 118);
            colorArray1[211][0] = new Color(191, 233, 115);
            colorArray1[213][0] = new Color(137, 120, 67);
            colorArray1[214][0] = new Color(103, 103, 103);
            colorArray1[221][0] = new Color(239, 90, 50);
            colorArray1[222][0] = new Color(231, 96, 228);
            colorArray1[223][0] = new Color(57, 85, 101);
            colorArray1[224][0] = new Color(107, 132, 139);
            colorArray1[225][0] = new Color(227, 125, 22);
            colorArray1[226][0] = new Color(141, 56, 0);
            colorArray1[229][0] = new Color((int) byte.MaxValue, 156, 12);
            colorArray1[230][0] = new Color(131, 79, 13);
            colorArray1[234][0] = new Color(53, 44, 41);
            colorArray1[235][0] = new Color(214, 184, 46);
            colorArray1[236][0] = new Color(149, 232, 87);
            colorArray1[237][0] = new Color((int) byte.MaxValue, 241, 51);
            colorArray1[238][0] = new Color(225, 128, 206);
            colorArray1[243][0] = new Color(198, 196, 170);
            colorArray1[248][0] = new Color(219, 71, 38);
            colorArray1[249][0] = new Color(235, 38, 231);
            colorArray1[250][0] = new Color(86, 85, 92);
            colorArray1[251][0] = new Color(235, 150, 23);
            colorArray1[252][0] = new Color(153, 131, 44);
            colorArray1[253][0] = new Color(57, 48, 97);
            colorArray1[254][0] = new Color(248, 158, 92);
            colorArray1[(int) byte.MaxValue][0] = new Color(107, 49, 154);
            colorArray1[256][0] = new Color(154, 148, 49);
            colorArray1[257][0] = new Color(49, 49, 154);
            colorArray1[258][0] = new Color(49, 154, 68);
            colorArray1[259][0] = new Color(154, 49, 77);
            colorArray1[260][0] = new Color(85, 89, 118);
            colorArray1[261][0] = new Color(154, 83, 49);
            colorArray1[262][0] = new Color(221, 79, (int) byte.MaxValue);
            colorArray1[263][0] = new Color(250, (int) byte.MaxValue, 79);
            colorArray1[264][0] = new Color(79, 102, (int) byte.MaxValue);
            colorArray1[265][0] = new Color(79, (int) byte.MaxValue, 89);
            colorArray1[266][0] = new Color((int) byte.MaxValue, 79, 79);
            colorArray1[267][0] = new Color(240, 240, 247);
            colorArray1[268][0] = new Color((int) byte.MaxValue, 145, 79);
            colorArray1[287][0] = new Color(79, 128, 17);
            color1 = new Color(122, 217, 232);
            colorArray1[275][0] = color1;
            colorArray1[276][0] = color1;
            colorArray1[277][0] = color1;
            colorArray1[278][0] = color1;
            colorArray1[279][0] = color1;
            colorArray1[280][0] = color1;
            colorArray1[281][0] = color1;
            colorArray1[282][0] = color1;
            colorArray1[285][0] = color1;
            colorArray1[286][0] = color1;
            colorArray1[288][0] = color1;
            colorArray1[289][0] = color1;
            colorArray1[290][0] = color1;
            colorArray1[291][0] = color1;
            colorArray1[292][0] = color1;
            colorArray1[293][0] = color1;
            colorArray1[294][0] = color1;
            colorArray1[295][0] = color1;
            colorArray1[296][0] = color1;
            colorArray1[297][0] = color1;
            colorArray1[298][0] = color1;
            colorArray1[299][0] = color1;
            colorArray1[309][0] = color1;
            colorArray1[310][0] = color1;
            colorArray1[413][0] = color1;
            colorArray1[339][0] = color1;
            colorArray1[358][0] = color1;
            colorArray1[359][0] = color1;
            colorArray1[360][0] = color1;
            colorArray1[361][0] = color1;
            colorArray1[362][0] = color1;
            colorArray1[363][0] = color1;
            colorArray1[364][0] = color1;
            colorArray1[391][0] = color1;
            colorArray1[392][0] = color1;
            colorArray1[393][0] = color1;
            colorArray1[394][0] = color1;
            colorArray1[414][0] = color1;
            colorArray1[408][0] = new Color(85, 83, 82);
            colorArray1[409][0] = new Color(85, 83, 82);
            colorArray1[415][0] = new Color(249, 75, 7);
            colorArray1[416][0] = new Color(0, 160, 170);
            colorArray1[417][0] = new Color(160, 87, 234);
            colorArray1[418][0] = new Color(22, 173, 254);
            colorArray1[311][0] = new Color(117, 61, 25);
            colorArray1[312][0] = new Color(204, 93, 73);
            colorArray1[313][0] = new Color(87, 150, 154);
            colorArray1[4][0] = new Color(253, 221, 3);
            colorArray1[4][1] = new Color(253, 221, 3);
            color1 = new Color(253, 221, 3);
            colorArray1[93][0] = color1;
            colorArray1[33][0] = color1;
            colorArray1[174][0] = color1;
            colorArray1[100][0] = color1;
            colorArray1[98][0] = color1;
            colorArray1[173][0] = color1;
            color1 = new Color(119, 105, 79);
            colorArray1[11][0] = color1;
            colorArray1[10][0] = color1;
            color1 = new Color(191, 142, 111);
            colorArray1[14][0] = color1;
            colorArray1[469][0] = color1;
            colorArray1[15][0] = color1;
            colorArray1[18][0] = color1;
            colorArray1[19][0] = color1;
            colorArray1[55][0] = color1;
            colorArray1[79][0] = color1;
            colorArray1[86][0] = color1;
            colorArray1[87][0] = color1;
            colorArray1[88][0] = color1;
            colorArray1[89][0] = color1;
            colorArray1[94][0] = color1;
            colorArray1[101][0] = color1;
            colorArray1[104][0] = color1;
            colorArray1[106][0] = color1;
            colorArray1[114][0] = color1;
            colorArray1[128][0] = color1;
            colorArray1[139][0] = color1;
            colorArray1[172][0] = color1;
            colorArray1[216][0] = color1;
            colorArray1[269][0] = color1;
            colorArray1[334][0] = color1;
            colorArray1[377][0] = color1;
            colorArray1[380][0] = color1;
            colorArray1[395][0] = color1;
            colorArray1[12][0] = new Color(174, 24, 69);
            colorArray1[13][0] = new Color(133, 213, 247);
            color1 = new Color(144, 148, 144);
            colorArray1[17][0] = color1;
            colorArray1[90][0] = color1;
            colorArray1[96][0] = color1;
            colorArray1[97][0] = color1;
            colorArray1[99][0] = color1;
            colorArray1[132][0] = color1;
            colorArray1[142][0] = color1;
            colorArray1[143][0] = color1;
            colorArray1[144][0] = color1;
            colorArray1[207][0] = color1;
            colorArray1[209][0] = color1;
            colorArray1[212][0] = color1;
            colorArray1[217][0] = color1;
            colorArray1[218][0] = color1;
            colorArray1[219][0] = color1;
            colorArray1[220][0] = color1;
            colorArray1[228][0] = color1;
            colorArray1[300][0] = color1;
            colorArray1[301][0] = color1;
            colorArray1[302][0] = color1;
            colorArray1[303][0] = color1;
            colorArray1[304][0] = color1;
            colorArray1[305][0] = color1;
            colorArray1[306][0] = color1;
            colorArray1[307][0] = color1;
            colorArray1[308][0] = color1;
            colorArray1[349][0] = new Color(144, 148, 144);
            colorArray1[105][0] = new Color(144, 148, 144);
            colorArray1[105][1] = new Color(177, 92, 31);
            colorArray1[105][2] = new Color(201, 188, 170);
            colorArray1[137][0] = new Color(144, 148, 144);
            colorArray1[137][1] = new Color(141, 56, 0);
            colorArray1[16][0] = new Color(140, 130, 116);
            colorArray1[26][0] = new Color(119, 101, 125);
            colorArray1[26][1] = new Color(214, (int) sbyte.MaxValue, 133);
            colorArray1[36][0] = new Color(230, 89, 92);
            colorArray1[28][0] = new Color(151, 79, 80);
            colorArray1[28][1] = new Color(90, 139, 140);
            colorArray1[28][2] = new Color(192, 136, 70);
            colorArray1[28][3] = new Color(203, 185, 151);
            colorArray1[28][4] = new Color(73, 56, 41);
            colorArray1[28][5] = new Color(148, 159, 67);
            colorArray1[28][6] = new Color(138, 172, 67);
            colorArray1[28][7] = new Color(226, 122, 47);
            colorArray1[28][8] = new Color(198, 87, 93);
            colorArray1[29][0] = new Color(175, 105, 128);
            colorArray1[51][0] = new Color(192, 202, 203);
            colorArray1[31][0] = new Color(141, 120, 168);
            colorArray1[31][1] = new Color(212, 105, 105);
            colorArray1[32][0] = new Color(151, 135, 183);
            colorArray1[42][0] = new Color(251, 235, (int) sbyte.MaxValue);
            colorArray1[50][0] = new Color(170, 48, 114);
            colorArray1[85][0] = new Color(192, 192, 192);
            colorArray1[69][0] = new Color(190, 150, 92);
            colorArray1[77][0] = new Color(238, 85, 70);
            colorArray1[81][0] = new Color(245, 133, 191);
            colorArray1[78][0] = new Color(121, 110, 97);
            colorArray1[141][0] = new Color(192, 59, 59);
            colorArray1[129][0] = new Color((int) byte.MaxValue, 117, 224);
            colorArray1[126][0] = new Color(159, 209, 229);
            colorArray1[125][0] = new Color(141, 175, (int) byte.MaxValue);
            colorArray1[103][0] = new Color(141, 98, 77);
            colorArray1[95][0] = new Color((int) byte.MaxValue, 162, 31);
            colorArray1[92][0] = new Color(213, 229, 237);
            colorArray1[91][0] = new Color(13, 88, 130);
            colorArray1[215][0] = new Color(254, 121, 2);
            colorArray1[316][0] = new Color(157, 176, 226);
            colorArray1[317][0] = new Color(118, 227, 129);
            colorArray1[318][0] = new Color(227, 118, 215);
            colorArray1[319][0] = new Color(96, 68, 48);
            colorArray1[320][0] = new Color(203, 185, 151);
            colorArray1[321][0] = new Color(96, 77, 64);
            colorArray1[322][0] = new Color(198, 170, 104);
            colorArray1[149][0] = new Color(220, 50, 50);
            colorArray1[149][1] = new Color(0, 220, 50);
            colorArray1[149][2] = new Color(50, 50, 220);
            colorArray1[133][0] = new Color(231, 53, 56);
            colorArray1[133][1] = new Color(192, 189, 221);
            colorArray1[134][0] = new Color(166, 187, 153);
            colorArray1[134][1] = new Color(241, 129, 249);
            colorArray1[102][0] = new Color(229, 212, 73);
            colorArray1[49][0] = new Color(89, 201, (int) byte.MaxValue);
            colorArray1[35][0] = new Color(226, 145, 30);
            colorArray1[34][0] = new Color(235, 166, 135);
            colorArray1[136][0] = new Color(213, 203, 204);
            colorArray1[231][0] = new Color(224, 194, 101);
            colorArray1[239][0] = new Color(224, 194, 101);
            colorArray1[240][0] = new Color(120, 85, 60);
            colorArray1[240][1] = new Color(99, 50, 30);
            colorArray1[240][2] = new Color(153, 153, 117);
            colorArray1[240][3] = new Color(112, 84, 56);
            colorArray1[240][4] = new Color(234, 231, 226);
            colorArray1[241][0] = new Color(77, 74, 72);
            colorArray1[244][0] = new Color(200, 245, 253);
            color1 = new Color(99, 50, 30);
            colorArray1[242][0] = color1;
            colorArray1[245][0] = color1;
            colorArray1[246][0] = color1;
            colorArray1[242][1] = new Color(185, 142, 97);
            colorArray1[247][0] = new Color(140, 150, 150);
            colorArray1[271][0] = new Color(107, 250, (int) byte.MaxValue);
            colorArray1[270][0] = new Color(187, (int) byte.MaxValue, 107);
            colorArray1[314][0] = new Color(181, 164, 125);
            colorArray1[324][0] = new Color(228, 213, 173);
            colorArray1[351][0] = new Color(31, 31, 31);
            colorArray1[424][0] = new Color(146, 155, 187);
            colorArray1[429][0] = new Color(220, 220, 220);
            colorArray1[445][0] = new Color(240, 240, 240);
            colorArray1[21][0] = new Color(174, 129, 92);
            colorArray1[21][1] = new Color(233, 207, 94);
            colorArray1[21][2] = new Color(137, 128, 200);
            colorArray1[21][3] = new Color(160, 160, 160);
            colorArray1[21][4] = new Color(106, 210, (int) byte.MaxValue);
            colorArray1[441][0] = colorArray1[21][0];
            colorArray1[441][1] = colorArray1[21][1];
            colorArray1[441][2] = colorArray1[21][2];
            colorArray1[441][3] = colorArray1[21][3];
            colorArray1[441][4] = colorArray1[21][4];
            colorArray1[27][0] = new Color(54, 154, 54);
            colorArray1[27][1] = new Color(226, 196, 49);
            color1 = new Color(246, 197, 26);
            colorArray1[82][0] = color1;
            colorArray1[83][0] = color1;
            colorArray1[84][0] = color1;
            color1 = new Color(76, 150, 216);
            colorArray1[82][1] = color1;
            colorArray1[83][1] = color1;
            colorArray1[84][1] = color1;
            color1 = new Color(185, 214, 42);
            colorArray1[82][2] = color1;
            colorArray1[83][2] = color1;
            colorArray1[84][2] = color1;
            color1 = new Color(167, 203, 37);
            colorArray1[82][3] = color1;
            colorArray1[83][3] = color1;
            colorArray1[84][3] = color1;
            color1 = new Color(72, 145, 125);
            colorArray1[82][4] = color1;
            colorArray1[83][4] = color1;
            colorArray1[84][4] = color1;
            color1 = new Color(177, 69, 49);
            colorArray1[82][5] = color1;
            colorArray1[83][5] = color1;
            colorArray1[84][5] = color1;
            color1 = new Color(40, 152, 240);
            colorArray1[82][6] = color1;
            colorArray1[83][6] = color1;
            colorArray1[84][6] = color1;
            colorArray1[165][0] = new Color(115, 173, 229);
            colorArray1[165][1] = new Color(100, 100, 100);
            colorArray1[165][2] = new Color(152, 152, 152);
            colorArray1[165][3] = new Color(227, 125, 22);
            colorArray1[178][0] = new Color(208, 94, 201);
            colorArray1[178][1] = new Color(233, 146, 69);
            colorArray1[178][2] = new Color(71, 146, 251);
            colorArray1[178][3] = new Color(60, 226, 133);
            colorArray1[178][4] = new Color(250, 30, 71);
            colorArray1[178][5] = new Color(166, 176, 204);
            colorArray1[178][6] = new Color((int) byte.MaxValue, 217, 120);
            colorArray1[184][0] = new Color(29, 106, 88);
            colorArray1[184][1] = new Color(94, 100, 36);
            colorArray1[184][2] = new Color(96, 44, 40);
            colorArray1[184][3] = new Color(34, 63, 102);
            colorArray1[184][4] = new Color(79, 35, 95);
            colorArray1[184][5] = new Color(253, 62, 3);
            color1 = new Color(99, 99, 99);
            colorArray1[185][0] = color1;
            colorArray1[186][0] = color1;
            colorArray1[187][0] = color1;
            color1 = new Color(114, 81, 56);
            colorArray1[185][1] = color1;
            colorArray1[186][1] = color1;
            colorArray1[187][1] = color1;
            color1 = new Color(133, 133, 101);
            colorArray1[185][2] = color1;
            colorArray1[186][2] = color1;
            colorArray1[187][2] = color1;
            color1 = new Color(151, 200, 211);
            colorArray1[185][3] = color1;
            colorArray1[186][3] = color1;
            colorArray1[187][3] = color1;
            color1 = new Color(177, 183, 161);
            colorArray1[185][4] = color1;
            colorArray1[186][4] = color1;
            colorArray1[187][4] = color1;
            color1 = new Color(134, 114, 38);
            colorArray1[185][5] = color1;
            colorArray1[186][5] = color1;
            colorArray1[187][5] = color1;
            color1 = new Color(82, 62, 66);
            colorArray1[185][6] = color1;
            colorArray1[186][6] = color1;
            colorArray1[187][6] = color1;
            color1 = new Color(143, 117, 121);
            colorArray1[185][7] = color1;
            colorArray1[186][7] = color1;
            colorArray1[187][7] = color1;
            color1 = new Color(177, 92, 31);
            colorArray1[185][8] = color1;
            colorArray1[186][8] = color1;
            colorArray1[187][8] = color1;
            color1 = new Color(85, 73, 87);
            colorArray1[185][9] = color1;
            colorArray1[186][9] = color1;
            colorArray1[187][9] = color1;
            colorArray1[227][0] = new Color(74, 197, 155);
            colorArray1[227][1] = new Color(54, 153, 88);
            colorArray1[227][2] = new Color(63, 126, 207);
            colorArray1[227][3] = new Color(240, 180, 4);
            colorArray1[227][4] = new Color(45, 68, 168);
            colorArray1[227][5] = new Color(61, 92, 0);
            colorArray1[227][6] = new Color(216, 112, 152);
            colorArray1[227][7] = new Color(200, 40, 24);
            colorArray1[227][8] = new Color(113, 45, 133);
            colorArray1[227][9] = new Color(235, 137, 2);
            colorArray1[227][10] = new Color(41, 152, 135);
            colorArray1[227][11] = new Color(198, 19, 78);
            colorArray1[373][0] = new Color(9, 61, 191);
            colorArray1[374][0] = new Color(253, 32, 3);
            colorArray1[375][0] = new Color((int) byte.MaxValue, 156, 12);
            colorArray1[461][0] = new Color((int) byte.MaxValue, 222, 100);
            colorArray1[323][0] = new Color(182, 141, 86);
            colorArray1[325][0] = new Color(129, 125, 93);
            colorArray1[326][0] = new Color(9, 61, 191);
            colorArray1[327][0] = new Color(253, 32, 3);
            colorArray1[330][0] = new Color(226, 118, 76);
            colorArray1[331][0] = new Color(161, 172, 173);
            colorArray1[332][0] = new Color(204, 181, 72);
            colorArray1[333][0] = new Color(190, 190, 178);
            colorArray1[335][0] = new Color(217, 174, 137);
            colorArray1[336][0] = new Color(253, 62, 3);
            colorArray1[337][0] = new Color(144, 148, 144);
            colorArray1[338][0] = new Color(85, (int) byte.MaxValue, 160);
            colorArray1[315][0] = new Color(235, 114, 80);
            colorArray1[340][0] = new Color(96, 248, 2);
            colorArray1[341][0] = new Color(105, 74, 202);
            colorArray1[342][0] = new Color(29, 240, (int) byte.MaxValue);
            colorArray1[343][0] = new Color(254, 202, 80);
            colorArray1[344][0] = new Color(131, 252, 245);
            colorArray1[345][0] = new Color((int) byte.MaxValue, 156, 12);
            colorArray1[346][0] = new Color(149, 212, 89);
            colorArray1[347][0] = new Color(236, 74, 79);
            colorArray1[348][0] = new Color(44, 26, 233);
            colorArray1[350][0] = new Color(55, 97, 155);
            colorArray1[352][0] = new Color(238, 97, 94);
            colorArray1[354][0] = new Color(141, 107, 89);
            colorArray1[355][0] = new Color(141, 107, 89);
            colorArray1[463][0] = new Color(155, 214, 240);
            colorArray1[464][0] = new Color(233, 183, 128);
            colorArray1[465][0] = new Color(51, 84, 195);
            colorArray1[466][0] = new Color(205, 153, 73);
            colorArray1[356][0] = new Color(233, 203, 24);
            colorArray1[357][0] = new Color(168, 178, 204);
            colorArray1[367][0] = new Color(168, 178, 204);
            colorArray1[365][0] = new Color(146, 136, 205);
            colorArray1[366][0] = new Color(223, 232, 233);
            colorArray1[368][0] = new Color(50, 46, 104);
            colorArray1[369][0] = new Color(50, 46, 104);
            colorArray1[370][0] = new Color((int) sbyte.MaxValue, 116, 194);
            colorArray1[372][0] = new Color(252, 128, 201);
            colorArray1[371][0] = new Color(249, 101, 189);
            colorArray1[376][0] = new Color(160, 120, 92);
            colorArray1[378][0] = new Color(160, 120, 100);
            colorArray1[379][0] = new Color(251, 209, 240);
            colorArray1[382][0] = new Color(28, 216, 94);
            colorArray1[383][0] = new Color(221, 136, 144);
            colorArray1[384][0] = new Color(131, 206, 12);
            colorArray1[385][0] = new Color(87, 21, 144);
            colorArray1[386][0] = new Color((int) sbyte.MaxValue, 92, 69);
            colorArray1[387][0] = new Color((int) sbyte.MaxValue, 92, 69);
            colorArray1[388][0] = new Color((int) sbyte.MaxValue, 92, 69);
            colorArray1[389][0] = new Color((int) sbyte.MaxValue, 92, 69);
            colorArray1[390][0] = new Color(253, 32, 3);
            colorArray1[397][0] = new Color(212, 192, 100);
            colorArray1[396][0] = new Color(198, 124, 78);
            colorArray1[398][0] = new Color(100, 82, 126);
            colorArray1[399][0] = new Color(77, 76, 66);
            colorArray1[400][0] = new Color(96, 68, 117);
            colorArray1[401][0] = new Color(68, 60, 51);
            colorArray1[402][0] = new Color(174, 168, 186);
            colorArray1[403][0] = new Color(205, 152, 186);
            colorArray1[404][0] = new Color(140, 84, 60);
            colorArray1[405][0] = new Color(140, 140, 140);
            colorArray1[406][0] = new Color(120, 120, 120);
            colorArray1[407][0] = new Color((int) byte.MaxValue, 227, 132);
            colorArray1[411][0] = new Color(227, 46, 46);
            colorArray1[421][0] = new Color(65, 75, 90);
            colorArray1[422][0] = new Color(65, 75, 90);
            colorArray1[425][0] = new Color(146, 155, 187);
            colorArray1[426][0] = new Color(168, 38, 47);
            colorArray1[430][0] = new Color(39, 168, 96);
            colorArray1[431][0] = new Color(39, 94, 168);
            colorArray1[432][0] = new Color(242, 221, 100);
            colorArray1[433][0] = new Color(224, 100, 242);
            colorArray1[434][0] = new Color(197, 193, 216);
            colorArray1[427][0] = new Color(183, 53, 62);
            colorArray1[435][0] = new Color(54, 183, 111);
            colorArray1[436][0] = new Color(54, 109, 183);
            colorArray1[437][0] = new Color((int) byte.MaxValue, 236, 115);
            colorArray1[438][0] = new Color(239, 115, (int) byte.MaxValue);
            colorArray1[439][0] = new Color(212, 208, 231);
            colorArray1[440][0] = new Color(238, 51, 53);
            colorArray1[440][1] = new Color(13, 107, 216);
            colorArray1[440][2] = new Color(33, 184, 115);
            colorArray1[440][3] = new Color((int) byte.MaxValue, 221, 62);
            colorArray1[440][4] = new Color(165, 0, 236);
            colorArray1[440][5] = new Color(223, 230, 238);
            colorArray1[440][6] = new Color(207, 101, 0);
            colorArray1[419][0] = new Color(88, 95, 114);
            colorArray1[419][1] = new Color(214, 225, 236);
            colorArray1[419][2] = new Color(25, 131, 205);
            colorArray1[423][0] = new Color(245, 197, 1);
            colorArray1[423][1] = new Color(185, 0, 224);
            colorArray1[423][2] = new Color(58, 240, 111);
            colorArray1[423][3] = new Color(50, 107, 197);
            colorArray1[423][4] = new Color(253, 91, 3);
            colorArray1[423][5] = new Color(254, 194, 20);
            colorArray1[423][6] = new Color(174, 195, 215);
            colorArray1[420][0] = new Color(99, (int) byte.MaxValue, 107);
            colorArray1[420][1] = new Color(99, (int) byte.MaxValue, 107);
            colorArray1[420][4] = new Color(99, (int) byte.MaxValue, 107);
            colorArray1[420][2] = new Color(218, 2, 5);
            colorArray1[420][3] = new Color(218, 2, 5);
            colorArray1[420][5] = new Color(218, 2, 5);
            colorArray1[410][0] = new Color(75, 139, 166);
            colorArray1[412][0] = new Color(75, 139, 166);
            colorArray1[443][0] = new Color(144, 148, 144);
            colorArray1[442][0] = new Color(3, 144, 201);
            colorArray1[444][0] = new Color(191, 176, 124);
            colorArray1[446][0] = new Color((int) byte.MaxValue, 66, 152);
            colorArray1[447][0] = new Color(179, 132, (int) byte.MaxValue);
            colorArray1[448][0] = new Color(0, 206, 180);
            colorArray1[449][0] = new Color(91, 186, 240);
            colorArray1[450][0] = new Color(92, 240, 91);
            colorArray1[451][0] = new Color(240, 91, 147);
            colorArray1[452][0] = new Color((int) byte.MaxValue, 150, 181);
            colorArray1[453][0] = new Color(179, 132, (int) byte.MaxValue);
            colorArray1[453][1] = new Color(0, 206, 180);
            colorArray1[453][2] = new Color((int) byte.MaxValue, 66, 152);
            colorArray1[454][0] = new Color(174, 16, 176);
            colorArray1[455][0] = new Color(48, 225, 110);
            colorArray1[456][0] = new Color(179, 132, (int) byte.MaxValue);
            colorArray1[457][0] = new Color(150, 164, 206);
            colorArray1[457][1] = new Color((int) byte.MaxValue, 132, 184);
            colorArray1[457][2] = new Color(74, (int) byte.MaxValue, 232);
            colorArray1[457][3] = new Color(215, 159, (int) byte.MaxValue);
            colorArray1[457][4] = new Color(229, 219, 234);
            colorArray1[458][0] = new Color(211, 198, 111);
            colorArray1[459][0] = new Color(190, 223, 232);
            colorArray1[460][0] = new Color(141, 163, 181);
            colorArray1[462][0] = new Color(231, 178, 28);
            colorArray1[467][0] = new Color(129, 56, 121);
            colorArray1[467][1] = new Color((int) byte.MaxValue, 249, 59);
            colorArray1[468][0] = colorArray1[467][0];
            colorArray1[468][1] = colorArray1[467][1];
            var colorArray2 = new Color[3] {new Color(9, 61, 191), new Color(253, 32, 3), new Color(254, 194, 20)};
            var colorArray3 = new Color[231][];
            for (var index = 0; index < 231; ++index)
                colorArray3[index] = new Color[2];
            colorArray3[158][0] = new Color(107, 49, 154);
            colorArray3[163][0] = new Color(154, 148, 49);
            colorArray3[162][0] = new Color(49, 49, 154);
            colorArray3[160][0] = new Color(49, 154, 68);
            colorArray3[161][0] = new Color(154, 49, 77);
            colorArray3[159][0] = new Color(85, 89, 118);
            colorArray3[157][0] = new Color(154, 83, 49);
            colorArray3[154][0] = new Color(221, 79, (int) byte.MaxValue);
            colorArray3[166][0] = new Color(250, (int) byte.MaxValue, 79);
            colorArray3[165][0] = new Color(79, 102, (int) byte.MaxValue);
            colorArray3[156][0] = new Color(79, (int) byte.MaxValue, 89);
            colorArray3[164][0] = new Color((int) byte.MaxValue, 79, 79);
            colorArray3[155][0] = new Color(240, 240, 247);
            colorArray3[153][0] = new Color((int) byte.MaxValue, 145, 79);
            colorArray3[169][0] = new Color(5, 5, 5);
            colorArray3[224][0] = new Color(57, 55, 52);
            colorArray3[225][0] = new Color(68, 68, 68);
            colorArray3[226][0] = new Color(148, 138, 74);
            colorArray3[227][0] = new Color(95, 137, 191);
            colorArray3[170][0] = new Color(59, 39, 22);
            colorArray3[171][0] = new Color(59, 39, 22);
            color1 = new Color(52, 52, 52);
            colorArray3[1][0] = color1;
            colorArray3[53][0] = color1;
            colorArray3[52][0] = color1;
            colorArray3[51][0] = color1;
            colorArray3[50][0] = color1;
            colorArray3[49][0] = color1;
            colorArray3[48][0] = color1;
            colorArray3[44][0] = color1;
            colorArray3[5][0] = color1;
            color1 = new Color(88, 61, 46);
            colorArray3[2][0] = color1;
            colorArray3[16][0] = color1;
            colorArray3[59][0] = color1;
            colorArray3[3][0] = new Color(61, 58, 78);
            colorArray3[4][0] = new Color(73, 51, 36);
            colorArray3[6][0] = new Color(91, 30, 30);
            color1 = new Color(27, 31, 42);
            colorArray3[7][0] = color1;
            colorArray3[17][0] = color1;
            color1 = new Color(32, 40, 45);
            colorArray3[94][0] = color1;
            colorArray3[100][0] = color1;
            color1 = new Color(44, 41, 50);
            colorArray3[95][0] = color1;
            colorArray3[101][0] = color1;
            color1 = new Color(31, 39, 26);
            colorArray3[8][0] = color1;
            colorArray3[18][0] = color1;
            color1 = new Color(36, 45, 44);
            colorArray3[98][0] = color1;
            colorArray3[104][0] = color1;
            color1 = new Color(38, 49, 50);
            colorArray3[99][0] = color1;
            colorArray3[105][0] = color1;
            color1 = new Color(41, 28, 36);
            colorArray3[9][0] = color1;
            colorArray3[19][0] = color1;
            color1 = new Color(72, 50, 77);
            colorArray3[96][0] = color1;
            colorArray3[102][0] = color1;
            color1 = new Color(78, 50, 69);
            colorArray3[97][0] = color1;
            colorArray3[103][0] = color1;
            colorArray3[10][0] = new Color(74, 62, 12);
            colorArray3[11][0] = new Color(46, 56, 59);
            colorArray3[12][0] = new Color(75, 32, 11);
            colorArray3[13][0] = new Color(67, 37, 37);
            color1 = new Color(15, 15, 15);
            colorArray3[14][0] = color1;
            colorArray3[20][0] = color1;
            colorArray3[15][0] = new Color(52, 43, 45);
            colorArray3[22][0] = new Color(113, 99, 99);
            colorArray3[23][0] = new Color(38, 38, 43);
            colorArray3[24][0] = new Color(53, 39, 41);
            colorArray3[25][0] = new Color(11, 35, 62);
            colorArray3[26][0] = new Color(21, 63, 70);
            colorArray3[27][0] = new Color(88, 61, 46);
            colorArray3[27][1] = new Color(52, 52, 52);
            colorArray3[28][0] = new Color(81, 84, 101);
            colorArray3[29][0] = new Color(88, 23, 23);
            colorArray3[30][0] = new Color(28, 88, 23);
            colorArray3[31][0] = new Color(78, 87, 99);
            color1 = new Color(69, 67, 41);
            colorArray3[34][0] = color1;
            colorArray3[37][0] = color1;
            colorArray3[32][0] = new Color(86, 17, 40);
            colorArray3[33][0] = new Color(49, 47, 83);
            colorArray3[35][0] = new Color(51, 51, 70);
            colorArray3[36][0] = new Color(87, 59, 55);
            colorArray3[38][0] = new Color(49, 57, 49);
            colorArray3[39][0] = new Color(78, 79, 73);
            colorArray3[45][0] = new Color(60, 59, 51);
            colorArray3[46][0] = new Color(48, 57, 47);
            colorArray3[47][0] = new Color(71, 77, 85);
            colorArray3[40][0] = new Color(85, 102, 103);
            colorArray3[41][0] = new Color(52, 50, 62);
            colorArray3[42][0] = new Color(71, 42, 44);
            colorArray3[43][0] = new Color(73, 66, 50);
            colorArray3[54][0] = new Color(40, 56, 50);
            colorArray3[55][0] = new Color(49, 48, 36);
            colorArray3[56][0] = new Color(43, 33, 32);
            colorArray3[57][0] = new Color(31, 40, 49);
            colorArray3[58][0] = new Color(48, 35, 52);
            colorArray3[60][0] = new Color(1, 52, 20);
            colorArray3[61][0] = new Color(55, 39, 26);
            colorArray3[62][0] = new Color(39, 33, 26);
            colorArray3[69][0] = new Color(43, 42, 68);
            colorArray3[70][0] = new Color(30, 70, 80);
            color1 = new Color(30, 80, 48);
            colorArray3[63][0] = color1;
            colorArray3[65][0] = color1;
            colorArray3[66][0] = color1;
            colorArray3[68][0] = color1;
            color1 = new Color(53, 80, 30);
            colorArray3[64][0] = color1;
            colorArray3[67][0] = color1;
            colorArray3[78][0] = new Color(63, 39, 26);
            colorArray3[71][0] = new Color(78, 105, 135);
            colorArray3[72][0] = new Color(52, 84, 12);
            colorArray3[73][0] = new Color(190, 204, 223);
            color1 = new Color(64, 62, 80);
            colorArray3[74][0] = color1;
            colorArray3[80][0] = color1;
            colorArray3[75][0] = new Color(65, 65, 35);
            colorArray3[76][0] = new Color(20, 46, 104);
            colorArray3[77][0] = new Color(61, 13, 16);
            colorArray3[79][0] = new Color(51, 47, 96);
            colorArray3[81][0] = new Color(101, 51, 51);
            colorArray3[82][0] = new Color(77, 64, 34);
            colorArray3[83][0] = new Color(62, 38, 41);
            colorArray3[84][0] = new Color(48, 78, 93);
            colorArray3[85][0] = new Color(54, 63, 69);
            color1 = new Color(138, 73, 38);
            colorArray3[86][0] = color1;
            colorArray3[108][0] = color1;
            color1 = new Color(50, 15, 8);
            colorArray3[87][0] = color1;
            colorArray3[112][0] = color1;
            colorArray3[109][0] = new Color(94, 25, 17);
            colorArray3[110][0] = new Color(125, 36, 122);
            colorArray3[111][0] = new Color(51, 35, 27);
            colorArray3[113][0] = new Color(135, 58, 0);
            colorArray3[114][0] = new Color(65, 52, 15);
            colorArray3[115][0] = new Color(39, 42, 51);
            colorArray3[116][0] = new Color(89, 26, 27);
            colorArray3[117][0] = new Color(126, 123, 115);
            colorArray3[118][0] = new Color(8, 50, 19);
            colorArray3[119][0] = new Color(95, 21, 24);
            colorArray3[120][0] = new Color(17, 31, 65);
            colorArray3[121][0] = new Color(192, 173, 143);
            colorArray3[122][0] = new Color(114, 114, 131);
            colorArray3[123][0] = new Color(136, 119, 7);
            colorArray3[124][0] = new Color(8, 72, 3);
            colorArray3[125][0] = new Color(117, 132, 82);
            colorArray3[126][0] = new Color(100, 102, 114);
            colorArray3[(int) sbyte.MaxValue][0] = new Color(30, 118, 226);
            colorArray3[128][0] = new Color(93, 6, 102);
            colorArray3[129][0] = new Color(64, 40, 169);
            colorArray3[130][0] = new Color(39, 34, 180);
            colorArray3[131][0] = new Color(87, 94, 125);
            colorArray3[132][0] = new Color(6, 6, 6);
            colorArray3[133][0] = new Color(69, 72, 186);
            colorArray3[134][0] = new Color(130, 62, 16);
            colorArray3[135][0] = new Color(22, 123, 163);
            colorArray3[136][0] = new Color(40, 86, 151);
            colorArray3[137][0] = new Color(183, 75, 15);
            colorArray3[138][0] = new Color(83, 80, 100);
            colorArray3[139][0] = new Color(115, 65, 68);
            colorArray3[140][0] = new Color(119, 108, 81);
            colorArray3[141][0] = new Color(59, 67, 71);
            colorArray3[142][0] = new Color(17, 172, 143);
            colorArray3[143][0] = new Color(90, 112, 105);
            colorArray3[144][0] = new Color(62, 28, 87);
            colorArray3[146][0] = new Color(120, 59, 19);
            colorArray3[147][0] = new Color(59, 59, 59);
            colorArray3[148][0] = new Color(229, 218, 161);
            colorArray3[149][0] = new Color(73, 59, 50);
            colorArray3[151][0] = new Color(102, 75, 34);
            colorArray3[167][0] = new Color(70, 68, 51);
            colorArray3[172][0] = new Color(163, 96, 0);
            colorArray3[173][0] = new Color(94, 163, 46);
            colorArray3[174][0] = new Color(117, 32, 59);
            colorArray3[175][0] = new Color(20, 11, 203);
            colorArray3[176][0] = new Color(74, 69, 88);
            colorArray3[177][0] = new Color(60, 30, 30);
            colorArray3[183][0] = new Color(111, 117, 135);
            colorArray3[179][0] = new Color(111, 117, 135);
            colorArray3[178][0] = new Color(111, 117, 135);
            colorArray3[184][0] = new Color(25, 23, 54);
            colorArray3[181][0] = new Color(25, 23, 54);
            colorArray3[180][0] = new Color(25, 23, 54);
            colorArray3[182][0] = new Color(74, 71, 129);
            colorArray3[185][0] = new Color(52, 52, 52);
            colorArray3[186][0] = new Color(38, 9, 66);
            colorArray3[216][0] = new Color(158, 100, 64);
            colorArray3[217][0] = new Color(62, 45, 75);
            colorArray3[218][0] = new Color(57, 14, 12);
            colorArray3[219][0] = new Color(96, 72, 133);
            colorArray3[187][0] = new Color(149, 80, 51);
            colorArray3[220][0] = new Color(67, 55, 80);
            colorArray3[221][0] = new Color(64, 37, 29);
            colorArray3[222][0] = new Color(70, 51, 91);
            colorArray3[188][0] = new Color(82, 63, 80);
            colorArray3[189][0] = new Color(65, 61, 77);
            colorArray3[190][0] = new Color(64, 65, 92);
            colorArray3[191][0] = new Color(76, 53, 84);
            colorArray3[192][0] = new Color(144, 67, 52);
            colorArray3[193][0] = new Color(149, 48, 48);
            colorArray3[194][0] = new Color(111, 32, 36);
            colorArray3[195][0] = new Color(147, 48, 55);
            colorArray3[196][0] = new Color(97, 67, 51);
            colorArray3[197][0] = new Color(112, 80, 62);
            colorArray3[198][0] = new Color(88, 61, 46);
            colorArray3[199][0] = new Color((int) sbyte.MaxValue, 94, 76);
            colorArray3[200][0] = new Color(143, 50, 123);
            colorArray3[201][0] = new Color(136, 120, 131);
            colorArray3[202][0] = new Color(219, 92, 143);
            colorArray3[203][0] = new Color(113, 64, 150);
            colorArray3[204][0] = new Color(74, 67, 60);
            colorArray3[205][0] = new Color(60, 78, 59);
            colorArray3[206][0] = new Color(0, 54, 21);
            colorArray3[207][0] = new Color(74, 97, 72);
            colorArray3[208][0] = new Color(40, 37, 35);
            colorArray3[209][0] = new Color(77, 63, 66);
            colorArray3[210][0] = new Color(111, 6, 6);
            colorArray3[211][0] = new Color(88, 67, 59);
            colorArray3[212][0] = new Color(88, 87, 80);
            colorArray3[213][0] = new Color(71, 71, 67);
            colorArray3[214][0] = new Color(76, 52, 60);
            colorArray3[215][0] = new Color(89, 48, 59);
            colorArray3[223][0] = new Color(51, 18, 4);
            colorArray3[228][0] = new Color(160, 2, 75);
            colorArray3[229][0] = new Color(100, 55, 164);
            colorArray3[230][0] = new Color(0, 117, 101);
            var colorArray4 = new Color[256];
            var color2 = new Color(50, 40, (int) byte.MaxValue);
            var color3 = new Color(145, 185, (int) byte.MaxValue);
            for (var index = 0; index < colorArray4.Length; ++index)
            {
                var num1 = (float) index / (float) colorArray4.Length;
                var num2 = 1f - num1;
                colorArray4[index] = new Color(
                    (int) (byte) ((double) color2.R * (double) num2 + (double) color3.R * (double) num1),
                    (int) (byte) ((double) color2.G * (double) num2 + (double) color3.G * (double) num1),
                    (int) (byte) ((double) color2.B * (double) num2 + (double) color3.B * (double) num1));
            }

            var colorArray5 = new Color[256];
            var color4 = new Color(88, 61, 46);
            var color5 = new Color(37, 78, 123);
            for (var index = 0; index < colorArray5.Length; ++index)
            {
                var num1 = (float) index / (float) byte.MaxValue;
                var num2 = 1f - num1;
                colorArray5[index] = new Color(
                    (int) (byte) ((double) color4.R * (double) num2 + (double) color5.R * (double) num1),
                    (int) (byte) ((double) color4.G * (double) num2 + (double) color5.G * (double) num1),
                    (int) (byte) ((double) color4.B * (double) num2 + (double) color5.B * (double) num1));
            }

            var colorArray6 = new Color[256];
            var color6 = new Color(74, 67, 60);
            color5 = new Color(53, 70, 97);
            for (var index = 0; index < colorArray6.Length; ++index)
            {
                var num1 = (float) index / (float) byte.MaxValue;
                var num2 = 1f - num1;
                colorArray6[index] = new Color(
                    (int) (byte) ((double) color6.R * (double) num2 + (double) color5.R * (double) num1),
                    (int) (byte) ((double) color6.G * (double) num2 + (double) color5.G * (double) num1),
                    (int) (byte) ((double) color6.B * (double) num2 + (double) color5.B * (double) num1));
            }

            var color7 = new Color(50, 44, 38);
            var num3 = 0;
            MapHelper.tileOptionCounts = new int[470];
            for (var index1 = 0; index1 < 470; ++index1)
            {
                var colorArray7 = colorArray1[index1];
                var index2 = 0;
                while (index2 < 12 && !(colorArray7[index2] == Color.Transparent))
                    ++index2;
                MapHelper.tileOptionCounts[index1] = index2;
                num3 += index2;
            }

            MapHelper.wallOptionCounts = new int[231];
            for (var index1 = 0; index1 < 231; ++index1)
            {
                var colorArray7 = colorArray3[index1];
                var index2 = 0;
                while (index2 < 2 && !(colorArray7[index2] == Color.Transparent))
                    ++index2;
                MapHelper.wallOptionCounts[index1] = index2;
                num3 += index2;
            }

            MapHelper.colorLookup = new Color[num3 + 773];
            MapHelper.colorLookup[0] = Color.Transparent;
            ushort num4 = 1;
            MapHelper.tilePosition = num4;
            MapHelper.tileLookup = new ushort[470];
            for (var index1 = 0; index1 < 470; ++index1)
            {
                if (MapHelper.tileOptionCounts[index1] > 0)
                {
                    var colorArray7 = colorArray1[index1];
                    MapHelper.tileLookup[index1] = num4;
                    for (var index2 = 0; index2 < MapHelper.tileOptionCounts[index1]; ++index2)
                    {
                        MapHelper.colorLookup[(int) num4] = colorArray1[index1][index2];
                        ++num4;
                    }
                }
                else
                    MapHelper.tileLookup[index1] = (ushort) 0;
            }

            MapHelper.wallPosition = num4;
            MapHelper.wallLookup = new ushort[231];
            MapHelper.wallRangeStart = num4;
            for (var index1 = 0; index1 < 231; ++index1)
            {
                if (MapHelper.wallOptionCounts[index1] > 0)
                {
                    var colorArray7 = colorArray3[index1];
                    MapHelper.wallLookup[index1] = num4;
                    for (var index2 = 0; index2 < MapHelper.wallOptionCounts[index1]; ++index2)
                    {
                        MapHelper.colorLookup[(int) num4] = colorArray3[index1][index2];
                        ++num4;
                    }
                }
                else
                    MapHelper.wallLookup[index1] = (ushort) 0;
            }

            MapHelper.wallRangeEnd = num4;
            MapHelper.liquidPosition = num4;
            for (var index = 0; index < 3; ++index)
            {
                MapHelper.colorLookup[(int) num4] = colorArray2[index];
                ++num4;
            }

            MapHelper.skyPosition = num4;
            for (var index = 0; index < 256; ++index)
            {
                MapHelper.colorLookup[(int) num4] = colorArray4[index];
                ++num4;
            }

            MapHelper.dirtPosition = num4;
            for (var index = 0; index < 256; ++index)
            {
                MapHelper.colorLookup[(int) num4] = colorArray5[index];
                ++num4;
            }

            MapHelper.rockPosition = num4;
            for (var index = 0; index < 256; ++index)
            {
                MapHelper.colorLookup[(int) num4] = colorArray6[index];
                ++num4;
            }

            MapHelper.hellPosition = num4;
            MapHelper.colorLookup[(int) num4] = color7;
            MapHelper.snowTypes = new ushort[6];
            MapHelper.snowTypes[0] = MapHelper.tileLookup[147];
            MapHelper.snowTypes[1] = MapHelper.tileLookup[161];
            MapHelper.snowTypes[2] = MapHelper.tileLookup[162];
            MapHelper.snowTypes[3] = MapHelper.tileLookup[163];
            MapHelper.snowTypes[4] = MapHelper.tileLookup[164];
            MapHelper.snowTypes[5] = MapHelper.tileLookup[200];
            Lang.BuildMapAtlas();
        }

        public static int TileToLookup(int tileType, int option)
        {
            return (int) MapHelper.tileLookup[tileType] + option;
        }

        public static int LookupCount()
        {
            return MapHelper.colorLookup.Length;
        }

        private static void MapColor(ushort type, ref Color oldColor, byte colorType)
        {
            var color = WorldGen.paintColor((int) colorType);
            var num1 = (float) oldColor.R / (float) byte.MaxValue;
            var num2 = (float) oldColor.G / (float) byte.MaxValue;
            var num3 = (float) oldColor.B / (float) byte.MaxValue;
            if ((double) num2 > (double) num1)
                num1 = num2;
            if ((double) num3 > (double) num1)
            {
                var num4 = num1;
                num1 = num3;
                num3 = num4;
            }

            switch (colorType)
            {
                case 29:
                    var num5 = num3 * 0.3f;
                    oldColor.R = (byte) ((double) color.R * (double) num5);
                    oldColor.G = (byte) ((double) color.G * (double) num5);
                    oldColor.B = (byte) ((double) color.B * (double) num5);
                    break;
                case 30:
                    if ((int) type >= (int) MapHelper.wallRangeStart && (int) type <= (int) MapHelper.wallRangeEnd)
                    {
                        oldColor.R = (byte) ((double) ((int) byte.MaxValue - (int) oldColor.R) * 0.5);
                        oldColor.G = (byte) ((double) ((int) byte.MaxValue - (int) oldColor.G) * 0.5);
                        oldColor.B = (byte) ((double) ((int) byte.MaxValue - (int) oldColor.B) * 0.5);
                        break;
                    }

                    oldColor.R = (byte) ((uint) byte.MaxValue - (uint) oldColor.R);
                    oldColor.G = (byte) ((uint) byte.MaxValue - (uint) oldColor.G);
                    oldColor.B = (byte) ((uint) byte.MaxValue - (uint) oldColor.B);
                    break;
                default:
                    var num6 = num1;
                    oldColor.R = (byte) ((double) color.R * (double) num6);
                    oldColor.G = (byte) ((double) color.G * (double) num6);
                    oldColor.B = (byte) ((double) color.B * (double) num6);
                    break;
            }
        }

        public static Color GetMapTileXnaColor(ref MapTile tile)
        {
            var oldColor = MapHelper.colorLookup[(int) tile.Type];
            var color = tile.Color;
            if (color > (byte) 0)
                MapHelper.MapColor(tile.Type, ref oldColor, color);
            if (tile.Light == byte.MaxValue)
                return oldColor;
            var num = (float) tile.Light / (float) byte.MaxValue;
            oldColor.R = (byte) ((double) oldColor.R * (double) num);
            oldColor.G = (byte) ((double) oldColor.G * (double) num);
            oldColor.B = (byte) ((double) oldColor.B * (double) num);
            return oldColor;
        }

        public static MapTile CreateMapTile(int i, int j, byte Light)
        {
            var tile = Main.tile[i, j] ?? (Main.tile[i, j] = new Tile());
            var num1 = 0;
            var num2 = (int) Light;
            var type1 = Main.Map[i, j].Type;
            var num3 = 0;
            var num4 = 0;
            if (tile.active())
            {
                var type2 = (int) tile.type;
                num3 = (int) MapHelper.tileLookup[type2];
                if (type2 == 51 && (i + j) % 2 == 0)
                    num3 = 0;
                if (num3 != 0)
                {
                    num1 = type2 != 160 ? (int) tile.color() : 0;
                    switch (type2)
                    {
                        case 4:
                            if (tile.frameX < (short) 66)
                                ;
                            num4 = 0;
                            break;
                        case 21:
                        case 441:
                            switch ((int) tile.frameX / 36)
                            {
                                case 1:
                                case 2:
                                case 10:
                                case 13:
                                case 15:
                                    num4 = 1;
                                    break;
                                case 3:
                                case 4:
                                    num4 = 2;
                                    break;
                                case 6:
                                    num4 = 3;
                                    break;
                                case 11:
                                case 17:
                                    num4 = 4;
                                    break;
                                default:
                                    num4 = 0;
                                    break;
                            }

                            break;
                        case 26:
                            num4 = tile.frameX < (short) 54 ? 0 : 1;
                            break;
                        case 27:
                            num4 = tile.frameY >= (short) 34 ? 0 : 1;
                            break;
                        case 28:
                            num4 = tile.frameY >= (short) 144
                                ? (tile.frameY >= (short) 252
                                    ? (tile.frameY < (short) 360 ||
                                       tile.frameY > (short) 900 && tile.frameY < (short) 1008
                                        ? 2
                                        : (tile.frameY >= (short) 468
                                            ? (tile.frameY >= (short) 576
                                                ? (tile.frameY >= (short) 684
                                                    ? (tile.frameY >= (short) 792
                                                        ? (tile.frameY >= (short) 898
                                                            ? (tile.frameY >= (short) 1006
                                                                ? (tile.frameY >= (short) 1114
                                                                    ? (tile.frameY >= (short) 1222 ? 7 : 3)
                                                                    : 0)
                                                                : 7)
                                                            : 8)
                                                        : 6)
                                                    : 5)
                                                : 4)
                                            : 3))
                                    : 1)
                                : 0;
                            break;
                        case 31:
                            num4 = tile.frameX < (short) 36 ? 0 : 1;
                            break;
                        case 82:
                        case 83:
                        case 84:
                            num4 = tile.frameX >= (short) 18
                                ? (tile.frameX >= (short) 36
                                    ? (tile.frameX >= (short) 54
                                        ? (tile.frameX >= (short) 72
                                            ? (tile.frameX >= (short) 90 ? (tile.frameX >= (short) 108 ? 6 : 5) : 4)
                                            : 3)
                                        : 2)
                                    : 1)
                                : 0;
                            break;
                        case 105:
                            num4 = tile.frameX < (short) 1548 || tile.frameX > (short) 1654
                                ? (tile.frameX < (short) 1656 || tile.frameX > (short) 1798 ? 0 : 2)
                                : 1;
                            break;
                        case 133:
                            num4 = tile.frameX >= (short) 52 ? 1 : 0;
                            break;
                        case 134:
                            num4 = tile.frameX >= (short) 28 ? 1 : 0;
                            break;
                        case 137:
                            num4 = tile.frameY != (short) 0 ? 1 : 0;
                            break;
                        case 149:
                            num4 = j % 3;
                            break;
                        case 160:
                            num4 = j % 3;
                            break;
                        case 165:
                            num4 = tile.frameX >= (short) 54
                                ? (tile.frameX >= (short) 106
                                    ? (tile.frameX < (short) 216 ? (tile.frameX >= (short) 162 ? 3 : 2) : 1)
                                    : 1)
                                : 0;
                            break;
                        case 178:
                            num4 = tile.frameX >= (short) 18
                                ? (tile.frameX >= (short) 36
                                    ? (tile.frameX >= (short) 54
                                        ? (tile.frameX >= (short) 72
                                            ? (tile.frameX >= (short) 90 ? (tile.frameX >= (short) 108 ? 6 : 5) : 4)
                                            : 3)
                                        : 2)
                                    : 1)
                                : 0;
                            break;
                        case 184:
                            num4 = tile.frameX >= (short) 22
                                ? (tile.frameX >= (short) 44
                                    ? (tile.frameX >= (short) 66
                                        ? (tile.frameX >= (short) 88 ? (tile.frameX >= (short) 110 ? 5 : 4) : 3)
                                        : 2)
                                    : 1)
                                : 0;
                            break;
                        case 185:
                            if (tile.frameY < (short) 18)
                            {
                                var num5 = (int) tile.frameX / 18;
                                if (num5 < 6 || num5 == 28 || (num5 == 29 || num5 == 30) || (num5 == 31 || num5 == 32))
                                {
                                    num4 = 0;
                                    break;
                                }

                                if (num5 < 12 || num5 == 33 || (num5 == 34 || num5 == 35))
                                {
                                    num4 = 1;
                                    break;
                                }

                                if (num5 < 28)
                                {
                                    num4 = 2;
                                    break;
                                }

                                if (num5 < 48)
                                {
                                    num4 = 3;
                                    break;
                                }

                                if (num5 < 54)
                                {
                                    num4 = 4;
                                    break;
                                }

                                break;
                            }

                            var num6 = (int) tile.frameX / 36;
                            if (num6 < 6 || num6 == 19 || (num6 == 20 || num6 == 21) ||
                                (num6 == 22 || num6 == 23 || (num6 == 24 || num6 == 33)) ||
                                (num6 == 38 || num6 == 39 || num6 == 40))
                            {
                                num4 = 0;
                                break;
                            }

                            if (num6 < 16)
                            {
                                num4 = 2;
                                break;
                            }

                            if (num6 < 19 || num6 == 31 || num6 == 32)
                            {
                                num4 = 1;
                                break;
                            }

                            if (num6 < 31)
                            {
                                num4 = 3;
                                break;
                            }

                            if (num6 < 38)
                            {
                                num4 = 4;
                                break;
                            }

                            break;
                        case 186:
                            var num7 = (int) tile.frameX / 54;
                            if (num7 < 7)
                            {
                                num4 = 2;
                                break;
                            }

                            if (num7 < 22 || num7 == 33 || (num7 == 34 || num7 == 35))
                            {
                                num4 = 0;
                                break;
                            }

                            if (num7 < 25)
                            {
                                num4 = 1;
                                break;
                            }

                            if (num7 == 25)
                            {
                                num4 = 5;
                                break;
                            }

                            if (num7 < 32)
                            {
                                num4 = 3;
                                break;
                            }

                            break;
                        case 187:
                            var num8 = (int) tile.frameX / 54;
                            if (num8 < 3 || num8 == 14 || (num8 == 15 || num8 == 16))
                            {
                                num4 = 0;
                                break;
                            }

                            if (num8 < 6)
                            {
                                num4 = 6;
                                break;
                            }

                            if (num8 < 9)
                            {
                                num4 = 7;
                                break;
                            }

                            if (num8 < 14)
                            {
                                num4 = 4;
                                break;
                            }

                            if (num8 < 18)
                            {
                                num4 = 4;
                                break;
                            }

                            if (num8 < 23)
                            {
                                num4 = 8;
                                break;
                            }

                            if (num8 < 25)
                            {
                                num4 = 0;
                                break;
                            }

                            if (num8 < 29)
                            {
                                num4 = 1;
                                break;
                            }

                            break;
                        case 227:
                            num4 = (int) tile.frameX / 34;
                            break;
                        case 240:
                            var num9 = (int) tile.frameX / 54 + (int) tile.frameY / 54 * 36;
                            if (num9 >= 0 && num9 <= 11 || num9 >= 47 && num9 <= 53)
                            {
                                num4 = 0;
                                break;
                            }

                            if (num9 >= 12 && num9 <= 15)
                            {
                                num4 = 1;
                                break;
                            }

                            if (num9 == 16 || num9 == 17)
                            {
                                num4 = 2;
                                break;
                            }

                            if (num9 >= 18 && num9 <= 35)
                            {
                                num4 = 1;
                                break;
                            }

                            if (num9 >= 41 && num9 <= 45)
                            {
                                num4 = 3;
                                break;
                            }

                            if (num9 == 46)
                            {
                                num4 = 4;
                                break;
                            }

                            break;
                        case 242:
                            var num10 = (int) tile.frameY / 72;
                            num4 = num10 < 22 || num10 > 24 ? 0 : 1;
                            break;
                        case 419:
                            var num11 = (int) tile.frameX / 18;
                            if (num11 > 2)
                                num11 = 2;
                            num4 = num11;
                            break;
                        case 420:
                            var num12 = (int) tile.frameY / 18;
                            if (num12 > 5)
                                num12 = 5;
                            num4 = num12;
                            break;
                        case 423:
                            var num13 = (int) tile.frameY / 18;
                            if (num13 > 6)
                                num13 = 6;
                            num4 = num13;
                            break;
                        case 428:
                            var num14 = (int) tile.frameY / 18;
                            if (num14 > 3)
                                num14 = 3;
                            num4 = num14;
                            break;
                        case 440:
                            var num15 = (int) tile.frameX / 54;
                            if (num15 > 6)
                                num15 = 6;
                            num4 = num15;
                            break;
                        case 453:
                            var num16 = (int) tile.frameX / 36;
                            if (num16 > 2)
                                num16 = 2;
                            num4 = num16;
                            break;
                        case 457:
                            var num17 = (int) tile.frameX / 36;
                            if (num17 > 4)
                                num17 = 4;
                            num4 = num17;
                            break;
                        case 467:
                        case 468:
                            switch ((int) tile.frameX / 36)
                            {
                                case 0:
                                    num4 = 0;
                                    break;
                                case 1:
                                    num4 = 1;
                                    break;
                                default:
                                    num4 = 0;
                                    break;
                            }

                            break;
                        default:
                            num4 = 0;
                            break;
                    }
                }
            }

            if (num3 == 0)
            {
                if (tile.liquid > (byte) 32)
                {
                    var num5 = (int) tile.liquidType();
                    num3 = (int) MapHelper.liquidPosition + num5;
                }
                else if (tile.wall > (byte) 0)
                {
                    var wall = (int) tile.wall;
                    num3 = (int) MapHelper.wallLookup[wall];
                    num1 = (int) tile.wallColor();
                    switch (wall)
                    {
                        case 21:
                        case 88:
                        case 89:
                        case 90:
                        case 91:
                        case 92:
                        case 93:
                        case 168:
                            num1 = 0;
                            break;
                        case 27:
                            num4 = i % 2;
                            break;
                        default:
                            num4 = 0;
                            break;
                    }
                }
            }

            if (num3 == 0)
            {
                if ((double) j < Main.worldSurface)
                {
                    var num5 = (int) (byte) ((double) byte.MaxValue * ((double) j / Main.worldSurface));
                    num3 = (int) MapHelper.skyPosition + num5;
                    num2 = (int) byte.MaxValue;
                    num1 = 0;
                }
                else if (j < Main.maxTilesY - 200)
                {
                    num1 = 0;
                    var flag = (int) type1 < (int) MapHelper.dirtPosition ||
                                (int) type1 >= (int) MapHelper.hellPosition;
                    byte num5 = 0;
                    var num6 = (float) ((double) Main.screenPosition.X / 16.0 - 5.0);
                    var num7 = (float) (((double) Main.screenPosition.X + (double) Main.screenWidth) / 16.0 + 5.0);
                    var num8 = (float) ((double) Main.screenPosition.Y / 16.0 - 5.0);
                    var num9 = (float) (((double) Main.screenPosition.Y + (double) Main.screenHeight) / 16.0 + 5.0);
                    if (((double) i < (double) num6 || (double) i > (double) num7 ||
                         ((double) j < (double) num8 || (double) j > (double) num9)) &&
                        (i > 40 && i < Main.maxTilesX - 40 && (j > 40 && j < Main.maxTilesY - 40)) && flag)
                    {
                        var num10 = i - 36;
                        while (num10 <= i + 30)
                        {
                            var num11 = j - 36;
                            while (num11 <= j + 30)
                            {
                                for (var index = 0; index < MapHelper.snowTypes.Length; ++index)
                                {
                                    if ((int) MapHelper.snowTypes[index] == (int) type1)
                                    {
                                        num5 = byte.MaxValue;
                                        num10 = i + 31;
                                        num11 = j + 31;
                                        break;
                                    }
                                }

                                num11 += 10;
                            }

                            num10 += 10;
                        }
                    }
                    else
                    {
                        var num10 = (float) Main.snowTiles / 1000f * (float) byte.MaxValue;
                        if ((double) num10 > (double) byte.MaxValue)
                            num10 = (float) byte.MaxValue;
                        num5 = (byte) num10;
                    }

                    num3 = (double) j >= Main.rockLayer
                        ? (int) MapHelper.rockPosition + (int) num5
                        : (int) MapHelper.dirtPosition + (int) num5;
                }
                else
                    num3 = (int) MapHelper.hellPosition;
            }

            return MapTile.Create((ushort) (num3 + num4), (byte) num2, (byte) num1);
        }

        public static void SaveMap()
        {
            var isCloudSave = Main.ActivePlayerFileData.IsCloudSave;
            if (isCloudSave && SocialAPI.Cloud == null || (!Main.mapEnabled || MapHelper.saveLock))
                return;
            var path1 = Main.playerPathName.Substring(0, Main.playerPathName.Length - 4);
            lock (MapHelper.padlock)
            {
                try
                {
                    MapHelper.saveLock = true;
                    try
                    {
                        if (!isCloudSave)
                            Directory.CreateDirectory(path1);
                    }
                    catch
                    {
                    }

                    var str = path1 + (object) Path.DirectorySeparatorChar;
                    var path2 = !Main.ActiveWorldFileData.UseGuidAsMapName
                        ? str + (object) Main.worldID + ".map"
                        : str + Main.ActiveWorldFileData.UniqueId.ToString() + ".map";
                    new Stopwatch().Start();
                    var flag1 = false;
                    if (!Main.gameMenu)
                        flag1 = true;
                    using (var memoryStream = new MemoryStream(4000))
                    {
                        using (var writer = new BinaryWriter((Stream) memoryStream))
                        {
                            using (var deflateStream =
                                new DeflateStream((Stream) memoryStream, (CompressionMode) 0))
                            {
                                var count = 0;
                                var buffer = new byte[16384];
                                writer.Write(193);
                                Main.MapFileMetadata.IncrementAndWrite(writer);
                                writer.Write(Main.worldName);
                                writer.Write(Main.worldID);
                                writer.Write(Main.maxTilesY);
                                writer.Write(Main.maxTilesX);
                                writer.Write((short) 470);
                                writer.Write((short) 231);
                                writer.Write((short) 3);
                                writer.Write((short) 256);
                                writer.Write((short) 256);
                                writer.Write((short) 256);
                                byte num1 = 1;
                                byte num2 = 0;
                                for (var index = 0; index < 470; ++index)
                                {
                                    if (MapHelper.tileOptionCounts[index] != 1)
                                        num2 |= num1;
                                    if (num1 == (byte) 128)
                                    {
                                        writer.Write(num2);
                                        num2 = (byte) 0;
                                        num1 = (byte) 1;
                                    }
                                    else
                                        num1 <<= 1;
                                }

                                if (num1 != (byte) 1)
                                    writer.Write(num2);
                                var index1 = 0;
                                byte num3 = 1;
                                byte num4 = 0;
                                for (; index1 < 231; ++index1)
                                {
                                    if (MapHelper.wallOptionCounts[index1] != 1)
                                        num4 |= num3;
                                    if (num3 == (byte) 128)
                                    {
                                        writer.Write(num4);
                                        num4 = (byte) 0;
                                        num3 = (byte) 1;
                                    }
                                    else
                                        num3 <<= 1;
                                }

                                if (num3 != (byte) 1)
                                    writer.Write(num4);
                                for (var index2 = 0; index2 < 470; ++index2)
                                {
                                    if (MapHelper.tileOptionCounts[index2] != 1)
                                        writer.Write((byte) MapHelper.tileOptionCounts[index2]);
                                }

                                for (var index2 = 0; index2 < 231; ++index2)
                                {
                                    if (MapHelper.wallOptionCounts[index2] != 1)
                                        writer.Write((byte) MapHelper.wallOptionCounts[index2]);
                                }

                                writer.Flush();
                                for (var index2 = 0; index2 < Main.maxTilesY; ++index2)
                                {
                                    if (!flag1)
                                    {
                                        var num5 = (float) index2 / (float) Main.maxTilesY;
                                        Main.statusText =
                                            Lang.gen[66].Value + " " + (object) (int) ((double) num5 * 100.0 + 1.0) +
                                            "%";
                                    }

                                    int num6;
                                    for (var index3 = 0; index3 < Main.maxTilesX; index3 = num6 + 1)
                                    {
                                        var mapTile = Main.Map[index3, index2];
                                        byte num5;
                                        var num7 = num5 = (byte) 0;
                                        var flag2 = true;
                                        var flag3 = true;
                                        var num8 = 0;
                                        var num9 = 0;
                                        byte num10 = 0;
                                        int num11;
                                        ushort num12;
                                        int num13;
                                        if (mapTile.Light <= (byte) 18)
                                        {
                                            flag3 = false;
                                            flag2 = false;
                                            num11 = 0;
                                            num12 = (ushort) 0;
                                            num13 = 0;
                                            var index4 = index3 + 1;
                                            for (var index5 = Main.maxTilesX - index3 - 1;
                                                index5 > 0 && Main.Map[index4, index2].Light <= (byte) 18;
                                                ++index4)
                                            {
                                                ++num13;
                                                --index5;
                                            }
                                        }
                                        else
                                        {
                                            num10 = mapTile.Color;
                                            num12 = mapTile.Type;
                                            if ((int) num12 < (int) MapHelper.wallPosition)
                                            {
                                                num11 = 1;
                                                num12 -= MapHelper.tilePosition;
                                            }
                                            else if ((int) num12 < (int) MapHelper.liquidPosition)
                                            {
                                                num11 = 2;
                                                num12 -= MapHelper.wallPosition;
                                            }
                                            else if ((int) num12 < (int) MapHelper.skyPosition)
                                            {
                                                num11 = 3 + ((int) num12 - (int) MapHelper.liquidPosition);
                                                flag2 = false;
                                            }
                                            else if ((int) num12 < (int) MapHelper.dirtPosition)
                                            {
                                                num11 = 6;
                                                flag3 = false;
                                                flag2 = false;
                                            }
                                            else if ((int) num12 < (int) MapHelper.hellPosition)
                                            {
                                                num11 = 7;
                                                if ((int) num12 < (int) MapHelper.rockPosition)
                                                    num12 -= MapHelper.dirtPosition;
                                                else
                                                    num12 -= MapHelper.rockPosition;
                                            }
                                            else
                                            {
                                                num11 = 6;
                                                flag2 = false;
                                            }

                                            if (mapTile.Light == byte.MaxValue)
                                                flag3 = false;
                                            if (flag3)
                                            {
                                                num13 = 0;
                                                var index4 = index3 + 1;
                                                var num14 = Main.maxTilesX - index3 - 1;
                                                num8 = index4;
                                                while (num14 > 0)
                                                {
                                                    var other = Main.Map[index4, index2];
                                                    if (mapTile.EqualsWithoutLight(ref other))
                                                    {
                                                        --num14;
                                                        ++num13;
                                                        ++index4;
                                                    }
                                                    else
                                                    {
                                                        num9 = index4;
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                num13 = 0;
                                                var index4 = index3 + 1;
                                                var num14 = Main.maxTilesX - index3 - 1;
                                                while (num14 > 0)
                                                {
                                                    var other = Main.Map[index4, index2];
                                                    if (mapTile.Equals(ref other))
                                                    {
                                                        --num14;
                                                        ++num13;
                                                        ++index4;
                                                    }
                                                    else
                                                        break;
                                                }
                                            }
                                        }

                                        if (num10 > (byte) 0)
                                            num5 |= (byte) ((uint) num10 << 1);
                                        if (num5 != (byte) 0)
                                            num7 |= (byte) 1;
                                        var num15 = (byte) ((uint) num7 | (uint) (byte) (num11 << 1));
                                        if (flag2 && num12 > (ushort) byte.MaxValue)
                                            num15 |= (byte) 16;
                                        if (flag3)
                                            num15 |= (byte) 32;
                                        if (num13 > 0)
                                        {
                                            if (num13 > (int) byte.MaxValue)
                                                num15 |= (byte) 128;
                                            else
                                                num15 |= (byte) 64;
                                        }

                                        buffer[count] = num15;
                                        ++count;
                                        if (num5 != (byte) 0)
                                        {
                                            buffer[count] = num5;
                                            ++count;
                                        }

                                        if (flag2)
                                        {
                                            buffer[count] = (byte) num12;
                                            ++count;
                                            if (num12 > (ushort) byte.MaxValue)
                                            {
                                                buffer[count] = (byte) ((uint) num12 >> 8);
                                                ++count;
                                            }
                                        }

                                        if (flag3)
                                        {
                                            buffer[count] = mapTile.Light;
                                            ++count;
                                        }

                                        if (num13 > 0)
                                        {
                                            buffer[count] = (byte) num13;
                                            ++count;
                                            if (num13 > (int) byte.MaxValue)
                                            {
                                                buffer[count] = (byte) (num13 >> 8);
                                                ++count;
                                            }
                                        }

                                        for (var index4 = num8; index4 < num9; ++index4)
                                        {
                                            buffer[count] = Main.Map[index4, index2].Light;
                                            ++count;
                                        }

                                        num6 = index3 + num13;
                                        if (count >= 4096)
                                        {
                                            ((Stream) deflateStream).Write(buffer, 0, count);
                                            count = 0;
                                        }
                                    }
                                }

                                if (count > 0)
                                    ((Stream) deflateStream).Write(buffer, 0, count);
                                ((Stream) deflateStream).Dispose();
                                FileUtilities.WriteAllBytes(path2, memoryStream.ToArray(), isCloudSave);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    using (var streamWriter = new StreamWriter("client-crashlog.txt", true))
                    {
                        streamWriter.WriteLine((object) DateTime.Now);
                        streamWriter.WriteLine((object) ex);
                        streamWriter.WriteLine("");
                    }
                }

                MapHelper.saveLock = false;
            }
        }

        public static void LoadMapVersion1(BinaryReader fileIO, int release)
        {
            var str = fileIO.ReadString();
            var num1 = fileIO.ReadInt32();
            var num2 = fileIO.ReadInt32();
            var num3 = fileIO.ReadInt32();
            if (str != Main.worldName || num1 != Main.worldID || (num3 != Main.maxTilesX || num2 != Main.maxTilesY))
                throw new Exception("Map meta-data is invalid.");
            for (var x = 0; x < Main.maxTilesX; ++x)
            {
                var num4 = (float) x / (float) Main.maxTilesX;
                Main.statusText = Lang.gen[67].Value + " " + (object) (int) ((double) num4 * 100.0 + 1.0) + "%";
                for (var y = 0; y < Main.maxTilesY; ++y)
                {
                    if (fileIO.ReadBoolean())
                    {
                        var index = release <= 77 ? (int) fileIO.ReadByte() : (int) fileIO.ReadUInt16();
                        var light = fileIO.ReadByte();
                        MapHelper.OldMapHelper oldMapHelper;
                        oldMapHelper.misc = fileIO.ReadByte();
                        oldMapHelper.misc2 = release < 50 ? (byte) 0 : fileIO.ReadByte();
                        var flag = false;
                        var num5 = (int) oldMapHelper.option();
                        int num6;
                        if (oldMapHelper.active())
                            num6 = num5 + (int) MapHelper.tileLookup[index];
                        else if (oldMapHelper.water())
                            num6 = (int) MapHelper.liquidPosition;
                        else if (oldMapHelper.lava())
                            num6 = (int) MapHelper.liquidPosition + 1;
                        else if (oldMapHelper.honey())
                            num6 = (int) MapHelper.liquidPosition + 2;
                        else if (oldMapHelper.wall())
                            num6 = num5 + (int) MapHelper.wallLookup[index];
                        else if ((double) y < Main.worldSurface)
                        {
                            flag = true;
                            var num7 = (int) (byte) (256.0 * ((double) y / Main.worldSurface));
                            num6 = (int) MapHelper.skyPosition + num7;
                        }
                        else if ((double) y < Main.rockLayer)
                        {
                            flag = true;
                            if (index > (int) byte.MaxValue)
                                index = (int) byte.MaxValue;
                            num6 = index + (int) MapHelper.dirtPosition;
                        }
                        else if (y < Main.maxTilesY - 200)
                        {
                            flag = true;
                            if (index > (int) byte.MaxValue)
                                index = (int) byte.MaxValue;
                            num6 = index + (int) MapHelper.rockPosition;
                        }
                        else
                            num6 = (int) MapHelper.hellPosition;

                        var tile = MapTile.Create((ushort) num6, light, (byte) 0);
                        Main.Map.SetTile(x, y, ref tile);
                        var num8 = (int) fileIO.ReadInt16();
                        if (light == byte.MaxValue)
                        {
                            while (num8 > 0)
                            {
                                --num8;
                                ++y;
                                if (flag)
                                {
                                    int num7;
                                    if ((double) y < Main.worldSurface)
                                    {
                                        flag = true;
                                        var num9 = (int) (byte) (256.0 * ((double) y / Main.worldSurface));
                                        num7 = (int) MapHelper.skyPosition + num9;
                                    }
                                    else if ((double) y < Main.rockLayer)
                                    {
                                        flag = true;
                                        num7 = index + (int) MapHelper.dirtPosition;
                                    }
                                    else if (y < Main.maxTilesY - 200)
                                    {
                                        flag = true;
                                        num7 = index + (int) MapHelper.rockPosition;
                                    }
                                    else
                                    {
                                        flag = true;
                                        num7 = (int) MapHelper.hellPosition;
                                    }

                                    tile.Type = (ushort) num7;
                                }

                                Main.Map.SetTile(x, y, ref tile);
                            }
                        }
                        else
                        {
                            while (num8 > 0)
                            {
                                ++y;
                                --num8;
                                var num7 = fileIO.ReadByte();
                                if (num7 > (byte) 18)
                                {
                                    tile.Light = num7;
                                    if (flag)
                                    {
                                        int num9;
                                        if ((double) y < Main.worldSurface)
                                        {
                                            flag = true;
                                            var num10 = (int) (byte) (256.0 * ((double) y / Main.worldSurface));
                                            num9 = (int) MapHelper.skyPosition + num10;
                                        }
                                        else if ((double) y < Main.rockLayer)
                                        {
                                            flag = true;
                                            num9 = index + (int) MapHelper.dirtPosition;
                                        }
                                        else if (y < Main.maxTilesY - 200)
                                        {
                                            flag = true;
                                            num9 = index + (int) MapHelper.rockPosition;
                                        }
                                        else
                                        {
                                            flag = true;
                                            num9 = (int) MapHelper.hellPosition;
                                        }

                                        tile.Type = (ushort) num9;
                                    }

                                    Main.Map.SetTile(x, y, ref tile);
                                }
                            }
                        }
                    }
                    else
                    {
                        var num5 = (int) fileIO.ReadInt16();
                        y += num5;
                    }
                }
            }
        }

        public static void LoadMapVersion2(BinaryReader fileIO, int release)
        {
            Main.MapFileMetadata = release < 135
                ? FileMetadata.FromCurrentSettings(FileType.Map)
                : FileMetadata.Read(fileIO, FileType.Map);
            var str = fileIO.ReadString();
            var num1 = fileIO.ReadInt32();
            var num2 = fileIO.ReadInt32();
            var num3 = fileIO.ReadInt32();
            if (str != Main.worldName || num1 != Main.worldID || (num3 != Main.maxTilesX || num2 != Main.maxTilesY))
                throw new Exception("Map meta-data is invalid.");
            var num4 = fileIO.ReadInt16();
            var num5 = fileIO.ReadInt16();
            var num6 = fileIO.ReadInt16();
            var num7 = fileIO.ReadInt16();
            var num8 = fileIO.ReadInt16();
            var num9 = fileIO.ReadInt16();
            var flagArray1 = new bool[(int) num4];
            byte num10 = 0;
            byte num11 = 128;
            for (var index = 0; index < (int) num4; ++index)
            {
                if (num11 == (byte) 128)
                {
                    num10 = fileIO.ReadByte();
                    num11 = (byte) 1;
                }
                else
                    num11 <<= 1;

                if (((int) num10 & (int) num11) == (int) num11)
                    flagArray1[index] = true;
            }

            var flagArray2 = new bool[(int) num5];
            byte num12 = 0;
            byte num13 = 128;
            for (var index = 0; index < (int) num5; ++index)
            {
                if (num13 == (byte) 128)
                {
                    num12 = fileIO.ReadByte();
                    num13 = (byte) 1;
                }
                else
                    num13 <<= 1;

                if (((int) num12 & (int) num13) == (int) num13)
                    flagArray2[index] = true;
            }

            var numArray1 = new byte[(int) num4];
            ushort num14 = 0;
            for (var index = 0; index < (int) num4; ++index)
            {
                numArray1[index] = !flagArray1[index] ? (byte) 1 : fileIO.ReadByte();
                num14 += (ushort) numArray1[index];
            }

            var numArray2 = new byte[(int) num5];
            ushort num15 = 0;
            for (var index = 0; index < (int) num5; ++index)
            {
                numArray2[index] = !flagArray2[index] ? (byte) 1 : fileIO.ReadByte();
                num15 += (ushort) numArray2[index];
            }

            var numArray3 =
                new ushort[(int) num14 + (int) num15 + (int) num6 + (int) num7 + (int) num8 + (int) num9 + 2];
            numArray3[0] = (ushort) 0;
            ushort num16 = 1;
            ushort num17 = 1;
            var num18 = num17;
            for (var index1 = 0; index1 < 470; ++index1)
            {
                if (index1 < (int) num4)
                {
                    var num19 = (int) numArray1[index1];
                    var tileOptionCount = MapHelper.tileOptionCounts[index1];
                    for (var index2 = 0; index2 < tileOptionCount; ++index2)
                    {
                        if (index2 < num19)
                        {
                            numArray3[(int) num17] = num16;
                            ++num17;
                        }

                        ++num16;
                    }
                }
                else
                    num16 += (ushort) MapHelper.tileOptionCounts[index1];
            }

            var num20 = num17;
            for (var index1 = 0; index1 < 231; ++index1)
            {
                if (index1 < (int) num5)
                {
                    var num19 = (int) numArray2[index1];
                    var wallOptionCount = MapHelper.wallOptionCounts[index1];
                    for (var index2 = 0; index2 < wallOptionCount; ++index2)
                    {
                        if (index2 < num19)
                        {
                            numArray3[(int) num17] = num16;
                            ++num17;
                        }

                        ++num16;
                    }
                }
                else
                    num16 += (ushort) MapHelper.wallOptionCounts[index1];
            }

            var num21 = num17;
            for (var index = 0; index < 3; ++index)
            {
                if (index < (int) num6)
                {
                    numArray3[(int) num17] = num16;
                    ++num17;
                }

                ++num16;
            }

            var num22 = num17;
            for (var index = 0; index < 256; ++index)
            {
                if (index < (int) num7)
                {
                    numArray3[(int) num17] = num16;
                    ++num17;
                }

                ++num16;
            }

            var num23 = num17;
            for (var index = 0; index < 256; ++index)
            {
                if (index < (int) num8)
                {
                    numArray3[(int) num17] = num16;
                    ++num17;
                }

                ++num16;
            }

            var num24 = num17;
            for (var index = 0; index < 256; ++index)
            {
                if (index < (int) num9)
                {
                    numArray3[(int) num17] = num16;
                    ++num17;
                }

                ++num16;
            }

            var num25 = num17;
            numArray3[(int) num17] = num16;
            var binaryReader = release < 93
                ? new BinaryReader(fileIO.BaseStream)
                : new BinaryReader((Stream) new DeflateStream((Stream) fileIO.BaseStream, (CompressionMode) 1));
            for (var y = 0; y < Main.maxTilesY; ++y)
            {
                var num19 = (float) y / (float) Main.maxTilesY;
                Main.statusText = Lang.gen[67].Value + " " + (object) (int) ((double) num19 * 100.0 + 1.0) + "%";
                for (var x = 0; x < Main.maxTilesX; ++x)
                {
                    var num26 = binaryReader.ReadByte();
                    var num27 = ((int) num26 & 1) != 1 ? (byte) 0 : binaryReader.ReadByte();
                    var num28 = (byte) (((int) num26 & 14) >> 1);
                    bool flag;
                    switch (num28)
                    {
                        case 0:
                            flag = false;
                            break;
                        case 1:
                        case 2:
                        case 7:
                            flag = true;
                            break;
                        case 3:
                        case 4:
                        case 5:
                            flag = false;
                            break;
                        case 6:
                            flag = false;
                            break;
                        default:
                            flag = false;
                            break;
                    }

                    var num29 = !flag
                        ? (ushort) 0
                        : (((int) num26 & 16) != 16 ? (ushort) binaryReader.ReadByte() : binaryReader.ReadUInt16());
                    var light = ((int) num26 & 32) != 32 ? byte.MaxValue : binaryReader.ReadByte();
                    int num30;
                    switch ((byte) (((int) num26 & 192) >> 6))
                    {
                        case 0:
                            num30 = 0;
                            break;
                        case 1:
                            num30 = (int) binaryReader.ReadByte();
                            break;
                        case 2:
                            num30 = (int) binaryReader.ReadInt16();
                            break;
                        default:
                            num30 = 0;
                            break;
                    }

                    switch (num28)
                    {
                        case 0:
                            x += num30;
                            break;
                        case 1:
                            num29 += num18;
                            goto default;
                        case 2:
                            num29 += num20;
                            goto default;
                        case 3:
                        case 4:
                        case 5:
                            num29 += (ushort) ((uint) num21 + ((uint) num28 - 3U));
                            goto default;
                        case 6:
                            if ((double) y < Main.worldSurface)
                            {
                                var num31 = (ushort) ((double) num7 * ((double) y / Main.worldSurface));
                                num29 += (ushort) ((uint) num22 + (uint) num31);
                                goto default;
                            }
                            else
                            {
                                num29 = num25;
                                goto default;
                            }
                        case 7:
                            if ((double) y < Main.rockLayer)
                            {
                                num29 += num23;
                                goto default;
                            }
                            else
                            {
                                num29 += num24;
                                goto default;
                            }
                        default:
                            var tile = MapTile.Create(numArray3[(int) num29], light,
                                (byte) ((int) num27 >> 1 & 31));
                            Main.Map.SetTile(x, y, ref tile);
                            if (light == byte.MaxValue)
                            {
                                for (; num30 > 0; --num30)
                                {
                                    ++x;
                                    Main.Map.SetTile(x, y, ref tile);
                                }

                                break;
                            }

                            for (; num30 > 0; --num30)
                            {
                                ++x;
                                tile = tile.WithLight(binaryReader.ReadByte());
                                Main.Map.SetTile(x, y, ref tile);
                            }

                            break;
                    }
                }
            }

            binaryReader.Close();
        }

        private struct OldMapHelper
        {
            public byte misc;
            public byte misc2;

            public bool active()
            {
                return ((int) this.misc & 1) == 1;
            }

            public bool water()
            {
                return ((int) this.misc & 2) == 2;
            }

            public bool lava()
            {
                return ((int) this.misc & 4) == 4;
            }

            public bool honey()
            {
                return ((int) this.misc2 & 64) == 64;
            }

            public bool changed()
            {
                return ((int) this.misc & 8) == 8;
            }

            public bool wall()
            {
                return ((int) this.misc & 16) == 16;
            }

            public byte option()
            {
                byte num = 0;
                if (((int) this.misc & 32) == 32)
                    ++num;
                if (((int) this.misc & 64) == 64)
                    num += (byte) 2;
                if (((int) this.misc & 128) == 128)
                    num += (byte) 4;
                if (((int) this.misc2 & 1) == 1)
                    num += (byte) 8;
                return num;
            }

            public byte color()
            {
                return (byte) (((int) this.misc2 & 30) >> 1);
            }
        }
    }
}