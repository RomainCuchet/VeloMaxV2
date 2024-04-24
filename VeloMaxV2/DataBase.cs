using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using System.Text;
using System.Threading.Tasks;

namespace VeloMaxV2
{
    internal class DataBase
    {
        public static string connection_string = "Server=localhost;Database=velomax;Uid=root;Pwd=root;";
        static bool test = true;
        static string name = "velomax";
        public static QueryResult execute_query(string query, bool select_display = false)
        {
            QueryResult qr = new QueryResult(); // stands dor query_result
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connection_string))
                {
                    connection.Open();
                    string field_name;
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            bool first = true;
                            while (reader.Read())
                            {
                                if (first)
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        qr.data.Add(reader.GetName(i), new List<object>());
                                    }
                                    first = false;
                                }
                                // Accéder aux données par leur nom de champ
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    field_name = reader.GetName(i);
                                    qr.data[field_name].Add(reader[field_name]);
                                }
                            }
                        }
                    }
                    qr.succes = true;
                }
                if (select_display)
                {
                    if (test) Console.WriteLine(query);
                    if (query.Substring(0, 6) == "SELECT") qr.Display();

                }


            }
            catch (Exception ex)
            {
                qr.succes = false;
                qr.error_message = $"{query}: {ex.Message}";
                qr.Display();
            }
            return qr;
        }
        public static bool check_fkey(int id_fkey, string champ, string table)
        {
            string query = $"SELECT EXISTS (SELECT 1 FROM {table} WHERE {champ} = {id_fkey}) AS {champ};";
            return Convert.ToInt32(execute_query(query).data[champ][0]) != 0;
        }
        public static bool check_primary_key(string tableName, string primaryKeyColumnName, int primaryKeyValue)
        {
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE {primaryKeyColumnName} = {primaryKeyValue};";
            QueryResult result = execute_query(query);
            int count = Convert.ToInt32(result.data[primaryKeyColumnName][0]);
            return count > 0;
        }

        public static int[] get_fkeys(string fkey, string table)
        {
            return execute_query($"SELECT {fkey} FROM {table}").data[fkey].OfType<int>().ToArray();
        }

        public static string[] get_nom_tables()
        {
            QueryResult qr = execute_query($"SELECT table_name FROM information_schema.tables WHERE table_schema = '{name}' AND table_type = 'BASE TABLE';");
            return qr.strins_column("TABLE_NAME");
        }

        public static void change_name(string new_name)
        {
            name = new_name;
        }
    }
}
