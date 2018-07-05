﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Shaders.WaterShaderData
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.DataStructures;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Terraria.GameContent.Shaders
{
    public class WaterShaderData : ScreenShaderData
    {
        public bool _useViscosityFilter = true;
        private Vector2 _lastDistortionDrawOffset = Vector2.Zero;
        private WaterShaderData.Ripple[] _rippleQueue = new WaterShaderData.Ripple[200];
        public bool _useProjectileWaves = true;
        private bool _useNPCWaves = true;
        private bool _usePlayerWaves = true;
        private bool _useRippleWaves = true;
        private bool _useCustomWaves = true;
        private bool _clearNextFrame = true;
        private Texture2D[] _viscosityMaskChain = new Texture2D[3];
        private bool _isWaveBufferDirty = true;
        private const float DISTORTION_BUFFER_SCALE = 0.25f;
        private const float WAVE_FRAMERATE = 0.01666667f;
        private const int MAX_RIPPLES_QUEUED = 200;
        private const int MAX_QUEUED_STEPS = 2;
        private RenderTarget2D _distortionTarget;
        private RenderTarget2D _distortionTargetSwap;
        private bool _usingRenderTargets;
        private float _progress;
        private int _rippleQueueCount;
        private int _lastScreenWidth;
        private int _lastScreenHeight;
        private int _activeViscosityMask;
        private Texture2D _rippleShapeTexture;
        private int _queuedSteps;

        public event Action<TileBatch> OnWaveDraw;

        public WaterShaderData(string passName)
            : base(passName)
        {
            Main.OnRenderTargetsInitialized += new ResolutionChangeEvent(this.InitRenderTargets);
            Main.OnRenderTargetsReleased += new Action(this.ReleaseRenderTargets);
            this._rippleShapeTexture = Main.instance.OurLoad<Texture2D>("Images/Misc/Ripples");
            Main.OnPreDraw += new Action<GameTime>(this.PreDraw);
        }

        public override void Update(GameTime gameTime)
        {
            this._useViscosityFilter = Main.WaveQuality >= 3;
            this._useProjectileWaves = Main.WaveQuality >= 3;
            this._usePlayerWaves = Main.WaveQuality >= 2;
            this._useRippleWaves = Main.WaveQuality >= 2;
            this._useCustomWaves = Main.WaveQuality >= 2;
            if (Main.gamePaused || !Main.hasFocus)
                return;
            this._progress += (float) (gameTime.ElapsedGameTime.TotalSeconds * (double) this.Intensity * 0.75);
            this._progress %= 86400f;
            if (this._useProjectileWaves || this._useRippleWaves || (this._useCustomWaves || this._usePlayerWaves))
                ++this._queuedSteps;
            base.Update(gameTime);
        }

        private void StepLiquids()
        {
            this._isWaveBufferDirty = true;
            var vector2_1 = Main.drawToScreen
                ? Vector2.Zero
                : new Vector2((float) Main.offScreenRange, (float) Main.offScreenRange);
            var vector2_2 = vector2_1 - Main.screenPosition;
            var tileBatch = Main.tileBatch;
            var graphicsDevice = Main.instance.GraphicsDevice;
            graphicsDevice.SetRenderTarget(this._distortionTarget);
            if (this._clearNextFrame)
            {
                graphicsDevice.Clear(new Color(0.5f, 0.5f, 0.0f, 1f));
                this._clearNextFrame = false;
            }

            this.DrawWaves();
            graphicsDevice.SetRenderTarget(this._distortionTargetSwap);
            graphicsDevice.Clear(new Color(0.5f, 0.5f, 0.5f, 1f));
            Main.tileBatch.Begin();
            var vector2_3 = vector2_2 * 0.25f;
            vector2_3.X = (float) Math.Floor((double) vector2_3.X);
            vector2_3.Y = (float) Math.Floor((double) vector2_3.Y);
            var vector2_4 = vector2_3 - this._lastDistortionDrawOffset;
            this._lastDistortionDrawOffset = vector2_3;
            tileBatch.Draw((Texture2D) this._distortionTarget,
                new Vector4(vector2_4.X, vector2_4.Y, (float) this._distortionTarget.Width,
                    (float) this._distortionTarget.Height), new VertexColors(Color.White));
            GameShaders.Misc["WaterProcessor"]
                .Apply(new DrawData?(new DrawData((Texture2D) this._distortionTarget, Vector2.Zero, Color.White)));
            tileBatch.End();
            var distortionTarget = this._distortionTarget;
            this._distortionTarget = this._distortionTargetSwap;
            this._distortionTargetSwap = distortionTarget;
            if (this._useViscosityFilter)
            {
                LiquidRenderer.Instance.SetWaveMaskData(ref this._viscosityMaskChain[this._activeViscosityMask]);
                tileBatch.Begin();
                var cachedDrawArea = LiquidRenderer.Instance.GetCachedDrawArea();
                var rectangle = new Rectangle(0, 0, cachedDrawArea.Height, cachedDrawArea.Width);
                var vector4 = new Vector4((float) (cachedDrawArea.X + cachedDrawArea.Width),
                                      (float) cachedDrawArea.Y, (float) cachedDrawArea.Height,
                                      (float) cachedDrawArea.Width) * 16f;
                vector4.X -= vector2_1.X;
                vector4.Y -= vector2_1.Y;
                var destination = vector4 * 0.25f;
                destination.X += vector2_3.X;
                destination.Y += vector2_3.Y;
                graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                tileBatch.Draw(this._viscosityMaskChain[this._activeViscosityMask], destination,
                    new Rectangle?(rectangle), new VertexColors(Color.White), Vector2.Zero,
                    SpriteEffects.FlipHorizontally, 1.570796f);
                tileBatch.End();
                ++this._activeViscosityMask;
                this._activeViscosityMask %= this._viscosityMaskChain.Length;
            }

            graphicsDevice.SetRenderTarget((RenderTarget2D) null);
        }

        private void DrawWaves()
        {
            var screenPosition = Main.screenPosition;
            var vector2_1 = -this._lastDistortionDrawOffset / 0.25f + (Main.drawToScreen
                                    ? Vector2.Zero
                                    : new Vector2((float) Main.offScreenRange, (float) Main.offScreenRange));
            var tileBatch = Main.tileBatch;
            var graphicsDevice = Main.instance.GraphicsDevice;
            var dimensions1 = new Vector2((float) Main.screenWidth, (float) Main.screenHeight);
            var vector2_2 = new Vector2(16f, 16f);
            tileBatch.Begin();
            GameShaders.Misc["WaterDistortionObject"].Apply(new DrawData?());
            if (this._useNPCWaves)
            {
                for (var index = 0; index < 200; ++index)
                {
                    if (Main.npc[index] != null && Main.npc[index].active &&
                        (Main.npc[index].wet || Main.npc[index].wetCount != (byte) 0) &&
                        Collision.CheckAABBvAABBCollision(screenPosition, dimensions1,
                            Main.npc[index].position - vector2_2, Main.npc[index].Size + vector2_2))
                    {
                        var npc = Main.npc[index];
                        var vector2_3 = npc.Center - vector2_1;
                        var vector2_4 = npc.velocity.RotatedBy(-(double) npc.rotation, new Vector2()) /
                                            new Vector2((float) npc.height, (float) npc.width);
                        var num1 = vector2_4.LengthSquared();
                        var num2 =
                            Math.Min(
                                (float) ((double) num1 * 0.300000011920929 + 0.699999988079071 * (double) num1 *
                                         (1024.0 / (double) (npc.height * npc.width))), 0.08f) +
                            (npc.velocity - npc.oldVelocity).Length() * 0.5f;
                        vector2_4.Normalize();
                        var velocity = npc.velocity;
                        velocity.Normalize();
                        vector2_3 -= velocity * 10f;
                        if (!this._useViscosityFilter && (npc.honeyWet || npc.lavaWet))
                            num2 *= 0.3f;
                        if (npc.wet)
                            tileBatch.Draw(Main.magicPixel,
                                new Vector4(vector2_3.X, vector2_3.Y, (float) npc.width * 2f, (float) npc.height * 2f) *
                                0.25f, new Rectangle?(),
                                new VertexColors(new Color((float) ((double) vector2_4.X * 0.5 + 0.5),
                                    (float) ((double) vector2_4.Y * 0.5 + 0.5), 0.5f * num2)),
                                new Vector2((float) Main.magicPixel.Width / 2f, (float) Main.magicPixel.Height / 2f),
                                SpriteEffects.None, npc.rotation);
                        if (npc.wetCount != (byte) 0)
                        {
                            var num3 = 0.195f * (float) Math.Sqrt((double) npc.velocity.Length());
                            var num4 = 5f;
                            if (!npc.wet)
                                num4 = -20f;
                            this.QueueRipple(npc.Center + velocity * num4,
                                new Color(0.5f, (float) ((npc.wet ? (double) num3 : -(double) num3) * 0.5 + 0.5), 0.0f,
                                    1f) * 0.5f,
                                new Vector2((float) npc.width, (float) npc.height * ((float) npc.wetCount / 9f)) *
                                MathHelper.Clamp(num3 * 10f, 0.0f, 1f), RippleShape.Circle, 0.0f);
                        }
                    }
                }
            }

            if (this._usePlayerWaves)
            {
                for (var index = 0; index < (int) byte.MaxValue; ++index)
                {
                    if (Main.player[index] != null && Main.player[index].active &&
                        (Main.player[index].wet || Main.player[index].wetCount != (byte) 0) &&
                        Collision.CheckAABBvAABBCollision(screenPosition, dimensions1,
                            Main.player[index].position - vector2_2, Main.player[index].Size + vector2_2))
                    {
                        var player = Main.player[index];
                        var vector2_3 = player.Center - vector2_1;
                        var num1 = 0.05f * (float) Math.Sqrt((double) player.velocity.Length());
                        var velocity = player.velocity;
                        velocity.Normalize();
                        var vector2_4 = vector2_3 - velocity * 10f;
                        if (!this._useViscosityFilter && (player.honeyWet || player.lavaWet))
                            num1 *= 0.3f;
                        if (player.wet)
                            tileBatch.Draw(Main.magicPixel,
                                new Vector4(vector2_4.X - (float) ((double) player.width * 2.0 * 0.5),
                                    vector2_4.Y - (float) ((double) player.height * 2.0 * 0.5),
                                    (float) player.width * 2f, (float) player.height * 2f) * 0.25f,
                                new VertexColors(new Color((float) ((double) velocity.X * 0.5 + 0.5),
                                    (float) ((double) velocity.Y * 0.5 + 0.5), 0.5f * num1)));
                        if (player.wetCount != (byte) 0)
                        {
                            var num2 = 5f;
                            if (!player.wet)
                                num2 = -20f;
                            var num3 = num1 * 3f;
                            this.QueueRipple(player.Center + velocity * num2, player.wet ? num3 : -num3,
                                new Vector2((float) player.width,
                                    (float) player.height * ((float) player.wetCount / 9f)) *
                                MathHelper.Clamp(num3 * 10f, 0.0f, 1f), RippleShape.Circle, 0.0f);
                        }
                    }
                }
            }

            if (this._useProjectileWaves)
            {
                for (var index = 0; index < 1000; ++index)
                {
                    var projectile = Main.projectile[index];
                    var flag1 = projectile.wet && !projectile.lavaWet && !projectile.honeyWet;
                    var flag2 = projectile.lavaWet;
                    var flag3 = projectile.honeyWet;
                    var flag4 = projectile.wet;
                    if (projectile.ignoreWater)
                        flag4 = true;
                    if (projectile != null && projectile.active &&
                        (ProjectileID.Sets.CanDistortWater[projectile.type] && flag4) &&
                        (!ProjectileID.Sets.NoLiquidDistortion[projectile.type] &&
                         Collision.CheckAABBvAABBCollision(screenPosition, dimensions1, projectile.position - vector2_2,
                             projectile.Size + vector2_2)))
                    {
                        if (projectile.ignoreWater)
                        {
                            var flag5 = Collision.LavaCollision(projectile.position, projectile.width,
                                projectile.height);
                            flag2 = Collision.WetCollision(projectile.position, projectile.width, projectile.height);
                            flag3 = Collision.honey;
                            if (!flag5 && !flag2 && !flag3)
                                continue;
                        }

                        var vector2_3 = projectile.Center - vector2_1;
                        var num = 2f * (float) Math.Sqrt(0.0500000007450581 * (double) projectile.velocity.Length());
                        var velocity = projectile.velocity;
                        velocity.Normalize();
                        if (!this._useViscosityFilter && (flag3 || flag2))
                            num *= 0.3f;
                        var z = Math.Max(12f, (float) projectile.width * 0.75f);
                        var w = Math.Max(12f, (float) projectile.height * 0.75f);
                        tileBatch.Draw(Main.magicPixel,
                            new Vector4(vector2_3.X - z * 0.5f, vector2_3.Y - w * 0.5f, z, w) * 0.25f,
                            new VertexColors(new Color((float) ((double) velocity.X * 0.5 + 0.5),
                                (float) ((double) velocity.Y * 0.5 + 0.5), num * 0.5f)));
                    }
                }
            }

            tileBatch.End();
            if (this._useRippleWaves)
            {
                tileBatch.Begin();
                for (var index = 0; index < this._rippleQueueCount; ++index)
                {
                    var vector2_3 = this._rippleQueue[index].Position - vector2_1;
                    var size = this._rippleQueue[index].Size;
                    var sourceRectangle = this._rippleQueue[index].SourceRectangle;
                    var rippleShapeTexture = this._rippleShapeTexture;
                    tileBatch.Draw(rippleShapeTexture, new Vector4(vector2_3.X, vector2_3.Y, size.X, size.Y) * 0.25f,
                        new Rectangle?(sourceRectangle), new VertexColors(this._rippleQueue[index].WaveData),
                        new Vector2((float) (sourceRectangle.Width / 2), (float) (sourceRectangle.Height / 2)),
                        SpriteEffects.None, this._rippleQueue[index].Rotation);
                }

                tileBatch.End();
            }

            this._rippleQueueCount = 0;
            if (!this._useCustomWaves || this.OnWaveDraw == null)
                return;
            tileBatch.Begin();
            this.OnWaveDraw(tileBatch);
            tileBatch.End();
        }

        private void PreDraw(GameTime gameTime)
        {
            this.ValidateRenderTargets();
            if (!this._usingRenderTargets || !Main.IsGraphicsDeviceAvailable)
                return;
            if (this._useProjectileWaves || this._useRippleWaves || (this._useCustomWaves || this._usePlayerWaves))
            {
                for (var index = 0; index < Math.Min(this._queuedSteps, 2); ++index)
                    this.StepLiquids();
            }
            else if (this._isWaveBufferDirty || this._clearNextFrame)
            {
                var graphicsDevice = Main.instance.GraphicsDevice;
                graphicsDevice.SetRenderTarget(this._distortionTarget);
                graphicsDevice.Clear(new Color(0.5f, 0.5f, 0.0f, 1f));
                this._clearNextFrame = false;
                this._isWaveBufferDirty = false;
                graphicsDevice.SetRenderTarget((RenderTarget2D) null);
            }

            this._queuedSteps = 0;
        }

        public override void Apply()
        {
            if (!this._usingRenderTargets || !Main.IsGraphicsDeviceAvailable)
                return;
            this.UseProgress(this._progress);
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            var vector2_1 = new Vector2((float) Main.screenWidth, (float) Main.screenHeight) * 0.5f *
                                (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);
            var vector2_2 =
                (Main.drawToScreen
                    ? Vector2.Zero
                    : new Vector2((float) Main.offScreenRange, (float) Main.offScreenRange)) - Main.screenPosition -
                vector2_1;
            this.UseImage((Texture2D) this._distortionTarget, 1, (SamplerState) null);
            this.UseImage((Texture2D) Main.waterTarget, 2, SamplerState.PointClamp);
            this.UseTargetPosition(Main.screenPosition - Main.sceneWaterPos +
                                   new Vector2((float) Main.offScreenRange, (float) Main.offScreenRange) + vector2_1);
            this.UseImageOffset(-(vector2_2 * 0.25f - this._lastDistortionDrawOffset) /
                                new Vector2((float) this._distortionTarget.Width,
                                    (float) this._distortionTarget.Height));
            base.Apply();
        }

        private void ValidateRenderTargets()
        {
            var backBufferWidth = Main.instance.GraphicsDevice.PresentationParameters.BackBufferWidth;
            var backBufferHeight = Main.instance.GraphicsDevice.PresentationParameters.BackBufferHeight;
            var flag = !Main.drawToScreen;
            if (this._usingRenderTargets && !flag)
                this.ReleaseRenderTargets();
            else if (!this._usingRenderTargets && flag)
            {
                this.InitRenderTargets(backBufferWidth, backBufferHeight);
            }
            else
            {
                if (!this._usingRenderTargets || !flag || !this._distortionTarget.IsContentLost &&
                    !this._distortionTargetSwap.IsContentLost)
                    return;
                this._clearNextFrame = true;
            }
        }

        private void InitRenderTargets(int width, int height)
        {
            this._lastScreenWidth = width;
            this._lastScreenHeight = height;
            width = (int) ((double) width * 0.25);
            height = (int) ((double) height * 0.25);
            try
            {
                this._distortionTarget = new RenderTarget2D(Main.instance.GraphicsDevice, width, height, false,
                    SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
                this._distortionTargetSwap = new RenderTarget2D(Main.instance.GraphicsDevice, width, height, false,
                    SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
                this._usingRenderTargets = true;
                this._clearNextFrame = true;
            }
            catch (Exception ex)
            {
                Lighting.lightMode = 2;
                this._usingRenderTargets = false;
                Console.WriteLine("Failed to create water distortion render targets. " + ex.ToString());
            }
        }

        private void ReleaseRenderTargets()
        {
            try
            {
                if (this._distortionTarget != null)
                    this._distortionTarget.Dispose();
                if (this._distortionTargetSwap != null)
                    this._distortionTargetSwap.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error disposing of water distortion render targets. " + ex.ToString());
            }

            this._distortionTarget = (RenderTarget2D) null;
            this._distortionTargetSwap = (RenderTarget2D) null;
            this._usingRenderTargets = false;
        }

        public void QueueRipple(Vector2 position, float strength = 1f, RippleShape shape = RippleShape.Square,
            float rotation = 0.0f)
        {
            var g = (float) ((double) strength * 0.5 + 0.5);
            var num = Math.Min(Math.Abs(strength), 1f);
            this.QueueRipple(position, new Color(0.5f, g, 0.0f, 1f) * num,
                new Vector2(4f * Math.Max(Math.Abs(strength), 1f)), shape, rotation);
        }

        public void QueueRipple(Vector2 position, float strength, Vector2 size, RippleShape shape = RippleShape.Square,
            float rotation = 0.0f)
        {
            var g = (float) ((double) strength * 0.5 + 0.5);
            var num = Math.Min(Math.Abs(strength), 1f);
            this.QueueRipple(position, new Color(0.5f, g, 0.0f, 1f) * num, size, shape, rotation);
        }

        public void QueueRipple(Vector2 position, Color waveData, Vector2 size, RippleShape shape = RippleShape.Square,
            float rotation = 0.0f)
        {
            if (!this._useRippleWaves || Main.drawToScreen)
            {
                this._rippleQueueCount = 0;
            }
            else
            {
                if (this._rippleQueueCount >= this._rippleQueue.Length)
                    return;
                this._rippleQueue[this._rippleQueueCount++] =
                    new WaterShaderData.Ripple(position, waveData, size, shape, rotation);
            }
        }

        private struct Ripple
        {
            private static readonly Rectangle[] RIPPLE_SHAPE_SOURCE_RECTS = new Rectangle[3]
                {new Rectangle(0, 0, 0, 0), new Rectangle(1, 1, 62, 62), new Rectangle(1, 65, 62, 62)};

            public readonly Vector2 Position;
            public readonly Color WaveData;
            public readonly Vector2 Size;
            public readonly RippleShape Shape;
            public readonly float Rotation;

            public Rectangle SourceRectangle
            {
                get { return WaterShaderData.Ripple.RIPPLE_SHAPE_SOURCE_RECTS[(int) this.Shape]; }
            }

            public Ripple(Vector2 position, Color waveData, Vector2 size, RippleShape shape, float rotation)
            {
                this.Position = position;
                this.WaveData = waveData;
                this.Size = size;
                this.Shape = shape;
                this.Rotation = rotation;
            }
        }
    }
}