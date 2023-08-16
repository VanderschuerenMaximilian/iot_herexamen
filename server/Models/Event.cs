
namespace server.Models;
public class Event
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
    [JsonProperty(PropertyName = "city")]
    public string City { get; set; }
    [JsonProperty(PropertyName = "creator")]
    public string Creator { get; set; }
    [JsonProperty(PropertyName = "start_time")]
    public DateTime StartTime { get; set; }
    [JsonProperty(PropertyName = "close_time")]
    public DateTime CloseTime { get; set; }
    [JsonProperty(PropertyName = "is_active")]
    public bool IsActive { get; set; }
}

