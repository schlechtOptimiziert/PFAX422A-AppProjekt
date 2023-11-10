using Blazored.Toast.Configuration;
using Blazored.Toast.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientComponents.Toast;

public sealed class ToastService
{
    private readonly IToastService blazoredToastService;

    public ToastService(IToastService blazoredToastService) => this.blazoredToastService = blazoredToastService;

    public void ShowToast(string message, ToastLevel level)
        => ShowToast(message, level, false);

    public void ShowToast(string message, ToastLevel level, bool permanent)
    {
        void CreateToastSettings(ToastSettings settings)
        {
            if (permanent)
                settings.DisableTimeout = true;
        }
        switch (level)
        {
            case ToastLevel.Info:
                blazoredToastService.ShowInfo(message, CreateToastSettings);
                break;
            case ToastLevel.Success:
                blazoredToastService.ShowSuccess(message, CreateToastSettings);
                break;
            case ToastLevel.Warning:
                blazoredToastService.ShowWarning(message, CreateToastSettings);
                break;
            case ToastLevel.Error:
                blazoredToastService.ShowError(message, CreateToastSettings);
                break;
        }
    }
}
