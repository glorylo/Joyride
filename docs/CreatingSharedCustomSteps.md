# Creating Shared Custom Steps

1. If you would like to share custom steps across different screens or even different apps, simply have their screens implement a shared interface.  You should have organize your project such that the MobileApplication classes are in a separate project outside your "Specs" project.  
2.  A typical example is the login screen you have on both ios and android.  Have an shared interface 

   ```csharp
      public interface ILogin
      {
         Screen LoginAs(string name, string password);
      }
   ```
3. Your screens should implement the ILogin interface.
4. Create a yet another separate project.  This project will hold all shared steps.  Create a new class with a binding to that interface method.

   ```csharp
      [Binding]
      public class LoginSteps
      {  
         [Given(@"I login as user ""([^""]*)"" with password ""([^""]*)""")]
         [When(@"I login as user ""([^""]*)"" with password ""([^""]*)""")]
         public void GivenILogin(string username, string password)
         {
             Context.MobileApp.Do<ILogin>(i => i.LoginAs(username, password));
         }
      }
   ```
5. Under your App.config for your specs project, add the assembly to take in the external steps

   ```xml
       <stepAssemblies>
          <stepAssembly assembly="Joyride.Specflow" />
          <stepAssembly assembly="MySharedSteps.Specflow" />
       </stepAssemblies>
   ```
