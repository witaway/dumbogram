using System.Runtime.CompilerServices;
using Dumbogram.Models.Messages;

namespace Dumbogram.Database.Pagination;

public class MessagesPagingQuery
{
    public int Take { get; set; }
    public bool? First { get; set; }
    public bool? Last { get; set; }
    public int? Before { get; set; }
    public int? After { get; set; }

    public PagingOptions<Message> GetPagingOptions()
    {
        PagingOptions<Message>.Before(x => x.Id, 1).Take(10);
        throw new SwitchExpressionException();
    }
}