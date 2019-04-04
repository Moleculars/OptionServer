using Bb.Exceptions;
using Bb.OptionServer;
using System;

namespace Bb
{

    public partial class OptionServices
    {

        public OptionServices(SqlManager manager)
        {
            _manager = manager;
        }

        private readonly SqlManager _manager;

    }
}
