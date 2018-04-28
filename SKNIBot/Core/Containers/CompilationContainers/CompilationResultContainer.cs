namespace SKNIBot.Core.Containers.CompilationContainers
{
    public class CompilationResultContainer
    {
        public string output { get; set; }
        public int statusCode { get; set; }
        public string memory { get; set; }
        public string cpuTime { get; set; }
    }
}
