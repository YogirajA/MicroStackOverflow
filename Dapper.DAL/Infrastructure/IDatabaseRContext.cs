using System;
using System.Data;

namespace Dapper.DAL.Infrastructure
{
    public interface IDatabaseContext
    {
        IDbConnection Connection { get; }
        IDbConnection ProfiledConnection { get; }
        
    }
}