# Naming things

This one is about naming things in C#. Naming things is hard. Naming things consistently is very hard. And consistently naming things when  colaborating with other developers is hell.

In years of developing applications on .NET platform, we have slowly adapted some naming conventions. This post covers them from project names to class member names.

## Projects and assemblies

Everything here is based on used architecture and type of project. When developing framework / reusable library we choose a bit different naming schema in comparison to application development.

### Frameworks & reusable libraries

When developing a reusable library we are always separating interfaces / contracts from actual implementation. This rule is bit violated in cases of simple / reference free implementations. These reference free implementations are typically placed in the contracts assembly.

A typical example here could be our `Formatters` library. A main assembly is called `Neptuo.Formatters` and it contains contracts like `ISerializer` or `ISerializerContext`. Also this assembly contains default implementation of serializer context contract, called `DefaultSerializerContext`.

Then, concrete implementation of formatters for JSON, which references `Newtonsoft.Json`, is placed in an assembly called `Neptuo.Formatters.Json`. To this point, it is a classic separation of contract and implementation. We call these assemblies 'implementation assembly'. In a namespace of implementation assemblies we skip the implementation name. So, default namespace of JSON formatters is `Neptuo.Formatters`, like the namespace of the contracts assembly. And all classes is this assembly starts with prefix `Json`. So full name of the JSON implementation of formatter is `Neptuo.Formatters.JsonFormatter` and sits in the `Neptuo.Formatters.Json` assembly. 

This naming convention makes easier implementation discoverability. When mapping `IFormatter` to concreate implementation, typically to IoC container in a composition root, we already have using for `Neptuo.Formatters` and simply be referencing `Neptuo.Formatters.Json` assembly / package, we have intellisence support `JsonFormatter` implementation.

Other implementations for XML and our 'composite' are organized in the same way.

Another example would be `Neptuo.Activators.IDependencyContainer` (from `Neptuo` assembly) and concrete implementations are `Neptuo.Activators.UnityContainer` (from `Neptuo.Activators.Unity` assembly) and `Neptuo.Activators.SimpleContainer` (from `Neptuo.Activators.Simple` assembly).

### Applications

This section is driven by used architecture. 

When we are using classic 3L architesture, we are creating vertical slices by module and splitting each module by layers. If we have a module for managing products and other for managing orders, we end up with 6 projects:

- Products.Data
- Products.Business
- Products.Presentation
- Orders.Data
- Orders.Business
- Orders.Presentation

Each project will contain classes for its module and layer. References are possible from `Data` to `Business` and from `Business` to `Presentation`. This applyes also for references between modules, with an extention to same layers - `Data` to `Data`, `Business` to `Business` and `Presentation` to `Presentation`. We are also trying make most inter module comunication on `Business` layer, but it's not always possible. Namespaces are organized similarly as for frameworks, a layer name is skipped. 

***Data*** layer contains classes for data access. These are typically generated as there is not much of code, that must be hand written. In 3L we typically reference here concrete data access libraries / classes.

***Business*** layer contains business rules, typically in the form of stateless service classes and POCO models.

***Presentation*** layer contains code required for exposing business logic from module to the outside world. Here are typically module specific UI controls, either for web, desktop, mobile, etc, or web services, cron jobs etc.

When we are using onion architecture (which is prefered) we are creating one main project / assembly for domain and adding as many as required projects for communication with outside world. For the same example as for 3L architecture:

- Products
- Products.EntityFramework
- Products.Presentation
- Products.ExternalCommunication
- Products.ScheduledTasks
- Orders
- Orders.EventSourcing
- Orders.ExternalCommunication

A main difference comes from the direction of references, defined by onion architecture. Here is the domain project in the center of everthing and all other projects / assemblies references it. 

> References between modules are here theoretically shrinked to references between domain projects, and better realized using `domain`-to-`domain` projects. Such project can be named `ProductsOrders` and this projects takes care of communication between `Products` and `Orders`, none of `Products` nor `Orders` references this project. But in case of `Products` and `Orders`, its cleare that there could be reference from `Products` to `Orders`.

## Namespaces

We like namespaces and we use them a lot. We are always trying to consolidate classes that stick to together to its namespace. On the other hand, we are also trying to minimize namespaces to contain only information, that is not present somewhere else. These are the rules:

### Removing layer name

We are never placing a name of the layer to the namespace. This information is already encoded in the project project. See the examples from previous section, `Data` and `Business` and `Presentation` / `UI` - these information is already contained in the project structure. So, do not include it in the namespace.

Another example is that we are trying not to use namespaces like `Services`, `Models`, `Entities`. Instead of placing such information in the namespace, we are trying to use projects to separate these "layers" and keep namespaces "clear", containing module informations only.

### Stop repeating yourself

When previous previous name is saying `Products`, do not repeat this information is sub namespaces. Using namespaces like `Products.ProductNotes` only makes namespaces longer and harder read, use `Products.Notes` instead.

### Plural, almost always

Most of out namespaces are in plural. This is the way, for us, to distinguish between classes and namespaces. This is the most weak in our code and for some scenarios we are breaking it. We are usign words like `Inventory` or `ContentManagement` for namespaces, but we are never using them elsewhere for class names.

## Classes & interfaces

When it comes to classes, we are also trying not to repeat information that is contained in the namespace. This is true in almost all cases. Some exceptions are classes that are placed in a namespace, but mainly used from another one, where their name could be ambiguous. An extra example could `Orders` module (in namespace `Orders`), where could be a service. This service will be called `OrderService` even the information about "Ordering" is also in the namespace. Otherwise we would end up with class called `Service`, which would be useless from other modules perspective.

We are separating contracts / interfaces for reading and writing. In most cases, a component that needs to read some data doesn't need to write them. For these scenarios we are heavily using name-patterns `***Provider`, which contains methods for reading data, and `***Collection` / `***Repository` which inherits provider contract and addes methods for chaning state.

## Member names

We don't much rules here. A common advise is to be as descriptive as possible, but don't go with too much detail, and don't bring up much implementation detail, where it is not necessary.

### Find / Get / TryGet

One of rules we use is method naming based on whether it can return null or not. When it comes to getting object from a component we always prefix method based on behavior for cases where response can't be provided.

We use a lot standard .NET pattern for try-getting a value. Such method returns boolean and has an extra out parameter for a result. Beside these, we offen offer a method which returns null when a result can't be provided. Such methods have always name prefix with Find. When a developer calls such method he/she must always check a result for a null value. When a method name starts with Get, it always returns a value or throws a exception, so developer doesn't have to check for nulls. This also brings an exception standartization for methods, because a developer doesn't have to think about which exception to throw.