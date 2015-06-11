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
