/* 

    LoadBalancer Class: Contains a method to distribute tasks among fog and cloud nodes. 
    It selects the least-loaded node that can handle each task.

*/
using System.Net.Http.Metrics;
using FLS; // https://github.com/davidgrupp/Fuzzy-Logic-Sharp
using FLS.Rules;

public enum MetricTypeEnum {
    Lower1, Lower2, Low1, Low2, Medium1, Medium2, Medium3, High1, High2, Higher1, Higher2
}
public class LoadBalancer
{
    private List<Node> fogNodes;
    private List<Node> cloudNodes;
    public Dictionary<MetricTypeEnum, double> metrics = new() {
        {MetricTypeEnum.Lower1, 0.0},
        {MetricTypeEnum.Lower2, 0.1},
        {MetricTypeEnum.Low1, 0.2},
        {MetricTypeEnum.Low2, 0.3},
        {MetricTypeEnum.Medium1, 0.4},
        {MetricTypeEnum.Medium2, 0.5},
        {MetricTypeEnum.Medium3, 0.6},
        {MetricTypeEnum.High1, 0.7},
        {MetricTypeEnum.High2, 0.8},
        {MetricTypeEnum.Higher1, 0.9},
        {MetricTypeEnum.Higher2, 1.0},
    };

    public LoadBalancer(List<Node> fogNodes, List<Node> cloudNodes)
    {
        this.fogNodes = fogNodes;
        this.cloudNodes = cloudNodes;
    }

    public void DistributeTasks(List<Task> tasks)
    {
        foreach (var task in tasks)
        {
            // Check for the task lenght and decide which one the task will load
            var fuzzyLogicResult = FuzzyLogic(task);

            if (fuzzyLogicResult <= 0.33) // Low
            {
                var fogNode = fogNodes.Where(n => n.CanHandleTask(fuzzyLogicResult)).OrderBy(n => n.CurrentCapacity).FirstOrDefault();
                task.setIsFog(true);
                SendToNode(task, fogNode, fuzzyLogicResult);
            }
            else // Medium and High
            {
                var cloudNode = cloudNodes.Where(n => n.CanHandleTask(fuzzyLogicResult)).OrderBy(n => n.CurrentCapacity).FirstOrDefault();
                task.setIsFog(false);
                SendToNode(task, cloudNode, fuzzyLogicResult);
            }
            
        }
    }

    public double FuzzyLogic(Task task) 
    {
        // Length metrics
        var length = new LinguisticVariable("Length");
        var lowL = length.MembershipFunctions.AddTriangle("Low", metrics[MetricTypeEnum.Lower1], metrics[MetricTypeEnum.Low1], metrics[MetricTypeEnum.Medium1]);
        var mediumL = length.MembershipFunctions.AddTriangle("Medium", metrics[MetricTypeEnum.Low2], metrics[MetricTypeEnum.Medium2], metrics[MetricTypeEnum.High1]);
        var highL = length.MembershipFunctions.AddTriangle("High", metrics[MetricTypeEnum.High1], metrics[MetricTypeEnum.High2], metrics[MetricTypeEnum.Higher2]);

        // Network metrics
        var network = new LinguisticVariable("Network");
        var poor = network.MembershipFunctions.AddTriangle("Poor", metrics[MetricTypeEnum.High1], metrics[MetricTypeEnum.High2], metrics[MetricTypeEnum.Higher2]);
        var average = network.MembershipFunctions.AddTriangle("Average", metrics[MetricTypeEnum.Low2], metrics[MetricTypeEnum.Medium2], metrics[MetricTypeEnum.High1]);
        var good = network.MembershipFunctions.AddTriangle("Good", metrics[MetricTypeEnum.Lower1], metrics[MetricTypeEnum.Low1], metrics[MetricTypeEnum.Medium1]);

        // Delay metrics
        var delay = new LinguisticVariable("Delay");
        var lowD = delay.MembershipFunctions.AddTriangle("Low", metrics[MetricTypeEnum.Lower1], metrics[MetricTypeEnum.Low1], metrics[MetricTypeEnum.Medium1]);
        var mediumD = delay.MembershipFunctions.AddTriangle("Medium", metrics[MetricTypeEnum.Low2], metrics[MetricTypeEnum.Medium2], metrics[MetricTypeEnum.High1]);
        var highD = delay.MembershipFunctions.AddTriangle("High", metrics[MetricTypeEnum.High1], metrics[MetricTypeEnum.High2], metrics[MetricTypeEnum.Higher2]);

        // Fog and Cloud metrics
        var allocation = new LinguisticVariable("Allocation");
        var fog = allocation.MembershipFunctions.AddTriangle("Fog", metrics[MetricTypeEnum.Lower1], metrics[MetricTypeEnum.Low2], metrics[MetricTypeEnum.Medium1]);
        var cloud = allocation.MembershipFunctions.AddTriangle("Cloud", metrics[MetricTypeEnum.Low2], metrics[MetricTypeEnum.High1], metrics[MetricTypeEnum.Higher2]);

        IFuzzyEngine fuzzyEngine = new FuzzyEngineFactory().Default();

        // The 27 possibilites of allocation
        var rule1 = Rule.If(length.Is(lowL).And(network.Is(good)).And(network.Is(lowD))).Then(allocation.Is(fog));
        var rule2 = Rule.If(length.Is(lowL).And(network.Is(good)).And(network.Is(mediumD))).Then(allocation.Is(fog));
        var rule3 = Rule.If(length.Is(lowL).And(network.Is(good)).And(network.Is(highD))).Then(allocation.Is(cloud));
        var rule4 = Rule.If(length.Is(lowL).And(network.Is(average)).And(network.Is(lowD))).Then(allocation.Is(fog));
        var rule5 = Rule.If(length.Is(lowL).And(network.Is(average)).And(network.Is(mediumD))).Then(allocation.Is(cloud));
        var rule6 = Rule.If(length.Is(lowL).And(network.Is(average)).And(network.Is(highD))).Then(allocation.Is(cloud));
        var rule7 = Rule.If(length.Is(lowL).And(network.Is(poor)).And(network.Is(lowD))).Then(allocation.Is(cloud));
        var rule8 = Rule.If(length.Is(lowL).And(network.Is(poor)).And(network.Is(mediumD))).Then(allocation.Is(cloud));
        var rule9 = Rule.If(length.Is(lowL).And(network.Is(poor)).And(network.Is(highD))).Then(allocation.Is(cloud));
        var rule10 = Rule.If(length.Is(mediumL).And(network.Is(good)).And(network.Is(lowD))).Then(allocation.Is(fog));
        var rule11 = Rule.If(length.Is(mediumL).And(network.Is(good)).And(network.Is(mediumD))).Then(allocation.Is(cloud));
        var rule12 = Rule.If(length.Is(mediumL).And(network.Is(good)).And(network.Is(highD))).Then(allocation.Is(cloud));
        var rule13 = Rule.If(length.Is(mediumL).And(network.Is(average)).And(network.Is(lowD))).Then(allocation.Is(cloud));
        var rule14 = Rule.If(length.Is(mediumL).And(network.Is(average)).And(network.Is(mediumD))).Then(allocation.Is(cloud));
        var rule15 = Rule.If(length.Is(mediumL).And(network.Is(average)).And(network.Is(highD))).Then(allocation.Is(cloud));
        var rule16 = Rule.If(length.Is(mediumL).And(network.Is(poor)).And(network.Is(lowD))).Then(allocation.Is(cloud));
        var rule17 = Rule.If(length.Is(mediumL).And(network.Is(poor)).And(network.Is(mediumD))).Then(allocation.Is(cloud));
        var rule18 = Rule.If(length.Is(mediumL).And(network.Is(poor)).And(network.Is(highD))).Then(allocation.Is(cloud));
        var rule19 = Rule.If(length.Is(highL).And(network.Is(good)).And(network.Is(lowD))).Then(allocation.Is(cloud));
        var rule20 = Rule.If(length.Is(highL).And(network.Is(good)).And(network.Is(mediumD))).Then(allocation.Is(cloud));
        var rule21 = Rule.If(length.Is(highL).And(network.Is(good)).And(network.Is(highD))).Then(allocation.Is(cloud));
        var rule22 = Rule.If(length.Is(highL).And(network.Is(average)).And(network.Is(lowD))).Then(allocation.Is(cloud));
        var rule23 = Rule.If(length.Is(highL).And(network.Is(average)).And(network.Is(mediumD))).Then(allocation.Is(cloud));
        var rule24 = Rule.If(length.Is(highL).And(network.Is(average)).And(network.Is(highD))).Then(allocation.Is(cloud));
        var rule25 = Rule.If(length.Is(highL).And(network.Is(poor)).And(network.Is(lowD))).Then(allocation.Is(cloud));
        var rule26 = Rule.If(length.Is(highL).And(network.Is(poor)).And(network.Is(mediumD))).Then(allocation.Is(cloud));
        var rule27 = Rule.If(length.Is(highL).And(network.Is(poor)).And(network.Is(highD))).Then(allocation.Is(cloud));
        fuzzyEngine.Rules.Add
        (
            rule1, rule2, rule3, rule4, rule5, rule6, rule7, rule8, rule9, rule10, 
            rule11, rule12, rule13, rule14, rule15, rule16, rule17, rule18, rule19, 
            rule20, rule21, rule22, rule23, rule24, rule25, rule26, rule27
        );

        var result = fuzzyEngine.Defuzzify(task);
        return result;
    }

    public void SendToNode(Task task, Node? node, double fuzzyLogicResult)
    {
        if(node != null) {
            node.AssignTask(task, fuzzyLogicResult);
        } 
        else
        {
            if(task.getIsFog())
            {
                Console.WriteLine($"No available fog node for task {task.Id}. Consider offloading to cloud.");
                // Send to the cloud
                var cloudNode = cloudNodes.Where(n => n.CanHandleTask(fuzzyLogicResult)).OrderBy(n => n.CurrentCapacity).FirstOrDefault();
                task.setIsFog(false);
                this.SendToNode(task, cloudNode, fuzzyLogicResult);
            }
            else
            {
                Console.WriteLine($"No available cloud node for task {task.Id}. Cannot process.");
            }
        }
    }
}