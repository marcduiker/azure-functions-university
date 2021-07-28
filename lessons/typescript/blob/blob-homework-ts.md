# Blob lesson Homework, improve your resume API, get the content from blob

## Goal 🎯

The goal for this lesson is to grab the resume API you built in the [first homework assignment](../http/http_homework-ts.md) and instead of including the JSON in your code, upload it to a Blob container and read its contents using Blob bindings.

## Assignment

Create an Azure Function with HTTP trigger and  Blob binding that returns your resume information in a JSON response object. The JSON should not be in your code, you should upload it to a Blob container and grab its contents from there, when the Function is triggered.

## Resources

- Solution can be found [here](../../../src/typescript/homework/resume-api-blob), try to accomplish it on your own first.
- Make sure to update your `local.settings.json` to use development storage and to have Azurite running.
- Make sure your resume file is in the blob container that your function is looking for.

## Share

Please share you solutions on LinkedIn and Twitter using the #AzFuncUni hashtag and mention us. We would love to see it!

[Christian](https://twitter.com/lechnerc77) and [Marc](https://twitter.com/marcduiker)