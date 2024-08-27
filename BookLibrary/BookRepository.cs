using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookLibrary
{
    public class BookRepository : IBookRepository
    {
        private List<Book> _books = new List<Book>();

        public void AddBook(Book book) => _books.Add(book);

        public Book GetBookByISBN(string isbn) => _books.FirstOrDefault(b => b.ISBN == isbn);

        public IEnumerable<Book> GetAllBooks() => _books;

        public IEnumerable<Book> GetBooksPaginated(int page, int pageSize)
            => _books.Skip((page - 1) * pageSize).Take(pageSize);

        public void UpdateBook(Book book)
        {
            var index = _books.FindIndex(b => b.ISBN == book.ISBN);
            if (index != -1)
                _books[index] = book;
        }

        public void DeleteBook(string isbn) => _books.RemoveAll(b => b.ISBN == isbn);

        public void SaveToJson(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };
            var json = JsonSerializer.Serialize(_books, options);
            File.WriteAllText(filePath, json);
        }

        public void LoadFromJson(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            var json = File.ReadAllText(filePath);
            _books = JsonSerializer.Deserialize<List<Book>>(json, options);
        }
    }
}
