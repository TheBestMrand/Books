using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookLibrary
{
    [JsonConverter(typeof(PublicationInfoConverter))]
    public abstract record PublicationInfo;
    public record Published(DateOnly PublishedDate) : PublicationInfo;
    public record Planned(DateOnly PlannedDate) : PublicationInfo;
    public record NotPublishedYet : PublicationInfo;
}
