# Durable Functions -  Chaining Lesson Homework (TypeScript)

## Goal 🎯

The goal for this homework assignment is to create a Durable Function that executes a local search for business entities around a certain address and returns the result. The function comprises two steps:

1. Geo-encode the address via [Bing Maps API](https://www.microsoft.com/en-us/maps/choose-your-bing-maps-api)
2. Execute a local search via [Bing Maps API](https://www.microsoft.com/en-us/maps/choose-your-bing-maps-api)

You can easily get a API key by getting a [Bing Maps Basic Key](https://www.microsoft.com/en-us/maps/create-a-bing-maps-key/)

## Assignment

Create an HTTP-triggered Durable Function consisting of two activities that need to be executed in sequence. The input is an URLencoded address (e.g. Seattle,%20Pioneer%20Square) where you want to do the local search and the [type of business entity](https://docs.microsoft.com/en-us/bingmaps/rest-services/common-parameters-and-types/type-identifiers/) (e.g. coffee) you want to search for.

The first activity calls the REST API of Bing Maps to ["Find a Location by Address"](https://docs.microsoft.com/en-us/bingmaps/rest-services/locations/find-a-location-by-address).
The second activity uses the results of the first activity to call the REST API of Bing Maps to do a ["Local Search"](https://docs.microsoft.com/en-us/bingmaps/rest-services/locations/local-search).

We recommend to use [Axios](https://www.npmjs.com/package/axios) as HTTP client.

## Resources

* Solution can be found in the folder [`/src/typescript/homework/durable-functions/chaining/bingmaps`](../../../../src/typescript/homework/durable-functions/chaining/bingmaps), try to accomplish it on your own first.
* Make sure to update your local.settings.json to use development storage and to have storage emulator running.
* Store your API key in the local.settings.json.

## Share

Please share you solutions on LinkedIn and Twitter using the #AzFuncUni hashtag and mention us. We would love to see it!

[Marc](https://twitter.com/marcduiker) and [Christian](https://twitter.com/lechnerc77)