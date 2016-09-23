I always wanted to write a blog, so let's start. Today's topic are the tools behind this website.

## Static sites

After years of using CMS for serving content that was changed once a week, or even less, I found the magic of static sites and came to the opinion that THIS IS GREAT. As nothing, it is not a silver bullet, but it fits for the most of wesites. And when it comes to web applications, some parts always can be pregenerated and served as static HTML. 

What I don't like the most, are the stylesheets, javascripts and images served by the application servers. It really is a mess for the application server to serve this content. When it comes to my favourite application framewrok, ASP.NET (just a side note I don't like at all, but I live C# and .NET, but talk about it in some of the next posts), starting managend code, running all the managed modules (in case of enabled routing) for serving something placed in the filesystem? Really? No, please don't do it. There are some many solutions areound the web, not like 5 years back. Use a someof the hosted CDN or your own Apache or NGINX, they are great at serving static content. Next great thing about such a solution is that you spread load between more servers and more (sub) domains. So this is the motion... and yes, this web site is created using ASP.NET MVC v4, I gues.

## HTML generators

Using tools like MVC is great even for static sites like this one. You can build in support for master pages, I'm sorry, layout pages. You can use localization with resources. You can even use database, like SQL, for reading content. I will never write a content in to the raw HTML. I will never loose so much of information
and structure just by putting it to the raw HTML.

The point is to use all these features, but instead of deploying as an application to some server, using kind of generator, crawler or call it just you like, to generate static HTML wherever it is posible. Our projects section changes, to be honest, maybe once a month. Our references & services section changes twice in a half of a year. Our homepage changes once a year. And the layout is going to be immortal.

I'm sure that page reads over comes these 'number' many times. So it is ideal candidate to be pregenerated. So you head this direction, you should some tools.

### 1) Crawler or generator

There are two options for generating staic HTML. The first and surely the easier, it to run your web application on the local development server and use some tool to walk through all the links and save HTML ale all used assets to some folder. This is the way we are currently using.

You can find a lot of tools on the internet, event open sourced, that can do this work. I'm sure you can even head to the bash and write a script which uses `curl`. As we are not that cool, so we didn't use bash. And because it is quite fun, we wrote our simple crawler and [source codes on github](http://github.com/maraf/StaticSiteCrawler). It is not awsome, it is even not that great, but it does the work we need. It takes a starter URL, typically link to the locally hosted homepage and walks through all the links it finds. Everytime a link is found in the output HTML, it checkes whether the URL is not already donwloaded and if not the placed to the queue. As we use extension less URLs, for every such a URL it creates a folder and places a index.html file in it. When there pages that not linked from other parts of the web, these must manually passed to the crawler, like the homepage URL.

As I've mentioned, there can another way to generate static HTML, but it requires support from the used CMS. The workflow could like: Go to the artifact edit form (in fiction CMS), update some content. Now, when the artifact is changed, the CMS can run a task to pregenerate the output. I will enrich this tought in the end of this post, in the [My desired solution](#my-desired-solution) section, as it modifies the whole pipeline.

Right now, we have pregenerated our static HTML in a local folder.

### 2) Verification (optional)

This step is optional, but I encourage to do it, at least in the beginnings. For this we use a simple cmd script that setups IIS Express site in the folder with pregenerated content. Using manual testing we verify the changed content. It's a bit simple step.

### 3) Deploy

I've ended last paragraph a phrase 'simple step'. This is event simplier and depends on your hosting provider. We use github.

## The GitHub 

I haven't tolds you how I like GitHub yet. GitHub is great, the last sentence reminds a quote I have read somewhere

> We all, all over the World, use git, the distributed source code version control, to push all over code to the single location, the GitHub.

Which reminds that it is always good to have some backup, for all of your code. You don't need to push all code to the github, but do use some, at least two remotes, where the code backed. For github and surely other public repository providers, you can use simple hook, that is notified about on every push automatocally pulls the changes you, or anyone other, have made. I'm using my Raspberry Pi2 for this task and it works really great.

So, the GitHub, I don't want to talk about features lime markdown, or wiki, or issues, or the newly added projects, but I do want to talk about hosting static pages with github.

> A side note: If you like Jekyll, you can setup a static site generator that does the work described here in a GitHub repo. But we, as we don't like that much bash, we even don't like that much Jekyll. So we don't use it and do the work manually. It has pros and cons, but we like the idea that we are not to bound to a technology we do not like. End of side note.

Hosting a static web site on GitHub is very simple. Create a new repository and in the Settings, in the Static site section, select a branch to use as a website. If you a custom domain, create a CNAME in the of your branch a place the of your domain in it, then change your DNS settings to point to the github.com. That's all. 

You can even use single repository to contain your source codes, probably in the master branch, and static pregenerate site in the gh-pages branch.

## My desired solution

TODO: The ideal pipeline (inspired by CQRS) with rpi for processing inputs/commands, hosted on 'hidden' domain, pregenerating static sites and pushing them to the github/static site hosting.