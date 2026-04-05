using Microsoft.Extensions.Options;
using Moq;
using Tutorial01B.Agents;
using Tutorial01B.Executors;
using Tutorial01B.Models;

namespace Tutorial01BTests;

[TestClass]
public class TranslationAgentTests
{
    private static IOptions<PromptSettings> CreateOptions(PromptSettings settings) =>
        Options.Create(settings);

    [TestMethod]
    public async Task ReplyAsync_UsesSystemPromptFromSettings()
    {
        // Arrange
        const string customPrompt = "Custom translation prompt";
        const string userInput = "こんにちは";
        const string expectedResponse = "Hello";

        var settings = new PromptSettings
        {
            Prompts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "TranslationAgent", customPrompt }
            }
        };

        var executorMock = new Mock<IChatCompletionExecutor>();
        executorMock
            .Setup(e => e.CompleteAsync(customPrompt, userInput, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ChatCompletionResult(expectedResponse));

        var agent = new TranslationAgent(executorMock.Object, CreateOptions(settings));

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
        const string userInput = "Hello";
        const string expectedResponse = "こんにちは";

        var settings = new PromptSettings(); // 空の設定（TranslationAgent のキーなし）

        var executorMock = new Mock<IChatCompletionExecutor>();
        executorMock
            .Setup(e => e.CompleteAsync(It.IsAny<string>(), userInput, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ChatCompletionResult(expectedResponse));

        var agent = new TranslationAgent(executorMock.Object, CreateOptions(settings));

        // Act
        var result = await agent.ReplyAsync(userInput);

        // Assert
        Assert.AreEqual(expectedResponse, result);
        executorMock.Verify(
            e => e.CompleteAsync(It.IsNotNull<string>(), userInput, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public void Name_ReturnsTranslationAgent()
    {
        // Arrange
        var settings = new PromptSettings();
        var executorMock = new Mock<IChatCompletionExecutor>();
        var agent = new TranslationAgent(executorMock.Object, CreateOptions(settings));

        // Assert
        Assert.AreEqual("TranslationAgent", agent.Name);
    }
}
