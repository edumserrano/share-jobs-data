# test-action-different-workflow-gh-marketplace

[![Test GH action from GH Marketplace: share data on different workflow](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-different-workflow-gh-marketplace.yml/badge.svg)](https://github.com/edumserrano/share-jobs-data/actions/workflows/test-action-different-workflow-gh-marketplace.yml)

[This workflow](/.github/workflows/test-action-different-workflow-gh-marketplace.yml):

- Is a copy of the [test-action-different-workflow workflow](/.github/workflows/test-action-different-workflow.yml). See the [documentation for that workflow](/docs/dev-notes/workflows/test-action-different-workflow.md) for more info.
- The main difference from this workflow and the `test-action-different-workflow workflow` are:
  - Tests the GitHub action from the Marketplace instead of building it from this repo. It makes sure that the published version is working.
  - Does not update a PR status because this is not a workflow that should add a status checks to PRs.
