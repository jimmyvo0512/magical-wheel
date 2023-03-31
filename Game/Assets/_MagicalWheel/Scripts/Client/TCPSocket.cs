using System;
using System.Net;
using System.Net.Sockets;

public class TCPSocket
{
    const int DATA_SIZE = 4096;

    TcpClient socket;
    byte[] rcvBuffer;

    public void Disconnect()
    {
        if (socket != null && socket.Connected)
        {
            socket.Close();
        }

        socket = null;
        rcvBuffer = null;
    }

    public void Connect()
    {
        try
        {
            rcvBuffer = new byte[DATA_SIZE];

            socket = new TcpClient
            {
                ReceiveBufferSize = DATA_SIZE,
                SendBufferSize = DATA_SIZE,
            };
            socket.Client.Blocking = false;
            socket.BeginConnect(IPAddress.Any, 7777, ConnectedCallback, socket);
        }
        catch (Exception err)
        {
            PanicDisconnect(err);
        }
    }

    public void Send(byte[] data)
    {
        try
        {
            BeginSend(data);
        }
        catch (SocketException err)
        {
            if (err.SocketErrorCode != SocketError.WouldBlock)
            {
                PanicDisconnect(err);
            }

            BeginSend(data);
        }
        catch (Exception err)
        {
            PanicDisconnect(err);
        }
    }

    private void BeginSend(byte[] data)
    {
        socket.Client.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);
    }

    private void ConnectedCallback(IAsyncResult asyncRes)
    {
        try
        {
            socket.EndConnect(asyncRes);
            if (!socket.Connected)
            {
                throw new Exception("Socket hasn't connected yet!");
            }

            BeginReceive();
        }
        catch (Exception err)
        {
            PanicDisconnect(err);
        }
    }

    private void BeginReceive()
    {
        socket.Client.BeginReceive(rcvBuffer, 0, DATA_SIZE, SocketFlags.None, ReceivedCallback, null);
    }

    private void ReceivedCallback(IAsyncResult asyncRes)
    {
        try
        {
            var len = socket.Client.EndReceive(asyncRes);
            if (len <= 0) { BeginReceive(); return; }

            var data = new byte[len];
            Array.Copy(rcvBuffer, data, len);

            TCPHandler.HandleData(data);

            BeginReceive();
        }
        catch (Exception err)
        {
            PanicDisconnect(err);
        }
    }

    private void PanicDisconnect(Exception err)
    {
        Disconnect();
        throw err;
    }
}