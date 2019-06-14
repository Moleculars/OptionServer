using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bb.OptionServer.Entities
{

    [DebuggerDisplay("{Name}")]
    public class ApplicationEntity
    {

        public ApplicationEntity()
        {
            this.Documents = new List<DocumentEntity>();
        }

        public string Name { get; set; }


        public Guid Id { get; set; }

        public Guid SecurityCoherence { get; set; }

        public GroupEntity Group { get; internal set; }

        public List<DocumentEntity> Documents { get; internal set; }

        public IEnumerable< DocumentEntity> DocumentByName(string name)
        {
            if (_documentByName == null)
                _documentByName = this.Documents.ToLookup(c => c.ConfigurationName);

            return _documentByName[name];
        }

        private ILookup<string, DocumentEntity> _documentByName;

        internal void ResetDocument()
        {
            _documentByName = null;
        }

    }

}
