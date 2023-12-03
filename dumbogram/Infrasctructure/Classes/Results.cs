using FluentResults;

namespace Dumbogram.Infrasctructure.Classes;

public class Results<TLabel, TValue>
{
    private readonly Dictionary<Identity, Result<TValue>> _results = new();

    public void Add(TLabel label, Result<TValue> result)
    {
        var savedAlready = _results.Count;
        var identity = new Identity(savedAlready + 1, label);

        _results.Add(identity, result);
    }

    public bool AllSucceeded()
    {
        return !_results.Any(x => x.Value.IsFailed);
    }

    public bool AllFailed()
    {
        return !_results.Any(x => x.Value.IsSuccess);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // ALL

    public IEnumerable<Result<TValue>> GetAllResults()
    {
        return _results
            .Select(x => x.Value);
    }

    public IEnumerable<IdentityWithResult> GetAllResultsWithIdentity()
    {
        return _results
            .Select(x => new IdentityWithResult(x.Key, x.Value));
    }

    // -----------------------------------------------------------------------------------------------------------------
    // SUCCEEDED

    public IEnumerable<IdentityWithValue> GetSucceededValuesWithIdentity()
    {
        return _results
            .Where(x => x.Value.IsSuccess)
            .Select(x => new IdentityWithValue(x.Key, x.Value.Value));
    }

    public IEnumerable<TValue> GetSucceededValues()
    {
        return _results
            .Where(x => x.Value.IsSuccess)
            .Select(x => x.Value.Value);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // FAILED

    public IEnumerable<IdentityWithErrors> GetFailedErrorsWithIdentity()
    {
        return _results
            .Where(x => x.Value.IsFailed)
            .Select(x => new IdentityWithErrors(x.Key, x.Value.Errors));
    }

    public IEnumerable<List<IError>> GetFailedErrors()
    {
        return _results
            .Where(x => x.Value.IsFailed)
            .Select(x => x.Value.Errors);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Record types

    public record IdentityWithResult(Identity Identity, Result<TValue> Result);

    public record IdentityWithValue(Identity Identity, TValue Value);

    public record IdentityWithErrors(Identity Identity, List<IError> Errors);

    public record Identity(int Number, TLabel Label);
}