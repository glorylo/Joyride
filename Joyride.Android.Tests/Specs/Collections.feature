@android
Feature: Collections
  In order to access groups of elements
  as a user
  I want to able to interact and inspect collections

Background: 
Given I launch the "Api Demo" mobile application

Scenario: Should be able to tap an item in a collection
Given I tap the "App" button
And I tap the "Fragment" button
And I tap the "Context Menu" button
And I tap the "Long Press" button and hold for "4" seconds
When I tap the "1" item in the "Menu" collection
Then I should be on the "Context Menu" screen
