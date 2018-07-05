// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.Effects.SkyManager
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Terraria.Graphics.Effects
{
    public class SkyManager : EffectManager<CustomSky>
    {
        public static SkyManager Instance = new SkyManager();
        private LinkedList<CustomSky> _activeSkies = new LinkedList<CustomSky>();
        private float _lastDepth;

        public void Reset()
        {
            foreach (var customSky in this._effects.Values)
                customSky.Reset();
            this._activeSkies.Clear();
        }

        public void Update(GameTime gameTime)
        {
            LinkedListNode<CustomSky> next;
            for (var node = this._activeSkies.First; node != null; node = next)
            {
                var customSky = node.Value;
                next = node.Next;
                customSky.Update(gameTime);
                if (!customSky.IsActive())
                    this._activeSkies.Remove(node);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.DrawDepthRange(spriteBatch, float.MinValue, float.MaxValue);
        }

        public void DrawToDepth(SpriteBatch spriteBatch, float minDepth)
        {
            if ((double) this._lastDepth <= (double) minDepth)
                return;
            this.DrawDepthRange(spriteBatch, minDepth, this._lastDepth);
            this._lastDepth = minDepth;
        }

        public void DrawDepthRange(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            foreach (var activeSky in this._activeSkies)
                activeSky.Draw(spriteBatch, minDepth, maxDepth);
        }

        public void DrawRemainingDepth(SpriteBatch spriteBatch)
        {
            this.DrawDepthRange(spriteBatch, float.MinValue, this._lastDepth);
            this._lastDepth = float.MinValue;
        }

        public void ResetDepthTracker()
        {
            this._lastDepth = float.MaxValue;
        }

        public void SetStartingDepth(float depth)
        {
            this._lastDepth = depth;
        }

        public override void OnActivate(CustomSky effect, Vector2 position)
        {
            this._activeSkies.Remove(effect);
            this._activeSkies.AddLast(effect);
        }

        public Color ProcessTileColor(Color color)
        {
            foreach (var activeSky in this._activeSkies)
                color = activeSky.OnTileColor(color);
            return color;
        }

        public float ProcessCloudAlpha()
        {
            var num = 1f;
            foreach (var activeSky in this._activeSkies)
                num *= activeSky.GetCloudAlpha();
            return MathHelper.Clamp(num, 0.0f, 1f);
        }
    }
}