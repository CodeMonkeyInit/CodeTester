using System;
using ByteSizeLib;

namespace CodeExecutionSystem.Contracts.Data
{
    public class Limits
    {
        public long TimeLimitInMs { get; set; } = TimeSpan.FromSeconds(30).Milliseconds;

        public long MemoryLimitInBytes { get; set; } = Convert.ToInt64(ByteSize.FromMegaBytes(50).Bytes);
    }
}