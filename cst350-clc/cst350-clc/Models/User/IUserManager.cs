namespace cst350_clc.Models.User
{
    /// <summary>
    /// Defines the CRUS usage contract of any collection or DAO objects for UserModel
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Method should return a List of all UserModels in the collection
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAllUsers();

        /// <summary>
        /// Gets a UserModel by ID
        /// </summary>
        /// <param name="id">ID of the user to search</param>
        /// <returns></returns>
        public UserModel GetUserById(int id);

        /// <summary>
        /// Adds a UserModel to collection
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int AddUser(UserModel user);

        /// <summary>
        /// Updates an existing UserModel within the collection
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(UserModel user);

        /// <summary>
        /// Delete a user by ID
        /// </summary>
        /// <param name="id"></param>
        public bool DeleteUser(int id);

        /// <summary>
        /// Delete a User given the UserModel to delete
        /// </summary>
        /// <param name="user"></param>
        public bool DeleteUser(UserModel user);

        /// <summary>
        /// Check a given set of credentaials against the collection and return the matching ID or -1 if not found
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int CheckCredentials(string username, string password, List<UserModel> users = null);

        /// <summary>
        /// Returns a Count of users in collection
        /// </summary>
        /// <returns></returns>
        public int Count();
    }
}
