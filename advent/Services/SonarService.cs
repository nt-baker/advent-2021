using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace advent
{
    public class SonarService : Sonar.SonarBase
    {
        private readonly ILogger<SonarService> _logger;
        public SonarService(ILogger<SonarService> logger)
        {
            _logger = logger;
        }

        public override Task<DepthIncreaseReply> GetDepthIncreases(DepthIncreaseRequest request, ServerCallContext context)
        {
            var prev = request.Depths.First();
            var increaseCount = 0;
            
            foreach (var depth in request.Depths)
            {
                if (depth > prev)
                {
                    increaseCount++;
                }
                prev = depth;
            }

            return Task.FromResult(new DepthIncreaseReply
            {
                Increases = increaseCount
            });
        }

        public override Task<DepthIncreaseReply> GetDepthSumIncreases(DepthIncreaseRequest request, ServerCallContext context)
        {
            var increaseCount = 0;
            var a = 2;
            var b = 3;
            var x = new List<int>();
            var y = new List<int>();

            x.Add(request.Depths[a-2]);
            x.Add(request.Depths[a-1]);
            x.Add(request.Depths[a]);

            y.Add(request.Depths[b-2]);
            y.Add(request.Depths[b-1]);
            y.Add(request.Depths[b]);

            if(y.Sum() > x.Sum())
            {
                increaseCount++;
            }

            a = b + 1;
            var c = request.Depths.Count - 1;

            while ((a <= c && b < c) 
                    || (a < c && b <= c))
            {
                if (b > a)
                {
                    y.Clear();
                    y.Add(request.Depths[b-2]);
                    y.Add(request.Depths[b-1]);
                    y.Add(request.Depths[b]);

                    if(y.Sum() > x.Sum())
                    {
                        increaseCount++;
                    }

                    a = b + 1;
                }
                else if (a > b)
                {
                    x.Clear();
                    x.Add(request.Depths[a-2]);
                    x.Add(request.Depths[a-1]);
                    x.Add(request.Depths[a]);

                    if(x.Sum() > y.Sum())
                    {
                        increaseCount++;
                    }

                    b = a + 1;
                }
            }

            return Task.FromResult(new DepthIncreaseReply
            {
                Increases = increaseCount
            });
        }
    }
}
