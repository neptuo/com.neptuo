## ViewModelLocator.cs

Every WPF project I start with a simple static class where I'm going to place a static property for every root view model I need for design-time data binding.

```C#

internal static class ViewModelLocator
{
    private static MainViewModel main;

    public static MainViewModel Main
    {
        get 
        {
            if (main == null)
            {
                main = new MainViewModel(new MockService1(), new MockService2());
            }

            return main;
        }
    }
}

```

## XAML

Than in WPF view, I just use `DataContext` from `http://schemas.microsoft.com/expression/blend/2008` to bind to this static property during design-time.

```XML

<Window x:Class="MainWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
    xmlns:dd="clr-namespace:Project.DesignData"
    mc:Ignorable="d" d:DataContext="{x:Static dd:ViewModelLocator.Main}">

```

> Also, the important part is to use `mc` to ignore `d` namespace in runtime.

## Enable project code

For this approach to work, designer must have 'Enable project code' enabled.

![Enable project code](/Content/Images/Blog/wpf-design-data/enable-project-code.png)

## UWP
UWP doesn't support `x:Static`, but a slightly modified approach works as well.
1) Remove static from the `ViewModelLocator`.
2) In `App.xaml` create an instance.

```XML

<Application xmlns:dd="using:Project.DesignData">
    <Application.Resources>
        <dd:ViewModelLocator x:Key="ViewModelLocator" />

    ...

```

3) Use `Source` property of `{Binding}` extension.

```XML

<Page
    x:Class="MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    mc:Ignorable="d" d:DataContext="{Binding Main, Source={StaticResource ViewModelLocator}}">

```

## Simplify property definition inside ViewModelLocator.

After years of using this approach and being bored with the definition of static lazy instantiated property, I have created a simple helper. 
It uses `Dictionary` and behaves a slightly poor performant, but that isn't a concern for design-time support.

With the boilerplate code and a little help of newer C# syntax, you can get to a single line of code for each view model.

```C#

internal static class ViewModelLocator
{
    #region Infrastructure

    private static readonly Dictionary<object, object> storage = new Dictionary<object, object>();

    private static T Get<T>(Func<T> factory)
    {
        object key = factory.Method.GetHashCode();
        if (!storage.TryGetValue(key, out object instance))
            storage[key] = instance = factory();

        return (T)instance;
    }

    #endregion

    public static IService1 Services1 => Get(() => new MockService1());

    public static MainViewModel Main => Get(() => new MainViewModel(Service1));
}

```

## Summary

It's very easy approach and inside `ViewModelLocator` properties you can code any logic needed to initialize your model. 
The only drawback is that you need to define interfaces to provide fakes/mocks for your services passed to view models.

In 'Build History' Visual Studio extension I have also used a `System.Threading.Tasks` to animate progress in time inside WPF designer.

## Examples

- [WPF](https://github.com/maraf/GitExtensions.PluginManager/tree/master/src/PackageManager.UI/Views).
- [UWP](https://github.com/maraf/Money/tree/master/src/Money.UI.Universal/Views).
- [VSIX with animation](https://github.com/neptuo/Productivity/tree/master/src/Neptuo.Productivity.BuildHistory/UI/Views).