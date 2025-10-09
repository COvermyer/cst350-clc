using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace cst350_clc.Models.User
{
    /// <summary>
    /// DAO class to pull UserModel data from DB
    /// </summary>
    public class UserDAO : IUserManager
    {
        /// <summary>
        /// CONNECTION_STRING constant will hold MySql.Data connection string data. Bad practice to put it here, but too bad.
        /// </summary>
        private const string CONNECTION_STRING = "datasource=localhost;port=8889;uid=root;pwd=root;database=minesweeperapp;";

        /// <summary>
        /// Adds a user to DB
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int AddUser(UserModel user)
        {
            using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                string sql = "INSERT INTO `users` (username, passwordHash, saltHexString, groups, first_name, last_name, age, email, state) " +
                    "VALUES (@username, @passwordHash, @saltHexString, @groups, @firstName, @lastName, @age, @email, @state)";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Format data into cmd statement
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@saltHexString", Convert.ToHexString(user.Salt));
                    cmd.Parameters.AddWithValue("@groups", user.Groups);
                    cmd.Parameters.AddWithValue("@firstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", user.LastName);
                    cmd.Parameters.AddWithValue("@age", user.Age);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@state", user.State);

                    // Execute Insert
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return Convert.ToInt32(cmd.LastInsertedId); // Return the ID of the new User
                    } catch (MySqlException ex)
                    { // if cmd cannot be executed, return -1 for failure
                        if (ex.Number == 1062) 
                        {// if caused by a uniquity check failure
                            if (ex.Message.Contains("username_UNIQUE"))
                                return -2; // Username uniquity failure response
                            else if (ex.Message.Contains("email_UNIQUE"))
                                return -3; // email uniquity failure response
                            else 
                                return -4; // unknown uniquity failure response (should be unreachable, in theory)
						}
                        else 
                        {   // Exceptions not caused by Uniquity checks throw general -1 failure response
							return -1;
						}
                    }
                }
            }
        }

        /// <summary>
        /// Checks a given set of credentials against either all users or a provided selection of users
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public int CheckCredentials(string username, string password, List<UserModel> users = null)
        {
            // Nullable check. If param users is not defined, pull all records.
            // This allows dev to check credentials against a selection of users if desired
            if (users == null)
                users = GetAllUsers();

            // Check each user in the collection
            foreach (UserModel user in users)
            {
                // Validate a username match without bothering to check passwords until username match is found
                if (!string.IsNullOrEmpty(user.Username) && username.ToUpper().Equals(user.Username.ToUpper())) // IGNORES CASING
                {
                    // If username match is found, validate password
                    if (!string.IsNullOrEmpty(password) && user.ValidatePassword(password))
                        return user.Id; // return the user ID if password can be validated
                }
            }

            return -1;
        }

        /// <summary>
        /// Deletes a user from DB by id
        /// </summary>
        /// <param name="id"></param>
        public bool DeleteUser(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING)) // prep disposal
            {
                conn.Open(); // Open a MySQL server connection
                String sql = "DELETE FROM `users` WHERE id = @id"; // Define the DELETE statement
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id); // Format the statement params

                    try
                    { // Attempt to execute query
                        cmd.ExecuteNonQuery(); // Execute the statement
                        return true; // return true if execution was successful
                    }
                    catch (MySqlException)
                    { // return false if cmd executiojn failed
                        return false;
                    }

                    
                }
            }
        }

        /// <summary>
        /// Deletes a user from DB based on a given user model, passes through the ID of the userModel
        /// </summary>
        /// <param name="user"></param>
        public bool DeleteUser(UserModel user)
        {
            // Call the delete method by ID for the given UserModel
            return DeleteUser(user.Id);
        }

        /// <summary>
        /// Returns all users in DB
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAllUsers()
        {
            // In-memory list of users
            List<UserModel> users = new List<UserModel>();

            using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING)) // Prep connection for disposal
            {
                conn.Open(); // Open a MySQL server connection
                string sql = "SELECT * FROM `users`"; // Define query
                using (MySqlDataReader reader = new MySqlCommand(sql, conn).ExecuteReader()) // Execute Query and push results into reader
                {
                    while (reader.Read()) // for each returned entity:
                    {
                        // Cast data to conform to model
                        UserModel user = new UserModel()
                        {
                            Id = reader.GetInt32("id"),
                            Username = reader.GetString("Username"),
                            PasswordHash = reader.GetString("passwordHash"),
                            Salt = Convert.FromHexString(reader.GetString("saltHexString")),
                            Groups = reader.GetString("groups"),
                            FirstName = reader.GetString("first_name"),
                            LastName = reader.GetString("last_name"),
                            Age = reader.GetInt32("age"),
                            Email = reader.GetString("email"),
                            State = reader.GetString("state")
                        };

                        // Add the new user to the in memory list
                        users.Add(user);
                    }
                }
            }

            // Return the in memory list when finished, all resources should be disposed of properly
            return users;
        }

        /// <summary>
        /// Returns a user from DB by ID param
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserModel GetUserById(int id)
        {
            string sql = "SELECT * FROM `users` WHERE id = @id";

            using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING)) // prep disposal
            using (MySqlCommand cmd = new MySqlCommand(sql, conn)) // prep command
            {
                conn.Open(); // Open MySQL server connection
                cmd.Parameters.AddWithValue("@id", id); // Format in param to query
                try
                { // attempt to execute query into reader
                    using (MySqlDataReader reader = cmd.ExecuteReader()) // Execute Query and push results into reader
                    {
                        if (reader.Read()) // If a result is present, format it to a model
                            return new UserModel() // ID field is PK and must be unique, so query can only return one entry
                            { // FIXME: Nullable Type Safety needed
                                Id = reader.GetInt32("id"),
                                Username = reader.GetString("Username"),
                                PasswordHash = reader.GetString("passwordHash"),
                                Salt = Convert.FromHexString(reader.GetString("saltHexString")),
                                Groups = reader.GetString("groups"),
                                FirstName = reader.GetString("first_name"),
                                LastName = reader.GetString("last_name"),
                                Age = reader.GetInt32("age"),
                                Email = reader.GetString("email"),
                                State = reader.GetString("state")
                            };
                    }
                } catch (MySqlException)
                { // return null if execution of cmd fails
                    return null;
                }
            }
            return null; // if no entity is found, release resources and return null
        }

        //TODO: Didn't get around to it yet, no use case in the code yet anyways... -COVERMYER
        public void UpdateUser(UserModel user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a count of entities in the database
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            string sql = "SELECT COUNT FROM `users`";
            using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                conn.Open();
                try
                { // Attempt to run query
                    return Convert.ToInt32(cmd.ExecuteScalar()); // return [1,1] data as integer result
                }
                catch (MySqlException)
                { // return -1 if query fails (should only fail if the table doesn't exist)
                    return -1;
                }
                
            }
        }
    }
}
