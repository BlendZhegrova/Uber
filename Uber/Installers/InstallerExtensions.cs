namespace Uber.Installers;

public class InstallerExtensions
{
    public static void InstallServicesInAssembly(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var installers = typeof(Program).Assembly.ExportedTypes.Where(x =>
                typeof(IInstaller).IsAssignableFrom(x) && !x.IsAbstract)
            .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();
        installers.ForEach(installer =>
        {
            installer.InstallServices(configuration,serviceCollection);
        });
    }
}