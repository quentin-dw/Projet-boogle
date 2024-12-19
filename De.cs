﻿using System;
using System.IO;

namespace Projet_boogle
{
    internal class De
    {
        #region Attributs
        private char[] faces;
        private char face_visible;
        private static char[] lettres;
        private static int[] point_lettres;
        private static int[] probabilite_lettre;
        #endregion

        #region Propriétés
        public char[] Faces { 
            get { return faces; } 
        }   
        public char Face_visible {  
            get { return face_visible; }
        }
        public static int Point_lettre(int i)
        {
            if (i<0 || i >= point_lettres.Length) return 0;
            return point_lettres[i];
        }
        #endregion

        #region Constructeur
        public De() {
            //Si on a oublié d'initialiser les valeur, alors on initialise en français (normallement pas besoin de le faire)
            if (lettres == null || point_lettres == null || probabilite_lettre == null)
            {
                initialisationValLettres("francais");
            }

            this.faces = new char[6];
            for (int i = 0; i < faces.Length; i++)//pour chaque face, on assigne une lettre aléatoirement en fonction des proba
            {
                faces[i] = Choisir_Lettre_Aleatoire(lettres, probabilite_lettre);
            }

            Lance(Program.random);// on utilise la methode lance pour assigner une face
        }
        #endregion

        #region Méthode
        /// <summary>
        /// Cette méthode initialise les variable static de la classe en fonction de la langue de jeu sélectionné en option
        /// </summary>
        /// <param name="langue"></param> Ce paramètre donne la langue du jeu en cours afin de récupérer le bon fichier
        public static void initialisationValLettres(string langue)
        {
            lire_fichier_lettres(out lettres, out point_lettres, out probabilite_lettre, "Lettres" + langue + ".txt");
        }

        /// <summary>
        /// Méthode pour modifier la face visible en tirant au hasard l'une des 6 faces du dé
        /// </summary>
        /// <param name="r"></param>
        public void Lance(Random r)
        {
            int numero_face = r.Next(0, Faces.Length);
            this.face_visible = faces[numero_face];
        }

        /// <summary>
        /// Méthode pour donner les informations sur le dé, c'est à dire ses faces ainsi que sa face visible
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            string r = "Faces du dé: ";
            for (int i = 0; i< faces.Length; i++) {
                r += faces[i] + ", ";
            }
            r += "\nFace visible: " + face_visible;
            return r;
        }

        /// <summary>
        /// Fonction pour choisir une lettre aleatoirement en fonction du pourcentage qu'a chaque lettre d'apparaittre
        ///
        /// Explication:
        /// Le fonctionnement n'est pas intuitif mais comme il n'y a pas de moyen d'utiliser la fonction random pour des pourcentages d'apparaitre.
        /// On utilise donc un systeme qui va, a chaque iteration, on ajoute le purcentage dans une somme. Si le nombre aléatoire tiré au debut
        /// est passé (inferieur ou egale) alors c'est la bonne lettre, sinon on test la lettre suivante
        /// </summary>
        /// <param name="lettres"></param>
        /// <param name="probabilite_lettre"></param>
        /// <returns></returns>
        public static char Choisir_Lettre_Aleatoire(char[] lettres, int[] probabilite_lettre)
        {
            int tirage = Program.random.Next(1, 101); // Nombre aléatoire entre 1 et 100
            int somme = 0;

            //pour chaque iteration, on ajoute le "pourcentage", et dès que ça correspond au nombre aléatoire alors on prend la lettre qui correspond
            for (int i = 0; i < lettres.Length; i++)
            {
                somme += probabilite_lettre[i];
                if (tirage <= somme)
                {
                    return lettres[i];
                }
            }

            // Retour par défaut au cas ou (normallement ça sert à rien mais bon on sait jamais)
            return lettres[lettres.Length - 1];
        }

        /// <summary>
        /// Fonction pour lire les informations du fichier lettre, puis tri chaque colone et place des informations dans 
        /// trois tableaux differents: les lettres, les points et la probabilité d'apparaitre.
        /// On utilise un try catch pour eviter que ça plante si on ne trouve pas le fichier
        /// </summary>
        /// <param name="lettres"></param>
        /// <param name="points_lettre"></param>
        /// <param name="probabilite_lettre"></param>
        /// <param name="fichier"></param>
        public static void lire_fichier_lettres(out char[] lettres, out int[] points_lettre, out int[] probabilite_lettre, string fichier)
        {
            try
            {
                // Lecture des lignes du fichier
                string[] lignes_fichier = File.ReadAllLines("../../" + fichier);

                // initiqlisation des tableaux
                lettres = new char[lignes_fichier.Length];
                points_lettre = new int[lignes_fichier.Length];
                probabilite_lettre = new int[lignes_fichier.Length];

                for (int i = 0; i < lignes_fichier.Length; i++)
                {
                    // Découper la ligne en trois parties, chaqu'une separée par ;
                    string[] parties = lignes_fichier[i].Split(';');

                    // Stocker chaque valeur dans le tableau correspondant
                    lettres[i] = char.Parse(parties[0]);          // lettres
                    points_lettre[i] = int.Parse(parties[1]);     // points pas lettres
                    probabilite_lettre[i] = int.Parse(parties[2]); // probabilité de chasue lettre
                }
            }

            catch (Exception ex)
            {
                lettres = null;
                points_lettre = null;
                probabilite_lettre = null;

                Console.WriteLine("Erreur lors de la lecture du fichier lettres : " + ex.Message);

            }
        }

        #endregion 
    }


}
