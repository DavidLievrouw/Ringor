. .\RunExe.ps1
. .\GitCleanFxd.ps1

function CreateNupkg {
  param([string] $templateName)
  
  $versionFile = (Get-Item -Path ".\${templateName}.Version.txt").FullName
  $version = Get-Content $versionFile -First 1
  $command = "dotnet"
  
  $params = "restore ""$templateName\$templateName.csproj"""
  RunExe -command $command -params $params
  
  $currentDirectory = (Get-Item -Path "$PSScriptRoot").FullName
  $params = "pack ""$templateName\$templateName.csproj"" --no-build -o $currentDirectory -p:NuspecFile=$templateName.nuspec -p:NuspecProperties=\""version=$version\"""
  RunExe -command $command -params $params
  
  if (Test-Path $templateName\bin) { Remove-Item $templateName\bin -Recurse -Force }
  if (Test-Path $templateName\obj) { Remove-Item $templateName\obj -Recurse -Force }
}

function RunForTemplate {
  param(
    [string] $templateName,
    [string] $sourceRoot
  )

  $destinationRoot = "${templateName}\content"
  
  # Clean source output  
  Write-Host "Cleaning contents for template ${templateName} in ${sourceRoot}..."
  GitCleanFxd $sourceRoot
  Write-Host "Done"
  
  # Update contents 
  Write-Host "Fetching contents for template ${templateName}..."
  if (Test-Path $destinationRoot) { Remove-Item $destinationRoot\* -Recurse -Force }
  if (!(Test-Path $destinationRoot)) { New-Item -ItemType directory -Path $destinationRoot }
  Copy-Item -Path "${sourceRoot}\*" -Recurse -Force -Destination $destinationRoot
  if (Test-Path $destinationRoot\bin) { Remove-Item $destinationRoot\bin -Recurse -Force }
  if (Test-Path $destinationRoot\obj) { Remove-Item $destinationRoot\obj -Recurse -Force }
  Write-Host "Done"
  
  # Create packages
  CreateNupkg -templateName $templateName
}

RunForTemplate -templateName "Dalion.WebApp" -sourceRoot (Get-Item -Path "..\src").FullName
