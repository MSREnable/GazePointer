param($installPath, $toolsPath, $package, $project)

$dllFiles = "Tobii.EyeX.Client.dll","TobiiGazeCore64.dll"

foreach ($dllFile in $dllFiles)
{
  $file1 = $project.ProjectItems.Item($dllFile)
  
  # set 'Copy To Output Directory' to 'Copy always'
  $copyToOutput1 = $file1.Properties.Item("CopyToOutputDirectory")
  $copyToOutput1.Value = 1
}
