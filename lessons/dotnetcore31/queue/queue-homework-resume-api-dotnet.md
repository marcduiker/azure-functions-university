# Queue lesson Homework (.NET Core)

## Goal 🎯

The goal for this lesson is to create a function that reads an `Accomplishment` message from the `accomplishment-items` queue and saves this as a json file to blob storage in a `accomplishments` container. Examples of accomplishments:

- A certificate you achieved.
- An online course your completed
- A personal coding project you put online.

## Assignment

Create a Queue triggered Azure Function. The queue name will be `accomplishment-items`.
Add an `Accomplishment.cs` class with these properties:

- `string` *Title*
- `DateTime` *Date*
- `string` *Link*

The function will read an `Accomplishment` message from the `accomplishment-items` queue. Add a blob output binding (check the [Blob lesson](../blob/README.md) for more details). Specify the `accomplishments` container as the target location. Ensure that the blob filename contains the date from the `Accomplishment` object.

## Resources

- Make sure to update your local.settings.json to use development storage and to have either storage emulator or Azurite running.

## Share

Please share you solutions on LinkedIn and Twitter using the #AzFuncUni hashtag and mention us. We would love to see it!

[Gwyneth](https://twitter.com/madebygps) and [Marc](https://twitter.com/marcduiker)
