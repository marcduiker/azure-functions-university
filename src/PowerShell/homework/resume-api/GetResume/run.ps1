using namespace System.Net

# Input bindings are passed in via param block.
param($Request, $TriggerMetadata)

# Write to the Azure Functions log stream.
Write-Host "PowerShell HTTP trigger function processed a request."

# Create an object to store all Resume data.
$ResumeData = [pscustomObject]@{
    firstName       = "Clippy"
    lastName        = "Clippit"
    location        = @{
        city        = 'Redmond'
        CountryCode = 'US'
    }
    currentPosition = "Office Assistant"
    "profiles"      = @(
        @{
            network = "Wikipedia"
            "url"   = "https://en.wikipedia.org/wiki/Office_Assistant"
        }
    )

}


# Associate values to output bindings by calling 'Push-OutputBinding'.
Push-OutputBinding -Name Response -Value ([HttpResponseContext]@{
        StatusCode = [HttpStatusCode]::OK
        # Change the object to a JSON file to return to the requester
        Body       = ($ResumeData | ConvertTo-Json)
    })
