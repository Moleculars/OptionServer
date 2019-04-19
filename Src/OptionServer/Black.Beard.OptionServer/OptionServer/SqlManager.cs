using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bb.OptionServer
{

    public class SqlManager
    {

        public SqlManager(SqlManagerConfiguration configuration)
        {
            _factory = System.Data.Common.DbProviderFactories.GetFactory(configuration.ProviderInvariantName);
            _configuration = configuration;
        }


        public DbProviderFactory Factory => _factory;


        public IEnumerable<T> Read<T>(string sql, params DbParameter[] parameters)
            where T : IMapperDbDataReader, new()
        {

            List<T> _results = new List<T>();

            bool initialized = false;
            DbConnection cnx = null;

            if (_cnx != null)
                cnx = _cnx;
            else
            {
                cnx = GetConnection();
                cnx.Open();
                initialized = true;
            }

            try
            {

                using (var cmd = GetCommand(sql, cnx))
                {

                    if (parameters != null)
                        foreach (var parameter in parameters)
                            cmd.Parameters.Add(parameter);

                    var ctx = new DbDataReaderContext();

                    foreach (DbDataReader item in GetReader(cmd))
                    {
                        ctx.Reader = item;
                        T result = new T();
                        result.Map(ctx);
                        _results.Add(result);
                    }

                }

            }
            finally
            {
                if (initialized)
                    cnx.Dispose();
            }

            return _results;

        }

        public object ReadScalar(string sql, params DbParameter[] parameters)
        {

            bool initialized = false;
            DbConnection cnx = null;

            if (_cnx != null)
                cnx = _cnx;
            else
            {
                cnx = GetConnection();
                cnx.Open();
                initialized = true;
            }

            try
            {

                using (var cmd = GetCommand(sql, cnx))
                {

                    if (parameters != null)
                        foreach (var parameter in parameters)
                            cmd.Parameters.Add(parameter);

                    return cmd.ExecuteScalar();

                }

            }
            finally
            {
                if (initialized)
                    cnx.Dispose();
            }

        }


        public bool Update(string sql, params DbParameter[] Items)
        {
            return Update(sql, Items.ToList());
        }

        public bool Update(string sql, List<DbParameter> Items)
        {

            bool initialized = false;
            DbConnection cnx = null;

            if (_cnx != null)
                cnx = _cnx;
            else
            {
                cnx = GetConnection();
                cnx.Open();
                initialized = true;
            }

            try
            {

                using (var cmd = GetCommand(sql, cnx))
                {

                    foreach (var parameter in Items)
                        cmd.Parameters.Add(parameter);

                    var result = cmd.ExecuteNonQuery();

                    return result > 0;

                }

            }
            finally
            {
                if (initialized)
                    cnx.Dispose();
            }

        }

        public DbParameter CreateParameter(string name, DbType type, object value)
        {
            var p = _factory.CreateParameter();
            p.ParameterName = name;
            p.DbType = type;
            p.Value = value;
            return p;
        }

        public IEnumerable<System.Data.Common.DbDataReader> GetReader(System.Data.Common.DbCommand cmd)
        {
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return reader;
            }
        }

        public Transaction GetTransaction()
        {

            if (_currentTransaction != null)
                throw new Exception("connection allready initialized");

            _cnx = GetConnection();
            _cnx.Open();
            return new Transaction(this);
        }

        private System.Data.Common.DbCommand GetCommand(string sql, DbConnection cnx)
        {
            var cmd = _factory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = cnx;
            if (_currentTransaction != null)
                cmd.Transaction = _currentTransaction.Current;
            return cmd;
        }


        private DbConnection GetConnection()
        {
            var cnx = _factory.CreateConnection();
            cnx.ConnectionString = _configuration.ConnectionString;
            return cnx;
        }


        private readonly DbProviderFactory _factory;
        private readonly SqlManagerConfiguration _configuration;
        internal DbConnection _cnx;
        internal Transaction _currentTransaction;

    }

    public class Transaction : IDisposable
    {

        public Transaction(SqlManager manager)
        {
            _commited = false;
            _manager = manager;
            _manager._currentTransaction = this;
            _tranction = _manager._cnx.BeginTransaction();
        }

        public DbTransaction Current => _tranction;

        public void Commit()
        {
            _tranction.Commit();
            _commited = true;
        }

        public void Dispose()
        {

            if (!_commited)
                _tranction.Rollback();

            _tranction.Dispose();
            _manager._cnx.Dispose();

            _manager._cnx = null;
            _manager._currentTransaction = null;

        }

        private bool _commited;
        private readonly SqlManager _manager;
        private readonly DbTransaction _tranction;

    }

}