// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'LocationRelationship'
    /// </summary>
    internal partial class LocationRelationshipTable : Table<LocationRelationship>
    {
        internal SarifLogDatabase Database;

        internal IColumn<int> Target;
        internal IColumn<IList<string>> Kinds;
        internal RefColumn Description;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal LocationRelationshipTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Target = AddColumn(nameof(Target), ColumnFactory.Build<int>(default));
            Kinds = AddColumn(nameof(Kinds), ColumnFactory.Build<IList<string>>(default));
            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override LocationRelationship Get(int index)
        {
            return (index == -1 ? null : new LocationRelationship(this, index));
        }
    }
}
