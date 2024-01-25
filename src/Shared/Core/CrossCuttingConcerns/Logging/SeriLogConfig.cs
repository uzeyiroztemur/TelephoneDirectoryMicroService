namespace Core.CrossCuttingConcerns.Logging
{
    public class SeriLogConfig
    {
        public string ProjectName { get; set; }
        public string ElasticUri { get; set; }
        public string Environment { get; set; }
        public string ElasticUser { get; set; }
        public string ElasticPassword { get; set; }
    }
}
