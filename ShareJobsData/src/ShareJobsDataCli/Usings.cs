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
global using ShareJobsDataCli.Common.Cli;
global using ShareJobsDataCli.Common.Cli.Errors;
global using ShareJobsDataCli.Common.Cli.Output;
global using ShareJobsDataCli.Common.Cli.Validators;
global using ShareJobsDataCli.Common.GitHub;
global using ShareJobsDataCli.Common.GitHub.Types;
global using ShareJobsDataCli.Common.Guards;
global using ShareJobsDataCli.Common.Http;
global using ShareJobsDataCli.Common.JobsData;
global using ShareJobsDataCli.Common.OneOfExtensions;
global using ShareJobsDataCli.Features.ReadDataCurrentWorkflow.DownloadArtifact;
global using ShareJobsDataCli.Features.ReadDataCurrentWorkflow.DownloadArtifact.HttpModels;
global using ShareJobsDataCli.Features.ReadDataCurrentWorkflow.DownloadArtifact.Results;
global using ShareJobsDataCli.Features.ReadDataCurrentWorkflow.Errors;
global using ShareJobsDataCli.Features.ReadDataCurrentWorkflow.Outputs;
global using ShareJobsDataCli.Features.ReadDataDifferentWorkflow.DownloadArtifact;
global using ShareJobsDataCli.Features.ReadDataDifferentWorkflow.DownloadArtifact.HttpModels;
global using ShareJobsDataCli.Features.ReadDataDifferentWorkflow.DownloadArtifact.Results;
global using ShareJobsDataCli.Features.ReadDataDifferentWorkflow.Errors;
global using ShareJobsDataCli.Features.ReadDataDifferentWorkflow.Outputs;
global using ShareJobsDataCli.Features.ReadDataDifferentWorkflow.Types;
global using ShareJobsDataCli.Features.SetData.Errors;
global using ShareJobsDataCli.Features.SetData.Outputs;
global using ShareJobsDataCli.Features.SetData.Types;
global using ShareJobsDataCli.Features.SetData.UploadArtifact;
global using ShareJobsDataCli.Features.SetData.UploadArtifact.HttpModels;
global using ShareJobsDataCli.Features.SetData.UploadArtifact.Results;
global using YamlDotNet.Core;
global using YamlDotNet.Serialization;
