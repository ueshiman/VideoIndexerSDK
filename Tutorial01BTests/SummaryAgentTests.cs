using Microsoft.Extensions.Options;
using Moq;
using Tutorial01B.Agents;
using Tutorial01B.Executors;
using Tutorial01B.Models;

namespace Tutorial01BTests;

[TestClass]
public class SummaryAgentTests
{
    private static IOptions<PromptSettings> CreateOptions(PromptSettings settings) =>
        Options.Create(settings);

    [TestMethod]
    public async Task ReplyAsync_UsesSystemPromptFromSettings()
    {
        // Arrange
        const string customPrompt = "Custom summary prompt";
        const string userInput = "Hello world";
        const string expectedResponse = "Summary response";

        var settings = new PromptSettings
        {
            Prompts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "SummaryAgent", customPrompt }
            }
        };

        var executorMock = new Mock<IChatCompletionExecutor>();
        executorMock
            .Setup(e => e.CompleteAsync(customPrompt, userInput, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ChatCompletionResult(expectedResponse));

        var agent = new SummaryAgent(executorMock.Object, CreateOptions(settings));

        // Act
        var result = await agent.ReplyAsync(userInput);

        // Assert
        Assert.AreEqual(expectedResponse, result);
        executorMock.Verify(
            e => e.CompleteAsync(customPrompt, userInput, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task ReplyAsync_FallsBackToDefaultPromptWhenNotInSettings()
    {
        // Arrange
        const string userInput = "Test input";
        const string expectedResponse = "Default summary";

        var settings = new PromptSettings(); // 空の設定（SummaryAgent のキーなし）

        var executorMock = new Mock<IChatCompletionExecutor>();
        executorMock
            .Setup(e => e.CompleteAsync(It.IsAny<string>(), userInput, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ChatCompletionResult(expectedResponse));

        var agent = new SummaryAgent(executorMock.Object, CreateOptions(settings));

        // Act
        var result = await agent.ReplyAsync(userInput);

        // Assert
        Assert.AreEqual(expectedResponse, result);
        // デフォルトプロンプト（空設定でも）で CompleteAsync が呼ばれることを確認
        executorMock.Verify(
            e => e.CompleteAsync(It.IsNotNull<string>(), userInput, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public void Name_ReturnsSummaryAgent()
    {
        // Arrange
        var settings = new PromptSettings();
        var executorMock = new Mock<IChatCompletionExecutor>();
        var agent = new SummaryAgent(executorMock.Object, CreateOptions(settings));

        // Assert
        Assert.AreEqual("SummaryAgent", agent.Name);
    }
}
