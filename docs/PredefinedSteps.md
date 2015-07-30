# Mobile Application Steps


##### Given and Whens
```gherkin
Given I launch the "<mobile name>" mobile application
Given I close the mobile application
```


# Screen Steps

##### Given and Whens

```gherkin
Given I rotate the screen to <orientation> orientation # orientation can be landscape or portrait
```

##### Thens

The *Then* steps are used for assertions.  Most of the them have a **should not** equivalent.  For example, you can use the two steps below:

```gherkin
# using should
Then the element "Sign Up" should be present

# using should not
Then the element "Sign Up" should not be present
```


```gherkin
Then I should see the label "<label>" with text (equals|starts with|containing) "<text>"
Then the element "<element>" should be present
Then I should see the element "<element>"
Then I should be on the "<screen>" screen
Then I should see the text of element "<element>" (equals|starts with|containing|matching) "<text>"
Then I fail the scenario with reason "<reason>"
Then the following elements should be present
Then I should see the following elements
```

# Gestures


##### Given and Whens

```gherkin
Given I tap the "<element>" button
Given I double tap the "<element>" button
Given I tap the "<element>" button and hold for "<secs>" seconds
Given I scroll the screen <direction>
Given I do a (slight|moderate) scroll <direction>
Given I swipe the screen <direction>
Given I swipe the "<element>" <direction>
Given I pull the screen <direction>
Given I scroll the screen <direction> until I see element "<element>"
Given I pinch the screen to zoom <direction>
```
##### Then

```gherkin
Then I (slowly|moderately) scroll the screen <direction> until I see the following elements
```

# Collections

##### Given and Whens
```gherkin
Given I tap the "(first|most recent|last)" item in the "<collection>" collection
Given I tap the "<ordinal>" item in the "<collection>" collection
Given I swipe <direction> the "<ordinal>" item in the "<collection>" collection
Given I tap up to "<number>" items in the "<collection>" collection
Given I inspect the number of items in collection "<collection>"

```

##### Then


```gherkin
Then I should see an item in collection "<collection>" with text (equals|starts with|containing|matching) "<text>"
Then I should see "<number>" items in "<collection>" collection
Then I should see the collection "<collection>" is (not empty|empty)
Then I should see the number of items in the collection "<collection>" to be (less than|greater than) "<number>"
Then I should see "<number>" (less|more) items in "<collection>" collection
Then there should exists "<property>" within the collection "<collection>"
Then I should see all the following properties within the collection "<collection>"
Then I should see all the following properties within the collection "<collection>" meeting the conditions

```


# Debugging

Debug Steps can be used for Givens, Whens, or Thens.

```gherkin
Given I dump DOM trace
Given I dump DOM trace in webview
Given I wait for "<secs>" seconds
Given I take a screenshot
```

# Modal Dialogs

##### Given and Whens

```gherkin
Given I (accept|dismiss) the "<modal dialog>" modal dialog
Given I (accept|dismiss) any modal dialog
Given I respond to the "<modal dialog>" modal dialog with "<response>"
```

##### Thens

```gherkin

Then I should see any modal dialog
Then I should see the "<modal dialog>" modal dialog
Then I should see the "<modal dialog>" modal dialog (equals|ends with|starts with|containing|matching) (body|title) text "<text>"

```


# UI Controls


##### Given and Whens
```gherkin
Given I enter "<text>" in the "<text field>" field
Given I (uncheck|check) the "<checkbox>" checkbox
```
