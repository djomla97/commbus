using SharedResources;
using SharedResources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace RepositoryLib
{
    public class Repository
    {
        private SqlConnection sqlConnection;
        private SqlCommand sqlCommand;
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;

        private string connectionString = @"Data Source = (local)\SQLEXPRESS;Initial Catalog = Projekat1Db; Integrated Security = True; Pooling=False";

        public Repository()
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }

        public IResponse DoQuery(string sqlQuery)
        {

            string query = sqlQuery.Replace("name", "rname");

            sqlCommand = new SqlCommand(query);
            sqlCommand.Connection = sqlConnection;

            dataAdapter = new SqlDataAdapter(sqlCommand);
            dataTable = new DataTable();

            dataAdapter.Fill(dataTable);

            IResponse response = new Response();
            response.Payload = new Payload(new Resource(), "");

            response.Payload.Resource.Name = dataTable.Rows[0]["rname"].ToString();

            return response;
        }
    }
}
