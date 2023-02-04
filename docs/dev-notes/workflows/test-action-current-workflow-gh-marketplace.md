# test-action-current-workflow-gh-marketplace

[![Test GH action from GH Marketplace: share data on current workflow](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-current-workflow-gh-marketplace.yml/badge.svg)](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-current-workflow-gh-marketplace.yml)

[This workflow](/.github/workflows/test-action-current-workflow-gh-marketplace.yml):

- Is a copy of the [test-action-current-workflow workflow](/.github/workflows/test-action-current-workflow.yml). See the [documentation for that workflow](/docs/dev-notes/workflows/test-action-current-workflow.md) for more info.
- The main difference from this workflow and the `test-action-current-workflow workflow` are:
  - Tests the GitHub action from the Marketplace instead of building it from this repo. It makes sure that the published version is working.
  - Does not update a PR status because this is not a workflow that should add a status checks to PRs.
