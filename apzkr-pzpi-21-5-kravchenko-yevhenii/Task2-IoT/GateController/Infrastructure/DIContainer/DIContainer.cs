namespace Infrastructure.DIContainer;
public static class DIContainer
{
    private static IServiceProvider? _services;

    public static IServiceProvider? Services 
    { 
        get => _services; 
        set
        {
            _services ??= value;
        } 
    }
}
