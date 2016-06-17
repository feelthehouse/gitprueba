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
    /// Lógica de interacción para OpcionesPeliculas.xaml
    /// </summary>
    public partial class OpcionesPeliculas : Window
    {
        public OpcionesPeliculas()
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
            // Cargar datos en la tabla film. Puede modificar este código según sea necesario.
            WpfSakila.sakilaDataSetTableAdapters.filmTableAdapter sakilaDataSetfilmTableAdapter = new WpfSakila.sakilaDataSetTableAdapters.filmTableAdapter();
            sakilaDataSetfilmTableAdapter.Fill(sakilaDataSet.film);
            System.Windows.Data.CollectionViewSource filmViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("filmViewSource")));
            filmViewSource.View.MoveCurrentToFirst();
            filmDataGrid.SelectedValuePath = "film_id";
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AgregarPelicula nuevaVentana = new AgregarPelicula();
            nuevaVentana.Show();         
        }
 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int film_id = Convert.ToInt32(filmDataGrid.SelectedValue.ToString());
            MessageBox.Show("Usted a seleccionado " + film_id);
            VentanaActualizarPelicula mostrarVentanaActualizarPelicula = new VentanaActualizarPelicula(film_id);
            mostrarVentanaActualizarPelicula.Show();
        }

        private void btnEliminarPelicula_Click(object sender, RoutedEventArgs e)
        {
            int peliculaSeleccionada = Convert.ToInt32(filmDataGrid.SelectedValue);
            Boolean existenPeliculasRentadas = verificarPeliculasRentada(peliculaSeleccionada);
            if (existenPeliculasRentadas)
            {
                MessageBox.Show("La pelicula esta en arriendo, por favor elimine el arriendo para así porder eliminar la pelicula. ");
            }
            else
            {
                eliminarPelicula(peliculaSeleccionada);
            }
        }

        private void eliminarPelicula(int film_id)
        {
            SqlConnection conecta = generarConexion();
            SqlCommand eliminadeinventory = new SqlCommand("delete from inventory where film_id=@p_film_id", conecta);
            eliminadeinventory.Parameters.AddWithValue("@p_film_id", film_id);
            SqlCommand elimina = new SqlCommand("delete from film where film_id=@p_film_id", conecta);
            elimina.Parameters.AddWithValue("@p_film_id", film_id);

            try
            {
                conecta.Open();
                eliminadeinventory.ExecuteNonQuery();
                elimina.ExecuteNonQuery();
                
                MessageBox.Show("Registro Borrado");
                conecta.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error :" + ex.Message);
            }
        }

        private Boolean verificarPeliculasRentada(int IdRenta)
        {
            Boolean existenRegistros = false;
            SqlConnection conecta = generarConexion();
            SqlCommand consultaFilm = new SqlCommand("select actor_id from film_actor where actor_id=@p_actor_id", conecta);
            consultaFilm.Parameters.AddWithValue("@p_actor_id", IdRenta);

            try
            {
                conecta.Open();
                SqlDataReader leeDatos = consultaFilm.ExecuteReader();
                leeDatos.Read();

                if (leeDatos.HasRows)
                {
                    existenRegistros = true;
                }
                else
                {
                    existenRegistros = false;
                }
                conecta.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
            return existenRegistros;
            }


        public void hardDelete()
        {
            int film_id = Convert.ToInt32(filmDataGrid.SelectedValue);

            SqlConnection conecta = generarConexion();

            SqlCommand ejecutarEliminacionRenta = new SqlCommand("delete from rental where inventory_id in(select inventory_id from inventory where film_id=@p_film_id)", conecta);
            ejecutarEliminacionRenta.Parameters.AddWithValue("@p_film_id", film_id);
            SqlCommand ejecutarEliminacionInventory = new SqlCommand("delete from inventory where film_id=@p_film_id", conecta);
            ejecutarEliminacionInventory.Parameters.AddWithValue("@p_film_id", film_id);
            SqlCommand ejecutarEliminacionActorFilm = new SqlCommand("delete from film_actor where film_id=@p_film_id", conecta);
            ejecutarEliminacionActorFilm.Parameters.AddWithValue("@p_film_id", film_id);
            SqlCommand ejecutarEliminacionCategoriaFilm = new SqlCommand("delete from film_category where film_id=@p_film_id", conecta);
            ejecutarEliminacionCategoriaFilm.Parameters.AddWithValue("@p_film_id", film_id);
            SqlCommand ejecutarEliminacionFilm = new SqlCommand("delete from film where film_id=@p_film_id;", conecta);
            ejecutarEliminacionFilm.Parameters.AddWithValue("@p_film_id", film_id);
            conecta.Open();

            try
            {

                MessageBox.Show("Advertencia !, a continuacion eliminara la pelicula " + film_id + " con todos sus registros en la base de datos, procure estar seguro antes de ejecutar esta operacion");
                
                ejecutarEliminacionRenta.ExecuteNonQuery();
                ejecutarEliminacionInventory.ExecuteNonQuery();
                ejecutarEliminacionActorFilm.ExecuteNonQuery();
                ejecutarEliminacionCategoriaFilm.ExecuteNonQuery();
                ejecutarEliminacionFilm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            conecta.Close();

        }
        private void btnHardDelete_Click(object sender, RoutedEventArgs e)
        {
            hardDelete();

        }


      

        
    }
}
