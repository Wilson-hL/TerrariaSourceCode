﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.States.UIVirtualKeyboard
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
    public class UIVirtualKeyboard : UIState
    {
        private static string _cancelCacheSign = "";
        private static string _cancelCacheChest = "";
        public static int OffsetDown = 0;
        private UITextPanel<object>[] _keyList = new UITextPanel<object>[50];
        private Color _internalBorderColor = new Color(89, 116, 213);
        private Color _internalBorderColorSelected = Main.OurFavoriteColor;
        private const string DEFAULT_KEYS = "1234567890qwertyuiopasdfghjkl'zxcvbnm,.?";
        private const string SHIFT_KEYS = "1234567890QWERTYUIOPASDFGHJKL'ZXCVBNM,.?";
        private const string SYMBOL_KEYS = "1234567890!@#$%^&*()-_+=/\\{}[]<>;:\"`|~£¥";
        private const float KEY_SPACING = 4f;
        private const float KEY_WIDTH = 48f;
        private const float KEY_HEIGHT = 37f;
        private static UIVirtualKeyboard _currentInstance;
        private UITextPanel<object> _shiftButton;
        private UITextPanel<object> _symbolButton;
        private UITextBox _textBox;
        private UITextPanel<LocalizedText> _submitButton;
        private UITextPanel<LocalizedText> _cancelButton;
        private UIText _label;
        private UITextPanel<object> _enterButton;
        private UITextPanel<object> _spacebarButton;
        private UITextPanel<object> _restoreButton;
        private Texture2D _textureShift;
        private Texture2D _textureBackspace;
        private UITextPanel<LocalizedText> _submitButton2;
        private UITextPanel<LocalizedText> _cancelButton2;
        private UIElement outerLayer1;
        private UIElement outerLayer2;
        private bool _allowEmpty;
        private UIVirtualKeyboard.KeyState _keyState;
        private UIVirtualKeyboard.KeyboardSubmitEvent _submitAction;
        private Action _cancelAction;
        private int _lastOffsetDown;
        private int _keyboardContext;
        private bool _edittingSign;
        private bool _edittingChest;
        private float _textBoxHeight;
        private float _labelHeight;
        private bool _canSubmit;

        public string Text
        {
            get { return this._textBox.Text; }
            set
            {
                this._textBox.SetText(value);
                this.ValidateText();
            }
        }

        public UIVirtualKeyboard(string labelText, string startingText,
            UIVirtualKeyboard.KeyboardSubmitEvent submitAction, Action cancelAction, int inputMode = 0,
            bool allowEmpty = false)
        {
            this._keyboardContext = inputMode;
            this._allowEmpty = allowEmpty;
            UIVirtualKeyboard.OffsetDown = 0;
            this._lastOffsetDown = 0;
            this._edittingSign = this._keyboardContext == 1;
            this._edittingChest = this._keyboardContext == 2;
            UIVirtualKeyboard._currentInstance = this;
            this._submitAction = submitAction;
            this._cancelAction = cancelAction;
            this._textureShift = TextureManager.Load("Images/UI/VK_Shift");
            this._textureBackspace = TextureManager.Load("Images/UI/VK_Backspace");
            this.Top.Pixels = (float) this._lastOffsetDown;
            var num1 = (float) (-5000 * this._edittingSign.ToInt());
            var maxValue = (float) byte.MaxValue;
            var num2 = 0.0f;
            var num3 = 516f;
            var element = new UIElement();
            element.Width.Pixels = (float) ((double) num3 + 8.0 + 16.0);
            element.Top.Precent = num2;
            element.Top.Pixels = maxValue;
            element.Height.Pixels = 266f;
            element.HAlign = 0.5f;
            element.SetPadding(0.0f);
            this.outerLayer1 = element;
            var uiElement = new UIElement();
            uiElement.Width.Pixels = (float) ((double) num3 + 8.0 + 16.0);
            uiElement.Top.Precent = num2;
            uiElement.Top.Pixels = maxValue;
            uiElement.Height.Pixels = 266f;
            uiElement.HAlign = 0.5f;
            uiElement.SetPadding(0.0f);
            this.outerLayer2 = uiElement;
            var mainPanel = new UIPanel();
            mainPanel.Width.Precent = 1f;
            mainPanel.Height.Pixels = 225f;
            mainPanel.BackgroundColor = new Color(23, 33, 69) * 0.7f;
            element.Append((UIElement) mainPanel);
            var num4 = -79f;
            this._textBox = new UITextBox("", 0.78f, true);
            this._textBox.BackgroundColor = Color.Transparent;
            this._textBox.BorderColor = Color.Transparent;
            this._textBox.HAlign = 0.5f;
            this._textBox.Width.Pixels = num3;
            this._textBox.Top.Pixels = (float) ((double) num4 + (double) maxValue - 10.0) + num1;
            this._textBox.Top.Precent = num2;
            this._textBox.Height.Pixels = 37f;
            this.Append((UIElement) this._textBox);
            for (var x = 0; x < 10; ++x)
            {
                for (var y = 0; y < 4; ++y)
                {
                    var keyboardButton = this.CreateKeyboardButton(
                        (object) "1234567890qwertyuiopasdfghjkl'zxcvbnm,.?"[y * 10 + x].ToString(), x, y, 1, true);
                    keyboardButton.OnClick += new UIElement.MouseEvent(this.TypeText);
                    mainPanel.Append((UIElement) keyboardButton);
                }
            }

            this._shiftButton = this.CreateKeyboardButton((object) "", 0, 4, 1, false);
            this._shiftButton.PaddingLeft = 0.0f;
            this._shiftButton.PaddingRight = 0.0f;
            this._shiftButton.PaddingBottom = this._shiftButton.PaddingTop = 0.0f;
            this._shiftButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            this._shiftButton.BorderColor = this._internalBorderColor * 0.7f;
            this._shiftButton.OnMouseOver += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                this._shiftButton.BorderColor = this._internalBorderColorSelected;
                if (this._keyState == UIVirtualKeyboard.KeyState.Shift)
                    return;
                this._shiftButton.BackgroundColor = new Color(73, 94, 171);
            });
            this._shiftButton.OnMouseOut += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                this._shiftButton.BorderColor = this._internalBorderColor * 0.7f;
                if (this._keyState == UIVirtualKeyboard.KeyState.Shift)
                    return;
                this._shiftButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            });
            this._shiftButton.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
                this.SetKeyState(this._keyState == UIVirtualKeyboard.KeyState.Shift
                    ? UIVirtualKeyboard.KeyState.Default
                    : UIVirtualKeyboard.KeyState.Shift);
            });
            var uiImage1 = new UIImage(this._textureShift);
            uiImage1.HAlign = 0.5f;
            uiImage1.VAlign = 0.5f;
            uiImage1.ImageScale = 0.85f;
            this._shiftButton.Append((UIElement) uiImage1);
            mainPanel.Append((UIElement) this._shiftButton);
            this._symbolButton = this.CreateKeyboardButton((object) "@%", 1, 4, 1, false);
            this._symbolButton.PaddingLeft = 0.0f;
            this._symbolButton.PaddingRight = 0.0f;
            this._symbolButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            this._symbolButton.BorderColor = this._internalBorderColor * 0.7f;
            this._symbolButton.OnMouseOver += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                this._symbolButton.BorderColor = this._internalBorderColorSelected;
                if (this._keyState == UIVirtualKeyboard.KeyState.Symbol)
                    return;
                this._symbolButton.BackgroundColor = new Color(73, 94, 171);
            });
            this._symbolButton.OnMouseOut += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                this._symbolButton.BorderColor = this._internalBorderColor * 0.7f;
                if (this._keyState == UIVirtualKeyboard.KeyState.Symbol)
                    return;
                this._symbolButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            });
            this._symbolButton.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
                this.SetKeyState(this._keyState == UIVirtualKeyboard.KeyState.Symbol
                    ? UIVirtualKeyboard.KeyState.Default
                    : UIVirtualKeyboard.KeyState.Symbol);
            });
            mainPanel.Append((UIElement) this._symbolButton);
            this.BuildSpaceBarArea(mainPanel);
            this._submitButton = new UITextPanel<LocalizedText>(
                this._edittingSign || this._edittingChest ? Language.GetText("UI.Save") : Language.GetText("UI.Submit"),
                0.4f, true);
            this._submitButton.Height.Pixels = 37f;
            this._submitButton.Width.Precent = 0.4f;
            this._submitButton.HAlign = 1f;
            this._submitButton.VAlign = 1f;
            this._submitButton.PaddingLeft = 0.0f;
            this._submitButton.PaddingRight = 0.0f;
            this.ValidateText();
            this._submitButton.OnMouseOver += (UIElement.MouseEvent) ((evt, listeningElement) => this.ValidateText());
            this._submitButton.OnMouseOut += (UIElement.MouseEvent) ((evt, listeningElement) => this.ValidateText());
            this._submitButton.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                var text = this.Text.Trim();
                if (text.Length <= 0 && !this._edittingSign && (!this._edittingChest && !this._allowEmpty))
                    return;
                Main.PlaySound(10, -1, -1, 1, 1f, 0.0f);
                this._submitAction(text);
            });
            element.Append((UIElement) this._submitButton);
            this._cancelButton = new UITextPanel<LocalizedText>(Language.GetText("UI.Cancel"), 0.4f, true);
            this.StyleKey<LocalizedText>(this._cancelButton, true);
            this._cancelButton.Height.Pixels = 37f;
            this._cancelButton.Width.Precent = 0.4f;
            this._cancelButton.VAlign = 1f;
            this._cancelButton.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                Main.PlaySound(11, -1, -1, 1, 1f, 0.0f);
                this._cancelAction();
            });
            element.Append((UIElement) this._cancelButton);
            this._submitButton2 = new UITextPanel<LocalizedText>(
                this._edittingSign || this._edittingChest ? Language.GetText("UI.Save") : Language.GetText("UI.Submit"),
                0.72f, true);
            this._submitButton2.TextColor = Color.Silver;
            this._submitButton2.DrawPanel = false;
            this._submitButton2.Height.Pixels = 60f;
            this._submitButton2.Width.Precent = 0.4f;
            this._submitButton2.HAlign = 0.5f;
            this._submitButton2.VAlign = 0.0f;
            this._submitButton2.OnMouseOver += (UIElement.MouseEvent) ((a, b) =>
            {
                ((UITextPanel<LocalizedText>) b).TextScale = 0.85f;
                ((UITextPanel<LocalizedText>) b).TextColor = Color.White;
            });
            this._submitButton2.OnMouseOut += (UIElement.MouseEvent) ((a, b) =>
            {
                ((UITextPanel<LocalizedText>) b).TextScale = 0.72f;
                ((UITextPanel<LocalizedText>) b).TextColor = Color.Silver;
            });
            this._submitButton2.Top.Pixels = 50f;
            this._submitButton2.PaddingLeft = 0.0f;
            this._submitButton2.PaddingRight = 0.0f;
            this.ValidateText();
            this._submitButton2.OnMouseOver += (UIElement.MouseEvent) ((evt, listeningElement) => this.ValidateText());
            this._submitButton2.OnMouseOut += (UIElement.MouseEvent) ((evt, listeningElement) => this.ValidateText());
            this._submitButton2.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                var text = this.Text.Trim();
                if (text.Length <= 0 && !this._edittingSign && (!this._edittingChest && !this._allowEmpty))
                    return;
                Main.PlaySound(10, -1, -1, 1, 1f, 0.0f);
                this._submitAction(text);
            });
            this.outerLayer2.Append((UIElement) this._submitButton2);
            this._cancelButton2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Cancel"), 0.72f, true);
            this._cancelButton2.TextColor = Color.Silver;
            this._cancelButton2.DrawPanel = false;
            this._cancelButton2.OnMouseOver += (UIElement.MouseEvent) ((a, b) =>
            {
                ((UITextPanel<LocalizedText>) b).TextScale = 0.85f;
                ((UITextPanel<LocalizedText>) b).TextColor = Color.White;
            });
            this._cancelButton2.OnMouseOut += (UIElement.MouseEvent) ((a, b) =>
            {
                ((UITextPanel<LocalizedText>) b).TextScale = 0.72f;
                ((UITextPanel<LocalizedText>) b).TextColor = Color.Silver;
            });
            this._cancelButton2.Height.Pixels = 60f;
            this._cancelButton2.Width.Precent = 0.4f;
            this._cancelButton2.Top.Pixels = 114f;
            this._cancelButton2.VAlign = 0.0f;
            this._cancelButton2.HAlign = 0.5f;
            this._cancelButton2.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                Main.PlaySound(11, -1, -1, 1, 1f, 0.0f);
                this._cancelAction();
            });
            this.outerLayer2.Append((UIElement) this._cancelButton2);
            var keyboardButton1 = this.CreateKeyboardButton((object) "", 8, 4, 2, true);
            keyboardButton1.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
                this._textBox.Backspace();
                this.ValidateText();
            });
            keyboardButton1.PaddingLeft = 0.0f;
            keyboardButton1.PaddingRight = 0.0f;
            keyboardButton1.PaddingBottom = keyboardButton1.PaddingTop = 0.0f;
            var uiImage2 = new UIImage(this._textureBackspace);
            uiImage2.HAlign = 0.5f;
            uiImage2.VAlign = 0.5f;
            uiImage2.ImageScale = 0.92f;
            keyboardButton1.Append((UIElement) uiImage2);
            mainPanel.Append((UIElement) keyboardButton1);
            var uiText = new UIText(labelText, 0.75f, true);
            uiText.HAlign = 0.5f;
            uiText.Width.Pixels = num3;
            uiText.Top.Pixels = (float) ((double) num4 - 37.0 - 4.0) + maxValue + num1;
            uiText.Top.Precent = num2;
            uiText.Height.Pixels = 37f;
            this.Append((UIElement) uiText);
            this._label = uiText;
            this.Append(element);
            this._textBox.SetTextMaxLength(this._edittingSign ? 99999 : 20);
            this.Text = startingText;
            if (this.Text.Length == 0)
                this.SetKeyState(UIVirtualKeyboard.KeyState.Shift);
            UIVirtualKeyboard.OffsetDown = 9999;
            this.UpdateOffsetDown();
        }

        private void BuildSpaceBarArea(UIPanel mainPanel)
        {
            var createTheseTwo = (Action) (() =>
            {
                var flag = this.CanRestore();
                var x = flag ? 4 : 5;
                var edittingSign = this._edittingSign;
                var width = !flag || !edittingSign ? 3 : 2;
                var keyboardButton1 = this.CreateKeyboardButton(
                    (object) Language.GetText("UI.SpaceButton"), 2, 4,
                    this._edittingSign || this._edittingChest && flag ? width : 6, true);
                keyboardButton1.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
                {
                    Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
                    this._textBox.Write(" ");
                    this.ValidateText();
                });
                mainPanel.Append((UIElement) keyboardButton1);
                this._spacebarButton = keyboardButton1;
                if (!edittingSign)
                    return;
                var keyboardButton2 =
                    this.CreateKeyboardButton((object) Language.GetText("UI.EnterButton"), x, 4, width, true);
                keyboardButton2.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
                {
                    Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
                    this._textBox.Write(string.Concat((object) '\n'));
                    this.ValidateText();
                });
                mainPanel.Append((UIElement) keyboardButton2);
                this._enterButton = keyboardButton2;
            });
            createTheseTwo();
            if (!this.CanRestore())
                return;
            var restoreBar =
                this.CreateKeyboardButton((object) Language.GetText("UI.RestoreButton"), 6, 4, 2, true);
            restoreBar.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
                this.RestoreCancelledInput(this._keyboardContext);
                this.ValidateText();
                restoreBar.Remove();
                this._enterButton.Remove();
                this._spacebarButton.Remove();
                createTheseTwo();
            });
            mainPanel.Append((UIElement) restoreBar);
            this._restoreButton = restoreBar;
        }

        private bool CanRestore()
        {
            if (this._edittingSign)
                return UIVirtualKeyboard._cancelCacheSign.Length > 0;
            if (this._edittingChest)
                return UIVirtualKeyboard._cancelCacheChest.Length > 0;
            return false;
        }

        private void TypeText(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
            var flag = this.Text.Length == 0;
            this._textBox.Write(((UITextPanel<object>) listeningElement).Text);
            this.ValidateText();
            if (!flag || this.Text.Length <= 0 || this._keyState != UIVirtualKeyboard.KeyState.Shift)
                return;
            this.SetKeyState(UIVirtualKeyboard.KeyState.Default);
        }

        public void SetKeyState(UIVirtualKeyboard.KeyState keyState)
        {
            var uiTextPanel1 = (UITextPanel<object>) null;
            switch (this._keyState)
            {
                case UIVirtualKeyboard.KeyState.Symbol:
                    uiTextPanel1 = this._symbolButton;
                    break;
                case UIVirtualKeyboard.KeyState.Shift:
                    uiTextPanel1 = this._shiftButton;
                    break;
            }

            if (uiTextPanel1 != null)
            {
                if (uiTextPanel1.IsMouseHovering)
                    uiTextPanel1.BackgroundColor = new Color(73, 94, 171);
                else
                    uiTextPanel1.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            }

            var str = (string) null;
            var uiTextPanel2 = (UITextPanel<object>) null;
            switch (keyState)
            {
                case UIVirtualKeyboard.KeyState.Default:
                    str = "1234567890qwertyuiopasdfghjkl'zxcvbnm,.?";
                    break;
                case UIVirtualKeyboard.KeyState.Symbol:
                    str = "1234567890!@#$%^&*()-_+=/\\{}[]<>;:\"`|~£¥";
                    uiTextPanel2 = this._symbolButton;
                    break;
                case UIVirtualKeyboard.KeyState.Shift:
                    str = "1234567890QWERTYUIOPASDFGHJKL'ZXCVBNM,.?";
                    uiTextPanel2 = this._shiftButton;
                    break;
            }

            for (var index = 0; index < str.Length; ++index)
                this._keyList[index].SetText((object) str[index].ToString());
            this._keyState = keyState;
            if (uiTextPanel2 == null)
                return;
            uiTextPanel2.BackgroundColor = new Color(93, 114, 191);
        }

        private void ValidateText()
        {
            if (this.Text.Trim().Length > 0 || this._edittingSign || (this._edittingChest || this._allowEmpty))
            {
                this._canSubmit = true;
                this._submitButton.TextColor = Color.White;
                if (this._submitButton.IsMouseHovering)
                    this._submitButton.BackgroundColor = new Color(73, 94, 171);
                else
                    this._submitButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            }
            else
            {
                this._canSubmit = false;
                this._submitButton.TextColor = Color.Gray;
                if (this._submitButton.IsMouseHovering)
                    this._submitButton.BackgroundColor = new Color(180, 60, 60) * 0.85f;
                else
                    this._submitButton.BackgroundColor = new Color(150, 40, 40) * 0.85f;
            }
        }

        private void StyleKey<T>(UITextPanel<T> button, bool external = false)
        {
            button.PaddingLeft = 0.0f;
            button.PaddingRight = 0.0f;
            button.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            if (!external)
                button.BorderColor = this._internalBorderColor * 0.7f;
            button.OnMouseOver += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                ((UIPanel) listeningElement).BackgroundColor = new Color(73, 94, 171) * 0.85f;
                if (external)
                    return;
                ((UIPanel) listeningElement).BorderColor = this._internalBorderColorSelected * 0.85f;
            });
            button.OnMouseOut += (UIElement.MouseEvent) ((evt, listeningElement) =>
            {
                ((UIPanel) listeningElement).BackgroundColor = new Color(63, 82, 151) * 0.7f;
                if (external)
                    return;
                ((UIPanel) listeningElement).BorderColor = this._internalBorderColor * 0.7f;
            });
        }

        private UITextPanel<object> CreateKeyboardButton(object text, int x, int y, int width = 1, bool style = true)
        {
            var num = 516f;
            var button = new UITextPanel<object>(text, 0.4f, true);
            button.Width.Pixels = (float) (48.0 * (double) width + 4.0 * (double) (width - 1));
            button.Height.Pixels = 37f;
            button.Left.Precent = 0.5f;
            button.Left.Pixels = (float) (52.0 * (double) x - (double) num * 0.5);
            button.Top.Pixels = 41f * (float) y;
            if (style)
                this.StyleKey<object>(button, false);
            for (var index = 0; index < width; ++index)
                this._keyList[y * 10 + x + index] = button;
            return button;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Main.gameMenu)
            {
                if (PlayerInput.UsingGamepad)
                {
                    this.outerLayer2.Remove();
                    if (!this.Elements.Contains(this.outerLayer1))
                        this.Append(this.outerLayer1);
                    this.outerLayer1.Activate();
                    this.outerLayer2.Deactivate();
                    this.Recalculate();
                    this.RecalculateChildren();
                    if ((double) this._labelHeight != 0.0)
                    {
                        this._textBox.Top.Pixels = this._textBoxHeight;
                        this._label.Top.Pixels = this._labelHeight;
                        this._textBox.Recalculate();
                        this._label.Recalculate();
                        this._labelHeight = this._textBoxHeight = 0.0f;
                        UserInterface.ActiveInstance.ResetLasts();
                    }
                }
                else
                {
                    this.outerLayer1.Remove();
                    if (!this.Elements.Contains(this.outerLayer2))
                        this.Append(this.outerLayer2);
                    this.outerLayer2.Activate();
                    this.outerLayer1.Deactivate();
                    this.Recalculate();
                    this.RecalculateChildren();
                    if ((double) this._textBoxHeight == 0.0)
                    {
                        this._textBoxHeight = this._textBox.Top.Pixels;
                        this._labelHeight = this._label.Top.Pixels;
                        this._textBox.Top.Pixels += 50f;
                        this._label.Top.Pixels += 50f;
                        this._textBox.Recalculate();
                        this._label.Recalculate();
                        UserInterface.ActiveInstance.ResetLasts();
                    }
                }
            }

            if (!Main.editSign && this._edittingSign)
                IngameFancyUI.Close();
            else if (!Main.editChest && this._edittingChest)
            {
                IngameFancyUI.Close();
            }
            else
            {
                base.DrawSelf(spriteBatch);
                this.UpdateOffsetDown();
                UIVirtualKeyboard.OffsetDown = 0;
                this.SetupGamepadPoints(spriteBatch);
                PlayerInput.WritingText = true;
                Main.instance.HandleIME();
                var position = new Vector2((float) (Main.screenWidth / 2),
                    (float) (this._textBox.GetDimensions().ToRectangle().Bottom + 32));
                Main.instance.DrawWindowsIMEPanel(position, 0.5f);
                var inputText = Main.GetInputText(this.Text);
                if (this._edittingSign && Main.inputTextEnter)
                {
                    inputText += (string) (object) '\n';
                }
                else
                {
                    if (this._edittingChest && Main.inputTextEnter)
                    {
                        ChestUI.RenameChestSubmit(Main.player[Main.myPlayer]);
                        IngameFancyUI.Close();
                        return;
                    }

                    if (Main.inputTextEnter && UIVirtualKeyboard.CanSubmit)
                        UIVirtualKeyboard.Submit();
                    else if (Main.inputTextEscape)
                    {
                        if (this._edittingSign)
                            Main.InputTextSignCancel();
                        if (this._edittingChest)
                            ChestUI.RenameChestCancel();
                        IngameFancyUI.Close();
                        return;
                    }
                }

                if (IngameFancyUI.CanShowVirtualKeyboard(this._keyboardContext))
                {
                    if (inputText != this.Text)
                        this.Text = inputText;
                    if (this._edittingSign)
                        this.CopyTextToSign();
                    if (this._edittingChest)
                        this.CopyTextToChest();
                }

                var num = (byte) (((int) byte.MaxValue + (int) Main.tileColor.R * 2) / 3);
                var color = new Color((int) num, (int) num, (int) num, (int) byte.MaxValue);
                this._textBox.TextColor = Color.Lerp(Color.White, color, 0.2f);
                this._label.TextColor = Color.Lerp(Color.White, color, 0.2f);
                position = new Vector2((float) (Main.screenWidth / 2),
                    (float) (this._textBox.GetDimensions().ToRectangle().Bottom + 32));
                Main.instance.DrawWindowsIMEPanel(position, 0.5f);
            }
        }

        private void UpdateOffsetDown()
        {
            var num1 = UIVirtualKeyboard.OffsetDown - this._lastOffsetDown;
            var num2 = num1;
            if (Math.Abs(num1) < 10)
                num2 = num1;
            this._lastOffsetDown += num2;
            if (num2 == 0)
                return;
            this.Top.Pixels += (float) num2;
            this.Recalculate();
        }

        public override void OnActivate()
        {
            if (!PlayerInput.UsingGamepadUI || this._restoreButton == null)
                return;
            UILinkPointNavigator.ChangePoint(3002);
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            PlayerInput.WritingText = false;
            UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS = 0;
        }

        private void SetupGamepadPoints(SpriteBatch spriteBatch)
        {
            UILinkPointNavigator.Shortcuts.BackButtonCommand = 6;
            UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS = 1;
            var num1 = 3002;
            UILinkPointNavigator.SetPosition(3000, this._cancelButton.GetDimensions().Center());
            var point1 = UILinkPointNavigator.Points[3000];
            point1.Unlink();
            point1.Right = 3001;
            point1.Up = num1 + 40;
            UILinkPointNavigator.SetPosition(3001, this._submitButton.GetDimensions().Center());
            var point2 = UILinkPointNavigator.Points[3001];
            point2.Unlink();
            point2.Left = 3000;
            point2.Up = num1 + 49;
            for (var index1 = 0; index1 < 5; ++index1)
            {
                for (var index2 = 0; index2 < 10; ++index2)
                {
                    var index3 = index1 * 10 + index2;
                    var ID = num1 + index3;
                    if (this._keyList[index3] != null)
                    {
                        UILinkPointNavigator.SetPosition(ID, this._keyList[index3].GetDimensions().Center());
                        var point3 = UILinkPointNavigator.Points[ID];
                        point3.Unlink();
                        var num2 = index2 - 1;
                        while (num2 >= 0 && this._keyList[index1 * 10 + num2] == this._keyList[index3])
                            --num2;
                        point3.Left = num2 == -1 ? index1 * 10 + 9 + num1 : index1 * 10 + num2 + num1;
                        var index4 = index2 + 1;
                        while (index4 <= 9 && this._keyList[index1 * 10 + index4] == this._keyList[index3])
                            ++index4;
                        point3.Right = index4 == 10 || this._keyList[index3] == this._keyList[index4]
                            ? index1 * 10 + num1
                            : index1 * 10 + index4 + num1;
                        if (index1 != 0)
                            point3.Up = ID - 10;
                        point3.Down = index1 == 4 ? (index2 < 5 ? 3000 : 3001) : ID + 10;
                    }
                }
            }
        }

        public static void CycleSymbols()
        {
            if (UIVirtualKeyboard._currentInstance == null)
                return;
            switch (UIVirtualKeyboard._currentInstance._keyState)
            {
                case UIVirtualKeyboard.KeyState.Default:
                    UIVirtualKeyboard._currentInstance.SetKeyState(UIVirtualKeyboard.KeyState.Shift);
                    break;
                case UIVirtualKeyboard.KeyState.Symbol:
                    UIVirtualKeyboard._currentInstance.SetKeyState(UIVirtualKeyboard.KeyState.Default);
                    break;
                case UIVirtualKeyboard.KeyState.Shift:
                    UIVirtualKeyboard._currentInstance.SetKeyState(UIVirtualKeyboard.KeyState.Symbol);
                    break;
            }
        }

        public static void BackSpace()
        {
            if (UIVirtualKeyboard._currentInstance == null)
                return;
            Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
            UIVirtualKeyboard._currentInstance._textBox.Backspace();
            UIVirtualKeyboard._currentInstance.ValidateText();
        }

        public static bool CanSubmit
        {
            get
            {
                if (UIVirtualKeyboard._currentInstance != null)
                    return UIVirtualKeyboard._currentInstance._canSubmit;
                return false;
            }
        }

        public static void Submit()
        {
            if (UIVirtualKeyboard._currentInstance == null)
                return;
            var text = UIVirtualKeyboard._currentInstance.Text.Trim();
            if (text.Length <= 0)
                return;
            Main.PlaySound(10, -1, -1, 1, 1f, 0.0f);
            UIVirtualKeyboard._currentInstance._submitAction(text);
        }

        public static void Cancel()
        {
            if (UIVirtualKeyboard._currentInstance == null)
                return;
            Main.PlaySound(11, -1, -1, 1, 1f, 0.0f);
            UIVirtualKeyboard._currentInstance._cancelAction();
        }

        public static void Write(string text)
        {
            if (UIVirtualKeyboard._currentInstance == null)
                return;
            Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
            var flag = UIVirtualKeyboard._currentInstance.Text.Length == 0;
            UIVirtualKeyboard._currentInstance._textBox.Write(text);
            UIVirtualKeyboard._currentInstance.ValidateText();
            if (!flag || UIVirtualKeyboard._currentInstance.Text.Length <= 0 ||
                UIVirtualKeyboard._currentInstance._keyState != UIVirtualKeyboard.KeyState.Shift)
                return;
            UIVirtualKeyboard._currentInstance.SetKeyState(UIVirtualKeyboard.KeyState.Default);
        }

        public static void CursorLeft()
        {
            if (UIVirtualKeyboard._currentInstance == null)
                return;
            Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
            UIVirtualKeyboard._currentInstance._textBox.CursorLeft();
        }

        public static void CursorRight()
        {
            if (UIVirtualKeyboard._currentInstance == null)
                return;
            Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
            UIVirtualKeyboard._currentInstance._textBox.CursorRight();
        }

        public static bool CanDisplay(int keyboardContext)
        {
            if (keyboardContext == 1)
                return Main.screenHeight > 700;
            return true;
        }

        public static int KeyboardContext
        {
            get
            {
                if (UIVirtualKeyboard._currentInstance == null)
                    return -1;
                return UIVirtualKeyboard._currentInstance._keyboardContext;
            }
        }

        public static void CacheCancelledInput(int cacheMode)
        {
            if (cacheMode != 1)
                return;
            UIVirtualKeyboard._cancelCacheSign = Main.npcChatText;
        }

        private void RestoreCancelledInput(int cacheMode)
        {
            if (cacheMode != 1)
                return;
            Main.npcChatText = UIVirtualKeyboard._cancelCacheSign;
            this.Text = Main.npcChatText;
            UIVirtualKeyboard._cancelCacheSign = "";
        }

        private void CopyTextToSign()
        {
            if (!this._edittingSign)
                return;
            var sign = Main.player[Main.myPlayer].sign;
            if (sign < 0 || Main.sign[sign] == null)
                return;
            Main.npcChatText = this.Text;
        }

        private void CopyTextToChest()
        {
            if (!this._edittingChest)
                return;
            Main.npcChatText = this.Text;
        }

        public delegate void KeyboardSubmitEvent(string text);

        public enum KeyState
        {
            Default,
            Symbol,
            Shift,
        }
    }
}