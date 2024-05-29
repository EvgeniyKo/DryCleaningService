using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DryCleaningService.api.BaseModelBinder
{
    public abstract class BaseModelBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelType = typeof(T);
            if (bindingContext.ModelType != modelType)
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName, $"Can't convert value to non {modelType.Name} type");
                return Task.CompletedTask;
            }

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var valueStr = value.FirstValue;
            if (string.IsNullOrEmpty(valueStr))
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName, $"Can't convert null to {modelType.Name}");
                return Task.CompletedTask;
            }

            if (TryConvertValue(valueStr, out var result))
                bindingContext.Result = ModelBindingResult.Success(result);
            else
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Wrong date format");

            return Task.CompletedTask;
        }

        protected abstract bool TryConvertValue(string valueStr, out T value);
    }
}
