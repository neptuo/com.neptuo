# Solution Runner v2.0 aka Move to Microsoft Store

I'm super happy to announce that Solution Runner has been moved from clickonce deployment to Microsoft Store.
You can find it as [Solution Runner by Neptuo](https://www.microsoft.com/store/productId/9MVTL05WZB3P).

The [main goals for v2](https://github.com/neptuo/Productivity.SolutionRunner/milestone/16?closed=1) were migration to MS store and run on .NET 5, you won't find any other changes in this release.

## Migration

Configuration remains without any changes. Anyway, MS store applications use different locations, so may need to do these two easy steps.

### Configuration

If you use non-standard location of configuration file (anything other than `%USER_PROFILE%\SolutionRunner.json`), you will need to set it after the first start.

![Configuration file path](...)

### Statistics

If you use statistics, you can import them to the v2. First, in the previous version, open "Internal state folder".

![Open internal state folder](...)

Copy the directory path and then in the v2, import the "Statistics.dat" file.

![Import statistics](...)

## Recapitulation of features

Solution Runner automatically scans for *.sln files and installed Visual Studio versions, but it can help you in other ways.

### Additional applications

![Additional applications](...)

You can configure any number of additional applications that can be opened empty or in a context of a selected solution file. 
Above you can see popular [Git Extensions](https://gitextensions.github.io) to quickly open a git repository where your solution file is contained.
Also, any application can have additional commands that are accessible through `Alt` hotkey combination. As an example, you can directly open Commit dialog of Git Extensions from the Solution Runner.

![Additional commands](...)

### Auto-select VS version based on solution file

If you have older solution file, that you always want to open in an older version of VS, you can configure solution runner to scan solution file for version and automatically
preselect a suitable VS version.

### Run Visual Studio in different modes

With additional commands for Visual Studio versions, you can run it as Administrator, run an experimental instance or start the updater.

![Visual Studio additional commands](...)

### Pinned solution files

With one keyboard shortcut `Ctrl+S`, you quickly manage frequently used solution files to appear on top of your list, even without entering any search phrase.

### Copy paths

With `Ctrl+C` you can copy a path to the solution file to clipboard.
With `Ctrl+Shift+C` you can copy a path of the selected application.

![Supported shortcuts](...)

## Future plans

You can see [planned feature in our issue list](https://github.com/neptuo/Productivity.SolutionRunner/issues).
If you have any ideas that are missing, please, feel free to submit them on GitHub.