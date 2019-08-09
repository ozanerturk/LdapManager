dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools/reportgenerator
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
"tools/reportgenerator/reportgenerator.exe" "-reports:LdapManager.Test/coverage.opencover.xml" "-targetdir:coverage" -reporttypes:Html;HtmlChart
pause