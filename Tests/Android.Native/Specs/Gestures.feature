@android
Feature: Gestures
	In order to use the api demo app
	As a user
	I want to be able to use gestures

Background: 
Given I launch the "Api Demo" mobile application

Scenario: Should be able to tap and hold
Given I tap the "App" button
And I tap the "Fragment" button
And I tap the "Context Menu" button
When I tap the "Long Press" button and hold for "4" seconds
Then I should see the collection "Menu" is not empty

Scenario: Should be able to double tap 
And I tap the "Animation" button
And I tap the "Default Layout Animations" button
When I double tap the "Add Button" button
Then I should see "2" items in "Buttons" collection
