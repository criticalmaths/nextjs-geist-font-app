using System;
using Microsoft.Win32;

namespace BlackBoxAI.VSExtension.Services
{
    public class SettingsService
    {
        private const string RegistryPath = @"Software\BlackBoxAI\VSExtension";
        private const string ApiKeyValueName = "ApiKey";

        public string GetApiKey()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath))
                {
                    return key?.GetValue(ApiKeyValueName)?.ToString() ?? string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public void SetApiKey(string apiKey)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryPath))
                {
                    key?.SetValue(ApiKeyValueName, apiKey);
                }
            }
            catch
            {
                // Handle registry write errors silently
            }
        }
    }
}
