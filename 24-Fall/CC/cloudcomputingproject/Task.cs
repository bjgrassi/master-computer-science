public class Task
{
    public string Id { get; set; }
    public double Length { get; set; } // intensity of a task/ latency. Measure the time from task assignment to completion.
    public double Network { get; set; } // Network bandwidth condition
    public double Delay { get; set; } // time sensitivity
    public bool isFog { get; set; } // Fog = true; Cloud = false;

    public Task(string id, double length, double network, double delay)
    {
        Id = id;
        Length = length;
        Network = network;
        Delay = delay;
    }

    public void setIsFog(bool type)
    {
        this.isFog = type;
    }
    public bool getIsFog()
    {
        return this.isFog;
    }
}