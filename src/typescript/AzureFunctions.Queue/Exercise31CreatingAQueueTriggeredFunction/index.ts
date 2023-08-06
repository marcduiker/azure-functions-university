import { AzureFunction, Context } from '@azure/functions'

const queueTrigger: AzureFunction = async function (context: Context, myQueueItem: string): Promise<void> {
  context.log('Queue trigger function processed work item', myQueueItem)
}

export default queueTrigger
