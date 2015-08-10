@android
Feature: Webview
	In order to interact with hybrid apps
	As a user
	I want to be able access an elements inside webviews

Background: 
Given I launch the "Api Demo" mobile application

# unable to access webviews likely because the app did not enabled web debugging
Scenario: Navigate the Webviews Screen
Given I scroll the screen down
And I tap the "Views" button
And I scroll the screen down until I see element "Webview"
When I tap the "Webview" button
Then I should be on the "Webview" screen
#And I should see the label "Hello World"