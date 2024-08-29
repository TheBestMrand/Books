using BookLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class LibrarySystemTests
    {
        private IBookRepository CreateBookRepository()
        {
            return new BookRepository();
        }

        private IPublisherRepository CreatePublisherRepository()
        {
            return new PublisherRepository();
        }

        private Library CreateLibrary()
        {
            return new Library(CreateBookRepository(), CreatePublisherRepository());
        }

        private Book CreateSampleBook(string isbn, string title, string author, PublicationInfo publicationInfo, Publisher publisher, List<string> categories)
        {
            return new Book
            {
                ISBN = isbn,
                Title = title,
                Author = author,
                Publication = publicationInfo,
                Publisher = publisher,
                Categories = categories
            };
        }

        [Fact]
        public void AddBook_ShouldAddBookToLibrary()
        {
            var library = CreateLibrary();
            var book = CreateSampleBook("1234567890", "Sample Book", "John Doe", new Published(new DateOnly(2022, 1, 1)), new Publisher { Name = "Sample Publisher", Country = "USA" }, new List<string> { "Fiction" });

            library.AddBook(book);

            Assert.Single(library.GetAllBooks());
            Assert.Equal(book, library.GetBook(book.ISBN));
        }

        [Fact]
        public void GetBooksPaginated_ShouldReturnCorrectPage()
        {
            var library = CreateLibrary();
            for (int i = 0; i < 10; i++)
            {
                library.AddBook(CreateSampleBook($"ISBN{i}", $"Book {i}", $"Author {i}", new Published(new DateOnly(2022, 1, 1)), new Publisher { Name = "Publisher", Country = "Country" }, new List<string> { "Category" }));
            }

            var result = library.GetBooksPaginated(2, 3).ToList();

            Assert.Equal(3, result.Count);
            Assert.Equal("ISBN3", result[0].ISBN);
            Assert.Equal("ISBN5", result[2].ISBN);
        }

        [Fact]
        public void AddReviewToBook_ShouldAddReviewCorrectly()
        {
            var library = CreateLibrary();
            var book = CreateSampleBook("1234567890", "Sample Book", "John Doe", new Published(new DateOnly(2022, 1, 1)), new Publisher { Name = "Sample Publisher", Country = "USA" }, new List<string> { "Fiction" });
            library.AddBook(book);

            var review = new BookReview { Id = 1, ReviewerName = "Alice", Rating = 5, Comment = "Great book!" };
            library.AddReviewToBook(book.ISBN, review);

            var updatedBook = library.GetBook(book.ISBN);
            Assert.Single(updatedBook.Reviews);
            Assert.Equal(review, updatedBook.Reviews[0]);
        }

        [Fact]
        public void SearchBooks_ShouldReturnCorrectBooks()
        {
            var library = CreateLibrary();
            var book1 = CreateSampleBook("1234567890", "Fiction Book", "John Doe", new Published(new DateOnly(2022, 1, 1)), new Publisher { Name = "Publisher A", Country = "USA" }, new List<string> { "Fiction" });
            var book2 = CreateSampleBook("0987654321", "Non-Fiction Book", "Jane Smith", new Published(new DateOnly(2023, 1, 1)), new Publisher { Name = "Publisher B", Country = "UK" }, new List<string> { "Non-Fiction" });
            library.AddBook(book1);
            library.AddBook(book2);

            var fictionBooks = library.SearchBooks(b => b.Categories.Contains("Fiction")).ToList();
            var usaBooks = library.SearchBooks(b => b.Publisher.Country == "USA").ToList();

            Assert.Single(fictionBooks);
            Assert.Equal(book1, fictionBooks[0]);
            Assert.Single(usaBooks);
            Assert.Equal(book1, usaBooks[0]);
        }

        [Fact]
        public void GetPublisherNameFromBook_ShouldReturnCorrectName()
        {
            var library = CreateLibrary();
            var book = CreateSampleBook("1234567890", "Sample Book", "John Doe", new Published(new DateOnly(2022, 1, 1)), new Publisher { Name = "Acme Publishing", Country = "USA" }, new List<string> { "Fiction" });
            library.AddBook(book);

            var retrievedBook = library.GetBook("1234567890");
            Assert.Equal("Acme Publishing", retrievedBook.Publisher.Name);
        }

        [Fact]
        public void GetBookByCategory_ShouldReturnCorrectBooks()
        {
            var library = CreateLibrary();
            var book1 = CreateSampleBook("1234567890", "Fiction Book", "John Doe", new Published(new DateOnly(2022, 1, 1)), new Publisher { Name = "Publisher A", Country = "USA" }, new List<string> { "Fiction", "Romance" });
            var book2 = CreateSampleBook("0987654321", "Non-Fiction Book", "Jane Smith", new Published(new DateOnly(2023, 1, 1)), new Publisher { Name = "Publisher B", Country = "UK" }, new List<string> { "Non-Fiction", "Science" });
            library.AddBook(book1);
            library.AddBook(book2);

            var romanceBooks = library.SearchBooks(b => b.Categories.Contains("Romance")).ToList();
            var scienceBooks = library.SearchBooks(b => b.Categories.Contains("Science")).ToList();

            Assert.Single(romanceBooks);
            Assert.Equal(book1, romanceBooks[0]);
            Assert.Single(scienceBooks);
            Assert.Equal(book2, scienceBooks[0]);
        }

        [Fact]
        public void GetBookByPublishingInfoAndCountry_ShouldReturnCorrectBooks()
        {
            var library = CreateLibrary();
            var book1 = CreateSampleBook("1234567890", "Book 1", "Author 1", new Published(new DateOnly(2022, 1, 1)), new Publisher { Name = "Publisher A", Country = "USA" }, new List<string> { "Fiction" });
            var book2 = CreateSampleBook("0987654321", "Book 2", "Author 2", new Planned(new DateOnly(2023, 12, 31)), new Publisher { Name = "Publisher B", Country = "UK" }, new List<string> { "Non-Fiction" });
            var book3 = CreateSampleBook("1122334455", "Book 3", "Author 3", new NotPublishedYet(), new Publisher { Name = "Publisher C", Country = "USA" }, new List<string> { "Fiction" });
            library.AddBook(book1);
            library.AddBook(book2);
            library.AddBook(book3);

            var publishedUsaBooks = library.SearchBooks(b => b.Publication is Published && b.Publisher.Country == "USA").ToList();
            var plannedBooks = library.SearchBooks(b => b.Publication is Planned).ToList();
            var notPublishedBooks = library.SearchBooks(b => b.Publication is NotPublishedYet).ToList();

            Assert.Single(publishedUsaBooks);
            Assert.Equal(book1, publishedUsaBooks[0]);
            Assert.Single(plannedBooks);
            Assert.Equal(book2, plannedBooks[0]);
            Assert.Single(notPublishedBooks);
            Assert.Equal(book3, notPublishedBooks[0]);
        }

        [Fact]
        public void SaveAndLoadLibrary_ShouldPreserveAllData()
        {
            var library = CreateLibrary();
            var book1 = CreateSampleBook("1234567890", "Book 1", "Author 1", new Published(new DateOnly(2022, 1, 1)), new Publisher { Name = "Publisher A", Country = "USA" }, new List<string> { "Fiction" });
            var book2 = CreateSampleBook("0987654321", "Book 2", "Author 2", new Planned(new DateOnly(2023, 12, 31)), new Publisher { Name = "Publisher B", Country = "UK" }, new List<string> { "Non-Fiction" });
            library.AddBook(book1);
            library.AddBook(book2);

            library.AddReviewToBook(book1.ISBN, new BookReview { Id = 1, ReviewerName = "Alice", Rating = 5, Comment = "Great book!" });

            string tempFile = Path.GetTempFileName();
            library.SaveLibraryToJson(tempFile);

            var newLibrary = CreateLibrary();
            newLibrary.LoadLibraryFromJson(tempFile);

            var loadedBooks = newLibrary.GetAllBooks().ToList();
            Assert.Equal(2, loadedBooks.Count);

            var loadedBook1 = newLibrary.GetBook("1234567890");
            Assert.Equal(book1.Title, loadedBook1.Title);
            Assert.Equal(book1.Author, loadedBook1.Author);
            Assert.Equal(book1.Publisher.Name, loadedBook1.Publisher.Name);
            Assert.Equal(book1.Publisher.Country, loadedBook1.Publisher.Country);
            Assert.Single(loadedBook1.Reviews);
            Assert.Equal("Alice", loadedBook1.Reviews[0].ReviewerName);

            var loadedBook2 = newLibrary.GetBook("0987654321");
            Assert.Equal(book2.Title, loadedBook2.Title);
            Assert.Equal(book2.Author, loadedBook2.Author);
            Assert.Equal(book2.Publisher.Name, loadedBook2.Publisher.Name);
            Assert.Equal(book2.Publisher.Country, loadedBook2.Publisher.Country);
            Assert.Empty(loadedBook2.Reviews);

            File.Delete(tempFile);
        }
    }
}
