using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Segunda_tarea_programada.Pages.Interfaz
{
    public class ArticulosModel : PageModel
    {
        private readonly string connectionString = "Data Source=Proyecto2.mssql.somee.com;Initial Catalog=Proyecto2;Persist Security Info=True;User ID=Proyecto2_SQLLogin_1;Password=c6iko37icl";

        // Propiedad para controlar la visibilidad de la tabla completa o filtrada
        public bool MostrarTablaCompleta { get; set; } = true;

        // Listas para almacenar los datos
        public List<ArticulosInfo> ListaFiltrada { get; set; } = new List<ArticulosInfo>();
        public List<ArticulosInfo> ListaArticulos { get; set; } = new List<ArticulosInfo>();
        public List<ClasesInfo> ListaClases { get; set; } = new List<ClasesInfo>();

        // Método OnGet para cargar los datos iniciales
        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Agrega los artículos a la lista
                    string procedureArticulos = "SelectInicio";
                    using (SqlCommand command = new SqlCommand(procedureArticulos, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ArticulosInfo articulos = new ArticulosInfo();
                                articulos.Codigo = reader.GetString(0);
                                articulos.Articulo = reader.GetString(1);
                                articulos.Clase = reader.GetString(2);
                                articulos.Precio = reader.GetDecimal(3);

                                ListaArticulos.Add(articulos);
                            }
                        }
                    }

                    // Agrega las clases a la lista
                    string procedureClases = "DesplegarClases";
                    using (SqlCommand command = new SqlCommand(procedureClases, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClasesInfo clases = new ClasesInfo();
                                clases.Nombre = reader.GetString(0);

                                ListaClases.Add(clases);
                            }
                        }
                    }
                }

                // Establece MostrarTablaCompleta en true (mostrar tabla completa inicialmente)
                MostrarTablaCompleta = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        // Método OnPost para el filtrado
        public IActionResult OnPostFiltrarPorClase(string Clase)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string procedureClase = "FiltraClase";

                    using (SqlCommand command = new SqlCommand(procedureClase, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Clase", Clase);
                        command.Parameters.AddWithValue("@IP", Request.HttpContext.Connection.RemoteIpAddress.ToString());
                        command.Parameters.AddWithValue("@User", "Valeria");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ArticulosInfo articulos = new ArticulosInfo();
                                articulos.Codigo = reader.GetString(0);
                                articulos.Articulo = reader.GetString(1);
                                articulos.Clase = reader.GetString(2);
                                articulos.Precio = reader.GetDecimal(3);

                                ListaFiltrada.Add(articulos);
                            }
                        }
                    }

                    // Establece el valor de MostrarTablaCompleta según si hay datos en ListaFiltrada
                    MostrarTablaCompleta = ListaFiltrada.Count == 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

            // Redirige a la página actual para mostrar los resultados
            return Page();
        }

        public IActionResult OnPostFiltrarPorNombre(string Nombre)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string procedureClase = "FiltraNombre";

                    using (SqlCommand command = new SqlCommand(procedureClase, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Filtro", Nombre);
                        command.Parameters.AddWithValue("@IP", Request.HttpContext.Connection.RemoteIpAddress.ToString());
                        command.Parameters.AddWithValue("@User", "Valeria");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ArticulosInfo articulos = new ArticulosInfo();
                                articulos.Codigo = reader.GetString(0);
                                articulos.Articulo = reader.GetString(1);
                                articulos.Clase = reader.GetString(2);
                                articulos.Precio = reader.GetDecimal(3);

                                ListaFiltrada.Add(articulos);
                            }
                        }
                    }

                    // Establece el valor de MostrarTablaCompleta según si hay datos en ListaFiltrada
                    MostrarTablaCompleta = ListaFiltrada.Count == 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

            // Redirige a la página actual para mostrar los resultados
            return Page();
        }

        public IActionResult OnPostFiltrarPorCantidad(decimal Cantidad)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string procedureClase = "FiltraCantidad";

                    using (SqlCommand command = new SqlCommand(procedureClase, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Cantidad", Cantidad);
                        command.Parameters.AddWithValue("@IP", Request.HttpContext.Connection.RemoteIpAddress.ToString());
                        command.Parameters.AddWithValue("@User", "Valeria");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ArticulosInfo articulos = new ArticulosInfo();
                                articulos.Codigo = reader.GetString(0);
                                articulos.Articulo = reader.GetString(1);
                                articulos.Clase = reader.GetString(2);
                                articulos.Precio = reader.GetDecimal(3);

                                ListaFiltrada.Add(articulos);
                            }
                        }
                    }

                    // Establece el valor de MostrarTablaCompleta según si hay datos en ListaFiltrada
                    MostrarTablaCompleta = ListaFiltrada.Count == 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

            // Redirige a la página actual para mostrar los resultados
            return Page();
        }


    }

    public class ArticulosInfo
    {
        public string Codigo { get; set; }
        public string Articulo { get; set; }
        public string Clase { get; set; }
        public decimal Precio { get; set; } = 99.99M;
    }

    public class ClasesInfo
    {
        public string Nombre { get; set; }
    }
}
