using BookLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class PublisherRepositoryTests
    {
        private IPublisherRepository CreatePublisherRepository()
        {
            return new PublisherRepository();
        }

        private Publisher CreateSamplePublisher(string name, string country)
        {
            return new Publisher { Name = name, Country = country };
        }

        [Fact]
        public void AddPublisher_ShouldAddPublisherToRepository()
        {
            var repo = CreatePublisherRepository();
            var publisher = CreateSamplePublisher("Test Publisher", "Test Country");

            repo.AddPublisher(publisher);

            Assert.Single(repo.GetAllPublishers());
            Assert.Equal(publisher, repo.GetPublisherByName(publisher.Name));
        }

        [Fact]
        public void GetPublisherByName_ShouldReturnCorrectPublisher()
        {
            var repo = CreatePublisherRepository();
            var publisher1 = CreateSamplePublisher("Publisher A", "Country A");
            var publisher2 = CreateSamplePublisher("Publisher B", "Country B");
            repo.AddPublisher(publisher1);
            repo.AddPublisher(publisher2);

            var result = repo.GetPublisherByName("Publisher A");

            Assert.Equal(publisher1, result);
        }

        [Fact]
        public void GetAllPublishers_ShouldReturnAllPublishers()
        {
            var repo = CreatePublisherRepository();
            var publisher1 = CreateSamplePublisher("Publisher A", "Country A");
            var publisher2 = CreateSamplePublisher("Publisher B", "Country B");
            repo.AddPublisher(publisher1);
            repo.AddPublisher(publisher2);

            var result = repo.GetAllPublishers();

            Assert.Equal(2, result.Count());
            Assert.Contains(publisher1, result);
            Assert.Contains(publisher2, result);
        }

        [Fact]
        public void UpdatePublisher_ShouldUpdateExistingPublisher()
        {
            var repo = CreatePublisherRepository();
            var publisher = CreateSamplePublisher("Test Publisher", "Test Country");
            repo.AddPublisher(publisher);

            publisher.Country = "Updated Country";
            repo.UpdatePublisher(publisher);

            var updatedPublisher = repo.GetPublisherByName(publisher.Name);
            Assert.Equal("Updated Country", updatedPublisher.Country);
        }

        [Fact]
        public void DeletePublisher_ShouldRemovePublisherFromRepository()
        {
            var repo = CreatePublisherRepository();
            var publisher = CreateSamplePublisher("Test Publisher", "Test Country");
            repo.AddPublisher(publisher);

            repo.DeletePublisher(publisher.Name);

            Assert.Empty(repo.GetAllPublishers());
        }

        [Fact]
        public void GetPublisherByName_ShouldReturnNullForNonexistentPublisher()
        {
            var repo = CreatePublisherRepository();

            var result = repo.GetPublisherByName("Nonexistent Publisher");

            Assert.Null(result);
        }

        [Fact]
        public void UpdatePublisher_ShouldNotAddNewPublisherIfNotExists()
        {
            var repo = CreatePublisherRepository();
            var publisher = CreateSamplePublisher("New Publisher", "New Country");

            repo.UpdatePublisher(publisher);

            Assert.Empty(repo.GetAllPublishers());
        }

        [Fact]
        public void DeletePublisher_ShouldNotThrowExceptionForNonexistentPublisher()
        {
            var repo = CreatePublisherRepository();

            var exception = Record.Exception(() => repo.DeletePublisher("Nonexistent Publisher"));
            Assert.Null(exception);
        }
    }
}
