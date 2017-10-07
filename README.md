# Chromely
Chromely is a .NET/.NET Core HTML5 Chromium desktop framework. It is focused on building apps using embedded Chromium without WinForm or WPF. Uses Windows and Linux native GUI API. It can be extended to use WinForm or WPF. Main form of communication with Chromium rendering process is via Ajax HTTP/XHR requests using custom schemes and domains (CefGlue, CefSharp) and .NET/Javascript integration (CefSharp).

### Platforms
Cross-platform - Windows, Linux. Built on CefGlue, CefSharp, NET Standard 2.0, .NET Core 2.0, .NET Framework 4.61 and above.

#### Chromely Demo 
For more on demos - [Demos](https://github.com/mattkol/Chromely/wiki/Demos)
![](https://github.com/mattkol/Chromely/blob/master/Screenshots/Cefsharp/chromely_cefsharp_index_info.png)


### VS Projects Description 
| Project | Framework| Comment |
| :---         |     :---      | :--- |
| Chromely.Unofficial.CefGlue.NetStd   | .NET Standard    | *** Unofficial port of Xilium.CefGlue.    |
| Chromely.Core    | .NET Standard       |   The core library required to build either a Chromely CefSharp or Chromely CefGlue apps.    |
| Chromely.CefGlue.Winapi    | .NET Standard        | Chromely CefGlue implementation library - this is in .NET Standard as it can be used in both .NET (Win) and .NET Core (Win, Linux)     |
| Chromely.CefSharp.Winapi     | .NET       | Chromely CefSharp implementation is only for .NET     |
