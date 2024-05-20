using CompactGit.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CompactGit.Components.Pages
{
    public partial class MyRepo : ComponentBase
    {
        private bool showTypeDropdown = false;
        private string FindInput { get; set; } = "";
        private List<RepoData> FilteredRepoList { get; set; } = new List<RepoData>();
        private List<RepoData> RepoList { get; set; } = new List<RepoData>();

        [Parameter]
        public string UserUrl { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public ICookie Cookie { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            RepoList = GetRepos(UserUrl) ?? new List<RepoData>();
            FilteredRepoList = new List<RepoData>(RepoList);

            await base.OnInitializedAsync();
        }

        private void NewButtonClickAsync()
        {
            NavigationManager.NavigateTo("/create-repo");
        }

        private void ToggleTypeDropdown()
        {
            showTypeDropdown = !showTypeDropdown;
        }

        private void FilterRepos(bool isPublic)
        {
            FilteredRepoList = RepoList.Where(repo => repo.IsPublic == isPublic).ToList();
            showTypeDropdown = false;
        }

        private void SettingsButtonClickAsync()
        {
            NavigationManager.NavigateTo("/settings/" + UserUrl);
        }

        private void RepositoryClickAsync(string name)
        {
            NavigationManager.NavigateTo(UserUrl + "/" + name);
        }

        private List<RepoData>? GetRepos(string userUrl)
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

                    return list;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
