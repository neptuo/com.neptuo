# Naming things

This one is about naming things in C#. Naming things is hard. Naming things consistently is very hard. And consistently naming things when  colaborating with other developers is hell.

In years of developing applications on .NET platform, we have slowly adapted some naming conventions. This post covers them from project names to class member names.

## Projects and assemblies

Everything here is based on used architecture and type of project. When developing framework / reusable library we choose a bit different naming schema in comparison to application development.

When developing a reusable library we are always separating interfaces / contracts from actual implementation. This rule is bit violated in cases of simple / reference free implementations. These reference free implementations are typically placed in the contracts assembly.

A typical example here could be our `Formatters` library. A main assembly is called `Neptuo.Formatters` and it contains contracts like `ISerializer` or `ISerializerContext`. Also this assembly contains default implementation of serializer context contract, called `DefaultSerializerContext`.

Then, concrete implementation of formatters for JSON, which references `Newtonsoft.Json`, is placed in an assembly called `Neptuo.Formatters.Json`. To this point, it is a classic separation of contract and implementation. We call these assemblies 'implementation assembly'. In a namespace of implementation assemblies we skip the implementation name. So, default namespace of JSON formatters is `Neptuo.Formatters`, like the namespace of the contracts assembly. And all classes is this assembly starts with prefix `Json`. So full name of the JSON implementation of formatter is `Neptuo.Formatters.JsonFormatter` and sits in the `Neptuo.Formatters.Json` assembly. 

This naming convention makes easier implementation discoverability. When mapping `IFormatter` to concreate implementation, typically to IoC container in a composition root, we already have using for `Neptuo.Formatters` and simply be referencing `Neptuo.Formatters.Json` assembly / package, we have intellisence support `JsonFormatter` implementation.

Other implementations for XML and our 'composite' are organized in the same way.

Another example would be `Neptuo.Activators.IDependencyContainer` (from `Neptuo` assembly) and concrete implementations are `Neptuo.Activators.UnityContainer` (from `Neptuo.Activators.Unity` assembly) and `Neptuo.Activators.SimpleContainer` (from `Neptuo.Activators.Simple` assembly).
