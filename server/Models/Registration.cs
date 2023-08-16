namespace server.Models;

public class Registation
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
    [JsonProperty(PropertyName = "bandId")]
    public string BandId { get; set; }
    [JsonProperty(PropertyName = "age")]
    public int Age { get; set; }
    [JsonProperty(PropertyName = "arrive_time")]
    public DateTime ArriveTime { get; set; }
    [JsonProperty(PropertyName = "leave_time")]
    public DateTime? LeaveTime { get; set; }
    [JsonProperty(PropertyName = "eventId")]
    public string EventId { get; set; }
}


