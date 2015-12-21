<#

.SYNOPSIS

Generates a HTML report based from an NUnit Xml input file.


.DESCRIPTION

GenerateReport utility can take a given XML file and output the generated HTML file.  You can optionally add a timestamp (-Timestamp) which will append timestamp to the output filename.  

For multiple input files, specify a given directory (-InputDir).  The directory should contain at least 2 XML files (although you can specify the exact number with -ExpectedPartitions).  The output file will be created for merged results.


#>

[cmdletbinding(DefaultParameterSetName="Single")]
Param (
  [Parameter(Mandatory=$true,Position=1)][string]$ProjectFile,
  [Parameter(Mandatory=$true,ValueFromPipeline=$true, ParameterSetName="Single")][string]$InputFile,
  [Parameter(Mandatory=$true,ParameterSetName="Multiple")][string]$InputDir,
  [Parameter(ParameterSetName="Multiple")][string]$Pattern = "*.xml",
  [Parameter(ParameterSetName="Multiple")][int]$ExpectedPartitions = 2,
  [Parameter(Mandatory=$true)][string]$OutputFile,
  [switch]$Timestamp
)

################# Update your paths to access specflow #################
$SPECFLOW_EXE = $PSScriptRoot + "\..\..\SpecFlow.1.9.0\specflow.exe"
$NUNITRESULTMERGER_EXE = $PSScriptRoot  + "\NUnitResultMerger.exe"
########################################################################

if (-Not(Test-Path ($ProjectFile))) {
  echo "**** Error: Unable to find project file:  $ProjectFile ****"
  exit 1
}

$ParamSetName = $PsCmdLet.ParameterSetName
$outputFileBase = [System.IO.Path]::GetFileNameWithoutExtension($OutputFile)
$OutputFileDir = Split-Path $OutputFile -Parent
$IntermediateResultsDir = ".\IntermediateResults"
$MergedFile = $IntermediateResultsDir + "\Merged.xml"  

if (-Not ([string]::IsNullOrEmpty($OutputFileDir) -Or (Test-Path ($OutputFileDir)))) {
   New-Item $OutputFileDir -ItemType Directory -Force
}

if ($ParamSetName -eq "Single") {

  if (-Not(Test-Path ($InputFile))) {
    echo "**** Error: Unable to find input file:  $InputFile ****"
    exit 1 
  }
  
  echo "Generating Report: $OutputFile"
   & $SPECFLOW_EXE nunitexecutionreport $ProjectFile /out:"$OutputFile" /xmlTestResult:"$InputFile"
}
else {
  $resultsFiles = $InputDir + "\" + $Pattern
  $countPartitions = (Get-ChildItem $resultsFiles).Count 

  if ($countPartitions -gt $ExpectedPartitions) 
  {
    echo "**** Error: The number of $countPartitions is greater than the expected:  $ExpectedPartitions ****"
    exit 1
  }

  if ($countPartitions -lt $ExpectedPartitions) 
  {
    echo "**** Error: Required $ExpectedPartitions partitions before proceeding with merging. ****"
    exit 1 
  }

  if (-Not (Test-Path $IntermediateResultsDir)) {
    New-Item $IntermediateResultsDir -ItemType Directory -Force 
  }
  
  del "$IntermediateResultsDir\*.xml"
  Copy-Item $resultsFiles $IntermediateResultsDir
  & $NUNITRESULTMERGER_EXE $IntermediateResultsDir *.xml $MergedFile
 
  Write-Host "Generating Report: $OutputFile"
  & $SPECFLOW_EXE nunitexecutionreport $ProjectFile /out:"$OutputFile" /xmlTestResult:"$MergedFile"

}

if ($Timestamp) {
  $datestring = (Get-Date).ToString("s").Replace(":","-")
  $timestamppedFile = $outputFileBase + '_' + "$datestring.html"
  Rename-Item $OutputFile $timestamppedFile -Force
  $timestamppedFilePath = $OutputFileDir + '\' + $timestamppedFile
  Write-Host "Created report with timestamp..." 
  Write-Output $timestamppedFilePath
} else {
  Write-Host "Created report: $OutputFile" 
  Write-Output $OutputFile
}




