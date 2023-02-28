using Fluxor.Deadlock.Store.Counter;
using Fluxor.Deadlock.Store.Counter.Actions;

namespace Fluxor.Deadlock.Middlewares;

public class StartupMiddleware : Middleware, IDisposable
{
    private readonly IState<CounterState> counterState;

    private IDispatcher? Dispatcher;
    private IStore? Store;

    public StartupMiddleware(IState<CounterState> counterState)
    {
        this.counterState = counterState;
    }

    public override Task InitializeAsync(IDispatcher dispatcher, IStore store)
    {
        Console.WriteLine($"{nameof(StartupMiddleware)}.{nameof(InitializeAsync)} - ENTRY");

        Dispatcher = dispatcher;
        Store = store;

        store.SubscribeToAction<StoreInitializedAction>(this, OnStoreInitialized);

        Console.WriteLine($"{nameof(StartupMiddleware)}.{nameof(InitializeAsync)} - EXIT");

        return Task.CompletedTask;
    }

    private void OnStoreInitialized(StoreInitializedAction action)
    {
        Console.WriteLine($"{nameof(StartupMiddleware)}.{nameof(OnStoreInitialized)} - ENTRY");

        Thread.Sleep(500); // simulation of initialization load (if you comment Thread.Sleep Fluxor won't get deadlock)

        Dispatcher?.Dispatch(new IncrementCounterAction());

        Console.WriteLine($"{nameof(StartupMiddleware)}.{nameof(OnStoreInitialized)} - EXIT");
    }

    public override void AfterDispatch(object action)
    {
        if (action is IncrementCounterAction)
            Console.WriteLine($"Counter value: {counterState.Value.Number}");
    }

    public void Dispose()
    {
        Store?.UnsubscribeFromAllActions(this);
    }
}