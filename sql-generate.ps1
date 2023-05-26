# SQL Migration Generator

$repo = "GreenKaleAndYams"
$sqlPath = "sql/migrations"
$project = "${repo}Api.Data"
$startup = "${repo}Api.WebFunction"
$context = "DatabaseContext"

echo "SQL Migration Generator started for $repo in $project project of context $context"

# Someone smarter than me generated this
$begin = @" 
PRINT 'Before TRY'
BEGIN TRY
	BEGIN TRAN
	PRINT 'First Statement in the TRY block'





"@
# Spacers in the output
$end = @" 



	PRINT 'Last Statement in the TRY block'
	COMMIT TRAN
END TRY
BEGIN CATCH
	PRINT 'In CATCH Block'
	IF(@@TRANCOUNT > 0)
		ROLLBACK TRAN;

	THROW; -- Raise error to the client.
END CATCH
PRINT 'After END CATCH'
GO
"@


$migrations = (dotnet ef migrations list --no-build --project "$project" --startup-project "$startup" --context "$context")

$lastMigration = "0"
foreach($migration in $migrations) {
	$migration = $migration -replace ' \(Pending\)', ''
	
	$exists = test-path $sqlPath/$migration.migration.sql
	if (!$exists) {
		echo "  Generating sql for $migration"

		dotnet ef migrations script $lastMigration $newMigration --project "$project" --startup-project "$startup" --context "$context" --output $sqlPath/$migration.migration.sql

		$exists = test-path $sqlPath/$migration.migration.sql
		if ($exists) {
			## read output file, replace GO statments and prepend and append transaction syntax
			$text = Get-Content $sqlPath/$migration.migration.sql -Raw 
			$text = $text.replace("GO`r`n","")
			"$begin$text$end" | Out-File $sqlPath/$migration.migration.sql -Encoding UTF8
		} else {
			echo "    --- FAILED --- unable to generate sql for $migration"
		}
	} else {
		echo "  Already exists for $migration"
	}

	$lastMigration = $migration
}

echo "Finished"