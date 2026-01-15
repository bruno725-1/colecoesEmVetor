using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<FilaBenchmark>();

public class FilaBenchmark
{
    private int _index;
    private int _capacity = 6;
    [Benchmark]
    public void ModuloLoop()
    {
        for (int i = 0; i < 10_000_000; i++)
            _index = (_index + 1) % _capacity;
    }

    [Benchmark]
    public void IfWrapLoop()
    {
        for (int i = 0; i < 10_000_000; i++)
        {
            _index++;
            if (_index == _capacity)
                _index = 0;
        }
    }
}
