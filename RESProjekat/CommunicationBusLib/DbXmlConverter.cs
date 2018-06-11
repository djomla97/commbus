using Newtonsoft.Json;
using SharedResources.Interfaces;
using System;
using System.Text;
using System.Xml.Linq;

namespace CommunicationBusLib
{
    public class DbXmlConverter
    {
        public string ConvertToXml(IResponse response)
        {
            string jsonResponse = JsonConvert.SerializeObject(response);
            XNode xmlResponse = JsonConvert.DeserializeXNode(jsonResponse, "Response");

            return xmlResponse.ToString(); // ovo ima \r\n  i tabove (zapravo 2 space-a)
        }

        public string ConvertFromXml(string request)
        {
            StringBuilder sqlRequest = new StringBuilder();
            request = request.Replace("\r\n", "");
            request = request.Replace(" ", "");

            // pozicije stringova
            int firstIndexOfVerb = request.IndexOf("<Verb>") + 6;
            int lastIndexOfVerb = request.IndexOf("</Verb>");

            int firstIndexOfNoun = request.IndexOf("<Noun>") + 6;
            int lastIndexOfNoun = request.IndexOf("</Noun>");

            // mozda nije unet query ili fields
            bool foundQuery = false;
            bool foundFields = false;
     
            int firstIndexOfQuery = request.IndexOf("<Query>");
            int lastIndexOfQuery = request.IndexOf("</Query>");

            if (firstIndexOfQuery != -1 || lastIndexOfQuery != -1)
            {
                firstIndexOfQuery += 7;
                foundQuery = true;
            }

            int firstIndexOfFields = request.IndexOf("<Fields>");
            int lastIndexOfFields = request.IndexOf("</Fields>");

            if (firstIndexOfFields != -1 || lastIndexOfFields != -1)
            {
                firstIndexOfFields += 8;
                foundFields = true;
            }

            // vrednosti polja             
            string verb = request.Substring(firstIndexOfVerb , lastIndexOfVerb - firstIndexOfVerb);
            string noun = request.Substring(firstIndexOfNoun, lastIndexOfNoun - firstIndexOfNoun);
            string table = noun.Split('/')[1];

            string query = string.Empty;
            string[] queries = null;
            string fields = string.Empty;

            if (foundQuery)
            {
                query = request.Substring(firstIndexOfQuery, lastIndexOfQuery - firstIndexOfQuery);
                queries = query.Split(';');
            }

            if (foundFields)
            {
                fields = request.Substring(firstIndexOfFields, lastIndexOfFields - firstIndexOfFields);
                fields = fields.Replace(";", ",");
            }


            string id = string.Empty;
            // konacne komande
            switch (verb)
            {
                case "GET":
                    if (foundFields)
                    {
                        sqlRequest.Append($"SELECT {fields} FROM {table} ");

                        if (foundQuery)
                        {
                            sqlRequest.Append("WHERE ");
                            for (int i = 0; i < queries.Length; i++)
                            {
                                if (i == queries.Length - 1)
                                    sqlRequest.Append($"{queries[i]};");
                                else
                                    sqlRequest.Append($"{queries[i]} AND ");
                            }
                        }
                        else
                        {
                            id = noun.Split('/')[2];
                            sqlRequest.Append($"WHERE id={id};");
                        }
                    }
                    else
                    {
                        sqlRequest.Append($"SELECT * FROM {table} ");

                        if (foundQuery)
                        {
                            sqlRequest.Append("WHERE ");
                            for (int i = 0; i < queries.Length; i++)
                            {
                                if (i == queries.Length - 1)
                                    sqlRequest.Append($"{queries[i]};");
                                else
                                    sqlRequest.Append($"{queries[i]} AND ");
                            }
                        }
                        else
                        {
                            id = noun.Split('/')[2];
                            sqlRequest.Append($"WHERE id={id};");
                        }
                    }

                    break;

                case "POST":
                    sqlRequest.Append($"INSERT INTO {table} (");
                    for(int i = 0; i < queries.Length; i++)
                    {
                        if(i == queries.Length - 1)
                            sqlRequest.Append($"{queries[i].Split('=')[0]})");
                        else
                            sqlRequest.Append($"{queries[i].Split('=')[0]}, ");
                    }

                    sqlRequest.Append(" VALUES (");
                    for (int i = 0; i < queries.Length; i++)
                    {
                        if (i == queries.Length - 1)
                            sqlRequest.Append($"{queries[i].Split('=')[1]});");
                        else
                            sqlRequest.Append($"{queries[i].Split('=')[1]}, ");
                    }

                    break;

                case "PATCH":
                    sqlRequest.Append($"UPDATE {table} SET ");
                    for (int i = 0; i < queries.Length; i++)
                    {
                        if (i == queries.Length - 1)
                            sqlRequest.Append($"{queries[i]} WHERE ");
                        else
                            sqlRequest.Append($"{queries[i]}, ");
                    }
                    id = noun.Split('/')[2];
                    sqlRequest.Append($"id={id};");

                    break;

                case "DELETE":
                    try
                    {
                        id = noun.Split('/')[2];
                        sqlRequest.Append($"DELETE FROM {table} WHERE id={id};");
                    }
                    catch (Exception)
                    {
                        sqlRequest.Append($"DELETE FROM {table} WHERE ");
                        for (int i = 0; i < queries.Length; i++)
                        {
                            if (i == queries.Length - 1)
                                sqlRequest.Append($"{queries[i]};");
                            else
                                sqlRequest.Append($"{queries[i]} AND ");
                        }
                    }

                    break;

                default:
                    //sqlRequest.Append("test");
                    break;
            }
            
            return sqlRequest.ToString();
        }
    }
}
