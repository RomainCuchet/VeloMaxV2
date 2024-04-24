using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloMaxV2
{
    internal class Test
    {
        public static void insert()
        {
            Table table = new("Client");
            table.display();
            Console.WriteLine(table.insert_tuple(2));

        }
        public static void get_tables()
        {
            Interface.print_listarray(DataBase.get_nom_tables());
        }
        public static void test_type()
        {
            Table table = new("Adresse");
            table.display();
            Console.WriteLine(Convert.ToInt32(table.qr.data["idAdresse"][0])+ Convert.ToInt32(table.qr.data["idAdresse"][0]));
            Console.WriteLine((int) table.qr.data["idAdresse"][0] );
            Table table2 = new("Salarie");
            table2.display();
            Console.WriteLine((DateTime) table2.qr.data["date_arrivee"][0]);
        }
    }
}
