using Maistanesys.Models;
using System.Data;
using System.Data.SqlClient;
namespace Maistanesys.Repos
{
    public class ItemRepository
    {
        private const string _conn = "Server=localhost;Database=MaistanesysDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        public List<Item> getItems()
        {
            SqlCommand cmd;
            SqlDataReader reader;
            String sql;
            SqlConnection cnn;
            List<Item> items = new List<Item>();

            cnn = new SqlConnection(_conn);
            cnn.Open();
            sql = "select Id,Name,Price,Category from dbo.Items";
            cmd = new SqlCommand(sql, cnn);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Item dat = new Item();
                dat.Id = int.Parse(string.Format("{0}", reader.GetValue(0)));
                dat.Name = string.Format("{0}", reader.GetValue(1));
                dat.Price = float.Parse(string.Format("{0}", reader.GetValue(2)));
                dat.Category = (Category)Enum.Parse(typeof(Category), Convert.ToString(reader.GetValue(3)));
                //dat.Amount = int.Parse(string.Format("{0}", reader.GetValue(4)));
                //dat.OrderId = int.Parse(string.Format("{0}", reader.GetValue(5)));
                items.Add(dat);
            }

            return items;
        }

        public List<Order> getAllOrders()
        {
            SqlCommand cmd;
            SqlDataReader reader;
            String sql;
            SqlConnection cnn;
            List<Order> items = new List<Order>();

            cnn = new SqlConnection(_conn);
            cnn.Open();
            sql = "select State,DeliveryAddress,EstimatedTime,UserId,Id from [dbo].[Order] where State != 0";
            cmd = new SqlCommand(sql, cnn);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Order dat = new Order();
                dat.State = (State)Enum.Parse(typeof(State), Convert.ToString(reader.GetValue(0)));
                dat.DeliveryAddress = string.Format("{0}", reader.GetValue(1));
                dat.EstimatedTime = DateTime.Parse(string.Format("{0}", reader.GetValue(2)));
                dat.UserId = (int)reader.GetValue(3);
                dat.Id = (int)reader.GetValue(4);
                items.Add(dat);
            }

            return items;
        }

        public void ChangeState(int orderId,int state)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "update [dbo].[Order] set State=@state where Id=@id";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = orderId;
                    cmd.Parameters.Add("@state", SqlDbType.Int).Value = state;
                    cmd.Connection = conn;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                }

            }
        }

        public void ChangeStateAddress(int orderId, int state,string address)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "if exists(select 1 from [dbo].[ItemOrder] where orderId=@id)" +
                        "BEGIN update [dbo].[Order] set State=@state, DeliveryAddress=@address where Id=@id END";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = orderId;
                    cmd.Parameters.Add("@state", SqlDbType.Int).Value = state;
                    cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = address;
                    cmd.Connection = conn;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                }

            }
        }

        public List<Item> getOrderItems(int id)
        {
            SqlCommand cmd;
            SqlDataReader reader;
            String sql;
            SqlConnection cnn;
            List<Item> items = new List<Item>();

            cnn = new SqlConnection(_conn);
            cnn.Open();
            sql = "select Id,Name,Price,Category,count from dbo.Items right join (select orderId,itemId,count" +
                " from dbo.ItemOrder where dbo.ItemOrder.orderId=@id) as io on io.itemId=dbo.Items.Id";     
            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Item dat = new Item();
                dat.Id = int.Parse(string.Format("{0}", reader.GetValue(0)));
                dat.Name = string.Format("{0}", reader.GetValue(1));
                dat.Price = float.Parse(string.Format("{0}", reader.GetValue(2)));
                dat.Category = (Category)Enum.Parse(typeof(Category), Convert.ToString(reader.GetValue(3)));
                dat.Amount = int.Parse(string.Format("{0}", reader.GetValue(4)));
                items.Add(dat);
            }

            return items;
        }
        public List<Order> getOrders()
        {
            SqlCommand cmd;
            SqlDataReader reader;
            String sql;
            SqlConnection cnn;
            List<Order> orders = new List<Order>();

            cnn = new SqlConnection(_conn);
            cnn.Open();
            sql = "select Id,State,DeliveryAddress,Date,UserId from dbo.Orders where dbo.Orders.State != 0";     
            cmd = new SqlCommand(sql, cnn);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Order dat = new Order();
                dat.Id = int.Parse(string.Format("{0}", reader.GetValue(0)));
                dat.State = (State)Enum.Parse(typeof(State), Convert.ToString(reader.GetValue(1)));
                dat.DeliveryAddress = string.Format("{0}", reader.GetValue(2));
                dat.EstimatedTime = DateTime.Parse(string.Format("{0}", reader.GetValue(3)));
                dat.UserId = int.Parse(string.Format("{0}", reader.GetValue(4)));
                orders.Add(dat);
            }

            return orders;
        }

        public List<Order> getUserOrders(int Uid)
        {
            SqlCommand cmd;
            SqlDataReader reader;
            String sql;
            SqlConnection cnn;
            List<Order> orders = new List<Order>();

            cnn = new SqlConnection(_conn);
            cnn.Open();
            sql = "select Id,State,DeliveryAddress,EstimatedTime,UserId from [dbo].[Order] where UserId = @Uid and State != 0";     
            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.Add("@Uid", SqlDbType.Int).Value = Uid;
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Order dat = new Order();
                dat.Id = int.Parse(string.Format("{0}", reader.GetValue(0)));
                dat.State = (State)Enum.Parse(typeof(State), Convert.ToString(reader.GetValue(1)));
                dat.DeliveryAddress = string.Format("{0}", reader.GetValue(2));
                dat.EstimatedTime = DateTime.Parse(string.Format("{0}", reader.GetValue(3)));
                dat.UserId = Uid;
                orders.Add(dat);
            }

            return orders;
        }

        private void StateChange(string name, string state)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SET IDENTITY_INSERT " + name + " " + state;
                    cmd.Connection = conn;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                }
            }
        }
        public void addMenu(int item, int order, int quantity)
        {
         //   StateChange("dbo.ItemOrder", "On");
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "IF EXISTS (SELECT 1 FROM dbo.ItemOrder WHERE itemId=@item AND orderId=@order)" +
                        "BEGIN " +
                        "UPDATE dbo.ItemOrder SET count = count + @quantity WHERE itemId=@item AND orderId=@order " +
                        "END ELSE BEGIN INSERT INTO dbo.ItemOrder (itemId,orderId, count) VALUES(@item,@order,@quantity) END";

                    cmd.Parameters.Add("@item", SqlDbType.Int).Value = item;
                    cmd.Parameters.Add("@order", SqlDbType.Int).Value = order;
                    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = quantity;
                    cmd.Connection = conn;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    //rowsAffected += 1;
                }

            }
         //   StateChange("dbo.ItemOrder", "Off");
        }

        public void addToMenu(string name, float price, int cat)
        {
            //   StateChange("dbo.ItemOrder", "On");
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into dbo.Items (Name,Price,Category) values (@name,@price,@cat)";
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value =name;
                    cmd.Parameters.Add("@price", SqlDbType.Float).Value = price;
                    cmd.Parameters.Add("@cat", SqlDbType.Int).Value = cat;
                    cmd.Connection = conn;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    //rowsAffected += 1;
                }

            }
            //   StateChange("dbo.ItemOrder", "Off");
        }

        public Item getItem(int id)
        {
            Item data = new Item();
            SqlCommand cmd;
            SqlDataReader reader;
            String sql;
            SqlConnection cnn;

            cnn = new SqlConnection(_conn);
            cnn.Open();       
            sql = "Use MaistanesysDB select Id,Name,Price,Category from dbo.Items where Id=@tab";
            cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.Add("@tab", SqlDbType.Int).Value = id;
            reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                data.Id = int.Parse(string.Format("{0}", reader.GetValue(0)));
                data.Name = string.Format("{0}", reader.GetValue(1));
                data.Price = float.Parse(string.Format("{0}", reader.GetValue(2)));
                data.Category = (Category)Enum.Parse(typeof(Category), Convert.ToString(reader.GetValue(3)));
                //data.Amount = int.Parse(string.Format("{0}", reader.GetValue(4)));
                //data.OrderId = int.Parse(string.Format("{0}", reader.GetValue(5)));
            }


            return data;
        }

        public bool UpdateItemForRestaurant(Item item)
        {
            try
            {
                SqlConnection mySqlConnection = new SqlConnection(_conn);
                string sqlquery = @"UPDATE dbo.Items SET Name=@name, Price=@price,
                                    Category=@category WHERE Id=@id";
                SqlCommand SqlCommand = new SqlCommand(sqlquery, mySqlConnection);
                SqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = item.Id;
                SqlCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = item.Name;
                SqlCommand.Parameters.Add("@price", SqlDbType.Float).Value = item.Price;
                SqlCommand.Parameters.Add("@category", SqlDbType.Int).Value = item.Category;
                //SqlCommand.Parameters.Add("@amount", SqlDbType.Int).Value = item.Amount;
                mySqlConnection.Open();
                SqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateItem(Item item, int id)
        {
            try
            {
                SqlConnection mySqlConnection = new SqlConnection(_conn);
                string sqlquery = @"UPDATE dbo.ItemOrder SET count=@amount WHERE orderId=@id and itemId = @itemId";
                SqlCommand SqlCommand = new SqlCommand(sqlquery, mySqlConnection);
                SqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                SqlCommand.Parameters.Add("@amount", SqlDbType.Int).Value = item.Amount;
                SqlCommand.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
                mySqlConnection.Open();
                SqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //if flag=1,delete from menu, if flag=0 from order
        public void DeleteItem(int id,int flag,int orderid)
        {
            if (flag == 1)
            {
                SqlConnection mySqlConnection = new SqlConnection(_conn);
                string sqlquery = @"DELETE FROM dbo.Items where Id=@id";
                SqlCommand mySqlCommand = new SqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
            }
            if(flag == 0){
                SqlConnection mySqlConnection = new SqlConnection(_conn);
                string sqlquery = @"use MaistanesysDB DELETE FROM dbo.ItemOrder where itemId=@itemId AND orderId=@orderId";
                SqlCommand mySqlCommand = new SqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("@itemId", SqlDbType.Int).Value = id;
                mySqlCommand.Parameters.Add("@orderId", SqlDbType.Int).Value = orderid;
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
            }

        }

        public void addOrderIfNone(int userId)
        {
            //   StateChange("dbo.ItemOrder", "On");
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "if not exists(select 1 from [dbo].[Order] where State=0 AND UserId=@id)" +
                        "BEGIN insert [dbo].[Order] (State,DeliveryAddress, EstimatedTime,UserId) values(0,'Adreso nėra',CURRENT_TIMESTAMP,@id) END";
                    cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = userId;
                    cmd.Connection = conn;
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    //rowsAffected += 1;
                }

            }
            //   StateChange("dbo.ItemOrder", "Off");
        }


    }
}
