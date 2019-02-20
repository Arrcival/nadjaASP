using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nadja.Command;

namespace Nadja.Models
{
    public static class Dal
    {
        private static MySqlConnection objMySqlCnx;

        public static void DoConnection()
        {
            // Database connection
            string strConnect = ConfigurationManager.AppSettings["DatabaseConnectionString"];
            objMySqlCnx = new MySqlConnection(strConnect);
            objMySqlCnx.Open();
        }
        public static void CloseConnection()
        {
            objMySqlCnx.Close();
        }

        public static User GetUser(string idUser)
        {
            User user = null;
            string sql = @"SELECT users.ID, users.DiscordName, users.Gems, users.Common, users.Uncommon, users.Rare, users.Epic 
                FROM users
                WHERE DiscordID = @val1;";

            MySqlCommand objSelect = new MySqlCommand(sql, objMySqlCnx);
            objSelect.Parameters.AddWithValue("@val1", idUser);
            objSelect.Prepare();
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

            if (ID == -1)
                return null;
            

            user = new User(ID, idUser, nameDiscord, gems, common, uncommon, rare, epic, new List<Legendary>());

            List<Legendary> legendaries = new List<Legendary>();

            sql = "SELECT possess.LegendaryID, legendaries.Name FROM possess, legendaries WHERE possess.LegendaryID = legendaries.ID AND possess.UserID = @val1;";
            
            objSelect = new MySqlCommand(sql, objMySqlCnx);
            objSelect.Parameters.AddWithValue("@val1", user.ID);
            objSelect.Prepare();
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

            return user;

        }

        public static ServerUser GetServerUser(string idUser, string idServer)
        {
            User user = GetUser(idUser);

            if (user == null)
                return null;

            ServerUser serverUser = new ServerUser(user);
            
            string sql = @"SELECT serverusers.DiscordServerName, serverusers.Points FROM serverusers
                WHERE serverusers.DiscordID = @val1
                AND serverusers.ServerID = @val2;";

            MySqlCommand objSelect = new MySqlCommand(sql, objMySqlCnx);
            objSelect.Parameters.AddWithValue("@val1", idUser);
            objSelect.Parameters.AddWithValue("@val2", idServer);
            objSelect.Prepare();
            MySqlDataReader objReader = objSelect.ExecuteReader();
            string serverName = null;
            int points = 0;
            while (objReader.Read())
            {
                serverName = objReader.GetValue(0).ToString();
                points = int.Parse(objReader.GetValue(1).ToString());
            }
            objReader.Close();

            if (serverName == null)
                return null;

            serverUser.ServerNameUser = serverName;
            serverUser.Points = points;
            serverUser.ServerID = idServer;

            return serverUser;
        }

        public static string GetIdUser(string name)
        {
            string sql = @"SELECT users.DiscordID FROM users
                WHERE users.DiscordName = @val1;";
            MySqlCommand objSelect = new MySqlCommand(sql, objMySqlCnx);
            objSelect.Parameters.AddWithValue("@val1", name);
            objSelect.Prepare();
            MySqlDataReader objReader = objSelect.ExecuteReader();
            string idUser = null;
            while (objReader.Read())
                idUser = objReader.GetValue(0).ToString();

            objReader.Close();

            return idUser;
        }

        public static int GetIDItem(string name)
        {
            int id = -1;
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

            return id;

        }

        public static Item GetItem(int idItem, bool complete)
        {
            Item item = null;
            string sql = @"SELECT * FROM items
                WHERE items.ID = @val1;";

            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.Parameters.AddWithValue("@val1", idItem);
            objGet.Prepare();
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


            if (item == null)
                return null;
            
            sql = @"SELECT locations.ID, locations.Name, itemlocation.Amount FROM itemlocation, locations
                WHERE itemlocation.LocationID = locations.ID
                AND itemlocation.ItemID = @val1;";

            List<Found> founds = new List<Found>();
            objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.Parameters.AddWithValue("@val1", item.ID);
            objGet.Prepare();
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

            for (int i = 0; i < founds.Count; i++)
                founds[i].Location = GetLocation(founds[i].Location.Name);

            item.Founds = founds;

            
            List<string> slangs = new List<string>();

            sql = @"SELECT Slang FROM slangs  
                WHERE ItemID = @val1;";
            objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.Parameters.AddWithValue("@val1", item.ID);
            objGet.Prepare();
            objReader = objGet.ExecuteReader();
            while (objReader.Read())
            {
                slangs.Add(objReader.GetValue(0).ToString());
            }

            objReader.Close();
            item.Slangs = slangs;
            


            if (complete)
            {
                sql = @"SELECT * FROM crafts
                    WHERE crafts.ItemProducedID = @val1;";
                objGet = new MySqlCommand(sql, objMySqlCnx);
                objGet.Parameters.AddWithValue("@val1", item.ID);
                objGet.Prepare();
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

                if (newItems != null)
                {
                    craft.ItemsNeeded = new List<Item>();
                    foreach (int id in newItems)
                        craft.ItemsNeeded.Add(GetItem(id, true));

                }
                if (craft != null)
                    item.Craft = craft;
            

            }

            return item;

        }
        public static Item GetRandomItem()
        {
            List<int> listIDitems = new List<int>();
            string sql = "SELECT items.ID FROM items;";
            MySqlCommand objSelect = new MySqlCommand(sql, objMySqlCnx);
            MySqlDataReader objReader = objSelect.ExecuteReader();
            while (objReader.Read())
                listIDitems.Add(int.Parse(objReader.GetValue(0).ToString()));

            objReader.Close();

            return GetItem(listIDitems[Helper.rng.Next(0, listIDitems.Count)], false);

        }
        public static List<ServerUser> GetEveryUser(string idServer)
        {
            List<string> idUsers = new List<string>();
            string sql = "SELECT serverusers.DiscordID FROM serverusers WHERE ServerID = '" + idServer + "';";
            MySqlCommand objSelect = new MySqlCommand(sql, objMySqlCnx);
            MySqlDataReader objReader = objSelect.ExecuteReader();
            while (objReader.Read())
                idUsers.Add(objReader.GetValue(0).ToString());

            objReader.Close();

            List<ServerUser> listUsers = new List<ServerUser>();
            for (int i = 0; i < idUsers.Count; i++)
                listUsers.Add(GetServerUser(idUsers[i], idServer));

            return listUsers;

        }
        public static Craft GetRandomCraft()
        {
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
            string sql = "UPDATE users" +
                " SET users.DiscordName = @val1" +
                ", users.Gems = " + user.Gems +
                ", users.Common = " + user.Common +
                ", users.Uncommon = " + user.Uncommon +
                ", users.Rare = " + user.Rare +
                ", users.Epic = " + user.Epic +
                " WHERE users.ID = '" + user.ID + "';";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.Parameters.AddWithValue("@val1", user.DiscordName);
            objGet.Prepare();
            objGet.ExecuteNonQuery();
        }

        public static void UpdateUserQuiz(ServerUser serverUser)
        {
            string sql = "UPDATE serverusers " +
                " SET serverusers.DiscordServerName = @val1" +
                ", serverusers.Points = " + serverUser.Points +
                " WHERE serverusers.DiscordID = '" + serverUser.DiscordID + "'" +
                " AND serverusers.ServerID = '" + serverUser.ServerID + "';";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.Parameters.AddWithValue("@val1", serverUser.ServerNameUser);
            objGet.Prepare();
            objGet.ExecuteNonQuery();
        }

        public static List<double> GetEverySearches()
        {
            string sql = "SELECT SUM(Common), SUM(Uncommon), SUM(Rare), SUM(Epic) FROM users;";
            MySqlCommand objSelect = new MySqlCommand(sql, objMySqlCnx);
            MySqlDataReader objReader = objSelect.ExecuteReader();
            objReader.Read();

            List<double> searches = new List<double>();
            for (int i = 0; i <= 3; i++)
                searches.Add(int.Parse(objReader.GetValue(i).ToString()));

            objReader.Close();

            sql = "SELECT COUNT(*) FROM possess;";
            objSelect = new MySqlCommand(sql, objMySqlCnx);
            objReader = objSelect.ExecuteReader();
            while(objReader.Read())
                searches.Add(int.Parse(objReader.GetValue(0).ToString()));

            objReader.Close();

            return searches;
        }


        public static Location GetLocation(string name)
        {
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
                return location;

            sql = "SELECT items.ID, items.Name, itemlocation.Amount" +
                " FROM itemlocation, items" +
                " WHERE itemlocation.LocationID = " + location.ID + 
                " AND items.ID = itemlocation.ItemID;";

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

            location.Founds = founds;

            return location;
        }

        public static Location GetLocationFromInt(int locationID)
        {
            string sql = "SELECT Name FROM locations WHERE ID = @val1;";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.Parameters.AddWithValue("@val1", locationID);
            objGet.Prepare();
            MySqlDataReader objReader = objGet.ExecuteReader();
            string name = null;
            while (objReader.Read())
            {
                name = objReader.GetValue(0).ToString();
            }
            objReader.Close();

            if (name == null)
                return null;
            else
                return GetLocation(name);
        }

        


        public static List<Legendary> GetEveryLegendaries()
        {
            List<Legendary> legendaries = new List<Legendary>();
            string sql = "SELECT * FROM legendaries;";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            MySqlDataReader objReader = objGet.ExecuteReader();
            while(objReader.Read())
            {
                Legendary legendary = new Legendary
                {
                    ID = int.Parse(objReader.GetValue(0).ToString()),
                    Name = objReader.GetValue(1).ToString()
                };
                legendaries.Add(legendary);
            }

            objReader.Close();
            return legendaries;
            
        }

        public static int GetEveryPossess()
        {
            int count = -1;
            string sql = "SELECT COUNT(*) FROM possess;";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            MySqlDataReader objReader = objGet.ExecuteReader();
            while (objReader.Read())
            {
                count = int.Parse(objReader.GetValue(0).ToString());
            }

            objReader.Close();
            return count;

        }

        public static void CreateUser(string idUser, string discordName)
        {
            string sql = "INSERT INTO users(ID, DiscordID, DiscordName, Gems, Common, Uncommon, Rare, Epic) VALUES (NULL, '" + idUser + "', '" + discordName + "', 0, 0, 0, 0, 0);";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.ExecuteNonQuery();
        }

        public static void CreateServerUser(User user, string serverID)
        {
            string sql = "INSERT INTO serverusers(ID, DiscordID, ServerID, DiscordServerName, Points) VALUES (NULL, '" + user.DiscordID + "', '" + serverID + "', '" + user.DiscordName + "', 0);";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.ExecuteNonQuery();
        }

        public static void AddLegendary(User user, Legendary legendary)
        {
            string sql = "INSERT INTO possess(ID, UserID, LegendaryID) VALUES (NULL, '" + user.ID + "', '" + legendary.ID + "');";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.ExecuteNonQuery();
        }

        public static void AddSlang(int idItem, string slang)
        {
            string sql = "INSERT INTO slangs(ID, ItemID, Slang) VALUES (NULL, '" + idItem + "', '" + slang + "');";
            MySqlCommand objGet = new MySqlCommand(sql, objMySqlCnx);
            objGet.ExecuteNonQuery();
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
