namespace Fluxor.Deadlock.Store.Counter;

public record CounterState(int Number)
{
	public static CounterState Empty => new(0);
}
