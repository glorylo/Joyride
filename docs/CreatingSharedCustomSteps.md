# Creating Shared Custom Steps

1. If you would like to share custom steps across different screens or even different apps, simply have their screens implement a shared interface.  You should have organize your project such that the MobileApplication classes are in a separate project outside your "Specs" project.  
2.  A typical example is the login screen you have on both ios and android.  You have a shared interface, *ILogin* 

   ```csharp
      public interface ILogin
      {
         Screen LoginAs(string name, string password);
      }
   ```
   Another benefit of the interfaces is for situations where you have two apps, iOS and Android, and they each have their own release timelines.  If one feature is implemented on one app over other, it is self evident by the interfaces one screen implements.  If the missing feature eventually gets added to the app, instantly the step code is available and all that is needed is for the screens to implement the said interface.
3. Have your *LoginScreen* screens implement the *ILogin* interface.
4. Create another separate project to hold only the shared steps.  You will not need the *Specs* folder, *App.config* nor the *SpecflowHooks.cs* file.  Create a new class file such as *LoginSteps.cs* with a binding to that interface method.

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
5. Under your *App.config* for your respective specs project, add the assembly to take in the external steps

   ```xml
       <stepAssemblies>
          <stepAssembly assembly="Joyride.Specflow" />
          <stepAssembly assembly="MySharedSteps.Specflow" />
       </stepAssemblies>
   ```
6. Rebuild your solution.  
7. Voila!  Now the shared steps will be available to you. 
