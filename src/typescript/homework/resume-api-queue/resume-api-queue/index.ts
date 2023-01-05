import { AzureFunction, Context } from '@azure/functions'

const queueTrigger: AzureFunction = async function (context: Context, myAccomplishmentMessage: string): Promise<void> {
  context.log('Queue trigger function processed work item', myAccomplishmentMessage)

  if (typeof (myAccomplishmentMessage) === 'object') {
    const accomplishment: Accomplishment = <any>myAccomplishmentMessage

    context.bindings.myAccomplishmentBlob = accomplishment

    context.log(`Accomplishment added to blob storage as stored-accomplishment-${accomplishment.id}-${accomplishment.date}.json`)
  }
  else {
    context.log.error(`Invalid input type of queue item. Provided ${typeof (myAccomplishmentMessage)}`)
  }

}

export default queueTrigger

interface Accomplishment {
  id: string
  type: string
  date: string
  grade: string
}