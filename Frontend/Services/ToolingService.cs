﻿using Docker.DotNet;
using Docker.DotNet.Models;
using Frontend.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public interface IToolingService
    {
        CommandResult Execute(Command command);
    }

    public class ToolingService : IToolingService
    {
        public DockerClient _client;
        private DockerSettings _settings;

        public ToolingService(IOptions<DockerSettings> settings)
        {
            _settings = settings.Value;
            _client = new DockerClientConfiguration(new Uri(_settings.DockerClientConfigURI))
                .CreateClient();
        }

        public CommandResult Execute(Command command)
        {
            var result = new CommandResult();

            StartContainer();

            if (command.NmapStandard)
            {
                command.Action = "nmap " + command.Ip;

                result.NmapResult = ExecuteCommandSync(command);
            }
            if (command.NiktoStandard)
            {
                command.Action = "nikto -host " + command.Ip;

                result.NiktoResult = ExecuteCommandSync(command);
            }
            if (command.XsserStandard)
            {
                command.Action = "xsser -u " + command.Hostname;

                result.XsserResult = ExecuteCommandSync(command);
            }

            StopContainer();

            return result;
        }

        private string ExecuteCommandSync(Command command)
        {
            string result; 

            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo(_settings.ShellFile, _settings.ShellArgs + "docker exec -t " + _settings.DockerContainerId + " " + command.Action);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = false;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                // Get the output into a string
                result = proc.StandardOutput.ReadToEnd();
            }
            catch (Exception objException)
            {
                // Log the exception
                result = objException.Message;
            }

            return result;
        }

        public async Task<IEnumerable<ContainerListResponse>> GetList()
        {
            IList<ContainerListResponse> containers = await _client.Containers.ListContainersAsync(
                new ContainersListParameters()
                {
                    Limit = 10,
                });

            return containers;
        }

        private async void StartContainer()
        {
            try
            {
                await _client.Containers.StartContainerAsync(
                    _settings.DockerContainerId,
                    null);
            }
            catch (Exception e)
            {

            }
        }

        private async void StopContainer()
        {
            try
            {
                var stopped = await _client.Containers.StopContainerAsync(
                    _settings.DockerContainerId,
                    new ContainerStopParameters
                    {
                        WaitBeforeKillSeconds = 30
                    },
                    CancellationToken.None);
            }
            catch (Exception e)
            {

            }
        }
    }
}
