// Copyright © 2015 Mimeo, Inc. All rights reserved.

namespace SendWithUs.Client
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISendWithUsClient
    {
        Task<ISendResponse> SendAsync(ISendRequest request);

        Task<IBatchResponse> BatchAsync(IEnumerable<IRequest> requests);
    }
}
