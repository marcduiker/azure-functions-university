import { AzureFunction, Context, HttpRequest } from '@azure/functions'

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
  const player: Player = req.body

  context.bindings.newPlayerItems = player

  context.log('HTTP trigger function processed a request.')

  context.res = {
    // status: 200, /* Defaults to 200 */
    body: `Queued the player with the nickname ${player.nickName}`
  }
}

export default httpTrigger

interface Player {
  id: string
  nickName: string
  email: string
  region: string
}
