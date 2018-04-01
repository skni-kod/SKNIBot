namespace SKNIBot.Core.Containers.OtherContainers
{
    public class LinkContainer
    {
        public DataObject Data { get; set; }
        public string Success { get; set; }
        public string Status { get; set; }
    }

    public class DataObject
    {

        public string Url { get; set; }
        public string Short_code { get; set; }
        public string Extension { get; set; }
    }
}
