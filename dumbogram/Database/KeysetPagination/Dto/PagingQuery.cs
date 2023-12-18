using System.Runtime.CompilerServices;
using Dumbogram.Models.Base;
using Dumbogram.Models.Chats;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dumbogram.Database.KeysetPagination.Dto;

public class PagingQuery : ICloneable
{
    [FromQuery(Name = "take")]
    public int? Take { get; set; }
    
    [FromQuery(Name = "order")]
    public string? Order { get; set; } = null!;

    [FromQuery(Name = "first")] 
    public bool First { get; set; }

    [FromQuery(Name = "last")] 
    public bool Last { get; set; }

    [FromQuery(Name = "prev_page_token")] 
    public string? PrevPageToken { get; set; }

    [FromQuery(Name = "next_page_token")] 
    public string? NextPageToken { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}

public class PagingQueryBaseValidator<TEntity> : AbstractValidator<PagingQuery> where TEntity : BaseEntity
{
    public PagingQueryBaseValidator()
    {
        RuleFor(q => q)
            .Must(q => OptionalsSpecifiedCount(q) <= 0)
            .WithMessage("Only one of first, last, prev_page_token, next_page_token is allowed");
    }

    private static int OptionalsSpecifiedCount(PagingQuery q)
    {
        var count = 0;
        if (q.First) count++;
        if (q.Last) count++;
        if (q.NextPageToken != null) count++;
        if (q.PrevPageToken != null) count++;
        return count;
    }
}
