﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UIKeybindingSliderItem
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
    public class UIKeybindingSliderItem : UIElement
    {
        private Color _color;
        private Func<string> _TextDisplayFunction;
        private Func<float> _GetStatusFunction;
        private Action<float> _SlideKeyboardAction;
        private Action _SlideGamepadAction;
        private int _sliderIDInPage;
        private Texture2D _toggleTexture;

        public UIKeybindingSliderItem(Func<string> getText, Func<float> getStatus, Action<float> setStatusKeyboard,
            Action setStatusGamepad, int sliderIDInPage, Color color)
        {
            this._color = color;
            this._toggleTexture = TextureManager.Load("Images/UI/Settings_Toggle");
            this._TextDisplayFunction = getText != null ? getText : (Func<string>) (() => "???");
            this._GetStatusFunction = getStatus != null ? getStatus : (Func<float>) (() => 0.0f);
            this._SlideKeyboardAction = setStatusKeyboard != null ? setStatusKeyboard : (Action<float>) (s => { });
            this._SlideGamepadAction = setStatusGamepad != null ? setStatusGamepad : (Action) (() => { });
            this._sliderIDInPage = sliderIDInPage;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var num1 = 6f;
            base.DrawSelf(spriteBatch);
            var lockState = 0;
            IngameOptions.rightHover = -1;
            if (!Main.mouseLeft)
                IngameOptions.rightLock = -1;
            if (IngameOptions.rightLock == this._sliderIDInPage)
                lockState = 1;
            else if (IngameOptions.rightLock != -1)
                lockState = 2;
            var dimensions = this.GetDimensions();
            var num2 = dimensions.Width + 1f;
            var vector2 = new Vector2(dimensions.X, dimensions.Y);
            var flag1 = false;
            var flag2 = this.IsMouseHovering;
            if (lockState == 1)
                flag2 = true;
            if (lockState == 2)
                flag2 = false;
            var baseScale = new Vector2(0.8f);
            var baseColor = Color.Lerp(flag1 ? Color.Gold : (flag2 ? Color.White : Color.Silver), Color.White,
                flag2 ? 0.5f : 0.0f);
            var color = flag2 ? this._color : this._color.MultiplyRGBA(new Color(180, 180, 180));
            var position = vector2;
            Utils.DrawSettingsPanel(spriteBatch, position, num2, color);
            position.X += 8f;
            position.Y += 2f + num1;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, this._TextDisplayFunction(),
                position, baseColor, 0.0f, Vector2.Zero, baseScale, num2, 2f);
            position.X -= 17f;
            Main.colorBarTexture.Frame(1, 1, 0, 0);
            position = new Vector2((float) ((double) dimensions.X + (double) dimensions.Width - 10.0),
                dimensions.Y + 10f + num1);
            IngameOptions.valuePosition = position;
            var num3 = IngameOptions.DrawValueBar(spriteBatch, 1f, this._GetStatusFunction(), lockState,
                (Utils.ColorLerpMethod) null);
            if (IngameOptions.inBar || IngameOptions.rightLock == this._sliderIDInPage)
            {
                IngameOptions.rightHover = this._sliderIDInPage;
                if (PlayerInput.Triggers.Current.MouseLeft && PlayerInput.CurrentProfile.AllowEditting &&
                    (!PlayerInput.UsingGamepad && IngameOptions.rightLock == this._sliderIDInPage))
                    this._SlideKeyboardAction(num3);
            }

            if (IngameOptions.rightHover != -1 && IngameOptions.rightLock == -1)
                IngameOptions.rightLock = IngameOptions.rightHover;
            if (!this.IsMouseHovering || !PlayerInput.CurrentProfile.AllowEditting)
                return;
            this._SlideGamepadAction();
        }
    }
}