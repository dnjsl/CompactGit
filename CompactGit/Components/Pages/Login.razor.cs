using CompactGit.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CompactGit.Components.Pages
{
    public partial class Login : ComponentBase
    {
        public string Id { get; set; } = "";
        public string Passwd { get; set; } = "";
        public string UserUrl { get; set; } = "";
        public string ErrorMsg { get; set; } = "";

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public ICookie Cookie { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            string loginCookie = await Cookie.GetValue("login");

            if (!string.IsNullOrEmpty(loginCookie))
            {
                NavigationManager.NavigateTo("/user/" + loginCookie);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private string PassHashing(string pass)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(pass)));
        }

        private async Task LoginButtonClick()
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Passwd))
            {
                ErrorMsg = "ID or Password is empty";

                return;
            }

            try
            {
                UserData? user = CatchUser(Id, Passwd);

                if (user != null)
                {
                    UserUrl = user.Id;
                    await Cookie.SetValue("login", UserUrl);
                    NavigationManager.NavigateTo("/user/" + UserUrl);
                }
                else
                {
                    ErrorMsg = "Invalid ID or Password";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                ErrorMsg = "An error occurred during login.";
            }
        }

        private UserData? CatchUser(string id, string pass)
        {
            if (!Directory.Exists("users"))
            {
                Directory.CreateDirectory("users");
            }

            if (File.Exists(Path.Combine("users", id)))
            {
                using (StreamReader sr = new StreamReader(Path.Combine("users", id)))
                {
                    string json = sr.ReadToEnd();

                    try
                    {
                        UserData user = JsonSerializer.Deserialize<UserData>(json)!;

                        if (user.Pw == PassHashing(pass))
                        {
                            return user;
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        private void SignUpButtonClick()
        {
            NavigationManager.NavigateTo("/sign-up");
        }
    }
}
