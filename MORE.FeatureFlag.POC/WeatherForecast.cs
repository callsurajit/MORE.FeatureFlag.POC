namespace MORE.FeatureFlag.POC {
    public class WeatherForecast {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
        public string RainExpected { get; set; }
        public bool AdvancedEnabled { get; set; }
    }
}