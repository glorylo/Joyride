# Handling Modal Dialogs

1. To handle modal dialogs you define an abstract base class, that all modal dialogs would subclass.  You can add common functionality for all your modal dialogs similar to screens.  

   ```csharp
       abstract public class MyCoolAppModalDialog : AndroidModalDialog
       {
       }
   ```
2. Add modal detection to your screens.  

   ```csharp
      public abstract class MyCoolAndroidScreen : AndroidScreen 
      { 
        static MyCoolAndroidScreen()
        {
            var assembly = Assembly.GetExecutingAssembly();
            ModalDialogDetector = new AndroidModalDialogDetector(assembly, typeof(MyCoolAppModalDialog));
        }         
      }
   ```
3. Create your first modal dialog by subclassing MyCoolAppModalDialog.  Here's a typical OK/Cancel modal dialog you would write on android:

   ```csharp
      [Detect(Priority = 10)]
      public class ConfirmSendMessageModalDialog : MyCoolAppModalDialog
      {
        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='OK']")]
        private IWebElement Ok;

        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='Cancel']")]
        private IWebElement Cancel;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/message']")]
        private IWebElement BodyMessage;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/alertTitle']")]
        private IWebElement AlertTitle;

        public ConfirmSendMessageModalDialog()
        {
           TransitionMap.Add("Ok", ScreenFactory.CreateScreen<InboxScreen>);
           TransitionMap.Add("Cancel", NoTransition);
        }

        public override bool IsOnScreen(int timeOutSecs)
        {
            return ElementExists("Alert Title", timeOutSecs) &&
                   AlertTitle.Text.Equals("Confirm sending the message?");
        }

        public override string Body
        {
            get { return BodyMessage.Text; }
        }

        public override string Title
        {
            get { return AlertTitle.Text; }
        }
        public override Screen Accept()
        {
            Ok.Click();
            return TransitionFromResponse("Ok");
        }

        public override Screen Dismiss()
        {
            Cancel.Click();
            return TransitionFromResponse("Cancel");
        }

        public override Screen RespondWith(string response)
        {
            switch (response)
            {
                case "Ok":
                    return Accept();
                case "Cancel":
                    return Dismiss();
                default:
                    throw new UndefinedResponseException("Undefined response of: " + response);
            }
        }
        public override string Name
        {
            get { return "Confirm Send Message"; }
        }
   ```
   There is lots going on here.  Similar to screens, you define the *Name* and *IsOnScreen* method.
   
4. You will also define the element mappings similar to screen. 

   ```csharp
        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='OK']")]
        private IWebElement Ok;

        [FindsBy(How = How.XPath, Using = "//android.widget.Button[@text='Cancel']")]
        private IWebElement Cancel;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/message']")]
        private IWebElement BodyMessage;

        [FindsBy(How = How.XPath, Using = "//*[@resource-id='android:id/alertTitle']")]
        private IWebElement AlertTitle;
   ```
   For Android, you can would assign mappings to all the buttons, body message and title text.  If your app has a similar look and feel to all your alert title, and body text, it would be better to put it up your base class.
5. In your constructor, initialize the transitions depending on user's responses.  

   ```csharp
      TransitionMap.Add("Ok", ScreenFactory.CreateScreen<InboxScreen>);
      TransitionMap.Add("Cancel", NoTransition);
   ```
   In this example, "Ok" transitions to *InboxScreen* while "Cancel" uses *NoTransition* (which returns it back to its original screen).

6. Implement your responses:

   ```csharp
      public override Screen Accept()
      {
         Ok.Click();
         return TransitionFromResponse("Ok");
      }

      public override Screen Dismiss()
      {
         Cancel.Click();
         return TransitionFromResponse("Cancel");
      }

      public override Screen RespondWith(string response)
      {
         switch (response)
         {
            case "Ok":
              return Accept();
            case "Cancel":
              return Dismiss();
            default:
              throw new UndefinedResponseException("Undefined response of: " + response);
          }
      }
   ```
   On Android, you would need to manually tap on the buttons such as "Ok" and "Cancel".  On iOS, you can use switch contexts and accept or dismiss the dialog:
   
   ```csharp
        // on an iOS modal dialog
        public override Screen Accept()
        {
            Driver.SwitchTo().Alert().Accept(); 
            return TransitionFromResponse("Ok");
        }
        public override Screen Dismiss()
        {
            Driver.SwitchTo().Alert().Dismiss();
            return TransitionFromResponse("Cancel");
        }
   ```
   Lastly the *RespondWith* is used for modal dialogs that have more than 2 options.

7. Check the [listing of steps](https://github.com/glorylo/Joyride/blob/develop/docs/PredefinedSteps.md#modal-dialogs) using modal dialogs
   

### Modal Dialog Detection

1. For step involving detecting any modal dialog, the *ModalDialogDetector* is used.  
   This occurs in the  step involving detecting any modal dialogs in:
   ```gherkin
      Given I dismiss any modal dialog
   ```
   You can give modal dialogs priority such as above with:

   ```csharp
      [Detect(Priority = 10)]
      public class ConfirmSendMessageModalDialog : MyCoolAppModalDialog
   ```
   The priority queue ranges from normal range of 1 - 100, where the lower the number, the higher priority.  You may have a several modal dialogs with the same priority and in which case their order of detection among each other is randomized. Without adding a *Detect* attribute, the detection of that particular modal dialog is dropped to the bottom of the queue of 100.  If you want something to be last in detection, simply give it a priority value of over 100.
   
2. Some screens you want to restrict the dialogs to a subset for detection.  Let's say you have a particular screen, which only has a set of modal dialogs appear on load.  If the *I dismiss any modal dialog* step is used, you can restrict the particular screen to dismissing any of the subset in the **order of dialogs to detect**.  You can override the *AcceptModalDialog* method:
   ```csharp
      public override Screen AcceptModalDialog(bool accept)
      {
         return AcceptModalDialogs(accept, 
            typeof (EnableLocationPermissionModalDialog),
            typeof (EnablePhotosPermissionModalDialog),
            typeof (EnableAccessContactPermissionModalDialog));
      }
   ```
   
