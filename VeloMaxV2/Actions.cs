using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace VeloMaxV2
{
    internal class Actions
    {

        public static void mine(Interface inter)
        {
            Console.WriteLine("Autojointure : trouver tous les clients ayant le même nom de famille");
            (DataBase.execute_query("SELECT DISTINCT p1.idPersonne, p1.nom, p1.prenom, p1.telephone, p1.mail, p1.idAdresse FROM Personne p1 JOIN Personne p2 ON p1.nom = p2.nom AND p1.idPersonne <> p2.idPersonne WHERE p1.prenom <> p2.prenom;")).Display();
            next();
            Console.WriteLine("Union : prix des velos et des pièces ");
            DataBase.execute_query("SELECT prix from piece Union SELECT prix from modele;").Display();
            next();
            Console.WriteLine("Syncronized : Recherche et modification du nombre de pièces de vend_modele idModele = 101");
            DataBase.execute_query("SELECT m.idModele, m.nom, m.prix, v.quantite FROM Modele m JOIN vend_modele v ON m.idModele = v.idModele WHERE v.idMagasin = 1;").Display();
            DataBase.execute_query("START TRANSACTION;");
            DataBase.execute_query("SELECT m.idModele, m.nom, m.prix, v.quantite FROM Modele m JOIN vend_modele v ON m.idModele = v.idModele WHERE v.idMagasin = 1;");
            DataBase.execute_query("UPDATE vend_modele SET quantite = quantite - 5 WHERE idMagasin = 1 AND idModele = 101;");
            DataBase.execute_query("COMMIT;");
            DataBase.execute_query("UPDATE vend_modele SET quantite = 105 WHERE `idModele` = 101;"); // juste pour la demo 
            next();
            inter.home.Activate();

        }

        public static void sql_interface(Interface inter)
        {
            Console.Clear();
            Console.WriteLine("Entrez votre requête SQL");
            string query = Console.ReadLine();
            QueryResult qr = DataBase.execute_query(query);
            qr.Display();
            Console.WriteLine("Appuyez sur une touche pour continuer");
            Console.ReadKey();
            inter.home.Activate();
        }
        public static void statistiques(Interface inter)
        {
            Console.WriteLine("Client par programme fidelio : ");
            for(int i = 1;i<4;i++)
            {
                DataBase.execute_query($"SELECT Personne.*,Client.idClient,Fidelio.info,souscription_fidelio.adhesion,Fidelio.durre AS durée_en_jours FROM Personne NATURAL JOIN Client JOIN souscription_fidelio ON souscription_fidelio.idClient = Client.idClient JOIN Fidelio ON Fidelio.idFidelio = souscription_fidelio.idFidelio WHERE Fidelio.idFidelio = {i}").Display();
            }
            next();
            Console.WriteLine("Meilleur client entreprise en terme de ventes : ");
            (DataBase.execute_query("SELECT entreprise.*, SUM(commande_velo.quantite * modele.prix)+SUM(commande_piece.quantite * piece.prix) AS chiffre_affaire FROM entreprise JOIN commande_entreprise ON entreprise.identreprise = commande_entreprise.identreprise JOIN commande ON commande_entreprise.idCommande = commande.idCommande JOIN commande_velo ON commande.idCommande = commande_velo.idCommande JOIN modele ON commande_velo.idModele = modele.idModele JOIN commande_piece ON commande.idCommande = commande_piece.idCommande JOIN piece ON commande_piece.idPiece = piece.idPiece GROUP BY entreprise.identreprise ORDER BY chiffre_affaire DESC LIMIT 1;")).Display();
            Console.WriteLine("Meilleur cliententreprise en terme de nombre de pièces vendues ");
            (DataBase.execute_query("SELECT entreprise.*, SUM(commande_velo.quantite)+SUM(commande_piece.quantite) AS total_piece FROM entreprise JOIN commande_entreprise ON entreprise.identreprise = commande_entreprise.identreprise JOIN commande ON commande_entreprise.idCommande = commande.idCommande JOIN commande_velo ON commande.idCommande = commande_velo.idCommande JOIN modele ON commande_velo.idModele = modele.idModele JOIN commande_piece ON commande.idCommande = commande_piece.idCommande JOIN piece ON commande_piece.idPiece = piece.idPiece GROUP BY entreprise.identreprise ORDER BY total_piece DESC LIMIT 1;")).Display();
            next();
            Console.WriteLine("Bonus par employé");
            (DataBase.execute_query("SELECT vendeur.*, chiffre_affaire*0.1 + magasin.moyenne_satisfaction*10 AS bonus FROM vendeur NATURAL JOIN magasin;")).Display();
            next();
            inter.home.Activate();
        }

        public static void inferior_2()
        {
            Console.WriteLine("Liste des produits dont le stoque est inférieur à deux");
            QueryResult qr = DataBase.execute_query("SELECT piece.idPiece, piece.info FROM piece NATURAL JOIN vend_piece GROUP BY piece.idPiece Having SUM(vend_piece.quantite)<=2;");
            qr.Display();
            qr = DataBase.execute_query("SELECT modele.idModele, modele.nom FROM modele NATURAL JOIN vend_modele GROUP BY modele.idModele Having SUM(vend_modele.quantite)<=2;");
            qr.Display();
        }

        public static void fournisseur()
        {
            Console.WriteLine("Nombre de pièce fournies par fournisseurs");
            (DataBase.execute_query("SELECT COUNT(*) as nbPiece, Fournisseur.nom, fournisseur.idFournisseur FROM fournit_piece JOIN Fournisseur ON fournit_piece.idFournisseur = Fournisseur.idFournisseur GROUP BY idFournisseur;")).Display();
            Console.WriteLine("Nombre de modèles fournies par fournisseurs");
            (DataBase.execute_query("SELECT COUNT(*) as nbModele, Fournisseur.nom,fournisseur.idFournisseur FROM fournit_modele JOIN Fournisseur ON fournit_modele.idFournisseur = Fournisseur.idFournisseur GROUP BY idFournisseur;")).Display();
        }

        public static void display_info()
        {
            Table client = new("Client");
            client.qr.add_column("dépenses_totales",0.0);
            client.qr.add_column("nom","");
            client.qr.add_column("prenom","");
            for (int i =0; i< client.qr.data["idClient"].Count; i++)
            {
                client.qr.data["nom"][i] = (DataBase.execute_query($"SELECT nom FROM Personne WHERE idPersonne = {client.qr.data["idPersonne"][i]};")).data["nom"][0];
                client.qr.data["prenom"][i] = (DataBase.execute_query($"SELECT prenom FROM Personne WHERE idPersonne = {client.qr.data["idPersonne"][i]};")).data["prenom"][0];
                string query = $"SELECT Client.idClient AS personne, SUM(montant) AS montant FROM commande_client JOIN Client ON client.idClient = commande_client.idClient JOIN Commande on Commande.idCommande = commande_client.idCommande WHERE Client.idClient = {client.qr.data["idClient"][i]} GROUP BY(Client.idClient);";
                QueryResult qr = DataBase.execute_query(query);
                if(qr.succes && qr.data.Count > 0)
                {
                    for (int j = 0; j < qr.data["montant"].Count; j++)
                    {
                        client.qr.data["dépenses_totales"][Convert.ToInt32(qr.data["personne"][j]) - 1] = (double)client.qr.data["dépenses_totales"][Convert.ToInt32(qr.data["personne"][j]) - 1] + (double)qr.data["montant"][j];
                    }
                }
                
                
            }

            client.display();
        }
        public static void next()
        {
            Console.WriteLine("Appuyez sur une touche pour continuer");
            Console.ReadKey();
            Console.Clear();
        }
        public static void demo(Interface inter)
        {
            
            Console.Clear();
            Console.WriteLine("Bienvenue dans la démo de VeloMax");
            Console.WriteLine("Pour l'ajout la suprresion et la modification veuillez consulter l'interface");
            next();
            Console.WriteLine($"Le nombre de client est de {DataBase.execute_query("SELECT COUNT(*) AS c FROM CLIENT;").data["c"][0]}");
            next();
            display_info();
            next();
            inferior_2();
            next();
            fournisseur();
            next();
            statistiques(inter);
            next();
            inter.home.Activate();
        }
    }
}
