dotnet tool install dotnet-reportgenerator-globaltool --tool-path tool
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
"tool/reportgenerator.exe" "-reports:LdapManager.Test/coverage.opencover.xml" "-targetdir:coverage" -reporttypes:Html;HtmlChart
pause