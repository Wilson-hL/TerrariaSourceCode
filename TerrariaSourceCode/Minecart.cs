﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Minecart
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria
{
    public static class Minecart
    {
        private static Vector2 _trackMagnetOffset = new Vector2(25f, 26f);
        private const int TotalFrames = 36;
        public const int LeftDownDecoration = 36;
        public const int RightDownDecoration = 37;
        public const int BouncyBumperDecoration = 38;
        public const int RegularBumperDecoration = 39;
        public const int Flag_OnTrack = 0;
        public const int Flag_BouncyBumper = 1;
        public const int Flag_UsedRamp = 2;
        public const int Flag_HitSwitch = 3;
        public const int Flag_BoostLeft = 4;
        public const int Flag_BoostRight = 5;
        private const int NoConnection = -1;
        private const int TopConnection = 0;
        private const int MiddleConnection = 1;
        private const int BottomConnection = 2;
        private const int BumperEnd = -1;
        private const int BouncyEnd = -2;
        private const int RampEnd = -3;
        private const int OpenEnd = -4;
        public const float BoosterSpeed = 4f;
        private const int Type_Normal = 0;
        private const int Type_Pressure = 1;
        private const int Type_Booster = 2;
        private const float MinecartTextureWidth = 50f;
        private static int[] _leftSideConnection;
        private static int[] _rightSideConnection;
        private static int[] _trackType;
        private static bool[] _boostLeft;
        private static Vector2[] _texturePosition;
        private static short _firstPressureFrame;
        private static short _firstLeftBoostFrame;
        private static short _firstRightBoostFrame;
        private static int[][] _trackSwitchOptions;
        private static int[][] _tileHeight;

        public static void Initialize()
        {
            if ((double) Main.minecartMountTexture.Width != 50.0)
                throw new Exception("Be sure to update Minecart.textureWidth to match the actual texture size of " +
                                    (object) 50f + ".");
            Minecart._rightSideConnection = new int[36];
            Minecart._leftSideConnection = new int[36];
            Minecart._trackType = new int[36];
            Minecart._boostLeft = new bool[36];
            Minecart._texturePosition = new Vector2[40];
            Minecart._tileHeight = new int[36][];
            for (var index1 = 0; index1 < 36; ++index1)
            {
                var numArray = new int[8];
                for (var index2 = 0; index2 < numArray.Length; ++index2)
                    numArray[index2] = 5;
                Minecart._tileHeight[index1] = numArray;
            }

            var index3 = 0;
            Minecart._leftSideConnection[index3] = -1;
            Minecart._rightSideConnection[index3] = -1;
            Minecart._tileHeight[index3][0] = -4;
            Minecart._tileHeight[index3][7] = -4;
            Minecart._texturePosition[index3] = new Vector2(0.0f, 0.0f);
            var index4 = index3 + 1;
            Minecart._leftSideConnection[index4] = 1;
            Minecart._rightSideConnection[index4] = 1;
            Minecart._texturePosition[index4] = new Vector2(1f, 0.0f);
            var index5 = index4 + 1;
            Minecart._leftSideConnection[index5] = -1;
            Minecart._rightSideConnection[index5] = 1;
            for (var index1 = 0; index1 < 4; ++index1)
                Minecart._tileHeight[index5][index1] = -1;
            Minecart._texturePosition[index5] = new Vector2(2f, 1f);
            var index6 = index5 + 1;
            Minecart._leftSideConnection[index6] = 1;
            Minecart._rightSideConnection[index6] = -1;
            for (var index1 = 4; index1 < 8; ++index1)
                Minecart._tileHeight[index6][index1] = -1;
            Minecart._texturePosition[index6] = new Vector2(3f, 1f);
            var index7 = index6 + 1;
            Minecart._leftSideConnection[index7] = 2;
            Minecart._rightSideConnection[index7] = 1;
            Minecart._tileHeight[index7][0] = 1;
            Minecart._tileHeight[index7][1] = 2;
            Minecart._tileHeight[index7][2] = 3;
            Minecart._tileHeight[index7][3] = 3;
            Minecart._tileHeight[index7][4] = 4;
            Minecart._tileHeight[index7][5] = 4;
            Minecart._texturePosition[index7] = new Vector2(0.0f, 2f);
            var index8 = index7 + 1;
            Minecart._leftSideConnection[index8] = 1;
            Minecart._rightSideConnection[index8] = 2;
            Minecart._tileHeight[index8][2] = 4;
            Minecart._tileHeight[index8][3] = 4;
            Minecart._tileHeight[index8][4] = 3;
            Minecart._tileHeight[index8][5] = 3;
            Minecart._tileHeight[index8][6] = 2;
            Minecart._tileHeight[index8][7] = 1;
            Minecart._texturePosition[index8] = new Vector2(1f, 2f);
            var index9 = index8 + 1;
            Minecart._leftSideConnection[index9] = 1;
            Minecart._rightSideConnection[index9] = 0;
            Minecart._tileHeight[index9][4] = 6;
            Minecart._tileHeight[index9][5] = 6;
            Minecart._tileHeight[index9][6] = 7;
            Minecart._tileHeight[index9][7] = 8;
            Minecart._texturePosition[index9] = new Vector2(0.0f, 1f);
            var index10 = index9 + 1;
            Minecart._leftSideConnection[index10] = 0;
            Minecart._rightSideConnection[index10] = 1;
            Minecart._tileHeight[index10][0] = 8;
            Minecart._tileHeight[index10][1] = 7;
            Minecart._tileHeight[index10][2] = 6;
            Minecart._tileHeight[index10][3] = 6;
            Minecart._texturePosition[index10] = new Vector2(1f, 1f);
            var index11 = index10 + 1;
            Minecart._leftSideConnection[index11] = 0;
            Minecart._rightSideConnection[index11] = 2;
            for (var index1 = 0; index1 < 8; ++index1)
                Minecart._tileHeight[index11][index1] = 8 - index1;
            Minecart._texturePosition[index11] = new Vector2(0.0f, 3f);
            var index12 = index11 + 1;
            Minecart._leftSideConnection[index12] = 2;
            Minecart._rightSideConnection[index12] = 0;
            for (var index1 = 0; index1 < 8; ++index1)
                Minecart._tileHeight[index12][index1] = index1 + 1;
            Minecart._texturePosition[index12] = new Vector2(1f, 3f);
            var index13 = index12 + 1;
            Minecart._leftSideConnection[index13] = 2;
            Minecart._rightSideConnection[index13] = -1;
            Minecart._tileHeight[index13][0] = 1;
            Minecart._tileHeight[index13][1] = 2;
            for (var index1 = 2; index1 < 8; ++index1)
                Minecart._tileHeight[index13][index1] = -1;
            Minecart._texturePosition[index13] = new Vector2(4f, 1f);
            var index14 = index13 + 1;
            Minecart._leftSideConnection[index14] = -1;
            Minecart._rightSideConnection[index14] = 2;
            Minecart._tileHeight[index14][6] = 2;
            Minecart._tileHeight[index14][7] = 1;
            for (var index1 = 0; index1 < 6; ++index1)
                Minecart._tileHeight[index14][index1] = -1;
            Minecart._texturePosition[index14] = new Vector2(5f, 1f);
            var index15 = index14 + 1;
            Minecart._leftSideConnection[index15] = 0;
            Minecart._rightSideConnection[index15] = -1;
            Minecart._tileHeight[index15][0] = 8;
            Minecart._tileHeight[index15][1] = 7;
            Minecart._tileHeight[index15][2] = 6;
            for (var index1 = 3; index1 < 8; ++index1)
                Minecart._tileHeight[index15][index1] = -1;
            Minecart._texturePosition[index15] = new Vector2(6f, 1f);
            var index16 = index15 + 1;
            Minecart._leftSideConnection[index16] = -1;
            Minecart._rightSideConnection[index16] = 0;
            Minecart._tileHeight[index16][5] = 6;
            Minecart._tileHeight[index16][6] = 7;
            Minecart._tileHeight[index16][7] = 8;
            for (var index1 = 0; index1 < 5; ++index1)
                Minecart._tileHeight[index16][index1] = -1;
            Minecart._texturePosition[index16] = new Vector2(7f, 1f);
            var index17 = index16 + 1;
            Minecart._leftSideConnection[index17] = -1;
            Minecart._rightSideConnection[index17] = 1;
            Minecart._tileHeight[index17][0] = -4;
            Minecart._texturePosition[index17] = new Vector2(2f, 0.0f);
            var index18 = index17 + 1;
            Minecart._leftSideConnection[index18] = 1;
            Minecart._rightSideConnection[index18] = -1;
            Minecart._tileHeight[index18][7] = -4;
            Minecart._texturePosition[index18] = new Vector2(3f, 0.0f);
            var index19 = index18 + 1;
            Minecart._leftSideConnection[index19] = 2;
            Minecart._rightSideConnection[index19] = -1;
            for (var index1 = 0; index1 < 6; ++index1)
                Minecart._tileHeight[index19][index1] = index1 + 1;
            Minecart._tileHeight[index19][6] = -3;
            Minecart._tileHeight[index19][7] = -3;
            Minecart._texturePosition[index19] = new Vector2(4f, 0.0f);
            var index20 = index19 + 1;
            Minecart._leftSideConnection[index20] = -1;
            Minecart._rightSideConnection[index20] = 2;
            Minecart._tileHeight[index20][0] = -3;
            Minecart._tileHeight[index20][1] = -3;
            for (var index1 = 2; index1 < 8; ++index1)
                Minecart._tileHeight[index20][index1] = 8 - index1;
            Minecart._texturePosition[index20] = new Vector2(5f, 0.0f);
            var index21 = index20 + 1;
            Minecart._leftSideConnection[index21] = 0;
            Minecart._rightSideConnection[index21] = -1;
            for (var index1 = 0; index1 < 6; ++index1)
                Minecart._tileHeight[index21][index1] = 8 - index1;
            Minecart._tileHeight[index21][6] = -3;
            Minecart._tileHeight[index21][7] = -3;
            Minecart._texturePosition[index21] = new Vector2(6f, 0.0f);
            var index22 = index21 + 1;
            Minecart._leftSideConnection[index22] = -1;
            Minecart._rightSideConnection[index22] = 0;
            Minecart._tileHeight[index22][0] = -3;
            Minecart._tileHeight[index22][1] = -3;
            for (var index1 = 2; index1 < 8; ++index1)
                Minecart._tileHeight[index22][index1] = index1 + 1;
            Minecart._texturePosition[index22] = new Vector2(7f, 0.0f);
            var index23 = index22 + 1;
            Minecart._leftSideConnection[index23] = -1;
            Minecart._rightSideConnection[index23] = -1;
            Minecart._tileHeight[index23][0] = -4;
            Minecart._tileHeight[index23][7] = -4;
            Minecart._trackType[index23] = 1;
            Minecart._texturePosition[index23] = new Vector2(0.0f, 4f);
            var index24 = index23 + 1;
            Minecart._leftSideConnection[index24] = 1;
            Minecart._rightSideConnection[index24] = 1;
            Minecart._trackType[index24] = 1;
            Minecart._texturePosition[index24] = new Vector2(1f, 4f);
            var index25 = index24 + 1;
            Minecart._leftSideConnection[index25] = -1;
            Minecart._rightSideConnection[index25] = 1;
            Minecart._tileHeight[index25][0] = -4;
            Minecart._trackType[index25] = 1;
            Minecart._texturePosition[index25] = new Vector2(0.0f, 5f);
            var index26 = index25 + 1;
            Minecart._leftSideConnection[index26] = 1;
            Minecart._rightSideConnection[index26] = -1;
            Minecart._tileHeight[index26][7] = -4;
            Minecart._trackType[index26] = 1;
            Minecart._texturePosition[index26] = new Vector2(1f, 5f);
            var index27 = index26 + 1;
            Minecart._leftSideConnection[index27] = -1;
            Minecart._rightSideConnection[index27] = 1;
            for (var index1 = 0; index1 < 6; ++index1)
                Minecart._tileHeight[index27][index1] = -2;
            Minecart._texturePosition[index27] = new Vector2(2f, 2f);
            var index28 = index27 + 1;
            Minecart._leftSideConnection[index28] = 1;
            Minecart._rightSideConnection[index28] = -1;
            for (var index1 = 2; index1 < 8; ++index1)
                Minecart._tileHeight[index28][index1] = -2;
            Minecart._texturePosition[index28] = new Vector2(3f, 2f);
            var index29 = index28 + 1;
            Minecart._leftSideConnection[index29] = 2;
            Minecart._rightSideConnection[index29] = -1;
            Minecart._tileHeight[index29][0] = 1;
            Minecart._tileHeight[index29][1] = 2;
            for (var index1 = 2; index1 < 8; ++index1)
                Minecart._tileHeight[index29][index1] = -2;
            Minecart._texturePosition[index29] = new Vector2(4f, 2f);
            var index30 = index29 + 1;
            Minecart._leftSideConnection[index30] = -1;
            Minecart._rightSideConnection[index30] = 2;
            Minecart._tileHeight[index30][6] = 2;
            Minecart._tileHeight[index30][7] = 1;
            for (var index1 = 0; index1 < 6; ++index1)
                Minecart._tileHeight[index30][index1] = -2;
            Minecart._texturePosition[index30] = new Vector2(5f, 2f);
            var index31 = index30 + 1;
            Minecart._leftSideConnection[index31] = 0;
            Minecart._rightSideConnection[index31] = -1;
            Minecart._tileHeight[index31][0] = 8;
            Minecart._tileHeight[index31][1] = 7;
            Minecart._tileHeight[index31][2] = 6;
            for (var index1 = 3; index1 < 8; ++index1)
                Minecart._tileHeight[index31][index1] = -2;
            Minecart._texturePosition[index31] = new Vector2(6f, 2f);
            var index32 = index31 + 1;
            Minecart._leftSideConnection[index32] = -1;
            Minecart._rightSideConnection[index32] = 0;
            Minecart._tileHeight[index32][5] = 6;
            Minecart._tileHeight[index32][6] = 7;
            Minecart._tileHeight[index32][7] = 8;
            for (var index1 = 0; index1 < 5; ++index1)
                Minecart._tileHeight[index32][index1] = -2;
            Minecart._texturePosition[index32] = new Vector2(7f, 2f);
            var index33 = index32 + 1;
            Minecart._leftSideConnection[index33] = 1;
            Minecart._rightSideConnection[index33] = 1;
            Minecart._trackType[index33] = 2;
            Minecart._boostLeft[index33] = false;
            Minecart._texturePosition[index33] = new Vector2(2f, 3f);
            var index34 = index33 + 1;
            Minecart._leftSideConnection[index34] = 1;
            Minecart._rightSideConnection[index34] = 1;
            Minecart._trackType[index34] = 2;
            Minecart._boostLeft[index34] = true;
            Minecart._texturePosition[index34] = new Vector2(3f, 3f);
            var index35 = index34 + 1;
            Minecart._leftSideConnection[index35] = 0;
            Minecart._rightSideConnection[index35] = 2;
            for (var index1 = 0; index1 < 8; ++index1)
                Minecart._tileHeight[index35][index1] = 8 - index1;
            Minecart._trackType[index35] = 2;
            Minecart._boostLeft[index35] = false;
            Minecart._texturePosition[index35] = new Vector2(4f, 3f);
            var index36 = index35 + 1;
            Minecart._leftSideConnection[index36] = 2;
            Minecart._rightSideConnection[index36] = 0;
            for (var index1 = 0; index1 < 8; ++index1)
                Minecart._tileHeight[index36][index1] = index1 + 1;
            Minecart._trackType[index36] = 2;
            Minecart._boostLeft[index36] = true;
            Minecart._texturePosition[index36] = new Vector2(5f, 3f);
            var index37 = index36 + 1;
            Minecart._leftSideConnection[index37] = 0;
            Minecart._rightSideConnection[index37] = 2;
            for (var index1 = 0; index1 < 8; ++index1)
                Minecart._tileHeight[index37][index1] = 8 - index1;
            Minecart._trackType[index37] = 2;
            Minecart._boostLeft[index37] = true;
            Minecart._texturePosition[index37] = new Vector2(6f, 3f);
            var index38 = index37 + 1;
            Minecart._leftSideConnection[index38] = 2;
            Minecart._rightSideConnection[index38] = 0;
            for (var index1 = 0; index1 < 8; ++index1)
                Minecart._tileHeight[index38][index1] = index1 + 1;
            Minecart._trackType[index38] = 2;
            Minecart._boostLeft[index38] = false;
            Minecart._texturePosition[index38] = new Vector2(7f, 3f);
            var num1 = index38 + 1;
            Minecart._texturePosition[36] = new Vector2(0.0f, 6f);
            Minecart._texturePosition[37] = new Vector2(1f, 6f);
            Minecart._texturePosition[39] = new Vector2(0.0f, 7f);
            Minecart._texturePosition[38] = new Vector2(1f, 7f);
            for (var index1 = 0; index1 < Minecart._texturePosition.Length; ++index1)
                Minecart._texturePosition[index1] = Minecart._texturePosition[index1] * 18f;
            for (var index1 = 0; index1 < Minecart._tileHeight.Length; ++index1)
            {
                var numArray = Minecart._tileHeight[index1];
                for (var index2 = 0; index2 < numArray.Length; ++index2)
                {
                    if (numArray[index2] >= 0)
                        numArray[index2] = (8 - numArray[index2]) * 2;
                }
            }

            var numArray1 = new int[36];
            Minecart._trackSwitchOptions = new int[64][];
            for (var index1 = 0; index1 < 64; ++index1)
            {
                var num2 = 0;
                var num3 = 1;
                while (num3 < 256)
                {
                    if ((index1 & num3) == num3)
                        ++num2;
                    num3 <<= 1;
                }

                var length = 0;
                for (var index2 = 0; index2 < 36; ++index2)
                {
                    numArray1[index2] = -1;
                    var num4 = 0;
                    switch (Minecart._leftSideConnection[index2])
                    {
                        case 0:
                            num4 |= 1;
                            break;
                        case 1:
                            num4 |= 2;
                            break;
                        case 2:
                            num4 |= 4;
                            break;
                    }

                    switch (Minecart._rightSideConnection[index2])
                    {
                        case 0:
                            num4 |= 8;
                            break;
                        case 1:
                            num4 |= 16;
                            break;
                        case 2:
                            num4 |= 32;
                            break;
                    }

                    if (num2 < 2)
                    {
                        if (index1 != num4)
                            continue;
                    }
                    else if (num4 == 0 || (index1 & num4) != num4)
                        continue;

                    numArray1[index2] = index2;
                    ++length;
                }

                if (length != 0)
                {
                    var numArray2 = new int[length];
                    var index2 = 0;
                    for (var index39 = 0; index39 < 36; ++index39)
                    {
                        if (numArray1[index39] != -1)
                        {
                            numArray2[index2] = numArray1[index39];
                            ++index2;
                        }
                    }

                    Minecart._trackSwitchOptions[index1] = numArray2;
                }
            }

            Minecart._firstPressureFrame = (short) -1;
            Minecart._firstLeftBoostFrame = (short) -1;
            Minecart._firstRightBoostFrame = (short) -1;
            for (var index1 = 0; index1 < Minecart._trackType.Length; ++index1)
            {
                switch (Minecart._trackType[index1])
                {
                    case 1:
                        if (Minecart._firstPressureFrame == (short) -1)
                        {
                            Minecart._firstPressureFrame = (short) index1;
                            break;
                        }

                        break;
                    case 2:
                        if (Minecart._boostLeft[index1])
                        {
                            if (Minecart._firstLeftBoostFrame == (short) -1)
                            {
                                Minecart._firstLeftBoostFrame = (short) index1;
                                break;
                            }

                            break;
                        }

                        if (Minecart._firstRightBoostFrame == (short) -1)
                        {
                            Minecart._firstRightBoostFrame = (short) index1;
                            break;
                        }

                        break;
                }
            }
        }

        public static BitsByte TrackCollision(ref Vector2 Position, ref Vector2 Velocity, ref Vector2 lastBoost,
            int Width, int Height, bool followDown, bool followUp, int fallStart, bool trackOnly,
            Action<Vector2> MinecartDust)
        {
            if (followDown && followUp)
            {
                followDown = false;
                followUp = false;
            }

            var vector2_1 = new Vector2((float) (Width / 2) - 25f, (float) (Height / 2));
            var vector2_2 = Position + new Vector2((float) (Width / 2) - 25f, (float) (Height / 2)) +
                                Minecart._trackMagnetOffset;
            var vector2_3 = Velocity;
            var num1 = vector2_3.Length();
            vector2_3.Normalize();
            var vector2_4 = vector2_2;
            var tileTrack = (Tile) null;
            var flag1 = false;
            var flag2 = true;
            var num2 = -1;
            var num3 = -1;
            var num4 = -1;
            var trackState1 = Minecart.TrackState.NoTrack;
            var flag3 = false;
            var flag4 = false;
            var flag5 = false;
            var flag6 = false;
            var vector2_5 = Vector2.Zero;
            var vector2_6 = Vector2.Zero;
            var bitsByte = new BitsByte();
            while (true)
            {
                var index1 = (int) ((double) vector2_4.X / 16.0);
                var index2 = (int) ((double) vector2_4.Y / 16.0);
                var index3 = (int) vector2_4.X % 16 / 2;
                if (flag2)
                    num4 = index3;
                var flag7 = index3 != num4;
                if ((trackState1 == Minecart.TrackState.OnBack || trackState1 == Minecart.TrackState.OnTrack ||
                     trackState1 == Minecart.TrackState.OnFront) && index1 != num2)
                {
                    var index4 = trackState1 != Minecart.TrackState.OnBack
                        ? (int) tileTrack.FrontTrack()
                        : (int) tileTrack.BackTrack();
                    switch ((double) vector2_3.X >= 0.0
                        ? Minecart._rightSideConnection[index4]
                        : Minecart._leftSideConnection[index4])
                    {
                        case 0:
                            --index2;
                            vector2_4.Y -= 2f;
                            break;
                        case 2:
                            ++index2;
                            vector2_4.Y += 2f;
                            break;
                    }
                }

                var trackState2 = Minecart.TrackState.NoTrack;
                var flag8 = false;
                if (index1 != num2 || index2 != num3)
                {
                    if (flag2)
                        flag2 = false;
                    else
                        flag8 = true;
                    tileTrack = Main.tile[index1, index2];
                    if (tileTrack == null)
                    {
                        tileTrack = new Tile();
                        Main.tile[index1, index2] = tileTrack;
                    }

                    flag1 = tileTrack.nactive() && tileTrack.type == (ushort) 314;
                }

                if (flag1)
                {
                    var trackState3 = Minecart.TrackState.NoTrack;
                    var index4 = (int) tileTrack.FrontTrack();
                    var index5 = (int) tileTrack.BackTrack();
                    var num5 = Minecart._tileHeight[index4][index3];
                    switch (num5)
                    {
                        case -4:
                            if (trackState1 == Minecart.TrackState.OnFront)
                            {
                                if (trackOnly)
                                {
                                    vector2_4 -= vector2_6;
                                    num1 = 0.0f;
                                    trackState2 = Minecart.TrackState.OnFront;
                                    flag6 = true;
                                    break;
                                }

                                trackState2 = Minecart.TrackState.NoTrack;
                                flag5 = true;
                                break;
                            }

                            break;
                        case -3:
                            if (trackState1 == Minecart.TrackState.OnFront)
                            {
                                trackState1 = Minecart.TrackState.NoTrack;
                                var matrix = (double) Velocity.X <= 0.0
                                    ? (Minecart._rightSideConnection[index4] != 2
                                        ? Matrix.CreateRotationZ(-0.7853982f)
                                        : Matrix.CreateRotationZ(0.7853982f))
                                    : (Minecart._leftSideConnection[index4] != 2
                                        ? Matrix.CreateRotationZ(0.7853982f)
                                        : Matrix.CreateRotationZ(-0.7853982f));
                                vector2_5 = Vector2.Transform(new Vector2(Velocity.X, 0.0f), matrix);
                                vector2_5.X = Velocity.X;
                                flag4 = true;
                                num1 = 0.0f;
                                break;
                            }

                            break;
                        case -2:
                            if (trackState1 == Minecart.TrackState.OnFront)
                            {
                                if (trackOnly)
                                {
                                    vector2_4 -= vector2_6;
                                    num1 = 0.0f;
                                    trackState2 = Minecart.TrackState.OnFront;
                                    flag6 = true;
                                    break;
                                }

                                if ((double) vector2_3.X < 0.0)
                                {
                                    var num6 = (float) (index1 * 16 + (index3 + 1) * 2) - vector2_4.X;
                                    vector2_4.X += num6;
                                    num1 += num6 / vector2_3.X;
                                }

                                vector2_3.X = -vector2_3.X;
                                bitsByte[1] = true;
                                trackState2 = Minecart.TrackState.OnFront;
                                break;
                            }

                            break;
                        case -1:
                            if (trackState1 == Minecart.TrackState.OnFront)
                            {
                                vector2_4 -= vector2_6;
                                num1 = 0.0f;
                                trackState2 = Minecart.TrackState.OnFront;
                                flag6 = true;
                                break;
                            }

                            break;
                        default:
                            var num7 = (float) (index2 * 16 + num5);
                            if (index1 != num2 && trackState1 == Minecart.TrackState.NoTrack &&
                                ((double) vector2_4.Y > (double) num7 && (double) vector2_4.Y - (double) num7 < 2.0))
                            {
                                flag8 = false;
                                trackState1 = Minecart.TrackState.AboveFront;
                            }

                            var trackState4 = (double) vector2_4.Y >= (double) num7
                                ? ((double) vector2_4.Y <= (double) num7
                                    ? Minecart.TrackState.OnTrack
                                    : Minecart.TrackState.BelowTrack)
                                : Minecart.TrackState.AboveTrack;
                            if (index5 != -1)
                            {
                                var num6 = (float) (index2 * 16 + Minecart._tileHeight[index5][index3]);
                                trackState3 = (double) vector2_4.Y >= (double) num6
                                    ? ((double) vector2_4.Y <= (double) num6
                                        ? Minecart.TrackState.OnTrack
                                        : Minecart.TrackState.BelowTrack)
                                    : Minecart.TrackState.AboveTrack;
                            }

                            switch (trackState4)
                            {
                                case Minecart.TrackState.AboveTrack:
                                    switch (trackState3)
                                    {
                                        case Minecart.TrackState.AboveTrack:
                                            trackState2 = Minecart.TrackState.AboveTrack;
                                            break;
                                        case Minecart.TrackState.OnTrack:
                                            trackState2 = Minecart.TrackState.OnBack;
                                            break;
                                        case Minecart.TrackState.BelowTrack:
                                            trackState2 = Minecart.TrackState.AboveFront;
                                            break;
                                        default:
                                            trackState2 = Minecart.TrackState.AboveFront;
                                            break;
                                    }

                                    //Fix By GScience
                                    break;
                                case Minecart.TrackState.OnTrack:
                                    trackState2 = trackState3 != Minecart.TrackState.OnTrack
                                        ? Minecart.TrackState.OnFront
                                        : Minecart.TrackState.OnTrack;
                                    break;
                                case Minecart.TrackState.BelowTrack:
                                    switch (trackState3)
                                    {
                                        case Minecart.TrackState.AboveTrack:
                                            trackState2 = Minecart.TrackState.AboveBack;
                                            break;
                                        case Minecart.TrackState.OnTrack:
                                            trackState2 = Minecart.TrackState.OnBack;
                                            break;
                                        case Minecart.TrackState.BelowTrack:
                                            trackState2 = Minecart.TrackState.BelowTrack;
                                            break;
                                        default:
                                            trackState2 = Minecart.TrackState.BelowTrack;
                                            break;
                                    }

                                    //Fix By GScience
                                    break;
                            }

                            break;
                    }
                }

                if (!flag8)
                {
                    if (trackState1 != trackState2)
                    {
                        var flag9 = false;
                        if (flag7 || (double) vector2_3.Y > 0.0)
                        {
                            switch (trackState1)
                            {
                                case Minecart.TrackState.AboveTrack:
                                    switch (trackState2)
                                    {
                                        case Minecart.TrackState.AboveTrack:
                                            trackState2 = Minecart.TrackState.OnTrack;
                                            break;
                                        case Minecart.TrackState.AboveFront:
                                            trackState2 = Minecart.TrackState.OnBack;
                                            break;
                                        case Minecart.TrackState.AboveBack:
                                            trackState2 = Minecart.TrackState.OnFront;
                                            break;
                                    }

                                    break;
                                case Minecart.TrackState.OnTrack:
                                    var num5 = Minecart._tileHeight[(int) tileTrack.FrontTrack()][index3];
                                    var num6 = Minecart._tileHeight[(int) tileTrack.BackTrack()][index3];
                                    trackState2 = !followDown
                                        ? (!followUp
                                            ? Minecart.TrackState.OnFront
                                            : (num5 >= num6 ? Minecart.TrackState.OnBack : Minecart.TrackState.OnFront))
                                        : (num5 >= num6 ? Minecart.TrackState.OnFront : Minecart.TrackState.OnBack);
                                    flag9 = true;
                                    break;
                                case Minecart.TrackState.AboveFront:
                                    if (trackState2 == Minecart.TrackState.BelowTrack)
                                    {
                                        trackState2 = Minecart.TrackState.OnFront;
                                        break;
                                    }

                                    break;
                                case Minecart.TrackState.AboveBack:
                                    if (trackState2 == Minecart.TrackState.BelowTrack)
                                    {
                                        trackState2 = Minecart.TrackState.OnBack;
                                        break;
                                    }

                                    break;
                                case Minecart.TrackState.OnFront:
                                    trackState2 = Minecart.TrackState.OnFront;
                                    flag9 = true;
                                    break;
                                case Minecart.TrackState.OnBack:
                                    trackState2 = Minecart.TrackState.OnBack;
                                    flag9 = true;
                                    break;
                            }

                            var index4 = -1;
                            switch (trackState2)
                            {
                                case Minecart.TrackState.OnTrack:
                                case Minecart.TrackState.OnFront:
                                    index4 = (int) tileTrack.FrontTrack();
                                    break;
                                case Minecart.TrackState.OnBack:
                                    index4 = (int) tileTrack.BackTrack();
                                    break;
                            }

                            if (index4 != -1)
                            {
                                if (!flag9 && (double) Velocity.Y > (double) Player.defaultGravity)
                                {
                                    var num7 = (int) ((double) Position.Y / 16.0);
                                    if (fallStart < num7 - 1)
                                    {
                                        Main.PlaySound(SoundID.Item53, (int) Position.X + Width / 2,
                                            (int) Position.Y + Height / 2);
                                        Minecart.WheelSparks(MinecartDust, Position, Width, Height, 10);
                                    }
                                }

                                if (trackState1 == Minecart.TrackState.AboveFront && Minecart._trackType[index4] == 1)
                                    flag3 = true;
                                vector2_3.Y = 0.0f;
                                vector2_4.Y = (float) (index2 * 16 + Minecart._tileHeight[index4][index3]);
                            }
                        }
                    }
                }
                else if (trackState2 == Minecart.TrackState.OnFront || trackState2 == Minecart.TrackState.OnBack ||
                         trackState2 == Minecart.TrackState.OnTrack)
                {
                    if (flag1 && Minecart._trackType[(int) tileTrack.FrontTrack()] == 1)
                        flag3 = true;
                    vector2_3.Y = 0.0f;
                }

                if (trackState2 == Minecart.TrackState.OnFront)
                {
                    var index4 = (int) tileTrack.FrontTrack();
                    if (Minecart._trackType[index4] == 2 && (double) lastBoost.X == 0.0 && (double) lastBoost.Y == 0.0)
                    {
                        lastBoost = new Vector2((float) index1, (float) index2);
                        if (Minecart._boostLeft[index4])
                            bitsByte[4] = true;
                        else
                            bitsByte[5] = true;
                    }
                }

                num4 = index3;
                trackState1 = trackState2;
                num2 = index1;
                num3 = index2;
                if ((double) num1 > 0.0)
                {
                    var num5 = vector2_4.X % 2f;
                    var num6 = vector2_4.Y % 2f;
                    var num7 = 3f;
                    var num8 = 3f;
                    if ((double) vector2_3.X < 0.0)
                        num7 = num5 + 0.125f;
                    else if ((double) vector2_3.X > 0.0)
                        num7 = 2f - num5;
                    if ((double) vector2_3.Y < 0.0)
                        num8 = num6 + 0.125f;
                    else if ((double) vector2_3.Y > 0.0)
                        num8 = 2f - num6;
                    if ((double) num7 != 3.0 || (double) num8 != 3.0)
                    {
                        var num9 = Math.Abs(num7 / vector2_3.X);
                        var num10 = Math.Abs(num8 / vector2_3.Y);
                        var num11 = (double) num9 < (double) num10 ? num9 : num10;
                        if ((double) num11 > (double) num1)
                        {
                            vector2_6 = vector2_3 * num1;
                            num1 = 0.0f;
                        }
                        else
                        {
                            vector2_6 = vector2_3 * num11;
                            num1 -= num11;
                        }

                        vector2_4 += vector2_6;
                    }
                    else
                        goto label_98;
                }
                else
                    break;
            }

            if ((double) lastBoost.X != (double) num2 || (double) lastBoost.Y != (double) num3)
                lastBoost = Vector2.Zero;
            label_98:
            if (flag3)
                bitsByte[3] = true;
            if (flag5)
            {
                Velocity.X = vector2_4.X - vector2_2.X;
                Velocity.Y = Player.defaultGravity;
            }
            else if (flag4)
            {
                bitsByte[2] = true;
                Velocity = vector2_5;
            }
            else if (bitsByte[1])
            {
                Velocity.X = -Velocity.X;
                Position.X = vector2_4.X - Minecart._trackMagnetOffset.X - vector2_1.X - Velocity.X;
                if ((double) vector2_3.Y == 0.0)
                    Velocity.Y = 0.0f;
            }
            else
            {
                if (flag6)
                    Velocity.X = vector2_4.X - vector2_2.X;
                if ((double) vector2_3.Y == 0.0)
                    Velocity.Y = 0.0f;
            }

            Position.Y += vector2_4.Y - vector2_2.Y - Velocity.Y;
            Position.Y = (float) Math.Round((double) Position.Y, 2);
            switch (trackState1)
            {
                case Minecart.TrackState.OnTrack:
                case Minecart.TrackState.OnFront:
                case Minecart.TrackState.OnBack:
                    bitsByte[0] = true;
                    break;
            }

            return bitsByte;
        }

        public static bool FrameTrack(int i, int j, bool pound, bool mute = false)
        {
            var index1 = 0;
            var tileTrack = Main.tile[i, j];
            if (tileTrack == null)
            {
                tileTrack = new Tile();
                Main.tile[i, j] = tileTrack;
            }

            if (mute && tileTrack.type != (ushort) 314)
                return false;
            if (Main.tile[i - 1, j - 1] != null && Main.tile[i - 1, j - 1].type == (ushort) 314)
                ++index1;
            if (Main.tile[i - 1, j] != null && Main.tile[i - 1, j].type == (ushort) 314)
                index1 += 2;
            if (Main.tile[i - 1, j + 1] != null && Main.tile[i - 1, j + 1].type == (ushort) 314)
                index1 += 4;
            if (Main.tile[i + 1, j - 1] != null && Main.tile[i + 1, j - 1].type == (ushort) 314)
                index1 += 8;
            if (Main.tile[i + 1, j] != null && Main.tile[i + 1, j].type == (ushort) 314)
                index1 += 16;
            if (Main.tile[i + 1, j + 1] != null && Main.tile[i + 1, j + 1].type == (ushort) 314)
                index1 += 32;
            var index2 = (int) tileTrack.FrontTrack();
            var num1 = (int) tileTrack.BackTrack();
            if (Minecart._trackType == null)
                return false;
            var num2 = index2 < 0 || index2 >= Minecart._trackType.Length ? 0 : Minecart._trackType[index2];
            var index3 = -1;
            var index4 = -1;
            var trackSwitchOption = Minecart._trackSwitchOptions[index1];
            if (trackSwitchOption == null)
            {
                if (pound)
                    return false;
                tileTrack.FrontTrack((short) 0);
                tileTrack.BackTrack((short) -1);
                return false;
            }

            if (!pound)
            {
                var num3 = -1;
                var num4 = -1;
                var flag = false;
                for (var index5 = 0; index5 < trackSwitchOption.Length; ++index5)
                {
                    var index6 = trackSwitchOption[index5];
                    if (num1 == trackSwitchOption[index5])
                        index4 = index5;
                    if (Minecart._trackType[index6] == num2)
                    {
                        if (Minecart._leftSideConnection[index6] == -1 || Minecart._rightSideConnection[index6] == -1)
                        {
                            if (index2 == trackSwitchOption[index5])
                            {
                                index3 = index5;
                                flag = true;
                            }

                            if (num3 == -1)
                                num3 = index5;
                        }
                        else
                        {
                            if (index2 == trackSwitchOption[index5])
                            {
                                index3 = index5;
                                flag = false;
                            }

                            if (num4 == -1)
                                num4 = index5;
                        }
                    }
                }

                if (num4 != -1)
                {
                    if (index3 == -1 || flag)
                        index3 = num4;
                }
                else
                {
                    if (index3 == -1)
                    {
                        if (num2 == 2 || num2 == 1)
                            return false;
                        index3 = num3;
                    }

                    index4 = -1;
                }
            }
            else
            {
                for (var index5 = 0; index5 < trackSwitchOption.Length; ++index5)
                {
                    if (index2 == trackSwitchOption[index5])
                        index3 = index5;
                    if (num1 == trackSwitchOption[index5])
                        index4 = index5;
                }

                var num3 = 0;
                var num4 = 0;
                for (var index5 = 0; index5 < trackSwitchOption.Length; ++index5)
                {
                    if (Minecart._trackType[trackSwitchOption[index5]] == num2)
                    {
                        if (Minecart._leftSideConnection[trackSwitchOption[index5]] == -1 ||
                            Minecart._rightSideConnection[trackSwitchOption[index5]] == -1)
                            ++num4;
                        else
                            ++num3;
                    }
                }

                if (num3 < 2 && num4 < 2)
                    return false;
                var flag1 = num3 == 0;
                var flag2 = false;
                if (!flag1)
                {
                    while (!flag2)
                    {
                        ++index4;
                        if (index4 >= trackSwitchOption.Length)
                        {
                            index4 = -1;
                            break;
                        }

                        if ((Minecart._leftSideConnection[trackSwitchOption[index4]] !=
                             Minecart._leftSideConnection[trackSwitchOption[index3]] ||
                             Minecart._rightSideConnection[trackSwitchOption[index4]] !=
                             Minecart._rightSideConnection[trackSwitchOption[index3]]) &&
                            (Minecart._trackType[trackSwitchOption[index4]] == num2 &&
                             Minecart._leftSideConnection[trackSwitchOption[index4]] != -1) &&
                            Minecart._rightSideConnection[trackSwitchOption[index4]] != -1)
                            flag2 = true;
                    }
                }

                if (!flag2)
                {
                    do
                    {
                        ++index3;
                        if (index3 >= trackSwitchOption.Length)
                        {
                            index3 = -1;
                            do
                            {
                                ++index3;
                            } while (Minecart._trackType[trackSwitchOption[index3]] != num2 ||
                                     (Minecart._leftSideConnection[trackSwitchOption[index3]] == -1
                                         ? 1
                                         : (Minecart._rightSideConnection[trackSwitchOption[index3]] == -1 ? 1 : 0)) !=
                                     (flag1 ? 1 : 0));

                            break;
                        }
                    } while (Minecart._trackType[trackSwitchOption[index3]] != num2 ||
                             (Minecart._leftSideConnection[trackSwitchOption[index3]] == -1
                                 ? 1
                                 : (Minecart._rightSideConnection[trackSwitchOption[index3]] == -1 ? 1 : 0)) !=
                             (flag1 ? 1 : 0));
                }
            }

            var flag3 = false;
            switch (index3)
            {
                case -2:
                    if ((int) tileTrack.FrontTrack() != (int) Minecart._firstPressureFrame)
                    {
                        flag3 = true;
                        break;
                    }

                    break;
                case -1:
                    if (tileTrack.FrontTrack() != (short) 0)
                    {
                        flag3 = true;
                        break;
                    }

                    break;
                default:
                    if ((int) tileTrack.FrontTrack() != trackSwitchOption[index3])
                    {
                        flag3 = true;
                        break;
                    }

                    break;
            }

            if (index4 == -1)
            {
                if (tileTrack.BackTrack() != (short) -1)
                    flag3 = true;
            }
            else if ((int) tileTrack.BackTrack() != trackSwitchOption[index4])
                flag3 = true;

            switch (index3)
            {
                case -2:
                    tileTrack.FrontTrack(Minecart._firstPressureFrame);
                    break;
                case -1:
                    tileTrack.FrontTrack((short) 0);
                    break;
                default:
                    tileTrack.FrontTrack((short) trackSwitchOption[index3]);
                    break;
            }

            if (index4 == -1)
                tileTrack.BackTrack((short) -1);
            else
                tileTrack.BackTrack((short) trackSwitchOption[index4]);
            if (pound && flag3 && !mute)
                WorldGen.KillTile(i, j, true, false, false);
            return true;
        }

        public static bool GetOnTrack(int tileX, int tileY, ref Vector2 Position, int Width, int Height)
        {
            var tile = Main.tile[tileX, tileY];
            if (tile.type != (ushort) 314)
                return false;
            var vector2_1 = new Vector2((float) (Width / 2) - 25f, (float) (Height / 2));
            var vector2_2 = Position + vector2_1 + Minecart._trackMagnetOffset;
            var num1 = (int) vector2_2.X % 16 / 2;
            var num2 = -1;
            var num3 = 0;
            for (var index = num1; index < 8; ++index)
            {
                num3 = Minecart._tileHeight[(int) tile.frameX][index];
                if (num3 >= 0)
                {
                    num2 = index;
                    break;
                }
            }

            if (num2 == -1)
            {
                for (var index = num1 - 1; index >= 0; --index)
                {
                    num3 = Minecart._tileHeight[(int) tile.frameX][index];
                    if (num3 >= 0)
                    {
                        num2 = index;
                        break;
                    }
                }
            }

            if (num2 == -1)
                return false;
            vector2_2.X = (float) (tileX * 16 + num2 * 2);
            vector2_2.Y = (float) (tileY * 16 + num3);
            vector2_2 -= Minecart._trackMagnetOffset;
            vector2_2 -= vector2_1;
            Position = vector2_2;
            return true;
        }

        public static bool OnTrack(Vector2 Position, int Width, int Height)
        {
            var vector2 = Position + new Vector2((float) (Width / 2) - 25f, (float) (Height / 2)) +
                              Minecart._trackMagnetOffset;
            var index1 = (int) ((double) vector2.X / 16.0);
            var index2 = (int) ((double) vector2.Y / 16.0);
            if (Main.tile[index1, index2] == null)
                return false;
            return Main.tile[index1, index2].type == (ushort) 314;
        }

        public static float TrackRotation(ref float rotation, Vector2 Position, int Width, int Height, bool followDown,
            bool followUp, Action<Vector2> MinecartDust)
        {
            var Position1 = Position;
            var Position2 = Position;
            var zero = Vector2.Zero;
            var Velocity = new Vector2(-12f, 0.0f);
            Minecart.TrackCollision(ref Position1, ref Velocity, ref zero, Width, Height, followDown, followUp, 0, true,
                MinecartDust);
            var vector2_1 = Position1 + Velocity;
            Velocity = new Vector2(12f, 0.0f);
            Minecart.TrackCollision(ref Position2, ref Velocity, ref zero, Width, Height, followDown, followUp, 0, true,
                MinecartDust);
            var vector2_2 = Position2 + Velocity;
            var num1 = vector2_2.Y - vector2_1.Y;
            var num2 = vector2_2.X - vector2_1.X;
            var num3 = num1 / num2;
            var num4 = vector2_1.Y + (Position.X - vector2_1.X) * num3;
            var num5 = (Position.X - (float) (int) Position.X) * num3;
            rotation = (float) Math.Atan2((double) num1, (double) num2);
            return num4 - Position.Y + num5;
        }

        public static void HitTrackSwitch(Vector2 Position, int Width, int Height)
        {
            var vector2_1 = new Vector2((float) (Width / 2) - 25f, (float) (Height / 2));
            var vector2_2 = Position + new Vector2((float) (Width / 2) - 25f, (float) (Height / 2)) +
                                Minecart._trackMagnetOffset;
            var num = (int) ((double) vector2_2.X / 16.0);
            var j = (int) ((double) vector2_2.Y / 16.0);
            Wiring.HitSwitch(num, j);
            NetMessage.SendData(59, -1, -1, (NetworkText) null, num, (float) j, 0.0f, 0.0f, 0, 0, 0);
        }

        public static void FlipSwitchTrack(int i, int j)
        {
            var tileTrack = Main.tile[i, j];
            short trackID = tileTrack.FrontTrack();
            if (trackID == (short) -1)
                return;
            switch (Minecart._trackType[(int) trackID])
            {
                case 0:
                    if (tileTrack.BackTrack() == (short) -1)
                        break;
                    tileTrack.FrontTrack(tileTrack.BackTrack());
                    tileTrack.BackTrack(trackID);
                    NetMessage.SendTileSquare(-1, i, j, 1, TileChangeType.None);
                    break;
                case 2:
                    Minecart.FrameTrack(i, j, true, true);
                    NetMessage.SendTileSquare(-1, i, j, 1, TileChangeType.None);
                    break;
            }
        }

        public static void TrackColors(int i, int j, Tile trackTile, out int frontColor, out int backColor)
        {
            if (trackTile.type == (ushort) 314)
            {
                frontColor = (int) trackTile.color();
                backColor = frontColor;
                if (trackTile.frameY == (short) -1)
                    return;
                var num1 = Minecart._leftSideConnection[(int) trackTile.frameX];
                var num2 = Minecart._rightSideConnection[(int) trackTile.frameX];
                var num3 = Minecart._leftSideConnection[(int) trackTile.frameY];
                var num4 = Minecart._rightSideConnection[(int) trackTile.frameY];
                var num5 = 0;
                var num6 = 0;
                var num7 = 0;
                var num8 = 0;
                for (var index = 0; index < 4; ++index)
                {
                    int num9;
                    switch (index - 1)
                    {
                        case 0:
                            num9 = num2;
                            break;
                        case 1:
                            num9 = num3;
                            break;
                        case 2:
                            num9 = num4;
                            break;
                        default:
                            num9 = num1;
                            break;
                    }

                    int num10;
                    switch (num9)
                    {
                        case 0:
                            num10 = -1;
                            break;
                        case 1:
                            num10 = 0;
                            break;
                        case 2:
                            num10 = 1;
                            break;
                        default:
                            num10 = 0;
                            break;
                    }

                    var tile = index % 2 != 0 ? Main.tile[i + 1, j + num10] : Main.tile[i - 1, j + num10];
                    var num11 = tile == null || !tile.active() || tile.type != (ushort) 314 ? 0 : (int) tile.color();
                    switch (index)
                    {
                        case 1:
                            num6 = num11;
                            break;
                        case 2:
                            num7 = num11;
                            break;
                        case 3:
                            num8 = num11;
                            break;
                        default:
                            num5 = num11;
                            break;
                    }
                }

                if (num1 == num3)
                {
                    if (num6 != 0)
                        frontColor = num6;
                    else if (num5 != 0)
                        frontColor = num5;
                    if (num8 != 0)
                    {
                        backColor = num8;
                    }
                    else
                    {
                        if (num7 == 0)
                            return;
                        backColor = num7;
                    }
                }
                else if (num2 == num4)
                {
                    if (num5 != 0)
                        frontColor = num5;
                    else if (num6 != 0)
                        frontColor = num6;
                    if (num7 != 0)
                    {
                        backColor = num7;
                    }
                    else
                    {
                        if (num8 == 0)
                            return;
                        backColor = num8;
                    }
                }
                else
                {
                    if (num6 == 0)
                    {
                        if (num5 != 0)
                            frontColor = num5;
                    }
                    else if (num5 != 0)
                        frontColor = num2 <= num1 ? num6 : num5;

                    if (num8 == 0)
                    {
                        if (num7 == 0)
                            return;
                        backColor = num7;
                    }
                    else
                    {
                        if (num7 == 0)
                            return;
                        backColor = num4 <= num3 ? num8 : num7;
                    }
                }
            }
            else
            {
                frontColor = 0;
                backColor = 0;
            }
        }

        public static bool DrawLeftDecoration(int frameID)
        {
            if (frameID < 0 || frameID >= 36)
                return false;
            return Minecart._leftSideConnection[frameID] == 2;
        }

        public static bool DrawRightDecoration(int frameID)
        {
            if (frameID < 0 || frameID >= 36)
                return false;
            return Minecart._rightSideConnection[frameID] == 2;
        }

        public static bool DrawBumper(int frameID)
        {
            if (frameID < 0 || frameID >= 36)
                return false;
            if (Minecart._tileHeight[frameID][0] != -1)
                return Minecart._tileHeight[frameID][7] == -1;
            return true;
        }

        public static bool DrawBouncyBumper(int frameID)
        {
            if (frameID < 0 || frameID >= 36)
                return false;
            if (Minecart._tileHeight[frameID][0] != -2)
                return Minecart._tileHeight[frameID][7] == -2;
            return true;
        }

        public static void PlaceTrack(Tile trackCache, int style)
        {
            trackCache.active(true);
            trackCache.type = (ushort) 314;
            trackCache.frameY = (short) -1;
            switch (style)
            {
                case 0:
                    trackCache.frameX = (short) -1;
                    break;
                case 1:
                    trackCache.frameX = Minecart._firstPressureFrame;
                    break;
                case 2:
                    trackCache.frameX = Minecart._firstLeftBoostFrame;
                    break;
                case 3:
                    trackCache.frameX = Minecart._firstRightBoostFrame;
                    break;
            }
        }

        public static int GetTrackItem(Tile trackCache)
        {
            switch (Minecart._trackType[(int) trackCache.frameX])
            {
                case 0:
                    return 2340;
                case 1:
                    return 2492;
                case 2:
                    return 2739;
                default:
                    return 0;
            }
        }

        public static Rectangle GetSourceRect(int frameID, int animationFrame = 0)
        {
            if (frameID < 0 || frameID >= 40)
                return new Rectangle(0, 0, 0, 0);
            var vector2 = Minecart._texturePosition[frameID];
            var rectangle = new Rectangle((int) vector2.X, (int) vector2.Y, 16, 16);
            if (frameID < 36 && Minecart._trackType[frameID] == 2)
                rectangle.Y += 18 * animationFrame;
            return rectangle;
        }

        public static void WheelSparks(Action<Vector2> DustAction, Vector2 Position, int Width, int Height,
            int sparkCount)
        {
            var vector2_1 = new Vector2((float) (Width / 2) - 25f, (float) (Height / 2));
            var vector2_2 = Position + vector2_1 + Minecart._trackMagnetOffset;
            for (var index = 0; index < sparkCount; ++index)
                DustAction(vector2_2);
        }

        private static short FrontTrack(this Tile tileTrack)
        {
            return tileTrack.frameX;
        }

        private static void FrontTrack(this Tile tileTrack, short trackID)
        {
            tileTrack.frameX = trackID;
        }

        private static short BackTrack(this Tile tileTrack)
        {
            return tileTrack.frameY;
        }

        private static void BackTrack(this Tile tileTrack, short trackID)
        {
            tileTrack.frameY = trackID;
        }

        private enum TrackState
        {
            NoTrack = -1,
            AboveTrack = 0,
            OnTrack = 1,
            BelowTrack = 2,
            AboveFront = 3,
            AboveBack = 4,
            OnFront = 5,
            OnBack = 6,
        }
    }
}