namespace SKNIBot.Core.Commands.SpaceX
{
    public class RocketData
    {
        public string Rocket_Name { get; set; }
        public string Rocket_Type { get; set; }
        public SecondStageData Second_Stage { get; set; }
    }
}
