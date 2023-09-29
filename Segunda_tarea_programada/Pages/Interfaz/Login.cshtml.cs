using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Segunda_tarea_programada.Pages.Interfaz
{
    public class LoginModel : PageModel
    {
        public string errorMensaje = "";

        [BindProperty]
        public LoginInfo Login { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                string connectionString = "Data Source=Proyecto2.mssql.somee.com;Initial Catalog=Proyecto2;Persist Security Info=True;User ID=Proyecto2_SQLLogin_1;Password=c6iko37icl";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string procedureName = "LogIn";

                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@User", Login.UserName);
                        command.Parameters.AddWithValue("@Pass", Login.Password);
                        command.Parameters.AddWithValue("@IP", Request.HttpContext.Connection.RemoteIpAddress.ToString());

                        SqlParameter returnParameter = command.Parameters.Add("RetVal", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        await command.ExecuteNonQueryAsync();

                        int returnValue = (int)returnParameter.Value;
                        if (returnValue == 1)
                        {
                            // Si la autenticación es exitosa, guarda el nombre de usuario y la IP
                            string UserName = Login.UserName;
                            string UserIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                            return RedirectToPage("/Interfaz/Articulos");
                        }
                        else
                        {
                            ViewData[errorMensaje] = "Inicio de sesión no exitoso"; // Mensaje de error en caso de que el inicio de sesión falle
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Excepción: " + ex.ToString());
                // Manejo de excepciones aquí
            }

            // Permanece en la página de inicio de sesión en caso de error
            return Page();
        }

        // Otras funciones de la clase pueden acceder a UserName y UserIP
    }

    public class LoginInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserIP { get; set; }
    }
}





