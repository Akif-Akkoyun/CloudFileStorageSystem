namespace App.WebUI.Models
{
    public class SharedByMeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public string Visibility { get; set; } = default!;
        public List<int> SharedWithUsers { get; set; } = new();
        public List<string> SharedWithUserNames { get; set; } = new();
    }
}
