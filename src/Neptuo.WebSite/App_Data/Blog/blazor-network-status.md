When building a [PWA](https://developer.mozilla.org/en-US/docs/Web/Progressive_web_apps), which can work offline, it is helpful to know the current status. In [Money app](https://money.neptuo.com) we implemented a simplified bag for storing expenses when a user is offline. 

> Yes, it is true that we can do much better, but it requires a bigger change and kind of synchronization for CQRS, wich is not so easy in my eyes. So simplified offline UI for now.

There is a quite simple [online/offline API in javascript](https://developer.mozilla.org/en-US/docs/Web/API/NavigatorOnLine/Online_and_offline_events). In the rest of this post, I'm going to show you how to consume it from [Blazor](https://github.com/aspnet/Blazor).

## Component
Let's start for the end, here is a typical usage of the resulting component we want build.

```html
...

<Network>
    <Online>
        ✅ We are live.
    </Online>
    <Offline>
        ❌ Sorry, but the internet is gone.
    </Offline>
</Network>

...
```

Razor template for this component is really simple.

```C#
@if (IsOnline)
{
    @Online
}
else 
{
    @Offline
}
```

Because it might be usable to have a service that other components can inject to get the information about the network status, most of the "code-behind" for the `<Network />` component just consumes the service `NetworkState`.

```C#
public partial class Network : IDisposable
{
    [Inject]
    internal NetworkState State { get; set; }

    [Parameter]
    public RenderFragment Online { get; set; }

    [Parameter]
    public RenderFragment Offline { get; set; }

    protected bool IsOnline => State.IsOnline;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        State.StatusChanged += StateHasChanged;
    }

    public void Dispose()
    {
        State.StatusChanged -= StateHasChanged;
    }
}
```

The component class declares parameters, injects the service and subscribes on the status chage event. Also it implements a `IDisposable` to unsubscribe from the event when removed from the UI.

## Service

As we saw in the `<Network />` component, the service serves two purposes:
- Get a current status.
- Get a notification when the status changes.

```C#
public class NetworkState
{
    public event Action StatusChanged;

    public NetworkState(IJSRuntime jsRuntime)
        => jsRuntime.InvokeVoidAsync("Network.Initialize", DotNetObjectReference.Create(this));

    [JSInvokable("Network.StatusChanged")]
    public void OnStatusChanged(bool isOnline)
    {
        if (IsOnline != isOnline)
        {
            IsOnline = isOnline;
            StatusChanged?.Invoke();
        }
    }

    public bool IsOnline { get; protected set; } = true;
}
```

The code is just a C# wrapper around native javascript API. It calls a javascript function in the constructor to bind to javascript events and get the current state. Passing `this` to the javascript allows it to be called back when there is a change. The function that can be called is `OnStatusChanged` (marked as `[JSInvokable]`). It just checks if there is really a change and if it fires the event.

> As you will see bellow, this class indirectly binds to javascript events. I'm using this class as a singleton, because it fits well for this kind of a service. If you want other lifecycle (transient or scoped), just implement a `IDisposable` and call javascript interop to unbind from events.

## Javascript (interop)

Lastly, this is the javascript for getting network information.

```javascript
window.Network = {
    Initialize: function (interop) {
        function handler() {
            interop.invokeMethodAsync("Network.StatusChanged", navigator.onLine);
        }

        window.addEventListener('online', handler);
        window.addEventListener('offline', handler);

        if (!navigator.onLine) {
            handler(navigator.onLine);
        }
    }
};
```

It's very simple. It declares a function to be called when DOM "online" or "offline" event is fired. This function uses passed .NET object (an object reference holding the instance of the `NetworkState`), calls its JSInvokable method and passes current network status.

Lastly, it raises the handler if the current status offline (because the `NetworkState` service optimistically expects we are online - see the last line of the `NetworkState`).

## Summary
That's is it. Blazor makes it really easy to create a reusable components and if they serve a single purpose ([SRP](https://en.wikipedia.org/wiki/Single_responsibility_principle)), you can easily share them between projects.

## Related files
- [Network.razor](https://github.com/maraf/Money/blob/e35ed6debbfde50c8aebaa504a2f24a0cebc55c7/src/Money.UI.Blazor/Components/Network.razor)
- [Network.razor.cs](https://github.com/maraf/Money/blob/e35ed6debbfde50c8aebaa504a2f24a0cebc55c7/src/Money.UI.Blazor/Components/Network.razor.cs)
- [Network part in site.js](https://github.com/maraf/Money/blob/e35ed6debbfde50c8aebaa504a2f24a0cebc55c7/src/Money.UI.Blazor/wwwroot/js/site.js#L20-L33)