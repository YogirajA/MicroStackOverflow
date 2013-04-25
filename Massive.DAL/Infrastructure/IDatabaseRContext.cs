using System;
using System.Data.SqlClient;

namespace Massive.DAL.Infrastructure
{
    public interface IDatabaseContext : IDisposable
    {
        SqlConnection Connection { get; }
    }
}