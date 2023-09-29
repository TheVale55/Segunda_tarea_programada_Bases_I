using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace Segunda_tarea_programada.Pages.Interfaz
{
    public class EliminarModel : PageModel
    {
        string ErrorMessage = "";
        [BindProperty]
        public codeEliminar Code { get; set; }

        public void OnGet()
        {
        }
     
        public IActionResult OnPost() 
        {
            try
            {
                string connectionString = "Data Source=Proyecto2.mssql.somee.com;Initial Catalog=Proyecto2;Persist Security Info=True;User ID=Proyecto2_SQLLogin_1;Password=c6iko37icl";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string procedureValidarCodigo = "ValidarCodigo";

                    using (SqlCommand command = new SqlCommand(procedureValidarCodigo, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Codigo", Code.Codigo);

                        // Agregar el par�metro de retorno
                        SqlParameter returnParameter = command.Parameters.Add("RetVal", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        command.ExecuteNonQuery();

                        // Obtener el valor de retorno
                        int returnValue = (int)returnParameter.Value;

                        if (returnValue == 1)
                        {
                            // Realiza la eliminaci�n del art�culo si el c�digo existe
                            // Puedes implementar la l�gica de eliminaci�n aqu�
                            string procedureEliminarArticulo = "Borrado";

                            using (SqlCommand eliminarCommand = new SqlCommand(procedureEliminarArticulo, connection))
                            {
                                eliminarCommand.CommandType = CommandType.StoredProcedure;

                                eliminarCommand.Parameters.AddWithValue("@Codigo", Code.Codigo);
                                eliminarCommand.Parameters.AddWithValue("@IP", Request.HttpContext.Connection.RemoteIpAddress.ToString());
                                eliminarCommand.Parameters.AddWithValue("@User", "Valeria");

                                eliminarCommand.ExecuteNonQuery();
                                return RedirectToPage("/Interfaz/Articulos");
                            }
                        }
                        else
                        {
                            ErrorMessage = "El c�digo no existe. Por favor, ingrese un c�digo v�lido.";
                        }
                    }
                }

                // Redirige a la p�gina actual despu�s de la eliminaci�n
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
                return RedirectToPage();
            }

        }

    }

    public class codeEliminar
    {
        public string Codigo { get; set; }
    }

}



