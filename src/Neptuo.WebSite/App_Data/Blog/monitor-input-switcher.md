Recently my home setup has changed. I have my own PC with two monitors. After entering a new job, I got a new PC, for work. Then I had to solve a problem.

How to use my two monitors, single keyboard and mouse with two PCs?

## USB switcher

Fro switching USB devices, I bought a [Ugreen USB 3.0 2 in 4 out Sharing KVM Switch Selector](https://www.amazon.com/UGREEN-Selector-Computers-Peripheral-Switcher/dp/B01N6GD9JO).
It has 4 ports and support switching between two PCs. Exactly what I neeeded.

> These a cheaper version supporting USB 2.0. I'm quite sure it will do the job for me...

So keyboard, mouse, webcam and bluetooth dongle solved.

## Monitor Switcher

My two monitors (Dell UltraSharp U2515H) has a lot of input ports. So I plugged HDMI cables to my personal PC and DP cables to my work PC.

> I love these monitors for coding (higher resolution, but still good readability with 100% scale). The only bad part about them are those touch buttons. I don't understand why there aren't normal buttons that you can find without light.

Thanks to touch buttons, manually inputs became a nightmare. I started to look for a software that could do that for me. Just by clicking an icon.

### Dell Monitor Manager

Dell provides [an application for managing their monitors](https://www.dell.com/support/kbdoc/en-us/000060112/what-is-dell-display-manager). The UI doesn't work in a way I wanted to - Single click to switch between PCs. Anyway, it accepts command line arguments.

```
.\ddm.exe /1:SetActiveInput hdmi1
```

The downside for me was that the application must be running. It must be installed on the PC. And to support my scenario, we would still need to code another application, that will communitace with the Dell's one.

### GitHub repositories

There is quite some number of custom application for switching monitor input. [Some examples here...](https://github.com/search?q=monitor+input+switch)

## Monitor Input Switcher

Any way, I decided to write my own. So it works exactly like I want to. Welcome Monitor Input Switcher.

![Monitor Input Switcher](/Content/Images/Blog/monitor-input-switcher/screenshot.png)

My Monitor Input Switcher is a tiny application running in the tray. The idea is that you provide it two configurations associated each associated to a machine name.

```json
{
  "W": {
      "0": 16,
      "1": 16
  },
  "M": {
      "0": 15,
      "1": 17
  }
}
```

My computer names are "M" (my personal) and "W" (work). This configuration is that shared between PCs. Based on the current machine name, the application determines which configuration is for the "current/this PC" and switch is for the "other PC".

With configuration in place, you can
- Left click or `Win+Alt+O` to switch all monitors to the "Other PC".
- Middle click or `Win+Alt+H` to switch all monitors to the "This PC".
- Right click to open context menu.
- Switch per-monitor.

## Summary

Now I can focus on work.

### Links
- [Documentation](https://www.neptuo.com/project/desktop/monitorinputswitcher)
- [GitHub repository](https://github.com/maraf/MonitorInputSwitcher).
- [Download](https://apps.neptuo.com/monitor-input-switcher).