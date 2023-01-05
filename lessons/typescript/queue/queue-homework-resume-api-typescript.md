# Queue lesson Homework (TypeScript)

## Goal 🎯

The goal for this lesson is to create a function that reads an `Accomplishment` message from the `accomplishment-items` queue and saves this as a JSON file to Blob Storage in a `accomplishments` container. Here are some examples of accomplishments:

- A certificate you achieved.
- An online course your completed.
- A personal coding project you put online.

## Assignment

Create a Queue triggered Azure Function. The queue name will be `accomplishment-items`.

The Function will read an `Accomplishment` message from the `accomplishment-items` queue. Add a Blob output binding (check the [Blob lesson](../blob/README.md) for more details). Specify the `accomplishments` container as the target location. Ensure that the Blob filename contains the date from the `Accomplishment` object.

## Resources

- Solution can be found [here](../../../src/typescript/homework/resume-api-queue), try to accomplish it on your own first.
- Make sure to update your `local.settings.json` to use development storage and to have Azurite running.

## Share

Please share you solutions on LinkedIn and Twitter using the #AzFuncUni hashtag and mention us. We would love to see it!

[Christian](https://twitter.com/lechnerc77) and [Marc](https://twitter.com/marcduiker)
