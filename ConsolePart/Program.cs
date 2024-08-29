using BookLibrary;
using DependencyInjection;

var container = new DIContainer();

container.Register<IBookRepository, BookRepository>();
container.Register<IPublisherRepository, PublisherRepository>();
container.Register<Library, Library>();

var library = container.Resolve<Library>();

var isbn1 = ISBNGenerator.GenerateISBN13();
library.AddBook(new Book
{
    ISBN = isbn1,
    Title = "Sample Book 1",
    Author = "Author 1",
    Publication = new Published(new DateOnly(2022, 1, 1)),
    Publisher = new Publisher { Name = "Publisher A", Country = "Country A" }
});

var isbn2 = ISBNGenerator.GenerateISBN13();
library.AddBook(new Book
{
    ISBN = isbn2,
    Title = "Sample Book 2",
    Author = "Author 2",
    Publication = new Published(new DateOnly(2023, 3, 3)),
    Publisher = new Publisher { Name = "Publisher B", Country = "Country B" }
});

library.AddReviewToBook(isbn1, new BookReview
{
    Id = 1,
    ReviewerName = "John Doe",
    Rating = 5,
    Comment = "Great book!"
});

library.AddReviewToBook(isbn2, new BookReview
{
    Id = 2,
    ReviewerName = "Jane Smith",
    Rating = 4,
    Comment = "Interesting read!"
});

library.SaveLibraryToJson("library.json");

var newLibrary = container.Resolve<Library>();
newLibrary.LoadLibraryFromJson("library.json");

var page = 1;
var pageSize = 10;
foreach (var book in newLibrary.GetBooksPaginated(page, pageSize))
{
    Console.WriteLine(book);
    if (book.Reviews.Any())
    {
        Console.WriteLine("  Reviews:");
        foreach (var review in book.Reviews)
        {
            Console.WriteLine($"    {review.ReviewerName}: {review.Rating}/5 - {review.Comment}");
        }
    }
    else
    {
        Console.WriteLine("  No reviews yet.");
    }
    Console.WriteLine();
}
