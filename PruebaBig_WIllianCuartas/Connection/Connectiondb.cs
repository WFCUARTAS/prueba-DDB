namespace PruebaBig_WIllianCuartas.Connection
{
    public class Connectiondb
    {
        private string ConnectionString = string.Empty;
        public Connectiondb()
        {

            var constructor = new ConfigurationBuilder().SetBasePath
                (Directory.GetCurrentDirectory()).AddJsonFile
                ("appsettings.json").Build();

            ConnectionString = constructor.GetSection("ConnectionStrings:conexionSQL").Value;

        }
        public string cadenaSQL()
        {
            return ConnectionString;
        }
    }
}
