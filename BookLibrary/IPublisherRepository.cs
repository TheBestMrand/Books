using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary
{
    public interface IPublisherRepository
    {
        void AddPublisher(Publisher publisher);
        Publisher GetPublisherByName(string name);
        IEnumerable<Publisher> GetAllPublishers();
        void UpdatePublisher(Publisher publisher);
        void DeletePublisher(string name);
    }
}
