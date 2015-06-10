@android 
Feature:  Modal Dialog 
    In order to interact with modal dialog
	As a user
	I want to be able to resond with accept / dismiss 


Background: 
Given I launch the "Api Demo App" mobile application

Scenario: Navigate to Activity screen
Given I tap the "App" button
When I tap the "Alert Dialogs" button
Then I should be on the "Alert Dialogs" screen


Scenario: Should detect a modal dialog 
Given I tap the "App" button
And I tap the "Alert Dialogs" button
When I tap the "Ok Cancel Dialog" button
Then I should see the "Lorem Ipsum" modal dialog

Scenario: Should see title containing text in modal dialog
Given I tap the "App" button
And I tap the "Alert Dialogs" button
When I tap the "Ok Cancel Dialog" button
Then I should see the "Lorem Ipsum" modal dialog containing title text "Plloaso mako nuto siwuf"

Scenario: Should see title ending text in modal dialog
Given I tap the "App" button
And I tap the "Alert Dialogs" button
When I tap the "Ok Cancel Dialog" button
Then I should see the "Lorem Ipsum" modal dialog ends with title text "cakso dodtos anr koop."

Scenario Outline: Should be able to accept or dismiss modal dialog
Given I tap the "App" button
And I tap the "Alert Dialogs" button
And I tap the "Ok Cancel Dialog" button
When I <acceptOrDismiss> the "Lorem Ipsum" modal dialog
Then I should be on the "Alert Dialogs" screen

Examples: 
| acceptOrDismiss |
| accept          |
| dismiss         |

Scenario: Should be able to dismiss any modal dialog
Given I tap the "App" button
And I tap the "Alert Dialogs" button
And I tap the "Ok Cancel Dialog" button
When I dismiss any modal dialog
Then I should be on the "Alert Dialogs" screen


Scenario: Should detect the long message modal dialog 
Given I tap the "App" button
And I tap the "Alert Dialogs" button
When I tap the "Ok Cancel Dialog With Long Message" button
Then I should see the "Long Message" modal dialog


Scenario Outline: Respond to with the long message modal dialog 
Given I tap the "App" button
And I tap the "Alert Dialogs" button
And I tap the "Ok Cancel Dialog With Long Message" button
When I respond to the "Long Message" modal dialog with "<response>"
Then I should be on the "Alert Dialogs" screen

Examples: 
| response  |
| Something |
| Ok        |
| Cancel    |

Scenario: Should not see any modal dialog
Given I tap the "App" button
When I tap the "Alert Dialogs" button
Then I should not see any modal dialog

Scenario: Should see any modal dialog
Given I tap the "App" button
And I tap the "Alert Dialogs" button
When I tap the "Ok Cancel Dialog With Long Message" button
Then I should see any modal dialog
