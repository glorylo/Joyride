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
Given I tap the "Animation" button
And I tap the "Default Layout Animations" button
When I double tap the "Add Button" button
Then I should see "2" items in "Buttons" collection

Scenario: Slight scroll to navigate to Views screen
Given I do a slight scroll down
When I tap the "Views" button
Then I should be on the "Views" screen

Scenario: Moderate scroll to navigate to Presentation Screen
Given I tap the "App" button
And I tap the "Activity" button
And I do a moderate scroll down
When I tap the "Presentation" button
Then I should be on the "Presentation" screen

Scenario: Scroll the screen down to navigate to Layouts screen
Given I scroll the screen down
And I tap the "Views" button
And I scroll the screen down
When I tap the "Layouts" button
Then I should be on the "Layouts" screen

Scenario:  Slowly scroll until Button 63 is visible
Given I scroll the screen down
And I tap the "Views" button
And I scroll the screen down
And I tap the "Layouts" button
And I tap the "Scroll View" button
And I tap the "Long" button
When I slowly scroll the screen down until I see element "Button 63"
Then I should see the button "Button 63"

Scenario:  Moderately scroll until Button 63 is visible
Given I scroll the screen down
And I tap the "Views" button
And I scroll the screen down
And I tap the "Layouts" button
And I tap the "Scroll View" button
And I tap the "Long" button
When I moderately scroll the screen down until I see element "Button 63"
Then I should see the button "Button 63"

Scenario:  Scroll until Button 63 is visible
Given I scroll the screen down
And I tap the "Views" button
And I scroll the screen down
And I tap the "Layouts" button
And I tap the "Scroll View" button
And I tap the "Long" button
When I scroll the screen down until I see element "Button 63"
Then I should see the button "Button 63"

Scenario: Pull the Screen down and up
Given I tap the "App" button
And I tap the "Notification" button
And I tap the "Incoming Message" button
And I pull the screen down
And I wait for "2" seconds
When I pull the screen up
Then I should be on the "Incoming Message" screen

