@android
Feature: Ui Control 
	In order to interact with form elements
	As a user
	I want to be able to enter text, toggle checkboxes, etc.

Background: 
Given I launch the "Api Demo" mobile application

Scenario: Navigate to Activity screen
Given I tap the "App" button
When I tap the "Activity" button
Then I should be on the "Activity" screen

Scenario: Navigate to Custom Title screen
Given I tap the "App" button
And I tap the "Activity" button
When I tap the "Custom Title" button
Then I should be on the "Custom Title" screen

Scenario: Should be able to see an Left Title label present on screen
Given I tap the "App" button
And I tap the "Activity" button
When I tap the "Custom Title" button
Then the element "Left Title" should be present

Scenario: Should be able to enter text and verify saved changes
Given I tap the "App" button
And I tap the "Activity" button
And I tap the "Custom Title" button
And I enter "New Left Title" in the "Left" field
When I tap the "Change Left" button
Then I should see the label "Left Title" with text equals "New Left Title"

Scenario: Navigate to Presentation Screen
Given I tap the "App" button
And I tap the "Activity" button
And I do a moderate scroll down
When I tap the "Presentation" button
Then I should be on the "Presentation" screen

Scenario: Should show label when checkox is enabled 
Given I tap the "App" button
And I tap the "Activity" button
And I do a moderate scroll down
And I tap the "Presentation" button
When I check the "Show All Displays" checkbox
Then I should see the "Show All Displays" element checked
And I should see a label equals text "Display #0: Built-in Screen"

Scenario: Should not show label when checkox is enabled 
Given I tap the "App" button
And I tap the "Activity" button
And I do a moderate scroll down
And I tap the "Presentation" button
And I check the "Show All Displays" checkbox
When I uncheck the "Show All Displays" checkbox
Then I should not see the "Show All Displays" element checked
And I should not see a label equals text "Display #0: Built-in Screen"

