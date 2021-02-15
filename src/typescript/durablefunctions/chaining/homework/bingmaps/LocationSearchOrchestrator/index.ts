import * as df from "durable-functions"

const orchestrator = df.orchestrator(function* (context) {

    const geolocation = yield context.df.callActivity("GetGeoencodedLocation", context.bindingData.input)

    context.bindingData.input.geolocationPoint0 = geolocation.resourceSets[0].resources[0].geocodePoints[0].coordinates[0]
    context.bindingData.input.geolocationPoint1 = geolocation.resourceSets[0].resources[0].geocodePoints[0].coordinates[1]

    const searchResult = yield context.df.callActivity("GetLocationSearchResult", context.bindingData.input)

    return searchResult.resourceSets[0].resources
})

export default orchestrator
