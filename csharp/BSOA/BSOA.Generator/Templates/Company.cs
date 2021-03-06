// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;

using BSOA.IO;
using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  BSOA GENERATED Root Entity for 'Company'
    /// </summary>
    public partial class Company : IRow
    {
        private CompanyTable _table;
        private int _index;

        internal CompanyDatabase Database => _table.Database;
        public ITreeSerializable DB => _table.Database;

        public Company() : this(new CompanyDatabase().Company)
        { }

        internal Company(CompanyTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Company(CompanyTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        // <ColumnList>
        //   <SimpleColumn>
        public long Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

        //   </SimpleColumn>
        //   <EnumColumn>
        public SecurityPolicy JoinPolicy
        {
            get => (SecurityPolicy)_table.JoinPolicy[_index];
            set => _table.JoinPolicy[_index] = (byte)value;
        }

        //   </EnumColumn>
        //  <RefColumn>
        public Employee Owner
        {
            get => _table.Database.Employee[_table.Owner[_index]];
        }
        //  </RefColumn>
        //  <RefListColumn>
        public IList<Employee> Members
        {
            get => _table.Database.Employee.List(_table.Members[_index]);
            set => _table.Database.Employee.List(_table.Members[_index]).SetTo(value);
        }

        //  </RefListColumn>
        public IList<Team> Teams
        {
            get => _table.Database.Team.List(_table.Teams[_index]);
            set => _table.Database.Team.List(_table.Teams[_index]).SetTo(value);
        }
        // </ColumnList>


        #region IEquatable<Company>
        public bool Equals(Company other)
        {
            if (other == null) { return false; }

            // <EqualsList>
            //  <Equals>
            if (this.Id != other.Id) { return false; }
            //  </Equals>
            if (this.JoinPolicy != other.JoinPolicy) { return false; }
            if (this.Owner != other.Owner) { return false; }
            if (this.Members != other.Members) { return false; }
            // </EqualsList>

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                // <GetHashCodeList>
                //  <GetHashCode>
                if (Id != default(long))
                {
                    result = (result * 31) + Id.GetHashCode();
                }

                //  </GetHashCode>
                if (JoinPolicy != default(SecurityPolicy))
                {
                    result = (result * 31) + JoinPolicy.GetHashCode();
                }

                if (Owner != default(Employee))
                {
                    result = (result * 31) + Owner.GetHashCode();
                }

                if (Members != default(IList<Employee>))
                {
                    result = (result * 31) + Members.GetHashCode();
                }
                // </GetHashCodeList>
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Company);
        }

        public static bool operator ==(Company left, Company right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Company left, Company right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return !object.ReferenceEquals(right, null);
            }

            return !left.Equals(right);
        }
        #endregion

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Next()
        {
            _index++;
        }
        #endregion

        #region Easy Serialization
        public void WriteBsoa(Stream stream)
        {
            using (BinaryTreeWriter writer = new BinaryTreeWriter(stream))
            {
                DB.Write(writer);
            }
        }

        public void WriteBsoa(string filePath)
        {
            WriteBsoa(File.Create(filePath));
        }

        public static Company ReadBsoa(Stream stream)
        {
            using (BinaryTreeReader reader = new BinaryTreeReader(stream))
            {
                Company result = new Company();
                result.DB.Read(reader);
                return result;
            }
        }

        public static Company ReadBsoa(string filePath)
        {
            return ReadBsoa(File.OpenRead(filePath));
        }

        public static TreeDiagnostics Diagnostics(string filePath)
        {
            return Diagnostics(File.OpenRead(filePath));
        }

        public static TreeDiagnostics Diagnostics(Stream stream)
        {
            using (BinaryTreeReader btr = new BinaryTreeReader(stream))
            using (TreeDiagnosticsReader reader = new TreeDiagnosticsReader(btr))
            {
                Company result = new Company();
                result.DB.Read(reader);
                return reader.Tree;
            }
        }
        #endregion
    }
}
