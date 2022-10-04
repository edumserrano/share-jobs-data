# test-action-current-workflow

[![Test GH action: share data on current workflow](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-current-workflow.yml/badge.svg)](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-current-workflow.yml)

[This workflow](/.github/workflows/test-action-current-workflow.yml):

- Builds and executes this action using the `set-data` command which uploads an artifact with shared data to the workflow.
- Checks that the outputs from the `set-data` command are as expected using different `output` options.
- Builds and executes this action using the `read-data-current-workflow` command which downloads the artifact published by the `set-data` command.
- Checks that the outputs from the `read-data-current-workflow` command are as expected using different `output` options.
- Checks that action should fail the workflow if the action fails.

Since this workflow executes the [Docker container action](https://docs.github.com/en/actions/creating-actions/creating-a-docker-container-action) it will build and execute the docker container so if there are any issues with the action's [Dockerfile](/Dockerfile) this workflow will detect it.
