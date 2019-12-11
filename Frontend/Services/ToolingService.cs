using Docker.DotNet;
using Docker.DotNet.Models;
using Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public interface IToolingService
    {
        string Execute(Command command);
    }

    public class ToolingService : IToolingService
    {
        public DockerClient _client;

        public ToolingService()
        {
            _client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"))
                .CreateClient();
        }

        public string Execute(Command command)
        {
            StartContainer(command.Id);

            string result = ExecuteCommandSync(command);

            StopContainer(command.Id);

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
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + "docker exec -t 25c5dd16c161 nmap -V");

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
                // Display the command output.
                Console.WriteLine(result);
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

        private async void StartContainer(string id)
        {
            await _client.Containers.StartContainerAsync(
                "25c5dd16c161",
                null);
        }

        private async void StopContainer(string id)
        {
            var stopped = await _client.Containers.StopContainerAsync(
                "25c5dd16c161",
                new ContainerStopParameters
                {
                    WaitBeforeKillSeconds = 30
                },
                CancellationToken.None);
        }
    }
}
