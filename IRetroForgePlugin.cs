


using OpenTK.Windowing.Common;

namespace RetroForge.NET;

/// <summary>
/// Defines the contract for a RetroForge plugin that can be executed with command-line arguments.
/// </summary>
/// <remarks>Implementations of this interface should provide the logic to execute the plugin's functionality when
/// the Run method is called. The interface is intended for use in plugin architectures where plugins are invoked with
/// specific arguments.</remarks>
/// <remarks>
/// Runs The Plugin Behaviour
/// </remarks>
/// <param name="args"></param>
public abstract class IRetroForgePlugin
{
    public abstract IRetroForgePlugin SetFields(Window window, string[] args);
    public Window Window;
    public string[] args;

    public abstract void Update(FrameEventArgs frame_args);
    public abstract void Render(FrameEventArgs frame_args);

    public abstract string View();
}
