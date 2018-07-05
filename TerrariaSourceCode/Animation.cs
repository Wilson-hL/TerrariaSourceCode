// Decompiled with JetBrains decompiler
// Type: Terraria.Animation
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.DataStructures;

namespace Terraria
{
    public class Animation
    {
        private static List<Animation> _animations;
        private static Dictionary<Point16, Animation> _temporaryAnimations;
        private static List<Point16> _awaitingRemoval;
        private static List<Animation> _awaitingAddition;
        private Point16 _coordinates;
        private int _frame;
        private int _frameCounter;
        private int _frameCounterMax;
        private int[] _frameData;
        private int _frameMax;
        private bool _temporary;
        private ushort _tileType;

        public static void Initialize()
        {
            _animations = new List<Animation>();
            _temporaryAnimations = new Dictionary<Point16, Animation>();
            _awaitingRemoval = new List<Point16>();
            _awaitingAddition = new List<Animation>();
        }

        private void SetDefaults(int type)
        {
            _tileType = 0;
            _frame = 0;
            _frameMax = 0;
            _frameCounter = 0;
            _frameCounterMax = 0;
            _temporary = false;
            switch (type)
            {
                case 0:
                    _frameMax = 5;
                    _frameCounterMax = 12;
                    _frameData = new int[_frameMax];
                    for (var index = 0; index < _frameMax; ++index)
                        _frameData[index] = index + 1;
                    break;
                case 1:
                    _frameMax = 5;
                    _frameCounterMax = 12;
                    _frameData = new int[_frameMax];
                    for (var index = 0; index < _frameMax; ++index)
                        _frameData[index] = 5 - index;
                    break;
                case 2:
                    _frameCounterMax = 6;
                    _frameData = new int[5] {1, 2, 2, 2, 1};
                    _frameMax = _frameData.Length;
                    break;
            }
        }

        public static void NewTemporaryAnimation(int type, ushort tileType, int x, int y)
        {
            var point16 = new Point16(x, y);
            if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
                return;
            var animation = new Animation();
            animation.SetDefaults(type);
            animation._tileType = tileType;
            animation._coordinates = point16;
            animation._temporary = true;
            _awaitingAddition.Add(animation);
            if (Main.netMode != 2)
                return;
            NetMessage.SendTemporaryAnimation(-1, type, tileType, x, y);
        }

        private static void RemoveTemporaryAnimation(short x, short y)
        {
            var key = new Point16(x, y);
            if (!_temporaryAnimations.ContainsKey(key))
                return;
            _awaitingRemoval.Add(key);
        }

        public static void UpdateAll()
        {
            for (var index = 0; index < _animations.Count; ++index)
                _animations[index].Update();
            if (_awaitingAddition.Count > 0)
            {
                for (var index = 0; index < _awaitingAddition.Count; ++index)
                {
                    var animation = _awaitingAddition[index];
                    _temporaryAnimations[animation._coordinates] = animation;
                }

                _awaitingAddition.Clear();
            }

            foreach (var temporaryAnimation in _temporaryAnimations)
                temporaryAnimation.Value.Update();
            if (_awaitingRemoval.Count <= 0)
                return;
            for (var index = 0; index < _awaitingRemoval.Count; ++index)
                _temporaryAnimations.Remove(_awaitingRemoval[index]);
            _awaitingRemoval.Clear();
        }

        public void Update()
        {
            if (_temporary)
            {
                var tile = Main.tile[_coordinates.X, _coordinates.Y];
                if (tile != null && tile.type != _tileType)
                {
                    RemoveTemporaryAnimation(_coordinates.X, _coordinates.Y);
                    return;
                }
            }

            ++_frameCounter;
            if (_frameCounter < _frameCounterMax)
                return;
            _frameCounter = 0;
            ++_frame;
            if (_frame < _frameMax)
                return;
            _frame = 0;
            if (!_temporary)
                return;
            RemoveTemporaryAnimation(_coordinates.X, _coordinates.Y);
        }

        public static bool GetTemporaryFrame(int x, int y, out int frameData)
        {
            var key = new Point16(x, y);
            Animation animation;
            if (!_temporaryAnimations.TryGetValue(key, out animation))
            {
                frameData = 0;
                return false;
            }

            frameData = animation._frameData[animation._frame];
            return true;
        }
    }
}