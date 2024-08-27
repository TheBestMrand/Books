using BookLibrary;

var repository = new BookRepository();
var library = new Library(repository);

// Add some books
library.AddBook(new Book("1234567890", "Sample Book 1", "Author 1",
    new Published(new DateOnly(2022, 1, 1)),
    new Publisher("Publisher A", "Country A")));

library.AddBook(new Book("0987654321", "Sample Book 2", "Author 2",
    new Planned(new DateOnly(2023, 12, 31)),
    new Publisher("Publisher B", "Country B")));

// Add a review
library.AddReviewToBook("1234567890", new BookReview
{
    Id = 1,
    ReviewerName = "John Doe",
    Rating = 5,
    Comment = "Great book!"
});

// Save to JSON
library.SaveLibraryToJson("library.json");

// Load from JSON
var newLibrary = new Library(new BookRepository());
newLibrary.LoadLibraryFromJson("library.json");

// Display books with pagination
var page = 1;
var pageSize = 10;
foreach (var book in newLibrary.GetBooksPaginated(page, pageSize))
{
    Console.WriteLine(book);
    foreach (var review in book.Reviews)
    {
        Console.WriteLine($"  Review: {review.Rating}/5 - {review.Comment}");
    }
}