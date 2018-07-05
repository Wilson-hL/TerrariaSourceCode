﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Chat.ItemTagHandler
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
    public class ItemTagHandler : ITagHandler
    {
        TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
        {
            var obj = new Item();
            int result1;
            if (int.TryParse(text, out result1))
                obj.netDefaults(result1);
            if (obj.type <= 0)
                return new TextSnippet(text);
            obj.stack = 1;
            if (options != null)
            {
                var strArray = options.Split(',');
                for (var index = 0; index < strArray.Length; ++index)
                {
                    if (strArray[index].Length != 0)
                    {
                        switch (strArray[index][0])
                        {
                            case 'p':
                                int result2;
                                if (int.TryParse(strArray[index].Substring(1), out result2))
                                {
                                    obj.Prefix((int) (byte) Utils.Clamp<int>(result2, 0, 84));
                                    continue;
                                }

                                continue;
                            case 's':
                            case 'x':
                                int result3;
                                if (int.TryParse(strArray[index].Substring(1), out result3))
                                {
                                    obj.stack = Utils.Clamp<int>(result3, 1, obj.maxStack);
                                    continue;
                                }

                                continue;
                            default:
                                continue;
                        }
                    }
                }
            }

            var str = "";
            if (obj.stack > 1)
                str = " (" + (object) obj.stack + ")";
            var itemSnippet = new ItemTagHandler.ItemSnippet(obj);
            itemSnippet.Text = "[" + obj.AffixName() + str + "]";
            itemSnippet.CheckForHover = true;
            itemSnippet.DeleteWhole = true;
            return (TextSnippet) itemSnippet;
        }

        public static string GenerateTag(Item I)
        {
            var str = "[i";
            if (I.prefix != (byte) 0)
                str = str + "/p" + (object) I.prefix;
            if (I.stack != 1)
                str = str + "/s" + (object) I.stack;
            return str + ":" + (object) I.netID + "]";
        }

        private class ItemSnippet : TextSnippet
        {
            private Item _item;

            public ItemSnippet(Item item)
                : base("")
            {
                this._item = item;
                this.Color = ItemRarity.GetColor(item.rare);
            }

            public override void OnHover()
            {
                Main.HoverItem = this._item.Clone();
                Main.instance.MouseText(this._item.Name, this._item.rare, (byte) 0, -1, -1, -1, -1);
            }

            public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch,
                Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
            {
                var num1 = 1f;
                var num2 = 1f;
                if (Main.netMode != 2 && !Main.dedServ)
                {
                    var texture2D = Main.itemTexture[this._item.type];
                    var rectangle = Main.itemAnimations[this._item.type] == null
                        ? texture2D.Frame(1, 1, 0, 0)
                        : Main.itemAnimations[this._item.type].GetFrame(texture2D);
                    if (rectangle.Height > 32)
                        num2 = 32f / (float) rectangle.Height;
                }

                var num3 = num2 * scale;
                var num4 = num1 * num3;
                if ((double) num4 > 0.75)
                    num4 = 0.75f;
                if (!justCheckingString && color != Color.Black)
                {
                    var inventoryScale = Main.inventoryScale;
                    Main.inventoryScale = scale * num4;
                    ItemSlot.Draw(spriteBatch, ref this._item, 14, position - new Vector2(10f) * scale * num4,
                        Color.White);
                    Main.inventoryScale = inventoryScale;
                }

                size = new Vector2(32f) * scale * num4;
                return true;
            }

            public override float GetStringLength(DynamicSpriteFont font)
            {
                return (float) (32.0 * (double) this.Scale * 0.649999976158142);
            }
        }
    }
}