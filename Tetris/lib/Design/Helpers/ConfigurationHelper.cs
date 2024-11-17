using System.Configuration;

namespace Tetris.lib.Design.Helpers
{
    public static class ConfigurationHelper
    {
        public static int GetInt(string key, int defaultValue = 0)
        {
            string? stringValue = ConfigurationManager.AppSettings.Get(key);
            if (!int.TryParse(stringValue, out int value))
            {
                value = defaultValue;
            }
            return value;
        }

        public static float GetFloat(string key, float defaultValue = 0.0f)
        {
            string? stringValue = ConfigurationManager.AppSettings.Get(key);
            if (!float.TryParse(stringValue, out float value))
            {
                value = defaultValue;
            }
            return value;
        }
    }
}
