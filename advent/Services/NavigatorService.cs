using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace advent
{
    public class NavigatorService : Navigator.NavigatorBase
    {
        private readonly ILogger<NavigatorService> _logger;
        public NavigatorService(ILogger<NavigatorService> logger)
        {
            _logger = logger;
        }

        public override Task<NavigationReply> RunAutopilot(NavigationRequest request, ServerCallContext context)
        {
            var cmds = PrepareCommands(request.Commands.ToArray());
            var hor = 0;
            var depth = 0;

            for(var i =0; i < request.Commands.Count; i++)
            {
                switch(cmds[i, 0]){
                    case 1:
                        hor += cmds[i, 1];
                        break;
                    case 2:
                        depth -= cmds[i, 1];
                        if (depth < 0) depth = 0;
                        break;
                    case 3:
                        depth += cmds[i, 1];
                        break;
                    default:
                        break;
                }
            }

            return Task.FromResult(new NavigationReply
            {
                FinalPosition = hor * depth
            });
        }

        public override Task<NavigationReply> RunAdvancedAutopilot (NavigationRequest request, ServerCallContext context)
        {
            var cmds = PrepareCommands(request.Commands.ToArray());
            var hor = 0;
            var depth = 0;
            var aim = 0;

            for(var i =0; i < request.Commands.Count; i++)
            {
                switch(cmds[i, 0]){
                    case 1:
                        hor += cmds[i, 1];
                        depth += aim * cmds[i, 1];
                        break;
                    case 2:
                        aim -= cmds[i, 1];
                        if (depth < 0) depth = 0;
                        break;
                    case 3:
                        aim += cmds[i, 1];
                        break;
                    default:
                        break;
                }
            }

            return Task.FromResult(new NavigationReply
            {
                FinalPosition = hor * depth
            });
        }

        private int[,] PrepareCommands(string[] input)
        {
            var cmds = new int[input.Count(),2];
            var row = 0;

            foreach(var cmd in input)
            {
                var pieces = cmd.Split(" ");
                cmds[row, 0] = ConvertDirectionToInt(pieces[0]);
                cmds[row, 1] = Convert.ToInt32(pieces[1]);
                row++;
            }

            return cmds;
        }

        private int ConvertDirectionToInt(string dir)
        {
            switch(dir){
                case "forward":
                    return 1;
                case "up":
                    return 2;
                case "down":
                    return 3;
                default:
                    return 0;
            }
        }
    }
}
