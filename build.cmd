@echo off

setlocal & pushd .

set DIR=%~dp0
cd %DIR%/src
TITLE Ringor -- Build
dotnet run --project "./Build/Build.csproj" --interactive --workingDirectory %DIR%/src --publishDirectory ../dist
if errorlevel 2 pause