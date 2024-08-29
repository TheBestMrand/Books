using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookLibrary
{
    public class PublicationInfoConverter : JsonConverter<PublicationInfo>
    {
        public override PublicationInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("PublishedDate", out JsonElement publishedDate))
                {
                    return new Published(DateOnly.Parse(publishedDate.GetString()));
                }
                else if (root.TryGetProperty("PlannedDate", out JsonElement plannedDate))
                {
                    return new Planned(DateOnly.Parse(plannedDate.GetString()));
                }
                else
                {
                    return new NotPublishedYet();
                }
            }
        }

        public override void Write(Utf8JsonWriter writer, PublicationInfo value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            switch (value)
            {
                case Published published:
                    writer.WriteString("PublishedDate", published.PublishedDate.ToString());
                    break;
                case Planned planned:
                    writer.WriteString("PlannedDate", planned.PlannedDate.ToString());
                    break;
                case NotPublishedYet:
                    break;
            }

            writer.WriteEndObject();
        }
    }
}
