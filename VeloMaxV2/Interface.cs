using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloMaxV2
{
    internal class Interface
    {
        public string name;
        public string racine;
        public Menu home;

        public Interface(string name, string racine)
        {
            Console.Title = name;
            this.name = name;
            this.racine = racine;
        }

        public void display(string s)
        {
            Console.Clear();
            Console.WriteLine(s);
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
            home.Activate();
        }

        public void initialize()
        {
            string[] files = DataBase.get_nom_tables();
            List<Action> actions = new List<Action>();
            foreach (string s in files)
            {
                actions.Add(() => {display(new Table(s).Tostring()) ; });
            }
            Menu menu_ConsulteTable = new(
                files,
                actions,
                name: "Consultation des tables"
            );

            Menu categorie_velo = new(
                prompts: ["VTT", "Vélo de course","Classisque","BMX"],
                name:"Catégorie de vélo"
                );

            actions = new List<Action>()
            {
                ()=> {Menu.Display(categorie_velo,(DataBase.execute_query("SELECT * FROM MODELE WHERE LIGNE_PRODUIT = 'VTT'")).Tostring()); },
                ()=> {Menu.Display(categorie_velo,(DataBase.execute_query("SELECT * FROM MODELE WHERE LIGNE_PRODUIT = 'Vélo de course'")).Tostring()); },
                ()=> {Menu.Display(categorie_velo,(DataBase.execute_query("SELECT * FROM MODELE WHERE LIGNE_PRODUIT = 'Classique'")).Tostring()); },
                ()=> {Menu.Display(categorie_velo,(DataBase.execute_query("SELECT * FROM MODELE WHERE LIGNE_PRODUIT = 'BMX'")).Tostring()); },
            };

            categorie_velo.InsertActions(actions);

           files = [
                "magasin",
                "pièces",
                "fournisseurs",
                "velo",
                ];

            Menu apercu_stocks = new(files,name: "Aperçu des stocks",next_menus:new List<Menu>() { categorie_velo});
            actions = new List<Action>()
            {
                () => {Menu.Display(apercu_stocks,(DataBase.execute_query("SELECT * FROM MAGASIN")).Tostring()); },
                () => {Menu.Display(apercu_stocks,(DataBase.execute_query("SELECT * FROM piece")).Tostring()); },
                () => {Menu.Display(apercu_stocks,(DataBase.execute_query("SELECT * FROM fournisseur")).Tostring()); },
                () => {Menu.Display(apercu_stocks,(DataBase.execute_query("SELECT * FROM MODELE")).Tostring()); },
            };
            apercu_stocks.InsertActions(actions);


            files = DataBase.get_nom_tables();
            Menu inserer_donnee = new(files,name: "Insérer de nouvelles données");
           
            actions = new List<Action>();
            foreach (string s in files)
            {
                actions.Add(() => { Menu.Display(inserer_donnee, Table.New(s)); });
            }
            inserer_donnee.InsertActions(actions);

            Menu modifier_donnee = new(files, name: "Modifier des données");
            actions = new List<Action>();
            foreach (string s in files)
            {
                actions.Add(() => { Menu.Display(modifier_donnee, Table.Update(s)); });
            }
            modifier_donnee.InsertActions(actions);

            Menu suppression = new(files, name: "Supression");
            actions = new List<Action>();
            foreach (string s in files)
            {
                actions.Add(() => { Menu.Display(modifier_donnee, Table.Delete(s)); });
            }
            suppression.InsertActions(actions);

            Menu menu_main = new(
                prompts: [
                "Evaluateur",
                "Statististiques",
                "SQL Interface",
                "Mes requêtes",
                ],
                actions: new List<Action>()
                {
                    () => { Actions.demo(this); },
                    () => { Actions.statistiques(this);},
                    () => { Actions.sql_interface(this); },
                    () => { Actions.mine(this); }
                },
                next_menus: new List<Menu>()
                {
                    menu_ConsulteTable,
                    apercu_stocks,
                    inserer_donnee,
                    modifier_donnee,
                    suppression,
                },
                "Menu principal"
            );

            home = menu_main;
            menu_main.Activate();

        }

        #region Tools

        public static DateTime GetValidDateTimeFromConsole()
        {
            DateTime result;
            string userInput;
            do
            {
                Console.Write("Veuillez saisir une date au format dd-MM-yyyy : ");
                userInput = Console.ReadLine();
            }
            while (!DateTime.TryParseExact(userInput, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out result)); // Continue jusqu'à ce que la saisie soit valide
            return result;
        }

        public static float GetValidFloatFromConsole()
        {
            float result = 0;
            bool valid = false;
            do
            {
                Console.WriteLine("Veuillez entrer un nombre à virgule flottante :");
                try
                {
                    result = Convert.ToSingle(Console.ReadLine());
                    valid = true;
                }
                catch { Console.WriteLine("Nombre non valide"); }
            } while (!valid);
            return result;
        }

        public static int GetValideIntFomConsole()
        {
            int result = 0;
            bool valid = false;
            do
            {
                Console.WriteLine("Veuillez entrez un entier");
                try
                {
                    result = Convert.ToInt32(Console.ReadLine());
                    valid = true;
                }
                catch { Console.WriteLine("Entier non valide"); };
            }
            while(!valid );
            return result;
        }

        public static string[] get_file_names_from_folder(string folderPath, string extension, bool with_ext = false)
        {
            List<string> file_names = new List<string>();

            try
            {
                Directory.GetFiles(folderPath, $"*.{extension}");
                // Vérifier si le dossier existe
                if (Directory.Exists(folderPath))
                {
                    // Obtenir tous les fichiers CSV dans le dossier
                    string[] files_names = Directory.GetFiles(folderPath, $"*.{extension}");

                    // Obtenir uniquement les noms de fichiers
                    foreach (string csvFile in files_names)
                    {
                        string fileName = Path.GetFileName(csvFile);
                        if (!with_ext)
                        {
                            fileName = fileName.Substring(0, fileName.Length - 1 - extension.Length);
                        }
                        file_names.Add(fileName);
                    }
                }
                else
                {
                    Console.WriteLine($"Le dossier spécifié n'existe pas :");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }
            return file_names.ToArray();
        }

        public static void print_center(string text)
        {
            int screenWidth = Console.WindowWidth;

            int leftPadding = (screenWidth - text.Length) / 2;
            if (leftPadding < 0)
            {
                leftPadding = 0; // Éviter une valeur négative si le texte est plus large que la console
            }

            Console.SetCursorPosition(leftPadding, Console.CursorTop);
            Console.WriteLine(text);
        }

       public static void print_listarray<T>(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }
        #endregion

    }
}