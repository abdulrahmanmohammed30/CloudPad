using CloudPad.Core.Dtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NoteTakingApp.Model_Binders;

public class CreateTagDtoModelBinderV2 : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var provider = bindingContext.ValueProvider;
        var tag = new CreateTagDto();


        var name = provider.GetValue("name").FirstValue;
        if ( name != null)
        {
            tag.Name = name;
        }

        var description = provider.GetValue("description").FirstValue; 
        if ( description != null)
        {
            tag.Description = description;
        }
        
        bindingContext.Result = ModelBindingResult.Success(tag);
        return Task.CompletedTask;
    }
}