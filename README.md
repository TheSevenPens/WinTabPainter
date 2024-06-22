WINTABPAINTER
==============


This is demo app to explore how drawing tablets work and prototype some ideas.

It's not intended to be a full-featured paint app. But there may be some
things you can learn from it, if you want to build something like that.

Key points
===========

- Built on .NET framework
- Uses WintabDotNet
- UI Framework. It uses WinForms which is not ideal. In the future, I'd liek to port it to WinUI 3
  
Issues 
=======

- Dealing with the scaling on Windows displays is hard coded into the code. If you
want it to paint correctly you'll need to make a code change

- It relies on the WinTab API

- WinForms and Windows screen scaling have some weird interactions with how things are rendered. 
