namespace CodeExecutionSystem.Contracts.Data
{
    public abstract class Code
    {
        public string Author { get; set; }
        
        public string Text { get; set; }

        public Language Language { get; set; }
    }
}