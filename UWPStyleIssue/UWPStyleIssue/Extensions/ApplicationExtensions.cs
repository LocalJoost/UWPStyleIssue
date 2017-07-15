using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace UWPStyleIssue.Extensions
{
    public static class ApplicationExtensions
    {
        public static void FixUWPStyling(this Application app)
        {
            if (Device.RuntimePlatform == Device.Windows)
            {
                app.ConvertAllOnPlatformToExplict();
                app.ConvertAllOnDoubleToPlainDouble();
            }
        }

        public static void ConvertAllOnPlatformToExplict(this Application app)
        {
            string[] doubleProperties = { "HeightRequest", "WidthRequest", "FontSize" };
            foreach (var key in app.Resources.Keys.ToList())
            {
                var style = app.Resources[key] as Style;
                if (style != null)
                {
                    foreach (var setter in style.Setters)
                    {
                        if (doubleProperties.Contains(setter.Property.PropertyName))
                        {
                            var onPlatform = setter.Value as OnPlatform<Double>;
                            var value = onPlatform.GetPlatFormValue(Device.Windows);
                            if (value != null)
                            {
                                setter.Value = Convert.ToDouble(value.Value, CultureInfo.InvariantCulture);
                            }
                        }
                        else if (setter.Property.PropertyName == "Margin")
                        {
                            var onPlatform = setter.Value as OnPlatform<Thickness>;
                            var value = onPlatform.GetPlatFormValue(Device.Windows);
                            if (value != null)
                            {
                                var parts = value.Value.ToString().Split(',');
                                setter.Value = new Thickness(
                                    Convert.ToDouble(parts[0], CultureInfo.InvariantCulture), 
                                    Convert.ToDouble(parts[1], CultureInfo.InvariantCulture),
                                    Convert.ToDouble(parts[2], CultureInfo.InvariantCulture), 
                                    Convert.ToDouble(parts[3], CultureInfo.InvariantCulture));
                            }
                        }
                    }
                }
            }
        }

        public static void ConvertAllOnDoubleToPlainDouble(this Application app, 
            string platform = Device.Windows)
        {
            foreach (var key in app.Resources.Keys.ToList())
            {
                app.ConvertOnDoubleToPlainDouble(key, platform);
            }
        }

        public static void ConvertOnDoubleToPlainDouble(this Application app, string key, 
            string platform = Device.Windows)
        {
            var resourceDictionary = app.Resources;

            var onPlatform = GetPlatFormKey<double>(app, key, platform);
            if (onPlatform != null)
            {
                resourceDictionary.Remove(key);
                resourceDictionary.Add(key,
                    Convert.ToDouble(onPlatform.Value, CultureInfo.InvariantCulture));
            }
        }

        public static On GetPlatFormValue<T>(this OnPlatform<T> onPlatform, string platform)
        {
            return onPlatform?.Platforms.FirstOrDefault(p => p.Platform.Contains(platform));
        }

        public static On GetPlatFormKey<T>(this Application app, string key, string platform)
        {
            var resourceDictionary = app.Resources;
            if (resourceDictionary.ContainsKey(key))
            {
                var onPlatform = resourceDictionary[key] as OnPlatform<T>;
                return onPlatform?.GetPlatFormValue(platform);
            }
            return null;
        }
    }
}
