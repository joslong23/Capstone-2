using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Transactions;

namespace ProjectsTests
{
    [TestClass]
    public abstract class ProjectTestBase
    {
        protected string ConnectionString { get; } = "Server=.\\SQLEXPRESS;Database=EmployeeDB;Trusted_Connection=True;";

        private TransactionScope transaction;

        [TestInitialize] // Gets called before every test runs
        public void Setup()
        {
            // Begin the transaction
            transaction = new TransactionScope(); // BEGIN TRANSACTION

            // Get the SQL Script to run
            string sql = File.ReadAllText("test-script.sql");

            // Execute the script
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup] // Gets called after every test runs
        public void Cleanup()
        {
            // Roll back the transaction
            if (transaction != null)
            {
                transaction.Dispose(); // ROLLBACK TRANSACTION
            }
        }

        protected int GetRowCount(string table)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {table}", conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count;
            }
        }

        protected string GetRowName(string columanName, string table )
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT {columanName} FROM {table}", conn);
                string result = Convert.ToString(cmd.ExecuteScalar());

                return result;
            }
        }
    }
}
