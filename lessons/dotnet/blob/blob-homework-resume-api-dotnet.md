# Blob lesson Homework, improve your resume API, get the content from blob 

## Goal 🎯

The goal for this lesson is to grab the resume API you built in the [first homework assignment](../lessons/dotnet/http/http-homework-dotnet.md) and instead of including the json in your code, upload it to a blob container and read its contents using Blob bindings. 

## Assignment

Create an Azure Function with HTTP trigger that and  Blob binding that returns your resume information in JSON. The JSON should not be in your code, you should upload it to a blob container and grab it's contents from there, when the function is triggered.

## Resources

- Solution can be found [here](../../../src/dotnet/homework/resume-api/ResumeFromBlob.cs), try to accomplish it on your own first.
- Make sure to update your local.settings.json to use development storage and to have either storage emulator or Azurite running.
- Make sure your resume json file is in the correct blob container that your function is looking for.


## Share

Please share you solutions on LinkedIn and Twitter using the #AzFuncUni hashtag and mention us. We would love to see it!

[Gwyneth](https://twitter.com/madebygps) and [Marc](https://twitter.com/marcduiker)