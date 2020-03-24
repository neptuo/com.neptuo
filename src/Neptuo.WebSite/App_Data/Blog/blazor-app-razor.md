For almost two years of playing with Blazor, `App.razor` (originally `App.cshtml`) was just a required placeholder to make the application work. This year I have found that it can be really useful and you can realize many application-wide concepts in there.

## A bit of theory

What is `App.razor` and how does it work? Well, Blazor has concept of "root components" and they are registered in `Program.cs`.

```C#
...

builder.RootComponents.Add<App>("app");

...

```

A typical line. What it does is that it registers a Blazor component `App` to be matched with `"App"`. What is this string literal? Well, you have it your `index.html`.

```html
...
<body>
    <app>
        Loading...
    </app>
</body>
...

```

Yes. Root component registrations map HTML elements from your `index.html` to Blazor components. This is the bootstrap of your application. You can have as many root components as you want.

## A default content

Now that we know how these root components work, let's look at the typical content of `App` component.

```html
<Router AppAssembly="typeof(Program).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="routeData" />
    </Found>
    <NotFound>
        Sorry, there's nothing at this address.
    </NotFound>
</Router>

```

If I simplify it a bit, it uses `Router` component to parse URL into `routeData` and if there is a match, it uses `RouteView` component to dynamicly display a mapped component. Otherwise it shows "Not found" content.

Ok. When authentication arrived into the Blazor framework, content of the default `App.razor` changed a bit.

```html
<CascadingAuthenticationState>
    <Router AppAssembly="@typeof(Program).Assembly">
        <Found Context="routeData">
            <AuthorizeRouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)">
                <NotAuthorized>
                    Sorry, your are not authorized to view this page.
                </NotAuthorized>
            </AuthorizeRouteView>
        </Found>
        <NotFound>
            Sorry, there's nothing at this address.
        </NotFound>
    </Router>
</CascadingAuthenticationState>

```

Now, all content is wrapped in `CascadingAuthenticationState` component just to provide an instance of `AuthenticationState`. More importantly from the point of view of this post, `RouteView` is swapped by `AuthorizeRouteView` and if the user is not authorized, a "Not authorized" content is shown. 

**Do you see the pattern?** Anytime you want to do something application wide, `App` component is the place to implement it. It doesn't matter on what URL the user is, it applies everywhere.

## How are we using it now?

A snippet of `App.razor` from the [Money app](http://github.com/maraf/Money).

```html
<CascadingAuthenticationState>
    <Router AppAssembly="@typeof(Program).Assembly">
        <Found Context="routeData">
            <Network>
                <Online>
                    <VersionChecker>
                        <ApiHubConnectionChecker>
                            <AuthorizeRouteView RouteData="routeData" DefaultLayout="typeof(Layout)">
                                <NotAuthorized>
                                    <Login />
                                </NotAuthorized>
                                <Authorizing>
                                    <AuthorizationProgress />
                                </Authorizing>
                            </AuthorizeRouteView>
                        </ApiHubConnectionChecker>
                    </VersionChecker>
                </Online>
                <Offline>
                    <LayoutView Layout="@typeof(Layout)">
                        <ExpenseBag />
                    </LayoutView>
                </Offline>
            </Network>
        </Found>
        <NotFound>
            <LayoutView Layout="@typeof(Layout)">
                <NotFound />
            </LayoutView>
        </NotFound>
    </Router>
</CascadingAuthenticationState>

```

A lot of content. We are using standard `CascadingAuthenticationState`, `Router` and `AuthorizeRouteView`. 

What we have added is:

### Network status

As Money app has some basic offline behavior, we have wrapped `AuthorizeRouteView` in `Network` component to check current status and if the user is offline, we display a simplified (means different) UI. This offline UI allows only creation of expsenses. Other functionalities are not supported for now.

### Version compatibility checking

When we added an offline support, a next category of problems appeared. The user can have an older version, we need to ensure that the client app and api versions are aligned. So we are sending the API version in every server response and if incompatibility is detected, the `VersionChecker` component stops rendering the usual content and a message is displayed instead.

### SignalR status

As the application is implemented using CQRS, we need background SignalR connection to get notifications about changes (= events). If this connection is interrupted, the state of the application might not be correct. So we need to inform the user that something is not working as it should. As with the `VersionChecker` component, the `ApiHubConnectionChecker` also displays an error content when something is wrong.

## Summary

For almost two years I completly ignored the `App.razor`, now I look at it as powerful tool. I'm also working on a non-open project, which is truly single-page app. There are no URLs because based on the global app state, only one screen is valid for all users. Gues what... everything is happening in the `App.razor`.

> When I'm saying 'everything', I mean the `App.razor` uses plenty of components, but nothing "magical" as routing happens (all components are directly used inside razor files, no "dynamic" loading).

### Referenced files from Money app
- [App.razor](https://github.com/maraf/Money/tree/c2e058ea19e47a3fd8c47f4e19a807751c1aeef0/src/Money.UI.Blazor/App.razor)
- [Network.razor](https://github.com/maraf/Money/tree/c2e058ea19e47a3fd8c47f4e19a807751c1aeef0/src/Money.UI.Blazor/Components/Network.razor)
- [ApiHubConnectionChecker.razor](https://github.com/maraf/Money/tree/c2e058ea19e47a3fd8c47f4e19a807751c1aeef0/src/Money.UI.Blazor/Pages/ApiHubConnectionChecker.razor)
- [VersionChecker.razor](https://github.com/maraf/Money/tree/c2e058ea19e47a3fd8c47f4e19a807751c1aeef0/src/Money.UI.Blazor/Pages/VersionChecker.razor)