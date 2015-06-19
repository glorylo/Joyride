
# Comment out and add the appropriate tag for your platform
# @android or @ios

# See the list of predefined steps:  
# Generic steps:  https://github.com/glorylo/Joyride/blob/develop/docs/PredefinedSteps.md
# ios steps: https://github.com/glorylo/Joyride/blob/develop/docs/IosPredefinedSteps.md
# android steps: https://github.com/glorylo/Joyride/blob/develop/docs/AndroidPredefinedSteps.md
# webview steps: https://github.com/glorylo/Joyride/blob/develop/docs/WebviewPredefinedSteps.md

Feature: My First Feature
	In order to do usercase on my app
	As a user
	I want to be do X

Scenario: Launch my cool App
Given I launch the "My Cool App" mobile application
And I wait for "2" seconds
And I take a screenshot
