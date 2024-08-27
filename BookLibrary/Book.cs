using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary
{
    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public PublicationInfo Publication { get; set; }
        public Publisher Publisher { get; set; }
        public List<BookReview> Reviews { get; set; } = new List<BookReview>();

        public Book(string isbn, string title, string author, PublicationInfo publication, Publisher publisher)
        {
            ISBN = isbn;
            Title = title;
            Author = author;
            Publication = publication;
            Publisher = publisher;
        }

        public override string ToString() => $"{Title} by {Author} ({ISBN})";
    }
}
