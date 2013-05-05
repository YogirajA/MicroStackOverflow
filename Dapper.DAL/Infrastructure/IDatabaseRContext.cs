using System;
using System.Data.Common;
using System.Data.SqlClient;
using StackExchange.Profiling.Data;

namespace Dapper.DAL.Infrastructure
{
    public interface IDatabaseContext : IDisposable
    {
        SqlConnection Connection { get; }
        DbConnection ProfiledConnection { get; }
        
    }
}