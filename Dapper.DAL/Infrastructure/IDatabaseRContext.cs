using System;
using System.Data.SqlClient;

namespace Dapper.DAL.Infrastructure
{
    public interface IDatabaseContext : IDisposable
    {
        SqlConnection Connection { get; }
    }
}