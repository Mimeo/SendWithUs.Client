//

namespace SendWithUs.Client
{
    using System.Collections.Generic;

    public interface IBatchResponse : IResponse
    {
        IEnumerable<IResponse> Items { get; }
    }
}
