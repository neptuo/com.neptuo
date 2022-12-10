GitHub pages is a solid solution for hosting static sites. I use it a lot and as a matter of fact, my very first blog post was about running this site on GitHub pages. I host all of my Blazor wasm apps on GitHub pages. Until recently I was always publishing them manually. You know, build my code, copy content of publish folder to a folder with checkout of the GitHub pages repo, and finally commit.

But! It is very easy to use GitHub actions to do this tedious process on my behalf. Here is my approach.

I always have a separate repo for GitHub pages. For example I have [maraf/Money](https://github.com/maraf/Money) repo with sources and then I have (two) repos for static sites [maraf/com.neptuo.money.app](https://github.com/maraf/com.neptuo.money.app) and [maraf/com.neptuo.money.app.beta](https://github.com/maraf/com.neptuo.money.app.beta).

In each of these static site repos I have created a GitHub actions workflow file [deploy.yaml](https://github.com/maraf/com.neptuo.money.app/blob/master/.github/workflows/deploy.yml). As mentioned in the title, I use semi-automatic approach, as I want to have a control over what gets published where. I configured the workflow to be triggered manually using `workflow_dispatch`.

## Step 0: Input arguments
I want to have the ability to select which commit gets published, so I configured the `workflow_dispatch` to accept one optional argument

```
inputs:
    commitHash:
    description: 'Commit hash'
    type: string
```

If the argument is not provided, it takes the head of the repo. 

## Step 1: Checkout

I use `actions/checkout@v3` and as I need to checkout a different repo than the current one, it needs a bit configuration

```
- name: Checkout
  uses: actions/checkout@v3 
  with:
    repository: 'maraf/Money'
    ref: '${{ inputs.commitHash }}'
```

## Step 2: Setup .NET

This one is straightforward

```
- name: Setup .NET
  uses: actions/setup-dotnet@v3
  with:
    dotnet-version: '7.0.x'
```

## Step 3: Install wasm-tools (optional)

I want to publish an AOT compiled version, so I need workload `wasm-tools`

```
- name: Install wasm-tools
  run: dotnet workload install wasm-tools
```

## Step 4: Build the code

Again, pretty straightforward. Compile my Blazor project in Release configuration, with AOT and put it in the `./artifacts` folder. The `-p:PublishDomain` is my way of generating `CNAME` file for GitHub pages. You can check it out in the [Money.Blazor.Host.csproj](https://github.com/maraf/Money/blob/master/src/Money.Blazor.Host/Money.Blazor.Host.csproj#L49-L74).

```
- name: Build
  run: dotnet publish ./src/Money.Blazor.Host/Money.Blazor.Host.csproj -c Release -p:PublishDomain=app.money.neptuo.com -p:RunAOTCompilation=true -o ./artifacts
```

## Step 4: Deploy to GitHub pages

Here I'm using `JamesIves/github-pages-deploy-action@v4` action to do it. There are quite some alternatives, but this one worked for me. It allows me to select which folder deploy to which branch, without need for explicit authentication token (since I'm publishing to the current repo).

```
- name: Deploy to GitHub Pages
  if: success()
  uses: JamesIves/github-pages-deploy-action@v4
  with:
    folder: ./artifacts/wwwroot
    branch: master
```

## Summary

And that's it. It's 30 lines of code that simplifies my life when publishing new version for my Blazor apps. No idea why it took me so long.