using System.Linq;

namespace LibraryService.Models
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Lang { get; set; }
        public int Pages { get; set; }
        public int AgeLimit { get; set; }
        public int PublicationDate { get; set; }
        public Author[] Authors { get; set; }

        public override string ToString() =>
            $"{Id}, {Title}, {Category}, {Lang}, {Pages}стр., " +
            $"{AgeLimit}, {PublicationDate}год., " +
            $"{string.Join<Author>(", ", Authors.ToArray())}";
    }
}