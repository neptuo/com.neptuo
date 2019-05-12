Sometimes I want to prevent from closing windows using `Alt+F4`. It's quite a rate case when I want the window to be closeable (so I can't bind to `Window.Closing` event and stop it), but I want to disable the `Alt+F4`. One of these scenarios is my clock showing window, a part of the [WinRun application](https://www.neptuo.com/project/desktop/winrun).

![WinRun clock showing window](/Content/Images/Blog/wpf-preventing-alt-f4/winrun-clock.png)

It's not a typical window. It basically acts like a live part of the desktop. It should almost be non-focusable. I'm using `Esc` to hide it, but sometimes, when it is the last opened window and I want to shutdown the computer using `Alt+F4`, invoked on the desktop, and instead of having focus on the dekstop I have focus on the clock window. It closes the window... Damn!


### The code
Here is a code snippet to actualy prevent `Alt+F4` on WPF window. The method is bound to a `Window.PreviewKeyDown` event.

```C#
private void OnPreviewKeyDown(object sender, KeyEventArgs e)
{
    if (e.Key == Key.System && e.SystemKey == Key.F4)
    {
        e.Handled = true;
    }
}
```

The "trick" is to test the pressed key for `Key.System` and then test `KeyEventArgs.SystemKey`.

### That's it. 
Hope it helps me some day in the future.