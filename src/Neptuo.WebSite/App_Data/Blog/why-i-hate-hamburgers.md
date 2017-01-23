# Why I hate hamburgers?

> Brief introduction: I'm not a UI guy, really. I'm a typical backend guy, maybe a bit frontend guy, but surely not a design guy. So keep that in mind while reading this (a bit 'hate') post.

In last years hamburger menus appeared everywhere. It started on mobiles and in Windows 10 it is also in desktop applications. 

[Desktop app screen]

In last months I'm playing with Universal Windows Applications too. And also I'm a proud owner of Lumia 550. I think that these modern applications are quite pretty and I like them. But when it comes to mobile, I really hate the hamburger menu. I'm using my phone in right hand and it is simply unreachable for me. Maybe I have a small hand, but everytime I'm trying to tap the hamburger, my Lumia is slipping out of my hand.

I use my phone in two modes. One mode is `two-handed`, mostly when I'm at home, or sitting somewhere, but I'm walking on a street, standing in a tram or bus, have a shopping bag in a hand, I need use my phone in single hand. I really need this mode. For me, it's a 50% of time a use the phone.

When developing a [Money|https://github.com/maraf/Money] I came with a solution. Just for a mobile version. A desktop version, when the window is bigger, it uses a typical (Windows 10 convension) hamburger menu in left-top corner. It uses a typical `SplitView` and then the window is big enough, it also shows a typical icon list, and when even bigger, it shows a whole menu with captions.

> I like this design!

[Desktop app screen in different sizes]

But when on a phone, I put the hamburger menu on bottom. And also, it opens like a flyout menu. 

[Mobile app screen with hamburger in bottom-left]

It is placed on the `BottomAppBar`, on the left side. Most of the applications don't need to much space for app bar buttons, which are placed from right to the left. So in most cases there is a place for a single button, the hamburger menu.

For me, it is a win-win. I can easily tap the button in single hand mode and for most cases it only fills an empty space.
