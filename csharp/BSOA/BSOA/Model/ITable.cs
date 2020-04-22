﻿using BSOA.IO;
using System.Collections.Generic;

namespace BSOA.Model
{
    public interface ITable : ITreeSerializable
    {
        void Clear();
        void Trim();
    }

    public interface ITable<T> : ITable, IReadOnlyList<T>
    {
        T Add();
    }
}
