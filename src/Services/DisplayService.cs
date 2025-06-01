using System.Threading.Tasks;
using BSDDisplayControl.Services.Interfaces;
using System.Diagnostics;
using System;
using System.Text.Json;


namespace BSDisplayControl.Services;

public class DisplayService : IDisplayService
{
    /// <summary>
    /// Gets the display information.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing the display information.</returns>
    public async Task<(string[], string)> GetDisplayInfo()
    {

        var scriptPath = System.IO.Path.Combine(
            AppContext.BaseDirectory, "Scripts", "display_detect_parse.py");


        // Simulate fetching display information
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ddcutil",
                Arguments = "detect",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();
        string output = await process.StandardOutput.ReadToEndAsync();
        var temp = output.Clone() as string;
        string error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        //return string.IsNullOrWhiteSpace(output) ? error : output;


        var pythonProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{scriptPath}\"",
                RedirectStandardInput = true, // <-- Add this line
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        pythonProcess.Start();
        await pythonProcess.StandardInput.WriteAsync(output);
        pythonProcess.StandardInput.Close();

        string pythonOutput = await pythonProcess.StandardOutput.ReadToEndAsync();
        string pythonError = await pythonProcess.StandardError.ReadToEndAsync();
        await pythonProcess.WaitForExitAsync();


        string[] displayNames = [];
        string verboseDisplayDetails;

        if (!string.IsNullOrWhiteSpace(pythonOutput))
        {
            displayNames = JsonSerializer.Deserialize<string[]>(pythonOutput);
        }
        else
        {
            throw new InvalidOperationException("Failed to retrieve display names from the Python script.");
        }

        if (!string.IsNullOrWhiteSpace(temp))
        {
            verboseDisplayDetails = temp;
        }
        else
        {
            verboseDisplayDetails = error;
        }

        return (displayNames, verboseDisplayDetails);

    }
}


