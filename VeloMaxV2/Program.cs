using MySqlX.XDevAPI;
using System;
using static Mysqlx.Expect.Open.Types.Condition.Types;
using System.Xml.Linq;

namespace VeloMaxV2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Interface("VeloMax", "../../../").initialize();
        }
    }
}