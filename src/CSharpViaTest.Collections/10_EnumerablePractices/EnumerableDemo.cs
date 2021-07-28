using System.Collections;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace CSharpViaTest.Collections._10_EnumerablePractices
{
    public class EnumerableDemo
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public EnumerableDemo(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        class EvenNumberEnumerable : IEnumerable
        {
            readonly ICollection<int> _numbers;

            public EvenNumberEnumerable(ICollection<int> numbers)
            {
                _numbers = numbers;
            }

            public IEnumerator GetEnumerator()
            {
                return new EvenNumberEnumerator(_numbers);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        class EvenNumberEnumerator : IEnumerator
        {
            private IEnumerator<int> Enumerator {get; set;}

            public EvenNumberEnumerator(ICollection<int> numbers)
            {
                Enumerator = numbers.GetEnumerator();
            }

            public bool MoveNext()
            {
                while(Enumerator.MoveNext())
                {
                    if ((int)Current % 2 == 0)
                    {
                        return true;
                    }
                }
                return false;
            }

            public void Reset()
            {
                Enumerator.Reset();
                // throw new System.NotImplementedException();
            }

            public object Current => Enumerator.Current;


            public void Dispose()
            {
                Enumerator.Dispose();
                // throw new System.NotImplementedException();
            }
        }

        [Fact]
        public void should_visit_the_even_number_elements()
        {
            int[] sequence = {1, 2, 3, 4, 5, 6};
            var evenNumberEnumerable = new EvenNumberEnumerable(sequence);

            int i = 0;
            int[] expectedResults = {2, 4, 6};
            foreach (int number in evenNumberEnumerable)
            {
                _testOutputHelper.WriteLine(number.ToString());
                Assert.Equal(expectedResults[i++], number);
            }
        }
    }
}