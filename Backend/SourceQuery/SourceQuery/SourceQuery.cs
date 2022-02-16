using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.SourceQuery.Models.ServerInfo;

namespace Infrastructure.SourceQuery
{
    // https://developer.valvesoftware.com/wiki/Server_queries:ru#.D0.9F.D1.80.D0.B8.D0.BC.D0.B5.D1.80_.D0.BE.D1.82.D0.B2.D0.B5.D1.82.D0.B0
    // https://github.com/Florian2406/Okolni-Source-Ouery/

    public class SourceQuery : IDisposable
    {
        UdpClient udpClient;
        IPEndPoint iPEnd;

        public void Connect(string ip, int port, int timeoutMiliSec)
        {
            if(string.IsNullOrEmpty(ip))
            {
                throw new ArgumentNullException("ip must be non-null");
            }

            if (port < 0x0 || port > 0xFFFF)
            {
                throw new ArgumentOutOfRangeException(nameof(port), "A Valid Port has to be specified (between 0x0000 and 0xFFFF)");
            }

            if (udpClient != null || (udpClient != null && udpClient.Client.Connected))
            {
                return;
            }

            iPEnd = new IPEndPoint(IPAddress.Parse(ip), port);
            udpClient = new UdpClient(ip, port);

            //udpClient.Client.Connect(iPEnd);
            udpClient.Client.SendTimeout = timeoutMiliSec;
            udpClient.Client.ReceiveTimeout = timeoutMiliSec;
        }

        public void Disconnect()
        {
            if (udpClient != null)
            {
                //if (udpClient.Client != null && udpClient.Client.Connected)
                //{
                // udpClient.Client.Disconnect(false);
                //}

                udpClient.Dispose();
            }
        }

        public InfoResponse GetServerInfo()
        {
            Task<byte[]> thread = Task.Run(() => RecieveMessage());

            udpClient.Send(Constants.A2S_INFO_REQUEST, Constants.A2S_INFO_REQUEST.Length);

            thread.Wait();

            if(thread.Result == null)
            {
                return null;
            }

            if (thread.Exception != null)
            {
                throw thread.Exception;
            }

            ByteReader byteReader = new ByteReader(thread.Result);

            if (!byteReader.GetLong().Equals(Constants.SimpleResponseHeader))
            {
                throw new NotImplementedException("Mulitpacket Responses are not yet supported.");
            }

            byte header = byteReader.GetByte();

            if (header != 0x49)
            {
                var str = byteReader.GetBytes().Select(it => it.ToString());

                StringBuilder stringBuilder = new StringBuilder();
                foreach (var it in str)
                {
                    stringBuilder.Append(it + " ");
                }
                
                throw new ArgumentException($"The fetched Response is no A2S_INFO Response. Current: {stringBuilder.ToString()} IP - {iPEnd.Address}:{iPEnd.Port}");
            }

            InfoResponse res = new InfoResponse();

            res.Header = header;
            res.Protocol = byteReader.GetByte();
            res.Name = byteReader.GetString();
            res.Map = byteReader.GetString();
            res.Folder = byteReader.GetString();
            res.Game = byteReader.GetString();
            res.ID = byteReader.GetShort();
            res.BusySlots = byteReader.GetByte();
            res.MaxPlayers = byteReader.GetByte();
            res.Bots = byteReader.GetByte();
            res.ServerType = byteReader.GetByte().ToServerType();
            res.Environment = byteReader.GetByte().ToEnvironment();
            res.Visibility = byteReader.GetByte().ToVisibility();
            res.VAC = byteReader.GetByte() == 0x01;

            res.Port = Convert.ToInt16(iPEnd.Port);
            res.Ip = iPEnd.Address.ToString();

            //Check for TheShip
            if (res.ID == 2400)
            {
                res.Mode = byteReader.GetByte().ToTheShipMode();
                res.Witnesses = byteReader.GetByte();
                res.Duration = TimeSpan.FromSeconds(byteReader.GetByte());
            }
            res.Version = byteReader.GetString();

            //IF Has EDF Flag 
            if (byteReader.Remaining > 0)
            {
                res.EDF = byteReader.GetByte();

                if ((res.EDF & 0x80) == 1)
                {
                    res.Port = byteReader.GetShort();
                }
                if ((res.EDF & 0x10) == 1)
                {
                    res.SteamID = byteReader.GetLong();
                }
                if ((res.EDF & 0x40) == 1)
                {
                    res.SourceTvPort = byteReader.GetShort();
                    res.SourceTvName = byteReader.GetString();
                }
                if ((res.EDF & 0x20) == 1)
                {
                    res.KeyWords = byteReader.GetString();
                }
                if ((res.EDF & 0x01) == 1)
                {
                    res.GameID = byteReader.GetLong();
                }
            }

           return res;
        }

        public PlayerResponse GetPlayers()
        {
            try
            {
                Task<byte[]> thread = Task.Run(() => RecieveMessage());

                udpClient.Send(Constants.A2S_PLAYER_CHALLENGE_REQUEST, Constants.A2S_PLAYER_CHALLENGE_REQUEST.Length);

                thread.Wait();

                if (thread.Result == null)
                {
                    return null;
                }

                if (thread.Exception != null)
                {
                    throw thread.Exception;
                }

                var byteReader = new ByteReader(thread.Result);
                byte header = byteReader.GetByte();

                if (header != Constants.CHALLENGE_RESPONE)
                {
                    var str = byteReader.GetBytes().Select(it => it.ToString());

                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var it in str)
                    {
                        stringBuilder.Append(it + " ");
                    }

                    throw new ArgumentException($"The fetched Response is no A2S_PLAYER_CHALLENGE_REQUEST. Current: {stringBuilder.ToString()} IP - {iPEnd.Address}:{iPEnd.Port}");
                }

                if (!header.Equals(Constants.A2S_PLAYER_CHALLENGE_REQUEST))
                    throw new ArgumentException("Response was no player response.");

                PlayerResponse playerResponse = new PlayerResponse() { Header = header, Players = new List<Player>() };
                int playercount = byteReader.GetByte();
                for (int i = 1; i <= playercount; i++)
                {
                    playerResponse.Players.Add(new Player()
                    {
                        Index = byteReader.GetByte(),
                        Name = byteReader.GetString(),
                        Score = byteReader.GetLong(),
                        Duration = TimeSpan.FromSeconds(byteReader.GetFloat())
                    });
                }

                //IF more bytes == THE SHIP
                if (byteReader.Remaining > 0)
                {
                    playerResponse.IsTheShip = true;
                    for (int i = 0; i < playercount; i++)
                    {
                        playerResponse.Players[i].Deaths = byteReader.GetLong();
                        playerResponse.Players[i].Money = byteReader.GetLong();
                    }
                }

                return playerResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not gather Players", ex);
            }
        }

        private byte[] RecieveMessage()
        {
            byte[] bytes = null;

            try
            {
                bytes = udpClient.Receive(ref iPEnd);
            }
            catch(SocketException exception)
            {
                throw exception;
            }

            return bytes;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
