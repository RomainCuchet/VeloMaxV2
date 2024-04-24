using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

namespace VeloMaxV2
{
    internal class Menu
    {
        public string name;
        public string[] prompts;
        public List<Action> actions;
        public Menu menu_parent;
        public List<Menu> next_menus;
        public Menu(string[] prompts, List<Action> actions = null, List<Menu> next_menus = null, string name = "", Menu menu_parent = null,bool verif = false)
        {
            this.name = name;
            if (actions == null)
            {
                actions = new List<Action>();
            }
            if (next_menus == null)
            {
                next_menus = new List<Menu>();
            }
            if (verif && prompts.Length != actions.Count)
            {
                throw new Exception("Menu must have the same number of prompts and actions ");
            }
            this.actions = new List<Action>();
            this.next_menus = next_menus;
            string[] choices_list = new string[prompts.Length + next_menus.Count];
            for (int i = 0; i < next_menus.Count; i++)
            {
                Menu currentNextMenu = next_menus[i];
                choices_list[i] = $"Menu {currentNextMenu.name}";
                this.actions.Add(() => { currentNextMenu.Activate(); });
            }

            for (int i = 0; i < prompts.Length; i++)
            {
                choices_list[i + next_menus.Count] = prompts[i];
            }
            this.prompts = choices_list;
            this.menu_parent = menu_parent;
            foreach (Action a in actions)
            {
                this.actions.Add(a);
            }

            foreach (Menu m in next_menus)
            {
                m.menu_parent = this;
            }
        }

        public void Activate()
        {
            int i = new Scrolling_menu(prompts, name).get_value();
            if (i == -1) { Back(); }
            actions[i]();
        }

        public static void Display(Menu menu, string s)
        {
            Console.WriteLine(s);
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey(true);
            menu.Activate();
        }

        public void InsertActions(List<Action> actions)
        {
            foreach (Action a in actions)
            {
                this.actions.Add(a);
            }
        }

        public void Back()
        {
            if (menu_parent == null)
            {
                Environment.Exit(0);
            }
            menu_parent.Activate();
        }

        public static void print_list_of_string(string[] array)
        {
            foreach (string s in array) { foreach (char c in s) { Console.Write(c); } Console.Write(" "); }
            Console.WriteLine();
        }

        public void Print(object obj)
        {
            Console.WriteLine(obj);
            Console.WriteLine("Cliquer sur une touche pour continuer");
            Activate();
        }
    }
}
