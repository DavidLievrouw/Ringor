function RunExe {
  param(
    [Parameter(Mandatory=$true, Position=0)] $command,
    [Parameter(Mandatory=$true, Position=1)] $params,
    [Parameter(Mandatory=$false, Position=2)] $workingDirectory)

  if (!$workingDirectory) {
    $workingDirectory = Get-Location
  }
  
  Write-Host "Running '$command $params' from $workingDirectory..."
  
  $args = $params.Split(" ")
  $tempFileName = [IO.Path]::GetTempFileName()
  $output = ""
  $spArgs = @{
    "FilePath" = $command
    "ArgumentList" = $args
    "NoNewWindow" = $true
    "Wait" = $true
    "RedirectStandardOutput" = $tempFileName
    "WorkingDirectory" = $workingDirectory
  }
  Start-Process @spArgs
  if (Test-Path $tempFileName) {
    $output = Get-Content $tempFileName
    Remove-Item $tempFileName
    Write-Host $output
  }
}