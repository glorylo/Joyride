# Element And Collection Mappings

Without elements to interact our specs would be pretty bland.  Each screen you define can contain many element mappings or a collection of related elements.

### Single Element Mappings

1. For any screen you define a element as a private field.  Here's how a screen may look like:

   ```csharp
      public class ContextMenuScreen : ApiDemoScreen
      {
         [FindsBy(How = How.Id, Using = "io.appium.android.apis:id/long_press")] 
         private IWebElement LongPress;

         [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/select_dialog_listview']//*[@resource-id='android:id/title']")]
         private IList<IWebElement> Menu;
         ...
      } 
   ```
2. In the *ContextMenuScreen*, it has two elements:  *LongPress* and *Menu*.  Notice that FindsBy attribute where *LongPress* uses an id locator while *Menu* uses an xpath locator.  You can make use of different locator strategies that best fits your situation.

3. Joyride makes use of Humanizer to convert elements into its readable equivalent.  In your specs if you were to access the *LongPress* element, you reference it as *Long Press* in your specs.  Here's how it reads in an actual scenario:
   ```gherkin
   Scenario: Should be able to see the correct count for collection
   Given ...
   When I tap the "Long Press" button and hold for "4" seconds
   ```
   
### Add Cache Lookup   
1. Certain elements are often repeatedly accessed while remaining on the same screen.  In such situations, you can add cache lookup to the element.  
   ```csharp
      public class DefaultLayoutAnimationsScreen : ApiDemoScreen
      {
         [FindsBy(How = How.Id, Using = "io.appium.android.apis:id/addNewButton")][CacheLookup]
         private IWebElement AddButton;
         ...
      }
   ```

2. A scenario that makes use of caching the *AddButton*.  
   ```gherkin
   Scenario: Should not see 3 more added to saved collection size
   And I tap the "Animation" button
   And I tap the "Default Layout Animations" button
   And I tap the "Add Button" button
   And I tap the "Add Button" button
   And I inspect the number of items in collection "Buttons"
   When I tap the "Add Button" button
   And I tap the "Add Button" button
   And I tap the "Add Button" button
   Then I should see "3" more item(s) in "Buttons" collection
   ```

### Collection Mappings
1. Sometimes you have a collection of related elements you want to group together such as photos or contacts.  You can make use of  collection mappings.
   ```csharp
      [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/list']//android.widget.TextView")]
      private IList<IWebElement> FirstNames;

      [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/list']/android.widget.RelativeLayout")]
      private IList<IWebElement> Contacts;
   ```
2. Doing so you can access collection steps in your specs:
   ```gherkin
      Then I should see an item in collection "First Names" with text equals "Kingsley"
      ...
      
      Then I should see the label equals text "Kingsley" within the "Contacts" collection
      ...
   ```
   See [Predefined Steps](https://github.com/glorylo/Joyride/blob/develop/docs/PredefinedSteps.md) for more built in steps for collections
   
