using Fluxor;
using Fluxor.Deadlock.Middlewares;
using Fluxor.Deadlock.Store.Counter.Actions;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

// Add fluxor
serviceCollection.AddFluxor(options =>
{
    options.AddMiddleware<StartupMiddleware>();
    options.ScanAssemblies(typeof(Program).Assembly);
});

var serviceProvider = serviceCollection.BuildServiceProvider();

using var scope = serviceProvider.CreateScope();
var dispatcher = scope.ServiceProvider.GetRequiredService<IDispatcher>();
var store = scope.ServiceProvider.GetRequiredService<IStore>();

// simulation of the receive data from bus and dispatch to the store
StartMessageBombardment();

await store.InitializeAsync();

// simulation of the dispatch other actions..
foreach (var _ in Enumerable.Range(0, 1000))
{
    dispatcher.Dispatch(new IncrementCounterAction());
}

Console.ReadLine();

// one of the possible fixes is to use one shared resource for dispatcher and store lock statements

void StartMessageBombardment()
{
    var thread = new Thread(() =>
    {
        foreach (var _ in Enumerable.Range(0, 1000))
        {
            dispatcher.Dispatch(new IncrementCounterAction());
            Thread.Sleep(50);
        }
    });
    thread.Start();
}