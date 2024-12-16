using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using static Task;
using static Node;
using static LoadBalancer;

public class Program
{
    public static void Main(string[] args)
    {
        // Create fog and cloud nodes
        var fogNodes = new List<Node>
        {
            new Node("FogNode1", "Fog", 0.3),
            // new Node("FogNode2", "Fog", 0.6),
            // new Node("FogNode3", "Fog", 0.8)
        };

        var cloudNodes = new List<Node>
        {
            new Node("CloudNode1", "Cloud", 5),
            new Node("CloudNode2", "Cloud", 5)
        };

        // Create a list of tasks
        var tasks = new List<Task>
        {
            new Task("Task1", 0.3, 0.3, 0.3),
            new Task("Task2", 0.1, 0.3, 0.2),
            new Task("Task3", 0.6, 0.5, 0.9),
            new Task("Task4", 0.3, 0.3, 0.7),
            new Task("Task5", 0.8, 0.9, 0.8),
            new Task("Task6", 0.1, 0.3, 0.2),
            new Task("Task7", 0.6, 0.5, 0.9),
            new Task("Task8", 0.3, 0.3, 0.3),
            new Task("Task9", 0.8, 0.9, 0.8)
        };

        // Create a load balancer
        var loadBalancer = new LoadBalancer(fogNodes, cloudNodes);
        loadBalancer.DistributeTasks(tasks);
    }
}
