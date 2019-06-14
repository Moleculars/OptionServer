using System;

namespace Bb.OptionServer.Repositories
{

    public interface IPrimaryTable
    {

        FieldValue<Guid> Id { get; }

    }

}
