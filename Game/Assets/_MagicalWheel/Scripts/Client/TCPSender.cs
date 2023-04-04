public enum ClientType
{
    Register = 1,
    Answer,
}

public class TCPSender
{
    public static void Register(string playerName)
    {
        var encoder = new TCPEncoder();
        encoder.AddInt8((sbyte)ClientType.Register);
        encoder.AddString(playerName);

        Send(encoder);
    }

    public static void Answer(char character, string keyword)
    {
        var encoder = new TCPEncoder();
        encoder.AddChar(character);
        encoder.AddInt8((sbyte)ClientType.Answer);
        encoder.AddString(keyword);

        Send(encoder);
    }

    private static void Send(TCPEncoder encoder)
    {
        TCPManager.Instance.Send(encoder.ToArray());
    }
}