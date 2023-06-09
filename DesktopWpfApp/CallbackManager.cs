﻿using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.Security.Principal;

namespace DesktopWpfApp
{
    class CallbackManager
    {
        private readonly string _name;

        public CallbackManager(string name)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            PipeSecurity ps = new PipeSecurity();

            ps.AddAccessRule(new PipeAccessRule(WindowsIdentity.GetCurrent().Owner, PipeAccessRights.FullControl, AccessControlType.Allow));
           // ps.AddAccessRule(new PipeAccessRule(new SecurityIdentifier("S-1-5-32-544"), PipeAccessRights.ReadWrite, AccessControlType.Allow));

            System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            PipeAccessRule par = new PipeAccessRule(sid, PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow);
            ps.AddAccessRule(par);
        }

        public int ClientConnectTimeoutSeconds { get; set; } = 1;

        public async Task RunClient(string args)
        {
            using (var client = new NamedPipeClientStream(".", _name, PipeDirection.Out))
            {
                await client.ConnectAsync(ClientConnectTimeoutSeconds * 1000);

                using (var sw = new StreamWriter(client) { AutoFlush = true })
                {
                    await sw.WriteAsync(args);
                }
            }
        }

        public async Task<string> RunServer(CancellationToken? token = null)
        {
            token = CancellationToken.None;

            using (var server = new NamedPipeServerStream(_name, PipeDirection.In))
            {
                await server.WaitForConnectionAsync(token.Value);

                using (var sr = new StreamReader(server))
                {
                    var msg = await sr.ReadToEndAsync();
                    return msg;
                }
            }
        }
    }
}
