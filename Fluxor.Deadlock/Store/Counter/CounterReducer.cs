using Fluxor.Deadlock.Store.Counter.Actions;

namespace Fluxor.Deadlock.Store.Counter;

public class CounterReducer : Reducer<CounterState, IncrementCounterAction>
{
    public override CounterState Reduce(CounterState state, IncrementCounterAction action)
    {
        return new CounterState(state.Number + 1);
    }
}