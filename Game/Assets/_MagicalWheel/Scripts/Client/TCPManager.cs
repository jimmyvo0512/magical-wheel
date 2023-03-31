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
            socket.Connect();
            Debug.Log("Connected to server!");
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