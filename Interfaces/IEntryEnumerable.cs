using System.Collections.Generic;

namespace Joyride.Interfaces
{
    /// <summary>
    /// Allows screens to iterate through entries within a collection
    /// </summary>
    public interface IEntryEnumerable : IEnumerable<IDictionary<string, object>>
    {
    }
}
