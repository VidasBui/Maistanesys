using Maistanesys.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Session;
namespace Maistanesys.Repos
{
    public class UserRepository
    {
        private const string _conn = "Server=localhost;Database=MaistanesysDB;Trusted_Connection=True;MultipleActiveResultSets=true";
       
        public void RegisterUser(string name, string password, int phone, string email)
        {

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "BEGIN INSERT INTO dbo.Users (Name,Password, IsAdmin, Phone, Email) "
                    +"VALUES(@name,@password,@isAdmin, @phone, @email) END";

                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                    cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
                    cmd.Parameters.Add("@isAdmin", SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@phone", SqlDbType.Int).Value = phone;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    cmd.Connection = conn;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                }

            }
         //   StateChange("dbo.ItemOrder", "Off");
        }

        public User GetUser(string name, string password)
        {
            User data = new User();
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "BEGIN select Name,Password, IsAdmin, Phone, Email,Id from dbo.Users where Name=@name and Password=@password END";

                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                    cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        
                        data.Name = string.Format("{0}", reader.GetValue(0));
                        data.Password = string.Format("{0}", reader.GetValue(1));
                        data.IsAdmin = (bool)reader.GetValue(2);
                        data.Phone = (int)reader.GetValue(3);
                        data.Email = string.Format("{0}", reader.GetValue(4));
                        data.Id = (int)reader.GetValue(5);

                    }
                }
            }
            return data;
        }

        public int GetUserId(string name)
        {
            int id=0;
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "BEGIN select Id from dbo.Users where Name=@name END";

                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        id =int.Parse( string.Format("{0}", reader.GetValue(0)));
                    }
                }
            }
            return id;
        }

        public int GetUserOrder(int id)
        {
            int orderId = 0;
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "BEGIN select Id,State,UserId from [dbo].[Order] where UserId=@id AND State=0  END";

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        orderId = int.Parse(string.Format("{0}", reader.GetValue(0)));
                    }
                }
            }
            return orderId;
        }

    }
}
