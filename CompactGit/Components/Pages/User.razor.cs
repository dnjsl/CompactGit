using CompactGit.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CompactGit.Components.Pages
{
    public partial class User
    {
        public bool IsNotMyProfile = false;

        [Parameter] 
        public string UserUrl { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public ICookie Cookie { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            string loginCookie = await Cookie.GetValue("login");

            if (loginCookie != UserUrl)
            {
                IsNotMyProfile = true;
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private string FindInput { get; set; } = "";

        private void FindReposButtonClick(MouseEventArgs e)
        {
            if (FindInput.Contains("/") == true)
                NavigationManager.NavigateTo("/repos/" + FindInput);

            else
                NavigationManager.NavigateTo("/user/" + FindInput);
        }

        private void ReposButtonClick(MouseEventArgs e)
        {
            NavigationManager.NavigateTo("/repos/" + UserUrl);
        }

        private async Task SettingsButtonClickAsync(MouseEventArgs e)
        {
            string msg = await Cookie.GetValue("login");

            if (msg != UserUrl)
            {
                return;
            }

            NavigationManager.NavigateTo("/settings/" + UserUrl);
        }

        private async Task LogoutButtonClickAsync(MouseEventArgs e)
        {
            string cookie = await Cookie.GetValue("login");

            if (cookie == UserUrl)
            {
                await Cookie.SetValue("login", "");
                string tmp = await Cookie.GetValue("login");

                NavigationManager.NavigateTo("/");
            }
        }
    }
}