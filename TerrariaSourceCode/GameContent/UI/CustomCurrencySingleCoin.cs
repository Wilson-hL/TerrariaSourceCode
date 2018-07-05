﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.CustomCurrencySingleCoin
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI
{
    public class CustomCurrencySingleCoin : CustomCurrencySystem
    {
        public float CurrencyDrawScale = 0.8f;
        public string CurrencyTextKey = "Currency.DefenderMedals";
        public Color CurrencyTextColor = new Color(240, 100, 120);

        public CustomCurrencySingleCoin(int coinItemID, long currencyCap)
        {
            this.Include(coinItemID, 1);
            this.SetCurrencyCap(currencyCap);
        }

        public override bool TryPurchasing(int price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty,
            List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3)
        {
            var cache = this.ItemCacheCreate(inv);
            var num1 = price;
            for (var index = 0; index < slotCoins.Count; ++index)
            {
                var slotCoin = slotCoins[index];
                var num2 = num1;
                if (inv[slotCoin.X][slotCoin.Y].stack < num2)
                    num2 = inv[slotCoin.X][slotCoin.Y].stack;
                num1 -= num2;
                inv[slotCoin.X][slotCoin.Y].stack -= num2;
                if (inv[slotCoin.X][slotCoin.Y].stack == 0)
                {
                    switch (slotCoin.X)
                    {
                        case 0:
                            slotsEmpty.Add(slotCoin);
                            break;
                        case 1:
                            slotEmptyBank.Add(slotCoin);
                            break;
                        case 2:
                            slotEmptyBank2.Add(slotCoin);
                            break;
                        case 3:
                            slotEmptyBank3.Add(slotCoin);
                            break;
                    }

                    slotCoins.Remove(slotCoin);
                    --index;
                }

                if (num1 == 0)
                    break;
            }

            if (num1 == 0)
                return true;
            this.ItemCacheRestore(cache, inv);
            return false;
        }

        public override void DrawSavingsMoney(SpriteBatch sb, string text, float shopx, float shopy, long totalCoins,
            bool horizontal = false)
        {
            var index = this._valuePerUnit.Keys.ElementAt<int>(0);
            var texture2D = Main.itemTexture[index];
            if (horizontal)
            {
                var position =
                    new Vector2(
                        (float) ((double) shopx +
                                 (double) ChatManager.GetStringSize(Main.fontMouseText, text, Vector2.One, -1f).X +
                                 45.0), shopy + 50f);
                sb.Draw(texture2D, position, new Rectangle?(), Color.White, 0.0f, texture2D.Size() / 2f,
                    this.CurrencyDrawScale, SpriteEffects.None, 0.0f);
                Utils.DrawBorderStringFourWay(sb, Main.fontItemStack, totalCoins.ToString(), position.X - 11f,
                    position.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
            }
            else
            {
                var num = totalCoins > 99L ? -6 : 0;
                sb.Draw(texture2D, new Vector2(shopx + 11f, shopy + 75f), new Rectangle?(), Color.White, 0.0f,
                    texture2D.Size() / 2f, this.CurrencyDrawScale, SpriteEffects.None, 0.0f);
                Utils.DrawBorderStringFourWay(sb, Main.fontItemStack, totalCoins.ToString(), shopx + (float) num,
                    shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
            }
        }

        public override void GetPriceText(string[] lines, ref int currentLine, int price)
        {
            var color = this.CurrencyTextColor * ((float) Main.mouseTextColor / (float) byte.MaxValue);
            lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", (object) color.R,
                (object) color.G, (object) color.B, (object) Lang.tip[50].Value, (object) price,
                (object) Language.GetTextValue(this.CurrencyTextKey).ToLower());
        }
    }
}