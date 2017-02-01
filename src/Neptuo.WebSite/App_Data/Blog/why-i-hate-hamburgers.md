> Brief introduction: I'm not a UI guy, really. I'm a typical backend guy, maybe a bit frontend (javascript) guy, but definitely not a design guy. So keep that in mind while reading this (a bit 'hate') post.

In last few years hamburger menus spread everywhere. It started on mobiles and in Windows 10 it is also in desktop applications. 

![Desktop app screen](/Content/Images/Blog/why-i-hate-hamburgers/youtube.png)

In last months I'm playing with Universal Windows Applications too. And also I'm a proud owner of Lumia 550. I think that these modern applications are quite pretty and I like them. But when it comes to mobile, I really hate the hamburger menu. I'm using my phone in right hand and it is simply unreachable for me. Maybe I have a small hand, but everytime I'm trying to tap the hamburger, my Lumia is slipping out of my hand.

I use my phone in two modes. One mode is `two-handed`, mostly when I'm at home, or sitting somewhere, but when I'm walking on a street, standing in a tram or bus, have a shopping bag in a hand, I need use my phone in single hand. I really need this mode. For me, it's about 50% of time when I use the phone.

When developing a [Money|https://github.com/maraf/Money] I came with a solution. Just for a mobile version. A desktop version, when the window is bigger, uses a typical (Windows 10 convensioned) hamburger menu in left-top corner. It uses a typical `SplitView` and when the window is big enough, it also shows a typical icon list, and when even bigger, it shows a whole menu with captions.

![Desktop app in middle window](/Content/Images/Blog/why-i-hate-hamburgers/middle.png)

I like this design! I really do.

![Desktop app in large window](/Content/Images/Blog/why-i-hate-hamburgers/large.png)

But when the application is running on a phone, I put the hamburger menu on bottom, and also, it opens like a flyout menu. 

![Mobile app screen with hamburger in bottom-left](/Content/Images/Blog/why-i-hate-hamburgers/mobile.png)

It is placed on the `BottomAppBar`, on the left side. Most of the applications don't need too much space for buttons, which are oriented right-to-left. So in most cases there is a space for a single button, the hamburger menu.

For me, it is a win-win. I can easily tap the button in single hand mode and for most cases it only fills an empty space.
