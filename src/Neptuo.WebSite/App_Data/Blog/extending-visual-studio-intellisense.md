# Extending Visual Studio IntelliSense

This is freaking easy. I have spend a lost of time investigating how to create a custom language service for Visual Studio. My master thesis resulted in a templating engine, which compiles templates to server-side .NET assemblies and to client-side javascript. For these templates I have extended HTML syntax with kind of WPF markup extensions. 

Well, the runtime support for quite an easy work, much harder work is to create a tooling, and specifically IntelliSense. By the time I was digging around I have found how easy is to extend build-in IntelliSense for C# and even HTML.

## C&#35;

First this first: Why would you extend an Intellisense for C#?

Well, for almost all cases the standard is totally sufficient. We are using it for kind a special use of explicit casting.

```C#

string localizedText = (L)"Hello, World!";

```

We use this syntax for a GetText inspired localization and we use a custom IntelliSense inside string literals for suggesting already known and localized texts.

### A side note about IntelliSense for string literals

Steps described later in the article are about providing items for IntelliSense. But in Visual Studio there is another component which is responsible for triggering or showing the IntelliSense. By default C# doesn't trigger IntelliSense for string literal and even if you complete the tutorial shown here, it will simply not show. I'm working on the second part where I will be covering this topic. For now, you can use [official tutorial for custom IntelliSense](https://msdn.microsoft.com/en-us/library/ee372314.aspx) where are described both components. The interface required to implement is a [IVsTextViewCreationListener](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.editor.ivstextviewcreationlistener.aspx).

### How to do it?

This one is really freaking easy. All you need to do is to register a `ICompletionSourceProvider` with content type `CSharp`.

```C#

[Export(typeof(ICompletionSourceProvider))]
[Order(After = "default")]
[ContentType("CSharp")]
internal class CSharpCompletionSourceProvider : ICompletionSourceProvider
{
    public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
    {
        return ...;
    }
}

```

> The `ExportAttribute` is part of the [MEF](https://msdn.microsoft.com/en-us/library/dd460648(v=vs.110).aspx). To make it automatically export inside Visual Studio we need to configure the extension that it contains MEF components. This is part of the `.vsixmanifest` and it is defined under "Assets" in the UI or using XML `<Asset Type="Microsoft.VisualStudio.MefComponent" ... />` element.

This component is created along side with standard C# provider. 
Using `ExportAttribute` we are telling MEF container (and Visual Studio) that this class should be registered as completion items provider. And by `ContentTypeAttribute` we are telling that the provider is capable of providing items for C# language.
Finally, by declaring `[Order(After = "default")]` we can even modify results from standard C# provider, as it is being used after standard provider.

When working with C# IntelliSense, we can inject current Visual Studio Roslyn workspace and get the exact projects, documents and source code trees as Visual Studio has.

```C#

[Import(typeof(VisualStudioWorkspace))]
internal VisualStudioWorkspace Workspace { get; set; }

```

Than we need to implementent `ICompletionSource` which is our IntelliSense items provider. 

```C#

internal class CSharpCompletionSource : ICompletionSource
{
    public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
    {
        var moniker = "CustomCSharpCompletion";
        var displayName = "Custom C# completions";
        var applicableTo = ...;
    
        CompletionSet newCompletionSet = new CompletionSet(
            moniker,
            displayName,
            applicableTo,
            result,
            null
        );
        newCompletionSet.SelectBestMatch();
        
        completionSets.Add(newCompletionSet);
    }
}

```

> A big note here is that we must install a same version of `Microsoft.CodeAnalysis` NuGet package as contained in the Visual Studio, or we can use a version from the GAC. Otherwise weird exceptions can raise in runtime.

This instance is hit everytime a Visual Studio needs a list of completion items. We can find a Roslyn node, where the cursor is, and provide items.

```C#

EXAMPLE: Find current node.

```

