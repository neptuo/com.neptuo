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

### How to do it?

This one is really freaking easy. All you need to do is to register a `ICompletionSourceProvider` with content type `CSharp`.

```C#

INTELLISENSE PROVIDER FOR C#

```

> The `ExportAttribute` is part of the (MEF)[https://msdn.microsoft.com/en-us/library/dd460648(v=vs.110).aspx]. To make it automatically export inside Visual Studio we need to configure the extension that it contains MEF components. This is part of the `.vsixmanifest` and it is defined under "Assets" in the UI or using XML `<Asset Type="Microsoft.VisualStudio.MefComponent" ... />` element.

This component is created along side with standard C# provider and by declaring `[Order(LAST)]` we can even modify results from standard C# provider, as it being used after standard provider.

When working with C# IntelliSense, we can inject current Visual Studio Roslyn workspace and the exact source code tree as Visual Studio does.

```C#

EXAMPLE: Inject current Roslyn workspace

```

Than we need to implementent `ICompletionSource` which is our IntelliSense items provider. 

```C#

EXAMPLE: ICompletionSource

```

> A big note here is that we must install a same version of `Microsoft.CodeAnalysis` NuGet package as contained in the Visual Studio, or we can use a version from the GAC. Otherwise weird exceptions can raise in runtime.

This instance is hit everytime a Visual Studio needs a list of completion items. We can find a Roslyn node, where the cursor is, and provide items.

```C#

EXAMPLE: Find current node.

```

