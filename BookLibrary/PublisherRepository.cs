using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary
{
    public class PublisherRepository : IPublisherRepository
    {
        private List<Publisher> _publishers = new List<Publisher>();

        public void AddPublisher(Publisher publisher) => _publishers.Add(publisher);

        public Publisher GetPublisherByName(string name) => _publishers.FirstOrDefault(p => p.Name == name);

        public IEnumerable<Publisher> GetAllPublishers() => _publishers;

        public void UpdatePublisher(Publisher publisher)
        {
            var index = _publishers.FindIndex(p => p.Name == publisher.Name);
            if (index != -1)
                _publishers[index] = publisher;
        }

        public void DeletePublisher(string name) => _publishers.RemoveAll(p => p.Name == name);
    }
}
