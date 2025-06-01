using System;
using System.Threading.Tasks;
using BSDDisplayControl.Services.Interfaces;

namespace BSDisplayControl.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private IDisplayService _displayService;


    public About About { get; } = new About();

    public string Greeting { get; } = "Welcome to Avalonia!";

    private string[] _commandOutput = Array.Empty<string>();
    public string[] CommandOutput
    {
        get => _commandOutput;
        set => SetProperty(ref _commandOutput, value); // Assumes ViewModelBase implements SetProperty
    }

    private string _fullDisplayInfo;

    public string FullDisplayInfo
    {
        get => _fullDisplayInfo;
        set => SetProperty(ref _fullDisplayInfo, value); // Assumes ViewModelBase implements SetProperty
    }

    public MainWindowViewModel(
        IDisplayService displayService
    )
    {
        _displayService = displayService;
        LoadCommandOutputAsync();
    }

    public async void LoadCommandOutputAsync()
    {
        (string[] res1, string res2) = await _displayService.GetDisplayInfo();
        FullDisplayInfo = res2 ?? "Failed to retrieve display info.";
        CommandOutput = res1;
    }
}
