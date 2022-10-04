# Share GitHub jobs data

[![Build and test](https://github.com/edumserrano/share-jobs-data/actions/workflows/build-test.yml/badge.svg)](https://github.com/edumserrano/share-jobs-data/actions/workflows/build-test.yml)
[![Test GH action: share data on current workflow](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-current-workflow.yml/badge.svg)](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-current-workflow.yml)
[![Test GH action: share data on different workflow](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-different-workflow.yml/badge.svg)](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-different-workflow.yml)
[![codecov](https://codecov.io/gh/edumserrano/share-jobs-data/branch/main/graph/badge.svg?token=MWdQgdSFZD)](https://codecov.io/gh/edumserrano/share-jobs-data)
[![GitHub Marketplace](https://img.shields.io/badge/Marketplace-Share%20GitHub%20jobs%20data-blue.svg?colorA=24292e&colorB=0366d6&style=flat&longCache=true&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAYAAAAfSC3RAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAM6wAADOsB5dZE0gAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAERSURBVCiRhZG/SsMxFEZPfsVJ61jbxaF0cRQRcRJ9hlYn30IHN/+9iquDCOIsblIrOjqKgy5aKoJQj4O3EEtbPwhJbr6Te28CmdSKeqzeqr0YbfVIrTBKakvtOl5dtTkK+v4HfA9PEyBFCY9AGVgCBLaBp1jPAyfAJ/AAdIEG0dNAiyP7+K1qIfMdonZic6+WJoBJvQlvuwDqcXadUuqPA1NKAlexbRTAIMvMOCjTbMwl1LtI/6KWJ5Q6rT6Ht1MA58AX8Apcqqt5r2qhrgAXQC3CZ6i1+KMd9TRu3MvA3aH/fFPnBodb6oe6HM8+lYHrGdRXW8M9bMZtPXUji69lmf5Cmamq7quNLFZXD9Rq7v0Bpc1o/tp0fisAAAAASUVORK5CYII=)](https://github.com/marketplace/actions/share-github-jobs-data)

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](./LICENSE)
[![GitHub Sponsors](https://img.shields.io/github/sponsors/edumserrano)](https://github.com/sponsors/edumserrano)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-Eduardo%20Serrano-blue.svg)](https://www.linkedin.com/in/eduardomserrano/)

- [Description](#description)
- [Motivation](#motivation)
- [Usage](#usage)
  - [Share data across jobs in the same workflow](#share-data-across-jobs-in-the-same-workflow)
  - [Share data across jobs in different workflows](#share-data-across-jobs-in-different-workflows)
- [Action inputs](#action-inputs)
- [Action outputs](#action-outputs)
- [Tips](#tips)
  - [What is the difference between using `strict-json` or `github-step-json` as the `ouput`?](#what-is-the-difference-between-using-strict-json-or-github-step-json-as-the-ouput)
  - [Can I use GitHub's `fromJson` function to parse the JSON data?](#can-i-use-githubs-fromjson-function-to-parse-the-json-data)
  - [Can I share multiple sets of data?](#can-i-share-multiple-sets-of-data)
  - [Can I read shared data in the same job?](#can-i-read-shared-data-in-the-same-job)
  - [Can I share data with multiline YAML?](#can-i-share-data-with-multiline-yaml)
  - [Can I use output from other steps when defining the data to share?](#can-i-use-output-from-other-steps-when-defining-the-data-to-share)
  - [Can I share any YAML data?](#can-i-share-any-yaml-data)
  - [Is there a limit to the amount of data I can share?](#is-there-a-limit-to-the-amount-of-data-i-can-share)
  - [What options are available for each `command`?](#what-options-are-available-for-each-command)
  - [I can't figure out the output from this action. What do I do?](#i-cant-figure-out-the-output-from-this-action-what-do-i-do)

## Description

A Docker container [GitHub action](https://docs.github.com/en/actions/learn-github-actions/finding-and-customizing-actions) that can be used to share data across [GitHub workflow jobs](https://docs.github.com/en/actions/using-jobs/using-jobs-in-a-workflow). The data to share is defined in YAML and stored as an artifact on the workflow. Subsquent jobs, in the same workflow or different workflows, download the artifact with the shared data and set it as an output of the step.

## Motivation

I built this GitHub action because I found myself repeating the same GitHub steps to share data across jobs. On the side of the job producing data, I would normally serialize the data I want to share into JSON format and upload it as an artifact. Then on the consuming job I would download the artifact and deserialize the data in JSON format so that I could make use of it.

Tired of repeating the GitHub steps to achieve this I decided to create this GitHub action.

## Usage

The examples below are as concise as possible to demonstrate how this action works. There are more action inputs you can use to configure the behavior of this action.

Also note that when reading the shared data, the step reading the data must have an `id` so that you can access the step's output later on.

### Share data across jobs in the same workflow

>**Note**
>
> Since the data is shared via a worklfow artifact, the job producing the data must finish and upload the data before the consuming job can access it. This dependency is declared via the `needs` field set to the consuming job.

```yml
name: Example of sharing data in the same workflow

on:
  push:
    branches: [ main ]

jobs:
  produce-job-data:
    name: Do something and produce some job data
    runs-on: ubuntu-latest
    steps:
    - name: Set data
      uses: edumserrano/share-jobs-data@v1.0.0
      with:
        command: set-data
        data: |
          name: Eduardo Serrano
          age: 21

  consume-job-data:
    name: Read job data and do something
    runs-on: ubuntu-latest
    needs: [produce-job-data] # wait for the producer to share the data
    steps:
    # The step below will set the shared data as an output of the step
    # so that you can use it on subsequent steps
    - name: Read data
      id: read-data  # must have an id so that you can access the output from this step which contains the shared data
      uses: edumserrano/share-jobs-data@v1.0.0
      with:
        command: read-data-current-workflow
    - name: Do something with the shared data
      shell: pwsh
      run: |
        Write-Output '${{ steps.read-data.outputs.name }}'  # outputs: Eduardo Serrano
        Write-Output '${{ steps.read-data.outputs.age }}'   # outputs: 21
```

### Share data across jobs in different workflows

>**Note**
>
> To use this flow, you need to know the `run id` of the job that shared the data. A possible approach is to create a workflow that is triggered when the workflow that shares the data completes. This will let you access the `run id` of the job that shared the data via `${{github.event.workflow_run.id}}`. You can do this as follows:

```yml
# This workflow will share data that will be consumed in a separate workflow
name: Example of sharing data in a workflow

on:
  push:
    branches: [ main ]

jobs:
  produce-job-data:
    name: Do something and produce some job data
    runs-on: ubuntu-latest
    steps:
    - name: Set data
      uses: edumserrano/share-jobs-data@v1.0.0
      with:
        command: set-data
        data: |
          name: Eduardo Serrano
          age: 21
```

```yml
name: Example of consuming data from a separate workflow

# This workflow is triggered when the workflow that produces the data is finished
on:
  workflow_run:
    workflows: [
      "Example of sharing data in a workflow",
    ]
    types:
    - completed

jobs:
  consume-job-data:
    name: Read job data and do something
    runs-on: ubuntu-latest
    steps:
    # The step below will set the shared data as an output of the step
    # so that you can use it on subsequent steps
    - name: Read data
      id: read-data  # must have an id so that you can access the output from this step which contains the shared data
      uses: edumserrano/share-jobs-data@v1.0.0
      with:
        command: read-data-different-workflow
        run-id: ${{ github.event.workflow_run.id }}  # we need to specify the run id of the workflow that shared the data
    - name: Do something with the shared data
      shell: pwsh
      run: |
        Write-Output '${{ steps.read-data.outputs.name }}'  # outputs: Eduardo Serrano
        Write-Output '${{ steps.read-data.outputs.age }}'   # outputs: 21
```

## Action inputs

This action supports the following flows:

- set and read data across jobs in the same workflow.
- set and read data across jobs in different workflow.

The operation that the action performs is controlled via the `command` action input.

>**Warning**
>
> Some action inputs are only valid for a specifc `command` as indicated by the `restricted to command` column.

| Name            | restricted to command          | Description                                                                                                                          | Required | Default                                  |
| --------------- | ------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------ | -------- | ---------------------------------------- |
| `command`       | ---                            | Command to use: `set-data`, `read-data-current-workflow` or `read-data-different-workflow`.                                          | yes      | ---                                      |
| `artifact-name` | ---                            | The name of the workflow artifact where data is going to be stored or retrieved from.                                                | yes      | `job-data`                               |
| `data-filename` | ---                            | The filename inside the workflow artifact that contains (in case of reading) or will contain (in case of writing) the data to share. | yes      | `job-data.json`                          |
| `output`        | ---                            | How to output the job data in the step's output. It must be one of: `none`, `strict-json`, `github-step-json`.                       | yes      | `github-step-json`                       |
| `data`          | `set-data`                     | The data to share in YAML format.                                                                                                    | no       | ---                                      |
| `auth-token`    | `read-data-different-workflow` | GitHub token used to download the job data artifact.                                                                                 | no       | `github.token` (job token)               |
| `repo`          | `read-data-different-workflow` | The repository for the workflow run in the format of `{owner}/{repo}`.                                                               | no       | `github.repository` (current repository) |
| `run-id`        | `read-data-different-workflow` | The unique identifier of the workflow run that contains the job data artifact.                                                       | no       | ---                                      |

| Command option                 | Description                                                                     |
| ------------------------------ | ------------------------------------------------------------------------------- |
| `set-data`                     | Shares data as a workflow artifact.                                             |
| `read-data-current-workflow`   | Reads data that has been previously shared by another job in the same workflow. |
| `read-data-different-workflow` | Reads data that has been shared by another workflow.                            |

| Output option      | Description                                                                                                 |
| ------------------ | ----------------------------------------------------------------------------------------------------------- |
| `none`             | Nothing is set on the step's output. This option is only available when the `command` is set to `set-data`. |
| `strict-json`      | Sets the shared data in JSON format on the step's output under the key `data`.                              |
| `github-step-json` | Sets the shared data in a `JSON like` format on the step's output.                                          |

See [here](#what-is-the-difference-between-using-strict-json-or-github-step-json-as-the-ouput) to better understand which `output` option you should use.

## Action outputs

| Name   | Description                                                                                     |
| ------ | ----------------------------------------------------------------------------------------------- |
| `data` | Job data in JSON format. Populated only when the `output` action input is set to `strict-json`. |

>**Warning**
>
>- When setting the `output` action input to `none`: nothing is written to the step's output.
>- When setting the `output` action input to `github-step-json`: the action outputs are dynamic, they will depend on the shared data. See [here](#what-is-the-difference-between-using-strict-json-or-github-step-json-as-the-ouput) for more detail.

## Tips

>**Note**
>
> Some of the examples below use GitHub shell steps using powershell but everything works the same if you prefer to use [another shell](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions#jobsjob_idstepsshell).

### What is the difference between using `strict-json` or `github-step-json` as the `ouput`?

When creating this action I wanted to make it easier as possible for the shared data to be consumed. As such I defined two options that can be used to control how this action's output:

- `strict-json`.
- `github-step-json`.

When using `strict-json`, the shared YAML data is converted to JSON and set as the action's `data` output. When using this `output` option you are responsible for processing the shared data before accessing it. For instance, you could use a powershell step to parse the JSON output before acessing the data:

```yml
# Imagine you have shared the following YAML:
#
# name: Eduardo Serrano
# age: 21
#
# After reading the shared data on a step with id 'read' and using the ouput option of `strict-json`
# you can access the JSON output as follows:
- name: Do something with the shared data
  shell: pwsh
  run: |
    # parse JSON into an object and the access the data
    $data = '${{ steps.read.outputs.data }}' | ConvertFrom-Json
    Write-Output $data.name # outputs: Eduardo Serrano
    Write-Output $data.age  # outputs: 21
```

When using `github-step-json`, the shared YAML data is converted to JSON but instead of setting the JSON string as the single output of the action, the action will set an output for each JSON value using the JSON path compatible with GitHub step output as the output key. For instance, when sharing a YAML object like this:

```yml
# Imagine you have shared the following YAML:
#
# name:
#   first: Eduardo
#   last: Serrano
# age: 21
#
# After reading the shared data on a step with id 'read' and using the ouput option of `github-step-json`
# you can access the values from the shared YAML as follows:
- name: Do something with the shared data
  shell: pwsh
  run: |
    Write-Output '${{ steps.read.outputs.name_first }}' # outputs: Eduardo
    Write-Output '${{ steps.read.outputs.name_last }}'  # outputs: Serrano
    Write-Output '${{ steps.read.outputs.age }}'        # outputs: 21
```

**What happens is that any `.` or `[` or `]` character that is normally used as part of the JSON path, is replaced by the `_` character. This is done because a GitHub action output cannot contain those characters.**

Using `github-step-json`, you don't have to parse the JSON from the shared data.

### Can I use GitHub's `fromJson` function to parse the JSON data?

Yes, you can use the [`fromJson` function](https://docs.github.com/en/actions/learn-github-actions/expressions#fromjson) as follows:

```yml
# Imagine you have shared the following YAML:
#
# name:
#   first: Eduardo
#   last: Serrano
# age: 21
#
# After reading the shared data on a step with id 'read' and using the ouput option of `strict-json`
# you can access the JSON output as follows:
- name: Do something with the shared data
  shell: pwsh
  run: |
    Write-Output '${{ fromJson(steps.read.outputs.data).name.first }}'  # outputs: Eduardo
    Write-Output '${{ fromJson(steps.read.outputs.data).name.last }}'   # outputs: Serrano
    Write-Output '${{ fromJson(steps.read.outputs.data).age }}'         # outputs: 21
```

### Can I share multiple sets of data?

Yes, you can use this action multiple times in the same workflow with the `set-data` command as long as you use unique names for the `artifact-name` action input. This action input defines the name of the artifact that is uploaded to the workflow with the shared data. You cannot have multiple artifacts with the same name in the same workflow.

### Can I read shared data in the same job?

Yes you can. In some scenarios you want to share some data in `Job A` to be accessed by `Job B` but you also want to access that data on the following steps in `Job A`.

To do this set the `output` action input to any value other than `none` when executing the `set-data` command. For instance:

```yml
jobs:
  produce-job-data:
    name: Do something and produce some job data
    runs-on: ubuntu-latest
    steps:
    - name: Set data
      id: set # must have an id so that you can access the output from this step on later steps in the same job
      uses: edumserrano/share-jobs-data@v1.0.0
      with:
        command: set-data
        output: github-step-json # or `strict-json`, if you use `none` there won't be any output set on this step
        data: |
          name: Eduardo Serrano
          age: 21
    - name: Read the shared data because I also want to do something with it here
      shell: pwsh
      run: |
        Write-Output '${{ steps.set.outputs.name }}' # outputs: Eduardo Serrano
        Write-Output '${{ steps.set.outputs.age}}'   # outputs: 21
```

### Can I share data with multiline YAML?

Yes, you should be able to use most of YAML syntax. To share data with multiple lines you can do:

```yml
- name: Set data
  uses: edumserrano/share-jobs-data@v1.0.0
  with:
    command: set-data
    data: |
      name: Eduardo Serrano
      age: 21
      address: |
        First line of address
        Second line of address
        Third line of address
```

When acessing the `address` from the shared data you will get back a multiline string as follows:

```
First line of address{newline}
Second line of address{newline}
Third line of address
```

### Can I use output from other steps when defining the data to share?

Yes, you can build the data to share by hardcoding values or by building it from data from other steps. For instance:

```yml
- name: Set name value
  id: set-name
  run: |
    Write-Output '::set-output name=name::Eduardo Serrano'
- name: Set data
  uses: edumserrano/share-jobs-data@v1.0.0
  with:
    command: set-data
    data: |
      name: ${{ steps.set-name.outputs.name }} # this is equivalent to 'name: Eduardo Serrano'
      age: 21
```

### Can I share any YAML data?

I didn't test all possible YAML syntax but I tested nested properties and lists without issue. If you find something that isn't supported open an issue and let's talk about it.

### Is there a limit to the amount of data I can share?

As of writing this, the [maximum size for the output of a step is 1 MB](https://docs.github.com/en/actions/creating-actions/metadata-syntax-for-github-actions#outputs-for-docker-container-and-javascript-actions). This means you are limited to share data that is less than 1MB.

### What options are available for each `command`?

You can figure out which options are valid for each `command` via the [Action inputs](#action-inputs) table but this is a more direct way to understand your options.

The `set-data` command supports:

- `command`
- `artifact-name`
- `data-filename`
- `output`
- `data`

The `read-data-current-workflow` command supports:

- `command`
- `artifact-name`
- `data-filename`
- `output`

The `read-data-different-workflow` command supports:

- `command`
- `artifact-name`
- `data-filename`
- `output`
- `auth-token`
- `repo`
- `run-id`

### I can't figure out the output from this action. What do I do?

If you're struggling to get the right keys for the output of this action then you can try the following:

```yml
# Imagine you have shared the following YAML:
#
# name: Eduardo Serrano
# age: 21
#
- name: Read data
  id: read-data  # must have an id so that you can access the output from this step which contains the shared data
  uses: edumserrano/share-jobs-data@v1.0.0
  with:
    command: read-data-current-workflow # this example works with any 'command' value
    output: github-step-json            # this example works with any 'output' value
# the step  below will log a JSON object with the key and values associated with each of the outputs
# from the previous step
- name: Dump outputs from previous step
  shell: pwsh
  env:
    STEP_OUTPUT: ${{ toJSON(steps.read-data.outputs) }}
  run: $env:STEP_OUTPUT
```

The above will show the following on the console log for the step named with id `read-data`:

- if using `output` set to `github-step-json`:

```json
{
  "name": "George Washington",
  "age": "89"
}
```

Which means you can access the values of `steps.read-data.outputs.name` and `steps.read-data.outputs.age`.

- if using `output` set to `strict-json`:

```json
{
  "data": "{\n  \"name\": \"George Washington\",\n  \"age\": \"89\"}\n}"
}
```

Which means you can access the values of `steps.read-data.outputs.data` and you will a JSON which you then [need to parse to get access to its value](#what-is-the-difference-between-using-strict-json-or-github-step-json-as-the-ouput).

<!-- ## Dev notes

For notes aimed at developers working on this repo or just trying to understand it go [here](/docs/dev-notes/README.md). It will show you how to build and run the solution among other things. -->
