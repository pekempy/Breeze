using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace GameLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            Trace.WriteLine("Fatal Unhandled Exception:  " + args.Exception);
            args.Handled = true;
            Environment.Exit(0);
        }
    }
}