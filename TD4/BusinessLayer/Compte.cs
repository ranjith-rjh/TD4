using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Compte
    {
        // Propriétés
        public int IdCompte { get; set; }
        public double Solde { get; set; }

        // Constructeurs
        public Compte()
        {

        }

        public Compte(int idCompte, double solde)
        {
            this.IdCompte = idCompte;
            this.Solde = solde;
        }


    }
}
