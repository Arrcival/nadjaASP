using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nadja.Models
{
    public static class Dal
    {
        private static MySqlConnection objMySqlCnx;

        private static void DoConnection()
        {
            // Database connection
            string strConnect = ConfigurationManager.AppSettings["DatabaseConnectionString"];
            objMySqlCnx = new MySqlConnection(strConnect);
            objMySqlCnx.Open();
        }

        private static void CloseConnection()
        {
            objMySqlCnx.Close();
        }

        public static User GetUser(string idUser)
        {
            DoConnection();
            User user = null;
            string sql = "SELECT users.ID, users.DiscordName, users.Gems, users.Common, users.Uncommon, users.Rare, users.Epic " +
                "FROM users " +
                "WHERE DiscordID = '" + idUser + "';";

            MySqlCommand objSelect = new MySqlCommand(sql, objMySqlCnx);
            MySqlDataReader objReader = objSelect.ExecuteReader();
            int ID = -1;
            string nameDiscord = null;
            int gems = 0, common = 0, uncommon = 0, rare = 0, epic = 0;
            
            while (objReader.Read())
            {
                ID = int.Parse(objReader.GetValue(0).ToString());
                nameDiscord = objReader.GetValue(1).ToString();
                gems = int.Parse(objReader.GetValue(2).ToString());
                common = int.Parse(objReader.GetValue(3).ToString());
                uncommon = int.Parse(objReader.GetValue(4).ToString());
                rare = int.Parse(objReader.GetValue(5).ToString());
                epic = int.Parse(objReader.GetValue(6).ToString());
            }


            objReader.Close();
            CloseConnection();

            if (ID == -1)
                return null;
            

            user = new User(ID, idUser, nameDiscord, gems, common, uncommon, rare, epic, new List<Legendary>());

            List<Legendary> legendaries = new List<Legendary>();

            sql = "SELECT possess.LegendaryID, legendaries.Name FROM possess, legendaries WHERE possess.LegendaryID = legendaries.ID AND possess.UserID = " + user.ID;

            DoConnection();
            objSelect = new MySqlCommand(sql, objMySqlCnx);
            objReader = objSelect.ExecuteReader();
            while (objReader.Read())
            {
                Legendary legendary = new Legendary
                {
                    ID = int.Parse(objReader.GetValue(0).ToString()),
                    Name = objReader.GetValue(1).ToString()
                };
                legendaries.Add(legendary);

            }

            user.Legendaries = legendaries;
            objReader.Close();
            CloseConnection();

            return user;

        }

        public static ServerUser GetServerUser(string idUser, string idServer)
        {
            User user = GetUser(idUser);

            if (user == null)
                return null;

            ServerUser serverUser = new ServerUser(user);

            DoConnection();
            string sql = "SELECT serverusers.DiscordServerName, serverusers.Points FROM serverusers" +
                " WHERE serverusers.DiscordID = '" + idUser + "' " +
                " AND serverusers.ServerID = '" + idServer + "';";

            MySqlCommand objSelect = new MySqlCommand(sql, objMySqlCnx);
            MySqlDataReader objReader = objSelect.ExecuteReader();
            string serverName = null;
            int points = 0;
            while (objReader.Read())
            {
                serverName = objReader.GetValue(0).ToString();
                points = int.Parse(objReader.GetValue(1).ToString());
            }
            objReader.Close();
            CloseConnection();

            serverUser.ServerNameUser = serverName;
            serverUser.Points = points;
            serverUser.ServerID = idServer;

            return serverUser;
        }

        public static string GetIdUser(string name)
        {
            DoConnection();
            string sql = "SELECT users.DiscordID,  FROM users," +
                "WHERE users.DiscordName = '" + name + "';";
            MySqlCommand objSelect = new MySqlCommand(sql, objMySqlCnx);
            MySqlDataReader objReader = objSelect.ExecuteReader();
            string idUser = null;
            while (objReader.Read())
                idUser = objReader.GetValue(0).ToString();

            return idUser;
        }

        public static int GetIDItem(string name)
        {
            int id = -1;
            DoConnection();
            string sql = "SELECT DISTINCT(items.ID) FROM items, slangs " +
                " WHERE items.Name LIKE @val1" +
                " OR (slangs.ItemID = items.ID  " +
                " AND slangs.Slang LIKE @val1);";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.Parameters.AddWithValue("@val1", name);
            objGet.Prepare();
            MySqlDataReader objReader = objGet.ExecuteReader();

            while (objReader.Read())
                id = int.Parse(objReader.GetValue(0).ToString());


            objReader.Close();
            CloseConnection();

            return id;

        }

        public static Item GetItem(int idItem, bool complete)
        {
            Item item = null;
            DoConnection();
            string sql = "SELECT * FROM items " +
                "WHERE items.ID = " + idItem + ";";

            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            MySqlDataReader objReader = objGet.ExecuteReader();
            while (objReader.Read())
            {
                item = new Item()
                {
                    ID = int.Parse(objReader.GetValue(0).ToString()),
                    Name = objReader.GetValue(1).ToString(),
                    Description = objReader.GetValue(2).ToString()
                };
            }


            objReader.Close();
            CloseConnection();


            if (item == null)
                return null;

            DoConnection();

            sql = "SELECT locations.ID, locations.Name, itemlocation.Amount FROM itemlocation, locations " +
                "WHERE itemlocation.LocationID = locations.ID " +
                "AND itemlocation.ItemID = " + item.ID + ";";

            List<Found> founds = new List<Found>();
            objGet = new MySqlCommand(sql, objMySqlCnx);
            objReader = objGet.ExecuteReader();
            while (objReader.Read())
            {
                founds.Add(
                    new Found()
                    {
                        Location = new Location(int.Parse(objReader.GetValue(0).ToString()), objReader.GetValue(1).ToString()),
                        Item = item,
                        Amount = int.Parse(objReader.GetValue(2).ToString())
                    });
            }

            objReader.Close();
            CloseConnection();

            for (int i = 0; i < founds.Count; i++)
                founds[i].Location = GetLocation(founds[i].Location.Name);

            item.Founds = founds;


            if (complete)
            {
                sql = "SELECT * FROM crafts " +
                "WHERE crafts.ItemProducedID = " + item.ID + ";";
                DoConnection();
                objGet = new MySqlCommand(sql, objMySqlCnx);
                objReader = objGet.ExecuteReader();
                List<int> newItems = null;
                Craft craft = null;
                while (objReader.Read())
                {
                    craft = new Craft()
                    {
                        ID = int.Parse(objReader.GetValue(0).ToString()),
                        ItemCrafted = item,
                        Amount = int.Parse(objReader.GetValue(4).ToString())
                    };
                    newItems = new List<int>()
                    {
                        int.Parse(objReader.GetValue(2).ToString()),
                        int.Parse(objReader.GetValue(3).ToString())
                    };
                }
                objReader.Close();
                CloseConnection();

                if (newItems != null)
                {
                    craft.ItemsNeeded = new List<Item>();
                    foreach (int id in newItems)
                        craft.ItemsNeeded.Add(GetItem(id, true));

                }
                if(craft != null)
                    item.Crafts = new List<Craft>(){craft};
            

            }

            return item;

        }
        public static Item GetRandomItem()
        {
            DoConnection();
            List<int> listIDitems = new List<int>();
            string sql = "SELECT items.ID FROM items;";
            MySqlCommand objSelect = new MySqlCommand(sql, objMySqlCnx);
            MySqlDataReader objReader = objSelect.ExecuteReader();
            while (objReader.Read())
                listIDitems.Add(int.Parse(objReader.GetValue(0).ToString()));

            objReader.Close();
            CloseConnection();

            return GetItem(listIDitems[Helper.rng.Next(0, listIDitems.Count)], false);

        }
        public static List<ServerUser> GetEveryUser(string idServer)
        {
            List<string> idUsers = new List<string>();
            DoConnection();
            string sql = "SELECT serverusers.DiscordID FROM serverusers WHERE ServerID = '" + idServer + "';";
            MySqlCommand objSelect = new MySqlCommand(sql, objMySqlCnx);
            MySqlDataReader objReader = objSelect.ExecuteReader();
            while (objReader.Read())
                idUsers.Add(objReader.GetValue(0).ToString());

            objReader.Close();
            CloseConnection();

            List<ServerUser> listUsers = new List<ServerUser>();
            for (int i = 0; i < idUsers.Count; i++)
                listUsers.Add(GetServerUser(idUsers[i], idServer));

            return listUsers;

        }
        public static Craft GetRandomCraft()
        {
            DoConnection();
            List<int> listIDcrafts = new List<int>();
            string sql = "SELECT crafts.ID FROM crafts;";
            MySqlCommand objSelect = new MySqlCommand(sql, objMySqlCnx);
            MySqlDataReader objReader = objSelect.ExecuteReader();
            while (objReader.Read())
                listIDcrafts.Add(int.Parse(objReader.GetValue(0).ToString()));

            objReader.Close();
            int craftChoose = listIDcrafts[Helper.rng.Next(0, listIDcrafts.Count)];

            sql = "SELECT crafts.ItemProducedID, crafts.ItemID1, crafts.ItemID2 FROM crafts WHERE crafts.ID = " + craftChoose + ";";
            objSelect = new MySqlCommand(sql, objMySqlCnx);
            objReader = objSelect.ExecuteReader();
            int itemProduced = 0;
            int item1 = 0;
            int item2 = 0;

            while (objReader.Read())
            {
                itemProduced = int.Parse(objReader.GetValue(0).ToString());
                item1 = int.Parse(objReader.GetValue(1).ToString());
                item2 = int.Parse(objReader.GetValue(2).ToString());
            }
            objReader.Close();
            CloseConnection();

            if (itemProduced == 0 || item1 == 0 || item2 == 0)
                return null;

            Craft craft = new Craft()
            {
                ID = craftChoose,
                ItemCrafted = GetItem(itemProduced, false),
                ItemsNeeded = new List<Item>(){
                    GetItem(item1, false),
                    GetItem(item2, false)
                }
            };

            return craft;
        }
        public static void UpdateUserSearch(User user)
        {
            string sql = "UPDATE users " +
                "SET users.DiscordName = @val1 " +
                " SET users.Gems = " + user.Gems +
                " SET users.Common = " + user.Common +
                " SET users.Uncommon = " + user.Uncommon +
                " SET users.Rare = " + user.Rare +
                " SET users.Epic = " + user.Epic +
                " WHERE users.DiscordID = '" + user.DiscordID + "';";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.Parameters.AddWithValue("@val1", user.DiscordName);
            objGet.Prepare();
            objGet.ExecuteNonQuery();
        }

        public static void UpdateUserQuiz(ServerUser serverUser)
        {
            string sql = "UPDATE serverusers " +
                "SET serverusers.DiscordServerName = @val1" +
                " SET serverusers.Points = " + serverUser.Points +
                " WHERE serverusers.DiscordID = '" + serverUser.DiscordID + "'" +
                " AND serverusers.ServerID = '" + serverUser.ServerID + "';";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.Parameters.AddWithValue("@val1", serverUser.ServerNameUser);
            objGet.Prepare();
            objGet.ExecuteNonQuery();
        }

        public static List<int> GetEverySearches()
        {
            DoConnection();
            string sql = "SELECT SUM(Common), SUM(Uncommon), SUM(Rare), SUM(Epic) FROM users;";
            MySqlCommand objSelect = new MySqlCommand(sql, objMySqlCnx);
            MySqlDataReader objReader = objSelect.ExecuteReader();
            objReader.Read();

            List<int> searches = new List<int>();
            for (int i = 0; i <= 3; i++)
                searches.Add(int.Parse(objReader.GetValue(i).ToString()));

            objReader.Close();

            sql = "SELECT COUNT(*) FROM possess;";
            objSelect = new MySqlCommand(sql, objMySqlCnx);
            objReader = objSelect.ExecuteReader();
            objReader.Read();
            searches.Add(int.Parse(objReader.GetValue(0).ToString()));

            objReader.Close();
            CloseConnection();

            return searches;
        }


        public static Location GetLocation(string name)
        {
            DoConnection();
            Location location = null;
            string sql = "SELECT * FROM locations WHERE Name LIKE @val1 OR Slang1 LIKE @val1 OR Slang2 LIKE @val1;";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.Parameters.AddWithValue("@val1", name);
            objGet.Prepare();
            MySqlDataReader objReader = objGet.ExecuteReader();
            while (objReader.Read())
            {
                int ID = int.Parse(objReader.GetValue(0).ToString());
                string Name = objReader.GetValue(1).ToString();
                location = new Location(ID, name);
            }

            objReader.Close();

            // If no location found
            if (location == null)
            {
                CloseConnection();
                return location;
            }

            sql = "SELECT items.ID, items.Name, itemLocation.Amount" +
                " FROM itemlocation, items" +
                " WHERE itemlocation.LocationID = " + location.ID + 
                " AND items.ID = itemLocation.ItemID;";

            objGet = new MySqlCommand(sql, objMySqlCnx);
            objReader = objGet.ExecuteReader();

            List<Found> founds = new List<Found>();
            while (objReader.Read())
            {
                int itemID = int.Parse(objReader.GetValue(0).ToString());
                string itemName = objReader.GetValue(1).ToString();
                int amount = int.Parse(objReader.GetValue(2).ToString());
                Item item = new Item
                {
                    ID = itemID,
                    Name = itemName
                };

                Found found = new Found
                {
                    Item = item,
                    Location = location,
                    Amount = amount
                };

                founds.Add(found);
            }

            objReader.Close();
            CloseConnection();

            location.Founds = founds;

            return location;
        }

        public static void AddLegendary(string idUser, Legendary legendary)
        {

        }

        public static void AddSlang(int idItem, string slang)
        {
            DoConnection();
            string sql = "INSERT INTO slangs(ID, ItemID, Slang) VALUES (NULL, '" + idItem + "', '" + slang + "');";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.ExecuteNonQuery();
            CloseConnection();
        }

        private static List<List<object>> Read(MySqlDataReader objReader)
        {
            List<List<object>> everyItems = new List<List<object>>();
            while(objReader.Read())
            {
                List<object> item = new List<object>();

                for(int i = 0; i < objReader.FieldCount; i++)
                    item.Add(objReader.GetValue(i));

                everyItems.Add(item);
            }
            objReader.Close();
            return everyItems;
        }

        

    }
}
