using CompactGit.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CompactGit.Components.Pages
{
    public partial class MyRepo : ComponentBase
    {
        private bool showTypeDropdown = false;
        private string FindInput { get; set; } = "";
        private List<RepoData> RepoList { get; set; } = new List<RepoData>();
        private List<RepoData> FilteredRepoList { get; set; } = new List<RepoData>();

        [Parameter]
        public string UserUrl { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public ICookie Cookie { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            List<RepoData> Repos = GetRepos(UserUrl);
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

        private List<RepoData> GetRepos(string userUrl)
        {

        }
    }
}
