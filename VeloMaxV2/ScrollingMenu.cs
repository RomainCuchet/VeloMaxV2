using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeloMaxV2;

namespace VeloMaxV2
{
    internal class Scrolling_menu
    {
        string[] prompts;
        bool is_activated;
        string title;
        Menu menu;


        public Scrolling_menu(string[] prompts, string title = "")
        {
            this.prompts = prompts;
            this.title = title;
        }

        void display(int i)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{title}");
            Console.WriteLine("*********************");
            Console.ResetColor();
            for (int j = 0; j < prompts.Length; j++)
            {
                if (j == i)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(prompts[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(prompts[j]);
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("*********************");
            Console.ResetColor();
        }

        public int get_value()
        {
            bool is_activated = true;
            ConsoleKeyInfo key;
            int i = 0;
            do
            {
                Console.Clear();
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Interface.print_center("************");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Interface.print_center("VeloMax");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Interface.print_center("************");
                Console.ResetColor();
                Console.WriteLine();
                display(i);
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (i == 0) i = prompts.Length - 1;
                    else i--;
                }
                if (key.Key == ConsoleKey.DownArrow)
                {
                    if (i == prompts.Length - 1) i = 0;
                    else i++;
                }
                if (key.Key == ConsoleKey.Enter)
                {
                    is_activated = false;
                }
                if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.LeftArrow)
                {
                    i = -1;
                    is_activated = false;
                }
                if (key.Key == ConsoleKey.F4)
                {
                    Environment.Exit(0);
                }
            }
            while (is_activated);
            return i;
        }
    }
}
