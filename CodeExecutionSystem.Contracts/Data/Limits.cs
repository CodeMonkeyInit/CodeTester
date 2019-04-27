using System;
using System.ComponentModel.DataAnnotations;
using ByteSizeLib;

namespace CodeExecutionSystem.Contracts.Data
{
    public class Limits
    {
        public long TimeLimitInMs { get; set; } = TimeSpan.FromSeconds(30).TotalMilliseconds.ToLong();

        [Range(5_000_000, int.MaxValue)]
        public long MemoryLimitInBytes { get; set; } = Convert.ToInt64(ByteSize.FromMegaBytes(50).Bytes);
    }
}