using CompactGit.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CompactGit.Components.Pages
{
    public partial class SignUp : ComponentBase
    {
        private string id = "";
        private string passwd = "";
        private string passwdRe = "";
        private string nickname = "";
        private string email = "";

        public string Id { get => id; set { id = Regex.Replace(value, "[^0-9a-zA-Z._-]", ""); } }
        public string Passwd { get => passwd; set { passwd = Regex.Replace(value, "[^\x20-\x7E]", ""); } }
        public string PasswdRe { get => passwdRe; set { passwdRe = Regex.Replace(value, "[^\x20 -\x7E]", ""); } }
        public string Nickname { get => nickname; set { nickname = Regex.Replace(value, "[^0-9a-zA-Z._-]", ""); } }
        public string Email { get => email; set { email = Regex.Replace(value, "[^0-9a-zA-Z._@-]", ""); } }
        public string ErrorMsg { get; set; } = "";

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        private string PassHashing(string pass)
        {
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(pass)));
        }

        private void SignUpButtonClick(MouseEventArgs e)
        {
            if (Passwd != PasswdRe || Id == "" || Passwd == "" || Nickname == "" || Email == "")
            {
                ErrorMsg = "There are space that's not filled in";
                return;
            }

            UserData? user = CreateUser(Id);

            if (user == null)
            {
                ErrorMsg = "There are space that's not filled in";
                return;
            }

            NavigationManager.NavigateTo("/");
        }

        private UserData? CreateUser(string id)
        {
            if (Directory.Exists("users"))
            {
                Directory.CreateDirectory("users");
            }

            if (!File.Exists(Path.Combine("users", id)))
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine("users", id)))
                {
                    try
                    {
                        UserData user = new UserData()
                        {
                            Id = Id,
                            Pw = PassHashing(Passwd),
                            Nickname = Nickname,
                            Email = Email,
                        };

                        string json = JsonSerializer.Serialize(user);

                        sw.Write(json);

                        return user;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            return null;
        }
    }
}
