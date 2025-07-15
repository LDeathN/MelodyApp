namespace MelodyApp.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
    
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
