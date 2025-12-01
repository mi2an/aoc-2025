using System.Reflection;
using Common;
using Microsoft.Extensions.DependencyModel;

namespace Runner;

class Program
{
    static void Main(string[] args)
    {
        var (minDay, maxDay) = GetDaysFromArgs(args);

        IEnumerable<Assembly> assemblies = LoadAssemblies();

        List<Solver> solvers = LoadInstancesOf<Solver>(assemblies)
            .Where(solver => solver.Day >= minDay && solver.Day <= maxDay)
            .ToList()!;


        foreach (var solver in solvers.OrderBy(s => s.Day))
        {
            var inputFile = $"inputs/{solver.Day:00}.txt";
            solver.Run(inputFile);
            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Done");
        }
    }

    private static IEnumerable<T> LoadInstancesOf<T>(IEnumerable<Assembly> assemblies)
    {
        return assemblies.SelectMany(a =>
        {
            try
            {
                return a.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t is not null)!;
            }
        })
        .Where(t => t is not null && t.IsClass && !t.IsAbstract && typeof(T).IsAssignableFrom(t))
        .Select(t => (T?)Activator.CreateInstance(t!))
        .Where(res => res is not null)!;
    }

    private static (int MinDay, int MaxDay) GetDaysFromArgs(string[] args)
    {
        int minDay = DateTime.Today.Day;
        int maxDay = DateTime.Today.Day;
        if (args.Length == 1 && int.TryParse(args[0], out var parsedDay))
        {
            minDay = parsedDay;
            maxDay = parsedDay;
        }
        if (args.Length == 2 && int.TryParse(args[0], out var parsedMinDay) && int.TryParse(args[1], out var parsedMaxDay))
        {
            minDay = parsedMinDay;
            maxDay = parsedMaxDay;
        }

        return (minDay, maxDay);
    }

    static IEnumerable<Assembly> LoadAssemblies()
    {
        return DependencyContext.Default?.RuntimeLibraries.Select(l =>
        {
            try
            {
                return Assembly.Load(new AssemblyName(l.Name));
            }
            catch
            {
                return null;
            }
        }).Where(a => a is not null)!.AsEnumerable<Assembly>() ?? [];
    }
}