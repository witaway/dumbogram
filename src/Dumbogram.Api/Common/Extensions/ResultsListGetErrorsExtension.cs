using FluentResults;

namespace Dumbogram.Api.Common.Extensions;

public static class ResultsListGetErrorsExtension
{
    public static IList<IError> GetErrors(this IList<Result> results)
    {
        return results
            .Where(result => result.IsFailed)
            .Select(result => result.Errors)
            .Aggregate(
                new List<IError>(),
                (acc, val) => acc.Concat(val).ToList()
            );
    }
}