# Extending Visual Studio IntelliSense

This is freaking easy. I have spend a lost of time investigating how to create a custom language service for Visual Studio. My master thesis resulted in a templating engine, which compiles templates to server-side .NET assemblies and to client-side javascript. For these templates I have extended HTML syntax with kind of WPF markup extensions. 

Well, the runtime support for quite an easy work, much harder work is to create a tooling, and specifically IntelliSense. By the time I was digging around I have found how easy is to extend build-in IntelliSense for C# and even HTML.

## C#

This one is really freaking easy. All you need to do is to register a `XXX` with content type `CSharp`.

```C#

INTELLISENSE PROVIDER FOR C#

```

This component is created along side with standard C# provider and by declaring `[Order(LAST)]` we can than even modify results from standard C# provider.
