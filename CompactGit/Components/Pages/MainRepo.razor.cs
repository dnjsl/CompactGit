using CompactGit.Utils;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace CompactGit.Components.Pages
{
    public partial class MainRepo : ComponentBase
    {
        [Parameter]
        public string UserUrl { get; set; } = default!;

        [Parameter]
        public string RepoUrl { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public ICookie Cookie { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await Initialize();

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task Initialize()
        {
            string user = await Cookie.GetValue("login");

            if (ValidRepo() && (user == UserUrl || IsPublicRepo()))
            {
            }
            else
            {
                NavigationManager.NavigateTo("/acc-din");
            }
        }

        private bool ValidRepo()
        {
            if (!File.Exists("repos/" + UserUrl))
            {
                return false;
            }

            using (StreamReader sr = new StreamReader("repos/" + UserUrl))
            {
                string json = sr.ReadToEnd();
                List<RepoData> repos = JsonSerializer.Deserialize<List<RepoData>>(json)!;

                RepoData data = repos.Find(x => x.RepoName == RepoUrl)!;

                return data != null;
            }
        }

        private bool IsPublicRepo()
        {
            using (StreamReader sr = new StreamReader("repos/" + UserUrl))
            {
                string json = sr.ReadToEnd();
                List<RepoData> repos = JsonSerializer.Deserialize<List<RepoData>>(json)!;

                RepoData data = repos.Find(x => x.RepoName == RepoUrl)!;

                return data.IsPublic;
            }
        }
    }
}