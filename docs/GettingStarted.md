
# Getting Started


## Tools of the Trade

Joyride uses built on top of appium to run the tests. Thus based from appium's requirements, you will need the following:

#### iOS Requirements

Mac OS X 10.7 or higher, 10.9.2 recommended
XCode >= 4.6.3, 5.1.1 recommended
Apple Developer Tools (iPhone simulator SDK, command line tools)
Ensure you read our documentation on setting yourself up for iOS testing!

#### Android Requirements

Android SDK API >= 17 (Additional features require 18/19)
Appium supports Android on OS X, Linux and Windows. Make sure you follow the directions for setting up your environment properly for testing on different OSes: linux, osx, windows


Follow appium's install guide which includes installing Node.js and installing appium as either the GUI version or the standalone command-line version.

A running instance of the appium server is required to run the tests.

  
#### Other Tools

Install the [https://visualstudiogallery.msdn.microsoft.com/9915524d-7fb0-43c3-bb3c-a8a14fbd40ee](Specflow extension) for Visual Studio 

The [http://nunit.org/index.php?p=vsTestAdapter&r=2.6.4](NUnit Test Runner).  Or if you use Resharper, this makes it easy.

Your target mobile app.  If you are testing on the device, which I also prefer, it is easy to have the app pre-installed and the environment and configurations ready to go.  For example, you are able to see the device using 'adb devices' on android.  You have the developer options enabled and some handy config settings such as disabling the lock screen and bumping the idle time before the phone sleeps.


## Creating your first project

  
1. Launch Visual Studio and create new solution.
2. Create a test project
3. Install Joyride.Specflow using via Nuget
4. From here, Joyride will install the other dependencies which include NUnit, Specflow, Joyride, etc.
5. Joyride adds several key files to your project:  App.config, Steps\SpecflowHooks.cs and a template Specs\FirstSpec.feature file.
6.  Modify the App.config with the appropriate settings under the joyride section. Y  