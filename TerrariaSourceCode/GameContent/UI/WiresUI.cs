﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.WiresUI
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameInput;

namespace Terraria.GameContent.UI
{
    public class WiresUI
    {
        private static WiresUI.WiresRadial radial = new WiresUI.WiresRadial();

        public static bool Open
        {
            get { return WiresUI.radial.active; }
        }

        public static void HandleWiresUI(SpriteBatch spriteBatch)
        {
            WiresUI.radial.Update();
            WiresUI.radial.Draw(spriteBatch);
        }

        public static class Settings
        {
            public static WiresUI.Settings.MultiToolMode ToolMode = WiresUI.Settings.MultiToolMode.Red;
            private static int _lastActuatorEnabled = 0;

            public static bool DrawWires
            {
                get
                {
                    if (Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].mech)
                        return true;
                    if (Main.player[Main.myPlayer].InfoAccMechShowWires)
                        return Main.player[Main.myPlayer].builderAccStatus[8] == 0;
                    return false;
                }
            }

            public static bool HideWires
            {
                get
                {
                    return Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].type == 3620;
                }
            }

            public static bool DrawToolModeUI
            {
                get
                {
                    var type = Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].type;
                    if (type != 3611)
                        return type == 3625;
                    return true;
                }
            }

            public static bool DrawToolAllowActuators
            {
                get
                {
                    var type = Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].type;
                    if (type == 3611)
                        WiresUI.Settings._lastActuatorEnabled = 2;
                    if (type == 3625)
                        WiresUI.Settings._lastActuatorEnabled = 1;
                    return WiresUI.Settings._lastActuatorEnabled == 2;
                }
            }

            [Flags]
            public enum MultiToolMode
            {
                Red = 1,
                Green = 2,
                Blue = 4,
                Yellow = 8,
                Actuator = 16, // 0x00000010
                Cutter = 32, // 0x00000020
            }
        }

        public class WiresRadial
        {
            public Vector2 position;
            public bool active;
            public bool OnWiresMenu;
            private float _lineOpacity;

            public void Update()
            {
                this.FlowerUpdate();
                this.LineUpdate();
            }

            private void LineUpdate()
            {
                var flag1 = true;
                var min = 0.75f;
                var player = Main.player[Main.myPlayer];
                if (!WiresUI.Settings.DrawToolModeUI || Main.drawingPlayerChat)
                {
                    flag1 = false;
                    min = 0.0f;
                }

                bool flag2;
                if (player.dead || Main.mouseItem.type > 0)
                {
                    flag2 = false;
                    this._lineOpacity = 0.0f;
                }
                else if (player.showItemIcon && player.showItemIcon2 != 0 && player.showItemIcon2 != 3625)
                {
                    flag2 = false;
                    this._lineOpacity = 0.0f;
                }
                else if (!player.showItemIcon &&
                         (!PlayerInput.UsingGamepad && !WiresUI.Settings.DrawToolAllowActuators ||
                          (player.mouseInterface || player.lastMouseInterface)) ||
                         (Main.ingameOptionsWindow || Main.InGameUI.IsVisible))
                {
                    flag2 = false;
                    this._lineOpacity = 0.0f;
                }
                else
                {
                    var num = Utils.Clamp<float>(this._lineOpacity + 0.05f * (float) flag1.ToDirectionInt(), min, 1f);
                    this._lineOpacity += 0.05f * (float) Math.Sign(num - this._lineOpacity);
                    if ((double) Math.Abs(this._lineOpacity - num) >= 0.0500000007450581)
                        return;
                    this._lineOpacity = num;
                }
            }

            private void FlowerUpdate()
            {
                var player = Main.player[Main.myPlayer];
                if (!WiresUI.Settings.DrawToolModeUI)
                    this.active = false;
                else if ((player.mouseInterface || player.lastMouseInterface) && !this.OnWiresMenu)
                    this.active = false;
                else if (player.dead || Main.mouseItem.type > 0)
                {
                    this.active = false;
                    this.OnWiresMenu = false;
                }
                else
                {
                    this.OnWiresMenu = false;
                    if (!Main.mouseRight || !Main.mouseRightRelease ||
                        (PlayerInput.LockTileUseButton || player.noThrow != 0) ||
                        (Main.HoveringOverAnNPC || player.talkNPC != -1))
                        return;
                    if (this.active)
                    {
                        this.active = false;
                    }
                    else
                    {
                        if (Main.SmartInteractShowingGenuine)
                            return;
                        this.active = true;
                        this.position = Main.MouseScreen;
                        if (!PlayerInput.UsingGamepad || !Main.SmartCursorEnabled)
                            return;
                        this.position = new Vector2((float) Main.screenWidth, (float) Main.screenHeight) / 2f;
                    }
                }
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                this.DrawFlower(spriteBatch);
                this.DrawCursorArea(spriteBatch);
            }

            private void DrawLine(SpriteBatch spriteBatch)
            {
                if (this.active || (double) this._lineOpacity == 0.0)
                    return;
                var vector2_1 = Main.MouseScreen;
                var vector2_2 = new Vector2((float) (Main.screenWidth / 2), (float) (Main.screenHeight - 70));
                if (PlayerInput.UsingGamepad)
                    vector2_1 = Vector2.Zero;
                var v = vector2_1 - vector2_2;
                var num1 = (double) Vector2.Dot(Vector2.Normalize(v), Vector2.UnitX);
                var num2 = (double) Vector2.Dot(Vector2.Normalize(v), Vector2.UnitY);
                var rotation = (double) v.ToRotation();
                var num3 = (double) v.Length();
                var flag1 = false;
                var toolAllowActuators = WiresUI.Settings.DrawToolAllowActuators;
                for (var index = 0; index < 6; ++index)
                {
                    if (toolAllowActuators || index != 5)
                    {
                        var flag2 =
                            WiresUI.Settings.ToolMode.HasFlag((Enum) (WiresUI.Settings.MultiToolMode) (1 << index));
                        if (index == 5)
                            flag2 = WiresUI.Settings.ToolMode.HasFlag((Enum) WiresUI.Settings.MultiToolMode.Actuator);
                        var vector2_3 = vector2_2 + Vector2.UnitX * (float) (45.0 * ((double) index - 1.5));
                        var num4 = index;
                        if (index == 0)
                            num4 = 3;
                        if (index == 3)
                            num4 = 0;
                        switch (num4)
                        {
                            case 0:
                            case 1:
                                vector2_3 = vector2_2 + new Vector2(
                                                (float) (45.0 + (toolAllowActuators ? 15.0 : 0.0)) * (float) (2 - num4),
                                                0.0f) * this._lineOpacity;
                                break;
                            case 2:
                            case 3:
                                vector2_3 = vector2_2 + new Vector2(
                                                (float) -(45.0 + (toolAllowActuators ? 15.0 : 0.0)) *
                                                (float) (num4 - 1), 0.0f) * this._lineOpacity;
                                break;
                            case 4:
                                flag2 = false;
                                vector2_3 = vector2_2 - new Vector2(0.0f, toolAllowActuators ? 22f : 0.0f) *
                                            this._lineOpacity;
                                break;
                            case 5:
                                vector2_3 = vector2_2 + new Vector2(0.0f, 22f) * this._lineOpacity;
                                break;
                        }

                        var flag3 = false;
                        if (!PlayerInput.UsingGamepad)
                            flag3 = (double) Vector2.Distance(vector2_3, vector2_1) < 19.0 * (double) this._lineOpacity;
                        if (flag1)
                            flag3 = false;
                        if (flag3)
                            flag1 = true;
                        var texture2D1 = Main.wireUITexture[
                            (WiresUI.Settings.ToolMode.HasFlag((Enum) WiresUI.Settings.MultiToolMode.Cutter) ? 8 : 0) +
                            (flag3 ? 1 : 0)];
                        var texture2D2 = (Texture2D) null;
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                texture2D2 = Main.wireUITexture[2 + index];
                                break;
                            case 4:
                                texture2D2 = Main.wireUITexture[
                                    WiresUI.Settings.ToolMode.HasFlag((Enum) WiresUI.Settings.MultiToolMode.Cutter)
                                        ? 7
                                        : 6];
                                break;
                            case 5:
                                texture2D2 = Main.wireUITexture[10];
                                break;
                        }

                        var color1 = Color.White;
                        var color2 = Color.White;
                        if (!flag2 && index != 4)
                        {
                            if (flag3)
                            {
                                color2 = new Color(100, 100, 100);
                                color2 = new Color(120, 120, 120);
                                color1 = new Color(200, 200, 200);
                            }
                            else
                            {
                                color2 = new Color(150, 150, 150);
                                color2 = new Color(80, 80, 80);
                                color1 = new Color(100, 100, 100);
                            }
                        }

                        Utils.CenteredRectangle(vector2_3, new Vector2(40f));
                        if (flag3)
                        {
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                switch (index)
                                {
                                    case 0:
                                        WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Red;
                                        break;
                                    case 1:
                                        WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Green;
                                        break;
                                    case 2:
                                        WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Blue;
                                        break;
                                    case 3:
                                        WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Yellow;
                                        break;
                                    case 4:
                                        WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Cutter;
                                        break;
                                    case 5:
                                        WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Actuator;
                                        break;
                                }
                            }

                            if (!Main.mouseLeft || Main.player[Main.myPlayer].mouseInterface)
                                Main.player[Main.myPlayer].mouseInterface = true;
                            this.OnWiresMenu = true;
                        }

                        var num5 = flag3 ? 1 : 0;
                        spriteBatch.Draw(texture2D1, vector2_3, new Rectangle?(), color1 * this._lineOpacity, 0.0f,
                            texture2D1.Size() / 2f, this._lineOpacity, SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(texture2D2, vector2_3, new Rectangle?(), color2 * this._lineOpacity, 0.0f,
                            texture2D2.Size() / 2f, this._lineOpacity, SpriteEffects.None, 0.0f);
                    }
                }

                if (!Main.mouseLeft || !Main.mouseLeftRelease || flag1)
                    return;
                this.active = false;
            }

            private void DrawFlower(SpriteBatch spriteBatch)
            {
                if (!this.active)
                    return;
                var vector2_1 = Main.MouseScreen;
                var position = this.position;
                if (PlayerInput.UsingGamepad && Main.SmartCursorEnabled)
                    vector2_1 = !(PlayerInput.GamepadThumbstickRight != Vector2.Zero)
                        ? (!(PlayerInput.GamepadThumbstickLeft != Vector2.Zero)
                            ? this.position
                            : this.position + PlayerInput.GamepadThumbstickLeft * 40f)
                        : this.position + PlayerInput.GamepadThumbstickRight * 40f;
                var v = vector2_1 - position;
                var num1 = (double) Vector2.Dot(Vector2.Normalize(v), Vector2.UnitX);
                var num2 = (double) Vector2.Dot(Vector2.Normalize(v), Vector2.UnitY);
                var rotation = v.ToRotation();
                var num3 = v.Length();
                var flag1 = false;
                var toolAllowActuators = WiresUI.Settings.DrawToolAllowActuators;
                var num4 = (float) (4 + toolAllowActuators.ToInt());
                var num5 = toolAllowActuators ? 11f : -0.5f;
                for (var index = 0; index < 6; ++index)
                {
                    if (toolAllowActuators || index != 5)
                    {
                        var flag2 =
                            WiresUI.Settings.ToolMode.HasFlag((Enum) (WiresUI.Settings.MultiToolMode) (1 << index));
                        if (index == 5)
                            flag2 = WiresUI.Settings.ToolMode.HasFlag((Enum) WiresUI.Settings.MultiToolMode.Actuator);
                        var vector2_2 = position + Vector2.UnitX * (float) (45.0 * ((double) index - 1.5));
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                var num6 = (float) index;
                                if (index == 0)
                                    num6 = 3f;
                                if (index == 3)
                                    num6 = 0.0f;
                                vector2_2 = position + Vector2.UnitX.RotatedBy(
                                                (double) num6 * 6.28318548202515 / (double) num4 -
                                                3.14159274101257 / (double) num5, new Vector2()) * 45f;
                                break;
                            case 4:
                                flag2 = false;
                                vector2_2 = position;
                                break;
                            case 5:
                                vector2_2 = position + Vector2.UnitX.RotatedBy(
                                                (double) (index - 1) * 6.28318548202515 / (double) num4 -
                                                3.14159274101257 / (double) num5, new Vector2()) * 45f;
                                break;
                        }

                        var flag3 = false;
                        if (index == 4)
                            flag3 = (double) num3 < 20.0;
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                            case 5:
                                var num7 = (vector2_2 - position).ToRotation().AngleTowards(rotation,
                                                 (float) (6.28318548202515 / ((double) num4 * 2.0))) - rotation;
                                if ((double) num3 >= 20.0 && (double) Math.Abs(num7) < 0.00999999977648258)
                                {
                                    flag3 = true;
                                    break;
                                }

                                break;
                            case 4:
                                flag3 = (double) num3 < 20.0;
                                break;
                        }

                        if (!PlayerInput.UsingGamepad)
                            flag3 = (double) Vector2.Distance(vector2_2, vector2_1) < 19.0;
                        if (flag1)
                            flag3 = false;
                        if (flag3)
                            flag1 = true;
                        var texture2D1 = Main.wireUITexture[
                            (WiresUI.Settings.ToolMode.HasFlag((Enum) WiresUI.Settings.MultiToolMode.Cutter) ? 8 : 0) +
                            (flag3 ? 1 : 0)];
                        var texture2D2 = (Texture2D) null;
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                texture2D2 = Main.wireUITexture[2 + index];
                                break;
                            case 4:
                                texture2D2 = Main.wireUITexture[
                                    WiresUI.Settings.ToolMode.HasFlag((Enum) WiresUI.Settings.MultiToolMode.Cutter)
                                        ? 7
                                        : 6];
                                break;
                            case 5:
                                texture2D2 = Main.wireUITexture[10];
                                break;
                        }

                        var color1 = Color.White;
                        var color2 = Color.White;
                        if (!flag2 && index != 4)
                        {
                            if (flag3)
                            {
                                color2 = new Color(100, 100, 100);
                                color2 = new Color(120, 120, 120);
                                color1 = new Color(200, 200, 200);
                            }
                            else
                            {
                                color2 = new Color(150, 150, 150);
                                color2 = new Color(80, 80, 80);
                                color1 = new Color(100, 100, 100);
                            }
                        }

                        Utils.CenteredRectangle(vector2_2, new Vector2(40f));
                        if (flag3)
                        {
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                switch (index)
                                {
                                    case 0:
                                        WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Red;
                                        break;
                                    case 1:
                                        WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Green;
                                        break;
                                    case 2:
                                        WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Blue;
                                        break;
                                    case 3:
                                        WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Yellow;
                                        break;
                                    case 4:
                                        WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Cutter;
                                        break;
                                    case 5:
                                        WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Actuator;
                                        break;
                                }
                            }

                            Main.player[Main.myPlayer].mouseInterface = true;
                            this.OnWiresMenu = true;
                        }

                        var num8 = flag3 ? 1 : 0;
                        spriteBatch.Draw(texture2D1, vector2_2, new Rectangle?(), color1, 0.0f, texture2D1.Size() / 2f,
                            1f, SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(texture2D2, vector2_2, new Rectangle?(), color2, 0.0f, texture2D2.Size() / 2f,
                            1f, SpriteEffects.None, 0.0f);
                    }
                }

                if (!Main.mouseLeft || !Main.mouseLeftRelease || flag1)
                    return;
                this.active = false;
            }

            private void DrawCursorArea(SpriteBatch spriteBatch)
            {
                if (this.active || (double) this._lineOpacity == 0.0)
                    return;
                var vector2 = Main.MouseScreen +
                                  new Vector2((float) (10 - 9 * PlayerInput.UsingGamepad.ToInt()), 25f);
                var color1 = new Color(50, 50, 50);
                var toolAllowActuators = WiresUI.Settings.DrawToolAllowActuators;
                if (!toolAllowActuators)
                {
                    if (!PlayerInput.UsingGamepad)
                        vector2 += new Vector2(-20f, 10f);
                    else
                        vector2 += new Vector2(0.0f, 10f);
                }

                var builderAccTexture = Main.builderAccTexture;
                var texture = builderAccTexture;
                var r1 = new Rectangle(140, 2, 6, 6);
                var r2 = new Rectangle(148, 2, 6, 6);
                var r3 = new Rectangle(128, 0, 10, 10);
                var num1 = 1f;
                var scale = 1f;
                var flag1 = false;
                if (flag1 && !toolAllowActuators)
                    num1 *= Main.cursorScale;
                var lineOpacity = this._lineOpacity;
                if (PlayerInput.UsingGamepad)
                    lineOpacity *= Main.GamepadCursorAlpha;
                for (var index = 0; index < 5; ++index)
                {
                    if (toolAllowActuators || index != 4)
                    {
                        var num2 = lineOpacity;
                        var vec = vector2 + Vector2.UnitX * (float) (45.0 * ((double) index - 1.5));
                        var num3 = index;
                        if (index == 0)
                            num3 = 3;
                        if (index == 1)
                            num3 = 2;
                        if (index == 2)
                            num3 = 1;
                        if (index == 3)
                            num3 = 0;
                        if (index == 4)
                            num3 = 5;
                        var num4 = num3;
                        switch (num4)
                        {
                            case 1:
                                num4 = 2;
                                break;
                            case 2:
                                num4 = 1;
                                break;
                        }

                        var flag2 =
                            WiresUI.Settings.ToolMode.HasFlag((Enum) (WiresUI.Settings.MultiToolMode) (1 << num4));
                        if (num4 == 5)
                            flag2 = WiresUI.Settings.ToolMode.HasFlag((Enum) WiresUI.Settings.MultiToolMode.Actuator);
                        var color2 = Color.HotPink;
                        switch (num3)
                        {
                            case 0:
                                color2 = new Color(253, 58, 61);
                                break;
                            case 1:
                                color2 = new Color(83, 180, 253);
                                break;
                            case 2:
                                color2 = new Color(83, 253, 153);
                                break;
                            case 3:
                                color2 = new Color(253, 254, 83);
                                break;
                            case 5:
                                color2 = Color.WhiteSmoke;
                                break;
                        }

                        if (!flag2)
                            color2 = Color.Lerp(color2, Color.Black, 0.65f);
                        if (flag1)
                        {
                            if (toolAllowActuators)
                            {
                                switch (num3)
                                {
                                    case 0:
                                        vec = vector2 + new Vector2(-12f, 0.0f) * num1;
                                        break;
                                    case 1:
                                        vec = vector2 + new Vector2(-6f, 12f) * num1;
                                        break;
                                    case 2:
                                        vec = vector2 + new Vector2(6f, 12f) * num1;
                                        break;
                                    case 3:
                                        vec = vector2 + new Vector2(12f, 0.0f) * num1;
                                        break;
                                    case 5:
                                        vec = vector2 + new Vector2(0.0f, 0.0f) * num1;
                                        break;
                                }
                            }
                            else
                                vec = vector2 + new Vector2((float) (12 * (num3 + 1)), (float) (12 * (3 - num3))) *
                                      num1;
                        }
                        else if (toolAllowActuators)
                        {
                            switch (num3)
                            {
                                case 0:
                                    vec = vector2 + new Vector2(-12f, 0.0f) * num1;
                                    break;
                                case 1:
                                    vec = vector2 + new Vector2(-6f, 12f) * num1;
                                    break;
                                case 2:
                                    vec = vector2 + new Vector2(6f, 12f) * num1;
                                    break;
                                case 3:
                                    vec = vector2 + new Vector2(12f, 0.0f) * num1;
                                    break;
                                case 5:
                                    vec = vector2 + new Vector2(0.0f, 0.0f) * num1;
                                    break;
                            }
                        }
                        else
                        {
                            var num5 = 0.7f;
                            switch (num3)
                            {
                                case 0:
                                    vec = vector2 + new Vector2(0.0f, -12f) * num1 * num5;
                                    break;
                                case 1:
                                    vec = vector2 + new Vector2(-12f, 0.0f) * num1 * num5;
                                    break;
                                case 2:
                                    vec = vector2 + new Vector2(0.0f, 12f) * num1 * num5;
                                    break;
                                case 3:
                                    vec = vector2 + new Vector2(12f, 0.0f) * num1 * num5;
                                    break;
                            }
                        }

                        var position = vec.Floor();
                        spriteBatch.Draw(texture, position, new Rectangle?(r3), color1 * num2, 0.0f, r3.Size() / 2f,
                            scale, SpriteEffects.None, 0.0f);
                        spriteBatch.Draw(builderAccTexture, position, new Rectangle?(r1), color2 * num2, 0.0f,
                            r1.Size() / 2f, scale, SpriteEffects.None, 0.0f);
                        if (WiresUI.Settings.ToolMode.HasFlag((Enum) WiresUI.Settings.MultiToolMode.Cutter))
                            spriteBatch.Draw(builderAccTexture, position, new Rectangle?(r2), color1 * num2, 0.0f,
                                r2.Size() / 2f, scale, SpriteEffects.None, 0.0f);
                    }
                }
            }
        }
    }
}