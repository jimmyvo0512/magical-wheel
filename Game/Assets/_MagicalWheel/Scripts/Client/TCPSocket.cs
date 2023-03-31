using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class TCPSocket
{
    const int DATA_SIZE = 4096;

    TCPHandler handler;

    TcpClient socket;
    byte[] rcvBuffer;

    public TCPSocket()
    {
        handler = new TCPHandler();
    }

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
            Debug.LogError(err);
            Disconnect();
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
            if (err.SocketErrorCode == SocketError.WouldBlock)
            {
                Debug.Log(err);
                BeginSend(data);
                return;
            }

            Debug.LogError(err);
            Disconnect();
        }
        catch (Exception err)
        {
            Debug.LogError(err);
            Disconnect();
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
            Debug.LogError(err);
            Disconnect();
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

            handler.HandleData(data);

            BeginReceive();
        }
        catch (Exception err)
        {
            Debug.LogError(err);
            Disconnect();
        }
    }
}