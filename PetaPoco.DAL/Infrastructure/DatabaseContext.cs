using System.Data;
using System.Data.SqlClient;
using PetaPoco.DAL.Models;

namespace PetaPoco.DAL.Infrastructure
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        private StackOverflowDB _stackOverflowDB;

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection Connection
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
        public StackOverflowDB StackOverflowDB
        {
            get
            {
                return _stackOverflowDB ?? (_stackOverflowDB = new StackOverflowDB(_connectionString));
            }
        }
        public void Dispose()
        {
            if (_stackOverflowDB != null)
                _stackOverflowDB.Dispose();

            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }
    }
}
