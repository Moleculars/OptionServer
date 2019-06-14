using System.Data.Common;

namespace Bb.OptionServer
{

    public interface IMapperDbDataReader
    {

        void Map(DbDataReaderContext item);

    }

}