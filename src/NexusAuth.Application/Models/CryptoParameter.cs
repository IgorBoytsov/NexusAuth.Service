namespace NexusAuth.Application.Models
{
    public sealed record class CryptoParameter
    {
        public byte[] Salt { get; set; }
        public int DegreeOfParallelism { get; set; }
        public int Iterations { get; set; }
        public int MemorySizeKb { get; set; }
    }
}