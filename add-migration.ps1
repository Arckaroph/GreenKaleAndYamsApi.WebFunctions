# Add New Migration

Param (
	[Parameter(Mandatory = $true)] [string] $migration
)

$repo = "GreenKaleAndYams"
$project = "${repo}Api.Data"
$startup = "${repo}Api.WebFunction"
$context = "DatabaseContext"

echo "Adding new migration $migration for $repo in $project project of context $context"
echo "dotnet ef migration add $migration --project $project --startup-project $startup --context $context"

dotnet ef migrations add $migration --project "$project" --startup-project "$startup" --context "$context"

echo "Finished"