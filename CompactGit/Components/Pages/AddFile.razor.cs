using CompactGit.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text.Json;

namespace CompactGit.Components.Pages
{
	public partial class AddFile : ComponentBase
    {

        [Parameter]
        public string UserUrl { get; set; } = default!;

        [Parameter]
        public string RepoUrl { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public ICookie Cookie { get; set; } = default!;

        private void SaveButtonClick()
        {
            NavigationManager.NavigateTo("/r/" + UserUrl + RepoUrl);
        }
    }
}
