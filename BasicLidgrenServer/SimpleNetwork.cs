using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicLidgrenServer
{
    public class SimpleNetwork
    {
        public static NetServer Server;
        public static NetPeerConfiguration Configuration;
        public static NetIncomingMessage IncomingMessage;
        public static NetOutgoingMessage OutgoingMessage;
        public static bool playerRefresh;

        public static void Update()
        {
            while((IncomingMessage = Server.ReadMessage()) != null)
            {
                switch(IncomingMessage.MessageType)
                {
                    case NetIncomingMessageType.Data:
                    {
                        string headStringMessage = IncomingMessage.ReadString();

                        switch (headStringMessage)
                        {
                            case "connect":
                            {
                                string name = IncomingMessage.ReadString();
                                //do some connect stuff here
                                break;
                            }

                            case "move":
                            {
                                string name = IncomingMessage.ReadString();
                                //Do move stuff here
                                break;
                            }

                            case "disconnect":
                            {
                                string name = IncomingMessage.ReadString();
                                //do some disconnect stuff
                                break;
                            }
                        }
                        break;
                    }

                }
            }
        }
    }
}
