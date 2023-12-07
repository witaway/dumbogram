using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Dumbogram.Models.Base;

namespace Dumbogram.Database.KeysetPagination;

public partial class Cursor<TEntity> where TEntity : BaseEntity
{
    public static string Encode(KeysetOrder<TEntity> keysetOrder, TEntity entity)
    {
        var jsonElement = new JsonObject();

        foreach (var column in keysetOrder.Columns)
        {
            var propertyName = column.Path;

            if (column is KeysetColumnOrder<TEntity, int> int32Column)
            {
                var propertyValue = int32Column.PropertySelector(entity);
                jsonElement.Add(propertyName, propertyValue);
            }
            else if (column is KeysetColumnOrder<TEntity, long> int64Column)
            {
                var propertyValue = int64Column.PropertySelector(entity);
                jsonElement.Add(propertyName, propertyValue);
            }
            else if (column is KeysetColumnOrder<TEntity, string> stringColumn)
            {
                var propertyValue = stringColumn.PropertySelector(entity);
                jsonElement.Add(propertyName, propertyValue);
            }
            else if (column is KeysetColumnOrder<TEntity, DateTimeOffset> dateTimeOffsetColumn)
            {
                var propertyValue = dateTimeOffsetColumn.PropertySelector(entity);
                jsonElement.Add(propertyName, propertyValue);
            }
            else if (column is KeysetColumnOrder<TEntity, Guid> guidColumn)
            {
                var propertyValue = guidColumn.PropertySelector(entity);
                jsonElement.Add(propertyName, propertyValue);
            }
        }

        var token = JsonSerializer.Serialize(jsonElement);
        var tokenBytes = Encoding.UTF8.GetBytes(token);
        var tokenEncoded = Convert.ToBase64String(tokenBytes);

        return tokenEncoded;
    }
}