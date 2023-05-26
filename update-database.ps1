Param
(
	[Parameter(Mandatory = $false)][string]$server = "localhost",
	[Parameter(Mandatory = $false)][string]$database = "GreenKaleAndYams",
	[Parameter(Mandatory = $false)][switch]$CreateDatabase
)

$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';

try {
	# ErrorAction must be Stop in order to trigger catch
	Import-Module SqlServer -ErrorAction Stop
} catch {
	[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
	Install-PackageProvider -Name NuGet -Force
	Install-Module -Name SqlServer -AllowClobber -Force
	Import-Module SqlServer
}

if ($CreateDatabase.IsPresent) {
	Write-Output "Creating database..."
	invoke-sqlcmd -Server "$server" -Query "CREATE database $database"
}

echo "update database..."


$scripts = @()
gci -Recurse @(".\sql\migrations\*.sql") | % {
	$scripts += $_.FullName
}
#gci -Recurse @(".\sql\proc\*.sql") | % {
#	$scripts += $_.FullName
#}
gci -Recurse @(".\sql\data\*.sql") | % {
	$scripts += $_.FullName
}


echo "Server: $server"
echo "Database: $database"

foreach ($script in $scripts) {
	echo "running $script"
	
	invoke-sqlcmd -Server "$server" -Database $database -inputFile "$script"
}

echo "done"
