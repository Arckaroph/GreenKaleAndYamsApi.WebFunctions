# Remove last Migration

$repo = "GreenKaleAndYams"
$project = "${repo}Api.Data"
$startup = "${repo}Api.WebFunction"
$context = "DatabaseContext"

echo "Removing last migration for $repo in $project project of context $context"

dotnet ef migrations remove --project "$project" --startup-project "$startup" --context "$context"

echo "Finished"