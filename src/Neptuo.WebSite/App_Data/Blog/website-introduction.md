I always wanted to write a blog, so let's start. Today's topic are the tools behind this website.

## Static sites

After years of using CMS for serving content that was changed once a week, or even less, I found the magic of static sites and came to the opinion that THIS IS GREAT. As nothing, it is not a silver bullet, but it fits for the most of wesites. And when it comes to web applications, some parts always can be pregenerated and served as static HTML. 

What I don't like the most, are the stylesheets, javascripts and images served by the application servers. It really is a mess for an application server to serve this content. When it comes to my favourite application framework, ASP.NET

> just a side note I don't like it at all, but I do like C# and .NET. It's a long story and let's talk about it in some of the next posts

 starting managend code, running all managed modules (in case of enabled routing) for serving something placed in the filesystem? Really? No, please don't do it. There are many solutions around the web, not just like 5 years back. We can use some of the hosted CDN or own Apache or NGINX. They are great at serving static content. Next great thing about such a solution is that you spread load between more servers and more (sub) domains. 

So this is the motivation behind this article.

## HTML generators

Tools and frameworks like MVC are great even for static sites, like this one (Yes, we are using ASP.NET MVC 4). You can use build-in support for master pages, sorry, layout pages. You can use localization with resources. You can even use database, like SQL, for reading structured content. I will never write a content into the raw HTML. I will never loose so much of information and structure just by putting it to the raw HTML.

The point is to use all these features, but instead of deploying as an application to some server, using kind of generator, crawler or call it just you like, to generate static HTML wherever it is posible. Our projects section changes, to be honest, maybe once a month. Our references & services section changes twice in a half of a year. Our homepage changes once a year. And the layout is going to be immortal.

I'm sure that page reads over comes these 'numbers' many times. So it is ideal candidate to be pregenerated. But if we are heading this direction, we need some tools and workflow.

### 1) Crawler or generator

There are two options for generating staic HTML. The first one and surely the easier is to run your web application on the local development server and use some tool to walk through all the links and save HTML and all used assets to some folder. This is the way we are currently using.

You can find a lot of tools on the internet, event open sourced, that can do this work. I'm sure you can even grab the bash file and write a script which uses `curl`. But we are not that cool, so we didn't use bash. And because it is quite fun, we wrote our simple crawler and [placed source codes on github](http://github.com/maraf/StaticSiteCrawler). It is not an awsome tool, it is even not that great, but it does the work for us. It takes a starter URL, typically link to the locally hosted homepage and walks through all the links it finds. Everytime a link is found in the output HTML, it checks whether the URL is not already donwloaded and if not the URL placed to the queue. As we use extension less URLs, for every such an URL, a folder is created and an index.html file is placed in it. For pages that are not linked from other parts of the web, we need pass these along with the homepage link to the crawler.

As I've mentioned, there can another way to generate static HTML, but it requires support from the CMS. The workflow could be like: Go to the artifact edit form (in fiction CMS), update some content. Now, when the artifact is changed, the CMS can run a task to pregenerate the output. I will elaborate a bit on this tought in the end of this post, in the [My desired solution](#my-desired-solution) section, as it modifies the whole pipeline.

Right now, we have pregenerated our static HTML in a local folder.

### 2) Verification (optional)

This step is optional, but I encourage you to do it, at least in the beginnings. We use a simple command-line script to start local IIS Express site in the folder with pregenerated content. Using manual testing we verify the changed content. It's a bit simple step.

### 3) Deploy

If I've ended last paragraph with a phrase 'simple step'. This is event simplier and depends on your hosting provider. We use github.

## The GitHub 

I haven't told you yet how I do like GitHub. GitHub is great, the last sentence reminds me a quote I have heard somewhere

> We all, all over the World, use git, the distributed source code version control, to push all our code to a single location, the GitHub.

Which reminds that it is always good to have some backup, for all of your code. You don't need to push all code to the github, but you should have at least two remotes, where the code is backed. For github and surely other public repository providers, you can use simple web hook that is notified on every push and automatically pulls the changes you have made. I'm using my Raspberry Pi2 for this task and it works really great.

So, the GitHub, I don't want to talk about features like markdown, wiki, issues or the newly added projects, but I do want to talk about hosting static pages with github.

> If you like [Jekyll](https://github.com/jekyll/jekyll), you can setup a [static site generator](https://help.github.com/articles/using-jekyll-as-a-static-site-generator-with-github-pages/) that does the work described here in a single GitHub repo. But we, as we don't like that much bash, we even don't like that much Jekyll. So we don't use it and do the work manually. It has pros and cons, but we like the idea that we are not bound to a technology we do not like.

Hosting a static web site on GitHub is very simple. Create a new repository and in the `Settings`, in the `GitHub Pages` section, select a branch to use as a website. If you have a custom domain, create a CNAME in the root of your branch a place the of your domain in it, or fill it in the `Custom domain` section. Then change your DNS settings to point to the github.com. That's all.

You can even use single repository to contain your source codes for the web site, probably in the master branch, and pregenerated site in the `gh-pages` branch. Our website can be used as an example.

## My desired solution

As mentioned earlier, the current pipeline is not ideal in our eyes. We would like to make the deplyment process automatic. We would like to have option to process some inputs from our visitors. 

Our current vision is to split reading (displaying content) and writing (processing input). This design is inspired by the [CQRS](http://martinfowler.com/bliki/CQRS.html) architecture, which scales differently write side and read side.

Our ideal CMS would generate as much of the content as possible directly after making a change. It will push changed HTML or assets to a production/static site server. Such CMS will be hidden on a 'not-so-public' domain and will processs only write side requests and than redirects user back to the static site. In some cases, the user will see the newly pregenerated page immediately after redirect, in other cases, he will be noticed that the request is processing and to try to reload the page in a moment.

We don't have a CMS like this in a moment and we didn't even found some, capable of such worflow...

## Summary

Today, you can very simply have a web site, which is always on, which runs on your domain and which costs you nothing. It is simply awsome!