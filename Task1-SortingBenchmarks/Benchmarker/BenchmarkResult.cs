using ADS_A2.input;
using Benchmarks;

namespace ADS_A2.Benchmarker;

public class BenchmarkResult
{
    public long MemoryUsage { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public InputFile FileProperties { get; set; }
    public string AlgorithmName { get; set; }
    // helpful to know the file size in megabytes for memory related benchmarks
    public double FileSizeMB { get; set; }
    
    public BenchmarkResult(long memoryUsage, TimeSpan executionTime, string algorithmName, InputFile file)
    {
        MemoryUsage = memoryUsage;
        ExecutionTime = executionTime;
        AlgorithmName = algorithmName;
        FileProperties = file;
        // get the file size in bytes then convert it to megabytes
        FileSizeMB = new FileInfo(file.FilePath).Length / (1024 * 1024);
    }
}