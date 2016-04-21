<#

.SYNOPSIS

Executes a given test library's specs and outputs the results in an xml file.


.DESCRIPTION

The RunSpecs utility executes your specs and returns the output file.  You can optionally specify which features to include or exclude.  In addition, you can specify a directory to store the logging information.  

RunSpecs can run in two modes:  Simple and Fire-And-Forget modes.  In Simple mode, it will run the tests under the same process.  In Fire-And-Forget mode, you have to specify a configuration file, which gets utilized for spawning a separate process for execution.  You can do so to spawn multiple test runs.  

#>

[cmdletbinding(DefaultParameterSetName='NoWait')]
Param(
  
  [Parameter(Mandatory=$true,Position=1)][String] $TestLib,
  [Parameter(Mandatory=$true,Position=2)][String] $BinDir,
  [Parameter(Mandatory=$true,ParameterSetName="NoWait",Position=3)][String] $ConfigFile,
  [String] $LogDir= $null,
  [String] $Include = $null,
  [String] $Exclude = $null,
  [String] $OutputXmlFile = $null,
  [Parameter(ParameterSetName="NoWait")][switch] $NoExit,
  [Parameter(ParameterSetName="Wait")][switch] $Wait
)

#$PSCmdlet.ParameterSetName

$testLibPath = $BinDir + "\" + $TestLib
$testLibConfileFilename = $TestLib + ".config"
$testLibConfigPath = $BinDir + "\" + $testLibConfileFilename
$defaultConfigFilename = $TestLib + ".config.default"
$defaultConfigPath = $testLibConfigPath + ".default"

if ((-Not ([string]::IsNullOrEmpty($OutputXmlFile))) -And (Test-Path ($OutputXmlFile))) {
  echo "Existing output file exists.  Removing..."
  Remove-Item $OutputXmlFile -Force
}

if (-Not (([string]::IsNullOrEmpty($ConfigFile)) -Or (Test-Path ($ConfigFile)))) {
  echo "**** Error: Unable to find config file:  $ConfigFile ****"
  exit 1
} 

if (-Not(Test-Path ($testLibConfigPath))) {
  echo "**** Error: Unable to find default config file:  $testLibConfigPath ****"
  exit 1
}

if (-Not ([string]::IsNullOrEmpty($ConfigFile))) {

  if (Test-Path ($defaultConfigPath)) {
     echo "Default config file already exist. Removing: $defaultConfigPath"
	 Remove-Item $defaultConfigPath -Force
  }
  
  echo "Renaming default config file..."
  Rename-Item $testLibConfigPath $defaultConfigFilename
  echo "Using config file: $ConfigFile" 
  Copy-Item $ConfigFile $testLibConfigPath -force	
} 


[string[]]$powershellArguments  = @()

if ($NoExit) { 
  $powershellArguments += ' -NoExit'
}


$ExePath = $PSScriptRoot + "\ExecuteNUnit.ps1"
$powershellArguments += " -File `"$ExePath`""
$powershellArguments += " -TestLib `"$TestLib`""
$powershellArguments += " -BinDir `"$BinDir`""


if (-Not ([string]::IsNullOrEmpty($LogDir))) {
  $powershellArguments += " -LogDir `"$LogDir`""
}

if (-Not ([string]::IsNullOrEmpty($Include))) {
  $powershellArguments += " -Include `"$Include`""
}

if (-Not ([string]::IsNullOrEmpty($Exclude))) {
  $powershellArguments += " -Exclude `"$Exclude`""
}

if (-Not ([string]::IsNullOrEmpty($OutputXmlFile))) {
  $powershellArguments += " -OutputXmlFile `"$OutputXmlFile`""
}

#$powershellArguments

echo "Starting Test Session..."

if ($Wait) {
  Start-Process powershell.exe -NoNewWindow -Wait -ArgumentList $powershellArguments
} else {
  Start-Process powershell.exe -ArgumentList $powershellArguments
  echo "Waiting for test initialization..."
  Start-Sleep -s 12  
}


if (-Not ([string]::IsNullOrEmpty($ConfigFile))) {
  echo "Restoring default config file..."
  Remove-Item $testLibConfigPath 
  Rename-Item "$testLibConfigPath.default" $testLibConfileFilename
} 

if ($Wait) {
  Write-Output $OutputXmlFile
} else {
  echo "**** Spawned test session! ****"
  exit 0
}





