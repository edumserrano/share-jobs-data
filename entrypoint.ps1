function Main()
{
  [OutputType([Void])]
  param ([string[]] $inputArgs)

  # Remove any arg that is empty or whitespace. This removes empty strings that are passed from the action.yml.
  # This is a workaround to deal with non-required action inputs when this script is called via the action.yml.
  $argsAsList = [Collections.Generic.List[String]]::new()
  foreach ($arg in $inputArgs)
  {
    if(![string]::IsNullOrWhiteSpace($arg))
    {
      $argsAsList.Add($arg)
    }
  }

  $command = $inputArgs[0]
  # The --auth-token and --repo have a default value set on the action.yml for ease of use.
  # However, these options are only valid for the read-data-different-workflow command so
  # we have to remove them if we are executing a different command or else the CLI will return an error.
  if($command -ne "read-data-different-workflow")
  {
    $authTokenIdx = $argsAsList.IndexOf("--auth-token")
    if($authTokenIdx -ne -1)
    {
      $authTokenOption = $argsAsList[$authTokenIdx]
      $authTokenValue = $argsAsList[$authTokenIdx + 1]
      $argsAsList.Remove($authTokenOption) | Out-Null # supress True from showing in the output. See https://devblogs.microsoft.com/powershell/suppressing-return-values-in-powershell-functions/
      $argsAsList.Remove($authTokenValue) | Out-Null
    }

    $repoIdx = $argsAsList.IndexOf("--repo")
    if($repoIdx -ne -1)
    {
      $repoOption = $argsAsList[$repoIdx]
      $repoValue = $argsAsList[$repoIdx + 1]
      $argsAsList.Remove($repoOption) | Out-Null
      $argsAsList.Remove($repoValue) | Out-Null
    }
  }

  Write-Output "Executing: dotnet '/app/ShareJobsDataCli.dll' $argsAsList"
  # dotnet '/app/ShareJobsDataCli.dll' $argsAsList >> $env:GITHUB_OUTPUT
  dotnet '/app/ShareJobsDataCli.dll' $argsAsList

  if($LASTEXITCODE -ne 0 ) {
      Write-Output "::error::Share data jobs didn't complete successfully. See the step's log for more details."
      exit $LASTEXITCODE
  }
}

# invoke entrypoint function
Main $args
