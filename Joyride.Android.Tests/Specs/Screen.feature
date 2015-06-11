@android 
Feature:  Basic App Actions
    In order to use the app
	As a user
	I want to be launch and close the app

Background: 
Given I launch the "Api Demo App" mobile application

Scenario: Launching the app should be on main screen
Then I should be on the "Main" screen

Scenario: Closing the app
When I close the mobile application


# Not reliable due to locked rotation
@ignore 
Scenario Outline: Rotate screen to proper orientation
And I wait for "3" seconds
When I rotate the screen to <orientation> orientation
Then the screen should be on <orientation> orientation

Examples: 
| orientation |
| landsape    |
| portrait    |

Scenario: Navigate to App screen
When I tap the "App" button
Then I should be on the "App" screen


Scenario: Navigate to back to Main screen
And I tap the "App" button
When I go back
Then I should be on the "Main" screen
