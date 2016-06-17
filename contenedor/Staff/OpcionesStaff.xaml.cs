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

namespace WpfSakila.contenedor.Staff
{
    /// <summary>
    /// Lógica de interacción para OpcionesStaff.xaml
    /// </summary>
    public partial class OpcionesStaff : Window
    {
        public OpcionesStaff()
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
            // Cargar datos en la tabla staff. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.staffTableAdapter sakilaDataSetstaffTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.staffTableAdapter();
            sakilaDataSetstaffTableAdapter.Fill(sakilaDataSet.staff);
            System.Windows.Data.CollectionViewSource staffViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("staffViewSource")));
            staffViewSource.View.MoveCurrentToFirst();
         // staffDataGrid.SelectedValue = "staff_id";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AgregarStaff1 ventanaAgregarStaff = new AgregarStaff1();
            ventanaAgregarStaff.Show();
        }

        private void btnEliminarStaff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection conecta = generarConexion();
                int idStaff = Convert.ToInt32(staffDataGrid.SelectedValue);
                MessageBox.Show("El id seleccionado es : " + idStaff);
  
                SqlCommand elimina = new SqlCommand("delete from staff where staff_id=@p_staff_id", conecta);
                elimina.Parameters.AddWithValue("@p_staff_id", idStaff);

                conecta.Open();

                elimina.ExecuteNonQuery();
                MessageBox.Show("El id" + idStaff + "ha sido eliminado correctamente , cierre la ventana para ver los cambios. ");
                conecta.Close();
            }

            catch (Exception ex)
            
            {
                MessageBox.Show("Error :" + ex.Message);
            }
  }
            }
}
    