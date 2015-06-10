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
