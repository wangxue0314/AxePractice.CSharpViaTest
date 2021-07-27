using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace CSharpViaTest.Collections._20_YieldPractices
{
    public class YieldDemo
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public YieldDemo(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public IEnumerable<int> GetFilterValuesByYield(IEnumerable<int> values)
        {
            foreach (int value in values)
            {
                if (value > 3)
                {
                    yield return value;
                }
            }
        }

        public IEnumerable<int> GetFilterValues(IEnumerable<int> values)
        {
            var tmp = new List<int>();
            foreach (int value in values)
            {
                if (value > 3)
                {
                    tmp.Add(value);
                }
            }

            return tmp;
        }

        [Fact]
        public void should_return_filtered_value()
        {
            var values = new List<int> {1, 2, 3, 4, 5};
            // Assert.Equal(new [] {4, 5}, GetFilterValuesByYield(values));
            // Assert.Equal(new [] {4, 5}, GetFilterValues(values));
            foreach (int value in GetFilterValuesByYield(values))
            {
                _testOutputHelper.WriteLine(value.ToString());
            }
        }
    }
}