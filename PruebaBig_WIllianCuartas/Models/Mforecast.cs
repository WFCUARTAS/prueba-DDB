using System;

namespace PruebaBig_WIllianCuartas.Models
{
    public class Mforecast
    {
        public int Id { get; set; }
        public string? Title { get; set; }
	    public DateTime DateClima { get; set; }
	    public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }
        public float RainProbability { get; set; }
        public string? Observation {  get; set; }
        public int IdCity { get; set; }
        public string? CityName { get; set; }
	    public int IdUserChage { get; set; }

    }
}
