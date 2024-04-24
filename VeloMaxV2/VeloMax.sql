-- Active: 1712592380272@@127.0.0.1@3306@velomax
CREATE DATABASE IF NOT EXISTS VeloMax;
USE VeloMax;

CREATE TABLE IF NOT EXISTS Adresse
(
    idAdresse INT AUTO_INCREMENT PRIMARY KEY,
    numero_rue INT,
    rue VARCHAR(50),
    ville VARCHAR(50),
    code_postal INT,
    province VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS Personne
(
    idPersonne INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(30),
    prenom VARCHAR(30),
    telephone INT NULL,
    mail VARCHAR(30) NULL,
    idAdresse INT NULL,
    FOREIGN KEY(idAdresse) REFERENCES Adresse(idAdresse) ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS Salarie
(
    idSalarie INT AUTO_INCREMENT PRIMARY KEY,
    idPersonne INT NOT NULL,
    FOREIGN KEY(idPersonne) REFERENCES Personne(idPersonne) ON DELETE CASCADE,
    date_arrivee DATE,
    salaire FLOAT,
    prime FLOAT
);

CREATE TABLE IF NOT EXISTS Magasin
(
    idMagasin INT AUTO_INCREMENT PRIMARY KEY,
    date_creation DATE,
    chiffre_affaire FLOAT,
    moyenne_satisfaction FLOAT,
    idAdresse INT,
    FOREIGN KEY(idAdresse) REFERENCES Adresse(idAdresse)
);
CREATE TABLE IF NOT EXISTS Piece
(
    idPiece INT AUTO_INCREMENT PRIMARY  KEY,
    info VARCHAR(255), 
    prix FLOAT,
    date_introduction DATE,
    date_discontinuation DATE
);

CREATE TABLE IF NOT EXISTS Modele
(
    idModele INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(30),
    grandeur FLOAT,
    prix FLOAT,
    date_introduction DATE,
    date_discontinuation DATE,
    ligne_produit VARCHAR(14) CONSTRAINT check_ligne CHECK (ligne_produit IN("VTT","V�lo de course","Classique","BMX"))
);

CREATE TABLE IF NOT EXISTS Fournisseur
(
    idFournisseur INT PRIMARY KEY,
    nom VARCHAR(30),
    telephone INT,
    mail VARCHAR(30),
    reactivite INT CONSTRAINT check_reactivite CHECK (reactivite BETWEEN 1 AND 4), 
    idAdresse INT,
    FOREIGN KEY(idAdresse) REFERENCES Adresse(idAdresse)
);

CREATE TABLE IF NOT EXISTS Commande
(
    idCommande INT AUTO_INCREMENT PRIMARY KEY,
    date_commande DATE,
    date_livraison DATE,
    montant FLOAT,
    idAdresse INT NOT NULL,
    FOREIGN KEY(idAdresse) REFERENCES Adresse(idAdresse)
);

CREATE TABLE IF NOT EXISTS Client
(
    idClient INT AUTO_INCREMENT PRIMARY KEY,
    idPersonne INT NOT NULL,
    FOREIGN KEY(idPersonne) REFERENCES Personne(idPersonne) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Fidelio
(
    idFidelio INT AUTO_INCREMENT PRIMARY KEY,
    info VARCHAR(255),
    cout FLOAT,
    durre INT,
    rabais FLOAT
);

CREATE TABLE IF NOT EXISTS Entreprise
(
    idEntreprise INT AUTO_INCREMENT PRIMARY KEY,
    nom_compagnie VARCHAR(30),
    telephone INT,
    mail VARCHAR(30),
    nom VARCHAR(30),
    remise FLOAT,
    idAdresse INT,
    FOREIGN KEY(idAdresse) REFERENCES Adresse(idAdresse)
);

CREATE TABLE IF NOT EXISTS manageur(
    idSalarie INT,
    idMagasin INT,
    FOREIGN KEY(idSalarie) REFERENCES Salarie(idSalarie) ON DELETE CASCADE,
    FOREIGN KEY(idMagasin) REFERENCES Magasin(idMagasin) ON DELETE CASCADE,
    PRIMARY KEY(idSalarie, idMagasin)
);

CREATE TABLE IF NOT EXISTS vendeur(
    idSalarie INT,
    idMagasin INT,
    chiffre_affaire FLOAT,
    FOREIGN KEY(idSalarie) REFERENCES Salarie(idSalarie) ON DELETE CASCADE,
    FOREIGN KEY(idMagasin) REFERENCES Magasin(idMagasin) ON DELETE CASCADE,
    PRIMARY KEY(idSalarie, idMagasin)        
);
CREATE TABLE IF NOT EXISTS vend_piece(
    idPiece INT,
    idMagasin INT,
    quantite INT,
    FOREIGN KEY(idPiece) REFERENCES Piece(idPiece) ON DELETE CASCADE,
    FOREIGN KEY(idMagasin) REFERENCES Magasin(idMagasin) ON DELETE CASCADE,
    PRIMARY KEY(idPiece, idMagasin)
);

CREATE TABLE IF NOT EXISTS vend_modele(
    idModele INT,
    idMagasin INT,
    quantite INT,
    FOREIGN KEY(idModele) REFERENCES Modele(idModele) ON DELETE CASCADE,
    FOREIGN KEY(idMagasin) REFERENCES Magasin(idMagasin) ON DELETE CASCADE,
    PRIMARY KEY(idModele, idMagasin)
);

CREATE TABLE IF NOT EXISTS fournit_piece(
    idPiece INT,
    idFournisseur INT,
    prix FLOAT,
    delai INT,
    FOREIGN KEY(idPiece) REFERENCES Piece(idPiece) ON DELETE CASCADE,
    FOREIGN KEY(idFournisseur) REFERENCES Fournisseur(idFournisseur) ON DELETE CASCADE,
    PRIMARY KEY(idPiece, idFournisseur)
);

CREATE TABLE IF NOT EXISTS fournit_modele(
    idModele INT,
    idFournisseur INT,
    prix FLOAT,
    delai INT,
    FOREIGN KEY(idModele) REFERENCES Modele(idModele) ON DELETE CASCADE,
    FOREIGN KEY(idFournisseur) REFERENCES Fournisseur(idFournisseur) ON DELETE CASCADE,
    PRIMARY KEY(idModele, idFournisseur)
);

CREATE TABLE IF NOT EXISTS commande_velo(
    idCommande INT,
    idModele INT,
    quantite INT,
    FOREIGN KEY(idCommande) REFERENCES Commande(idCommande) ON DELETE CASCADE,
    FOREIGN KEY(idModele) REFERENCES Modele(idModele) ON DELETE CASCADE,
    PRIMARY KEY(idCommande, idModele)
);
CREATE TABLE IF NOT EXISTS commande_piece(
    idCommande INT,
    idPiece INT,
    quantite INT,
    FOREIGN KEY(idCommande) REFERENCES Commande(idCommande) ON DELETE CASCADE,
    FOREIGN KEY(idPiece) REFERENCES Piece(idPiece) ON DELETE CASCADE,
    PRIMARY KEY(idCommande, idPiece)
);

CREATE TABLE IF NOT EXISTS commande_entreprise(
    idCommande INT,
    idEntreprise INT,
    FOREIGN KEY(idCommande) REFERENCES Commande(idCommande) ON DELETE CASCADE,
    FOREIGN KEY(idEntreprise) REFERENCES Entreprise(idEntreprise) ON DELETE CASCADE,
    PRIMARY KEY(idCommande, idEntreprise)
);

CREATE TABLE IF NOT EXISTS commande_client(
    idCommande INT,
    idClient INT,
    FOREIGN KEY(idCommande) REFERENCES Commande(idCommande) ON DELETE CASCADE,
    FOREIGN KEY(idClient) REFERENCES Client(idClient) ON DELETE CASCADE,
    PRIMARY KEY(idCommande, idClient)
);
CREATE TABLE IF NOT EXISTS souscription_fidelio(
    idClient INT,
    idFidelio INT,
    FOREIGN KEY(idClient) REFERENCES Client(idClient) ON DELETE CASCADE,
    FOREIGN KEY(idFidelio) REFERENCES Fidelio(idFidelio) ON DELETE CASCADE,
    regle BOOLEAN,
    adhesion DATE,
    PRIMARY KEY(idClient, idFidelio)
);

--la génération ne fonctionne pas sur votre machine car les is ne sont pas les mêmes en raison de AUTO_INCREMENT
INSERT INTO Adresse (numero_rue, rue, ville, code_postal, province)
VALUES 
(123, 'Rue de la Paix', 'Paris', 75001, 'Île-de-France'),
(456, 'Main Street', 'New York', 10001, 'New York'),
(789, 'Rue Sainte-Catherine', 'Montréal', 8000, 'Québec'),
(1011, 'Calle de Alcalá', 'Madrid', 28009, 'Madrid'),
(1315, 'Yeouido-dong', 'Séoul', 07336, 'Séoul'),
(1718, 'Nanjing Road', 'Shanghai', 200001, 'Shanghai'),
(1920, 'Piazza di Spagna', 'Rome', 00187, 'Rome'),
(2122, 'Karl Johans gate', 'Oslo', 0154, 'Oslo'),
(2122, 'Mont saint Michel', 'France', 0154, 'France'),
(2324, 'Orchard Road', 'Singapour', 238879, 'Singapour');

INSERT INTO Personne (nom, prenom, telephone, mail, idAdresse)
VALUES ('Dupont', 'Marie', 123456789, 'marie.dupont@example.com', 1),
('Smith', 'John', 987654321, 'john.smith@example.com', 2),
('Tremblay', 'Jean', 0130587862, 'jean.tremblay@example.com', 3),
('Garcia', 'Sofia', 0130569889,'sofia.garcia@example.com', 4),
('Kim', 'Minho', 0130569889, 'minho.kim@example.com', 5),
('Chen', 'Wei', 0130569845, 'wei.chen@example.com', 6),
('Johnson', 'Emma', 0630569862, 'emma.johnson@example.com', 7),
('Dubois', 'Pierre', 0130585862, 'pierre.dubois@example.com', 8),
('Martinez', 'Elena', 0880569862,'elena.martinez@example.com', 9);

INSERT INTO Salarie (idPersonne, date_arrivee, salaire, prime)
VALUES(109, '2021-01-01', 2000, 100),
(110, '2022-02-01', 2060, 400),
(111, '2021-08-04', 2000, 100),
(112, '2020-07-15', 1900, 600),
(113, '2021-01-01', 2000, 90),
(113, '2021-01-01', 4000, 600);

INSERT INTO Magasin (date_creation, chiffre_affaire, moyenne_satisfaction, idAdresse)
VALUES('2021-01-01', 10000, 4, 1),
('2022-02-01', 20000, 3, 2),
('2021-08-04', 15000, 2, 3),
('2020-07-15', 12000, 1, 4);
INSERT INTO vendeur (idSalarie, chiffre_affaire,idMagasin)
VALUES(11, 10000,1),
(12, 20000,2),
(13, 15000,3),
(14, 12000,4);
SELECT * FROM salarie;
INSERT INTO manageur (idSalarie, idMagasin) VALUES(15, 1),(16,2);

INSERT INTO client (idPersonne)
VALUES(113),
(114),
(115),
(116);

INSERT INTO Piece (info, prix, date_introduction, date_discontinuation)
VALUES 
('Roue de vélo de route', 59.99, '2023-05-15', NULL),
('Pneu de VTT', 29.99, '2023-04-20', NULL),
('Chaîne de vélo', 12.99, '2023-06-10', NULL),
('Selle de vélo de course', 89.99, '2023-07-01', NULL),
('Freins à disque hydrauliques', 149.99, '2023-08-05', NULL);

INSERT INTO Commande (date_commande, date_livraison, montant, idAdresse)
VALUES 
('2024-04-25', '2024-05-02', 300.50, 1),
('2024-04-26', '2024-05-03', 420.75, 2),
('2024-04-27', '2024-05-04', 250.00, 3),
('2024-04-28', '2024-05-05', 800.00, 4),
('2024-04-29', '2024-05-06', 1200.25, 5),
('2024-04-30', '2024-05-07', 600.75, 6),
('2024-05-01', '2024-05-08', 950.00, 7),
('2024-05-02', '2024-05-09', 700.50, 8),
('2024-05-03', '2024-05-10', 1100.25, 9);

INSERT INTO commande_piece (idCommande, idPiece, quantite)
VALUES 
(1, 1, 2),
(1, 2, 4),
(2, 3, 1),
(2, 4, 3),
(3, 5, 2);

INSERT INTO commande_velo (idCommande, idModele, quantite)
VALUES 
(1, 105, 1),
(1, 106, 2),
(2, 107, 3),
(2, 108, 1),
(3, 109, 2);

INSERT INTO commande_client (idCommande, idClient)
VALUES 
(1, 1),
(1, 2),
(2, 3),
(3, 4),
(3, 5);

INSERT INTO Entreprise (nom_compagnie, telephone, mail, nom, remise, idAdresse)
VALUES 
('Tech Solutions', 0130826569, 'info@techsolutions.com', 'John Doe', 0.05, 1),
('Global Imports', 0130826569, 'info@globalimports.com', 'Jane Smith', 0.1, 2),
('Innovative Ventures', 0130826569, 'info@innovativeventures.com', 'Alex Johnson', 0.08, 3),
('Smart Solutions', 0130826569, 'info@smartsolutions.com', 'Emma Davis', 0.12, 4),
('Sunrise Enterprises', 0130826569, 'info@sunriseenterprises.com', 'Michael Wilson', 0.15, 5);

-- Commandes d'entreprises
INSERT INTO commande_entreprise (idCommande, idEntreprise)
VALUES 
(1, 6),
(2, 7),
(3, 8),
(4, 9),
(5, 10);

INSERT INTO Fournisseur (idFournisseur, nom, telephone, mail, reactivite, idAdresse)
VALUES 
(1, 'Bike Parts Supplier', 0130826569, 'info@bikepartssupplier.com', 3, 1),
(2, 'Cycling Gear Inc.', 0130826569, 'info@cyclinggearinc.com', 4, 2),
(3, 'RideTech Corporation', 0130826569, 'info@ridetechcorp.com', 2, 3),
(4, 'Cycle Accessories Ltd.', 0130826569, 'info@cycleaccessories.com', 3, 4),
(5, 'Speedy Bike Components', 0130826569, 'info@speedybikecomponents.com', 4, 5);

INSERT INTO Fidelio (info, cout, durre, rabais)
VALUES 
('Fidélio', 15.00, 1, 0.05),
('Fidélio Or', 25.00, 2, 0.08),
('Fidélio Platine', 60.00, 2, 0.10),
('Fidélio Max', 100.00, 3, 0.12);

INSERT INTO fournit_piece (idPiece, idFournisseur, prix, delai)
VALUES 
(1, 1, 50.00, 7),
(2, 2, 25.00, 5),
(3, 3, 10.00, 3),
(4, 4, 80.00, 10),
(5, 5, 120.00, 14);

INSERT INTO fournit_modele (idModele, idFournisseur, prix, delai)
VALUES 
(101, 1, 200.00, 14),
(102, 2, 150.00, 10),
(103, 3, 120.00, 7),
(104, 4, 250.00, 21),
(105, 5, 300.00, 28);
INSERT INTO souscription_fidelio (idClient, idFidelio, regle, adhesion)
VALUES 
(1, 9, 1, '2023-01-15'),
(2, 10, 1, '2023-02-20'),
(3, 11, 1, '2023-03-10'),
(4, 12, 0, '2023-04-05'),
(5, 9, 1, '2023-05-12'),
(6, 10, 1, '2023-06-30'),
(7, 11, 1, '2023-07-25'),
(8, 12, 0, '2023-08-15');

Select * From magasin;

INSERT INTO vend_modele (idModele, idMagasin, quantite)
VALUES 
(101, 1, 10),
(102, 2, 15),
(103, 3, 20),
(104, 4, 12),
(105, 4, 8),
(106, 1, 5),
(107, 2, 10),
(108, 3, 15),
(109, 4, 20),
(110, 4, 12),
(111, 1, 8),
(112, 2, 5);

INSERT INTO vend_piece (idPiece, idMagasin, quantite)
VALUES 
(1, 1, 2),
(2, 2, 30),
(3, 3, 25),
(4, 4, 15),
(5, 1, 2);

CREATE USER 'bozo'@'localhost' IDENTIFIED BY 'bozo';
GRANT SELECT ON VeloMax.* TO 'bozo'@'localhost';
FLUSH PRIVILEGES;
