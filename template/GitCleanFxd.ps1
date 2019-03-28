. .\RunExe.ps1

function GitCleanFxd {
  param([string] $directory)
  
  $command = "git"
  $params = "clean -fxd"
  RunExe -command $command -params $params -workingDirectory $directory
}
