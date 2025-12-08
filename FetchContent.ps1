param (
    [Parameter(Mandatory=$false)]
    [string]$Year = "2025",
    [Parameter(Mandatory=$true)]
    [string]$Day,
    [Parameter(Mandatory=$false)]
    [string]$SessionStr = "53616c7465645f5f3dc549a03976656a780e57e456bf06d83cf804adbe20f6f9c0ad03d7ac0a89b3ca78eb8ccbc40d9991397ef79fbd8da203dcb2d1d8aab0e9"
)


$session = New-Object Microsoft.PowerShell.Commands.WebRequestSession

                    
$cookie = New-Object System.Net.Cookie 

$cookie.Name = "session"
$cookie.Value = $SessionStr
$cookie.Domain = "adventofcode.com"

$session.Cookies.Add($cookie);

$uri = "https://adventofcode.com/$Year/day/$Day/input"
$Response = Invoke-WebRequest -Uri $uri -WebSession $session

$location = Get-Location

$Stream = [System.IO.StreamWriter]::new("$location\Day$Day\input.txt", $false)

try {
   $Stream.Write($Response.Content)
} finally {
   $Stream.Dispose()
}