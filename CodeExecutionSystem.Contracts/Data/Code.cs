using System.ComponentModel.DataAnnotations;

namespace CodeExecutionSystem.Contracts.Data
{
    public abstract class Code
    {
        public string Author { get; set; }

        [Required] public string Text { get; set; }

        [Required] public Language Language { get; set; }
    }
}