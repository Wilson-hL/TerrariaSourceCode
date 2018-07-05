// Decompiled with JetBrains decompiler
// Type: Terraria.GameInput.LockOnHelper
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ID;

namespace Terraria.GameInput
{
    public class LockOnHelper
    {
        public static LockOnHelper.LockOnMode UseMode = LockOnHelper.LockOnMode.ThreeDS;
        private static bool _canLockOn = false;
        private static List<int> _targets = new List<int>();
        private static int _threeDSTarget = -1;
        private static float[,] _drawProgress = new float[200, 2];
        private const float LOCKON_RANGE = 2000f;
        private const int LOCKON_HOLD_LIFETIME = 40;
        private static bool _enabled;
        private static int _pickedTarget;
        private static int _lifeTimeCounter;
        private static int _lifeTimeArrowDisplay;

        public static void CycleUseModes()
        {
            switch (LockOnHelper.UseMode)
            {
                case LockOnHelper.LockOnMode.FocusTarget:
                    LockOnHelper.UseMode = LockOnHelper.LockOnMode.TargetClosest;
                    break;
                case LockOnHelper.LockOnMode.TargetClosest:
                    LockOnHelper.UseMode = LockOnHelper.LockOnMode.ThreeDS;
                    break;
                case LockOnHelper.LockOnMode.ThreeDS:
                    LockOnHelper.UseMode = LockOnHelper.LockOnMode.TargetClosest;
                    break;
            }
        }

        public static NPC AimedTarget
        {
            get
            {
                if (LockOnHelper._pickedTarget == -1 || LockOnHelper._targets.Count < 1)
                    return (NPC) null;
                return Main.npc[LockOnHelper._targets[LockOnHelper._pickedTarget]];
            }
        }

        public static Vector2 PredictedPosition
        {
            get
            {
                var aimedTarget = LockOnHelper.AimedTarget;
                if (aimedTarget == null)
                    return Vector2.Zero;
                var vec = aimedTarget.Center;
                int index1;
                Vector2 pos;
                if (NPC.GetNPCLocation(LockOnHelper._targets[LockOnHelper._pickedTarget], true, false, out index1,
                    out pos))
                    vec = pos + Main.npc[index1].Distance(Main.player[Main.myPlayer].Center) / 2000f *
                          Main.npc[index1].velocity * 45f;
                var player = Main.player[Main.myPlayer];
                for (var index2 = ItemID.Sets.LockOnAimAbove[player.inventory[player.selectedItem].type];
                    index2 > 0 && (double) vec.Y > 100.0;
                    --index2)
                {
                    var tileCoordinates = vec.ToTileCoordinates();
                    tileCoordinates.Y -= 4;
                    if (WorldGen.InWorld(tileCoordinates.X, tileCoordinates.Y, 10) &&
                        !WorldGen.SolidTile(tileCoordinates.X, tileCoordinates.Y))
                        vec.Y -= 16f;
                    else
                        break;
                }

                var nullable = ItemID.Sets.LockOnAimCompensation[player.inventory[player.selectedItem].type];
                if (nullable.HasValue)
                {
                    vec.Y -= (float) (aimedTarget.height / 2);
                    var v = vec - player.Center;
                    var vector2 = v.SafeNormalize(Vector2.Zero);
                    --vector2.Y;
                    var num = (float) Math.Pow((double) v.Length() / 700.0, 2.0) * 700f;
                    vec.Y += (float) ((double) vector2.Y * (double) num * (double) nullable.Value * 1.0);
                    vec.X += (float) (-(double) vector2.X * (double) num * (double) nullable.Value * 1.0);
                }

                return vec;
            }
        }

        public static void Update()
        {
            LockOnHelper._canLockOn = false;
            if (!PlayerInput.UsingGamepad)
            {
                LockOnHelper.SetActive(false);
            }
            else
            {
                if (--LockOnHelper._lifeTimeArrowDisplay < 0)
                    LockOnHelper._lifeTimeArrowDisplay = 0;
                LockOnHelper.Handle3DSTarget();
                if (PlayerInput.Triggers.JustPressed.LockOn && !PlayerInput.WritingText)
                {
                    LockOnHelper._lifeTimeCounter = 40;
                    LockOnHelper._lifeTimeArrowDisplay = 30;
                    LockOnHelper.HandlePressing();
                }

                if (!LockOnHelper._enabled)
                    return;
                if (LockOnHelper.UseMode == LockOnHelper.LockOnMode.FocusTarget && PlayerInput.Triggers.Current.LockOn)
                {
                    if (LockOnHelper._lifeTimeCounter <= 0)
                    {
                        LockOnHelper.SetActive(false);
                        return;
                    }

                    --LockOnHelper._lifeTimeCounter;
                }

                var aimedTarget = LockOnHelper.AimedTarget;
                if (!LockOnHelper.ValidTarget(aimedTarget))
                    LockOnHelper.SetActive(false);
                if (LockOnHelper.UseMode == LockOnHelper.LockOnMode.TargetClosest)
                {
                    LockOnHelper.SetActive(false);
                    LockOnHelper.SetActive(LockOnHelper.CanEnable());
                }

                if (!LockOnHelper._enabled)
                    return;
                var p = Main.player[Main.myPlayer];
                var predictedPosition = LockOnHelper.PredictedPosition;
                var flag = false;
                if (LockOnHelper.ShouldLockOn(p) &&
                    (ItemID.Sets.LockOnIgnoresCollision[p.inventory[p.selectedItem].type] ||
                     Collision.CanHit(p.Center, 0, 0, predictedPosition, 0, 0) ||
                     (Collision.CanHitLine(p.Center, 0, 0, predictedPosition, 0, 0) ||
                      Collision.CanHit(p.Center, 0, 0, aimedTarget.Center, 0, 0)) ||
                     Collision.CanHitLine(p.Center, 0, 0, aimedTarget.Center, 0, 0)))
                    flag = true;
                if (!flag)
                    return;
                LockOnHelper._canLockOn = true;
            }
        }

        public static void SetUP()
        {
            if (!LockOnHelper._canLockOn)
                return;
            var aimedTarget = LockOnHelper.AimedTarget;
            LockOnHelper.SetLockPosition(
                Main.ReverseGravitySupport(LockOnHelper.PredictedPosition - Main.screenPosition, 0.0f));
        }

        public static void SetDOWN()
        {
            if (!LockOnHelper._canLockOn)
                return;
            LockOnHelper.ResetLockPosition();
        }

        private static bool ShouldLockOn(Player p)
        {
            return p.inventory[p.selectedItem].type != 496;
        }

        public static void Toggle(bool forceOff = false)
        {
            LockOnHelper._lifeTimeCounter = 40;
            LockOnHelper._lifeTimeArrowDisplay = 30;
            LockOnHelper.HandlePressing();
            if (!forceOff)
                return;
            LockOnHelper._enabled = false;
        }

        public static bool Enabled
        {
            get { return LockOnHelper._enabled; }
        }

        private static void Handle3DSTarget()
        {
            LockOnHelper._threeDSTarget = -1;
            if (LockOnHelper.UseMode != LockOnHelper.LockOnMode.ThreeDS || !PlayerInput.UsingGamepad)
                return;
            var t1_1 = new List<int>();
            var t1_2 = -1;
            Utils.Swap<List<int>>(ref t1_1, ref LockOnHelper._targets);
            Utils.Swap<int>(ref t1_2, ref LockOnHelper._pickedTarget);
            LockOnHelper.RefreshTargets(Main.MouseWorld, 2000f);
            LockOnHelper.GetClosestTarget(Main.MouseWorld);
            Utils.Swap<List<int>>(ref t1_1, ref LockOnHelper._targets);
            Utils.Swap<int>(ref t1_2, ref LockOnHelper._pickedTarget);
            if (t1_2 >= 0)
                LockOnHelper._threeDSTarget = t1_1[t1_2];
            t1_1.Clear();
        }

        private static void HandlePressing()
        {
            switch (LockOnHelper.UseMode)
            {
                case LockOnHelper.LockOnMode.TargetClosest:
                    LockOnHelper.SetActive(!LockOnHelper._enabled);
                    break;
                case LockOnHelper.LockOnMode.ThreeDS:
                    if (!LockOnHelper._enabled)
                    {
                        LockOnHelper.SetActive(true);
                        break;
                    }

                    LockOnHelper.CycleTargetThreeDS();
                    break;
                default:
                    if (!LockOnHelper._enabled)
                    {
                        LockOnHelper.SetActive(true);
                        break;
                    }

                    LockOnHelper.CycleTargetFocus();
                    break;
            }
        }

        private static void CycleTargetFocus()
        {
            var target = LockOnHelper._targets[LockOnHelper._pickedTarget];
            LockOnHelper.RefreshTargets(Main.MouseWorld, 2000f);
            if (LockOnHelper._targets.Count < 1 ||
                LockOnHelper._targets.Count == 1 && target == LockOnHelper._targets[0])
            {
                LockOnHelper.SetActive(false);
            }
            else
            {
                LockOnHelper._pickedTarget = 0;
                for (var index = 0; index < LockOnHelper._targets.Count; ++index)
                {
                    if (LockOnHelper._targets[index] > target)
                    {
                        LockOnHelper._pickedTarget = index;
                        break;
                    }
                }
            }
        }

        private static void CycleTargetThreeDS()
        {
            var target = LockOnHelper._targets[LockOnHelper._pickedTarget];
            LockOnHelper.RefreshTargets(Main.MouseWorld, 2000f);
            LockOnHelper.GetClosestTarget(Main.MouseWorld);
            if (LockOnHelper._targets.Count >= 1 &&
                (LockOnHelper._targets.Count != 1 || target != LockOnHelper._targets[0]) &&
                target != LockOnHelper._targets[LockOnHelper._pickedTarget])
                return;
            LockOnHelper.SetActive(false);
        }

        private static bool CanEnable()
        {
            return !Main.player[Main.myPlayer].dead;
        }

        private static void SetActive(bool on)
        {
            if (on)
            {
                if (!LockOnHelper.CanEnable())
                    return;
                LockOnHelper.RefreshTargets(Main.MouseWorld, 2000f);
                LockOnHelper.GetClosestTarget(Main.MouseWorld);
                if (LockOnHelper._pickedTarget < 0)
                    return;
                LockOnHelper._enabled = true;
            }
            else
            {
                LockOnHelper._enabled = false;
                LockOnHelper._targets.Clear();
                LockOnHelper._lifeTimeCounter = 0;
            }
        }

        private static void RefreshTargets(Vector2 position, float radius)
        {
            LockOnHelper._targets.Clear();
            var rectangle = Utils.CenteredRectangle(Main.player[Main.myPlayer].Center, new Vector2(1920f, 1200f));
            var center = Main.player[Main.myPlayer].Center;
            var vector2 = Main.player[Main.myPlayer].DirectionTo(Main.MouseWorld);
            for (var index = 0; index < Main.npc.Length; ++index)
            {
                var n = Main.npc[index];
                if (LockOnHelper.ValidTarget(n) && (double) n.Distance(position) <= (double) radius &&
                    rectangle.Intersects(n.Hitbox) &&
                    ((double) Lighting.GetSubLight(n.Center).Length() / 3.0 >= 0.00999999977648258 &&
                     (LockOnHelper.UseMode != LockOnHelper.LockOnMode.ThreeDS ||
                      (double) Vector2.Dot(n.DirectionFrom(center), vector2) >= 0.649999976158142)))
                    LockOnHelper._targets.Add(index);
            }
        }

        private static void GetClosestTarget(Vector2 position)
        {
            LockOnHelper._pickedTarget = -1;
            var num1 = -1f;
            if (LockOnHelper.UseMode == LockOnHelper.LockOnMode.ThreeDS)
            {
                var center = Main.player[Main.myPlayer].Center;
                var vector2 = Main.player[Main.myPlayer].DirectionTo(Main.MouseWorld);
                for (var index = 0; index < LockOnHelper._targets.Count; ++index)
                {
                    var target = LockOnHelper._targets[index];
                    var n = Main.npc[target];
                    var num2 = Vector2.Dot(n.DirectionFrom(center), vector2);
                    if (LockOnHelper.ValidTarget(n) &&
                        (LockOnHelper._pickedTarget == -1 || (double) num2 > (double) num1))
                    {
                        LockOnHelper._pickedTarget = index;
                        num1 = num2;
                    }
                }
            }
            else
            {
                for (var index = 0; index < LockOnHelper._targets.Count; ++index)
                {
                    var target = LockOnHelper._targets[index];
                    var n = Main.npc[target];
                    if (LockOnHelper.ValidTarget(n) &&
                        (LockOnHelper._pickedTarget == -1 || (double) n.Distance(position) < (double) num1))
                    {
                        LockOnHelper._pickedTarget = index;
                        num1 = n.Distance(position);
                    }
                }
            }
        }

        private static bool ValidTarget(NPC n)
        {
            return n != null && n.active && (!n.dontTakeDamage && !n.friendly) &&
                   (!n.townNPC && n.life >= 1 && !n.immortal) && (n.aiStyle != 25 || (double) n.ai[0] != 0.0);
        }

        private static void SetLockPosition(Vector2 position)
        {
            PlayerInput.LockOnCachePosition();
            Main.mouseX = PlayerInput.MouseX = (int) position.X;
            Main.mouseY = PlayerInput.MouseY = (int) position.Y;
        }

        private static void ResetLockPosition()
        {
            PlayerInput.LockOnUnCachePosition();
            Main.mouseX = PlayerInput.MouseX;
            Main.mouseY = PlayerInput.MouseY;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (Main.gameMenu)
                return;
            var lockOnCursorTexture = Main.LockOnCursorTexture;
            var r1 = new Rectangle(0, 0, lockOnCursorTexture.Width, 12);
            var r2 = new Rectangle(0, 16, lockOnCursorTexture.Width, 12);
            var color1 = Main.OurFavoriteColor.MultiplyRGBA(new Color(0.75f, 0.75f, 0.75f, 1f));
            color1.A = (byte) 220;
            var favoriteColor = Main.OurFavoriteColor;
            favoriteColor.A = (byte) 220;
            var num1 = (float) (0.939999997615814 +
                                  Math.Sin((double) Main.GlobalTime * 6.28318548202515) * 0.0599999986588955);
            favoriteColor *= num1;
            var t1 = color1 * num1;
            Utils.Swap<Color>(ref t1, ref favoriteColor);
            var color2 = t1.MultiplyRGBA(new Color(0.8f, 0.8f, 0.8f, 0.8f));
            var color3 = t1.MultiplyRGBA(new Color(0.8f, 0.8f, 0.8f, 0.8f));
            var gravDir = Main.player[Main.myPlayer].gravDir;
            var num2 = 1f;
            var num3 = 0.1f;
            var num4 = 0.8f;
            var num5 = 1f;
            var num6 = 10f;
            var num7 = 10f;
            var flag = false;
            for (var i = 0; i < LockOnHelper._drawProgress.GetLength(0); ++i)
            {
                var num8 = 0;
                if (LockOnHelper._pickedTarget != -1 && LockOnHelper._targets.Count > 0 &&
                    i == LockOnHelper._targets[LockOnHelper._pickedTarget])
                    num8 = 2;
                else if (flag && LockOnHelper._targets.Contains(i) ||
                         LockOnHelper.UseMode == LockOnHelper.LockOnMode.ThreeDS && LockOnHelper._threeDSTarget == i)
                    num8 = 1;
                LockOnHelper._drawProgress[i, 0] =
                    MathHelper.Clamp(LockOnHelper._drawProgress[i, 0] + (num8 == 1 ? num3 : -num3), 0.0f, 1f);
                LockOnHelper._drawProgress[i, 1] =
                    MathHelper.Clamp(LockOnHelper._drawProgress[i, 1] + (num8 == 2 ? num3 : -num3), 0.0f, 1f);
                var num9 = LockOnHelper._drawProgress[i, 0];
                if ((double) num9 > 0.0)
                {
                    var num10 = (float) (1.0 - (double) num9 * (double) num9);
                    var position = Main.ReverseGravitySupport(
                        Main.npc[i].Top + new Vector2(0.0f, (float) (-(double) num7 - (double) num10 * (double) num6)) -
                        Main.screenPosition, (float) Main.npc[i].height);
                    spriteBatch.Draw(lockOnCursorTexture, position, new Rectangle?(r1), color2 * num9, 0.0f,
                        r1.Size() / 2f, new Vector2(0.58f, 1f) * num2 * num4 * (1f + num9) / 2f, SpriteEffects.None,
                        0.0f);
                    spriteBatch.Draw(lockOnCursorTexture, position, new Rectangle?(r2), color3 * num9 * num9, 0.0f,
                        r2.Size() / 2f, new Vector2(0.58f, 1f) * num2 * num4 * (1f + num9) / 2f, SpriteEffects.None,
                        0.0f);
                }

                var num11 = LockOnHelper._drawProgress[i, 1];
                if ((double) num11 > 0.0)
                {
                    var num10 = Main.npc[i].width;
                    if (Main.npc[i].height > num10)
                        num10 = Main.npc[i].height;
                    var num12 = num10 + 20;
                    if ((double) num12 < 70.0)
                        num5 *= (float) num12 / 70f;
                    var num13 = 3f;
                    var vector2_1 = Main.npc[i].Center;
                    int index1;
                    Vector2 pos;
                    if (LockOnHelper._targets.Count >= 0 && LockOnHelper._pickedTarget >= 0 &&
                        (LockOnHelper._pickedTarget < LockOnHelper._targets.Count &&
                         i == LockOnHelper._targets[LockOnHelper._pickedTarget]) &&
                        NPC.GetNPCLocation(i, true, false, out index1, out pos))
                        vector2_1 = pos;
                    for (var index2 = 0; (double) index2 < (double) num13; ++index2)
                    {
                        var num14 = (float) (6.28318548202515 / (double) num13 * (double) index2 +
                                               (double) Main.GlobalTime * 6.28318548202515 * 0.25);
                        var vector2_2 =
                            new Vector2(0.0f, (float) (num12 / 2)).RotatedBy((double) num14, new Vector2());
                        var position =
                            Main.ReverseGravitySupport(vector2_1 + vector2_2 - Main.screenPosition, 0.0f);
                        var rotation = (float) ((double) num14 * ((double) gravDir == 1.0 ? 1.0 : -1.0) +
                                                  3.14159274101257 * ((double) gravDir == 1.0 ? 1.0 : 0.0));
                        spriteBatch.Draw(lockOnCursorTexture, position, new Rectangle?(r1), t1 * num11, rotation,
                            r1.Size() / 2f, new Vector2(0.58f, 1f) * num2 * num5 * (1f + num11) / 2f,
                            SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(lockOnCursorTexture, position, new Rectangle?(r2),
                            favoriteColor * num11 * num11, rotation, r2.Size() / 2f,
                            new Vector2(0.58f, 1f) * num2 * num5 * (1f + num11) / 2f, SpriteEffects.None, 0.0f);
                    }
                }
            }
        }

        public enum LockOnMode
        {
            FocusTarget,
            TargetClosest,
            ThreeDS,
        }
    }
}