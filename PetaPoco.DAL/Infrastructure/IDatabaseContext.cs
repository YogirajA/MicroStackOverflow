using System;
using System.Data.SqlClient;
using PetaPoco.DAL.Models;

namespace PetaPoco.DAL.Infrastructure
{
    public interface IDatabaseContext : IDisposable
    {
        SqlConnection Connection { get; }
        StackOverflowDB StackOverflowDB { get; }
    }
}
