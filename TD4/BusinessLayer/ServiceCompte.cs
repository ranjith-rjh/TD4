using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace BusinessLayer
{
    public class ServiceCompte
    {
        public ServiceCompte()
        {

        }

        DataAccess maDataAccess = new DataAccess();
        
        /// <summary>
        /// Récupère la liste de tous les comptes (id et solde) de la base de données
        /// </summary>
        /// <returns>Liste des comptes bancaires</returns>
        public List<Compte>? GetAllComptes()
        {
            List<Compte> lesComptes = new List<Compte>();

            try
            {
                DataTable maDataTable = maDataAccess.GetData("SELECT * " +
                                                            "FROM vComptes");
                int idCompte = 0;
                double solde = 0;

                foreach (DataRow uneLigne in maDataTable.Rows)
                {
                    foreach (DataColumn uneColonne in maDataTable.Columns)
                    {
                        if (uneColonne.ColumnName == "idcompte")
                            idCompte = int.Parse(uneLigne[uneColonne].ToString());

                        else if (uneColonne.ColumnName == "solde")
                            solde = double.Parse(uneLigne[uneColonne].ToString());
                    }

                    lesComptes.Add(new Compte(idCompte, solde));
                }

                return lesComptes;
            }

            catch(Exception e)
            {
                MessageBox.Show(e.ToString(), "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Met à jour le solde du compte passé en paramètre en fonction du montant passé en paramètre.Si montant<0 => débit, sinon crédit.
        /// </summary>
        /// <param name="compte">Compte à débiter ou créditer</param>
        /// <param name="montant">Montant du débit (si <0) ou crédit (si >0)</param>
        /// <returns>Résultat de la mise à jour (update) du solde du compte : true => réussi,false => échec</returns>
        public bool SetDebitCredit(Compte compte, double montant)
        {
            double res = compte.Solde;
            res += montant;

            // Tester 
            if (res <= 0)
            {
                MessageBox.Show("Le solde est insuffisant...", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            else
            {
                int succes = maDataAccess.SetData("UPDATE Compte\r\n" +
                                                  "SET solde = " + res + "\r\n" +
                                                  "WHERE idcompte = " + compte.IdCompte + "\r\n");
                if (succes == 0)
                    return false;
                else
                    return true;
            }

        }

        /// <summary>
        /// Réalise un virement du compte en paramètre n°1 vers le compte en paramètre n°2 d'un montant en paramètre n°3
        /// </summary>
        /// <param name="compteDebit">Compte à débiter</param>
        /// <param name="compteCredit">Compte à créditer</param>
        /// <param name="montant">Montant du virement</param>
        /// <returns>Résultat de la mise à jour (update) des soldes des 2 comptes : true => réussi(nombre de lignes modifiées par SetData = 2), false => échec</returns>
        public bool Virement(Compte compteDebit, Compte compteCredit, double montant)
        {
            if(compteDebit.Solde - montant <= 0)
            {
                MessageBox.Show("Compte " + compteDebit.IdCompte + " : Solde insuffisant", "Solde insuffisant", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            else
            {
                int succes = maDataAccess.SetData("Begin;\r\n" +
                                                  "UPDATE compte set solde = solde-" + montant + "where idcompte=" + compteDebit.IdCompte + ";\r\n" +
                                                  "UPDATE compte set solde = solde+" + montant + "where idcompte=" + compteCredit.IdCompte + ";\r\n" +
                                                  "COMMIT\r\n");

                if (succes == 0)
                    return false;
                else
                    return true;
            }

            
        }

    }
}
