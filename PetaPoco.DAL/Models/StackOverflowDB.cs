using System.Data;
using System.Data.Common;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace PetaPoco.DAL.Models
{
    public partial class StackOverflowDB
    {
        public override IDbConnection OnConnectionOpened(
         IDbConnection connection)
        {            
            return new ProfiledDbConnection(connection as DbConnection, MiniProfiler.Current);
        }
    }
}
