﻿using SharedResources;
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
        }

        private void ReadConfig()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(@"..\..\..\config.xml");
            XmlNode sqlConnectionString = xmlDocument.DocumentElement.SelectSingleNode("/config/sqlconnectionstring");
            connectionString = sqlConnectionString.InnerText;
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
                dataAdapter.Fill(dataTable);


                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    response.Payload.Resource.Add(new Resource(new ResourceType()));


                    response.Payload.Resource[i].ID = int.Parse(dataTable.Rows[i]["id"].ToString());
                    response.Payload.Resource[i].Name = dataTable.Rows[i]["rname"].ToString();
                    response.Payload.Resource[i].Description = dataTable.Rows[i]["description"].ToString();
                    response.Payload.Resource[i].Title = dataTable.Rows[i]["title"].ToString();

                    int secondId = response.Payload.Resource[i].ID; //the id we are looking in the second table
                    sqlCommand2 = new SqlCommand(String.Format(secondTable + secondId)); //

                    sqlCommand2.Connection = sqlConnection;

                    dataAdapter2 = new SqlDataAdapter(sqlCommand2);
                    dataTable2 = new DataTable();
                    dataAdapter2.Fill(dataTable2);


                    response.Payload.Resource[i].Type.ID = int.Parse(dataTable2.Rows[0]["id"].ToString());
                    response.Payload.Resource[i].Type.Title = dataTable2.Rows[0]["title"].ToString();


                    //  response.StatusCode = StatusCode.SUCCESS_CODE;
                    //  response.Status = "SUCCESS";



                }



            }

            else if (sqlSplited[0] == "INSERT")
            {
                sqlCommand.ExecuteNonQuery(); //izvrsi

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

                //upis u drugu tabelu:
                if (query.Contains("type"))
                {
                    int idx1 = query.IndexOf("VALUES") + 6;

                    string commandFortType = query.Substring(idx1); // (name, type)
                    string[] getType = commandFortType.Split(','); //[0]'name', [1]'type'
                    sqlCommand3 = new SqlCommand("INSERT INTO TypeTable (title) VALUES (" + getType.Last());
                }
                else //ako ne upisuje type onda stavljamo null
                {
                    sqlCommand3 = new SqlCommand("INSERT INTO TypeTable (title) VALUES (null)");
                }
                sqlCommand3.Connection = sqlConnection;
                sqlCommand3.ExecuteNonQuery();

                //pristpu drugoj tabeli:
                int secondId = response.Payload.Resource[0].ID; //the id we are looking in the second table
                sqlCommand2 = new SqlCommand(String.Format(secondTable + secondId)); //

                sqlCommand2.Connection = sqlConnection;

                dataAdapter2 = new SqlDataAdapter(sqlCommand2);
                dataTable2 = new DataTable();
                dataAdapter2.Fill(dataTable2);


                response.Payload.Resource[0].Type.ID = int.Parse(dataTable2.Rows[0]["id"].ToString());
                response.Payload.Resource[0].Type.Title = dataTable2.Rows[0]["title"].ToString();

                //  ress.StatusCode = StatusCode.SUCCESS_CODE;
                //  ress.Status = "SUCCESS";

                //  responses.Add(ress);
            }
            else if (sqlSplited[0] == "UPDATE")
            {
                sqlCommand.ExecuteNonQuery(); //izvrsi

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
                if (query.Contains("type"))
                {
                    int idx1 = query.IndexOf("type=") + 5; //pocetak
                    int idx2 = query.IndexOf("WHERE"); //kraj

                    string commandFortType = query.Substring(idx1, idx2 - idx1);
                    sqlCommand3 = new SqlCommand("UPDATE TypeTable SET title=" + commandFortType + "WHERE id=" + response.Payload.Resource[0].ID);
                    sqlCommand3.Connection = sqlConnection;
                    sqlCommand3.ExecuteNonQuery();
                }

                //pristpu drugoj tabeli:
                int secondId = response.Payload.Resource[0].ID; //the id we are looking in the second table
                sqlCommand2 = new SqlCommand(String.Format(secondTable + secondId)); //

                sqlCommand2.Connection = sqlConnection;

                dataAdapter2 = new SqlDataAdapter(sqlCommand2);
                dataTable2 = new DataTable();
                dataAdapter2.Fill(dataTable2);


                response.Payload.Resource[0].Type.ID = int.Parse(dataTable2.Rows[0]["id"].ToString());
                response.Payload.Resource[0].Type.Title = dataTable2.Rows[0]["title"].ToString();

            }

            else //delete
            {
                string deleteInTheSecondTable = "DELETE FROM TypeTable WHERE id=";
                string takeCommand = "SELECT * FROM resource WHERE ";
                int idx1 = query.IndexOf("WHERE ") + 6;
                string arguments = query.Substring(idx1);

                sqlCommand3 = new SqlCommand(takeCommand + arguments); //vracanje novog
                sqlCommand3.Connection = sqlConnection;

                dataAdapter3 = new SqlDataAdapter(sqlCommand3);
                dataTable3 = new DataTable();
                dataAdapter3.Fill(dataTable3);

                for (int i = 0; i < dataTable3.Rows.Count; i++)
                {
                    string id = dataTable3.Rows[i]["id"].ToString() + ";";

                    sqlCommand2 = new SqlCommand(deleteInTheSecondTable + id);
                    sqlCommand2.Connection = sqlConnection;
                    sqlCommand2.ExecuteNonQuery();      //brisemo iz prvo iz TypeTable
                }


                sqlCommand.ExecuteNonQuery(); //obrisi




            }


            response.Status = "SUCCESS";
            response.StatusCode = StatusCode.SUCCESS_CODE;

            return response;
        }
    }
}
