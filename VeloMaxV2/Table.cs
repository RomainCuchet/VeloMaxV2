using Org.BouncyCastle.Asn1.X509.Qualified;
using Org.BouncyCastle.Security.Certificates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloMaxV2
{
    internal class Table
    {
        public string name;
        int indice_new;
        public QueryResult qr = null;

        public Table(string name)
        {
            this.name = name;
            qr = DataBase.execute_query($"SELECT * FROM {name};");
            indice_new = qr.data.Count-1;
        }

        public static string Delete(string name)
        {
            int n;
            bool valid;
            try
            {
                DataBase.execute_query($"SELECT * FROM {name};").Display();
                do
                {
                    Console.WriteLine("Pour sélectionner un élement veuillez entrer son identifiant");
                    n = Interface.GetValideIntFomConsole();
                    valid = DataBase.check_fkey(n, $"id{name}", name);
                    if (!valid)
                    {
                        Console.WriteLine($"La clef {n} n'existe pas");
                        (DataBase.execute_query($"SELECT * FROM {name}")).Display();
                    }

                }
                while (!valid);
                return $"Supression du tuple : {(DataBase.execute_query($"DELETE FROM {name} WHERE id{name} ={n}")).succes}";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string Update(string name)
        {
            try
            {
                (DataBase.execute_query($"SELECT * FROM {name}")).Display();
                Console.WriteLine("Pour sélectionner un élement veuillez entrer son identifiant");
                int n;
                bool valid = false;
                do
                {
                    n = Interface.GetValideIntFomConsole();
                    valid = DataBase.check_fkey(n, $"id{name}", name);
                    if (!valid)
                    {
                        Console.WriteLine($"La clef {n} n'existe pas");
                        (DataBase.execute_query($"SELECT * FROM {name}")).Display();
                    }

                }
                while (!valid);
                QueryResult qr = DataBase.execute_query($"SELECT * FROM {name} WHERE id{name} = {n}");
                string[] keys = qr.data.Keys.ToList().ToArray();
                bool exists = false;
                for (int i = 0; i < qr.data.Count(); i++)
                {
                    if (!keys[i].ToLower().Contains(name))
                    {
                        Console.WriteLine($"Voulez-vous modifier {keys[i]} ? (yes/no)");
                        if (Console.ReadLine() == "yes")
                        {
                            if (keys[i].Contains("id")) // foreign key
                            {
                                while (!exists)
                                {
                                    try
                                    {
                                        Console.WriteLine($"Veuillez entrer une valeur pour la clef étrangère : {keys[i]}");
                                        n = Convert.ToInt32(Console.ReadLine());
                                        exists = DataBase.check_fkey(n, keys[i], keys[i].Substring(2));
                                        if (!exists)
                                        {
                                            Console.WriteLine($"Les clefs étrangères pour {keys[i]} sont :");
                                            Interface.print_listarray(DataBase.get_fkeys(keys[i], keys[i].Substring(2)));
                                        }
                                        else { qr.data[keys[i]][0] = n; }
                                    }
                                    catch { Console.WriteLine($"Vous devez entrer un nombre"); }

                                }
                            }
                            else if (qr.data[keys[i]][0].GetType() == typeof(DateTime)) // datetime
                            {
                                Console.WriteLine(keys[i]);
                                qr.data[keys[i]][0] = Interface.GetValidDateTimeFromConsole();
                            }
                            else if (qr.data[keys[i]][0].GetType() == (typeof(int))) // datetime
                            {
                                Console.WriteLine(keys[i]);
                                qr.data[keys[i]][0] = Interface.GetValideIntFomConsole();
                            }
                            else if (qr.data[keys[i]][0].GetType() == typeof(float)) // datetime
                            {
                                Console.WriteLine(keys[i]);
                                qr.data[keys[i]][0] = Interface.GetValidFloatFromConsole();
                            }
                            else
                            {
                                Console.WriteLine(keys[i]);
                                qr.data[keys[i]][0] = Console.ReadLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Le paramètre sera inchangé");
                        }
                        
                    }
                }
                qr.Display();
                return $"Tuple modifié avec succès : {(DataBase.execute_query(update_tuple(qr, name,n))).succes}";
            }
            catch (Exception ex) { return ex.Message; }
            
        }
     
        public static string New(string name)
        {
            name = name.ToLower();
            try
            {
                QueryResult qr = DataBase.execute_query($"SELECT * FROM {name} LIMIT 1");
                string[] keys = qr.data.Keys.ToList().ToArray();
                bool exists = false;
                int n;
                for (int i = 0; i < qr.data.Count(); i++)
                {
                    if (!keys[i].ToLower().Contains(name))
                    {
                        if (keys[i].Contains("id")) // foreign key
                        {
                            while (!exists)
                            {
                                try
                                {
                                    Console.WriteLine($"Veuillez entrer une valeur pour la clef étrangère : {keys[i]}");
                                    n = Convert.ToInt32(Console.ReadLine());
                                    exists = DataBase.check_fkey(n, keys[i], keys[i].Substring(2));
                                    if (!exists)
                                    {
                                        Console.WriteLine($"Les clefs étrangères pour {keys[i]} sont :");
                                        Interface.print_listarray(DataBase.get_fkeys(keys[i], keys[i].Substring(2)));
                                    }
                                    else { qr.data[keys[i]][0] = n; }
                                }
                                catch { Console.WriteLine($"Vous devez entrer un nombre"); }
                                
                            }
                        }
                        else if (qr.data[keys[i]][0].GetType() == typeof(DateTime) ) // datetime
                        {
                            Console.WriteLine(keys[i]);
                            qr.data[keys[i]][0] = Interface.GetValidDateTimeFromConsole();
                        }
                        else if (qr.data[keys[i]][0].GetType() == (typeof(int))) // datetime
                        {
                            Console.WriteLine(keys[i]);
                            qr.data[keys[i]][0] = Interface.GetValideIntFomConsole();
                        }
                        else if (qr.data[keys[i]][0].GetType() == typeof(float)) // datetime
                        {
                            Console.WriteLine(keys[i]);
                            qr.data[keys[i]][0] = Interface.GetValidFloatFromConsole();
                        }
                        else
                        {
                            Console.WriteLine(keys[i]);
                            qr.data[keys[i]][0] = Console.ReadLine();
                        }
                    }
                }
                qr.Display();
                return $"Tuple ajouté avec succès : {(DataBase.execute_query(insert_tuple(qr, name))).succes}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string insert_tuple(int indice)
        {
            if(indice>= qr.get_length()||indice<indice_new)
            {
                throw new Exception($"La Table {name} n'a pas de tuples à {indice} ou tuple déja enregistré");
            }
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();
            foreach (string key in qr.data.Keys)
            {
                if (!key.Contains(name))
                {
                    columns.Append(key + ", ");
                    values.Append($"{qr.data[key][indice]}, ");
                }
            }

            columns.Length -= 2; // Supprimer la virgule et l'espace supplémentaires à la fin
            values.Length -= 2; // Supprimer la virgule et l'espace supplémentaires à la fin

            return $"INSERT INTO {name} ({columns}) VALUES ({values});";
        }

        public static string insert_tuple(QueryResult qr,string name)
        {
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();
            foreach (string key in qr.data.Keys)
            {
                if (!key.ToLower().Contains(name.ToLower()))
                {
                    columns.Append(key + ", ");
                    if(qr.data[key][0].GetType() == typeof(int) ) { values.Append($"{qr.data[key][0]}, "); }
                    else if(qr.data[key][0].GetType() == typeof(float)) { values.Append($"{((float)qr.data[key][0]).ToString("0.0",CultureInfo.InvariantCulture)}, "); }
                    else if(qr.data[key][0].GetType() == typeof(DateTime)) { values.Append($"'{((DateTime)qr.data[key][0]).ToString("yyyy-MM-dd HH:mm:ss")}', "); }
                    else { values.Append($"'{qr.data[key][0]}', "); };
                    
                }
            }

            columns.Length -= 2; // Supprimer la virgule et l'espace supplémentaires à la fin
            values.Length -= 2; // Supprimer la virgule et l'espace supplémentaires à la fin

            return $"INSERT INTO {name} ({columns}) VALUES ({values});";
        }

        public static string update_tuple(QueryResult qr, string name,int id)
        {
            string query = $"UPDATE {name} SET ";
            foreach (string key in qr.data.Keys)
            {
                
                if (!key.ToLower().Contains(name.ToLower()))
                {
                    query += key + "=";
                    if (qr.data[key][0].GetType() == typeof(int)) { query += $"{qr.data[key][0]}"; }
                    else if (qr.data[key][0].GetType() == typeof(float)) { query += $"{((float)qr.data[key][0]).ToString("0.0", CultureInfo.InvariantCulture)}"; }
                    else if (qr.data[key][0].GetType() == typeof(DateTime)) { query += $"'{((DateTime)qr.data[key][0]).ToString("yyyy-MM-dd HH:mm:ss")}"; }
                    else { query += $"'{qr.data[key][0]}'"; };
                    query += " ,";
                }
            }
            return query.Substring(0, query.Length - 2);
        }


        public string Tostring()
        {
            return $"Table {name}\n{qr.Tostring()}";
        }

        public void display()
        {
            Console.WriteLine(Tostring());
        }
    }


}
