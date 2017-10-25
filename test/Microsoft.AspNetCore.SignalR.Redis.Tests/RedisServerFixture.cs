// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.SignalR.Tests.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace Microsoft.AspNetCore.SignalR.Redis.Tests
{
    public class RedisServerFixture<TStartup> : IDisposable
        where TStartup : class
    {
        public ServerFixture<TStartup> FirstServer { get; private set; }
        public ServerFixture<TStartup> SecondServer { get; private set; }

        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDisposable _logToken;

        public RedisServerFixture()
        {
            if (Docker.Default == null)
            {
                return;
            }

            var testLog = AssemblyTestLog.ForAssembly(typeof(RedisServerFixture<TStartup>).Assembly);
            _logToken = testLog.StartTestLog(null, $"{nameof(RedisServerFixture<TStartup>)}_{typeof(TStartup).Name}", out _loggerFactory, "ServerFixture");
            _logger = _loggerFactory.CreateLogger<RedisServerFixture<TStartup>>();

            FirstServer = new ServerFixture<TStartup>();
            SecondServer = new ServerFixture<TStartup>("http://localhost:3123");

            Docker.Default.Start(_logger);
        }

        public void Dispose()
        {
            if (Docker.Default != null)
            {
                FirstServer.Dispose();
                SecondServer.Dispose();
                Docker.Default.Stop(_logger);
                _logToken.Dispose();
            }
        }
    }
}