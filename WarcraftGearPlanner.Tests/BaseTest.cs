using Moq.AutoMock;

namespace WarcraftGearPlanner.Tests;

public abstract class BaseTest<T> where T : class
{
	protected readonly AutoMocker Mocker = new();
	protected readonly T Instance;

	public BaseTest()
	{
		Setup();

		Instance = Mocker.CreateInstance<T>();
	}

	protected virtual void Setup() { }
}