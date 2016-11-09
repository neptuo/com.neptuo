## Building processing pipeline with behaviors

I hadn't planned to write this post, but after I have read post from Jeremy D. Miller from Lost techies - about pipelines in MediatoR - I had to.

We are using the request -> handler (-> response) design a lost. The handler has a single method responsible for processing request (and provinding response). It can be used nearly everwhere and for everything. Typically we have a component called `dispatcher` which hides the registration of handlers for particular request types. 

One of the examples can be validation pipeline. We have a 

```C#

public interface IValidationDispatcher
{
    IValidationResult Validate<T>(T model);
}

```

Which is used from the code everywhere validation is requested. This component hides registered handler for each model type.

```C#

public interface IValidationHandler<T>
{
    IValidationResult Validate(T model);
}

```

With this architecture, you can create handlers for cross-cutting services. There are typically composed from inner handler that does the real job and some logic which decorates the execution. Example can be authentication or, for the validation pipeline, some validation based on implemented interface.

### Behaviors

We have taken this pipeline composing a step further and created a library called `Behaviors`. It is a kind of interception for pipeline. Behavior can't only be a attribute, like in typical interception, it could also be an implemented interface or simply configured behavior for all handlers etc.

We first used behaviors in our `WebStack`. The main design goal was to create a simple class for every visible HTTP endpoint. Start with a POCO.

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

In the `WebStack` there where behaviors of two types - those that provide values (from request) and those that process output values. After using this design for a while, we have extracted it to general pipeline library called [Neptuo.Behaviors](https://github.com/neptuo/Framework/wiki/Behaviors).

### Execution

We ship two modes for executing behaviors. First one is based on reflection and uses reflection metadata everytime an instance of pipeline is required. We use it during bootstrap, because there is no need for executing it twice during application lifetime.

Second compiles tiny proxy of target handler and so, no reflection is used in the runtime. The performace of these pipelines is the as directly composing handlers together. On the other, code generation must be enabled on the .NET runtime.

### Summary

These single method pipelines are very powerful. They are like building blocks that can composed to gather to reuse and build bigger and bigger blocks. Behaviors, or interception, are even better, because they are independent of the target pipeline, and so, we can reuse reprocessing behavior on pipelines of various kinds.

### Links

 - [MediatR Pipeline Examples](https://lostechies.com/jimmybogard/2016/10/13/mediatr-pipeline-examples/)
 - [Neptuo.Behaviors documentation](https://github.com/neptuo/Framework/wiki/Behaviors)
 - [Neptuo.Behaviors on NuGet](https://www.nuget.org/packages/Neptuo.Behaviors/)
