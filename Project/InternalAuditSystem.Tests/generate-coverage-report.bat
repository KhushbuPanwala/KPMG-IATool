@echo off
setlocal EnableDelayedExpansion

SET dotnet="C:\Program Files\dotnet\dotnet.exe"
SET reportgenerator=".nuget\packages\reportgenerator\4.5.0\tools\net47\ReportGenerator.exe"

SET targetargs="test .\InternalAuditSystem.Test.csproj"
SET outputfolder=.\TestResults
SET coveragefilename=\coverage.cobertura.xml  
SET reportdir=.\TestResults\Report

REM Create folder if testresult folder not exist 
if not exist "%outputfolder%\*" md "%outputfolder%" || pause & goto :GenerateCoverage

REM create separate html file format for CI pipeline 
if "%1"== "CI" (SET outputfileFormat=HtmlInline_AzurePipelines;Cobertura ) else (SET outputfileFormat=Html) 

:GenerateCoverage 
REM Run code coverage analysis
dotnet test --results-directory ./ /p:CollectCoverage=true /p:CoverletOutput=%outputfolder%\ /p:CoverletOutputFormat=cobertura /p:Exclude="[xunit.*]*

REM Generate the report
%userprofile%\%reportgenerator% -reports:%outputfolder%%coveragefilename% -targetdir:%reportdir% -reporttypes:%outputfileFormat% 

REM Open the report
start "report" "%reportdir%\index.htm" 

pause

