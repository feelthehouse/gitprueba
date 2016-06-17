using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfSakila.contenedor.arriendos
{
    /// <summary>
    /// Lógica de interacción para OpcionesArriendo.xaml
    /// </summary>
    public partial class OpcionesArriendo : Window
    {
        public OpcionesArriendo()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            WpfSakila.sakilaDataSet sakilaDataSet = ((WpfSakila.sakilaDataSet)(this.FindResource("sakilaDataSet")));
            // Cargar datos en la tabla rental. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.rentalTableAdapter sakilaDataSetrentalTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.rentalTableAdapter();
            sakilaDataSetrentalTableAdapter.Fill(sakilaDataSet.rental);
            System.Windows.Data.CollectionViewSource rentalViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("rentalViewSource")));
            rentalViewSource.View.MoveCurrentToFirst();
        }

        private void btnAgregarArriendo_Click(object sender, RoutedEventArgs e)
        {
            VentanaAgregarArriendo abrirVentana = new VentanaAgregarArriendo();
            abrirVentana.Show();
        }
    }
}
