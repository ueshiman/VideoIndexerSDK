using Moq;
using Tutorial01B.Agents;
using Tutorial01B.Services;

namespace Tutorial01BTests;

[TestClass]
public class AgentOrchestratorTests
{
    [TestMethod]
    public async Task HandleAsync_CallsAllAgentsAndReturnsResults()
    {
        // Arrange
        const string message = "Test message";

        var agentA = new Mock<IAgent>();
        agentA.SetupGet(a => a.Name).Returns("AgentA");
        agentA.Setup(a => a.ReplyAsync(message, It.IsAny<CancellationToken>()))
              .ReturnsAsync("Response from A");

        var agentB = new Mock<IAgent>();
        agentB.SetupGet(a => a.Name).Returns("AgentB");
        agentB.Setup(a => a.ReplyAsync(message, It.IsAny<CancellationToken>()))
              .ReturnsAsync("Response from B");

        var orchestrator = new AgentOrchestrator([agentA.Object, agentB.Object]);

        // Act
        var results = await orchestrator.HandleAsync(message);

        // Assert
        Assert.AreEqual(2, results.Count);
        Assert.AreEqual("Response from A", results["AgentA"]);
        Assert.AreEqual("Response from B", results["AgentB"]);
    }

    [TestMethod]
    public async Task HandleAsync_WithNoAgents_ReturnsEmptyDictionary()
    {
        // Arrange
        var orchestrator = new AgentOrchestrator([]);

        // Act
        var results = await orchestrator.HandleAsync("Hello");

        // Assert
        Assert.AreEqual(0, results.Count);
    }
}
