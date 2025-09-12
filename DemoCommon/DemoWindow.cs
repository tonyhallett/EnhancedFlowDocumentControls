using System;
using System.Windows;

namespace DemoCommon
{
    public class DemoWindow : Window
    {
        public DemoWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Uri uri = new Uri(
                "/DemoCommon;component/DemoWindowResources.xaml",
                UriKind.Relative);
            ResourceDictionary loadedResourceDictionary = new ResourceDictionary()
            {
                Source = uri,
            };

            Resources.MergedDictionaries.Add(loadedResourceDictionary);
            if (!(loadedResourceDictionary[typeof(DemoWindow)] is Style style))
            {
                return;
            }

            Style = style;
        }
    }
}
