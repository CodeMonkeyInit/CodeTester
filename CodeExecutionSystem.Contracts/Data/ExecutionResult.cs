namespace CodeExecutionSystem.Contracts.Data
{
    public enum ExecutionResult
    {
        Success = 0,
        KilledByTimeout = 1,
        KilledByMemoryLimit = 2
    }
}