using Microsoft.AspNetCore.Components;
using System;

namespace MobileClient.Client.Components;

public enum Platform
{
    All,
    PC,
    Playstation,
    Xbox,
    Switch
}

public static class PlatformExtensions
{
    public static Platform? GetPlatformFromUri(NavigationManager navigationManager)
    {
        if (navigationManager.TryGetQueryString("platform", out string platformString))
            return (Platform?)Enum.Parse(typeof(Platform), platformString);
        else
            return null;
    }
}

