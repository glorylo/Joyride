
# Getting Started


## Tools of the Trade

Joyride is built on top of appium to run the tests. Thus based from appium's requirements, you will need the following:

#### iOS Requirements

Mac OS X 10.7 or higher, 10.9.2 recommended
XCode >= 4.6.3, 5.1.1 recommended
Apple Developer Tools (iPhone simulator SDK, command line tools)


#### Android Requirements

Android SDK API >= 17 (Additional features require 18/19)
Appium supports Android on OS X, Linux and Windows. Make sure you follow the directions for setting up your environment properly for testing on different OSes: linux, osx, windows


Follow appium's install guide for your environment.  Appium install also includes Node.js.  You can install the appium server as either the GUI version or the standalone command-line version.

A running instance of the appium server is required to run the tests.

  
#### Other Tools

Install the [Specflow extension](https://visualstudiogallery.msdn.microsoft.com/9915524d-7fb0-43c3-bb3c-a8a14fbd40ee) for Visual Studio 

Joyride uses NUnit for the testing framework.  The [NUnit Test Runner](http://nunit.org/index.php?p=vsTestAdapter&r=2.6.4).  Or if you use Resharper, this makes it easy.

Your target mobile app.  

A real device.  If available, we prefer testing a real device over emulator or simulator.  If you are using a device, ensure your device is setup properly for your environment.  

##### Android Devices
* Ensure you are able to see the device using 'adb devices' on android.  
* You have the developer options enabled.
* Disabling the lock screen
* Bumping Display setting such as the idle time before the phone sleeps to 30 minutes

##### iOS Devices
* Enabled UI Automation.  Settings -> Developer -> Enable UI Automation
* Have brew installed and install Webkit Debug Proxy for hybrid apps (brew install ios_webkit_debug_proxy)
* Turn on web inspector on Safari. Settings -> Safari -> Advanced -> Web Inspector ON



## Creating your first project

  
1. Launch Visual Studio and create new solution.
2. Create a test project
3. Install Joyride.Starter using Nuget
4. From here, Joyride.Starter will install the other dependencies which include NUnit, Specflow, Joyride, Joyride.Specflow etc.
5. Joyride adds several directories: Logs, Screenshots, Steps, and Specs.  The "Logs" and "Screenshots" folder store tracing dom data and screenshots respectively.  The "Steps" folder should include your custom "step" binding code for your specs.  The "Specs" folder contains all your .feature files.  After the install, you should see these key files:  *App.config*, *Steps\SpecflowHooks.cs* and a template *Specs\FirstSpec.feature* file.
6. Modify the *App.config* with the appropriate settings under the *joyride* section. The template provides the following:
  ```xml
    <joyride>
        <log>
          <add name="relativeLogPath" value="\..\..\Logs\" />
          <add name="relativeScreenshotPath" value="\..\..\Screenshots\" />
       </log>
       <capabilities>
          <add name="autoLaunch" value="false" type="System.Boolean" />
          <add name="fullReset" value="false" type="System.Boolean" />      
          <android>
            <add name="platformName" value="Android" />
            <add name="appPackage" value="com.my.test.app" />
            <add name="appActivity" value="activity-change-or-delete-me" />

          <devices>
              <device name="nexus5">
              <add name="deviceName" value="device-id-replace-me" />
              </device>

            <device name="nexus5_emulator">            
               <add name="appActivity" value="change-or-delete-me" />
               <add name="appWaitActivity" value="change-or-delete-me" />
            </device>
          </devices>  
        </android>

         <ios>
           <add name="platformName" value="iOS" />
            <add name="bundleId" value="com.my.test.app" />
            <add name="sendKeyStrategy" value="setValue" />

           <devices>
              <device name="iphone5s_4">
                <add name="deviceName" value="iPhone" />
                <add name="platformVersion" value="8.3" />
                <add name="udid" value="udid-replace-me" />
              </device>
            </devices>
         </ios>
       </capabilities>
       <servers>
         <add name="dev" value="http://127.0.0.1:4723/wd/hub" />
         <add name="ci" value="http://127.0.0.1:4723/wd/hub" />
        </servers>  
       <run>
         <add name="server" value="dev" />        <!-- change me to one of your available servers -->
         <add name="platform" value="android" />  <!-- either 'android' or 'ios' -->
         <add name="device" value="nexus5" />     <!-- change to target device's name -->
       </run>        
    </joyride>
   ```
   The *log* section sets relative paths to the working directory of your binary.  By default, the folders "Log" and "Screenshots" are saved in the same directory as your project directory.
   
   All the settings under *capabilities* map directly to [Appiums Capabilities](http://appium.io/slate/en/master/?csharp#appium-server-capabilities).  The settings are bundled together using [HandyConfig](https://www.nuget.org/packages/HandyConfig/). 

   The *capabilities* section includes global capabilities.  Joyride prefers to launch the app manually by setting *autoLaunch* to false.  Note the *type="System.Boolean"*.  You have to supply the correct type for the capabilities.  For example, you want the *newCommandTimeout* capability with a value of "70", also include *type="System.Int32"*.  If the *type* is not specified, the *"System.String"* is used as the default.

   The *android* and *ios* includes their respective platform capabilities. You can safely remove the section for the unused platform.  Update your app identifier with the appropriate *bundleId* for *ios* or *appPackage* for *android*.  

   Similarly, create a *device* target with device specfic capabilities.  If the same capabilitiy is specified here, it will supersede the capability setting before it.  Note the *name* of the device.  

   Review the *servers* and *run* section.  The above settings runs on *dev* for localhost, for *platform* using *android* on a *device* target of *nexus5*.  Change your run values accordingly.  For example, on ios, you will be running on remote appium server with a mac, the *platform* of *ios* and your desired device.

7. Define a new app.  In *Steps\SpecflowHooks.cs*, the new app will be used to set Context.MobileApp as below.  
   ```csharp
   [BeforeScenario]
   public void BeforeScenario()
   {
        Context.Driver = RemoteMobileDriver.GetInstance();
        // Add your test app here
        // Context.MobileApp = new TestApp();
        Context.MobileApp = new MyCoolApp();  
   }

   ```
   If you are creating an app you want to eventually share code, it is best to create a separate project for this.  Create a  new project in VS and add a new class.  Subclass the appropriate platform for your app.  Use *AndroidMobileApplication* or *IosMobileApplication*.

   ```csharp 
    public class MyCoolApp : AndroidMobileApplication
    {
        public override string Identifier { get { return "com.my.cool.app"; }}
    }
   ```

   Update SpecflowHooks with the appropriate new app.

8. The first spec is created for you under *Specs\FirstSpec.feature*.  Add the appropriate tag, either *@android* or *@ios*.   
   ```gherkin 
   # Comment out and add the appropriate tag for your platform
   # @android or @ios

   Feature: My First Feature
	In order to do usercase on my app
	As a user
	I want to be do X

   Scenario: Launch my cool App
   Given I launch the "My Cool App" mobile application
   And I wait for "2" seconds
   And I take a screenshot

   ```
   This first spec will simply launch your app and take a screen shot.  
9.  Build your project and run your first test.  
10.  If all goes well, you have just ran your first test!
