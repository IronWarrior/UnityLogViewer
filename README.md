View `Player.log` files either in a standalone app or a Unity Editor window. Download the standalone Windows app from [Releases](https://github.com/IronWarrior/UnityLogViewer). Note that it supports right-clicking a file and choosing the exe under _Open with_. To use it from within the Unity Editor, copy the `LogViewer` folder into your project, then access the window via `Window > Analysis > Log Viewer`.

It doesn't always split the log events perfectly, as Unity logs do not follow a universal standard. Tested with logs generated from a 2020.3 project.

Makes use of [UnityWindowsFileDrag-Drop by Bunny83](https://github.com/Bunny83/UnityWindowsFileDrag-Drop/tree/master) to allow drag and dropping files into the player window.

![Screenshot of log viewer window displaying example log messages.](https://i.imgur.com/dzsKztd.png)
