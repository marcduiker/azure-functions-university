import { AzureFunction, Context, HttpRequest } from '@azure/functions'

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
  const player1AsString = 'Player 1'
  context.bindings.newPlayerItems = player1AsString

  // Variant: Transfer an array of messages to the queue
  // const player1AsString = 'Player 1'
  // const player2AsString = 'Player 2'
  // context.bindings.newPlayerItems = [player1AsString, player2AsString]

  context.log('HTTP trigger function processed a queue output binding.')

  context.res = {
    body: `Queued the player ${player1AsString}`
  }
}

export default httpTrigger
