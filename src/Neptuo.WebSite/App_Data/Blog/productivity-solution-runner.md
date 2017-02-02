# Productivity - Solution Runner

This a brief presentation of [Solution Runner](http://www.neptuo.com/project/desktop/solutionrunner) - a first product of our Productivity pack.

## Are working with many Visual Studio solutions? 

## Are you frequently switching between solutions?

Then Solution Runner is a live saving application. What it is capable of?

[Preview of main window]

The application scans your file system for `*.sln` files and provides super fast way to open any. In the current `v1.6.2` version there are many features and configurations to match your needs.

### Let's start

The only thing needed at the beginning is a path to folder where to search for `*.sln` files. After confirming this path, the application scanns current file system and setups file watcher for any changes made during runtime.

### Foreground vs. Background

If you dont's specify a windows-wide hotkey, the application is running like any standart windows application. It has an item on the taskbar, is visible in the tab-window-list. You can close it pressing `Esc` or `Alt+F4`. After that it shuts down.

If a global hotkey is set, the application switches to a `background` mode. It is no longer visible on taskbar, nor in tab-window-list. When you the hokey, a main window shows up, after starting a Visual Studio, or pressing `Esc`, the window vanishes to the background, like the application is not running.

Searching works in two modes which can be switched in the configuration window. A first and the default mode searches matching a solution file name from the start. In this mode, we can't search in the directory name and must typ a solution file name from the start.

A second one enables searching in the parts of the whole path. In this mode a space has a meaning of wildcard. So let's say we have a

- `D:\Development\Framework\Neptuo.sln`

Than you find such a file using an example phrase `Fram Nept`. It works in the way a Visual Studio file opener works.

### Additional applications

From version `v1.4` there is support for manually specifying additional applications, which are shown beside installed Visual Studio versions. These applications can be run in a context of a selected solution file, and you can configure whether file name or containing directory name is required for the additional application.

### Statisticts

In version `v1.6` we have added totally opt-in support for storing statistics of used applications and solution files. These statistics can be turned on in the configuration window. If enabled, we can view current state under the `Statistics` button.

The view in the initial phase and we are working on improving it. But the base overview it's working.

### Small UI tweaks

These are the main functions. Beside them there are some configuration tweaks that can be turned on/off based on user preferences. These contains main window position settings, searching result list size, displayed file path settings, preselecting Visual Studio version and others.

## Summary

Try it. 

You will love it. Or don't. 

We can't imagine a day without it.

[Download a click-once application](http://www.neptuo.com/project/desktop/solutionrunner).
