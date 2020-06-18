// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ReportingDescriptor'
    /// </summary>
    internal partial class ReportingDescriptorTable : Table<ReportingDescriptor>
    {
        internal SarifLogDatabase Database;

        internal IColumn<string> Id;
        internal IColumn<IList<string>> DeprecatedIds;
        internal IColumn<string> Guid;
        internal IColumn<IList<string>> DeprecatedGuids;
        internal IColumn<string> Name;
        internal IColumn<IList<string>> DeprecatedNames;
        internal RefColumn ShortDescription;
        internal RefColumn FullDescription;
        internal IColumn<IDictionary<string, MultiformatMessageString>> MessageStrings;
        internal RefColumn DefaultConfiguration;
        internal IColumn<Uri> HelpUri;
        internal RefColumn Help;
        internal RefListColumn Relationships;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ReportingDescriptorTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), ColumnFactory.Build<string>(default));
            DeprecatedIds = AddColumn(nameof(DeprecatedIds), ColumnFactory.Build<IList<string>>(default));
            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<string>(default));
            DeprecatedGuids = AddColumn(nameof(DeprecatedGuids), ColumnFactory.Build<IList<string>>(default));
            Name = AddColumn(nameof(Name), ColumnFactory.Build<string>(default));
            DeprecatedNames = AddColumn(nameof(DeprecatedNames), ColumnFactory.Build<IList<string>>(default));
            ShortDescription = AddColumn(nameof(ShortDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            FullDescription = AddColumn(nameof(FullDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            MessageStrings = AddColumn(nameof(MessageStrings), new DictionaryColumn<string, MultiformatMessageString>(new StringColumn(), new MultiformatMessageStringColumn(this.Database)));
            DefaultConfiguration = AddColumn(nameof(DefaultConfiguration), new RefColumn(nameof(SarifLogDatabase.ReportingConfiguration)));
            HelpUri = AddColumn(nameof(HelpUri), ColumnFactory.Build<Uri>(default));
            Help = AddColumn(nameof(Help), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            Relationships = AddColumn(nameof(Relationships), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptorRelationship)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override ReportingDescriptor Get(int index)
        {
            return (index == -1 ? null : new ReportingDescriptor(this, index));
        }
    }
}
