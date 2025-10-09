using cst350_clc.Filters;
using cst350_clc.Models.User;
using cst350_clc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace cst350_clc.Controllers
{
    public class UserController : Controller
    {
        static UserDAO users = new UserDAO();

        /// <summary>
        /// Routes to Login Page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            // Index is the Login page
            return View();
        }

        /// <summary>
        /// Routes to Registration Page
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        /// <summary>
        /// Processes the registration of a new user, either returning them back to the registration with an error (if failed)
        /// or sending them to the login page if successful
        /// </summary>
        /// <param name="registerViewModel"></param>
        /// <returns></returns>
        public IActionResult ProcessRegister(RegisterViewModel registerViewModel)
        {
            // Define a new user based on user input
            UserModel newUser = new UserModel(); // Salting is auto generated
            newUser.Username = registerViewModel.Username;
            newUser.SetPassword(registerViewModel.Password);
            
            // LINQ expression to safely default to an empty string if no groups are selected.
            newUser.Groups = string.Join(",",
                registerViewModel.Groups?
                    .Where(g => g.IsSelected)
                    .Select(g => g.GroupName)
                ?? Array.Empty<string>()
            ).Trim(',');

            // FIXME: TESTONLY
            newUser.Groups = "USER";

            newUser.FirstName = registerViewModel.FirstName;
            newUser.LastName = registerViewModel.LastName;
            newUser.Email = registerViewModel.Email;
            newUser.Age = registerViewModel.Age;
            newUser.State = registerViewModel.State;
            
            // Add the newly formed user
            var result = users.AddUser(newUser);
           
            if (result >= 0) // Any positive integer returned is a record
            { // Successful register case,
                ViewBag.Message = "Thanks for Registering! Please Log In."; // Thanks message
                return View("Index"); // Send to login page
            }

            // Failed register case
            // Process uniquity checks (caught in error handling by DAO object)
            if (result == -1 || result <= -4) //Failure to add or unknown uniquity check failure
                ViewBag.Error = "An unknown error has occurred. Refresh the page and try again.";
            else if (result == -2) // Username uniquity check failure
                ViewBag.Error = "Username already taken.";
            else if (result == -3)
                ViewBag.Error = "Email already in use.";
            
           // passing in the old model to preserve the invalid username to help the user in not putting the same username in a second time
            return View("Register", registerViewModel); 
        }

        /// <summary>
        /// Processes the login and either kicks back to the login screen with an error message or returns
        /// the signed-in user to the home screen (with a personalized message)
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        public IActionResult ProcessLogin(LoginViewModel loginViewModel)
        {
            // check the login models against database credentials
            var result = users.CheckCredentials(loginViewModel.Username, loginViewModel.Password);
            if (result != -1) // Successful login case
            {
                // Find the corresponding user
                UserModel user = users.GetUserById(result);

                // Store user info in session
                HttpContext.Session.SetInt32("User", user.Id);
                HttpContext.Session.SetString("Username", user.Username);

                // Send back to the welcome page
                return RedirectToAction("Index", "Home");
            }

            // Failed login case - Returns to Login page with error warning
            ViewBag.Error = "Invalid Username or Password";
            return View("Index");
        }

        /// <summary>
        /// Logout method clears Session data and returns session state to fresh
        /// </summary>
        /// <returns></returns>
        [SessionCheckFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User"); // Remove the user info
            HttpContext.Session.Clear(); // Clear the session
            return RedirectToAction("Index", "Home"); // Return to the Home page
        }
    }
}
