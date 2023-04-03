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
        try
        {
            Debug.Log("Start connecting to server...");
            socket.Connect();
        }
        catch (Exception err)
        {
            Debug.LogError("Connect Err: " + err.Message);
        }
    }

    public void Send(byte[] data)
    {
        try
        {
            socket.Send(data);
            Debug.Log("Sent!");
        }
        catch (Exception err)
        {
            Debug.LogError("Send Err: " + err.Message);
        }
    }
}