﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UIKeybindingListItem
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent.UI.Chat;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
    public class UIKeybindingListItem : UIElement
    {
        private InputMode _inputmode;
        private Color _color;
        private string _keybind;

        public UIKeybindingListItem(string bind, InputMode mode, Color color)
        {
            this._keybind = bind;
            this._inputmode = mode;
            this._color = color;
            this.OnClick += new UIElement.MouseEvent(this.OnClickMethod);
        }

        public void OnClickMethod(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!(PlayerInput.ListeningTrigger != this._keybind))
                return;
            if (PlayerInput.CurrentProfile.AllowEditting)
                PlayerInput.ListenFor(this._keybind, this._inputmode);
            else
                PlayerInput.ListenFor((string) null, this._inputmode);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var num1 = 6f;
            base.DrawSelf(spriteBatch);
            var dimensions = this.GetDimensions();
            var num2 = dimensions.Width + 1f;
            var vector2 = new Vector2(dimensions.X, dimensions.Y);
            var flag = PlayerInput.ListeningTrigger == this._keybind;
            var baseScale = new Vector2(0.8f);
            var baseColor = Color.Lerp(flag ? Color.Gold : (this.IsMouseHovering ? Color.White : Color.Silver),
                Color.White, this.IsMouseHovering ? 0.5f : 0.0f);
            var color = this.IsMouseHovering ? this._color : this._color.MultiplyRGBA(new Color(180, 180, 180));
            var position = vector2;
            Utils.DrawSettingsPanel(spriteBatch, position, num2, color);
            position.X += 8f;
            position.Y += 2f + num1;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, this.GetFriendlyName(),
                position, baseColor, 0.0f, Vector2.Zero, baseScale, num2, 2f);
            position.X -= 17f;
            var text =
                this.GenInput(PlayerInput.CurrentProfile.InputModes[this._inputmode].KeyStatus[this._keybind]);
            if (string.IsNullOrEmpty(text))
            {
                text = Lang.menu[195].Value;
                if (!flag)
                    baseColor = new Color(80, 80, 80);
            }

            var stringSize = ChatManager.GetStringSize(Main.fontItemStack, text, baseScale, -1f);
            position = new Vector2(
                (float) ((double) dimensions.X + (double) dimensions.Width - (double) stringSize.X - 10.0),
                dimensions.Y + 2f + num1);
            if (this._inputmode == InputMode.XBoxGamepad || this._inputmode == InputMode.XBoxGamepadUI)
                position += new Vector2(0.0f, -3f);
            GlyphTagHandler.GlyphsScale = 0.85f;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, text, position, baseColor, 0.0f,
                Vector2.Zero, baseScale, num2, 2f);
            GlyphTagHandler.GlyphsScale = 1f;
        }

        private string GenInput(List<string> list)
        {
            if (list.Count == 0)
                return "";
            var str = "";
            switch (this._inputmode)
            {
                case InputMode.Keyboard:
                case InputMode.KeyboardUI:
                case InputMode.Mouse:
                    str = list[0];
                    for (var index = 1; index < list.Count; ++index)
                        str = str + "/" + list[index];
                    break;
                case InputMode.XBoxGamepad:
                case InputMode.XBoxGamepadUI:
                    str = GlyphTagHandler.GenerateTag(list[0]);
                    for (var index = 1; index < list.Count; ++index)
                        str = str + "/" + GlyphTagHandler.GenerateTag(list[index]);
                    break;
            }

            return str;
        }

        private string GetFriendlyName()
        {
            switch (this._keybind)
            {
                case "MouseLeft":
                    return Lang.menu[162].Value;
                case "MouseRight":
                    return Lang.menu[163].Value;
                case "Up":
                    return Lang.menu[148].Value;
                case "Down":
                    return Lang.menu[149].Value;
                case "Left":
                    return Lang.menu[150].Value;
                case "Right":
                    return Lang.menu[151].Value;
                case "Jump":
                    return Lang.menu[152].Value;
                case "Throw":
                    return Lang.menu[153].Value;
                case "Inventory":
                    return Lang.menu[154].Value;
                case "Grapple":
                    return Lang.menu[155].Value;
                case "SmartSelect":
                    return Lang.menu[160].Value;
                case "SmartCursor":
                    return Lang.menu[161].Value;
                case "QuickMount":
                    return Lang.menu[158].Value;
                case "QuickHeal":
                    return Lang.menu[159].Value;
                case "QuickMana":
                    return Lang.menu[156].Value;
                case "QuickBuff":
                    return Lang.menu[157].Value;
                case "MapZoomIn":
                    return Lang.menu[168].Value;
                case "MapZoomOut":
                    return Lang.menu[169].Value;
                case "MapAlphaUp":
                    return Lang.menu[171].Value;
                case "MapAlphaDown":
                    return Lang.menu[170].Value;
                case "MapFull":
                    return Lang.menu[173].Value;
                case "MapStyle":
                    return Lang.menu[172].Value;
                case "Hotbar1":
                    return Lang.menu[176].Value;
                case "Hotbar2":
                    return Lang.menu[177].Value;
                case "Hotbar3":
                    return Lang.menu[178].Value;
                case "Hotbar4":
                    return Lang.menu[179].Value;
                case "Hotbar5":
                    return Lang.menu[180].Value;
                case "Hotbar6":
                    return Lang.menu[181].Value;
                case "Hotbar7":
                    return Lang.menu[182].Value;
                case "Hotbar8":
                    return Lang.menu[183].Value;
                case "Hotbar9":
                    return Lang.menu[184].Value;
                case "Hotbar10":
                    return Lang.menu[185].Value;
                case "HotbarMinus":
                    return Lang.menu[174].Value;
                case "HotbarPlus":
                    return Lang.menu[175].Value;
                case "DpadRadial1":
                    return Lang.menu[186].Value;
                case "DpadRadial2":
                    return Lang.menu[187].Value;
                case "DpadRadial3":
                    return Lang.menu[188].Value;
                case "DpadRadial4":
                    return Lang.menu[189].Value;
                case "RadialHotbar":
                    return Lang.menu[190].Value;
                case "RadialQuickbar":
                    return Lang.menu[244].Value;
                case "DpadSnap1":
                    return Lang.menu[191].Value;
                case "DpadSnap2":
                    return Lang.menu[192].Value;
                case "DpadSnap3":
                    return Lang.menu[193].Value;
                case "DpadSnap4":
                    return Lang.menu[194].Value;
                case "LockOn":
                    return Lang.menu[231].Value;
                case "ViewZoomIn":
                    return Language.GetTextValue("UI.ZoomIn");
                case "ViewZoomOut":
                    return Language.GetTextValue("UI.ZoomOut");
                default:
                    return this._keybind;
            }
        }
    }
}