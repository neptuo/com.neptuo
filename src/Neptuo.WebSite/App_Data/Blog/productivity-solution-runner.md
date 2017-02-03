This is a brief presentation of [Solution Runner](http://www.neptuo.com/project/desktop/solutionrunner) - a first product of our Productivity pack.

 - Are working with many Visual Studio solutions? 
 - Are you frequently switching between solutions?
 - Do you frequently need to run applications in context of solution folder?

Then Solution Runner is a live saving application. What it is capable of?

![Preview of main window](/Content/Images/Blog/productivity-solution-runner/main-window.png)

The application scans your file system for `*.sln` files and provides super fast way to open them. In the current `v1.6.2` version there are many features and configurations to match your needs.

### Let's start

The only thing needed at the beginning is a path to folder where to search for `*.sln` files. After confirming this path, the application scans current file system state and setups file watcher for any changes made during runtime.

Searching works in two modes which can be switched in the configuration window. A first and the default mode searches matching a solution file name from the start. In this mode, we can't search in the directory name and you must type a solution file name from the start.

A second one enables searching in the parts of the whole path. In this mode a space has a meaning of wildcard. So let's say we have a

- `D:\Development\Framework\Neptuo.sln`

Than you can find this file using an example phrase `Fram Nept` or `Dev Fr tuo`. It works in the way a Visual Studio file opener works.

### Foreground vs. Background

If you dont's specify a windows-wide hotkey, the application is running like any standart windows application. It has an item on the taskbar, it is visible in the alt-tab list. You can close it pressing `Esc` or `Alt+F4`. After that it shuts down.

If a global hotkey is set, the application switches to a `background` mode. It is no longer visible on hte taskbar, nor in alt-tab list. When you press the hotkey, a main window shows up, after starting a Visual Studio, or pressing `Esc`, the window vanishes to the background, like the application is not running at all.

### Additional applications

![Preview of additional applications](/Content/Images/Blog/productivity-solution-runner/additional-applications.png)

From version `v1.4` there is a support for manually specifying additional applications, which are shown beside installed Visual Studio versions. These applications can be run in a context of a selected solution file, and you can configure whether file name or containing directory name is passed to the additional application.

### Statisticts

In version `v1.6` we have added totally opt-in support for storing statistics of used applications and solution files. These statistics can be turned on in the configuration window. If enabled, you can view current state under the `Statistics` button.

The view of statistics is in the initial phase and we are working on improving it. But for the base overview it's working.

### Small UI tweaks

These are the main functions. Beside them, there are some configuration tweaks that can be turned on/off based on user preferences. These contains main window position settings, searching result list size, displayed file path settings, preselecting Visual Studio version, tray-icon and others.

## Summary

Try it. 

You will love it. Or don't. 

We can't imagine a day without it.

[Download a click-once installer](http://www.neptuo.com/project/desktop/solutionrunner).

[Submit a feature request](https://github.com/neptuo/Productivity.SolutionRunner/issues).
