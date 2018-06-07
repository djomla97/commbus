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
        private SqlCommand sqlCommand2;
        private SqlDataAdapter dataAdapter2;
        private DataTable dataTable2;

        public Repository()
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }

        public List<Response> DoQuery(string sqlQuery)
        {
            string secondTable = "SELECT * FROM TypeTable WHERE id="; //za pristup drugoj tabeli

            string query = sqlQuery.Replace("name", "rname"); //
            var sqlSplited = query.Split(' ');

            sqlCommand = new SqlCommand(query);
            sqlCommand.Connection = sqlConnection;

            dataAdapter = new SqlDataAdapter(sqlCommand);
            dataTable = new DataTable();

            List<Response> responses = new List<Response>();


            if (sqlSplited[0] == "SELECT") //method
            {
                dataAdapter.Fill(dataTable);


                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Response response = new Response();
                    response.Payload = new Payload(new Resource(new ResourceType()), "");
                    response.Payload.Resource.ID = int.Parse(dataTable.Rows[i]["id"].ToString());
                    response.Payload.Resource.Name = dataTable.Rows[i]["rname"].ToString();
                    response.Payload.Resource.Description = dataTable.Rows[i]["description"].ToString();
                    response.Payload.Resource.Title = dataTable.Rows[i]["title"].ToString();

                    int secondId = response.Payload.Resource.ID; //the id we are looking in the second table
                    sqlCommand2 = new SqlCommand(String.Format(secondTable + secondId)); //

                    sqlCommand2.Connection = sqlConnection;

                    dataAdapter2 = new SqlDataAdapter(sqlCommand2);
                    dataTable2 = new DataTable();
                    dataAdapter2.Fill(dataTable2);

                    response.Payload.Resource.Type.ID = int.Parse(dataTable2.Rows[0]["id"].ToString());
                    response.Payload.Resource.Type.Title = dataTable2.Rows[0]["title"].ToString();
                    // response.Payload.Resource.Type.Title = "bla";

                    response.StatusCode = StatusCode.SUCCESS_CODE;
                    response.Status = "SUCCESS";

                    responses.Add(response);

                }



            }

            else
            {
                sqlCommand.ExecuteNonQuery();
                Response ress = new Response();
                ress.StatusCode = StatusCode.SUCCESS_CODE;
                ress.Status = "SUCCESS";

                responses.Add(ress);
            }

            
            
           

           

            
           

          //  response.Payload.Resource.Name = dataTable.Rows[0]["rname"].ToString();

            return responses;
        }
    }
}
