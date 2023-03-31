public class TCP
{
    public static void Register(string playerName)
    {
        var tcp = new TCPPacketEncoder();
        tcp.AddInt(1);
        tcp.AddString(playerName);

        Send(tcp);
    }

    public static void Answer(string answer)
    {
        var tcp = new TCPPacketEncoder();
        tcp.AddInt(2);
        tcp.AddString(answer);

        Send(tcp);
    }

    private static void Send(TCPPacketEncoder tcp)
    {
        TCPManager.Instance.Send(tcp.ToArray());
    }
}