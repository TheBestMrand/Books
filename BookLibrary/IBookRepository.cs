using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary
{
    public interface IBookRepository
    {
        void AddBook(Book book);
        Book GetBookByISBN(string isbn);
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> GetBooksPaginated(int page, int pageSize);
        void UpdateBook(Book book);
        void DeleteBook(string isbn);
        void SaveToJson(string filePath);
        void LoadFromJson(string filePath);
    }
}
