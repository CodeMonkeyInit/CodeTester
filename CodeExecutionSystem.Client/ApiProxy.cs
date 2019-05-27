using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Configuration;

namespace CodeExecutionSystem.Client
{
    public class ApiProxy
    {
        protected void ValidateArgument( object argument)
        {
                Validator.ValidateObject(argument, new ValidationContext(argument));
            
        }
        
        protected string GetKeyFromConfiguration(string key, IConfiguration configuration)
        {
            return configuration[key]
                   ?? throw new ArgumentException(
                       $"{key} is not present in {nameof(IConfiguration)}");
        }

        protected Task<TResult> PostToApi<TResult>(string path, object argument)
        {
            ValidateArgument(argument);
            
            return path.PostJsonAsync(argument).ReceiveJson<TResult>();
        }
    }
}