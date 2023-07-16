using System;

namespace PruebaBig_WIllianCuartas.Models
{
    public class MForecast
    {
        public int? Id { get; set; } = null;
        public string? Title { get; set; }
	    public DateTime DateClima { get; set; }
	    public int? MinTemperature { get; set; } = null;
        public int? MaxTemperature { get; set; } = null;
        public int? RainProbability { get; set; } = null;
        public string? Observation {  get; set; }
        public int IdCity { get; set; }
        public string? CityName { get; set; }
	    public int? IdUserChage { get; set; } = null;

    }
}
