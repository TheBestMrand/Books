using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using DependencyInjection;

namespace BookLibrary
{
    public class Library
    {
        private readonly IBookRepository _bookRepository;
        private readonly IPublisherRepository _publisherRepository;

        [Inject]
        public Library(IBookRepository bookRepository, IPublisherRepository publisherRepository)
        {
            _bookRepository = bookRepository;
            _publisherRepository = publisherRepository;
        }

        public void AddBook(Book book)
        {
            var existingPublisher = _publisherRepository.GetPublisherByName(book.Publisher.Name);
            if (existingPublisher == null)
            {
                _publisherRepository.AddPublisher(book.Publisher);
            }
            else
            {
                book.Publisher = existingPublisher;
            }

            _bookRepository.AddBook(book);
        }

        public Book GetBook(string isbn) => _bookRepository.GetBookByISBN(isbn);

        public IEnumerable<Book> GetAllBooks() => _bookRepository.GetAllBooks();

        public IEnumerable<Book> GetBooksPaginated(int page, int pageSize)
            => _bookRepository.GetBooksPaginated(page, pageSize);

        public IEnumerable<Book> SearchBooks(Func<Book, bool> predicate)
            => _bookRepository.GetAllBooks().Where(predicate);

        public void UpdateBook(Book book) => _bookRepository.UpdateBook(book);

        public void DeleteBook(string isbn) => _bookRepository.DeleteBook(isbn);

        public Publisher GetPublisher(string name) => _publisherRepository.GetPublisherByName(name);

        public IEnumerable<Publisher> GetAllPublishers() => _publisherRepository.GetAllPublishers();

        public void AddReviewToBook(string isbn, BookReview review)
        {
            var book = GetBook(isbn);
            if (book != null)
            {
                book.Reviews.Add(review);
                UpdateBook(book);
            }
        }

        public void SaveLibraryToJson(string filePath) => _bookRepository.SaveToJson(filePath);

        public void LoadLibraryFromJson(string filePath) => _bookRepository.LoadFromJson(filePath);
    }
}
