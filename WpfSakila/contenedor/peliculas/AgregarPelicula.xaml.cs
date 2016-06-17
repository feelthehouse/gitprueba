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

namespace WpfSakila.contenedor.peliculas
{
    /// <summary>
    /// Lógica de interacción para AgregarPelicula.xaml
    /// </summary>
    public partial class AgregarPelicula : Window
    {
        public AgregarPelicula()
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
            // Cargar datos en la tabla language. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.languageTableAdapter sakilaDataSetlanguageTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.languageTableAdapter();
            sakilaDataSetlanguageTableAdapter.Fill(sakilaDataSet.language);
            System.Windows.Data.CollectionViewSource languageViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("languageViewSource")));
            languageViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla category. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.categoryTableAdapter sakilaDataSetcategoryTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.categoryTableAdapter();
            sakilaDataSetcategoryTableAdapter.Fill(sakilaDataSet.category);
            System.Windows.Data.CollectionViewSource categoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("categoryViewSource")));
            categoryViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla store. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.storeTableAdapter sakilaDataSetstoreTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.storeTableAdapter();
            sakilaDataSetstoreTableAdapter.Fill(sakilaDataSet.store);
            System.Windows.Data.CollectionViewSource storeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("storeViewSource")));
            storeViewSource.View.MoveCurrentToFirst();
            // Cargar datos en la tabla address. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.addressTableAdapter sakilaDataSetaddressTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.addressTableAdapter();
            sakilaDataSetaddressTableAdapter.Fill(sakilaDataSet.address);
            System.Windows.Data.CollectionViewSource addressViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("addressViewSource")));
            addressViewSource.View.MoveCurrentToFirst();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            DateTime fechaActual = DateTime.Now;
            int idIdioma = Convert.ToInt32(languageComboBox.SelectedValue);
            int rentalDuration = 0; //se asigna 0 por que la pelicula aún no es arrendada
       
            SqlConnection crearConexion = generarConexion();
            SqlCommand insertarValoresPelicula = new SqlCommand("INSERT INTO film(title,description,release_year,language_id,rental_duration,rental_rate,length,replacement_cost,rating,special_features,last_update) VALUES(@p_title,@p_description,@p_release_year,@p_language_id,@p_rental_duration,@p_rental_rate,@p_length,@p_replacement_cost,@p_rating,@p_special_features,@p_last_update)", crearConexion);
           
            insertarValoresPelicula.Parameters.AddWithValue("@p_title", txtNombrePelicula.Text.ToUpper());
            insertarValoresPelicula.Parameters.AddWithValue("@p_description", txtDescripcion.Text);
            insertarValoresPelicula.Parameters.AddWithValue("@p_release_year", txtAñoPelicula.Text);
            insertarValoresPelicula.Parameters.AddWithValue("@p_language_id", idIdioma);
            insertarValoresPelicula.Parameters.AddWithValue("@p_rental_duration", rentalDuration);
            insertarValoresPelicula.Parameters.AddWithValue("@p_rental_rate", txtValorRenta.Text);
            insertarValoresPelicula.Parameters.AddWithValue("@p_length", txtDuracion.Text);
            insertarValoresPelicula.Parameters.AddWithValue("@p_replacement_cost", txtValorRemplazo.Text);
            insertarValoresPelicula.Parameters.AddWithValue("@p_rating", comboBoxCategoria.SelectionBoxItem);
            insertarValoresPelicula.Parameters.AddWithValue("@p_special_features", comboBoxCaracteristicasEspeciales.SelectionBoxItem);
            insertarValoresPelicula.Parameters.AddWithValue("@p_last_update", fechaActual.Date);





            crearConexion.Open();

            try
            {
                insertarValoresPelicula.ExecuteNonQuery();
                MessageBox.Show("Pelicula Guardada");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex);
            }

           
            // aqui hace la pega el current id , para seleccionar el id al momento que se guarda la pelicula
            try   
            {
                SqlCommand seleccionarId = new SqlCommand("SELECT IDENT_CURRENT('film') as film_id", crearConexion);
            //seleccionarId.Parameters.AddWithValue("@p_film_id", film_id);
                SqlDataReader consultaDatos = seleccionarId.ExecuteReader();
                consultaDatos.Read();
                int film_id = int.Parse(consultaDatos["film_id"].ToString());
                MessageBox.Show("film_id " + film_id);

                consultaDatos.Close();
                

                SqlCommand insertarDatosEnInventory = new SqlCommand("INSERT INTO inventory(film_id,store_id) VALUES(@p_film_id,@p_store_id)", crearConexion);
                insertarDatosEnInventory.Parameters.AddWithValue("@p_film_id", film_id);
                insertarDatosEnInventory.Parameters.AddWithValue("@p_store_id", store_idComboBox.SelectedValue);

                try
                {
                    insertarDatosEnInventory.ExecuteNonQuery();
                    MessageBox.Show("insertó en inventory");
                }
                catch (Exception ex )
                {

                    MessageBox.Show("Error :"+ ex.Message);
                }
                
            }
            catch (Exception mensajeError)
            {

                MessageBox.Show("Error " + mensajeError.Message);
            }
            crearConexion.Close();

        }



      //así se captura el id antes de guardar la pelicula, arriba de esto hace la misma pega , pero
      //junto con agregar la pelicula ...( hace toda la wea junta) xd

      /*  public void consultarId(int film_id)
        {
            film_id = 0;
            SqlConnection crearConexion = generarConexion();
            SqlCommand seleccionarId = new SqlCommand("SELECT IDENT_CURRENT('film') as film_id", crearConexion);
            seleccionarId.Parameters.AddWithValue("@p_film_id", film_id);

            try
            {
                crearConexion.Open();
                SqlDataReader consultaDatos = seleccionarId.ExecuteReader();
                consultaDatos.Read();
                film_id = int.Parse(consultaDatos["film_id"].ToString());

                consultaDatos.Close();
                crearConexion.Close();

                SqlCommand insertarDatosEnInventory = new SqlCommand("INSERT INTO inventory(film_id,store_id) VALUES(@p_film_id,@p_store_id)", crearConexion);
                insertarDatosEnInventory.Parameters.AddWithValue("@p_film_id", film_id);
                insertarDatosEnInventory.Parameters.AddWithValue("@p_store_id", addressComboBox);


            }
            catch (Exception mensajeError)
            {

                MessageBox.Show("Error " + mensajeError.Message);
            }*/


        }

    }
    
    

