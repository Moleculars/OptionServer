using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bb.OptionServer
{
    public class DbUpdateContext
    {


        public DbUpdateContext()
        {
            this.Items = new List<DbParameter>();
        }

        public System.Data.Common.DbProviderFactory Factory { get; set; }
        
        public List<DbParameter> Items { get; }

        public DbParameter CreateParameter(string name, DbType type, object value)
        {

            var p = Factory.CreateParameter();
            p.ParameterName = name;
            p.DbType = type;
            p.Value = value;

            this.Items.Add(p);

            return p;
        }

    }
}