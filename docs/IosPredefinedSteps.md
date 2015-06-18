# iOS Steps

Add the @ios tag for your feature to unlock android specific steps

##### Given and Whens
```gherkin
Given I drag the "<slider>" slider with value "<value>"
Given I hide the keyboard
```


##### Thens

```gherkin
Then I should see the "<number>" in collection "<collection>" with (name|value) (equals|starts with|containing|matching) "<text>"
Then I should see the navigation bar title "<title>"

```


