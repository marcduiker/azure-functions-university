import { AzureFunction, Context } from '@azure/functions'

const queueTrigger: AzureFunction = async function (context: Context, myQueueItem: string): Promise<void> {
  context.log('Queue trigger function processed work item', myQueueItem)

  if (typeof (myQueueItem) === 'object') {
    const player: Player = <any>myQueueItem

    context.bindings.players = player

    context.log(`Player added to blob storage as stored-queue-message-${player.id}-${player.nickName}.json`)
  }
  else {
    context.log.error(`Invalid input type of queue item. Provided ${typeof (myQueueItem)}`)
  }

}

export default queueTrigger

interface Player {
  id: string
  nickName: string
  email: string
  region: string
}