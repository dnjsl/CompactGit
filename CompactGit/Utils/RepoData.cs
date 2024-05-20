namespace CompactGit.Utils
{
    public class RepoData
    {
        public string UserId { get; set; } = null!;

        public string RepoName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public bool IsPublic { get; set; }
    }
}
