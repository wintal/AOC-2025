param (
    [Parameter(Mandatory=$false)]
    [string]$Year = "2025",
    [Parameter(Mandatory=$true)]
    [string]$Day,
    [Parameter(Mandatory=$false)]
    [string]$SessionStr = "53616c7465645f5f869dbc0a33651b5adcb5a0f6ffdb19eca6f05d9002343ab1d7064282b03c3c339c3f78b1cdb2148bc0613cece6098d211a8686c348add53a"
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