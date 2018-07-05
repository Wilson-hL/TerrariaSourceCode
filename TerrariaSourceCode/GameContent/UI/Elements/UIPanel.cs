﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UIPanel
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
    public class UIPanel : UIElement
    {
        private static int CORNER_SIZE = 12;
        private static int BAR_SIZE = 4;
        public Color BorderColor = Color.Black;
        public Color BackgroundColor = new Color(63, 82, 151) * 0.7f;
        private static Texture2D _borderTexture;
        private static Texture2D _backgroundTexture;

        public UIPanel()
        {
            if (UIPanel._borderTexture == null)
                UIPanel._borderTexture = TextureManager.Load("Images/UI/PanelBorder");
            if (UIPanel._backgroundTexture == null)
                UIPanel._backgroundTexture = TextureManager.Load("Images/UI/PanelBackground");
            this.SetPadding((float) UIPanel.CORNER_SIZE);
        }

        private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
            var dimensions = this.GetDimensions();
            var point1 = new Point((int) dimensions.X, (int) dimensions.Y);
            var point2 = new Point(point1.X + (int) dimensions.Width - UIPanel.CORNER_SIZE,
                point1.Y + (int) dimensions.Height - UIPanel.CORNER_SIZE);
            var width = point2.X - point1.X - UIPanel.CORNER_SIZE;
            var height = point2.Y - point1.Y - UIPanel.CORNER_SIZE;
            spriteBatch.Draw(texture, new Rectangle(point1.X, point1.Y, UIPanel.CORNER_SIZE, UIPanel.CORNER_SIZE),
                new Rectangle?(new Rectangle(0, 0, UIPanel.CORNER_SIZE, UIPanel.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point1.Y, UIPanel.CORNER_SIZE, UIPanel.CORNER_SIZE),
                new Rectangle?(new Rectangle(UIPanel.CORNER_SIZE + UIPanel.BAR_SIZE, 0, UIPanel.CORNER_SIZE,
                    UIPanel.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point1.X, point2.Y, UIPanel.CORNER_SIZE, UIPanel.CORNER_SIZE),
                new Rectangle?(new Rectangle(0, UIPanel.CORNER_SIZE + UIPanel.BAR_SIZE, UIPanel.CORNER_SIZE,
                    UIPanel.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, UIPanel.CORNER_SIZE, UIPanel.CORNER_SIZE),
                new Rectangle?(new Rectangle(UIPanel.CORNER_SIZE + UIPanel.BAR_SIZE,
                    UIPanel.CORNER_SIZE + UIPanel.BAR_SIZE, UIPanel.CORNER_SIZE, UIPanel.CORNER_SIZE)), color);
            spriteBatch.Draw(texture,
                new Rectangle(point1.X + UIPanel.CORNER_SIZE, point1.Y, width, UIPanel.CORNER_SIZE),
                new Rectangle?(new Rectangle(UIPanel.CORNER_SIZE, 0, UIPanel.BAR_SIZE, UIPanel.CORNER_SIZE)), color);
            spriteBatch.Draw(texture,
                new Rectangle(point1.X + UIPanel.CORNER_SIZE, point2.Y, width, UIPanel.CORNER_SIZE),
                new Rectangle?(new Rectangle(UIPanel.CORNER_SIZE, UIPanel.CORNER_SIZE + UIPanel.BAR_SIZE,
                    UIPanel.BAR_SIZE, UIPanel.CORNER_SIZE)), color);
            spriteBatch.Draw(texture,
                new Rectangle(point1.X, point1.Y + UIPanel.CORNER_SIZE, UIPanel.CORNER_SIZE, height),
                new Rectangle?(new Rectangle(0, UIPanel.CORNER_SIZE, UIPanel.CORNER_SIZE, UIPanel.BAR_SIZE)), color);
            spriteBatch.Draw(texture,
                new Rectangle(point2.X, point1.Y + UIPanel.CORNER_SIZE, UIPanel.CORNER_SIZE, height),
                new Rectangle?(new Rectangle(UIPanel.CORNER_SIZE + UIPanel.BAR_SIZE, UIPanel.CORNER_SIZE,
                    UIPanel.CORNER_SIZE, UIPanel.BAR_SIZE)), color);
            spriteBatch.Draw(texture,
                new Rectangle(point1.X + UIPanel.CORNER_SIZE, point1.Y + UIPanel.CORNER_SIZE, width, height),
                new Rectangle?(new Rectangle(UIPanel.CORNER_SIZE, UIPanel.CORNER_SIZE, UIPanel.BAR_SIZE,
                    UIPanel.BAR_SIZE)), color);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            this.DrawPanel(spriteBatch, UIPanel._backgroundTexture, this.BackgroundColor);
            this.DrawPanel(spriteBatch, UIPanel._borderTexture, this.BorderColor);
        }
    }
}