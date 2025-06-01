using System;
using System.Threading.Tasks;
using BSDDisplayControl.Services.Interfaces;

namespace BSDisplayControl.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private IDisplayService _displayService;



    public string Greeting { get; } = "Welcome to Avalonia!";

    private string _commandOutput;
    public string CommandOutput
    {
        get => _commandOutput;
        set => SetProperty(ref _commandOutput, value); // Assumes ViewModelBase implements SetProperty
    }

    public string FullDisplayInfo
    {
        get => _commandOutput;
        set => SetProperty(ref _fullDisplayInfo, value); // Assumes ViewModelBase implements SetProperty
    }

    private string _fullDisplayInfo;

    public MainWindowViewModel(
        IDisplayService displayService
    )
    {
        _displayService = displayService;
        LoadCommandOutputAsync();
    }

    public async void LoadCommandOutputAsync()
    {
        (string res1, string res2) = await _displayService.GetDisplayInfo();
        Console.WriteLine($"Command Output: {res1}");
        Console.WriteLine($"Full Display Info: {res2}");

        FullDisplayInfo = res2;
        CommandOutput = res1;
    }
}
