WinTab Painter
==============

This is demo app to explore how drawing tablets work and prototype some ideas.

It's not intended to be a full-featured paint app. But there may be some
things you can learn from it, if you want to build something like that.

Notes
===========

- Built on .NET 8
- Uses modified code from WinTabDotNet to work with .NET 8
- App uses WinForms. In the future, I'd like to port it to WinUI3 or WPF.
- Uses screen coordinates not the higher resolution coordinates from tablet
  
User Features
===========
- Basic dab-based brush rendering
- Position smoothing
- Pressure curve
- Pressure smoothing
- Pressure quantization - simulate having fewer levels of pressure
- Use pressure to control width of brush
- Tilt detection using both altitude & azimuth angles and xtilt & ytilt angles
- Detection of button state
- Open PNG
- Save image as PNG
- Copy image to clipboard
- Record/Replay drawing packets
- Save/Load Drawing packets
- Shortcut keys to clear canvas, increase/decrease brush size

Library
===========
- TabletSession class encapsulates the interaction with WinTab. So easy to get pen pressure in a .NET app
- TabletSession has simple way to register for pen events, and button events
- Geometry structs to deal with integer based Points and Sizes. Comes in both integer and double versions. With implicit conversions to System.Drawing objects

Future features
===========
- Option to disable pressure sensitivity
- Add chatter to input pen events
- Use pressure to control opacity of brush

Issues 
===========
- **Pen API** - As the name implies it relies on the WinTab API it does not use Windows Ink.
- **Gaps in Stroke rendering** - Dabs are drawn but can leave gaps when pen is moving fast

