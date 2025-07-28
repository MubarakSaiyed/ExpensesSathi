using System.Text.Json;
using ExpensesSathi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesSathi.Services
{
    public class UserService
    {
        private readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "users.json");
        private List<UserModel> Users;

        public UserService()
        {
            LoadUsers();
        }

        // Ensure users are loaded correctly
        private async void LoadUsers()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var jsonData = await File.ReadAllTextAsync(FilePath);
                    Users = JsonSerializer.Deserialize<List<UserModel>>(jsonData) ?? new List<UserModel>();
                }
                else
                {
                    Users = new List<UserModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
                Users = new List<UserModel>(); // Ensure Users list is initialized in case of error
            }
        }

        // Check user authentication
        public bool AuthenticateUser(string username, string password)
        {
            LoadUsers(); // Ensure users are loaded before authentication

            // Check if the user exists and if the password matches
            return Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
        }

        public async Task<bool> RegisterUserAsync(UserModel user)
        {
            try
            {
                // Ensure users are loaded before adding new user
                LoadUsers();

                if (Users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                {
                    // Username already exists
                    return false;
                }

                Users.Add(user);

                // Save updated list to file asynchronously
                var jsonData = JsonSerializer.Serialize(Users);
                await File.WriteAllTextAsync(FilePath, jsonData);

                return true;
            }
            catch (Exception ex)
            {
                // Handle any file-related errors
                Console.WriteLine($"Error saving user: {ex.Message}");
                return false;
            }
        }
    }
}
