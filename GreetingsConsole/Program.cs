using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GreetingsConsole.Models;

namespace GreetingsConsole
{
    /// <summary>
    /// Main program class for the GreetingsConsole application
    /// </summary>
    class Program
    {
        /// <summary>
        /// The base URL for the MessageService API
        /// </summary>
        private const string MessageServiceUrl = "http://localhost:8080";

        /// <summary>
        /// Main entry point for the console application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Async main method
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("=== GreetingsConsole Application ===");
            Console.WriteLine($"Connecting to MessageService at: {MessageServiceUrl}");
            Console.WriteLine();

            try
            {
                // Create HttpClient to call the REST API
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(MessageServiceUrl);

                    // Call the message endpoint
                    Console.WriteLine("Calling /api/message endpoint...");
                    var response = await httpClient.GetAsync("/api/message");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonContent = await response.Content.ReadAsStringAsync();
                        var messageResponse = JsonConvert.DeserializeObject<MessageResponse>(jsonContent);

                        if (messageResponse != null)
                        {
                            Console.WriteLine();
                            Console.WriteLine("=== Response Received ===");
                            Console.WriteLine($"Message: {messageResponse.Message}");
                            Console.WriteLine($"Timestamp: {messageResponse.Timestamp:yyyy-MM-dd HH:mm:ss}");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Error: Received empty response from the service.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: Received status code {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Error: Unable to connect to MessageService.");
                Console.WriteLine($"Details: {ex.Message}");
                Console.WriteLine();
                Console.WriteLine("Please ensure the MessageService is running at " + MessageServiceUrl);
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Unexpected error: {ex.Message}");
                Environment.Exit(1);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

