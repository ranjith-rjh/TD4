using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using BusinessLayer;

namespace WpfVirement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public ObservableCollection<Compte> LesComptes { get; set; }

        ServiceCompte monServiceCompte = new ServiceCompte();

        public MainWindow()
        {
            InitializeComponent();

            // Comptes
            LesComptes = new ObservableCollection<Compte>(monServiceCompte.GetAllComptes());

            this.DataContext = this;
        }
        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            int idCompteDebiter = int.Parse(this.cbCompteDebiter.Text);
            int idCompteCrediter = int.Parse(this.cbCompteCrediter.Text);
            double montant = double.Parse(this.tboxMontant.Text);

            List<Compte> lesComptes = LesComptes.ToList();
            Compte leCompteDebiter = lesComptes.Find(c => c.IdCompte == idCompteDebiter);
            Compte leCompteCrediter = lesComptes.Find(c => c.IdCompte == idCompteCrediter);

            bool success = monServiceCompte.Virement(leCompteDebiter, leCompteCrediter, montant);

            if (success)
                MessageBox.Show("Virement success", "Virement", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Virement echec", "Virement", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.cbCompteCrediter.SelectedValue = null;
            this.cbCompteDebiter.SelectedValue = null;
            this.tboxMontant.Text = null;
        }
    }
}
