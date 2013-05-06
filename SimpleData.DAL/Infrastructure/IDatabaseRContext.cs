using System;
using System.Data.Common;
using System.Data.SqlClient;
using Simple.Data;

namespace SimpleData.DAL.Infrastructure
{
    public interface IDatabaseContext
    {
        dynamic StackOverflowDb { get; }
    }
}