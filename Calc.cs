using System;
using System.Globalization;

namespace Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variables 
            bool continuer = true;
            double nombre1, nombre2;
            char signe;
            string? entreeUtilisateur;

            // Constantes 
            const string OPERATIONS_VALIDES = "+-*/";
            CultureInfo cultureFr = new CultureInfo("fr-FR");
            // Création d'une culture personnalisée qui accepte à la fois le point et la virgule
            NumberFormatInfo formatPersonnalise = new NumberFormatInfo
            {
                NumberDecimalSeparator = ",",
                NumberGroupSeparator = ".",
                CurrencyDecimalSeparator = ",",
                CurrencyGroupSeparator = ".",
                PercentDecimalSeparator = ",",
                PercentGroupSeparator = "."
            };

            // Affichage du message de bienvenue
            AfficherBienvenue();

            do{
                // Affichage de l'invite de commande
                AfficherInvite();
                
                // Récupération et vérification de l'entrée utilisateur
                entreeUtilisateur = RecupererEntreeUtilisateur();
                if (entreeUtilisateur == null) continue;

                // Recherche du signe d'opération
                int indexSigne = RechercherSigneOperation(entreeUtilisateur, OPERATIONS_VALIDES);
                if (indexSigne == -1)
                {
                    Console.WriteLine("Opération non valide. Utilisez +, -, * ou /");
                    continue;
                }

                // Extraction du signe et des nombres
                if (!ExtraireSigneEtNombres(entreeUtilisateur, indexSigne, formatPersonnalise, out signe, out nombre1, out nombre2))
                {
                    continue;
                }

                // Calcul du résultat
                double resultat = CalculerResultat(signe, nombre1, nombre2);
                if (double.IsNaN(resultat)) continue;

                // Affichage du résultat avec la culture française
                // Utilisation de G8 pour ne pas afficher les zéros inutiles
                Console.WriteLine($"Résultat: {nombre1.ToString("G8", cultureFr)} {signe} {nombre2.ToString("G8", cultureFr)} = {resultat.ToString("G8", cultureFr)}");
                
            }while(continuer);
        }

        static void AfficherBienvenue()
        {
            Console.WriteLine("Calculatrice");
            Console.WriteLine("Entrez votre calcul sous la forme: <nombre1><opération><nombre2>");
            Console.WriteLine("Opérations disponibles: +, -, *, /");
            Console.WriteLine("Exemple: 5,3+2,1, -5,3+2,1, 5,3+-2,1, -5,3+-2,1");
        }

        static void AfficherInvite()
        {
            Console.Write("> ");
        }

        static string? RecupererEntreeUtilisateur()
        {
            string? entree = Console.ReadLine()?.Trim();
            while (string.IsNullOrEmpty(entree))
            {
                Console.WriteLine("Veuillez entrer une expression valide (ex: 5,3+2,1 ou -5,3+2,1)");
            }
            return entree;
        }

        static int RechercherSigneOperation(string expression, string operationsValides)
        {
            // On commence à 1 pour éviter de confondre le signe négatif du premier nombre
            for (int i = 1; i < expression.Length; i++)
            {
                if (operationsValides.Contains(expression[i]))
                {
                    // Si on trouve un signe négatif, on vérifie qu'il n'est pas suivi d'un chiffre
                    if (expression[i] == '-')
                    {
                        // Si le caractère précédent est un chiffre ou une virgule, c'est une soustraction
                        if (i > 0 && (char.IsDigit(expression[i - 1]) || expression[i - 1] == ',' || expression[i - 1] == '.'))
                        {
                            return i;
                        }
                        // Sinon, c'est un nombre négatif, on continue la recherche
                        continue;
                    }
                    return i;
                }
            }
            return -1;
        }

        static bool ExtraireSigneEtNombres(string entree, int indexSigne, NumberFormatInfo format, 
            out char signe, out double nombre1, out double nombre2)
        {
            // Initialisation des paramètres out
            signe = ' ';
            nombre1 = 0;
            nombre2 = 0;

            signe = entree[indexSigne];
            string nombre1Str = entree.Substring(0, indexSigne);
            string nombre2Str = entree.Substring(indexSigne + 1);

            // Remplacement des points par des virgules pour la conversion
            nombre1Str = nombre1Str.Replace('.', ',');
            nombre2Str = nombre2Str.Replace('.', ',');

            if (!double.TryParse(nombre1Str, NumberStyles.Any, format, out nombre1) || 
                !double.TryParse(nombre2Str, NumberStyles.Any, format, out nombre2))
            {
                Console.WriteLine("Les nombres doivent être valides (utilisez la virgule ou le point pour les décimaux)");
                return false;
            }

            return true;
        }

        static double CalculerResultat(char signe, double nombre1, double nombre2)
        {
            switch (signe)
            {
                case '+':
                    return Addition(nombre1, nombre2);
                case '-':
                    return Soustraction(nombre1, nombre2);
                case '*':
                    return Multiplication(nombre1, nombre2);
                case '/':
                    if (nombre2 == 0)
                    {
                        Console.WriteLine("Division par zéro impossible");
                        return double.NaN;
                    }
                    return Division(nombre1, nombre2);
                default:
                    return double.NaN;
            }
        }

        static double Addition(double nbr1, double nbr2){
            return nbr1 + nbr2;
        }

        static double Soustraction(double nbr1, double nbr2){
            return nbr1 - nbr2;
        }

        static double Multiplication(double nbr1, double nbr2){
            return nbr1 * nbr2;
        }

        static double Division(double nbr1, double nbr2){
            return nbr1 / nbr2;
        }
    }
}
