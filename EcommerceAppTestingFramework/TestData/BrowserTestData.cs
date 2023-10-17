using System.Collections;
using System.Collections.Generic;

public class BrowserTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Provide your test case data here
        yield return new object[] { "Chrome" };
        yield return new object[] { "Firefox" };
        yield return new object[] { "Edge" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
