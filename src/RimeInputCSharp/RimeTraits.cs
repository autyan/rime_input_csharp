namespace RimeInputCSharp
{
    public class RimeTraits
    {
        public string ShardDataDir { get; set; }

        public string UserDataDir { get; set; }

        public string DistributionName { get; set; }

        public string DistributionCodeName { get; set; }

        public string DistributionVersion { get; set; }

        public string AppName { get; set; }

        public string[] Modules { get; set; }

        public int MinLogLevel { get; set; }

        public string LogDir { get; set; }

        public string PrebuiltDataDir { get; set; }

        public string StagingDir { get; set; }
    }
}
