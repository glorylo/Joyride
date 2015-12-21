<#

.SYNOPSIS

Executes tests using NUnit's console runner.


.DESCRIPTION

The ExecuteNUnit utility runs the tests given the test library.  The results are saved in a xml file (default: TestResult.xml).  You can optionally add exclude and include tags to restrict which tests will be executed.  Finally you can optionally specify a directory (-LogDir) to output the test log.

#>

Param(
  
  [Parameter(Mandatory=$true,Position=1)][String] $TestLib,
  [Parameter(Mandatory=$true,Position=2)][String] $BinDir,
  [String] $LogDir = ".",
  [String] $Include = $null,
  [String] $Exclude = $null,
  [String] $OutputXmlFile = ".\TestResult.xml"
)

####### Update path for Nunit Console #######
$NUNITCONSOLE_EXE = $PSScriptRoot + "\..\..\NUnit.Runners.2.6.4\tools\nunit-console.exe"
#############################################

$testLibPath = $BinDir + "\" + $TestLib
$baseFileName = [System.IO.Path]::GetFileNameWithoutExtension($OutputXmlFile)
$OutputXmlFileDir = Split-Path $OutputXmlFile -Parent
$OutputLogFilePath = $LogDir + '\' + $baseFileName + '.txt'

#echo "BasefileName = $baseFileName"
#echo "LogDir = $LogDir"
#echo "OutputXmlFile = $OutputXmlFile"
#echo "OutputLogFilePath = $OutputLogFilePath"
#echo "Bin Directory = $BinDir"
#echo "Include = $Include"
#echo "Exclude = $Exclude"
#echo "Test Lib = $testLibPath"


if (-Not(Test-Path ($testLibPath))) {
  echo "**** Error: Unable to find test library:  $testLibPath ****"
  exit 1
}

if (-Not ([string]::IsNullOrEmpty($Include))) {
  $include=' /include:"' + $include + '"'
}

if (-Not ([string]::IsNullOrEmpty($Exclude))) {  
  $exclude=' /exclude:"' + $exclude + '"'
}

if (-Not (Test-Path ($LogDir))) {
  New-Item $LogDir -ItemType Directory -Force
}

if (-Not ([string]::IsNullOrEmpty($OutputXmlFileDir) -Or (Test-Path ($OutputXmlFileDir)))) {
   New-Item $OutputXmlFileDir -ItemType Directory -Force
}

if (Test-Path ($OutputXmlFile)) {
  echo "Existing output file exists.  Removing..."
  Remove-Item $OutputXmlFile -Force
}

$testOutput = ' /out="' + $OutputLogFilePath + '" ' + ' /xml="' + $OutputXmlFile + '"'
$runCommandExe = $NUNITCONSOLE_EXE
$runCommandParams = " $testLibPath" + $include + $exclude + " /labels /framework:net-4.0" + $testOutput
$runCommand  = $runCommandExe  + $runCommandParams

echo "Running command: $runCommand"


Invoke-Expression $runCommand 
