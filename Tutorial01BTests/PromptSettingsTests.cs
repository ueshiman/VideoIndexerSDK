using Tutorial01B.Models;

namespace Tutorial01BTests;

[TestClass]
public class PromptSettingsTests
{
    [TestMethod]
    public void PromptSettings_DefaultPromptsIsEmpty()
    {
        var settings = new PromptSettings();
        Assert.AreEqual(0, settings.Prompts.Count);
    }

    [TestMethod]
    public void PromptSettings_CaseInsensitiveLookup()
    {
        // Arrange
        var settings = new PromptSettings
        {
            Prompts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "SummaryAgent", "Some prompt" }
            }
        };

        // Act & Assert
        Assert.IsTrue(settings.Prompts.TryGetValue("summaryagent", out var value));
        Assert.AreEqual("Some prompt", value);
    }

    [TestMethod]
    public void PromptSettings_SectionNameIsPrompts()
    {
        Assert.AreEqual("Prompts", PromptSettings.SectionName);
    }

    [TestMethod]
    public void AgentSettings_DefaultEnabledIsEmpty()
    {
        var settings = new AgentSettings();
        Assert.AreEqual(0, settings.Enabled.Count);
    }

    [TestMethod]
    public void AgentSettings_SectionNameIsAgents()
    {
        Assert.AreEqual("Agents", AgentSettings.SectionName);
    }
}
