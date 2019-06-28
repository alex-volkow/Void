@echo off

REM delete existing nuget packages
del bin\Release\*.nupkg

dotnet pack Void.Box.csproj -c Release

REM nuget push \bin\Release\*.nupkg
dotnet nuget push bin\Release\*.nupkg -k oy2e6jpimkn24iz26liqzw65ad4ijkmexk2dfsjozmys3a -s https://api.nuget.org/v3/index.json

@pause