using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;

namespace Segunda_tarea_programada.Pages.Interfaz
{
    public class CodigoModel : PageModel
    {

        string ErrorMessage = "";
        [BindProperty]
        public codeEditar Code { get; set; }
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

                        // Agregar el parámetro de retorno
                        SqlParameter returnParameter = command.Parameters.Add("RetVal", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        command.ExecuteNonQuery();

                        // Obtener el valor de retorno
                        int returnValue = (int)returnParameter.Value;

                        if (returnValue == 1)
                        {
                            return RedirectToPage("/Interfaz/Editar");
                        }
                    }
                }
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return RedirectToPage();
            }
        }

        public class codeEditar
        {
            public string Codigo { get; set; }
        }

    }
}
