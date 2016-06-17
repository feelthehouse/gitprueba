using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
    /// Lógica de interacción para AgregarStaff1.xaml
    /// </summary>
    public partial class AgregarStaff1 : Window
    {
        public AgregarStaff1()
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
            // Cargar datos en la tabla address. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.addressTableAdapter sakilaDataSetaddressTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.addressTableAdapter();
            sakilaDataSetaddressTableAdapter.Fill(sakilaDataSet.address);
            System.Windows.Data.CollectionViewSource addressViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("addressViewSource")));
            addressViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla store. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.storeTableAdapter sakilaDataSetstoreTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.storeTableAdapter();
            sakilaDataSetstoreTableAdapter.Fill(sakilaDataSet.store);
            System.Windows.Data.CollectionViewSource storeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("storeViewSource")));
            storeViewSource.View.MoveCurrentToFirst();

        }

        private void btnGuardarStaff_Click(object sender, RoutedEventArgs e)
        
        {
            SqlConnection conecta = generarConexion();
            SqlCommand insertarStaff = new SqlCommand("insert into staff(first_name,last_name,address_id,email,store_id,username,password) Values (@p_first_name,@p_last_name,@p_address_id,@p_email,@p_store_id,@p_username,@p_password)", conecta);

            string credencialUsuario = nombreUsuarioStaff.Text;
            string credencialPassword = passwordStaff.Password.ToString();



            insertarStaff.Parameters.AddWithValue("@p_first_name", txtNombreStaff.Text);
            insertarStaff.Parameters.AddWithValue("@p_last_name", txtApellidoStaff.Text);
            insertarStaff.Parameters.AddWithValue("@p_address_id", addressComboBox.SelectedValue);
            insertarStaff.Parameters.AddWithValue("@p_email", txtEmailStaff.Text);
            insertarStaff.Parameters.AddWithValue("@p_store_id",store_idComboBox.SelectedValue);

            insertarStaff.Parameters.AddWithValue("@p_username", nombreUsuarioStaff.Text);
            insertarStaff.Parameters.AddWithValue("@p_password", encriptarSha1(credencialPassword));

            conecta.Open();
            try
            {
                
                insertarStaff.ExecuteNonQuery();

                MessageBox.Show("Staff agregado con exito, cierre la ventana para ver los cambios.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.StackTrace);

            }
            conecta.Close();


            /* Verifica que los identificadores no estén vacíos. Si es vacío muestra un mensaje y limpia las cajas de texto.*/
            if (String.IsNullOrEmpty(credencialUsuario) || String.IsNullOrEmpty(credencialPassword))
            {
                MessageBox.Show("No pueden ser valores nulos");
                nombreUsuarioStaff.Clear();
                passwordStaff.Clear();

            }

            /* Si los datos no vienen vacíos*/
            else
            {
                UTF8Encoding codificacionCaracteres = new UTF8Encoding();
                byte[] bytes_clave_ingresada = codificacionCaracteres.GetBytes(credencialPassword);


            }
        }

        public string encriptarSha1(string v_clave_ingresada)
        {
            UTF8Encoding codificacionCaracteres = new UTF8Encoding();
            byte[] bytes_clave_ingresada = codificacionCaracteres.GetBytes(v_clave_ingresada);

            SHA1CryptoServiceProvider encriptarSHA1 = new SHA1CryptoServiceProvider();
            string clave_encriptada = BitConverter.ToString(encriptarSHA1.ComputeHash(bytes_clave_ingresada)).Replace("-", "");
            return clave_encriptada;


        }
  
    }
}
