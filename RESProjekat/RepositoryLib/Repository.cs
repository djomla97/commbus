using SharedResources;
using SharedResources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using Microsoft.SqlServer;

namespace RepositoryLib
{
    public class Repository
    {
        private SqlConnection sqlConnection;
        private SqlCommand sqlCommand;          //
        private SqlDataAdapter dataAdapter;     // Za pristup prvoj tabeli 'resource'
        private DataTable dataTable;            //

        private string connectionString;
        //private string connectionString = @"Data Source=DESKTOP-GBOAIN9\NEWSQLEXPRESS;Initial Catalog=projekatdb;Integrated Security=True;Pooling=False";
        //private string connectionString = @"Data Source = (local)\SQLEXPRESS;Initial Catalog = Projekat1Db; Integrated Security = True; Pooling=False";
        private SqlCommand sqlCommand2;         //
        private SqlDataAdapter dataAdapter2;    // Za pristup drugoj tabeli 'TypeTable'
        private DataTable dataTable2;           //

        private SqlCommand sqlCommand3;         //
        private SqlDataAdapter dataAdapter3;    // Za vracanje novog
        private DataTable dataTable3;           //

        public Repository()
        {
            ReadConfig();
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            InitDB();
        }

        private void ReadConfig()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(@"..\..\..\config.xml");
            XmlNode sqlConnectionString = xmlDocument.DocumentElement.SelectSingleNode("/config/sqlconnectionstring");
            connectionString = sqlConnectionString.InnerText;
        }

        private void InitDB() {
            // drop tables
            string dropScript = File.ReadAllText(@"..\..\..\drop_tables.sql");
            sqlCommand = new SqlCommand(dropScript);
            sqlCommand.Connection = sqlConnection;
            sqlCommand.ExecuteNonQuery();

            // create tables - bitan raspored
            string createScript = File.ReadAllText(@"..\..\..\create_connection.sql");
            sqlCommand = new SqlCommand(createScript);
            sqlCommand.Connection = sqlConnection;
            sqlCommand.ExecuteNonQuery();

            createScript = File.ReadAllText(@"..\..\..\create_typetable.sql");
            sqlCommand = new SqlCommand(createScript);
            sqlCommand.Connection = sqlConnection;
            sqlCommand.ExecuteNonQuery();

            createScript = File.ReadAllText(@"..\..\..\create_resource.sql");
            sqlCommand = new SqlCommand(createScript);
            sqlCommand.Connection = sqlConnection;
            sqlCommand.ExecuteNonQuery();

            // insert
            string insertScript = File.ReadAllText(@"..\..\..\insert_data.sql");
            sqlCommand = new SqlCommand(insertScript);
            sqlCommand.Connection = sqlConnection;
            sqlCommand.ExecuteNonQuery();

        }

        public Response DoQuery(string sqlQuery)
        {
            string secondTable = "SELECT * FROM TypeTable WHERE id="; //za pristup drugoj tabeli

            string query = sqlQuery.Replace("name", "rname");
            //query = query.Replace('\'', '\"');
            var sqlSplited = query.Split(' ');

            sqlCommand = new SqlCommand(query);
            sqlCommand.Connection = sqlConnection;

            dataAdapter = new SqlDataAdapter(sqlCommand);
            dataTable = new DataTable();

            Response response = new Response();

            List<IResource> resources = new List<IResource>();
            response.Payload = new Payload(resources, "");

            if (sqlSplited[0] == "SELECT") //method
            {
                try
                {
                    dataAdapter.Fill(dataTable);
                }
                catch
                {
                    response.Status = "Lose ste uneli podatke";
                    response.StatusCode = StatusCode.BAD_FORMAT_CODE;
                    return response;
                }
                

                if(dataTable.Rows.Count == 0)
                {
                    response.Status = "Ne postoji u tabeli";
                    response.StatusCode = StatusCode.REJECTED_CODE;
                    return response;
                }

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    response.Payload.Resource.Add(new Resource(new ResourceType()));


                    response.Payload.Resource[i].ID = int.Parse(dataTable.Rows[i]["id"].ToString());
                    response.Payload.Resource[i].Name = dataTable.Rows[i]["rname"].ToString();
                    response.Payload.Resource[i].Description = dataTable.Rows[i]["description"].ToString();
                    response.Payload.Resource[i].Title = dataTable.Rows[i]["title"].ToString();

                    if (dataTable.Rows[i]["type"].ToString().ToLower() != "null" && dataTable.Rows[i]["type"].ToString() != "")
                    {
                        int secondId = int.Parse(dataTable.Rows[i]["type"].ToString()); //the id we are looking in the second table
                        sqlCommand2 = new SqlCommand(String.Format(secondTable + secondId)); //

                        sqlCommand2.Connection = sqlConnection;

                        dataAdapter2 = new SqlDataAdapter(sqlCommand2);
                        dataTable2 = new DataTable();
                        dataAdapter2.Fill(dataTable2);


                        response.Payload.Resource[i].Type.ID = int.Parse(dataTable2.Rows[0]["id"].ToString());
                        response.Payload.Resource[i].Type.Title = dataTable2.Rows[0]["title"].ToString();
                    }
                    else
                    {
                        sqlCommand = new SqlCommand("SELECT * FROM TypeTable WHERE id=1"); //ako nije naveo type, stavljamo da je 1
                        sqlCommand.Connection = sqlConnection;

                        dataAdapter = new SqlDataAdapter(sqlCommand);
                        dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        response.Payload.Resource[0].Type.ID = 1;
                        response.Payload.Resource[0].Type.Title = dataTable.Rows[0]["title"].ToString();


                    }

                    //  response.StatusCode = StatusCode.SUCCESS_CODE;
                    //  response.Status = "SUCCESS";



                }



            }

            else if (sqlSplited[0] == "INSERT")
            {
                if(query.Contains("type"))
                {
                    // razdvojimo na tokene i dobijemo unutar zagrada koje vrednosti unosi
                    string[] tokens = sqlQuery.Split('(');
                    string[] columnNames = tokens[1].Split(')');
                    string[] columns = columnNames[0].Split(',');

                    // zatim pronadjemo koja je po redu (od 0) vrednost 'type'
                    // a ako nema 'type' unesen onda je -1             
                    int typeIndex = -1;
                    for(int i = 0; i < columns.Length; i++)
                    {
                        if (columns[i].Contains("type"))
                        {
                            typeIndex = i;
                            break;
                        }
                    }

                    // sad kad imamo index onda samo preuzmemo iz VALUES(...) vrednost type
                    int type = -1;
                    if(typeIndex != -1)
                    {
                        string[] valueTokens = tokens[2].Split(')');
                        string[] values = valueTokens[0].Split(',');
                        type = Int32.Parse(values[typeIndex]);
                    }
                    else
                    {
                        // ovde postaviti type na default vrednost, ne znam tacno kako si ceo ovaj insert radio
                    }

                    int idx = query.IndexOf("VALUES (") + 8;
                    string resultType = query.Substring(idx);
                    string[] result = resultType.Split(',');
                    string[] lastResult = result.Last().Split(')');

                    sqlCommand3 = new SqlCommand("SELECT * FROM TypeTable");
                    sqlCommand3.Connection = sqlConnection;

                    dataAdapter3 = new SqlDataAdapter(sqlCommand3);
                    dataTable3 = new DataTable();
                    dataAdapter3.Fill(dataTable3);

                    // ovde sam izmenio da proveri vrednost type
                    if(type > dataTable3.Rows.Count || type == -1)
                    {
                        response.Status = "Ne postoji takav type";
                        response.StatusCode = StatusCode.REJECTED_CODE;
                        return response;
                    }
                    
                }
                try
                {
                    sqlCommand.ExecuteNonQuery(); //izvrsi
                }
                catch
                {
                    response.Status = "Lose ste uneli podatke";
                    response.StatusCode = StatusCode.BAD_FORMAT_CODE;
                    return response;
                }

                

                if (!query.Contains("connection")) //za resource
                {

                    sqlCommand3 = new SqlCommand("SELECT * FROM resource WHERE id = SCOPE_IDENTITY();"); //vracanje novog
                    sqlCommand3.Connection = sqlConnection;

                    dataAdapter3 = new SqlDataAdapter(sqlCommand3);
                    dataTable3 = new DataTable();
                    dataAdapter3.Fill(dataTable3);

                    response.Payload.Resource.Add(new Resource(new ResourceType()));

                    response.Payload.Resource[0].ID = int.Parse(dataTable3.Rows[0]["id"].ToString());
                    response.Payload.Resource[0].Name = dataTable3.Rows[0]["rname"].ToString();
                    response.Payload.Resource[0].Description = dataTable3.Rows[0]["description"].ToString();
                    response.Payload.Resource[0].Title = dataTable3.Rows[0]["title"].ToString();

                    ////upis u drugu tabelu:
                    //if (query.Contains("type"))
                    //{
                    //    int idx1 = query.IndexOf("VALUES") + 6;

                    //    string commandFortType = query.Substring(idx1); // (name, type)
                    //    string[] getType = commandFortType.Split(','); //[0]'name', [1]'type'
                    //    sqlCommand3 = new SqlCommand("INSERT INTO TypeTable (title) VALUES (" + getType.Last());
                    //}
                    //else //ako ne upisuje type onda stavljamo null
                    //{
                    //    sqlCommand3 = new SqlCommand("INSERT INTO TypeTable (title) VALUES (null)");
                    //}
                    //sqlCommand3.Connection = sqlConnection;
                    //sqlCommand3.ExecuteNonQuery();

                    //pristpu drugoj tabeli:
                    if (dataTable3.Rows[0]["type"].ToString().ToLower() != "null" && dataTable3.Rows[0]["type"].ToString() != "")
                    {
                        int secondId = int.Parse(dataTable3.Rows[0]["type"].ToString()); //the id we are looking in the second table
                        sqlCommand2 = new SqlCommand(String.Format(secondTable + secondId)); //

                        sqlCommand2.Connection = sqlConnection;

                        dataAdapter2 = new SqlDataAdapter(sqlCommand2);
                        dataTable2 = new DataTable();
                        dataAdapter2.Fill(dataTable2);


                        response.Payload.Resource[0].Type.ID = int.Parse(dataTable2.Rows[0]["id"].ToString());
                        response.Payload.Resource[0].Type.Title = dataTable2.Rows[0]["title"].ToString();
                    }
                    else
                    {
                        sqlCommand = new SqlCommand("SELECT * FROM TypeTable WHERE id=1"); //ako nije naveo type, stavljamo da je 1
                        sqlCommand.Connection = sqlConnection;

                        dataAdapter = new SqlDataAdapter(sqlCommand);
                        dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        response.Payload.Resource[0].Type.ID = 1;
                        response.Payload.Resource[0].Type.Title = dataTable.Rows[0]["title"].ToString();

                    }
                }

                else //za connection taeblu
                {
                    sqlCommand3 = new SqlCommand("SELECT * FROM connection WHERE id = SCOPE_IDENTITY();"); //vracanje novog
                    sqlCommand3.Connection = sqlConnection;

                    dataAdapter3 = new SqlDataAdapter(sqlCommand3);
                    dataTable3 = new DataTable();
                    dataAdapter3.Fill(dataTable3);

                    response.Payload.Resource.Add(new Resource(new ResourceType()));

//-------------------------------------------------------------------------------Menjati kad se promeni resource:---------------------------------------------------------
                    response.Payload.Resource[0].ID = int.Parse(dataTable3.Rows[0]["id"].ToString()); //menjati kad se promeni resource
                    response.Payload.Resource[0].Description = dataTable3.Rows[0]["one"].ToString() + dataTable3.Rows[0]["two"].ToString();
//-------------------------------------------------------------------------------------------------------------------------------------------------
                }

                //  ress.StatusCode = StatusCode.SUCCESS_CODE;
                //  ress.Status = "SUCCESS";

                //  responses.Add(ress);
            }
            else if (sqlSplited[0] == "UPDATE")
            {

                try
                {
                    sqlCommand.ExecuteNonQuery(); //izvrsi
                }
                catch
                {
                    response.Status = "Lose ste uneli podatke";
                    response.StatusCode = StatusCode.BAD_FORMAT_CODE;
                    return response;
                }

                string commandType = "SELECT * FROM resource WHERE id=";
                int idx = query.IndexOf("id=") + 3;
                string command = query.Substring(idx);
                string[] lastCommand = command.Split(';');

                sqlCommand3 = new SqlCommand(commandType + lastCommand[0]); //vracanje novog
                sqlCommand3.Connection = sqlConnection;

                dataAdapter3 = new SqlDataAdapter(sqlCommand3);
                dataTable3 = new DataTable();
                dataAdapter3.Fill(dataTable3);
                if(dataTable3.Rows.Count == 0)
                {
                    response.Status = "Ne postoji taj id u tabeli";
                    response.StatusCode = StatusCode.REJECTED_CODE;
                    return response;
                }

                string commandForReturningUpdated = "SELECT * FROM resource ";
                int takingId = query.IndexOf("WHERE");
                commandForReturningUpdated += query.Substring(takingId);

                sqlCommand3 = new SqlCommand(commandForReturningUpdated); //vracanje novog
                sqlCommand3.Connection = sqlConnection;

                dataAdapter3 = new SqlDataAdapter(sqlCommand3);
                dataTable3 = new DataTable();
                dataAdapter3.Fill(dataTable3);

                response.Payload.Resource.Add(new Resource(new ResourceType()));

                response.Payload.Resource[0].ID = int.Parse(dataTable3.Rows[0]["id"].ToString());
                response.Payload.Resource[0].Name = dataTable3.Rows[0]["rname"].ToString();
                response.Payload.Resource[0].Description = dataTable3.Rows[0]["description"].ToString();
                response.Payload.Resource[0].Title = dataTable3.Rows[0]["title"].ToString();

                //update u drugoj tabeli:
                //if (query.Contains("type"))
                //{
                //    int idx1 = query.IndexOf("type=") + 5; //pocetak
                //    int idx2 = query.IndexOf("WHERE"); //kraj

                //    string commandFortType = query.Substring(idx1, idx2 - idx1);
                //    sqlCommand3 = new SqlCommand("UPDATE TypeTable SET title=" + commandFortType + "WHERE id=" + response.Payload.Resource[0].ID);
                //    sqlCommand3.Connection = sqlConnection;
                //    sqlCommand3.ExecuteNonQuery();
                //}

                //pristpu drugoj tabeli:

                if (dataTable3.Rows[0]["type"].ToString().ToLower() != "null" && dataTable3.Rows[0]["type"].ToString() != "")
                {
                    int secondId = int.Parse(dataTable3.Rows[0]["type"].ToString()); //the id we are looking in the second table
                    sqlCommand2 = new SqlCommand(String.Format(secondTable + secondId)); //

                    sqlCommand2.Connection = sqlConnection;

                    dataAdapter2 = new SqlDataAdapter(sqlCommand2);
                    dataTable2 = new DataTable();
                    dataAdapter2.Fill(dataTable2);


                    response.Payload.Resource[0].Type.ID = int.Parse(dataTable2.Rows[0]["id"].ToString());
                    response.Payload.Resource[0].Type.Title = dataTable2.Rows[0]["title"].ToString();
                }
                else
                {
                    sqlCommand = new SqlCommand("SELECT * FROM TypeTable WHERE id=1"); //ako nije naveo type, stavljamo da je 1
                    sqlCommand.Connection = sqlConnection;

                    dataAdapter = new SqlDataAdapter(sqlCommand);
                    dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    response.Payload.Resource[0].Type.ID = 1;
                    response.Payload.Resource[0].Type.Title = dataTable.Rows[0]["title"].ToString();

                }




            }

            else //delete
            {
                //string deleteInTheSecondTable = "DELETE FROM TypeTable WHERE id=";
                //string takeCommand = "SELECT * FROM resource WHERE ";
                //int idx1 = query.IndexOf("WHERE ") + 6;
                //string arguments = query.Substring(idx1);

                //sqlCommand3 = new SqlCommand(takeCommand + arguments); //vracanje novog
                //sqlCommand3.Connection = sqlConnection;

                //dataAdapter3 = new SqlDataAdapter(sqlCommand3);
                //dataTable3 = new DataTable();
                //dataAdapter3.Fill(dataTable3);

                //for (int i = 0; i < dataTable3.Rows.Count; i++)
                //{
                //    string id = dataTable3.Rows[i]["id"].ToString() + ";";

                //       sqlCommand2 = new SqlCommand(deleteInTheSecondTable + id);
                //       sqlCommand2.Connection = sqlConnection;
                //     sqlCommand2.ExecuteNonQuery();      //brisemo iz prvo iz TypeTable
                //   }


                try
                {
                    sqlCommand.ExecuteNonQuery(); //izvrsi
                }
                catch
                {
                    response.Status = "Lose ste uneli podatke";
                    response.StatusCode = StatusCode.BAD_FORMAT_CODE;
                    return response;
                }




            }


            response.Status = "SUCCESS";
            response.StatusCode = StatusCode.SUCCESS_CODE;

            return response;
        }
    }
}
