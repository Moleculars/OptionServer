using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Bb.OptionServer
{

    public class SqlManager
    {

        public SqlManager(SqlManagerConfiguration configuration)
        {
            _factory = DbProviderFactories.GetFactory(configuration.ProviderInvariantName);
            _configuration = configuration;
            _space = "\t";
        }

        public DbProviderFactory Factory => _factory;

        public bool Log { get; private set; }

        public IQueryGenerator QueryGenerator { get; internal set; }

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

        public IEnumerable<T> Read<T>(string sql, ObjectMapping mapping, params DbParameter[] parameters)
            where T : new()
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
                        mapping.Map<T>(ctx, result);
                        result.Reset(mapping);
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

                    LogCommand(cmd);

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

                    LogCommand(cmd);

                    var result = cmd.ExecuteNonQuery();

                    if (Log || System.Diagnostics.Debugger.IsAttached)
                    {
                        string plurial = result > 1 ? "s" : string.Empty;
                        Trace.WriteLine($"{result} impact{plurial} by query", "Sql");
                    }

                    return result > 0;

                }

            }
            catch (Exception e1)
            {

                Trace.WriteLine($"Sql error {e1.Message}", "Sql");

                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();

                throw e1;

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
            LogCommand(cmd);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
                yield return reader;

        }

        private void LogCommand(DbCommand cmd)
        {
            if (Log || System.Diagnostics.Debugger.IsAttached)
            {
                StringBuilder sb = new StringBuilder(200);
                if (_currentTransaction == null)
                    sb.AppendLine();

                foreach (DbParameter parameter in cmd.Parameters)
                    if (parameter.Value == null || parameter.Value == DBNull.Value)
                        sb.AppendLine(QueryGenerator.Declare(parameter.ParameterName, parameter.DbType, "DbNull"));
                    else
                        sb.AppendLine(QueryGenerator.Declare(parameter.ParameterName, parameter.DbType, QueryGenerator.GetValue(parameter.DbType, parameter.Value)));

                sb.Append(cmd.CommandText);
                Trace.WriteLine(sb.ToString(), "Sql");

            }
        }

        public Transaction GetTransaction(bool autocommit = false)
        {

            if (_currentTransaction != null)
                throw new Exception("connection allready initialized");

            _cnx = GetConnection();
            _cnx.Open();

            return new Transaction(this, autocommit);

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
        private readonly string _space;
        internal DbConnection _cnx;
        internal Transaction _currentTransaction;

    }

    public class Transaction : IDisposable
    {

        public Transaction(SqlManager manager, bool autocommit)
        {
            _commited = false;
            _manager = manager;
            _manager._currentTransaction = this;
            _tranction = _manager._cnx.BeginTransaction();
            _autocommit = autocommit;

            if (_manager.Log || System.Diagnostics.Debugger.IsAttached)
                Trace.WriteLine("Begin transaction", "Sql");

        }

        public DbTransaction Current => _tranction;

        public void Commit()
        {

            _tranction.Commit();
            _commited = true;

            if (_manager.Log || System.Diagnostics.Debugger.IsAttached)
                Trace.WriteLine("Commit transaction", "Sql");

        }

        public void Dispose()
        {

            if (!_commited)
            {
                if (_autocommit)
                    Commit();

                else
                {
                    if (_manager.Log || System.Diagnostics.Debugger.IsAttached)
                        Trace.WriteLine("Rollback transaction", "Sql");
                    _tranction.Rollback();
                }
            }

            _tranction.Dispose();
            _manager._cnx.Dispose();

            _manager._cnx = null;
            _manager._currentTransaction = null;

            if (_manager.Log || System.Diagnostics.Debugger.IsAttached)
                Trace.WriteLine("End transaction", "Sql");

        }

        private bool _commited;
        private readonly SqlManager _manager;
        private readonly DbTransaction _tranction;
        private readonly bool _autocommit;

    }

}