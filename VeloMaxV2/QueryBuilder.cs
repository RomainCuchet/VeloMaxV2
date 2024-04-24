using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VeloMaxV2
{
    internal class QueryBuilder
    {
        public static string get_insert_query(Table table, int indice)
        {
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();
            foreach (string key in table.qr.data.Keys)
            {
                if (!key.Contains(table.name))
                {
                    columns.Append(key + ", ");
                    values.Append($"{table.qr.data[key][indice]}, ");
                }
            }

            columns.Length -= 2; // Supprimer la virgule et l'espace supplémentaires à la fin
            values.Length -= 2; // Supprimer la virgule et l'espace supplémentaires à la fin

            return $"INSERT INTO {table.name} ({columns}) VALUES ({values});";
        }
    }
}
