using AutoMapper;
using CodeExecution.Contracts;
using CodeExecutionSystem.Contracts.Data;

namespace CodeExecution.MappingProfiles
{
    public class CodeExecutionMappingProfile : Profile
    {
        public CodeExecutionMappingProfile()
        {
            CreateMap<TestingCode, PhpCode>();
            CreateMap<TestingCode, CPlusPlusCode>();
            CreateMap<TestingCode, PascalCode>();
            CreateMap<TestingCode, JavaScriptCode>();
        }
    }
}