using System.Security.Cryptography;
using System.Text;

namespace cst350_clc.Models.User
{
    /// <summary>
    /// Represents a User and their login information
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Id for serialization and uniquity; each Id must be unique to conform to DB
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Username must also be unique
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Stores the Password as a Hashed value
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Stores the Salting value as an array of bytes
        /// </summary>
        public byte[] Salt { get; set; }

        /// <summary>
        /// Access groups user belongs to
        /// </summary>
        public string Groups { get; set; }

        // ===== Arbitrary Data, Not Important =====
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string State { get; set; }


        // Hashing
        private const int KeySize = 64;
        private const int Iterations = 350000;
        private readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        /// <summary>
        /// Default Constructor, will generate new Salting value
        /// </summary>
        public UserModel()
        {
            // When importing from an existing source, this should be overwritten with the existing Salt value
            // Otherwise, password will not properly de-hash
            Salt = RandomNumberGenerator.GetBytes(KeySize); // Create a new, unique Salting set for each UserModel
        }

        /// <summary>
        /// Constructor for ID wrapping
        /// </summary>
        /// <param name="id"></param>
        public UserModel(int id)
        {
            Salt = RandomNumberGenerator.GetBytes(KeySize); // Create a new, unique Salting set for each UserModel
            this.Id = id;
        }

        /// <summary>
        /// Copy constructor, able to fill class fields
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="passwordHash"></param>
        /// <param name="salt"></param>
        public UserModel(int id, string username, string passwordHash, byte[] salt)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            Salt = salt;
        }

        /// <summary>
        /// Hashes a password with the UserModel's Salt value
        /// </summary>
        /// <param name="password">The pasword to Hash with salting</param>
        /// <returns></returns>
        private string HashPassword(string password)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                Salt,
                Iterations,
                hashAlgorithm,
                KeySize);

            return Convert.ToHexString(hash);
        }

        /// <summary>
        /// Takes a plaintext password and hashes it for storage within the UserModel
        /// </summary>
        /// <param name="password"></param>
        public void SetPassword(string password)
        {
            PasswordHash = HashPassword(password);
        }

        /// <summary>
        /// Check if an input password, when hashed, matches the existing hashed password for validation
        /// </summary>
        /// <param name="plain">plaintext of the input password</param>
        /// <returns></returns>
        public bool ValidatePassword(string plain)
        {
            // Hash the incoming password using this user models Salt value.
            // Compare the newly hashed incoming password to the existing password hash
            // If they match, return true.
            // If they don't match, return false without revealing what this Hash contains.
            return (PasswordHash == HashPassword(plain)) ? true : false;
        }
    }
}
