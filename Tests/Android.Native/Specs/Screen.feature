@android 
Feature:  Screen actions
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
Given I wait for "3" seconds
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
Given I tap the "App" button
When I go back
Then I should be on the "Main" screen

Scenario: Should not be on Activity screen
Given I tap the "App" button
Then I should not be on the "Activity" screen

Scenario: Navigate to Activity screen
Given I tap the "App" button
When I tap the "Activity" button
Then I should be on the "Activity" screen

Scenario: Navigate to Custom Title screen
Given I tap the "App" button
And I tap the "Activity" button
When I tap the "Custom Title" button
Then I should be on the "Custom Title" screen

Scenario: Navigate to Animation screen
Given I tap the "Animation" button
Then I should be on the "Animation" screen

Scenario: Navigate to Default Layout Animations screen
Given I tap the "Animation" button
When I tap the "Default Layout Animations" button
Then I should be on the "Default Layout Animations" screen

Scenario: Navigate to Fragment Screen
Given I tap the "App" button
When I tap the "Fragment" button
Then I should be on the "Fragment" screen

Scenario: Navigate to Context Menu Screen
Given I tap the "App" button
And I tap the "Fragment" button
When I tap the "Context Menu" button
Then I should be on the "Context Menu" screen

Scenario:  Navigate to the Long Screen
Given I scroll the screen down
And I tap the "Views" button
And I scroll the screen down
And I tap the "Layouts" button
And I tap the "Scroll View" button
When I tap the "Long" button
Then I should be on the "Long" screen

Scenario: Navigate to Quick Contact Demo Screen
Given I tap the "App" button
And I tap the "Activity" button
And I scroll the screen down
When I tap the "Quick Contacts Demo" button
Then I should be on the "Quick Contacts Demo" screen

Scenario: Should see the following elements
Given I tap the "App" button
Then I should see the following elements
| Element      |
| Activity     |
| Fragment     |
| Notification |

Scenario: Should not have the following elements present
Given I tap the "App" button
When I tap the "Activity" button
Then the following elements should not be present
| Element  |
| Quick Contacts Demo |

