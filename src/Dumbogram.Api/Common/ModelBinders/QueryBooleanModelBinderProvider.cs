using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dumbogram.Api.Common.ModelBinders;

internal class QueryBooleanModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(bool) &&
            context.BindingInfo.BindingSource != null &&
            context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource.Query))
        {
            return new QueryBooleanModelBinder();
        }

        return null;
    }
}