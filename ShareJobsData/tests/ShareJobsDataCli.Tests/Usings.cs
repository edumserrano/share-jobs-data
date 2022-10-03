global using System.Collections;
global using System.Collections.Concurrent;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Net.Mime;
global using System.Runtime.CompilerServices;
global using System.Text;
global using CliFx.Exceptions;
global using CliFx.Extensibility;
global using CliFx.Infrastructure;
global using DotNet.Sdk.Extensions.Testing.HttpMocking.HttpMessageHandlers;
global using DotNet.Sdk.Extensions.Testing.HttpMocking.HttpMessageHandlers.ResponseMocking;
global using ShareJobsDataCli.Common.Cli.Output;
global using ShareJobsDataCli.Common.GitHub;
global using ShareJobsDataCli.Common.Guards;
global using ShareJobsDataCli.Features.ReadDataCurrentWorkflow;
global using ShareJobsDataCli.Features.ReadDataDifferentWorkflow;
global using ShareJobsDataCli.Features.SetData;
global using ShareJobsDataCli.Tests.Auxiliary.CliApp;
global using ShareJobsDataCli.Tests.Auxiliary.Http;
global using ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient;
global using ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient.CurrentWorkflowRun;
global using ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient.DifferentWorkflowRun;
global using ShareJobsDataCli.Tests.Auxiliary.Http.ResponseContentFromFiles;
global using ShareJobsDataCli.Tests.Auxiliary.Verify;
global using Shouldly;
global using VerifyTests.Http;
