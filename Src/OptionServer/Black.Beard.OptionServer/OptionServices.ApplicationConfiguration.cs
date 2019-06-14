using Bb.OptionServer.Entities;
using Bb.OptionServer.Repositories.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb.OptionServer
{

    public partial class OptionServices
    {

        public void DeleteDocument(UserEntity user, string applicationPath, string name, int version = 0, string environmentName = null)
        {

            var path = user.ResolveApplication(applicationPath);
            var group = user.CheckGroup(path, AccessEntityEnum.Operator, objectKingEnum.Application);
            var application = path.Application.Infos;
            LoadDocuments(application, user);

            var environment = string.IsNullOrEmpty(environmentName) ? group.GetEnvironment(environmentName) : null;

            List<DocumentVersionTable> docs = new List<DocumentVersionTable>();
            foreach (var item in application.Documents)
            {

                if (version != 0 && item.Version != version)
                    continue;

                if (environment != null && environment.EnvironmentId != item.Environment.EnvironmentId)
                    continue;

                docs.Add(item.ToTable());

            }

            using (var trans = _dto.Sql.GetTransaction())
            {
                foreach (var doc in docs)
                {
                    doc.Deleted.Value = true;
                    Configurations.Update(doc);
                }
                trans.Commit();
            }

        }
        public DocumentEntity SetDocument(UserEntity user, string applicationPath, string environmentName, string typeName, string name, string content)
        {

            var path = user.ResolveApplication(applicationPath);
            var group = user.CheckGroup(path, AccessEntityEnum.Operator, objectKingEnum.Application);

            var application = path.Application.Infos;

            return SetDocument(user, application, environmentName, typeName, name, content);

        }

        public DocumentEntity SetDocument(UserEntity user, ApplicationEntity application, string environmentName, string typeName, string name, string content)
        {

            var group = user.CheckGroup(application.Group, AccessEntityEnum.Operator, objectKingEnum.Application);
            var environment = group.GetEnvironment(environmentName);

            TypeEntity type = null;
            string extension = null;

            if (!string.IsNullOrEmpty(typeName))
            {
                type = group.GetType(typeName);
                extension = type?.Extension;
            }

            if (name.Contains("."))
            {
                extension = System.IO.Path.GetFileNameWithoutExtension(name).ToLower();
                if (type != null)
                {
                    if (type.Extension != extension)
                        throw new Exceptions.ConflictException($"extension {extension} don't match with type extension {type.Extension}");
                }
                else
                    type = group.TypeByExtension(extension);

            }
            else if (!string.IsNullOrEmpty(extension))
                name += extension;

            if (type == null)
                throw new Exceptions.InvalidValueException("type can't be resolved");

            LoadDocuments(application, user);

            int vers = 1;
            string hash = Sha.Sha256_hash(content);
            var doc = application.DocumentByName(name).LastOrDefault();
            if (doc != null)
            {
                if (doc.Sha256 == hash)
                    return doc;
                vers = doc.Version + 1;
            }

            DocumentVersionTable document = new DocumentVersionTable(Guid.NewGuid(), name, application.Id, environment.EnvironmentId, type.TypeId, type.CurrentVersion.Id, content, vers, hash, Guid.NewGuid());

            var a = Configurations.Insert(document);
            if (a)
            {
                doc = new DocumentEntity()
                {
                    ConfigurationId = document.Id,
                    Application = application,
                    ConfigurationName = document.Name,
                    Payload = document.Payload,
                    Version = document.Version,
                    Environment = group.GetEnvironment(document.EnvironmentId),
                    Type = group.GetType(document.TypeId),
                    Sha256 = document.Sha256,
                    Deleted = document.Deleted,
                    SecurityCoherence = document.SecurityCoherence,
                };

                doc.TypeVersion = doc.Type.Versions[document.TypeVersionId];

                application.Documents.Add(doc);
                application.ResetDocument();
            }

            return doc;

        }

        public void LoadDocuments(ApplicationEntity application, UserEntity user, EnvironmentEntity environment, bool withData = false)
        {

            application.Documents.Clear();
            var items = Configurations.ReadDocumentForApplication(application.Id, environment.EnvironmentId, withData).ToList();
            var group = application.Group;

            List<DocumentEntity> list = Convert(application, items, group);

            application.Documents.AddRange(list.OrderBy(c => c.Version));

        }

        public void LoadDocuments(ApplicationEntity application, UserEntity user, bool withData = false)
        {

            application.Documents.Clear();
            var items = Configurations.ReadDocumentForApplication(application.Id, withData).ToList();
            var group = application.Group;

            List<DocumentEntity> list = Convert(application, items, group);

            application.Documents.AddRange(list.OrderBy(c => c.Version));

        }

        private static List<DocumentEntity> Convert(ApplicationEntity application, List<DocumentVersionTable> items, GroupEntity group)
        {

            var list = new List<DocumentEntity>();

            foreach (DocumentVersionTable item in items)
            {
                var doc = new DocumentEntity()
                {
                    ConfigurationId = item.Id,
                    Application = application,
                    ConfigurationName = item.Name,
                    Payload = item.Payload,
                    Version = item.Version,
                    Environment = group.GetEnvironment(item.EnvironmentId),
                    Type = group.GetType(item.TypeId),
                    Sha256 = item.Sha256,
                    Deleted = item.Deleted,
                    SecurityCoherence = item.SecurityCoherence
                };

                doc.TypeVersion = doc.Type.Versions[item.TypeVersionId];

                list.Add(doc);

            }

            return list;

        }
    }
}
