# test-action-different-workflow

[![Test GH action: share data on different workflow](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-different-workflow.yml/badge.svg)](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-different-workflow.yml)

[This workflow](/.github/workflows/test-action-different-workflow.yml):

- This workflow is triggered when the [test-action-current-workflow](/.github/workflows/test-action-current-workflow.yml) completes.
- Builds and executes this action using the `read-data-different-workflow` command which downloads the artifact published by the [test-action-current-workflow](/.github/workflows/test-action-current-workflow.yml).
- Checks that the outputs from the `read-data-different-workflow` command are as expected using different `output` options.
- If the workflow that triggered it was from a Pull Request, then upon completion it will update the PR with a status check.

Since this workflow executes the [Docker container action](https://docs.github.com/en/actions/creating-actions/creating-a-docker-container-action) it will build and execute the docker container so if there are any issues with the action's [Dockerfile](/Dockerfile) this workflow will detect it.
