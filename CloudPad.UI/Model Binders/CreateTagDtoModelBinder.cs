namespace NoteTakingApp.Model_Binders;

using Microsoft.AspNetCore.Mvc.ModelBinding;

public class CreateTagDtoModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueResult = bindingContext.ValueProvider.GetValue("tag");

        var instance = Activator.CreateInstance(bindingContext.ModelType);

        if (valueResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        // TagId=3;Name=urgent;Description=Requires immediate attention;CreatedAt=2025-02-27T12:34:56;UpdatedAt=2025-02-27T12:34:56;UserId=1;IsDeleted=false

        var rawValue = valueResult.FirstValue;

        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return Task.CompletedTask;
        }

        var pairs = rawValue.Split(";", StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
        foreach (var pair in pairs)
        {
            {
                var keyValue = pair.Split("=", StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);
                if (keyValue.Length != 2)
                {
                    continue;
                }

                var property = bindingContext.ModelType.GetProperty(keyValue[0]);

                if (property == null)
                {
                    continue;
                }

                try
                {
                    property.SetValue(instance, keyValue[1]);
                }
                catch (Exception)
                {
                    bindingContext.ModelState.AddModelError($"{bindingContext.ModelName}",
                        $"Invalid property for key {keyValue[0]}");
                }
            }
        }

        bindingContext.Result = ModelBindingResult.Success(instance);
        return Task.CompletedTask;
    }
}
