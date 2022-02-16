using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.SourceQuery.Models.ServerInfo
{
    public class RuleResponse
    {
        /// <summary>
        /// Always equal to 'E' (0x45)
        /// </summary>
        public byte Header { get; set; }

        /// <summary>
        /// Dictionary of Rules on the Server
        /// </summary>
        public Dictionary<string, string> Rules { get; set; }
    }
}