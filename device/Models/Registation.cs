namespace device.Models;

public class Registation
{
    public string Name { get; set; }
    public Guid BandId { get; set; }
    public int Age { get; set; }
    public DateTime Arrive_time { get; set; }
    public DateTime? Leave_time { get; set; }
    public string EventId { get; set; }      
}

