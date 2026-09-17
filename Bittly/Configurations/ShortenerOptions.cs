namespace Bittly.Configurations
{
    public class ShortenerOptions
    {
        public string Alphabet { get; set; } = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public int ShortCodeLength { get; set; } = 6;
    }
}
