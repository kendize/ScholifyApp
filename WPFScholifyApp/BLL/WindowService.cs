using System.Windows;
using System;
using Microsoft.Extensions.DependencyInjection;

public interface IWindowService
{
    public void Show<TWindow>(Action<TWindow>? configure = null) where TWindow : Window;
}

public class WindowService : IWindowService
{
    private readonly IServiceProvider _services;

    public WindowService(IServiceProvider services)
    {
        _services = services;
    }

    public void Show<TWindow>(Action<TWindow>? configure = null) where TWindow : Window
    {
        var window = _services.GetService<TWindow>();
        if (configure != null)
        {
            configure(window!);
        }

        window.Show();
    }
}
