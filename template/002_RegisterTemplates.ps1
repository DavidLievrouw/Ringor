. .\RunExe.ps1

Write-Host "Registering templates..."
$command = "dotnet"
$params = "new -i"
$packages = Get-ChildItem Dalion.*.nupkg
foreach($package in $packages) {
  $paramsWithPackage = "$params ""$package"""
  Write-Host "Registering package $package..."
  RunExe -command $command -params $paramsWithPackage
}
