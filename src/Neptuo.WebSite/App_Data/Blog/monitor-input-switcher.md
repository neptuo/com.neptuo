Recently my home setup has changed. I have my own PC with two monitors. After entering a new job, I got a new PC, for work. Then I had to solve a problem.

**How to use my two monitors, single keyboard and mouse with two PCs?**

## USB switcher

For switching USB devices, I bought a [Ugreen USB 3.0 2 in 4 out Sharing KVM Switch Selector](https://www.amazon.com/UGREEN-Selector-Computers-Peripheral-Switcher/dp/B01N6GD9JO).
It has 4 ports and support switching between two PCs. Exactly what I neeeded.

> There is a cheaper version supporting USB 2.0. I'm quite sure it will do the job for me...

So keyboard, mouse, webcam and bluetooth dongle are solved.

## Monitor Switcher

My two monitors (Dell UltraSharp U2515H) have a lot of input ports. So I plugged HDMI cables to my personal PC and DP cables to my work PC.

> I love these monitors for coding (higher resolution, but still good readability with 100% scale). The only bad part about them are those touch buttons. I don't understand why there aren't normal buttons that you can find without light.

Thanks to the touch buttons, switching inputs manually became a nightmare. I started to look for a software that could do that for me. Just by clicking an icon...

### Dell Monitor Manager

Dell provides [an application for managing their monitors](https://www.dell.com/support/kbdoc/en-us/000060112/what-is-dell-display-manager), but the UI doesn't work in a way I wanted to (single click to switch between PCs). Anyway, it accepts command line arguments.

```
.\ddm.exe /1:SetActiveInput hdmi1
```

The downside for me was that the application must be running. It must be installed on the PC. And to support my scenario, I would still need to code another application, that will communicate with the Dell's one.

### GitHub repositories

There is quite some number of custom application for switching monitor input. [Some examples here...](https://github.com/search?q=monitor+input+switch)
None of them matches my expectations.

## My own Monitor Input Switcher

So, I decided to write my own. So it works exactly like I want to. Welcome Monitor Input Switcher.

![Monitor Input Switcher](/Content/Images/Blog/monitor-input-switcher/contextmenu.png)

My Monitor Input Switcher is running in the tray. The idea is that you provide it with two configurations each associated with a machine name.

```json
{
  "M": {
      "0": 15, // Monitor 0 to input 15 (Dell's HDMI 1)
      "1": 17  // Monitor 1 to input 17 (Dell's Display Port)
  },
  "W": {
      "0": 16, // Monitor 0 to input 16 (Dell's mini Display Port)
      "1": 16  // Monitor 1 to input 16 (Dell's mini Display Port)
  }
}
```

> I didn't find a description of what input number maps to what connector on the monitor. So I just interated in a loop to find these.

My computer names are "M" (my personal) and "W" (work). This configuration file is than shared between PCs. Based on the current machine name, the application determines which configuration is for the "current/this PC" and which is for the "other PC".

With configuration in place, you can
- `Left click` or `Win+Alt+O` to switch all monitors to the "Other PC".
- `Middle click` or `Win+Alt+H` to switch all monitors to the "This PC".
- `Right click` to open context menu.
- Or switch per-monitor.

![Help window](/Content/Images/Blog/monitor-input-switcher/help.png)

## Summary

Now I can focus on work.

### Links
- [Documentation](https://www.neptuo.com/project/desktop/monitorinputswitcher)
- [GitHub repository](https://github.com/maraf/MonitorInputSwitcher).
- [Download](https://apps.neptuo.com/monitor-input-switcher).