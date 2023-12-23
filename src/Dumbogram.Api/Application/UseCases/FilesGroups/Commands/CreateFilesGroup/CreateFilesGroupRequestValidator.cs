using FluentValidation;

namespace Dumbogram.Api.Application.UseCases.FilesGroups.Commands.CreateFilesGroup;

public class CreateFilesGroupRequestValidator : AbstractValidator<CreateFilesGroupRequest>
{
    public CreateFilesGroupRequestValidator()
    {
        RuleFor(request => request)
            .Must(FilesGroupTypeGetterShouldNotThrowException)
            .WithMessage("Incorrect group type");
    }

    // Checks wheter getter throws exception or not. If it throws - FilesGroupTypeName is incorrect
    private static bool FilesGroupTypeGetterShouldNotThrowException(CreateFilesGroupRequest request)
    {
        try
        {
            var _ = request.FilesGroupType;
            return true;
        }
        catch
        {
            return false;
        }
    }
}