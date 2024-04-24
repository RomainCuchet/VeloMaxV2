using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VeloMaxV2
{
    internal class QueryResult
    {
        static int cpt = 1;
        public Dictionary<string, List<object>> data;
        public bool succes;
        public string error_message;
        public int id;
        bool print_id;

        public QueryResult(bool print_id = false)
        {
            data = new Dictionary<string, List<object>>();
            id = cpt;
            cpt++;
            this.print_id = print_id;
        }

        public void add_column(string column,object default_value)
        {
            data.Add(column, new List<object>());
            for(int i = 0;i< data[data.Keys.First()].Count; i++)
            {
                data[column].Add(default_value);
            }
        }

        public string[] strins_column(string column)
        {
            List<object> vals = data[column];
            string[]  columns = new string[vals.Count];
            for(int i =0;i < vals.Count; i++)
            {
                columns[i] = (string)vals[i];
            }
            return columns;
        }

        #region display
        public void Display(bool with_type = false)
        {
            Console.WriteLine(Tostring(with_type));
        }
        public string column_tostring(string column)
        {
            string res = $"{column}\n";
            for (int i = 0; i < data[column].Count; i++)
            {
                res += data[column][i] + "\n";
            }
            return res;
        }

        public int get_length()
        {
            try { return data[data.Keys.First()].Count(); }
            catch { return -1; };
        }

        public string Tostring(bool with_type = false)
        {
            if (data == null)
            {
                if (succes) return "data is null";
            }
            string res ="";
            if (print_id) res += $"requête {id} : \n";
            if (!succes) return res + "ERROR \n" + error_message;
            IEnumerable<string> keys = data.Keys;
            foreach (string key in keys)
            {
                res += ($"{key} ");
            }
            res += "\n";
            int nb_tuples = get_length();
            for (int i = 0; i < nb_tuples; i++)
            {
                foreach (string key in keys)
                {
                    if (with_type) res += ($"{data[key][i]} : {data[key][i].GetType()} ");
                    else res += (($"{data[key][i]} "));

                }
                res += "\n";
            }
            return res;

        }
        #endregion
    }
}
