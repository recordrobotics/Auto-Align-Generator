using AutoAlignGenerator.nt;
using AutoAlignGenerator.ui.graphics;
using AutoAlignGenerator.ui.graphics.ui;
using Silk.NET.Maths;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowOptions = Silk.NET.Windowing.WindowOptions;

namespace AutoAlignGenerator.ui
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var options = WindowOptions.Default;
            var dpi = Window.GetSystemDpiScale();
            options.Size = new Vector2D<int>((int)(1920 * dpi.X), (int)(1080 * dpi.Y));
            options.Title = "Auto Align Generator";
            options.VSync = false;
            options.Samples = 16;
            options.IsVisible = false;
            options.WindowBorder = Silk.NET.Windowing.WindowBorder.Resizable;
            options.WindowState = Silk.NET.Windowing.WindowState.Maximized;

            Window window = new Window(options);
            window.Load += Window_Load;
            window.Run();
        }

        private static void Window_Load()
        {
            Window.Current.Internal.IsVisible = true; // only show window after loading graphics api
            Window.Current.Internal.WindowState = Silk.NET.Windowing.WindowState.Maximized;
            Scene.CreateTestScene();
        }
    }
}