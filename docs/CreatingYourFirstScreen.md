# Creating Your First Screen

1. Continuing after the "Getting Started" Guide, we'll create our first screen.  
   Create an abstract base class to AndroidScreen.  This base class will be used for anything globally for all your application's screens.  
   ```csharp
      public abstract class MyCoolAndroidScreen : AndroidScreen 
      { 
       
      }
   ```
2. Create your first screen that appears when your app launches.  Each screen needs a *Name* and an implementation of the *IsOnScreen* method.  
   ```csharp
      public class FirstScreen : MyCoolAndroidScreen 
      {
         public override string Name
         {
            get { return "First Screen" }
         }

         public override bool IsOnScreen(int timeOutSecs)  
         {
            throw new NotImplementedException();
         }

         public override Screen GoBack()
         {
            throw new NotImplementedException();
         }
      }
   ```
   Ideally, it would be great to have your in house developers create identifiers for all your screens.  This makes things significantly easier when trying to detect which screen you are on.
    
   If you are using Android, supply a *GoBack()* method.  This method handles the transition when the Android button redirects the user to another screen.  

   The MyCoolAndroidScreen class is a good place to provide default implementation, especially if the implementation is mostly the same across all your screens.
3. Create a Null Screen, for situations when the screen shown is unknown.   
   ```csharp
     public class NullMyCoolAndroidScreen : MyCoolAndroidScreen
     {
        public override string Name
        {
            get { return "Null"; }
        }

        public override bool IsOnScreen(int timeOutSecs)
        {
            return false;
        }

        public override Screen GoBack()
        {
            Driver.Navigate().Back();            
            return this;
        }
     } 
   ```
4. Back to your MyCoolApp, we will instantiate the starting screen.  Conversely, when the app is closed, we will set the screen to the Null screen.  We instantiate screens using the ScreenFactory.
   ```csharp 
     public class MyCoolApp : AndroidMobileApplication
     {
        public override string Identifier { get { return "com.my.cool.app"; }}

        public override void Launch()
        {
            base.Launch();
            CurrentScreen = ScreenFactory.CreateScreen<FirstScreen>();
        }

        public override void Close()
        {
            base.Close();
            CurrentScreen = ScreenFactory.CreateScreen<NullMyCoolAndroidScreen>();
        }
     }
   ```
   *CurrentScreen* is a property that holds the screen as it transitions when the user work through your app.  
5. You now write a scenario to check if the launch sets the appropriate current screen.
   ```gherkin
    Scenario: Launching the app should be on first screen
    Given I launch the "My Cool App" mobile application
    Then I should be on the "First Screen" screen
   ```
   When the *Then* step is executed, it makes an invocation to *IsOnScreen* and asserts that the screen is being displayed.

6. If all is well, running your test should pass!  
