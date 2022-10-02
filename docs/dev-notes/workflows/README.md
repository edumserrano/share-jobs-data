# GitHub worflows

There are two workflows setup on this repo:

<!-- the &nbsp; is a trick to expand the width of the table column. You add as many &nbsp; as required to get the width you want. -->
| Worflow &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; | Status and link                                                                                                                                                                                                                                                                                   | Description                                                                    |
| --------------------------------------------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | :----------------------------------------------------------------------------- |
| [build-and-test](/.github/workflows/build-test.yml)                                                       | [![Build and test](https://github.com/edumserrano/markdown-link-check-log-parser/actions/workflows/build-test.yml/badge.svg)](https://github.com/edumserrano/markdown-link-check-log-parser/actions/workflows/build-test.yml)                                                                     | Builds the solution and runs tests                                             |
| [test-action](/.github/workflows/test-action.yml)                                                         | [![Test GitHub action](https://github.com/edumserrano/markdown-link-check-log-parser/actions/workflows/test-action.yml/badge.svg)](https://github.com/edumserrano/markdown-link-check-log-parser/actions/workflows/test-action.yml)                                                               | Builds and tests the GitHub action                                             |
| [markdown-link-check-with-errors](/.github/workflows/markdown-link-check-with-errors.yml)                 | [![Markdown Link Check with errors](https://github.com/edumserrano/markdown-link-check-log-parser/actions/workflows/markdown-link-check-with-errors.yml/badge.svg)](https://github.com/edumserrano/markdown-link-check-log-parser/actions/workflows/markdown-link-check-with-errors.yml)          | Used to trigger the GitHub action for a Markdown Link Check log with errors    |
| [markdown-link-check-without-errors](/.github/workflows/markdown-link-check-without-errors.yml)           | [![Markdown Link Check without errors](https://github.com/edumserrano/markdown-link-check-log-parser/actions/workflows/markdown-link-check-without-errors.yml/badge.svg)](https://github.com/edumserrano/markdown-link-check-log-parser/actions/workflows/markdown-link-check-without-errors.yml) | Used to trigger the GitHub action for a Markdown Link Check log without errors |

## Workflows' documentation

- [build-and-test](/docs/dev-notes/workflows/build-and-test-workflow.md)
- [test-action](/docs/dev-notes/workflows/test-action-workflow.md)
- [markdown-link-check-with-errors](/docs/dev-notes/workflows/markdown-link-check-with-errors-workflow.md)
- [markdown-link-check-without-errors](/docs/dev-notes/workflows/markdown-link-check-without-errors-workflow.md)
