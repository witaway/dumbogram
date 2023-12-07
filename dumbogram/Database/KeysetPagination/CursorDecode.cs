using System.Text;
using System.Text.Json;
using Dumbogram.Models.Base;

namespace Dumbogram.Database.KeysetPagination;

public partial class Cursor<TEntity> where TEntity : BaseEntity
{
    public static Cursor<TEntity> Decode(KeysetOrder<TEntity> keysetOrder, string token,
        KeysetPaginationDirection direction, int take)
    {
        var data = Convert.FromBase64String(token);
        var decodedToken = Encoding.UTF8.GetString(data);

        var cursor = new Cursor<TEntity>(keysetOrder, direction, take);

        var jsonElement = JsonSerializer.Deserialize<JsonElement>(decodedToken);

        foreach (var column in keysetOrder.Columns)
        {
            var propertyName = column.Path;
            var propertyElement = jsonElement.GetProperty(propertyName);

            if (column is KeysetColumnOrder<TEntity, int> int32Column)
            {
                var propertySelector = int32Column.PropertySelectorExpression;
                var propertyValue = propertyElement.GetInt32();
                cursor.ColumnValue(propertySelector, propertyValue);
            }
            else if (column is KeysetColumnOrder<TEntity, long> int64Column)
            {
                var propertySelector = int64Column.PropertySelectorExpression;
                var propertyValue = propertyElement.GetInt64();
                cursor.ColumnValue(propertySelector, propertyValue);
            }
            else if (column is KeysetColumnOrder<TEntity, string> stringColumn)
            {
                var propertySelector = stringColumn.PropertySelectorExpression;
                var propertyValue = propertyElement.GetString()!;
                cursor.ColumnValue(propertySelector, propertyValue);
            }
            else if (column is KeysetColumnOrder<TEntity, DateTimeOffset> dateTimeOffsetColumn)
            {
                var propertySelector = dateTimeOffsetColumn.PropertySelectorExpression;
                var propertyValue = propertyElement.GetDateTimeOffset();
                cursor.ColumnValue(propertySelector, propertyValue);
            }
            else if (column is KeysetColumnOrder<TEntity, Guid> guidColumn)
            {
                var propertySelector = guidColumn.PropertySelectorExpression;
                var propertyValue = propertyElement.GetGuid();
                cursor.ColumnValue(propertySelector, propertyValue);
            }
        }

        return cursor;
    }
}