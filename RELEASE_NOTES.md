
# Release History

### Version 0.0.14
- changed the way config settings is used to intiialize with appium server
- update your *SpecflowHooks.cs* and your *App.config* for compatibility moving forward

Add the *run* element in your *joyride* settings in your *App.config*:
```xml
  <joyride>
    <capabilities> ... </capabilities>
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
Update your *SpecflowHooks.cs* from 

```csharp
   	public const Platform TargetPlatform = Platform.Android;  // update either Platform.Android or Platform.Ios

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
		var projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
		JoyrideConfiguration.SetWorkingDirectory(projectDir);
		var capabilities = JoyrideConfiguration.BundleCapabilities(TargetPlatform, "nexus5"); // change the device
		var server = JoyrideConfiguration.GetServer(); // change the server.  default is "dev"
		RemoteMobileDriver.Initialize(server, TargetPlatform, capabilities);
    }
```

to

```csharp
	[BeforeTestRun]
	public static void BeforeTestRun()
	{
		var projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
		JoyrideConfiguration.SetWorkingDirectory(projectDir);
		var capabilities = JoyrideConfiguration.BundleCapabilities(); 
		var server = JoyrideConfiguration.GetServerUri(); 
		var targetPlatform = JoyrideConfiguration.TargetPlatform;
		RemoteMobileDriver.Initialize(server, targetPlatform, capabilities);
	}	
```

### Version 0.0.13
- added step for hasLabel on ios 

### Version 0.0.12
- added page indicator step
- moved a step for getting attributes in collection on ios to be deprecated

### Version 0.0.11
- added element disable / enabled step

### Version 0.0.10
- added clear text step 

### Version 0.0.9
- added scroll screen x times step

### Version 0.0.8
- verifying element text step can be now use curly braces
- added slowly scroll screen
- removed unused steps; old steps are tagged with deprecated

### Version 0.0.8
- verifying element text step can be now use curly braces
- added slowly scroll screen
- removed unused steps; old steps are tagged with deprecated
 
### Version 0.0.7
- replaced a Then step on verifying element text

### Version 0.0.6
- updated collection steps for better readability of plural/singular items

### Version 0.0.5
- added Then steps to verify elements from a table

### Version 0.0.4
- updated default ios title and body text for modal dialogs
- added symbols with package 

### Version 0.0.3
- modified IGesture and added swiping elements 
- moved starter project files into separate package:  Joyride.Starter 
- joyride.specflow now only contains the assembly.  If you are upgrading, backup your app.config file.  Moving forward you can simply upgrade without your config files being affected 
- added steps to swipe elements and pull down screen 

### Version 0.0.2

- added EnterTextBySetValue for iOS if you do not wish to use sendKeyStrategy with setValue strategy as the default
- added more negation steps   

### Version 0.0.1

- Initial release  
