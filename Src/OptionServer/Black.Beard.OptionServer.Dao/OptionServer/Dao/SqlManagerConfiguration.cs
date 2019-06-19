using System;

namespace Bb.OptionServer
{

    public class SqlManagerConfiguration
    {

        public string ProviderInvariantName { get; set; }

        public string ConnectionString { get; set; }

        public Func<SqlManager, IQueryGenerator> QueryManager { get; set; }

    }

}