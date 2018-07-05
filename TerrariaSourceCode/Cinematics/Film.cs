// Decompiled with JetBrains decompiler
// Type: Terraria.Cinematics.Film
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Cinematics
{
    public class Film
    {
        private readonly List<Sequence> _sequences = new List<Sequence>();

        public int Frame { get; private set; }

        public int FrameCount { get; private set; }

        public int AppendPoint { get; private set; }

        public bool IsActive { get; private set; }

        public void AddSequence(int start, int duration, FrameEvent frameEvent)
        {
            _sequences.Add(new Sequence(frameEvent, start, duration));
            AppendPoint = Math.Max(AppendPoint, start + duration);
            FrameCount = Math.Max(FrameCount, start + duration);
        }

        public void AppendSequence(int duration, FrameEvent frameEvent)
        {
            AddSequence(AppendPoint, duration, frameEvent);
        }

        public void AddSequences(int start, int duration, params FrameEvent[] frameEvents)
        {
            foreach (var frameEvent in frameEvents)
                AddSequence(start, duration, frameEvent);
        }

        public void AppendSequences(int duration, params FrameEvent[] frameEvents)
        {
            var sequenceAppendTime = AppendPoint;
            foreach (var frameEvent in frameEvents)
            {
                _sequences.Add(new Sequence(frameEvent, sequenceAppendTime, duration));
                AppendPoint = Math.Max(AppendPoint, sequenceAppendTime + duration);
                FrameCount = Math.Max(FrameCount, sequenceAppendTime + duration);
            }
        }

        public void AppendEmptySequence(int duration)
        {
            AddSequence(AppendPoint, duration, EmptyFrameEvent);
        }

        public void AppendKeyFrame(FrameEvent frameEvent)
        {
            AddKeyFrame(AppendPoint, frameEvent);
        }

        public void AppendKeyFrames(params FrameEvent[] frameEvents)
        {
            var sequenceAppendTime = AppendPoint;
            foreach (var frameEvent in frameEvents)
                _sequences.Add(new Sequence(frameEvent, sequenceAppendTime, 1));
            FrameCount = Math.Max(FrameCount, sequenceAppendTime + 1);
        }

        public void AddKeyFrame(int frame, FrameEvent frameEvent)
        {
            _sequences.Add(new Sequence(frameEvent, frame, 1));
            FrameCount = Math.Max(FrameCount, frame + 1);
        }

        public void AddKeyFrames(int frame, params FrameEvent[] frameEvents)
        {
            foreach (var frameEvent in frameEvents)
                AddKeyFrame(frame, frameEvent);
        }

        public bool OnUpdate(GameTime gameTime)
        {
            if (_sequences.Count == 0)
                return false;
            foreach (var sequence in _sequences)
            {
                var num = Frame - sequence.Start;
                if (num >= 0 && num < sequence.Duration)
                    sequence.Event(new FrameEventData(Frame, sequence.Start, sequence.Duration));
            }

            return ++Frame != FrameCount;
        }

        public virtual void OnBegin()
        {
            IsActive = true;
        }

        public virtual void OnEnd()
        {
            IsActive = false;
        }

        private static void EmptyFrameEvent(FrameEventData evt)
        {
        }

        private class Sequence
        {
            public Sequence(FrameEvent frameEvent, int start, int duration)
            {
                Event = frameEvent;
                Start = start;
                Duration = duration;
            }

            public FrameEvent Event { get; }

            public int Duration { get; }

            public int Start { get; }
        }
    }
}