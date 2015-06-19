# Screen Detection

1. Screen detection uses the same mechanism as modal dialog detection.  Although there are no steps that makes use of it, there may be cases where you want to use screen detection internally such as the Android's back button or where there are various possible branching transitions. 
2. In your base screen add screen detection with the following
   ```csharp
      public abstract class MyCoolAndroidScreen : AndroidScreen 
      {  
          protected static IDetector<Screen> ScreenDetector;
          
          static MyCoolAndroidScreen()
          {
            ...  
             var unknownScreen = ScreenFactory.CreateScreen<NullMyCoolAndroidScreen>();
             ScreenDetector = new AndroidScreenDetector(assembly, typeof(MyCoolAndroidScreen), unknownScreen);
          }         
  }
   ```
   The *NullMyCoolAndroidScreen* will be set if the detector is unable to find any defined screens.
   
3. You can then create a static method like so
   ```csharp
      protected static Screen Detect(params Type[] screenTypes)
      {
         return (!screenTypes.Any()) ? ScreenDetector.Detect() : ScreenDetector.Detect(screenTypes);
      }
   ```

4. You can create a default "back" button behavior for MyCoolAndroidScreen.  When using the go back step in
   ```gherkin
   When I go back
   ```
   
   this will be invoked to detect any screen as the default.
   ```csharp
      public override Screen GoBack()
      {
          Driver.Navigate().Back();
          return Detect();
      }
   ```
   Since it will attempt to detect all screens, you can make use of the *Detect* attribute exactly like the modal dialogs.
   ```csharp
      [Detect(Priority = 30)]
      public class InboxScreen : MyCoolAndroidScreen
   ```
5. If a screen transitions from a selected set of screens you can restrict detection similarly to dialogs. Simply provide a list of screens you wish to detect **in that order**.  

   ```csharp
      public override Screen GoBack()
      {
          Driver.Navigate().Back();
          // detects only 3 possible screens when going back
          return Detect(typeof(DashboardScreen), typeof(InboxScreen), typeof(OutboxScreen));
      }   
   ```
