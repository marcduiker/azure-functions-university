# Configuration lesson Homework

## Goal 🎯

The goal for this lesson is to extend the Blob Resume API homework and add a configurable message to the resume output.

## Assignment

If you haven't done so, familiarize yourself with the  [Blob Resume API](../blob/blob-homework-resume-api-dotnet.md) assignment. Instead of returning the resume json from Blob storage as is, add a message to the HTTP response object. It should look like this:

```json
{
    "message": "This resume was downloaded on Sunday 28 March 2021.",
    "resume": { [resume data] }
}
```

Ensure that these parts of the `message` field are configurable:

* *'This resume was downloaded on'*
* The date/time format of the timestamp.

## Share

Please share you solutions on LinkedIn and Twitter using the #AzFuncUni hashtag and mention us. We would love to see it!

[Stacy](https://twitter.com/Stacy_Cash) and [Marc](https://twitter.com/marcduiker)