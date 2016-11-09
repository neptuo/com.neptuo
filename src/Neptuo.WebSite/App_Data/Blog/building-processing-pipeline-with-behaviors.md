## Building processing pipeline with behaviors

I hadn't planned to write this post, but after I have read post from Jeremy D. Miller from Lost techies - [about pipelines in MediatoR](https://lostechies.com/jimmybogard/2016/10/13/mediatr-pipeline-examples/) - I had to.

We are using the request -> handler (-> response) design a lot. The handler has a single method responsible for processing request (and optionaly provinding response). It can be used nearly everywhere and for everything. Typically we have a component called `dispatcher` which hides the registration of handlers for particular request types. 

One of the examples can be validation pipeline. We have a 

```C#
public interface IValidationDispatcher
{
    IValidationResult Validate<T>(T model);
}
```

A 'dispatcher' is used from the code everywhere validation is required. This component hides registered handler for each model type.

```C#

public interface IValidationHandler<T>
{
    IValidationResult Validate(T model);
}

```

> Behind dispatcher interface, there could any type of delegation and for simple applications, there could be a nasty-simple switch based on model type. But for more complex application, it is not-an-option to register separate handlers.

With this architecture, you can create handlers for cross-cutting services. There are typically composed from inner handler that does the real job and some logic which decorates the execution. Example can be authentication or, for the validation pipeline, some validation based on implemented interfaces, etc. This is described in the mentioned post from Jeremy D. Miller.

### Behaviors

We have taken this pipeline composing a step further and created a library called `Behaviors`. It is a kind of interception for pipelines. Most interception behaviors are attributes, but behaviors were originally marker interfaces. That we extend them to attributes and even globally registered components.

We first used behaviors in our `WebStack`. The main design goal was to create a simple class for every visible HTTP endpoint, starting with a POCO.

```C#
public class ProductList
{  }
```

Add a supported HTTP method, in this case a GET.

```C#
[Url("~/api/products")]
public class ProductList : IGet
{  
    public void Get()
    {
        ...
    }
}
```

So, everytime someone access the URL (configured using attribute behavior), an instance of this class is created and `Get` method is executed. Now, let's provide some output. We want to return an enumeration of `ProductModel`.

```C#
public class ProductList : IGet, IOutput<IEnumerable<ProductModel>>
{
    public IEnumerable<ProductModel> Output { get; private set; }

    public void Get()
    {
        Output = Enumerable.Empty<IEnumerable<ProductModel>>();
    }
}
```

In the `WebStack` there where behaviors of two types - those that provide values (from the request) and those that process output values (and writes them to the response). After using this design for a while, we have extracted it to a general purpose pipeline library called [Neptuo.Behaviors](https://github.com/neptuo/Framework/wiki/Behaviors).

### Execution

We ship two modes for executing behaviors. First one is based on reflection and uses reflection metadata everytime an instance of pipeline is required. We use it for example during bootstrap, because there is no need for executing it twice in the application lifetime.

Second compiles on the fly a tiny proxy of a target handler and so, no reflection is used in the runtime. The performace of these pipelines is the same as directly composing handlers together. On the other, code generation must be enabled on the .NET runtime.

### Summary

These single method pipelines are very powerful. They are like building blocks that can composed to gather and build bigger and bigger blocks. Behaviors, or interception, are even better, because they are independent of the target pipeline, and so, we can reuse for example reprocessing behavior on pipelines of various kinds.

As the public execution method is always defined be an interface, we can can compose behaviors like any hand writtern pipeline and compile code for it or the fly. This is possible just because pipelines has always interface and because this interface is uniform, we don't need to write it for every service/component, we don't need to spend a time doing it and can reuse single compiled pipeline library.

### Links

 - [MediatR Pipeline Examples](https://lostechies.com/jimmybogard/2016/10/13/mediatr-pipeline-examples/)
 - [Neptuo.Behaviors documentation](https://github.com/neptuo/Framework/wiki/Behaviors)
 - [Neptuo.Behaviors on NuGet](https://www.nuget.org/packages/Neptuo.Behaviors/)
