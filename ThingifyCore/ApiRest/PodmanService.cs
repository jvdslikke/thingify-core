using System;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Security;
using System.Threading.Tasks;

/// <summary>
/// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-9.0#typed-clients
/// </summary>
public class PodmanService 
{
    private readonly HttpClient _httpClient;

    public PodmanService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri("http://d/v5.0.0/");
    }

    public Task<string> ListImages()
    {
        // filter example: %7B%22label%22%3A%5B%22tech.thingify.name%22%5D%7D 
        // https://stackoverflow.com/questions/49678463/filtering-on-labels-in-docker-api-not-working-possible-bug
        return _httpClient.GetStringAsync("images/json?filters=%7B%22label%22%3A%5B%22tech.thingify.name%22%5D%7D");
    }

    /// <summary>
    /// https://andrewlock.net/using-unix-domain-sockets-with-aspnetcore-and-httpclient/
    /// </summary>
    public static SocketsHttpHandler CreateSocketsHttpHandler()
    {
        return new SocketsHttpHandler
        {
            ConnectCallback = async (ctx, ct) =>
            {
                var runtimeDirEnvironmentVariableName = "XDG_RUNTIME_DIR";
                var runtimeDir = Environment.GetEnvironmentVariable(runtimeDirEnvironmentVariableName);
                if (runtimeDir == null)
                {
                    throw new Exception($"Runtime dir not found in environment variable '{runtimeDirEnvironmentVariableName}'");
                }

                var socketPath = Path.Combine(runtimeDir, "podman", "podman.sock");
                // Define the type of socket we want, i.e. a UDS stream-oriented socket
                var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);

                // Create a UDS endpoint using the socket path
                var endpoint = new UnixDomainSocketEndPoint(socketPath);
                
                // Connect to the server!
                await socket.ConnectAsync(endpoint, ct);
                
                // Wrap the socket in a NetworkStream and return it
                // Setting ownsSocket: true means the NetworkStream will 
                // close and dispose the Socket when the stream is disposed
                return new NetworkStream(socket, ownsSocket: true);
            }
        };
    }
}