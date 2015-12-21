
#### Update your bin folder here ####
$ROOT_BIN = $PSScriptRoot + "\..\bin"
#####################################

$TOOLS_DIR = $PSScriptRoot

# Use Debug directory if exists, otherwise fallback using the release directory.
function GetBinDirectory {
  $DIR = $ROOT_BIN + "\Debug"
  if (-Not (Test-Path $DIR)) {
      if (-Not (Test-Path "$ROOT_BIN\Release")) {
         # error encountered.         
         return $null
      }
      else {
        $DIR = "$ROOT_BIN\Release"
      }
  }
  $DIR = [IO.Path]::GetFullPath($DIR)
  return $DIR
}


$RUNSPECS_EXE = $TOOLS_DIR + "\RunSpecs.ps1"
$GENERATEREPORT_EXE = $TOOLS_DIR + "\GenerateReport.ps1"
$BIN_DIR = GetBinDirectory


