using Moq;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.BaseLibrary;

public abstract class BuilderBase<T> where T : class
{
    private readonly IDictionary<string, object> _mockContainer;

    protected BuilderBase() => _mockContainer = new Dictionary<string, object>();

    public T Build() => BuildInternal();

    protected abstract T BuildInternal();

    public TInput Get<TInput>() where TInput : class => GetMock<TInput>().Object;

    public Mock<TInput> GetMock<TInput>() where TInput : class
    {
        var key = typeof(TInput).FullName;
        _mockContainer.TryGetValue(key, out object dependency);

        if (dependency == null)
        {
            var mock = new Mock<TInput>();
            WithMock(mock);
            return mock;
        }

        return (Mock<TInput>)dependency;
    }

    public BuilderBase<T> WithMock<TDependency>([NotNull] Mock<TDependency> mock) where TDependency : class
    {
        var key = typeof(TDependency).FullName;

        if (!_mockContainer.ContainsKey(key))
            _mockContainer.Add(key, mock);
        return this;
    }
}
