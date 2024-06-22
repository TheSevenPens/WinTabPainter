WINTABPAINTER
==============


This is demo app to explore how drawing tablets work and prototype some ideas.

It's not intended to be a full-featured paint app. But there may be some
things you can learn from it, if you want to build something like that.

Notes
===========

- Built on .NET framework 4.7.2
- Uses WinTabDotNet
- UI Framework. It uses WinForms which is not ideal. In the future, I'd liek to port it to WinUI 3
- I'd like to move it to .NET 7.0 pr .NET 8.0 but WinTabDotNet seems to have problems running with those
  
Issues 
=======

- **UI Scaling** - Dealing with the scaling on Windows displays is hard coded into the code. If you
want it to paint correctly you'll need to make a code change

- **Pen API** - It relies on the WinTab API

- **DPI** - WinForms and Windows screen scaling have some weird interactions with how things are rendered.
In particular you'll notice that the bitmap is not shown with a 1:1 zoom. It's scaled up a little
and the pixels are fuzzy

