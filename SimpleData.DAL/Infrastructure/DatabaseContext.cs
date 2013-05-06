using Simple.Data;

namespace SimpleData.DAL.Infrastructure
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly string _connectionString;
      

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public dynamic StackOverflowDb
        {
            get
            {
                return Database.OpenConnection(_connectionString);
                //Does not hold open Connection so need for dispose
            }
        }
       
    }
}
