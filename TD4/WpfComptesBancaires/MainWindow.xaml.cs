using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfComptesBancaires
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> TypeOperation { get; set; }
        public ObservableCollection<Compte> LesComptes { get; set; }
        

        ServiceCompte monServiceCompte = new ServiceCompte();

        public MainWindow()
        {
            InitializeComponent();

            // Type Operation
            TypeOperation = new ObservableCollection<string>();
            TypeOperation.Add("Retrait");
            TypeOperation.Add("Depot");

            // Comptes
            LesComptes = new ObservableCollection<Compte>(monServiceCompte.GetAllComptes());

            this.DataContext = this;
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            string operation = this.cbTypeOperation.Text;
            int idCompte = int.Parse(this.cbCompte.Text);
            double montant = double.Parse(this.tboxMontant.Text);
            double soldeFinal;

            List<Compte> lesComptes = LesComptes.ToList();
            Compte leCompte = lesComptes.Find(c => c.IdCompte == idCompte);

            bool success;

            if(operation == "Retrait")
            {
                success = monServiceCompte.SetDebitCredit(leCompte, -montant);
                soldeFinal = leCompte.Solde - montant;
            }
            else
            {
                success = monServiceCompte.SetDebitCredit(leCompte, montant);
                soldeFinal = leCompte.Solde + montant;
            }

            if (success)
                MessageBox.Show(operation + " success", "Compte Bancaire", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show(operation + " echec", "Compte Bancaire", MessageBoxButton.OK, MessageBoxImage.Error);

            // MAJ 
            LesComptes = new ObservableCollection<Compte>(monServiceCompte.GetAllComptes());
            MessageBox.Show("Solde restant : " + soldeFinal, "Solde restant", MessageBoxButton.OK, MessageBoxImage.Information);


        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.cbTypeOperation.SelectedValue = null;
            this.cbCompte.SelectedValue = null;
            this.tboxMontant.Text = null;
        }
    }
}
