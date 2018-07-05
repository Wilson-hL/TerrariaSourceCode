﻿// Decompiled with JetBrains decompiler
// Type: Terraria.UI.Chat.ChatManager
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;

namespace Terraria.UI.Chat
{
    public static class ChatManager
    {
        public static readonly ChatCommandProcessor Commands = new ChatCommandProcessor();

        private static ConcurrentDictionary<string, ITagHandler> _handlers =
            new ConcurrentDictionary<string, ITagHandler>();

        public static readonly Vector2[] ShadowDirections = new Vector2[4]
        {
            -Vector2.UnitX,
            Vector2.UnitX,
            -Vector2.UnitY,
            Vector2.UnitY
        };

        public static Color WaveColor(Color color)
        {
            var num = (float) Main.mouseTextColor / (float) byte.MaxValue;
            color = Color.Lerp(color, Color.Black, 1f - num);
            color.A = Main.mouseTextColor;
            return color;
        }

        public static void ConvertNormalSnippets(TextSnippet[] snippets)
        {
            for (var index = 0; index < snippets.Length; ++index)
            {
                var snippet = snippets[index];
                if (snippets[index].GetType() == typeof(TextSnippet))
                {
                    var plainSnippet =
                        new PlainTagHandler.PlainSnippet(snippet.Text, snippet.Color, snippet.Scale);
                    snippets[index] = (TextSnippet) plainSnippet;
                }
            }
        }

        public static void Register<T>(params string[] names) where T : ITagHandler, new()
        {
            var obj = new T();
            for (var index = 0; index < names.Length; ++index)
                ChatManager._handlers[names[index].ToLower()] = (ITagHandler) obj;
        }

        private static ITagHandler GetHandler(string tagName)
        {
            var lower = tagName.ToLower();
            if (ChatManager._handlers.ContainsKey(lower))
                return ChatManager._handlers[lower];
            return (ITagHandler) null;
        }

        public static List<TextSnippet> ParseMessage(string text, Color baseColor)
        {
            var matchCollection = ChatManager.Regexes.Format.Matches(text);
            var textSnippetList = new List<TextSnippet>();
            var startIndex = 0;
            foreach (Match match in matchCollection)
            {
                if (match.Index > startIndex)
                    textSnippetList.Add(new TextSnippet(text.Substring(startIndex, match.Index - startIndex), baseColor,
                        1f));
                startIndex = match.Index + match.Length;
                var tagName = match.Groups["tag"].Value;
                var text1 = match.Groups[nameof(text)].Value;
                var options = match.Groups["options"].Value;
                var handler = ChatManager.GetHandler(tagName);
                if (handler != null)
                {
                    textSnippetList.Add(handler.Parse(text1, baseColor, options));
                    textSnippetList[textSnippetList.Count - 1].TextOriginal = match.ToString();
                }
                else
                    textSnippetList.Add(new TextSnippet(text1, baseColor, 1f));
            }

            if (text.Length > startIndex)
                textSnippetList.Add(
                    new TextSnippet(text.Substring(startIndex, text.Length - startIndex), baseColor, 1f));
            return textSnippetList;
        }

        public static bool AddChatText(DynamicSpriteFont font, string text, Vector2 baseScale)
        {
            var num = Main.screenWidth - 330;
            if ((double) ChatManager.GetStringSize(font, Main.chatText + text, baseScale, -1f).X > (double) num)
                return false;
            Main.chatText += text;
            return true;
        }

        public static Vector2 GetStringSize(DynamicSpriteFont font, string text, Vector2 baseScale,
            float maxWidth = -1f)
        {
            var array = ChatManager.ParseMessage(text, Color.White).ToArray();
            return ChatManager.GetStringSize(font, array, baseScale, maxWidth);
        }

        public static Vector2 GetStringSize(DynamicSpriteFont font, TextSnippet[] snippets, Vector2 baseScale,
            float maxWidth = -1f)
        {
            var vec = new Vector2((float) Main.mouseX, (float) Main.mouseY);
            var zero = Vector2.Zero;
            var minimum = zero;
            var vector2_1 = minimum;
            var x = font.MeasureString(" ").X;
            var num1 = 0.0f;
            for (var index1 = 0; index1 < snippets.Length; ++index1)
            {
                var snippet = snippets[index1];
                snippet.Update();
                var scale = snippet.Scale;
                Vector2 size;
                if (snippet.UniqueDraw(true, out size, (SpriteBatch) null, new Vector2(), new Color(), 1f))
                {
                    minimum.X += size.X * baseScale.X * scale;
                    vector2_1.X = Math.Max(vector2_1.X, minimum.X);
                    vector2_1.Y = Math.Max(vector2_1.Y, minimum.Y + size.Y);
                }
                else
                {
                    var strArray1 = snippet.Text.Split('\n');
                    foreach (var str in strArray1)
                    {
                        var chArray = new char[1] {' '};
                        var strArray2 = str.Split(chArray);
                        for (var index2 = 0; index2 < strArray2.Length; ++index2)
                        {
                            if (index2 != 0)
                                minimum.X += x * baseScale.X * scale;
                            if ((double) maxWidth > 0.0)
                            {
                                var num2 = font.MeasureString(strArray2[index2]).X * baseScale.X * scale;
                                if ((double) minimum.X - (double) zero.X + (double) num2 > (double) maxWidth)
                                {
                                    minimum.X = zero.X;
                                    minimum.Y += (float) font.LineSpacing * num1 * baseScale.Y;
                                    vector2_1.Y = Math.Max(vector2_1.Y, minimum.Y);
                                    num1 = 0.0f;
                                }
                            }

                            if ((double) num1 < (double) scale)
                                num1 = scale;
                            var vector2_2 = font.MeasureString(strArray2[index2]);
                            vec.Between(minimum, minimum + vector2_2);
                            minimum.X += vector2_2.X * baseScale.X * scale;
                            vector2_1.X = Math.Max(vector2_1.X, minimum.X);
                            vector2_1.Y = Math.Max(vector2_1.Y, minimum.Y + vector2_2.Y);
                        }

                        if (strArray1.Length > 1)
                        {
                            minimum.X = zero.X;
                            minimum.Y += (float) font.LineSpacing * num1 * baseScale.Y;
                            vector2_1.Y = Math.Max(vector2_1.Y, minimum.Y);
                            num1 = 0.0f;
                        }
                    }
                }
            }

            return vector2_1;
        }

        public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, DynamicSpriteFont font,
            TextSnippet[] snippets, Vector2 position, Color baseColor, float rotation, Vector2 origin,
            Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
        {
            for (var index = 0; index < ChatManager.ShadowDirections.Length; ++index)
            {
                int hoveredSnippet;
                ChatManager.DrawColorCodedString(spriteBatch, font, snippets,
                    position + ChatManager.ShadowDirections[index] * spread, baseColor, rotation, origin, baseScale,
                    out hoveredSnippet, maxWidth, true);
            }
        }

        public static Vector2 DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font,
            TextSnippet[] snippets, Vector2 position, Color baseColor, float rotation, Vector2 origin,
            Vector2 baseScale, out int hoveredSnippet, float maxWidth, bool ignoreColors = false)
        {
            var num1 = -1;
            var vec = new Vector2((float) Main.mouseX, (float) Main.mouseY);
            var vector2_1 = position;
            var vector2_2 = vector2_1;
            var x = font.MeasureString(" ").X;
            var color = baseColor;
            var num2 = 0.0f;
            for (var index1 = 0; index1 < snippets.Length; ++index1)
            {
                var snippet = snippets[index1];
                snippet.Update();
                if (!ignoreColors)
                    color = snippet.GetVisibleColor();
                var scale = snippet.Scale;
                Vector2 size;
                if (snippet.UniqueDraw(false, out size, spriteBatch, vector2_1, color, scale))
                {
                    if (vec.Between(vector2_1, vector2_1 + size))
                        num1 = index1;
                    vector2_1.X += size.X * baseScale.X * scale;
                    vector2_2.X = Math.Max(vector2_2.X, vector2_1.X);
                }
                else
                {
                    var strArray1 = snippet.Text.Split('\n');
                    foreach (var str in strArray1)
                    {
                        var chArray = new char[1] {' '};
                        var strArray2 = str.Split(chArray);
                        for (var index2 = 0; index2 < strArray2.Length; ++index2)
                        {
                            if (index2 != 0)
                                vector2_1.X += x * baseScale.X * scale;
                            if ((double) maxWidth > 0.0)
                            {
                                var num3 = font.MeasureString(strArray2[index2]).X * baseScale.X * scale;
                                if ((double) vector2_1.X - (double) position.X + (double) num3 > (double) maxWidth)
                                {
                                    vector2_1.X = position.X;
                                    vector2_1.Y += (float) font.LineSpacing * num2 * baseScale.Y;
                                    vector2_2.Y = Math.Max(vector2_2.Y, vector2_1.Y);
                                    num2 = 0.0f;
                                }
                            }

                            if ((double) num2 < (double) scale)
                                num2 = scale;
                            DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, strArray2[index2],
                                vector2_1, color, rotation, origin, baseScale * snippet.Scale * scale,
                                SpriteEffects.None, 0.0f);
                            var vector2_3 = font.MeasureString(strArray2[index2]);
                            if (vec.Between(vector2_1, vector2_1 + vector2_3))
                                num1 = index1;
                            vector2_1.X += vector2_3.X * baseScale.X * scale;
                            vector2_2.X = Math.Max(vector2_2.X, vector2_1.X);
                        }

                        if (strArray1.Length > 1)
                        {
                            vector2_1.Y += (float) font.LineSpacing * num2 * baseScale.Y;
                            vector2_1.X = position.X;
                            vector2_2.Y = Math.Max(vector2_2.Y, vector2_1.Y);
                            num2 = 0.0f;
                        }
                    }
                }
            }

            hoveredSnippet = num1;
            return vector2_2;
        }

        public static Vector2 DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font,
            TextSnippet[] snippets, Vector2 position, float rotation, Vector2 origin, Vector2 baseScale,
            out int hoveredSnippet, float maxWidth = -1f, float spread = 2f)
        {
            ChatManager.DrawColorCodedStringShadow(spriteBatch, font, snippets, position, Color.Black, rotation, origin,
                baseScale, maxWidth, spread);
            return ChatManager.DrawColorCodedString(spriteBatch, font, snippets, position, Color.White, rotation,
                origin, baseScale, out hoveredSnippet, maxWidth, false);
        }

        public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, string text,
            Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f,
            float spread = 2f)
        {
            for (var index = 0; index < ChatManager.ShadowDirections.Length; ++index)
                ChatManager.DrawColorCodedString(spriteBatch, font, text,
                    position + ChatManager.ShadowDirections[index] * spread, baseColor, rotation, origin, baseScale,
                    maxWidth, true);
        }

        public static Vector2 DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, string text,
            Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f,
            bool ignoreColors = false)
        {
            var vector2_1 = position;
            var vector2_2 = vector2_1;
            var strArray1 = text.Split('\n');
            var x = font.MeasureString(" ").X;
            var color = baseColor;
            var num1 = 1f;
            var num2 = 0.0f;
            foreach (var str1 in strArray1)
            {
                var chArray = new char[1] {':'};
                foreach (var str2 in str1.Split(chArray))
                {
                    if (str2.StartsWith("sss"))
                    {
                        if (str2.StartsWith("sss1"))
                        {
                            if (!ignoreColors)
                                color = Color.Red;
                        }
                        else if (str2.StartsWith("sss2"))
                        {
                            if (!ignoreColors)
                                color = Color.Blue;
                        }
                        else if (str2.StartsWith("sssr") && !ignoreColors)
                            color = Color.White;
                    }
                    else
                    {
                        var strArray2 = str2.Split(' ');
                        for (var index = 0; index < strArray2.Length; ++index)
                        {
                            if (index != 0)
                                vector2_1.X += x * baseScale.X * num1;
                            if ((double) maxWidth > 0.0)
                            {
                                var num3 = font.MeasureString(strArray2[index]).X * baseScale.X * num1;
                                if ((double) vector2_1.X - (double) position.X + (double) num3 > (double) maxWidth)
                                {
                                    vector2_1.X = position.X;
                                    vector2_1.Y += (float) font.LineSpacing * num2 * baseScale.Y;
                                    vector2_2.Y = Math.Max(vector2_2.Y, vector2_1.Y);
                                    num2 = 0.0f;
                                }
                            }

                            if ((double) num2 < (double) num1)
                                num2 = num1;
                            DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, strArray2[index], vector2_1,
                                color, rotation, origin, baseScale * num1, SpriteEffects.None, 0.0f);
                            vector2_1.X += font.MeasureString(strArray2[index]).X * baseScale.X * num1;
                            vector2_2.X = Math.Max(vector2_2.X, vector2_1.X);
                        }
                    }
                }

                vector2_1.X = position.X;
                vector2_1.Y += (float) font.LineSpacing * num2 * baseScale.Y;
                vector2_2.Y = Math.Max(vector2_2.Y, vector2_1.Y);
                num2 = 0.0f;
            }

            return vector2_2;
        }

        public static Vector2 DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font,
            string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale,
            float maxWidth = -1f, float spread = 2f)
        {
            var array = ChatManager.ParseMessage(text, baseColor).ToArray();
            ChatManager.ConvertNormalSnippets(array);
            ChatManager.DrawColorCodedStringShadow(spriteBatch, font, array, position, Color.Black, rotation, origin,
                baseScale, maxWidth, spread);
            int hoveredSnippet;
            return ChatManager.DrawColorCodedString(spriteBatch, font, array, position, Color.White, rotation, origin,
                baseScale, out hoveredSnippet, maxWidth, false);
        }

        public static class Regexes
        {
            public static readonly Regex Format =
                new Regex("(?<!\\\\)\\[(?<tag>[a-zA-Z]{1,10})(\\/(?<options>[^:]+))?:(?<text>.+?)(?<!\\\\)\\]",
                    RegexOptions.Compiled);
        }
    }
}