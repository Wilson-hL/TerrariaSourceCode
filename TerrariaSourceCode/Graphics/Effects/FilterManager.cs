﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.Effects.FilterManager
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.IO;

namespace Terraria.Graphics.Effects
{
    public class FilterManager : EffectManager<Filter>
    {
        private LinkedList<Filter> _activeFilters = new LinkedList<Filter>();
        private int _filterLimit = 16;
        private const float OPACITY_RATE = 1f;
        private EffectPriority _priorityThreshold;
        private int _activeFilterCount;
        private bool _captureThisFrame;

        public event Action OnPostDraw;

        public FilterManager()
        {
            Main.Configuration.OnLoad += (Action<Preferences>) (preferences =>
            {
                this._filterLimit = preferences.Get<int>("FilterLimit", 16);
                EffectPriority result;
                if (!Enum.TryParse<EffectPriority>(preferences.Get<string>("FilterPriorityThreshold", "VeryLow"),
                    out result))
                    return;
                this._priorityThreshold = result;
            });
            Main.Configuration.OnSave += (Action<Preferences>) (preferences =>
            {
                preferences.Put("FilterLimit", (object) this._filterLimit);
                preferences.Put("FilterPriorityThreshold",
                    (object) Enum.GetName(typeof(EffectPriority), (object) this._priorityThreshold));
            });
        }

        public override void OnActivate(Filter effect, Vector2 position)
        {
            if (this._activeFilters.Contains(effect))
            {
                if (effect.Active)
                    return;
                if (effect.Priority >= this._priorityThreshold)
                    --this._activeFilterCount;
                this._activeFilters.Remove(effect);
            }
            else
                effect.Opacity = 0.0f;

            if (effect.Priority >= this._priorityThreshold)
                ++this._activeFilterCount;
            if (this._activeFilters.Count == 0)
            {
                this._activeFilters.AddLast(effect);
            }
            else
            {
                for (var node = this._activeFilters.First; node != null; node = node.Next)
                {
                    var filter = node.Value;
                    if (effect.Priority <= filter.Priority)
                    {
                        this._activeFilters.AddAfter(node, effect);
                        return;
                    }
                }

                this._activeFilters.AddLast(effect);
            }
        }

        public void BeginCapture()
        {
            if (this._activeFilterCount == 0 && this.OnPostDraw == null)
            {
                this._captureThisFrame = false;
            }
            else
            {
                this._captureThisFrame = true;
                Main.instance.GraphicsDevice.SetRenderTarget(Main.screenTarget);
                Main.instance.GraphicsDevice.Clear(Color.Black);
            }
        }

        public void Update(GameTime gameTime)
        {
            var node = this._activeFilters.First;
            var count = this._activeFilters.Count;
            var num = 0;
            LinkedListNode<Filter> next;
            for (; node != null; node = next)
            {
                var filter = node.Value;
                next = node.Next;
                var flag = false;
                if (filter.Priority >= this._priorityThreshold)
                {
                    ++num;
                    if (num > this._activeFilterCount - this._filterLimit)
                    {
                        filter.Update(gameTime);
                        flag = true;
                    }
                }

                if (filter.Active && flag)
                    filter.Opacity = Math.Min(filter.Opacity + (float) (gameTime.ElapsedGameTime.TotalSeconds * 1.0),
                        1f);
                else
                    filter.Opacity = Math.Max(filter.Opacity - (float) (gameTime.ElapsedGameTime.TotalSeconds * 1.0),
                        0.0f);
                if (!filter.Active && (double) filter.Opacity == 0.0)
                {
                    if (filter.Priority >= this._priorityThreshold)
                        --this._activeFilterCount;
                    this._activeFilters.Remove(node);
                }
            }
        }

        public void EndCapture()
        {
            if (!this._captureThisFrame)
                return;
            var linkedListNode = this._activeFilters.First;
            var count = this._activeFilters.Count;
            var filter1 = (Filter) null;
            var renderTarget2D = Main.screenTarget;
            var graphicsDevice = Main.instance.GraphicsDevice;
            var num = 0;
            if ((double) Main.player[Main.myPlayer].gravDir == -1.0)
            {
                var screenTargetSwap = Main.screenTargetSwap;
                graphicsDevice.SetRenderTarget(screenTargetSwap);
                graphicsDevice.Clear(Color.Black);
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                    DepthStencilState.Default, RasterizerState.CullNone, (Effect) null,
                    Matrix.Invert(Main.GameViewMatrix.EffectMatrix));
                Main.spriteBatch.Draw((Texture2D) renderTarget2D, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
                renderTarget2D = Main.screenTargetSwap;
            }

            LinkedListNode<Filter> next;
            for (; linkedListNode != null; linkedListNode = next)
            {
                var filter2 = linkedListNode.Value;
                next = linkedListNode.Next;
                if (filter2.Priority >= this._priorityThreshold)
                {
                    ++num;
                    if (num > this._activeFilterCount - this._filterLimit && filter2.IsVisible())
                    {
                        if (filter1 != null)
                        {
                            var renderTarget = renderTarget2D != Main.screenTarget
                                ? Main.screenTarget
                                : Main.screenTargetSwap;
                            graphicsDevice.SetRenderTarget(renderTarget);
                            graphicsDevice.Clear(Color.Black);
                            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                            filter1.Apply();
                            Main.spriteBatch.Draw((Texture2D) renderTarget2D, Vector2.Zero, Main.bgColor);
                            Main.spriteBatch.End();
                            renderTarget2D = renderTarget2D != Main.screenTarget
                                ? Main.screenTarget
                                : Main.screenTargetSwap;
                        }

                        filter1 = filter2;
                    }
                }
            }

            graphicsDevice.SetRenderTarget((RenderTarget2D) null);
            graphicsDevice.Clear(Color.Black);
            if ((double) Main.player[Main.myPlayer].gravDir == -1.0)
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                    DepthStencilState.Default, RasterizerState.CullNone, (Effect) null,
                    Main.GameViewMatrix.EffectMatrix);
            else
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (filter1 != null)
            {
                filter1.Apply();
                Main.spriteBatch.Draw((Texture2D) renderTarget2D, Vector2.Zero, Main.bgColor);
            }
            else
                Main.spriteBatch.Draw((Texture2D) renderTarget2D, Vector2.Zero, Color.White);

            Main.spriteBatch.End();
            for (var index = 0; index < 8; ++index)
                graphicsDevice.Textures[index] = (Texture) null;
            if (this.OnPostDraw == null)
                return;
            this.OnPostDraw();
        }

        public bool HasActiveFilter()
        {
            return this._activeFilters.Count != 0;
        }

        public bool CanCapture()
        {
            if (!this.HasActiveFilter())
                return this.OnPostDraw != null;
            return true;
        }
    }
}