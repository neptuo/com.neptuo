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

We first used behaviors in our `WebStack`.
