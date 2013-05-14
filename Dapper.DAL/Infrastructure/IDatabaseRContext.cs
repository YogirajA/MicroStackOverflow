using System;
using System.Data;

namespace Dapper.DAL.Infrastructure
{
    public interface IDatabaseContext : IDisposable
    {
        IDbConnection Connection { get; }
        IDbConnection ProfiledConnection { get; }
        
    }
}