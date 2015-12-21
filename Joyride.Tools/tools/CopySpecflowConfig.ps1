$ConfigPath = $PSScriptRoot + "\..\..\SpecFlow.1.9.0\tools\specflow.exe.config"
$SPECFLOW_DIR = $PSScriptRoot + "\..\..\SpecFlow.1.9.0\tools"

echo "Checking specflow configuration..."
If ( -Not (Test-Path "$ConfigPath")) {

  copy "$PSScriptRoot\specflow.exe.config" $SPECFLOW_DIR
  echo "Setting config to run on .NET 4.0"
  exit 0
} 
else {
  echo "Skipping copying config file..."
}



