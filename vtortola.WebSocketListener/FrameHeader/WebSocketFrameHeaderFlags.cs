﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vtortola.WebSockets
{
    public sealed class WebSocketFrameHeaderFlags
    {
        readonly Boolean[] _byte1, _byte2;

        public Boolean FIN { get { return _byte1[7]; } private set { _byte1[7] = value; } }
        public Boolean RSV1 { get { return _byte1[6]; } private set { _byte1[6] = value; } }
        public Boolean RSV2 { get { return _byte1[5]; } private set { _byte1[5] = value; } }
        public Boolean RSV3 { get { return _byte1[4]; } private set { _byte1[4] = value; } }
        public Boolean OPT4 { get { return _byte1[3]; } private set { _byte1[3] = value; } }
        public Boolean OPT3 { get { return _byte1[2]; } private set { _byte1[2] = value; } }
        public Boolean OPT2 { get { return _byte1[1]; } private set { _byte1[1] = value; } }
        public Boolean OPT1 { get { return _byte1[0]; } private set { _byte1[0] = value; } }
        public Boolean MASK { get { return _byte2[7]; } private set { _byte2[7] = value; } }
        public WebSocketFrameOption Option { get; private set; }

        public WebSocketFrameHeaderFlags(Byte[] header, Int32 start)
        {
            if (header == null || header.Length - start < 2)
                throw new ArgumentException();
            _byte1 = new Boolean[8];
            _byte2 = new Boolean[8];

            Byte[] byte1 = new Byte[] { header[0] };
            Byte[] byte2 = new Byte[] { header[1] };

            BitArray bitArray = new BitArray(byte1);
            bitArray.CopyTo(_byte1, 0);

            bitArray = new BitArray(byte2);
            bitArray.CopyTo(_byte2, 0);

            Int32 value = header[0];
            value = value > 128 ? value - 128 : value;

            if (!Enum.IsDefined(typeof(WebSocketFrameOption), value))
                throw new WebSocketException("Cannot parse header [option], value was: " + value);

            Option = (WebSocketFrameOption)value;
        }

        public WebSocketFrameHeaderFlags(bool isComplete, WebSocketFrameOption option)
        {
            this.Option = option;
            _byte1 = new Boolean[8];
            _byte2 = new Boolean[8];

            _byte1[7] = isComplete;
            switch (option)
            {
                case WebSocketFrameOption.Text:
                    this.OPT1 = true;
                    break;
                case WebSocketFrameOption.Binary:
                    this.OPT2 = true;
                    break;
                case WebSocketFrameOption.ConnectionClose:
                    this.OPT4 = true;
                    break;
                case WebSocketFrameOption.Ping:
                    this.OPT1 = this.OPT4 = true;
                    break;
                case WebSocketFrameOption.Pong:
                    this.OPT4 = this.OPT2 = true;
                    break;
            }
        }
    }
}