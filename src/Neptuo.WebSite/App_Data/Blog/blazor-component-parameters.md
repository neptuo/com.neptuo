> [Blazor](https://github.com/aspnet/Blazor) is an experimental Web UI framework from Microsoft. There are plenty of posts about so I'm not going to describe it and I'm directly heading to my topic.

Recently I was implementing [confirmation dialogs before deleting](https://github.com/maraf/Money/issues/149) an item. 

![Confirm Dialog in Action](/Content/Images/Blog/blazor-component-parameters/confirm.png)

As such dialog should be always the same except for message, I have created a component:

```html
<Confirm Message="@DeleteCategoryMessage?" YesClick="OnDeleteConfirmed" bind-IsVisible="@IsDeleteConfirmVisible" />
```

A typical 'codebehind' looked like:

```C#
private IKey deleteCategoryKey;
protected string DeleteCategoryMessage { get; set; }
protected bool IsDeleteConfirmVisible { get; set; }

protected void OnDeleteButtonClick(Category c) 
{
    deleteCategoryKey = c.Key;
    DeleteCategoryMessage = $"Do you really want to delete category '{c.Name}'";
    IsDeleteConfirmVisible = true;
}

protected async void OnDeleteConfirmed() 
{
    await DeleteCategoryAsync(deleteCategoryKey);

    deleteCategoryKey = null;
    DeleteCategoryMessage = null;
    IsDeleteConfirmVisible = false;
}

```

This would be needed everytime I would want to use confirmation dialog. It would be 90% the same in all use cases. 
Then I realized I can refactor it to a class and then I realized I can pass this whole class to `Confirm` component.

So the final component usage is:

```
<Confirm Context="@Delete" />
```

In codebehind I only need to create instance of [DeleteContext](https://github.com/maraf/Money/blob/master/src/Money.UI.Blazor/Models/Confirmation/DeleteContext.cs) class:

```C#
protected DeleteContext<CategoryModel> Delete { get; }

public void Initialize() 
{
    Delete = new DeleteContext<CategoryModel>()
    Delete.Confirmed += async model => await DeleteCategoryAsync(model.Key);
    Delete.MessageFormatter = model => $"Do you really want to delete category '{model.Name}'?";
}

protected void OnDeleteButtonClick(CategoryModel model)
{
    Delete.Model = model;
}

```

## Summary

Until last week I have never tough about passing complex objects to components, but it works so easily. 
The complete solution can be found in these files:

- [Confirm](https://github.com/maraf/Money/blob/master/src/Money.UI.Blazor/Components/Confirm.cshtml) - UI component using Bootstrap modal to show confirmation dialogs.
- [DeleteContext](https://github.com/maraf/Money/blob/master/src/Money.UI.Blazor/Models/Confirmation/DeleteContext.cs) - model containing boilerplate code for deleting with confirmation.
- [ConfirmContext](https://github.com/maraf/Money/blob/master/src/Money.UI.Blazor/Models/Confirmation/ConfirmContext.cs) - abstract base class required by `Confirm` component.
- [CategoriesBase](https://github.com/maraf/Money/blob/master/src/Money.UI.Blazor/Pages/CategoriesBase.cs) - typical usage.