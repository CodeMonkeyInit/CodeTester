using AutoMapper;
using CodeAnalysis.CodeAnalyzers.JavaScript.EsLint;
using CodeExecutionSystem.Contracts.Data;

namespace CodeAnalysis.MappingProfiles
{
    public class Eslint: Profile
    {
        public Eslint()
        {
            CreateMap<Details, AnalysisResult>();
        }
    }
}