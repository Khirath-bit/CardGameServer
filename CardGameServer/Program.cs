namespace CardGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new ConnectionHandler();
            handler.Listen();
        }
    }
}
