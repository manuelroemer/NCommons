Set-Location ../src
dotnet restore
dotnet build --configuration Release --no-restore

if ($LASTEXITCODE -ne 0) {
    throw 'Building the projects failed.'
}

Set-Location ../doc

# Clean any potentially left over files from a recent build.
# DocFX should automatically do this, but let's be safe.
Remove-Item _site -Recurse -ErrorAction Ignore
Remove-Item obj   -Recurse -ErrorAction Ignore
Remove-Item api/.manifest  -ErrorAction Ignore
Remove-Item api/*.yml      -ErrorAction Ignore

docfx metadata -f
docfx build