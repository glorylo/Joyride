@android
Feature: Gestures
	In order to use the api demo app
	As a user
	I want to be able to use gestures

Background: 
Given I launch the "Api Demo" mobile application

Scenario: Navigate to Fragment Screen
Given I tap the "App" button
When I tap the "Fragment" button
Then I should be on the "Fragment" screen

Scenario: Navigate to Context Menu Screen
Given I tap the "App" button
And I tap the "Fragment" button
When I tap the "Context Menu" button
Then I should be on the "Context Menu" screen

Scenario: Should be able to tap and hold
Given I tap the "App" button
And I tap the "Fragment" button
And I tap the "Context Menu" button
When I tap the "Long Press" button and hold for "4" seconds
Then I should see "2" items in "Menu" collection


