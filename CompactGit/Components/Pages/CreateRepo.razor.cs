using CompactGit.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompactGit.Components.Pages
{
    public partial class CreateRepo : ComponentBase
    {
        private string RepositoryName { get; set; } = "";
        private string RepositoryDescription { get; set; } = "";
        private bool AddReadme { get; set; } = true;
        private bool ChooseGitignore { get; set; } = true;
        private bool IsPublic { get; set; } = false;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public ICookie Cookie { get; set; } = default!;

        private async Task CreateRepositoryAsync()
        {
            string loginCookie = await Cookie.GetValue("login");

            List<RepoData>? list = AddRepo(loginCookie);

            if (list == null)
            {
                return;
            }

            WriteRepo(list, loginCookie);

            Cmd.CallCmd("git init --bare " + RepositoryName);

            NavigationManager.NavigateTo("/" + loginCookie + "/" + RepositoryName);
        }

        private void WriteRepo(List<RepoData>? list, string userUrl)
        {
            if (!Directory.Exists("repos"))
            {
                Directory.CreateDirectory("repos");
            }

            if (!File.Exists(Path.Combine("repos", userUrl)))
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine("repos", userUrl)))
                {
                    sw.Write("[]");
                }
            }

            using (StreamWriter sw = new StreamWriter(Path.Combine("repos", userUrl)))
            {
                string json = JsonSerializer.Serialize(list);

                sw.Write(json);
            }
        }

        private List<RepoData>? AddRepo(string userUrl)
        {
            if (!Directory.Exists("repos"))
            {
                Directory.CreateDirectory("repos");
            }

            if (!File.Exists(Path.Combine("repos", userUrl)))
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine("repos", userUrl)))
                {
                    sw.Write("[]");
                }
            }

            using (StreamReader sr = new StreamReader(Path.Combine("repos", userUrl)))
            {
                string json = sr.ReadToEnd();

                try
                {
                    List<RepoData> list = JsonSerializer.Deserialize<List<RepoData>>(json)!;

                    if (list.Find(x=>x.RepoName == RepositoryName) != null)
                    {
                        return null;
                    }
                    else
                    {
                        RepoData data = new RepoData()
                        {
                            UserId = userUrl,
                            RepoName = RepositoryName,
                            Description = RepositoryDescription,
                            IsPublic = IsPublic,
                        };

                        list.Add(data);

                        return list;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}