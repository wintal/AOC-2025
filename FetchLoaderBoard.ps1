param (
    [Parameter(Mandatory=$false)]
    [string]$SessionStr = "53616c7465645f5f869dbc0a33651b5adcb5a0f6ffdb19eca6f05d9002343ab1d7064282b03c3c339c3f78b1cdb2148bc0613cece6098d211a8686c348add53a"
)

$session = New-Object Microsoft.PowerShell.Commands.WebRequestSession

                    
$cookie = New-Object System.Net.Cookie 

$cookie.Name = "session"
$cookie.Value = $SessionStr
$cookie.Domain = "adventofcode.com"

$session.Cookies.Add($cookie);

$uri = "https://adventofcode.com/2025/leaderboard/private/view/3229725.json"
$Response = Invoke-WebRequest -Uri $uri -WebSession $session

$Stream = [System.IO.StreamWriter]::new("Leaderboard.json", $false)

try {
   $Stream.Write($Response.Content)
} finally {
   $Stream.Dispose()
}