﻿using System.Data;
using System.Data.SqlClient;
using PetaPoco.DAL.Models;

namespace PetaPoco.DAL.Infrastructure
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly string _connectionStringName;
       
        private StackOverflowDB _stackOverflowDB;

        public DatabaseContext(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

  
        public StackOverflowDB StackOverflowDB
        {
            get
            {
                return _stackOverflowDB ?? (_stackOverflowDB = new StackOverflowDB(_connectionStringName));
            }
        }
        public void Dispose()
        {
            if (_stackOverflowDB != null)
                _stackOverflowDB.Dispose();

            
        }
    }
}