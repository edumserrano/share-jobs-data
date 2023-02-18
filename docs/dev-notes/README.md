# Dev notes

- [Building the ShareJobsData solution](#building-the-sharejobsdata-solution)
  - [Building with Visual Studio](#building-with-visual-studio)
  - [Building with dotnet CLI](#building-with-dotnet-cli)
- [Running ShareJobsData solution tests](#running-sharejobsdata-solution-tests)
  - [Run tests with Visual Studio](#run-tests-with-visual-studio)
  - [Run tests with dotnet CLI](#run-tests-with-dotnet-cli)
- [Building and running the Docker container action](#building-and-running-the-docker-container-action)
- [Projects wide configuration](#projects-wide-configuration)
- [Deterministic Build configuration](#deterministic-build-configuration)
- [Repository configuration](#repository-configuration)
- [GitHub Workflows](#github-workflows)
- [GitHub marketplace](#github-marketplace)
- [Note about the Docker container action](#note-about-the-docker-container-action)
  - [As of writing this, the log for building the docker action looks as follows](#as-of-writing-this-the-log-for-building-the-docker-action-looks-as-follows)
  - [As of writing this, the log for running the docker action looks as follows](#as-of-writing-this-the-log-for-running-the-docker-action-looks-as-follows)

## Building the ShareJobsData solution

### Building with Visual Studio

1) Clone the repo and open the **ShareJobsData.sln** solution file at `/ShareJobsData`.
2) Press build on Visual Studio.

### Building with dotnet CLI

1) Clone the repo and browse to the solution's directory at `/ShareJobsData` using your favorite shell.
2) Run **`dotnet build ShareJobsData.sln`** to build the source of the CLI app.

## Running ShareJobsData solution tests

### Run tests with Visual Studio

1) Clone the repo and open the **ShareJobsData.sln** solution file at `/ShareJobsData`.
2) Go to the test explorer in Visual Studio and run tests.

**Note:** [Remote testing](https://docs.microsoft.com/en-us/visualstudio/test/remote-testing?view=vs-2022) with WSL is configured on the solution which enables you to run the tests locally on Linux or on Windows. You can view the configuration file at [testenvironments.json](/ShareJobsData/testenvironments.json). To run the tests on Linux you need to have at least `Visual Studio 2022` and the Linux distro `Ubuntu-22.04` installed on [WSL](https://docs.microsoft.com/en-us/windows/wsl/install).

### Run tests with dotnet CLI

1) Clone the repo and browse to the solution's directory at `/ShareJobsData` using your favorite shell.
2) Run **`dotnet test ShareJobsData.sln`** to run tests.

## Building and running the Docker container action

The steps below show how to run the Docker container action against a set of test data provided by the repo. However you can follow the same steps and provide any data you wish to test.

1) Clone the repo and browse to the repo's directory.
2) Create an empty file named `github-step-output.txt` that will store the GitHub step output of the action. To create an empty file you can do something like `echo $null >> github-step-output.txt`.
3) Run `docker build -t share-jobs-data .`
4) Run the docker container and pass the required inputs to execute one of the available commands. To know the options for each available command you can check the documentation on the [main readme](/README.md) or do:

> **Note**
>
> When executing the docker image make sure you keep the docker run flags as shown below. To run any command other than the help command just change the input parameters for the `share-jobs-data` image.

```
# returns the available commands
docker run --rm --env GITHUB_OUTPUT=/workspace/github-step-output.txt `
-v ${pwd}:/workspace `
-v ${pwd}/github-step-output.txt:/workspace/github-step-output.txt `
--workdir /workspace `
share-jobs-data -h
```

```
# returns the usage options for the set-data command
docker run --rm --env GITHUB_OUTPUT=/workspace/github-step-output.txt `
-v ${pwd}:/workspace `
-v ${pwd}/github-step-output.txt:/workspace/github-step-output.txt `
--workdir /workspace `
share-jobs-data set-data -h
```

```
# returns the usage options for the read-data-current-workflow command
docker run --rm --env GITHUB_OUTPUT=/workspace/github-step-output.txt `
-v ${pwd}:/workspace `
-v ${pwd}/github-step-output.txt:/workspace/github-step-output.txt `
--workdir /workspace `
share-jobs-data read-data-current-workflow -h
```

```
# returns the usage options for the read-data-different-workflow command
docker run --rm --env GITHUB_OUTPUT=/workspace/github-step-output.txt `
-v ${pwd}:/workspace `
-v ${pwd}/github-step-output.txt:/workspace/github-step-output.txt `
--workdir /workspace `
share-jobs-data read-data-different-workflow -h
```

>**Warning**
>
> At the moment you cannot execute the `set-data` and `read-data-current-workflow` locally because they require a special kind of auth token which is only available on the agents running the workflow. See [this comment](https://github.com/actions/upload-artifact/issues/180#issuecomment-1086306269):
>
> "They're effectively "internal" APIs that don't hit api.github.com but some of our backend services. Anyone can hit them but we're deliberately not advertising this and these APIs are not documented on https://docs.github.com/en/rest/reference/actions#artifacts because it works with a special token the runner has as opposed to GITHUB_TOKEN or a PAT. Outside of the context of a run you can't upload artifacts."

## Projects wide configuration

The [Directory.Build.props](/ShareJobsData/Directory.Build.props) enables several settings as well as adds some common NuGet packages for all projects.

There is a set of NuGet packages that are only applied in test projects by using the condition `"'$(IsTestProject)' == 'true'"`. To make this work the `csproj` for the test projects must have the `<IsTestProject>true</IsTestProject>` property defined. Adding this property manually shouldn't be needed because it should be added by the `Microsoft.NET.Test.Sdk` package however there seems to be an issue with this when running tests outside of Visual Studio. See [this GitHub issue](https://github.com/dotnet/sdk/issues/3790#issuecomment-1100773198) for more info.

## Deterministic Build configuration

Following the guide from [Deterministic Builds](https://github.com/clairernovotny/DeterministicBuilds) the `ContinuousIntegrationBuild` setting on the [Directory.Build.props](/ShareJobsData/Directory.Build.props) is set to true, if the build is being executed in GitHub actions.

## Repository configuration

From all the GitHub repository settings the configurations worth mentioning are:

- **Automatically delete head branches** is enabled: after pull requests are merged, head branches are deleted automatically.
- **Branch protection rules**. There is a branch protection rule for the the `main` branch that will enforce the following:
  - Require status checks to pass before merging.
  - Require branches to be up to date before merging.
  - Require linear history.

## GitHub Workflows

For more information about the GitHub workflows configured for this repo go [here](/docs/dev-notes/workflows/README.md).

## GitHub marketplace

This action is published to the [GitHub marketplace](https://github.com/marketplace/actions/share-github-jobs-data).

**Currently there is no workflow setup to publish this action to the marketplace. The publishing act is a manual process following the instructions below.**

When publishing a new version:

- create a new tag for the release and update the latest major tag to the same commit as the new tag.
- the docker image tag for `docker://ghcr.io/edumserrano/github-issue-forms-parser` in the [action.yml](/action.yml) file needs to be updated to a new major version if a new major version has been published.

Once a new tag is pushed the workflow to publish a docker image will execute and publish a docker image to GitHub packages that will contain a docker image tag that matches the new GitHub tag.

From here on you can follow the instruction at [how to publish or remove an action from the marketplace](https://docs.github.com/en/actions/creating-actions/publishing-actions-in-github-marketplace).

## Note about the Docker container action

This repo provides a [Docker container action](https://docs.github.com/en/actions/creating-actions/creating-a-docker-container-action). If executing the `Share Jobs Data CLI` fails then the action [will fail](https://docs.github.com/en/enterprise-cloud@latest/actions/creating-actions/setting-exit-codes-for-actions#setting-a-failure-exit-code-in-a-docker-container-action). See here for more information about the [syntax for a Docker container action](https://docs.github.com/en/actions/creating-actions/metadata-syntax-for-github-actions#runs-for-docker-container-actions).

To understand better how the action builds and executes the Docker container look at the log for the steps that build and run the action.

### As of writing this, the log for building the docker action looks as follows

> **Note**
>
> This is the log when building the docker image for the action, which happens for instance on the [test-action-current-workflow workflow](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-current-workflow.yml) because using the published action from GitHub Marketplace will download the package from the GitHub packages and so the log will look different.
>
> The information mentioned here is still valuable to understand more about how GitHub Docker actions work.

```
/usr/bin/docker build
-t 5364e3:10ad17d734e944afa36bd83acaec12cc
 -f "/home/runner/work/_actions/edumserrano/share-jobs-data/v1.0.0/Dockerfile"
 "/home/runner/work/_actions/edumserrano/share-jobs-data/v1.0.0"
```

Note that the `docker build` command points to the Dockerfile at `/home/runner/work/_actions/edumserrano/share-jobs-data/v1.0.0/Dockerfile`. What is happening here is that GitHub clones the action's repository into the GitHub runner's working directory of the repo making use of this action. The clone of action's repo will be under the `_actions` folder.

This way it can successfully build the Dockerfile for this action which would otherwise fail since the Dockerfile references files in the action's repository which would not be present in the repository making use of this action.

**Example:**

- Repository `hello-world` creates a workflow that uses the `Share GitHub jobs data` action.
- When the workflow is executing, it contains a setup step that runs before any of the workflow defined steps. This step will clone the `Share GitHub jobs data` action's repo into the runner's working directory under the `_actions` folder and build the Docker container.
- This allows the Dockerfile to reference files in the `Share GitHub jobs data` repo even though the workflow has not explicitly checked it out.

### As of writing this, the log for running the docker action looks as follows

> **Note**
>
> This is the log when building the docker image for the action, which happens for instance on the [test-action-current-workflow workflow](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-current-workflow.yml) because using the published action from GitHub Marketplace will download the package from the GitHub packages and so the log will look different.
>
> The information mentioned here is still valuable to understand more about how GitHub Docker actions work.

```
/usr/bin/docker run
--name e310ad17d734e944afa36bd83acaec12cc_ae3534
--label 5364e3
--workdir /github/workspace
--rm
-e "INPUT_COMMAND" -e "INPUT_ARTIFACT-NAME" -e "INPUT_DATA-FILENAME"
-e "INPUT_RUN-ID" -e "INPUT_OUTPUT" -e "INPUT_DATA"
-e "INPUT_AUTH-TOKEN" -e "INPUT_REPO" -e "HOME"
-e "GITHUB_JOB" -e "GITHUB_REF" -e "GITHUB_SHA"
-e "GITHUB_REPOSITORY" -e "GITHUB_REPOSITORY_OWNER" -e "GITHUB_RUN_ID"
-e "GITHUB_RUN_NUMBER" -e "GITHUB_RETENTION_DAYS" -e "GITHUB_RUN_ATTEMPT"
-e "GITHUB_ACTOR" -e "GITHUB_TRIGGERING_ACTOR" -e "GITHUB_WORKFLOW"
-e "GITHUB_HEAD_REF" -e "GITHUB_BASE_REF" -e "GITHUB_EVENT_NAME"
-e "GITHUB_SERVER_URL" -e "GITHUB_API_URL" -e "GITHUB_GRAPHQL_URL"
-e "GITHUB_REF_NAME" -e "GITHUB_REF_PROTECTED" -e "GITHUB_REF_TYPE"
-e "GITHUB_WORKSPACE" -e "GITHUB_ACTION" -e "GITHUB_EVENT_PATH"
-e "GITHUB_ACTION_REPOSITORY" -e "GITHUB_ACTION_REF" -e "GITHUB_PATH"
-e "GITHUB_ENV" -e "GITHUB_STEP_SUMMARY" -e "GITHUB_STATE"
-e "GITHUB_OUTPUT" -e "GITHUB_ACTION_PATH" -e "RUNNER_OS"
-e "RUNNER_ARCH" -e "RUNNER_NAME" -e "RUNNER_TOOL_CACHE"
-e "RUNNER_TEMP" -e "RUNNER_WORKSPACE" -e "ACTIONS_RUNTIME_URL"
-e "ACTIONS_RUNTIME_TOKEN" -e "ACTIONS_CACHE_URL" -e GITHUB_ACTIONS=true -e CI=true
-v "/var/run/docker.sock":"/var/run/docker.sock"
-v "/home/runner/work/_temp/_github_home":"/github/home"
-v "/home/runner/work/_temp/_github_workflow":"/github/workflow"
-v "/home/runner/work/_temp/_runner_file_commands":"/github/file_commands"
-v "/home/runner/work/dotnet-sdk-extensions/dotnet-sdk-extensions":"/github/workspace"
5364e3:10ad17d734e944afa36bd83acaec12cc <action input parameters>
```

When running the docker container there are lots of docker parameters set. Besides all the environment variables note that there are several volume mounts. More importantly, note that the contents of the checked out repo where the action is executing is mounted into the container at `/github/workspace` and that the `workdir` is also set to `/github/workspace`.

>**Note**
>
> Currently this action does not access any files checked out by the workflow and does not produce any output files. However the volume mapping explained above is what would allow this action to do both if required.

**Imagine the following scenario:**

- Repository `hello-world` creates a workflow that uses the `Share GitHub jobs data` action.
- For the purpose of this scenario, imagine that there was an `output` option that would allow you to write the shared data to a file. Let's say we define the output file to be named `./output.json`.
- When the workflow is executing the Docker container is will output the file to the github workspace with the chosen names because because the contents of the checked out `hello-world` repo are mounted into the Docker container at `/github/workspace`. Furthermore the output of the file `./output.json` doesn't need to start with `/github/workspace` because the `workdir` parameter is set to `/github/workspace` when executing the Docker container.
- When the action finishes and the container is terminated the user can access the output file at `${{ github.workspace}}/output.json`.
