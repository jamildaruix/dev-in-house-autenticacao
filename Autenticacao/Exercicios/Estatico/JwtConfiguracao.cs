namespace Exercicios.Estatico
{
    public static class JwtConfiguracao
    {
        public static string Secret { get; set; }
        public static string Issuer { get; set; }
        public static string Audience { get; set; }
        public static byte[] Key { get; set; }
        
    }
}
