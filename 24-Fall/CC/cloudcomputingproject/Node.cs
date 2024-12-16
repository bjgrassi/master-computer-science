
/*
    Node Class: Represents a node (either fog or cloud) with an ID and current load. 
    It has methods to check if it can handle a task and to assign a task.
*/
public class Node
{
    public string Id { get; set; }
    public string Type { get; set; }
    public double Capacity { get; set; }
    public double CurrentCapacity { get; set; }

    public Node(string id, string type, double capacity)
    {
        Id = id;
        Type = type;
        Capacity = capacity; //capacity
        CurrentCapacity = 0;
    }

    // CHECK THE CAPACITY
    public bool CanHandleTask(double fuzzyLogicResult)
    {
        // Is checking if the capacity is full or not.
        return CurrentCapacity + fuzzyLogicResult <= Capacity;
    }

    public void AssignTask(Task task, double fuzzyLogicResult)
    {
        CurrentCapacity += fuzzyLogicResult;
        Console.WriteLine($"Assigned task {task.Id} to node {Id}. Current load: {CurrentCapacity}");
    }
}