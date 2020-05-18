﻿using BSOA.Extensions;
using BSOA.IO;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA
{
    /// <summary>
    ///  BitVector provides set operations, tracking whether each item is included with a bit in an int[].
    ///  BitVector is an extremely compact way to represent a set from [0, Count).
    /// </summary>
    public class BitVector : IEnumerable<int>, ITreeSerializable
    {
        private const uint FirstBit = 0x1U << 31;
        private bool _defaultValue;
        private uint[] _array;

        public BitVector(bool defaultValue, int capacity)
        {
            _defaultValue = defaultValue;
            _array = null;
            Capacity = capacity;
            Count = (_defaultValue ? capacity : 0);
        }

        public int Count { get; private set; }
        public int Capacity { get; private set; }
        public uint[] Array => _array;

        public bool this[int index]
        {
            get
            {
                if (index < 0) { throw new IndexOutOfRangeException(nameof(index)); }
                if (_array == null || _array.Length <= (index >> 5)) { return _defaultValue; }
                return (_array[index >> 5] & (FirstBit >> (index & 31))) != 0UL;
            }
            set
            {
                if (index >= Capacity)
                {
                    if (_defaultValue) { Count += (index + 1 - Capacity); }
                    Capacity = index + 1;
                }

                if (_array == null || _array.Length <= (index >> 5))
                {
                    if (value == _defaultValue) { return; }
                    ArrayExtensions.ResizeTo(ref _array, ((Capacity + 31) >> 5), (_defaultValue ? ~0U : 0U));
                }

                if (value)
                {
                    Count += (int)(~_array[index >> 5] >> (31 - (index & 31)) & 1);
                    _array[index >> 5] |= (FirstBit >> (index & 31));
                }
                else
                {
                    Count -= (int)(_array[index >> 5] >> (31 - (index & 31)) & 1);
                    _array[index >> 5] &= ~(FirstBit >> (index & 31));
                }
            }
        }

        public void RemoveFromEnd(int count)
        {
            int newLastIndex = ((Capacity - 1) - count);
            int firstRemovedBlock = (newLastIndex >> 5) + 1;
            uint defaultInt = (_defaultValue ? ~0U : 0U);

            // Remove whole 32-bit chunks now out of range
            if (_array != null)
            {
                for (int i = firstRemovedBlock; i < _array.Length; ++i)
                {
                    _array[i] = defaultInt;
                }
            }

            // Reset values over new count in last block
            int firstInvalidIndex = (firstRemovedBlock << 5);
            for (int i = newLastIndex + 1; i < firstInvalidIndex; ++i)
            {
                this[i] = _defaultValue;
            }

            // Track reduced size
            Capacity -= count;

            // Recalculate Count
            int newCount = 0;
            for (int i = 0; i < Capacity; ++i)
            {
                if (this[i]) { newCount++; }
            }
            Count = newCount;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new BitVectorEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new BitVectorEnumerator(this);
        }

        public bool Add(int item)
        {
            bool wasSet = this[item];
            this[item] = true;
            return (!wasSet);
        }

        public bool Contains(int item)
        {
            return this[item];
        }

        public bool Remove(int item)
        {
            bool wasSet = this[item];
            this[item] = false;
            return wasSet;
        }

        public void UnionWith(IEnumerable<int> other)
        {
            foreach (int item in other)
            {
                this[item] = true;
            }
        }

        public void ExceptWith(IEnumerable<int> other)
        {
            foreach (int item in other)
            {
                this[item] = false;
            }
        }

        public void Clear()
        {
            Count = 0;
            Capacity = 0;
            _array = null;
        }

        public void Trim()
        { }

        private static Dictionary<string, Setter<BitVector>> setters = new Dictionary<string, Setter<BitVector>>()
        {
            [Names.Capacity] = (r, me) => me.Capacity = r.ReadAsInt32(),
            [Names.Array] = (r, me) => me._array = r.ReadBlockArray<uint>()

        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(Names.Capacity, Capacity);
            writer.WritePropertyName(Names.Array);
            writer.WriteBlockArray(Array);
            writer.WriteEndObject();
        }
    }

    public struct BitVectorEnumerator : IEnumerator<int>
    {
        private BitVector _vector;
        public int Current { get; private set; }
        object IEnumerator.Current => Current;

        public BitVectorEnumerator(BitVector vector)
        {
            _vector = vector;
            Current = -1;
        }

        public void Reset()
        {
            Current = -1;
        }

        public bool MoveNext()
        {
            Current++;

            // Look for the next set bit
            for (; Current < _vector.Capacity; ++Current)
            {
                if (_vector[Current]) { return true; }
            }

            return false;
        }

        public void Dispose()
        { }
    }
}