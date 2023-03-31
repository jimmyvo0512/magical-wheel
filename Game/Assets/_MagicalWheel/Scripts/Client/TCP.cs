public enum ClientType
{
    Register = 0,
    Answer,
}

public class TCP
{
    public static void Register(string playerName)
    {
        var tcp = new TCPEncoder();
        tcp.AddInt((int)ClientType.Register);
        tcp.AddString(playerName);

        Send(tcp);
    }

    public static void Answer(string answer)
    {
        var tcp = new TCPEncoder();
        tcp.AddInt((int)ClientType.Answer);
        tcp.AddString(answer);

        Send(tcp);
    }

    private static void Send(TCPEncoder tcp)
    {
        TCPManager.Instance.Send(tcp.ToArray());
    }
}