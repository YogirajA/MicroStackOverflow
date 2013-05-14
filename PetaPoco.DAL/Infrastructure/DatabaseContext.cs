using PetaPoco.DAL.Models;

namespace PetaPoco.DAL.Infrastructure
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly string _connectionStringName;
       
        private StackOverflowDB _stackOverflowDb;

        public DatabaseContext(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

  
        public StackOverflowDB StackOverflowDB
        {
            get
            {   
                return _stackOverflowDb ?? (_stackOverflowDb = new StackOverflowDB(_connectionStringName));
            }
        }
        public void Dispose()
        {
            if (_stackOverflowDb != null)
                _stackOverflowDb.Dispose();

            
        }
    }
}
