﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.Effects.OverlayManager
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Terraria.Graphics.Effects
{
    public class OverlayManager : EffectManager<Overlay>
    {
        private LinkedList<Overlay>[] _activeOverlays =
            new LinkedList<Overlay>[Enum.GetNames(typeof(EffectPriority)).Length];

        private const float OPACITY_RATE = 1f;
        private int _overlayCount;

        public OverlayManager()
        {
            for (var index = 0; index < this._activeOverlays.Length; ++index)
                this._activeOverlays[index] = new LinkedList<Overlay>();
        }

        public override void OnActivate(Overlay overlay, Vector2 position)
        {
            var activeOverlay = this._activeOverlays[(int) overlay.Priority];
            if (overlay.Mode == OverlayMode.FadeIn || overlay.Mode == OverlayMode.Active)
                return;
            if (overlay.Mode == OverlayMode.FadeOut)
            {
                activeOverlay.Remove(overlay);
                --this._overlayCount;
            }
            else
                overlay.Opacity = 0.0f;

            if (activeOverlay.Count != 0)
            {
                foreach (var overlay1 in activeOverlay)
                    overlay1.Mode = OverlayMode.FadeOut;
            }

            activeOverlay.AddLast(overlay);
            ++this._overlayCount;
        }

        public void Update(GameTime gameTime)
        {
            LinkedListNode<Overlay> next;
            for (var index = 0; index < this._activeOverlays.Length; ++index)
            {
                for (var node = this._activeOverlays[index].First; node != null; node = next)
                {
                    var overlay = node.Value;
                    next = node.Next;
                    overlay.Update(gameTime);
                    switch (overlay.Mode)
                    {
                        case OverlayMode.FadeIn:
                            overlay.Opacity += (float) (gameTime.ElapsedGameTime.TotalSeconds * 1.0);
                            if ((double) overlay.Opacity >= 1.0)
                            {
                                overlay.Opacity = 1f;
                                overlay.Mode = OverlayMode.Active;
                                break;
                            }

                            break;
                        case OverlayMode.Active:
                            overlay.Opacity = Math.Min(1f,
                                overlay.Opacity + (float) (gameTime.ElapsedGameTime.TotalSeconds * 1.0));
                            break;
                        case OverlayMode.FadeOut:
                            overlay.Opacity -= (float) (gameTime.ElapsedGameTime.TotalSeconds * 1.0);
                            if ((double) overlay.Opacity <= 0.0)
                            {
                                overlay.Opacity = 0.0f;
                                overlay.Mode = OverlayMode.Inactive;
                                this._activeOverlays[index].Remove(node);
                                --this._overlayCount;
                                break;
                            }

                            break;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, RenderLayers layer)
        {
            if (this._overlayCount == 0)
                return;
            var flag = false;
            for (var index = 0; index < this._activeOverlays.Length; ++index)
            {
                for (var linkedListNode = this._activeOverlays[index].First;
                    linkedListNode != null;
                    linkedListNode = linkedListNode.Next)
                {
                    var overlay = linkedListNode.Value;
                    if (overlay.Layer == layer && overlay.IsVisible())
                    {
                        if (!flag)
                        {
                            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                                DepthStencilState.Default, RasterizerState.CullNone, (Effect) null, Main.Transform);
                            flag = true;
                        }

                        overlay.Draw(spriteBatch);
                    }
                }
            }

            if (!flag)
                return;
            spriteBatch.End();
        }
    }
}