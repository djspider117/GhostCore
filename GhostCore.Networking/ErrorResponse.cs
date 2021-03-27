namespace GhostCore.Networking
{
    public class ErrorResponse
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public int ServerCode { get; set; }
    }
}
