namespace PruebaBig_WIllianCuartas.Models
{
    public class MUser
    {
        public int Id { get; set; }
        public string? Email { get; set; } = null;
        public string? Password { get; set; }= null;
        public string? FullName { get; set; }
        public string? UserRol { get; set; }
        public string? token { get; set; }
    }
}
