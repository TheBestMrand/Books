using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary
{
    public class Book
    {
        public required string ISBN { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public PublicationInfo Publication { get; set; }
        public Publisher Publisher { get; set; }
        public List<BookReview> Reviews { get; set; } = new List<BookReview>();

        public override string ToString() => $"{Title} by {Author} ({ISBN})";
    }
}
