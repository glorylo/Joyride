

# Introduction

Joyride is a cross platform mobile framework to interact with screens.  It supplies some of the basic touch gestures for native and hybrid mobile applications and is built on top of [Appium](http://appium.io).

Joyride.Specflow is the Behaviour Driven Development (BDD) layer that supplies a stock of useful actions such as tap and entering text, etc.  

# Dependencies

* Appium.WebDriver
* Selenium.WebDriver
* Selenium.Support
* HandyConfig
* Humanizer
* PredicateParser
* Newtonsoft.Json
* NUnit
* SpecFlow
* SpecFlow.NUnit

# Usage
1. Create a new project in Visual Studio
1. Add Joyride and Joyride.Specflow as references
1. Modify your app.config from specflows default.
1. Ensure app.config is using nunit:
     ```
   <specFlow>
     <!-- For additional details on SpecFlow configuration options see http://go.specflow.org/doc-config -->
     <unitTestProvider name="NUnit" />
 
     <stepAssemblies>
       <stepAssembly assembly="Joyride.Specflow" />
       <stepAssembly assembly="Pof.MobileApps.Specflow" />
 
     </stepAssemblies>
 
   </specFlow>
     ```
5. Add Joyride.Specflow as an external step assembly:
     ```
     <stepAssemblies>
       <stepAssembly assembly="Joyride.Specflow" />
     ...
 	<stepAssemblies>
     ```
 
6. More to come.


Have fun! 

Glory
