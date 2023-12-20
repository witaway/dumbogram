﻿using System.Linq.Expressions;
using Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;
using Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.Enum;
using Dumbogram.Api.Persistence.Infrastructure.KeysetPagination.Internals;

namespace Dumbogram.Api.Persistence.Infrastructure.KeysetPagination;

public partial class Cursor<TEntity> where TEntity : BaseEntity
{
    public readonly List<IKeysetColumnValue<TEntity>> Values = new();
    public Keyset<TEntity> Keyset;
    public CursorType Type;


    public Cursor(Keyset<TEntity> keyset)
    {
        Keyset = keyset;
        Type = CursorType.Default;
    }

    protected Cursor(CursorType type)
    {
        Type = type;
    }

    public static Cursor<TEntity> First()
    {
        return new Cursor<TEntity>(CursorType.First);
    }

    public static Cursor<TEntity> Last()
    {
        return new Cursor<TEntity>(CursorType.Last);
    }

    private Cursor<TEntity> ColumnValue<TColumn>(
        Expression<Func<TEntity, TColumn>> propertySelector,
        TColumn value,
        string? name = null
    )
        where TColumn : IComparable
    {
        Values.Add(new KeysetColumnValue<TEntity, TColumn>(propertySelector, value, name));
        return this;
    }
}