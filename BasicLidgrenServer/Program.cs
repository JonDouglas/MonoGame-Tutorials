using System;
using System.Threading;
using Lidgren.Network;

namespace BasicLidgrenServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SimpleNetwork.Configuration = new NetPeerConfiguration("BasicLidgrenServer");
            SimpleNetwork.Configuration.Port = 14242; //Port from lidgren tutorial
            SimpleNetwork.Server = new NetServer(SimpleNetwork.Configuration);
            SimpleNetwork.Server.Start();

            double nextSendUpdates = NetTime.Now;

            Console.WriteLine("Server has started" + "\r\n");
            Console.WriteLine("Waiting for connections..." + "\r\n");

            while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
            {
                NetIncomingMessage message;
                while ((message = SimpleNetwork.Server.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryRequest:
                        {
                            SimpleNetwork.Server.SendDiscoveryResponse(null, message.SenderEndPoint);
                            break;
                        }
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        {
                            Console.WriteLine(message.ReadString());
                            break;
                        }
                        case NetIncomingMessageType.StatusChanged:
                        {
                            var status = (NetConnectionStatus) message.ReadByte();
                            if (status == NetConnectionStatus.Connected)
                            {
                                Console.WriteLine(
                                    NetUtility.ToHexString(message.SenderConnection.RemoteUniqueIdentifier) +
                                    " connected!");

                                message.SenderConnection.Tag = new[]
                                {
                                    NetRandom.Instance.Next(10, 100),
                                    NetRandom.Instance.Next(10, 100)
                                };
                            }
                            break;
                        }
                        case NetIncomingMessageType.Data:
                        {
                            int xinput = message.ReadInt32();
                            int yinput = message.ReadInt32();

                            var pos = message.SenderConnection.Tag as int[];

                            // fancy movement logic goes here; we just append input to position
                            pos[0] += xinput;
                            pos[1] += yinput;
                            break;
                        }
                    }

                    double now = NetTime.Now;
                    if (now > nextSendUpdates)
                    {
                        // Yes, it's time to send position updates

                        // for each player...
                        foreach (NetConnection player in SimpleNetwork.Server.Connections)
                        {
                            // ... send information about every other player (actually including self)
                            foreach (NetConnection otherPlayer in SimpleNetwork.Server.Connections)
                            {
                                // send position update about 'otherPlayer' to 'player'
                                NetOutgoingMessage om = SimpleNetwork.Server.CreateMessage();

                                // write who this position is for
                                om.Write(otherPlayer.RemoteUniqueIdentifier);

                                if (otherPlayer.Tag == null)
                                    otherPlayer.Tag = new int[2];

                                var pos = otherPlayer.Tag as int[];
                                om.Write(pos[0]);
                                om.Write(pos[1]);

                                // send message
                                SimpleNetwork.Server.SendMessage(om, player, NetDeliveryMethod.Unreliable);
                            }
                        }

                        // schedule next update
                        nextSendUpdates += (1.0/30.0);
                    }
                }

                // sleep to allow other processes to run smoothly
                Thread.Sleep(1);
            }

            SimpleNetwork.Server.Shutdown("app exiting");
        }
    }
}