using System.Data;
using System.Data.SqlClient;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace Dapper.DAL.Infrastructure
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new SqlConnection(_connectionString);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                return _connection;
            }
        }
        public IDbConnection ProfiledConnection
        {
            get
            {
                if (_connection == null)
                    _connection = new SqlConnection(_connectionString);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                return new ProfiledDbConnection(_connection, MiniProfiler.Current);
            }
            
        }
        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }
    }
}
