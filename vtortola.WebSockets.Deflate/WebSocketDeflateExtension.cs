﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vtortola.WebSockets.Deflate
{
    public sealed class WebSocketDeflateExtension:IWebSocketMessageExtension
    {
        public string Name { get { return "permessage-deflate"; } }

        static readonly WebSocketExtension _response = new WebSocketExtension("permessage-deflate", new List<WebSocketExtensionOption>(new[] { new WebSocketExtensionOption() { Name = "client_no_context_takeover" } }));
        public bool TryNegotiate(WebSocketHttpRequest request, out WebSocketExtension extensionResponse, out IWebSocketMessageExtensionContext context)
        {
            extensionResponse = _response;
            context = new WebSocketDeflateContext();
            return true;
        }
    }
}
