using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using System.Runtime.Loader;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

namespace RetroForge.NET
{

	public class Engine
	{
		public static string gameName = "RetroForge";
		public static Engine Instance;
		private Window gameWindow;
		private readonly List<IRetroForgePlugin> plugins = [];
		private readonly List<RetroForgePluginData> pluginData = [];
		private IRetroForgePlugin? plugin;
		public static Dictionary<string, Font> Fonts = [];
		public static int drawcalls = 0;

        public Engine(ref Window gameWindow)
        {
			Instance = this;
            this.gameWindow = gameWindow;

            RegisterKeyDownEvent(args =>
            {
                if (args.Key != Keys.Escape) return;
                Logger.Log("Closing");
                this.gameWindow.Close();
            });
            AddSystemFonts();
        }

        public Window Window { get => gameWindow; }
		public static string BaseVertSource => File.ReadAllText(AppData("shaders", "Base.vert"));
		public static string BaseFragSource => File.ReadAllText(AppData("shaders", "Base.frag"));
        public static double Now => 1000d*TimeProvider.System.GetTimestamp()/TimeProvider.System.TimestampFrequency;
        public void OnLoad()
        {
            plugin.SetFields(Window, Environment.GetCommandLineArgs());
            plugin.OnLoad();
        }

        public void Render(FrameEventArgs frameEventArgs) => plugin.Render(frameEventArgs);
		public void Update(FrameEventArgs frameEventArgs) => plugin.Update(frameEventArgs);

		public void RegisterKeyEvent(Action<KeyboardKeyEventArgs> KeyDown, Action<KeyboardKeyEventArgs> KeyUp)
		{
			Logger.Assert(() => gameWindow is not null);
			if (gameWindow != null) gameWindow.KeyDown += KeyDown;
			if (gameWindow != null) gameWindow.KeyUp += KeyUp;
		}
		public void RegisterKeyDownEvent(Action<KeyboardKeyEventArgs> KeyDown)
		{
			if (gameWindow != null) gameWindow.KeyDown += KeyDown;
		}
		public void RegisterKeyUpEvent(Action<KeyboardKeyEventArgs> KeyUp)
		{
			if (gameWindow != null) gameWindow.KeyUp += KeyUp;
		}
		public void LoadPlugins(DirectoryInfo pluginPath)
		{
			if (!Directory.Exists(pluginPath.FullName))
			{
				throw new DirectoryNotFoundException(pluginPath.FullName);
			}
			var PluginLoader = new AssemblyLoadContext(null, true);
			var pluginAssemblies = pluginPath.EnumerateFiles("*-Game.dll");
			foreach (var assembly in pluginAssemblies)
			{
				var types = PluginLoader.LoadFromAssemblyPath(assembly.FullName).GetExportedTypes();
				plugins.AddRange(
					from type in types
					where type.IsAssignableTo(typeof(IRetroForgePlugin))
					select (IRetroForgePlugin)Activator.CreateInstance(type)!
					);
				plugins.ForEach((plugin) =>
				{
					plugin.RegisterEngine(this);
				});
			}
		}
		public void LogPlugins()
		{
			Logger.Log($"{plugins.Count} plugins loaded.");
			if (plugins.Count == 0)
				return;
			foreach (var _plugin in plugins)
			{
				Logger.Log(_plugin.View());
			}
		}
		public void LoadPluginAt(FileInfo dll)
		{
			if (!dll.Exists) throw new FileNotFoundException($"{dll.FullName} does not exits at the location specified");
			var PluginLoader = new AssemblyLoadContext(null, true);

			var types = PluginLoader.LoadFromAssemblyPath(dll.FullName).GetExportedTypes();
			plugins.AddRange(
				from type in types
				where type.IsAssignableTo(typeof(IRetroForgePlugin))
				select (IRetroForgePlugin)Activator.CreateInstance(type)!
				);

		}
        public void SetPlugin(int n) => plugin = plugins[n];
        public void AddFont(string name) => Fonts.Add(name, new Font());
        public static void AddSystemFonts() => Logger.Error("AddSystemFonts() hasn't been implemented yet");
        public static string AppData(string SubfolderPath, string? Item = null) => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), gameName, SubfolderPath, Item ?? "");
        public static string AppData() => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), gameName);
		void OnDebugMessage(DebugSource source, DebugType type, uint id, DebugSeverity severity, int length, nint pmessage, nint userParam)
		{
			//if (id == 1281) return;
			string message = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(pmessage, length);
			switch (severity)
			{
				case DebugSeverity.DontCare:
					Logger.Log(ConsoleColor.White, $"{source} {type} {id} => {message}");
					break;
				case DebugSeverity.DebugSeverityLow:
					Logger.Log($"{source} {type} {id} => {message}");
					break;
				case DebugSeverity.DebugSeverityMedium:
					Logger.Log($"{source} {type} {id} => {message}");
					break;
				case DebugSeverity.DebugSeverityHigh:
					Logger.Log($"{source} {type} {id} => {message}");
					break;
			}
			return;
		}

    }
}