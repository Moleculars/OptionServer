using Bb.OptionServer.Repositories.Tables;
using System;

namespace Bb.OptionServer.Entities
{
    public class DocumentEntity
    {

        public DocumentEntity()
        {

        }

        public Guid ConfigurationId { get; set; }

        public string ConfigurationName { get; set; }

        public ApplicationEntity Application { get; set; }

        public string Payload { get; set; }

        public string Sha256 { get; set; }

        public int Version { get; set; }

        public EnvironmentEntity Environment { get; internal set; }
        public TypeEntity Type { get; internal set; }
        public TypeVersionEntity TypeVersion { get; internal set; }
        public bool Deleted { get;  set; }
        public Guid SecurityCoherence { get;  set; }

        internal DocumentVersionTable ToTable()
        {
            return new DocumentVersionTable(
                this.ConfigurationId,
                this.ConfigurationName,
                this.Application.Id, 
                this.Environment.EnvironmentId, 
                this.Type.TypeId, 
                this.TypeVersion.Id,
                this.Payload,
                this.Version,
                this.Sha256,
                this.SecurityCoherence
                );
        }
    }

}
