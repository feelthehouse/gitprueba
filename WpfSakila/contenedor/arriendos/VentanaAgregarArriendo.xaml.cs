using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
    /// Lógica de interacción para VentanaAgregarArriendo.xaml
    /// </summary>
    public partial class VentanaAgregarArriendo : Window
    {
        public VentanaAgregarArriendo()
        {
            InitializeComponent();
        }

        private SqlConnection generarConexion()
        {
            SqlConnection crearConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["WpfSakila.Properties.Settings.sakilaConnectionString"].ToString());
            return crearConexion;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            WpfSakila.sakilaDataSet sakilaDataSet = ((WpfSakila.sakilaDataSet)(this.FindResource("sakilaDataSet")));
            // Cargar datos en la tabla rental. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.rentalTableAdapter sakilaDataSetrentalTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.rentalTableAdapter();
            sakilaDataSetrentalTableAdapter.Fill(sakilaDataSet.rental);
            System.Windows.Data.CollectionViewSource rentalViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("rentalViewSource")));
            rentalViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla customer. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.customerTableAdapter sakilaDataSetcustomerTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.customerTableAdapter();
            sakilaDataSetcustomerTableAdapter.Fill(sakilaDataSet.customer);
            System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla staff. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.staffTableAdapter sakilaDataSetstaffTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.staffTableAdapter();
            sakilaDataSetstaffTableAdapter.Fill(sakilaDataSet.staff);
            System.Windows.Data.CollectionViewSource staffViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("staffViewSource")));
            staffViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla payment. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.paymentTableAdapter sakilaDataSetpaymentTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.paymentTableAdapter();
            sakilaDataSetpaymentTableAdapter.Fill(sakilaDataSet.payment);
            System.Windows.Data.CollectionViewSource paymentViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("paymentViewSource")));
            paymentViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla inventory. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.inventoryTableAdapter sakilaDataSetinventoryTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.inventoryTableAdapter();
            sakilaDataSetinventoryTableAdapter.Fill(sakilaDataSet.inventory);
            System.Windows.Data.CollectionViewSource inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            inventoryViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla staff_list. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.staff_listTableAdapter sakilaDataSetstaff_listTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.staff_listTableAdapter();
            sakilaDataSetstaff_listTableAdapter.Fill(sakilaDataSet.staff_list);
            System.Windows.Data.CollectionViewSource staff_listViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("staff_listViewSource")));
            staff_listViewSource.View.MoveCurrentToFirst();
        }

        private void btnAgregarArriendo_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conecta = generarConexion();
            SqlCommand insertarValores = new SqlCommand("INSERT INTO film(rental_date,inventory_id,costumer_id,return_date,staff_id) VALUES(@p_rental_date,@p_inventory_id,@p_costumer_id,@p_return_date,@p_staff_id)", conecta);

            insertarValores.Parameters.AddWithValue("@p_rental_date", rental_dateDatePicker);
            insertarValores.Parameters.AddWithValue("@p_inventory_id", inventoryComboBox.SelectedValue);
            insertarValores.Parameters.AddWithValue("@p_costumer_id", customerComboBox.SelectedValue);
            insertarValores.Parameters.AddWithValue("@p_return_date", return_dateDatePicker);
            insertarValores.Parameters.AddWithValue("@p_staff_id", staffComboBox.SelectedValue);

            MessageBox.Show("Agregada Correctamente");
        }
    }
}
