using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GameLauncher.Models
{
    public static class KeyboardFocusAttractor
    {
        public static readonly DependencyProperty IsAttracted = DependencyProperty.RegisterAttached("IsAttracted",
            typeof(bool), typeof(KeyboardFocusAttractor), new PropertyMetadata(false, OnIsAttracted));


        private static void OnIsAttracted(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var isAttracted = (bool)e.NewValue;
            var controlWithInputFocus = d as Control;
            if (controlWithInputFocus != null)
            {
                if (isAttracted)
                {
                    new KeyboardInputFocusEventManager(controlWithInputFocus);
                }
            }
        }

        public static void SetIsAttracted(DependencyObject dp, bool value)
        {
            dp.SetValue(IsAttracted, value);
        }

        public static bool GetIsAttracted(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsAttracted);
        }

        private class KeyboardInputFocusEventManager
        {
            private readonly Control _control;
            private Window _window;

            public KeyboardInputFocusEventManager(Control control)
            {
                _control = control;
                _control.Loaded += ControlLoaded;
                _control.IsVisibleChanged += ControlIsVisibleChanged;
                _control.Unloaded += ControlUnloaded;
            }

            private void ControlLoaded(object sender, RoutedEventArgs e)
            {
                _window = Window.GetWindow(_control);
                if (_window != null)
                {
                    _control.Unloaded += ControlUnloaded;
                    _control.IsVisibleChanged += ControlIsVisibleChanged;
                    if (_control.IsVisible)
                    {
                        _window.PreviewKeyDown += ParentWindowPreviewKeyDown;
                    }
                }
            }

            private void ControlUnloaded(object sender, RoutedEventArgs e)
            {
                _control.Unloaded -= ControlUnloaded;
                _control.IsVisibleChanged -= ControlIsVisibleChanged;
            }

            private void ControlIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
            {
                if (_window != null)
                {
                    _window.PreviewKeyDown -= ParentWindowPreviewKeyDown;
                }

                if (_control.IsVisible)
                {
                    _window = Window.GetWindow(_control);
                    if (_window != null)
                    {
                        _window.PreviewKeyDown += ParentWindowPreviewKeyDown;
                    }
                }
            }

            private void ParentWindowPreviewKeyDown(object sender, KeyEventArgs e)
            {
                if (((MainWindow)Application.Current.MainWindow).isDialogOpen == false)
                    Keyboard.Focus(_control);
            }
        }
    }
}
