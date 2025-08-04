using FluentValidation.Results;

namespace NexusAuth.Application.Services.Abstractions
{
    public interface IValidationService
    {
        Task<ValidationResult> ValidateAsync<T>(T model);
    }
}