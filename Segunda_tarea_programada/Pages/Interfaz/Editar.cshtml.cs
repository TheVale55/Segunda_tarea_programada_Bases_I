using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using static Segunda_tarea_programada.Pages.Interfaz.CodigoModel;

namespace Segunda_tarea_programada.Pages.Interfaz
{
    public class EditarModel : PageModel
    {
        string connectionString = "Data Source=Proyecto2.mssql.somee.com;Initial Catalog=Proyecto2;Persist Security Info=True;User ID=Proyecto2_SQLLogin_1;Password=c6iko37icl";

        public ArticulosInfo articulosInfo = new ArticulosInfo();
        public ClasesInfo clasesInfo = new ClasesInfo();

        public List<ClasesInfo> ListaClases = new List<ClasesInfo>();
        

        public string errorMensaje = "";
        public string mensajeExito = "";

        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Agrega las clases a la lista
                    string procedureClases = "DesplegarClases";
                    using (SqlCommand command = new SqlCommand(procedureClases, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClasesInfo clases = new ClasesInfo();
                                clases.Nombre = "" + reader.GetString(0);

                                ListaClases.Add(clases);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errorMensaje = ex.Message;
            }
        }


        public void OnPost()
        {
            articulosInfo.Codigo = Request.Form["Codigo"];
            articulosInfo.Articulo = Request.Form["Nombre"];
            articulosInfo.Clase = Request.Form["Clase"];
            string precioString = Request.Form["Precio"];
         
            if (articulosInfo.Codigo.Length == 0)
            {
                errorMensaje = "Todos los campos deben completarse";
                return;
            }

            if (articulosInfo.Articulo.Length == 0)
            {
                errorMensaje = "Todos los campos deben completarse";
                return;
            }


            if (articulosInfo.Clase.Length == 0)
            {
                errorMensaje = "Todos los campos deben completarse";
                return;
            }

            //Guardar la info en la base de datos
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string procedureName = "Actualizar";
                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CodigoArticulo", articulosInfo.Codigo);
                        command.Parameters.AddWithValue("@NuevoCodigo", articulosInfo.Codigo);
                        command.Parameters.AddWithValue("@NuevoNombre", articulosInfo.Articulo);
                        command.Parameters.AddWithValue("@NuevoClase", articulosInfo.Clase);
                        command.Parameters.AddWithValue("NuevoPrecio", articulosInfo.Precio);
                        command.Parameters.AddWithValue("@IP", Request.HttpContext.Connection.RemoteIpAddress.ToString());
                        command.Parameters.AddWithValue("@User", "Valeria");



                        // Agregar el parámetro de retorno
                        SqlParameter returnParameter = command.Parameters.Add("RetVal", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        command.ExecuteNonQuery();

                        // Obtener el valor de retorno
                        int returnValue = (int)returnParameter.Value;
                        if (returnValue == 1)
                        {
                            errorMensaje = "El producto ya existe";
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMensaje = ex.Message;
                return;
            }

            articulosInfo.Codigo = ""; articulosInfo.Articulo = ""; articulosInfo.Clase = ""; articulosInfo.Precio = 0;
            mensajeExito = "Producto editado exitosamente";

            Response.Redirect("/Interfaz/Articulos");

        }

    }
}

