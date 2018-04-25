using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Containers.CompilationContainers
{
    public class CompileRequestContainer
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string Script { get; set; }
        public string StdIn { get; set; }
        public string Language { get; set; }
        public string VersionIndex { get; set; }
    }
}
