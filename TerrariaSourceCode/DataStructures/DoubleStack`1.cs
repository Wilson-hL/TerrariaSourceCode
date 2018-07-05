// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.DoubleStack`1
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System;

namespace Terraria.DataStructures
{
    public class DoubleStack<T1>
    {
        private readonly int _segmentShiftPosition;
        private readonly int _segmentSize;
        private int _end;
        private int _last;
        private int _segmentCount;
        private T1[][] _segmentList;
        private int _start;

        public DoubleStack(int segmentSize = 1024, int initialSize = 0)
        {
            if (segmentSize < 16)
                segmentSize = 16;
            _start = segmentSize / 2;
            _end = _start;
            Count = 0;
            _segmentShiftPosition = segmentSize + _start;
            initialSize += _start;
            var length = initialSize / segmentSize + 1;
            _segmentList = new T1[length][];
            for (var index = 0; index < length; ++index)
                _segmentList[index] = new T1[segmentSize];
            _segmentSize = segmentSize;
            _segmentCount = length;
            _last = _segmentSize * _segmentCount - 1;
        }

        public int Count { get; private set; }

        public void PushFront(T1 front)
        {
            if (_start == 0)
            {
                var objArray = new T1[_segmentCount + 1][];
                for (var index = 0; index < _segmentCount; ++index)
                    objArray[index + 1] = _segmentList[index];
                objArray[0] = new T1[_segmentSize];
                _segmentList = objArray;
                ++_segmentCount;
                _start += _segmentSize;
                _end += _segmentSize;
                _last += _segmentSize;
            }

            --_start;
            _segmentList[_start / _segmentSize][_start % _segmentSize] = front;
            ++Count;
        }

        public T1 PopFront()
        {
            if (Count == 0)
                throw new InvalidOperationException("The DoubleStack is empty.");
            var segment1 = _segmentList[_start / _segmentSize];
            var index1 = _start % _segmentSize;
            var obj = segment1[index1];
            segment1[index1] = default(T1);
            ++_start;
            --Count;
            if (_start >= _segmentShiftPosition)
            {
                var segment2 = _segmentList[0];
                for (var index2 = 0; index2 < _segmentCount - 1; ++index2)
                    _segmentList[index2] = _segmentList[index2 + 1];
                _segmentList[_segmentCount - 1] = segment2;
                _start -= _segmentSize;
                _end -= _segmentSize;
            }

            if (Count == 0)
            {
                _start = _segmentSize / 2;
                _end = _start;
            }

            return obj;
        }

        public T1 PeekFront()
        {
            if (Count == 0)
                throw new InvalidOperationException("The DoubleStack is empty.");
            return _segmentList[_start / _segmentSize][_start % _segmentSize];
        }

        public void PushBack(T1 back)
        {
            if (_end == _last)
            {
                var objArray = new T1[_segmentCount + 1][];
                for (var index = 0; index < _segmentCount; ++index)
                    objArray[index] = _segmentList[index];
                objArray[_segmentCount] = new T1[_segmentSize];
                ++_segmentCount;
                _segmentList = objArray;
                _last += _segmentSize;
            }

            _segmentList[_end / _segmentSize][_end % _segmentSize] = back;
            ++_end;
            ++Count;
        }

        public T1 PopBack()
        {
            if (Count == 0)
                throw new InvalidOperationException("The DoubleStack is empty.");
            var segment = _segmentList[_end / _segmentSize];
            var index = _end % _segmentSize;
            var obj = segment[index];
            segment[index] = default(T1);
            --_end;
            --Count;
            if (Count == 0)
            {
                _start = _segmentSize / 2;
                _end = _start;
            }

            return obj;
        }

        public T1 PeekBack()
        {
            if (Count == 0)
                throw new InvalidOperationException("The DoubleStack is empty.");
            return _segmentList[_end / _segmentSize][_end % _segmentSize];
        }

        public void Clear(bool quickClear = false)
        {
            if (!quickClear)
                for (var index = 0; index < _segmentCount; ++index)
                    Array.Clear(_segmentList[index], 0, _segmentSize);

            _start = _segmentSize / 2;
            _end = _start;
            Count = 0;
        }
    }
}