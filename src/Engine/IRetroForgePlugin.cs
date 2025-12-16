using OpenTK.Windowing.Common;

namespace RetroForge.NET;
public abstract class IRetroForgePlugin
{
    public abstract IRetroForgePlugin SetFields(Window window, string[] args);
    public Window Window;
    public string[] args;
    public Engine Engine;


    public abstract void Update(FrameEventArgs frame_args);
    public abstract void Render(FrameEventArgs frame_args);

    public abstract string View();
    public abstract void OnLoad();
    public void RegisterEngine(Engine engine)
    {
        Engine = engine;
    }
}
