import { AzureFunction, Context } from '@azure/functions'

const queueTrigger: AzureFunction = async function (context: Context, myQueueItem: string): Promise<void> {
  throw new Error(`Let's throw an exception to test the retry logic`)
}

export default queueTrigger
