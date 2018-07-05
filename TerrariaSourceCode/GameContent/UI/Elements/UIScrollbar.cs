﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UIScrollbar
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
    public class UIScrollbar : UIElement
    {
        private float _viewSize = 1f;
        private float _maxViewSize = 20f;
        private float _viewPosition;
        private bool _isDragging;
        private bool _isHoveringOverHandle;
        private float _dragYOffset;
        private Texture2D _texture;
        private Texture2D _innerTexture;

        public float ViewPosition
        {
            get { return this._viewPosition; }
            set { this._viewPosition = MathHelper.Clamp(value, 0.0f, this._maxViewSize - this._viewSize); }
        }

        public UIScrollbar()
        {
            this.Width.Set(20f, 0.0f);
            this.MaxWidth.Set(20f, 0.0f);
            this._texture = TextureManager.Load("Images/UI/Scrollbar");
            this._innerTexture = TextureManager.Load("Images/UI/ScrollbarInner");
            this.PaddingTop = 5f;
            this.PaddingBottom = 5f;
        }

        public void SetView(float viewSize, float maxViewSize)
        {
            viewSize = MathHelper.Clamp(viewSize, 0.0f, maxViewSize);
            this._viewPosition = MathHelper.Clamp(this._viewPosition, 0.0f, maxViewSize - viewSize);
            this._viewSize = viewSize;
            this._maxViewSize = maxViewSize;
        }

        public float GetValue()
        {
            return this._viewPosition;
        }

        private Rectangle GetHandleRectangle()
        {
            CalculatedStyle innerDimensions = this.GetInnerDimensions();
            if ((double) this._maxViewSize == 0.0 && (double) this._viewSize == 0.0)
            {
                this._viewSize = 1f;
                this._maxViewSize = 1f;
            }

            return new Rectangle((int) innerDimensions.X,
                (int) ((double) innerDimensions.Y + (double) innerDimensions.Height *
                       ((double) this._viewPosition / (double) this._maxViewSize)) - 3, 20,
                (int) ((double) innerDimensions.Height * ((double) this._viewSize / (double) this._maxViewSize)) + 7);
        }

        private void DrawBar(SpriteBatch spriteBatch, Texture2D texture, Rectangle dimensions, Color color)
        {
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y - 6, dimensions.Width, 6),
                new Rectangle?(new Rectangle(0, 0, texture.Width, 6)), color);
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height),
                new Rectangle?(new Rectangle(0, 6, texture.Width, 4)), color);
            spriteBatch.Draw(texture,
                new Rectangle(dimensions.X, dimensions.Y + dimensions.Height, dimensions.Width, 6),
                new Rectangle?(new Rectangle(0, texture.Height - 6, texture.Width, 6)), color);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            CalculatedStyle innerDimensions = this.GetInnerDimensions();
            if (this._isDragging)
                this._viewPosition =
                    MathHelper.Clamp(
                        (UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - this._dragYOffset) /
                        innerDimensions.Height * this._maxViewSize, 0.0f, this._maxViewSize - this._viewSize);
            Rectangle handleRectangle = this.GetHandleRectangle();
            Vector2 mousePosition = UserInterface.ActiveInstance.MousePosition;
            bool hoveringOverHandle = this._isHoveringOverHandle;
            this._isHoveringOverHandle =
                handleRectangle.Contains(new Point((int) mousePosition.X, (int) mousePosition.Y));
            if (!hoveringOverHandle && this._isHoveringOverHandle && Main.hasFocus)
                Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
            this.DrawBar(spriteBatch, this._texture, dimensions.ToRectangle(), Color.White);
            this.DrawBar(spriteBatch, this._innerTexture, handleRectangle,
                Color.White * (this._isDragging || this._isHoveringOverHandle ? 1f : 0.85f));
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            if (evt.Target != this)
                return;
            Rectangle handleRectangle = this.GetHandleRectangle();
            if (handleRectangle.Contains(new Point((int) evt.MousePosition.X, (int) evt.MousePosition.Y)))
            {
                this._isDragging = true;
                this._dragYOffset = evt.MousePosition.Y - (float) handleRectangle.Y;
            }
            else
            {
                CalculatedStyle innerDimensions = this.GetInnerDimensions();
                this._viewPosition = MathHelper.Clamp(
                    (UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y -
                     (float) (handleRectangle.Height >> 1)) / innerDimensions.Height * this._maxViewSize, 0.0f,
                    this._maxViewSize - this._viewSize);
            }
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            this._isDragging = false;
        }
    }
}