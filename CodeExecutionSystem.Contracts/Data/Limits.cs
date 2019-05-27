using System;
using System.ComponentModel.DataAnnotations;
using ByteSizeLib;
using Helpers;

namespace CodeExecutionSystem.Contracts.Data
{
    public class Limits
    {
        private const int DockerMinimumMemoryLimitInBytes = 5_000_000;

        public long TimeLimitInMs { get; set; } = TimeSpan.FromMinutes(1).TotalMilliseconds.ToLong();

        [Range(DockerMinimumMemoryLimitInBytes, int.MaxValue)]
        public long MemoryLimitInBytes { get; set; } = ByteSize.FromMegaBytes(50).Bytes.ToLong();
    }
}