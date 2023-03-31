using System;
using UnityEngine;

public class TCPManager : Singleton<TCPManager>
{
    TCPSocket socket;

    protected override void Awake()
    {
        base.Awake();
        socket = new TCPSocket();
        Application.quitting += socket.Disconnect;
    }

    private void Start()
    {
        Connect();
    }

    public void Connect()
    {
        try { socket.Connect(); }
        catch (Exception err)
        {
            Debug.LogError(err);
        }
    }

    public void Send(byte[] data)
    {
        socket.Send(data);
    }
}