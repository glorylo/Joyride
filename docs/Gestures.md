# Gestures

### Overriding Default Behaviour

Joyride provides a stock of basic gestures such as tap, swipe, scroll.  Much of the Joyride's default behaviour can be overriden and this includes things such as entering text, setting checkbox, etc. -- beyond simply gestures.  However overridding default behaviour is especially important concerning screen transitions.  Tapping an element is the most common example.   Let's say you have an "Inbox" button on the *Dashboard* screen.  After you tap it, it directs you to *Inbox* screen.  Here's how you would you would override the *Tap* method on the *Dashboard* screen:
   ```csharp
   
      public override Screen Tap(string elementName, bool precise = false)
      {
         var screen = base.Tap(elementName, precise);
         switch (elementName)
         {
            case "Inbox":
               return ScreenFactory.CreateScreen<InboxScreen>();
            ...
            default:
               return screen;
         }
      }
   ```

Here's an example of overriding tapping for collection.  You have a *PhotoAlbum* screen and tapping on one of the photos takes you to the *ImageViewer* screen.  You would override the *TapInCollection* method in *PhotoAlbumScreen* like:
   ```csharp
        public override Screen TapInCollection(string collectionName, int oridinal = 1, bool last = false, bool precise = false)
        {
            var screen = base.TapInCollection(collectionName, oridinal, last, precise);
            if (collectionName == "Photos")
              return ScreenFactory.CreateScreen<ImageViewerScreen>();
            return screen;
        }
   ```

Here's an example of overriding swipe.  You have other screens and swiping to the right takes you to one screen, while swiping left takes you to another.  Override the Swipe method with:
   ```csharp
    public override Screen Swipe(Direction direction, double scale = 1.0, long durationMilliSecs = 500)
    {
        var screen = base.Swipe(direction, scale, durationMilliSecs);
        if (direction == Direction.Left)
          return ScreenFactory.CreateScreen<SwipedLeftScreen>();
        if (direction == Direction.Right)
          return ScreenFactory.CreateScreen<SwipedRightScreen>();
        return screen;
    }   
   ```
