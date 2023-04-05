using System;
using UnityEngine;

public class TCPMgr : Singleton<TCPMgr>
{
    TCPSocket socket;
    ConnectingState state = ConnectingState.Waiting;

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

    private void Update()
    {
        if (socket.state != state)
        {
            state = socket.state;
            switch (state)
            {
                case ConnectingState.Connected:
                    GameMgr.Instance.HandleConnecting(true);
                    return;
                default:
                    GameMgr.Instance.HandleConnecting(false);
                    return;
            }
        }
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