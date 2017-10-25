// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.SignalR.Tests.Common;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.SignalR.Redis.Tests
{
    public class RedisServerFixture<TStartup> : IDisposable
        where TStartup : class
    {
        public ServerFixture<TStartup> FirstServer { get; private set; }
        public ServerFixture<TStartup> SecondServer { get; private set; }

        private ILogger _logger;

        public RedisServerFixture()
        {
            if (Docker.Default == null)
            {
                return;
            }

            FirstServer = new ServerFixture<TStartup>();
            SecondServer = new ServerFixture<TStartup>("http://localhost:3123");

            _logger = FirstServer._logger;

            Docker.Default.Start(_logger);
        }

        public void Dispose()
        {
            if (Docker.Default != null)
            {
                FirstServer.Dispose();
                SecondServer.Dispose();
                Docker.Default.Stop(_logger);
            }
        }
    }
}