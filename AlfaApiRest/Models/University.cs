using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AlfaApiRest.Models
{
	
    public class University
    {
        public string alpha_two_code { get; set; }
        public List<string> domains { get; set; }
        public string country { get; set; }
        [JsonProperty("state-province")]
        public object state_province { get; set; }
        public List<string> web_pages { get; set; }
        public string name { get; set; }
    }
}

