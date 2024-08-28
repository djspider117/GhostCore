namespace GhostCore.Security.Cryptography.Hash.Abstract
{
    public interface IHashStrategy
    {
        byte[] Hash(byte[] input);
        byte[] Hash(string input);
        string HashToString(byte[] input, bool safeString = false);
        string HashToString(string input, bool safeString = false);
    }
}
