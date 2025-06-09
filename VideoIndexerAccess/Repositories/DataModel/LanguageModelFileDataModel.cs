using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class LanguageModelFileDataModel
    {
        /// <summary>File ID (GUID)</summary>
        public string? Id { get; set; }

        /// <summary>File name</summary>
        public string? Name { get; set; }

        /// <summary>Indicates whether the file is enabled</summary>
        public bool Enable { get; set; }

        /// <summary>Creator of the file</summary>
        public string? Creator { get; set; }

        /// <summary>File creation timestamp</summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>File content</summary>
        public string? Content { get; set; }
    }
}
