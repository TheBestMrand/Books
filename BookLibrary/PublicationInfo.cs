using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary
{
    public abstract record PublicationInfo;
    public record Published(DateOnly PublishedDate) : PublicationInfo;
    public record Planned(DateOnly PlannedDate) : PublicationInfo;
    public record NotPublishedYet : PublicationInfo;
}
