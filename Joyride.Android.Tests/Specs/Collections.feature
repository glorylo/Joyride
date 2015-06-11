@android
Feature: Collections
  In order to access groups of elements
  as a user
  I want to able to interact and inspect collections

Background: 
Given I launch the "Api Demo" mobile application

Scenario: Should be able to tap the first item in a collection
Given I tap the "App" button
And I tap the "Fragment" button
And I tap the "Context Menu" button
And I tap the "Long Press" button and hold for "4" seconds
When I tap the "first" item in the "Menu" collection
Then I should be on the "Context Menu" screen

Scenario: Should be able to tap the last item in a collection
Given I tap the "App" button
And I tap the "Fragment" button
And I tap the "Context Menu" button
And I tap the "Long Press" button and hold for "4" seconds
When I tap the "last" item in the "Menu" collection
Then I should be on the "Context Menu" screen

Scenario: Should be able to tap the most recent item (same as last) in a collection
Given I tap the "App" button
And I tap the "Fragment" button
And I tap the "Context Menu" button
And I tap the "Long Press" button and hold for "4" seconds
When I tap the "most recent" item in the "Menu" collection
Then I should be on the "Context Menu" screen

Scenario: Should be able to tap the item by index in a collection
Given I tap the "App" button
And I tap the "Fragment" button
And I tap the "Context Menu" button
And I tap the "Long Press" button and hold for "4" seconds
When I tap the "2nd" item in the "Menu" collection
Then I should be on the "Context Menu" screen


Scenario: Should be able to see the correct count for collection
Given I tap the "App" button
And I tap the "Fragment" button
And I tap the "Context Menu" button
When I tap the "Long Press" button and hold for "4" seconds
Then I should see the collection "Menu" is not empty
Then I should see "2" items in "Menu" collection

Scenario: Tap adds 1 to size of collection
And I tap the "Animation" button
And I tap the "Default Layout Animations" button
When I tap the "Add Button" button
Then I should see "1" items in "Buttons" collection


Scenario: Should not see previous size of collection
And I tap the "Animation" button
And I tap the "Default Layout Animations" button
When I tap the "Add Button" button
And I tap the "Add Button" button
Then I should not see "1" items in "Buttons" collection

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

Scenario: Should not see 3 less items to saved collection size
And I tap the "Animation" button
And I tap the "Default Layout Animations" button
And I tap the "Add Button" button
And I tap the "Add Button" button
And I tap the "Add Button" button
And I tap the "Add Button" button
And I tap the "Add Button" button
And I inspect the number of items in collection "Buttons"
When I tap up to "3" item(s) in the "Buttons" collection
Then I should see "3" less item(s) in "Buttons" collection