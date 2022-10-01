global using System.Diagnostics.CodeAnalysis;
global using System.Globalization;
global using System.IO.Compression;
global using System.Net.Http.Headers;
global using System.Net.Http.Json;
global using System.Net.Mime;
global using System.Runtime.CompilerServices;
global using System.Text;
global using System.Text.Json.Serialization;
global using CliFx;
global using CliFx.Attributes;
global using CliFx.Extensibility;
global using CliFx.Infrastructure;
global using FluentValidation;
global using FluentValidation.Results;
global using Flurl;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Linq;
global using OneOf;
global using ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.DownloadArtifact;
global using ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.DownloadArtifact.HttpModels;
global using ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.DownloadArtifact.Results;
global using ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.Errors;
global using ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.Outputs;
global using ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.DownloadArtifact;
global using ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.DownloadArtifact.HttpModels;
global using ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.DownloadArtifact.Results;
global using ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.Errors;
global using ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.Outputs;
global using ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.Types;
global using ShareJobsDataCli.CliCommands.Commands.SetData.Errors;
global using ShareJobsDataCli.CliCommands.Commands.SetData.Outputs;
global using ShareJobsDataCli.CliCommands.Commands.SetData.Types;
global using ShareJobsDataCli.CliCommands.Commands.SetData.UploadArtifact;
global using ShareJobsDataCli.CliCommands.Commands.SetData.UploadArtifact.HttpModels;
global using ShareJobsDataCli.CliCommands.Commands.SetData.UploadArtifact.Results;
global using ShareJobsDataCli.CliCommands.Errors;
global using ShareJobsDataCli.CliCommands.Output;
global using ShareJobsDataCli.CliCommands.Validators;
global using ShareJobsDataCli.GitHub;
global using ShareJobsDataCli.GitHub.Types;
global using ShareJobsDataCli.Guards;
global using ShareJobsDataCli.Http;
global using ShareJobsDataCli.JobsData;
global using ShareJobsDataCli.OneOfExtensions;
global using YamlDotNet.Core;
global using YamlDotNet.Serialization;
