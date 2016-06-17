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
    /// Lógica de interacción para VentanaActualizarPelicula.xaml
    /// </summary>
    public partial class VentanaActualizarPelicula : Window
    {
        public VentanaActualizarPelicula()
        {
            InitializeComponent();
           
        }

        public VentanaActualizarPelicula(int film_id)
        {
            InitializeComponent();
            generarDatosPelicula(film_id);
        }

        private SqlConnection generarConexion()
        {
            SqlConnection crearConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["WpfSakila.Properties.Settings.sakilaConnectionString"].ToString());
            return crearConexion;
        }

        public void generarDatosPelicula(int film_id)
        {
            SqlConnection crearConexion = generarConexion();
            SqlCommand consultarValores = new SqlCommand("select film_id,title,description,release_year,length,rental_rate,replacement_cost from film WHERE film_id = @p_film_id", crearConexion);
            consultarValores.Parameters.AddWithValue("@p_film_id", film_id);
            

            try
            {
                crearConexion.Open();
                SqlDataReader leeDatos = consultarValores.ExecuteReader();
                leeDatos.Read();

                labelIdFilm.Content = film_id.ToString();
                txtActualizarNombrePelicula1.Text = leeDatos["title"].ToString();
                txtActualizarDescripcionPelicula.Text = leeDatos["description"].ToString();
                txtActualizarAñoPelicula.Text = leeDatos["release_year"].ToString();
                txtActualizarDuracionPelicula.Text = leeDatos["length"].ToString();
                txtActualizarValorRenta.Text = leeDatos["rental_rate"].ToString();
                txtActualizarValorReemplazo.Text = leeDatos["replacement_cost"].ToString();
                
                crearConexion.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error, "+  ex.StackTrace);
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            WpfSakila.sakilaDataSet sakilaDataSet = ((WpfSakila.sakilaDataSet)(this.FindResource("sakilaDataSet")));
            // Cargar datos en la tabla language. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.languageTableAdapter sakilaDataSetlanguageTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.languageTableAdapter();
            sakilaDataSetlanguageTableAdapter.Fill(sakilaDataSet.language);
            System.Windows.Data.CollectionViewSource languageViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("languageViewSource")));
            languageViewSource.View.MoveCurrentToFirst();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conectar = generarConexion();
            SqlCommand actualizarPelicula = new SqlCommand("UPDATE film SET title=@p_title,description=@p_description,release_year=@p_release_year,length=@p_length,rental_rate=@p_rental_rate,replacement_cost=@p_replacement_cost WHERE film_id=@p_film_id", conectar);

            actualizarPelicula.Parameters.AddWithValue("@p_film_id", labelIdFilm.Content);
            actualizarPelicula.Parameters.AddWithValue("@p_title", txtActualizarNombrePelicula1.Text);
            actualizarPelicula.Parameters.AddWithValue("@p_description", txtActualizarDescripcionPelicula.Text);
            actualizarPelicula.Parameters.AddWithValue("@p_release_year", txtActualizarAñoPelicula.Text);
            actualizarPelicula.Parameters.AddWithValue("@p_length", txtActualizarDuracionPelicula.Text);
            actualizarPelicula.Parameters.AddWithValue("@p_rental_rate", txtActualizarValorRenta.Text);
            actualizarPelicula.Parameters.AddWithValue("@p_replacement_cost", txtActualizarValorReemplazo.Text);
            try
            {
                conectar.Open();
                actualizarPelicula.ExecuteNonQuery();

                MessageBox.Show("Registro Actualizado Correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }


        }
    }
}
