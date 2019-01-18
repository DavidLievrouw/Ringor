@echo off

setlocal & pushd .

set "DIR=%~dp0"
cd %~dp0
TITLE Ringor -- Build
dotnet run --project "./src/Build/Build.csproj" --interactive
if errorlevel 2 pause