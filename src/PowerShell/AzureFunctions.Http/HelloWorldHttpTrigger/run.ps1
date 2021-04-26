using namespace System.Net

# Input bindings are passed in via param block.
param($Person, $TriggerMetadata)

# Write to the Azure Functions log stream.
Write-Host "PowerShell HTTP trigger function processed a request."

# Interact with query parameters or the body of the request.
if ($Person.Method -eq "GET") {
    $Name = $Person.Query.Name
}
elseif ($Person.Method -eq "POST") {
    $Name = $Person.Body.Name
}

if ([string]::IsNullOrEmpty($Name)) {
    $Body = "Pass a name in the query string or in the request body for a personalized response."
    $Result = [HttpStatusCode]::BadRequest
}
else {
    $Body = "Hello $Name. This HTTP triggered function executed successfully."
    $Result = [HttpStatusCode]::OK
}

# Associate values to output bindings by calling 'Push-OutputBinding'.
Push-OutputBinding -Name Response -Value ([HttpResponseContext]@{
        StatusCode = $Result
        Body       = $Body
    })