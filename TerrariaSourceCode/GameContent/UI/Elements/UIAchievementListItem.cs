﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UIAchievementListItem
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Achievements;
using Terraria.Graphics;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
    public class UIAchievementListItem : UIPanel
    {
        private const int _iconSize = 64;
        private const int _iconSizeWithSpace = 66;
        private const int _iconsPerRow = 8;
        private Achievement _achievement;
        private UIImageFramed _achievementIcon;
        private UIImage _achievementIconBorders;
        private int _iconIndex;
        private Rectangle _iconFrame;
        private Rectangle _iconFrameUnlocked;
        private Rectangle _iconFrameLocked;
        private Texture2D _innerPanelTopTexture;
        private Texture2D _innerPanelBottomTexture;
        private Texture2D _categoryTexture;
        private bool _locked;
        private bool _large;

        public UIAchievementListItem(Achievement achievement, bool largeForOtherLanguages)
        {
            this._large = largeForOtherLanguages;
            this.BackgroundColor = new Color(26, 40, 89) * 0.8f;
            this.BorderColor = new Color(13, 20, 44) * 0.8f;
            var num = (float) (16 + this._large.ToInt() * 20);
            var pixels1 = (float) (this._large.ToInt() * 6);
            var pixels2 = (float) (this._large.ToInt() * 12);
            this._achievement = achievement;
            this.Height.Set(66f + num, 0.0f);
            this.Width.Set(0.0f, 1f);
            this.PaddingTop = 8f;
            this.PaddingLeft = 9f;
            var iconIndex = Main.Achievements.GetIconIndex(achievement.Name);
            this._iconIndex = iconIndex;
            this._iconFrameUnlocked = new Rectangle(iconIndex % 8 * 66, iconIndex / 8 * 66, 64, 64);
            this._iconFrameLocked = this._iconFrameUnlocked;
            this._iconFrameLocked.X += 528;
            this._iconFrame = this._iconFrameLocked;
            this.UpdateIconFrame();
            this._achievementIcon = new UIImageFramed(TextureManager.Load("Images/UI/Achievements"), this._iconFrame);
            this._achievementIcon.Left.Set(pixels1, 0.0f);
            this._achievementIcon.Top.Set(pixels2, 0.0f);
            this.Append((UIElement) this._achievementIcon);
            this._achievementIconBorders = new UIImage(TextureManager.Load("Images/UI/Achievement_Borders"));
            this._achievementIconBorders.Left.Set(pixels1 - 4f, 0.0f);
            this._achievementIconBorders.Top.Set(pixels2 - 4f, 0.0f);
            this.Append((UIElement) this._achievementIconBorders);
            this._innerPanelTopTexture = TextureManager.Load("Images/UI/Achievement_InnerPanelTop");
            this._innerPanelBottomTexture = !this._large
                ? TextureManager.Load("Images/UI/Achievement_InnerPanelBottom")
                : TextureManager.Load("Images/UI/Achievement_InnerPanelBottom_Large");
            this._categoryTexture = TextureManager.Load("Images/UI/Achievement_Categories");
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            var num1 = this._large.ToInt() * 6;
            var vector2_1 = new Vector2((float) num1, 0.0f);
            this._locked = !this._achievement.IsCompleted;
            this.UpdateIconFrame();
            var innerDimensions = this.GetInnerDimensions();
            var dimensions = this._achievementIconBorders.GetDimensions();
            var vector2_2 = new Vector2(dimensions.X + dimensions.Width + 7f, innerDimensions.Y);
            var trackerValues = this.GetTrackerValues();
            var flag = false;
            if ((!(trackerValues.Item1 == new Decimal(0)) || !(trackerValues.Item2 == new Decimal(0))) && this._locked)
                flag = true;
            var num2 = (float) ((double) innerDimensions.Width - (double) dimensions.Width + 1.0) -
                         (float) (num1 * 2);
            var baseScale1 = new Vector2(0.85f);
            var baseScale2 = new Vector2(0.92f);
            var wrappedText = Main.fontItemStack.CreateWrappedText(this._achievement.Description.Value,
                (float) (((double) num2 - 20.0) * (1.0 / (double) baseScale2.X)), Language.ActiveCulture.CultureInfo);
            var stringSize1 = ChatManager.GetStringSize(Main.fontItemStack, wrappedText, baseScale2, num2);
            if (!this._large)
                stringSize1 = ChatManager.GetStringSize(Main.fontItemStack, this._achievement.Description.Value,
                    baseScale2, num2);
            var num3 = (float) (38.0 + (this._large ? 20.0 : 0.0));
            if ((double) stringSize1.Y > (double) num3)
                baseScale2.Y *= num3 / stringSize1.Y;
            var baseColor1 = Color.Lerp(this._locked ? Color.Silver : Color.Gold, Color.White,
                this.IsMouseHovering ? 0.5f : 0.0f);
            var baseColor2 = Color.Lerp(this._locked ? Color.DarkGray : Color.Silver, Color.White,
                this.IsMouseHovering ? 1f : 0.0f);
            var color1 = this.IsMouseHovering ? Color.White : Color.Gray;
            var position1 = vector2_2 - Vector2.UnitY * 2f + vector2_1;
            this.DrawPanelTop(spriteBatch, position1, num2, color1);
            var category = this._achievement.Category;
            position1.Y += 2f;
            position1.X += 4f;
            spriteBatch.Draw(this._categoryTexture, position1,
                new Rectangle?(this._categoryTexture.Frame(4, 2, (int) category, 0)),
                this.IsMouseHovering ? Color.White : Color.Silver, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
            position1.X += 4f;
            position1.X += 17f;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack,
                this._achievement.FriendlyName.Value, position1, baseColor1, 0.0f, Vector2.Zero, baseScale1, num2, 2f);
            position1.X -= 17f;
            var position2 = vector2_2 + Vector2.UnitY * 27f + vector2_1;
            this.DrawPanelBottom(spriteBatch, position2, num2, color1);
            position2.X += 8f;
            position2.Y += 4f;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, wrappedText, position2,
                baseColor2, 0.0f, Vector2.Zero, baseScale2, -1f, 2f);
            if (!flag)
                return;
            var position3 = position1 + Vector2.UnitX * num2 + Vector2.UnitY;
            var text = ((int) trackerValues.Item1).ToString() + "/" + ((int) trackerValues.Item2).ToString();
            var baseScale3 = new Vector2(0.75f);
            var stringSize2 = ChatManager.GetStringSize(Main.fontItemStack, text, baseScale3, -1f);
            var progress = (float) (trackerValues.Item1 / trackerValues.Item2);
            var Width = 80f;
            var color2 = new Color(100, (int) byte.MaxValue, 100);
            if (!this.IsMouseHovering)
                color2 = Color.Lerp(color2, Color.Black, 0.25f);
            var BackColor = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            if (!this.IsMouseHovering)
                BackColor = Color.Lerp(BackColor, Color.Black, 0.25f);
            this.DrawProgressBar(spriteBatch, progress, position3 - Vector2.UnitX * Width * 0.7f, Width, BackColor,
                color2, color2.MultiplyRGBA(new Color(new Vector4(1f, 1f, 1f, 0.5f))));
            position3.X -= Width * 1.4f + stringSize2.X;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, text, position3, baseColor1,
                0.0f, new Vector2(0.0f, 0.0f), baseScale3, 90f, 2f);
        }

        private void UpdateIconFrame()
        {
            this._iconFrame = this._locked ? this._iconFrameLocked : this._iconFrameUnlocked;
            if (this._achievementIcon == null)
                return;
            this._achievementIcon.SetFrame(this._iconFrame);
        }

        private void DrawPanelTop(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
        {
            spriteBatch.Draw(this._innerPanelTopTexture, position,
                new Rectangle?(new Rectangle(0, 0, 2, this._innerPanelTopTexture.Height)), color);
            spriteBatch.Draw(this._innerPanelTopTexture, new Vector2(position.X + 2f, position.Y),
                new Rectangle?(new Rectangle(2, 0, 2, this._innerPanelTopTexture.Height)), color, 0.0f, Vector2.Zero,
                new Vector2((float) (((double) width - 4.0) / 2.0), 1f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(this._innerPanelTopTexture,
                new Vector2((float) ((double) position.X + (double) width - 2.0), position.Y),
                new Rectangle?(new Rectangle(4, 0, 2, this._innerPanelTopTexture.Height)), color);
        }

        private void DrawPanelBottom(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
        {
            spriteBatch.Draw(this._innerPanelBottomTexture, position,
                new Rectangle?(new Rectangle(0, 0, 6, this._innerPanelBottomTexture.Height)), color);
            spriteBatch.Draw(this._innerPanelBottomTexture, new Vector2(position.X + 6f, position.Y),
                new Rectangle?(new Rectangle(6, 0, 7, this._innerPanelBottomTexture.Height)), color, 0.0f, Vector2.Zero,
                new Vector2((float) (((double) width - 12.0) / 7.0), 1f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(this._innerPanelBottomTexture,
                new Vector2((float) ((double) position.X + (double) width - 6.0), position.Y),
                new Rectangle?(new Rectangle(13, 0, 6, this._innerPanelBottomTexture.Height)), color);
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            this.BackgroundColor = new Color(46, 60, 119);
            this.BorderColor = new Color(20, 30, 56);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
            this.BackgroundColor = new Color(26, 40, 89) * 0.8f;
            this.BorderColor = new Color(13, 20, 44) * 0.8f;
        }

        public Achievement GetAchievement()
        {
            return this._achievement;
        }

        private Tuple<Decimal, Decimal> GetTrackerValues()
        {
            if (!this._achievement.HasTracker)
                return Tuple.Create<Decimal, Decimal>(new Decimal(0), new Decimal(0));
            var tracker = this._achievement.GetTracker();
            if (tracker.GetTrackerType() == TrackerType.Int)
            {
                var achievementTracker = (AchievementTracker<int>) tracker;
                return Tuple.Create<Decimal, Decimal>((Decimal) achievementTracker.Value,
                    (Decimal) achievementTracker.MaxValue);
            }

            if (tracker.GetTrackerType() != TrackerType.Float)
                return Tuple.Create<Decimal, Decimal>(new Decimal(0), new Decimal(0));
            var achievementTracker1 = (AchievementTracker<float>) tracker;
            return Tuple.Create<Decimal, Decimal>((Decimal) achievementTracker1.Value,
                (Decimal) achievementTracker1.MaxValue);
        }

        private void DrawProgressBar(SpriteBatch spriteBatch, float progress, Vector2 spot, float Width = 169f,
            Color BackColor = default(Color), Color FillingColor = default(Color), Color BlipColor = default(Color))
        {
            if (BlipColor == Color.Transparent)
                BlipColor = new Color((int) byte.MaxValue, 165, 0, (int) sbyte.MaxValue);
            if (FillingColor == Color.Transparent)
                FillingColor = new Color((int) byte.MaxValue, 241, 51);
            if (BackColor == Color.Transparent)
                FillingColor = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
            var colorBarTexture = Main.colorBarTexture;
            var colorBlipTexture = Main.colorBlipTexture;
            var magicPixel = Main.magicPixel;
            var num1 = MathHelper.Clamp(progress, 0.0f, 1f);
            var num2 = Width * 1f;
            var y = 8f;
            var x = num2 / 169f;
            var vector2 = spot + Vector2.UnitY * y + Vector2.UnitX * 1f;
            spriteBatch.Draw(colorBarTexture, spot,
                new Rectangle?(new Rectangle(5, 0, colorBarTexture.Width - 9, colorBarTexture.Height)), BackColor, 0.0f,
                new Vector2(84.5f, 0.0f), new Vector2(x, 1f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(colorBarTexture, spot + new Vector2((float) (-(double) x * 84.5 - 5.0), 0.0f),
                new Rectangle?(new Rectangle(0, 0, 5, colorBarTexture.Height)), BackColor, 0.0f, Vector2.Zero,
                Vector2.One, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(colorBarTexture, spot + new Vector2(x * 84.5f, 0.0f),
                new Rectangle?(new Rectangle(colorBarTexture.Width - 4, 0, 4, colorBarTexture.Height)), BackColor, 0.0f,
                Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
            var position = vector2 + Vector2.UnitX * (num1 - 0.5f) * num2;
            --position.X;
            spriteBatch.Draw(magicPixel, position, new Rectangle?(new Rectangle(0, 0, 1, 1)), FillingColor, 0.0f,
                new Vector2(1f, 0.5f), new Vector2(num2 * num1, y), SpriteEffects.None, 0.0f);
            if ((double) progress != 0.0)
                spriteBatch.Draw(magicPixel, position, new Rectangle?(new Rectangle(0, 0, 1, 1)), BlipColor, 0.0f,
                    new Vector2(1f, 0.5f), new Vector2(2f, y), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(magicPixel, position, new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black, 0.0f,
                new Vector2(0.0f, 0.5f), new Vector2(num2 * (1f - num1), y), SpriteEffects.None, 0.0f);
        }

        public override int CompareTo(object obj)
        {
            var achievementListItem = obj as UIAchievementListItem;
            if (achievementListItem == null)
                return 0;
            if (this._achievement.IsCompleted && !achievementListItem._achievement.IsCompleted)
                return -1;
            if (!this._achievement.IsCompleted && achievementListItem._achievement.IsCompleted)
                return 1;
            return this._achievement.Id.CompareTo(achievementListItem._achievement.Id);
        }
    }
}