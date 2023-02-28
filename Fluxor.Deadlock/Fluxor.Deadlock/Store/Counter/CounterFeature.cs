namespace Fluxor.Deadlock.Store.Counter;

public class CounterFeature : Feature<CounterState>
{
	public override string GetName() => nameof(CounterState);
	protected override CounterState GetInitialState() => CounterState.Empty;
}
