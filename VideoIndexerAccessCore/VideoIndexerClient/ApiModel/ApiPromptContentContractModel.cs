﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiPromptContentContractModel
    {
        public string? partition { get; set; }
        public string? name { get; set; }
        public List<ApiPromptContentItemModel>? sections { get; set; }
    }
}
