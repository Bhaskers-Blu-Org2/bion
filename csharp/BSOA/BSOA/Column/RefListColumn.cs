﻿using BSOA.IO;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Column
{
    /// <summary>
    ///  RefListColumn provides a reference from an item in one table to a set
    ///  of items in another table. It stores the integer indices of the references.
    /// </summary>
    public class RefListColumn : IColumn<NumberList<int>>
    {
        private NumberListColumn<int> _inner;
        public string ReferencedTableName { get; }

        public int Count => _inner.Count;
        public bool Empty => Count == 0;

        public RefListColumn(string referencedTableName)
        {
            _inner = new NumberListColumn<int>();
            ReferencedTableName = referencedTableName;
        }

        public NumberList<int> this[int index] 
        {
            get => new NumberList<int>(_inner, index);
            set => _inner[index] = value.Slice;
        }

        public IEnumerator<NumberList<int>> GetEnumerator()
        {
            return new ListEnumerator<NumberList<int>>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<NumberList<int>>(this);
        }

        public void Trim()
        {
            _inner.Trim();
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public void RemoveFromEnd(int count)
        {
            _inner.RemoveFromEnd(count);
        }

        public void Swap(int index1, int index2)
        {
            // Swapping slices directly is efficient, making each row 
            // refer to the existing ArraySlice from the other.
            _inner.Swap(index1, index2);
        }

        public void Write(ITreeWriter writer)
        {
            _inner.Write(writer);
        }

        public void Read(ITreeReader reader)
        {
            _inner.Read(reader);
        }
    }
}