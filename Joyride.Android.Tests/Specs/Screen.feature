@android 
Feature:  Basic App Actions
    In order to use the app
	As a user
	I want to be launch and close the app


Scenario: Launching the app should be on main screen
Given I launch the "Api Demo App" mobile application
Then I should be on the "Main" screen

Scenario: Closing the app
Given I launch the "Api Demo App" mobile application
When I close the mobile application


# Not reliable due to locked rotation
@ignore 
Scenario Outline: Rotate screen to proper orientation
Given I launch the "Api Demo App" mobile application
And I wait for "3" seconds
When I rotate the screen to <orientation> orientation
Then the screen should be on <orientation> orientation

Examples: 
| orientation |
| landsape    |
| portrait    |

Scenario: Navigate to App screen
Given I launch the "Api Demo App" mobile application
When I tap the "App" button
Then I should be on the "App" screen