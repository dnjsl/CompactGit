using Microsoft.AspNetCore.Components;

namespace CompactGit.Components.Pages
{
    public partial class MainRepo
    {
        [Parameter]
        public string UserUrl { get; set; } = default!;

        [Parameter]
        public string RepoUrl { get; set; } = default!;
    }
}