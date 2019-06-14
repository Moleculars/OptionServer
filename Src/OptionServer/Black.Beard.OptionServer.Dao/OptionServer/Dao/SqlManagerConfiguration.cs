using System.Data.Common;

namespace Bb.OptionServer
{
    public class SqlManagerConfiguration
    {
        public string ProviderInvariantName { get; set; }
        public string ConnectionString { get; set; }
    }
}