using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Client
{
    public class CreateModel : PageModel
    {
        public ClientInfo ClientInfo = new ClientInfo();
        public String errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }
        public void OnPost() 
        {
            ClientInfo.name = Request.Form["name"];
            ClientInfo.email = Request.Form["email"];
            ClientInfo.phone = Request.Form["phone"];
            ClientInfo.address = Request.Form["address"];

            if(ClientInfo.name.Length==0|| ClientInfo.email.Length==0 ||
                ClientInfo.phone.Length==0 || ClientInfo.address.Length==0)
            {
                errorMessage = "All the fields are required";
                return;
            }
            // save the data in database

            try
            {
                String connectingString = " Data Source =.; Initial Catalog = MyStore; Integrated Security = True";
                using (SqlConnection connection = new SqlConnection(connectingString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients" +
                        "(name, email,phone,address) VALUES" +
                        "(@name, @email,@phone,@address);";

                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", ClientInfo.name);
                        command.Parameters.AddWithValue ("@email", ClientInfo.email);
                        command.Parameters.AddWithValue("@phone", ClientInfo.phone);
                        command.Parameters.AddWithValue("@address", ClientInfo.address);


                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
                
            }

            ClientInfo.name = "";ClientInfo.email = "";ClientInfo.phone = "";ClientInfo.address= "";
            successMessage = "New Client Added Correctly";

            Response.Redirect("/Client/Index");
        }
    }
}
