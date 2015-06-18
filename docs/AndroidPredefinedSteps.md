# Android Steps

Add the @android tag for your feature to unlock android specific steps

##### Given and Whens

```gherkin
Given I go back
Given I hide the keyboard
```


##### Thens

```gherkin
Then I should see a label (equals|starts with|containing|matching) text "<text>"
Then I should see the "<element>" checkbox checked
```

