﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.Capture.CaptureCamera
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Terraria.Localization;

namespace Terraria.Graphics.Capture
{
    internal class CaptureCamera
    {
        private readonly object _captureLock = new object();
        private Queue<CaptureCamera.CaptureChunk> _renderQueue = new Queue<CaptureCamera.CaptureChunk>();
        public const int CHUNK_SIZE = 128;
        public const int FRAMEBUFFER_PIXEL_SIZE = 2048;
        public const int INNER_CHUNK_SIZE = 126;
        public const int MAX_IMAGE_SIZE = 4096;
        public const string CAPTURE_DIRECTORY = "Captures";
        private static bool CameraExists;
        private RenderTarget2D _frameBuffer;
        private RenderTarget2D _scaledFrameBuffer;
        private GraphicsDevice _graphics;
        private bool _isDisposed;
        private CaptureSettings _activeSettings;
        private SpriteBatch _spriteBatch;
        private byte[] _scaledFrameData;
        private byte[] _outputData;
        private Size _outputImageSize;
        private SamplerState _downscaleSampleState;
        private float _tilesProcessed;
        private float _totalTiles;

        public bool IsCapturing
        {
            get
            {
                Monitor.Enter(this._captureLock);
                var flag = this._activeSettings != null;
                Monitor.Exit(this._captureLock);
                return flag;
            }
        }

        public CaptureCamera(GraphicsDevice graphics)
        {
            CaptureCamera.CameraExists = true;
            this._graphics = graphics;
            this._spriteBatch = new SpriteBatch(graphics);
            try
            {
                this._frameBuffer = new RenderTarget2D(graphics, 2048, 2048, false,
                    graphics.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            }
            catch
            {
                Main.CaptureModeDisabled = true;
                return;
            }

            this._downscaleSampleState = SamplerState.AnisotropicClamp;
        }

        ~CaptureCamera()
        {
            this.Dispose();
        }

        public void Capture(CaptureSettings settings)
        {
            Main.GlobalTimerPaused = true;
            Monitor.Enter(this._captureLock);
            if (this._activeSettings != null)
                throw new InvalidOperationException("Capture called while another capture was already active.");
            this._activeSettings = settings;
            var area = settings.Area;
            var num1 = 1f;
            if (settings.UseScaling)
            {
                if (area.Width << 4 > 4096)
                    num1 = 4096f / (float) (area.Width << 4);
                if (area.Height << 4 > 4096)
                    num1 = Math.Min(num1, 4096f / (float) (area.Height << 4));
                num1 = Math.Min(1f, num1);
                this._outputImageSize =
                    new Size(
                        (int) MathHelper.Clamp((float) (int) ((double) num1 * (double) (area.Width << 4)), 1f, 4096f),
                        (int) MathHelper.Clamp((float) (int) ((double) num1 * (double) (area.Height << 4)), 1f, 4096f));
                this._outputData = new byte[4 * this._outputImageSize.Width * this._outputImageSize.Height];
                var num2 = (int) Math.Floor((double) num1 * 2048.0);
                this._scaledFrameData = new byte[4 * num2 * num2];
                this._scaledFrameBuffer = new RenderTarget2D(this._graphics, num2, num2, false,
                    this._graphics.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            }
            else
                this._outputData = new byte[16777216];

            this._tilesProcessed = 0.0f;
            this._totalTiles = (float) (area.Width * area.Height);
            var x1 = area.X;
            while (x1 < area.X + area.Width)
            {
                var y1 = area.Y;
                while (y1 < area.Y + area.Height)
                {
                    var width1 = Math.Min(128, area.X + area.Width - x1);
                    var height1 = Math.Min(128, area.Y + area.Height - y1);
                    var width2 = (int) Math.Floor((double) num1 * (double) (width1 << 4));
                    var height2 = (int) Math.Floor((double) num1 * (double) (height1 << 4));
                    var x2 = (int) Math.Floor((double) num1 * (double) (x1 - area.X << 4));
                    var y2 = (int) Math.Floor((double) num1 * (double) (y1 - area.Y << 4));
                    this._renderQueue.Enqueue(new CaptureCamera.CaptureChunk(
                        new Microsoft.Xna.Framework.Rectangle(x1, y1, width1, height1),
                        new Microsoft.Xna.Framework.Rectangle(x2, y2, width2, height2)));
                    y1 += 126;
                }

                x1 += 126;
            }

            Monitor.Exit(this._captureLock);
        }

        public void DrawTick()
        {
            Monitor.Enter(this._captureLock);
            if (this._activeSettings == null)
                return;
            if (this._renderQueue.Count > 0)
            {
                var captureChunk = this._renderQueue.Dequeue();
                this._graphics.SetRenderTarget(this._frameBuffer);
                this._graphics.Clear(Microsoft.Xna.Framework.Color.Transparent);
                Main.instance.DrawCapture(captureChunk.Area, this._activeSettings);
                if (this._activeSettings.UseScaling)
                {
                    this._graphics.SetRenderTarget(this._scaledFrameBuffer);
                    this._graphics.Clear(Microsoft.Xna.Framework.Color.Transparent);
                    this._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, this._downscaleSampleState,
                        DepthStencilState.Default, RasterizerState.CullNone);
                    this._spriteBatch.Draw((Texture2D) this._frameBuffer,
                        new Microsoft.Xna.Framework.Rectangle(0, 0, this._scaledFrameBuffer.Width,
                            this._scaledFrameBuffer.Height), Microsoft.Xna.Framework.Color.White);
                    this._spriteBatch.End();
                    this._graphics.SetRenderTarget((RenderTarget2D) null);
                    this._scaledFrameBuffer.GetData<byte>(this._scaledFrameData, 0,
                        this._scaledFrameBuffer.Width * this._scaledFrameBuffer.Height * 4);
                    this.DrawBytesToBuffer(this._scaledFrameData, this._outputData, this._scaledFrameBuffer.Width,
                        this._outputImageSize.Width, captureChunk.ScaledArea);
                }
                else
                {
                    this._graphics.SetRenderTarget((RenderTarget2D) null);
                    this.SaveImage((Texture2D) this._frameBuffer, captureChunk.ScaledArea.Width,
                        captureChunk.ScaledArea.Height, ImageFormat.Png, this._activeSettings.OutputName,
                        captureChunk.Area.X.ToString() + "-" + (object) captureChunk.Area.Y + ".png");
                }

                this._tilesProcessed += (float) (captureChunk.Area.Width * captureChunk.Area.Height);
            }

            if (this._renderQueue.Count == 0)
                this.FinishCapture();
            Monitor.Exit(this._captureLock);
        }

        //Fix By GScience(Attention)
        private unsafe void DrawBytesToBuffer(byte[] sourceBuffer, byte[] destinationBuffer, int sourceBufferWidth,
            int destinationBufferWidth, Microsoft.Xna.Framework.Rectangle area)
        {
            fixed (byte* numPtr1 = &destinationBuffer[0])
            fixed (byte* numPtr2Fixed = &sourceBuffer[0])
            {
                //Fix By GScience(Attention)
                var numPtr2 = numPtr2Fixed;
                var numPtr3 = numPtr1 + (destinationBufferWidth * area.Y + area.X << 2);
                for (var index1 = 0; index1 < area.Height; ++index1)
                {
                    for (var index2 = 0; index2 < area.Width; ++index2)
                    {
                        numPtr3[2] = *numPtr2;
                        numPtr3[1] = numPtr2[1];
                        *numPtr3 = numPtr2[2];
                        numPtr3[3] = numPtr2[3];
                        //Fix By GScience(Attention)
                        numPtr2++;
                        numPtr3++;
                    }

                    //Fix By GScience(Attention)
                    numPtr2 += sourceBufferWidth - area.Width << 2;
                    numPtr3 += destinationBufferWidth - area.Width << 2;
                }
            }
        }

        public float GetProgress()
        {
            return this._tilesProcessed / this._totalTiles;
        }

        private bool SaveImage(int width, int height, ImageFormat imageFormat, string filename)
        {
            try
            {
                Directory.CreateDirectory(Main.SavePath + (object) Path.DirectorySeparatorChar + "Captures" +
                                          (object) Path.DirectorySeparatorChar);
                using (var bitmap = new Bitmap(width, height))
                {
                    var rect = new System.Drawing.Rectangle(0, 0, width, height);
                    var bitmapdata =
                        bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
                    Marshal.Copy(this._outputData, 0, bitmapdata.Scan0, width * height * 4);
                    bitmap.UnlockBits(bitmapdata);
                    bitmap.Save(filename, imageFormat);
                    bitmap.Dispose();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine((object) ex);
                return false;
            }
        }

        private void SaveImage(Texture2D texture, int width, int height, ImageFormat imageFormat, string foldername,
            string filename)
        {
            var str = Main.SavePath + (object) Path.DirectorySeparatorChar + "Captures" +
                         (object) Path.DirectorySeparatorChar + foldername;
            var filename1 = Path.Combine(str, filename);
            Directory.CreateDirectory(str);
            using (var bitmap = new Bitmap(width, height))
            {
                var rect = new System.Drawing.Rectangle(0, 0, width, height);
                var elementCount = texture.Width * texture.Height * 4;
                texture.GetData<byte>(this._outputData, 0, elementCount);
                var index1 = 0;
                var index2 = 0;
                for (var index3 = 0; index3 < height; ++index3)
                {
                    for (var index4 = 0; index4 < width; ++index4)
                    {
                        var num = this._outputData[index1 + 2];
                        this._outputData[index2 + 2] = this._outputData[index1];
                        this._outputData[index2] = num;
                        this._outputData[index2 + 1] = this._outputData[index1 + 1];
                        this._outputData[index2 + 3] = this._outputData[index1 + 3];
                        index1 += 4;
                        index2 += 4;
                    }

                    index1 += texture.Width - width << 2;
                }

                var bitmapdata = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
                Marshal.Copy(this._outputData, 0, bitmapdata.Scan0, width * height * 4);
                bitmap.UnlockBits(bitmapdata);
                bitmap.Save(filename1, imageFormat);
            }
        }

        private void FinishCapture()
        {
            if (this._activeSettings.UseScaling)
            {
                var num = 0;
                do
                {
                    if (!this.SaveImage(this._outputImageSize.Width, this._outputImageSize.Height, ImageFormat.Png,
                        Main.SavePath + (object) Path.DirectorySeparatorChar + "Captures" +
                        (object) Path.DirectorySeparatorChar + this._activeSettings.OutputName + ".png"))
                    {
                        GC.Collect();
                        Thread.Sleep(5);
                        ++num;
                        Console.WriteLine(Language.GetTextValue("Error.CaptureError"));
                    }
                    else
                        goto label_5;
                } while (num <= 5);

                Console.WriteLine(Language.GetTextValue("Error.UnableToCapture"));
            }

            label_5:
            this._outputData = (byte[]) null;
            this._scaledFrameData = (byte[]) null;
            Main.GlobalTimerPaused = false;
            CaptureInterface.EndCamera();
            if (this._scaledFrameBuffer != null)
            {
                this._scaledFrameBuffer.Dispose();
                this._scaledFrameBuffer = (RenderTarget2D) null;
            }

            this._activeSettings = (CaptureSettings) null;
        }

        public void Dispose()
        {
            Monitor.Enter(this._captureLock);
            if (this._isDisposed)
                return;
            this._frameBuffer.Dispose();
            if (this._scaledFrameBuffer != null)
            {
                this._scaledFrameBuffer.Dispose();
                this._scaledFrameBuffer = (RenderTarget2D) null;
            }

            CaptureCamera.CameraExists = false;
            this._isDisposed = true;
            Monitor.Exit(this._captureLock);
        }

        private class CaptureChunk
        {
            public readonly Microsoft.Xna.Framework.Rectangle Area;
            public readonly Microsoft.Xna.Framework.Rectangle ScaledArea;

            public CaptureChunk(Microsoft.Xna.Framework.Rectangle area, Microsoft.Xna.Framework.Rectangle scaledArea)
            {
                this.Area = area;
                this.ScaledArea = scaledArea;
            }
        }
    }
}