# Azure Durable Functions - Advanced Patterns Homework (TypeScript)

## Goal 🎯

The goal for this homework assignment is to create a Durable Function flow that covers the booking of a business trip to a conference (when on-site conferences are a thing again ...) using different patterns like fan-out/fan-in. The booking tasks that need to be executed are:

- Buy a conference ticket
- Book a hotel room
- Book a train ride

## Assignment

Create the following workflows making use of the fan-out/fan-in pattern, the sub-orchestration and the external event handling of Azure Durable Functions:

1. Create a workflow that executes the three tasks in parallel for one person (fan-out/fan-in).
2. Create a workflow that triggers the three tasks in parallel for multiple persons (sub-orchestration).
3. Create a workflow that models a manual approval for the booking of the hotel room (external event).

## Resources

- Solution can be found in the folder [`/src/typescript/homework/durable-functions/advanced-patterns`](../../../../src/typescript/homework/durable-functions/advanced-patterns), try to accomplish it on your own first.
- Provide and use input parameters as you need to model the workflows.
- Mimic the tasks via Activity Functions that "just" log the executed task.
- Make sure to update your `local.settings.json` to use development storage and to have storage emulator running.
- We recommend to create new Orchestrator Function for each challenge of the assignment.

## Share

Please share you solutions on LinkedIn and Twitter using the #AzFuncUni hashtag and mention us. We would love to see it!

[Marc](https://twitter.com/marcduiker) and [Christian](https://twitter.com/lechnerc77)