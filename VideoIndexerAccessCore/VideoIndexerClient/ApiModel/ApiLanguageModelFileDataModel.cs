/// <summary>
/// API model representing language model file data and metadata.
/// </summary>
public class ApiLanguageModelFileDataModel
{
    /// <summary>File ID (GUID)</summary>
    public string? id { get; set; }

    /// <summary>File name</summary>
    public string? name { get; set; }

    /// <summary>Indicates whether the file is enabled</summary>
    public bool enable { get; set; }

    /// <summary>Creator of the file</summary>
    public string? creator { get; set; }

    /// <summary>File creation timestamp</summary>
    public DateTime? creationTime { get; set; }

    /// <summary>File content</summary>
    public string? content { get; set; }
}
