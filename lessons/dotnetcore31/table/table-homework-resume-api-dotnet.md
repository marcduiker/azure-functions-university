# Table lesson Homework (.NET Core)

## Goal 🎯

The goal for this lesson is to create a function that reads an `Accomplishment` message from the `accomplishment-items` queue and saves this an an `AccomplishmentEntity` in an `accomplishments` table in Table Storage. Examples of accomplishments:

- A certificate you achieved.
- An online course your completed
- A personal coding project you put online.

## Assignment

Create a Queue triggered Azure Function. The queue name will be `accomplishment-items`.
Add an `AccomplishmentEntity.cs` class and inherit from `Entity` (reference the `Microsoft.Azure.Cosmos.Table` package). Add these properties:

- `string` *AccomplishmentType*
- `string` *Title*
- `DateTime` *Date*
- `string` *Link*

Add a constructor to the class and set the *PartitionKey* based on the *AccomplishmentType* (e.g. Certificate, Course, Project) and the *RowKey* based on the *Title*. Note that there are restrictions on the characters which can be used for the *PartitionKey* and *RowKey* fields, see [Understanding the Table service data model](https://docs.microsoft.com/rest/api/storageservices/Understanding-the-Table-Service-Data-Model).

The function will read an `Accomplishment` message from the `accomplishment-items` queue. Add a table output binding (check the [Table lesson](README.md) for more details). Specify the `accomplishments` table in the binding.

## Resources

- Make sure to update your local.settings.json to use development storage and to have storage emulator running (or use an Azure Storage account).

## Share

Please share you solutions on LinkedIn and Twitter using the #AzFuncUni hashtag and mention us. We would love to see it!

[Gwyneth](https://twitter.com/madebygps) and [Marc](https://twitter.com/marcduiker)
