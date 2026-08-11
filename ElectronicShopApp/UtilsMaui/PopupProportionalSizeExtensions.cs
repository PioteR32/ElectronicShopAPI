using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopApp.UtilsMaui
{
    public static class PopupProportionalSizeExtensions
    {
        public static void AttachAutoResizing(this Popup popup, double heightRatio = 0.8, double widthRatio = 0.3)
        {
            void UpdateSize(Window window)
            {
                popup.HeightRequest = window.Height * heightRatio;
                popup.WidthRequest = window.Width * widthRatio;
            }

            void OnWindowSizeChanged(object? sender, EventArgs e)
            {
                if (sender is Window window)
                {
                    UpdateSize(window);
                }
            }

            popup.Opened += (sender, e) =>
            {
                var mainWindow = Application.Current?.Windows.FirstOrDefault();
                if (mainWindow != null)
                {
                    UpdateSize(mainWindow);
                    mainWindow.SizeChanged += OnWindowSizeChanged;
                }
            };

            popup.Closed += (sender, e) =>
            {
                var mainWindow = Application.Current?.Windows.FirstOrDefault();
                if (mainWindow != null)
                {
                    mainWindow.SizeChanged -= OnWindowSizeChanged;
                }
            };
        }
    }
}
