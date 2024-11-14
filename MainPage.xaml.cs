using Microsoft.Maui.Controls;
using Newtonsoft.Json;


namespace MauiWeatherApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnGetWeatherClicked(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(CityEntry.Text))
            {
                AlertLabel.Text = "Please enter a city.";
                return;
            }

            
            var weatherData = await GetWeatherData(CityEntry.Text);

            if (weatherData != null)
            {   
                string iconCode = weatherData.Icon;
                string iconUrl = $"https://openweathermap.org/img/wn/{iconCode}@2x.png";

                WeatherInfoLabel.Text = $"Temp: {weatherData.temp}°C\n" +
                                        $"Min: {weatherData.temp_min}°C\n" +
                                        $"Max: {weatherData.temp_max}°C\n" +
                                        $"Wind: {weatherData.WindSpeed}m/s\n" +
                                        $"Description: {weatherData.Description}";

        WeatherIcon.Source = new UriImageSource
    {
        Uri = new Uri(iconUrl),
        CachingEnabled = true, //* for faster loading
        CacheValidity = TimeSpan.FromHours(1)
    };

                
                if (weatherData.WindSpeed > 4)
                {
                    AlertLabel.Text = "Wind speed alert: More than 4 m/s!";
                }
                else
                {
                    AlertLabel.Text = "";
                }
            }
            else
            {
                WeatherInfoLabel.Text = "Weather data could not be retrieved.";
            }
        }

       
        private async Task<WeatherData> GetWeatherData(string city)
        {
            
{
    try
    {
        string apiKey = "41a8bbca562b148eb901743dad61fa52";  
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
        
        using (var client = new HttpClient())
        {
            var response = await client.GetStringAsync(url);
            var weatherResponse = JsonConvert.DeserializeObject<WeatherApiResponse>(response);

            return new WeatherData
            {
                temp = weatherResponse.Main.temp,
                temp_min = weatherResponse.Main.temp_min,
                temp_max = weatherResponse.Main.temp_max,
                Description = weatherResponse.Weather[0].Description,
                WindSpeed = weatherResponse.Wind.Speed,
                Icon = weatherResponse.Weather[0].Icon
            };
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return null;
    }
}

        }
    }

    
    public class WeatherData
    {
        public double temp { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public string Description { get; set; }
        public double WindSpeed { get; set; }
        public string Icon { get; set; }
    }

    public class WeatherApiResponse
{
    public Main Main { get; set; }
    public Weather[] Weather { get; set; }
    public Wind Wind { get; set; }
}

public class Main
{
    public double temp { get; set; }
    public double temp_min { get; set; }
    public double temp_max { get; set; }
}

public class Weather
{
    public string Description { get; set; }
     public string Icon { get; set; }
}

public class Wind
{
    public double Speed { get; set; }
}

}
