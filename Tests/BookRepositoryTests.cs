using BookLibrary;

namespace Tests
{
    public class BookRepositoryTests
    {

        [Fact]
        public void AddBook_ShouldAddBookToRepository()
        {
            var repo = new BookRepository();
            var book = CreateSampleBook();

            repo.AddBook(book);

            Assert.Single(repo.GetAllBooks());
            Assert.Equal(book, repo.GetBookByISBN(book.ISBN));
        }

        [Fact]
        public void GetBookByISBN_ShouldReturnCorrectBook()
        {
            var repo = new BookRepository();
            var book1 = CreateSampleBook();
            var book2 = CreateSampleBook();
            repo.AddBook(book1);
            repo.AddBook(book2);

            var result = repo.GetBookByISBN(book1.ISBN);

            Assert.Equal(book1, result);
        }

        [Fact]
        public void GetAllBooks_ShouldReturnAllBooks()
        {
            var repo = new BookRepository();
            var book1 = CreateSampleBook();
            var book2 = CreateSampleBook();
            repo.AddBook(book1);
            repo.AddBook(book2);

            var result = repo.GetAllBooks();

            Assert.Equal(2, result.Count());
            Assert.Contains(book1, result);
            Assert.Contains(book2, result);
        }

        [Fact]
        public void GetBooksPaginated_ShouldReturnCorrectPage()
        {
            var repo = new BookRepository();
            for (int i = 0; i < 10; i++)
            {
                repo.AddBook(CreateSampleBook());
            }

            var result = repo.GetBooksPaginated(2, 3).ToList();

            Assert.Equal(3, result.Count);
            Assert.Equal(repo.GetAllBooks().ElementAt(3), result[0]);
            Assert.Equal(repo.GetAllBooks().ElementAt(5), result[2]);
        }

        [Fact]
        public void UpdateBook_ShouldUpdateExistingBook()
        {
            var repo = new BookRepository();
            var book = CreateSampleBook();
            repo.AddBook(book);

            book.Title = "Updated Title";
            repo.UpdateBook(book);

            var updatedBook = repo.GetBookByISBN(book.ISBN);
            Assert.Equal("Updated Title", updatedBook.Title);
        }

        [Fact]
        public void DeleteBook_ShouldRemoveBookFromRepository()
        {
            var repo = new BookRepository();
            var book = CreateSampleBook();
            repo.AddBook(book);

            repo.DeleteBook(book.ISBN);

            Assert.Empty(repo.GetAllBooks());
        }

        [Fact]
        public void SaveToJson_LoadFromJson_ShouldPreserveData()
        {
            var repo = new BookRepository();
            var book1 = CreateSampleBook();
            var book2 = CreateSampleBook();
            repo.AddBook(book1);
            repo.AddBook(book2);
            var tempFile = Path.GetTempFileName();

            repo.SaveToJson(tempFile);
            var newRepo = new BookRepository();
            newRepo.LoadFromJson(tempFile);

            Assert.Equal(2, newRepo.GetAllBooks().Count());
            Assert.Equal(book1.ISBN, newRepo.GetAllBooks().First().ISBN);
            Assert.Equal(book2.ISBN, newRepo.GetAllBooks().Last().ISBN);

            File.Delete(tempFile);
        }

        private Book CreateSampleBook()
        {
            return new Book
            {
                ISBN = ISBNGenerator.GenerateISBN13(),
                Title = "Sample Book",
                Author = "John Doe",
                Publication = new Published(DateOnly.FromDateTime(DateTime.Now)),
                Publisher = new Publisher { Name = "Sample Publisher", Country = "Sample Country" },
                Reviews = new List<BookReview>()
            };
        }
    }
}