using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace heatmiser_sharp
{
    public class State : IState
    {
        public Decimal TempActual { get; set; }
        public Decimal TempSetting { get; set; }
        public bool IsHeatingOn { get; set; }
        public bool IsWaterOn { get; set; }
        public Uri BaseEndpointAddress { get; set; }

        private HttpClient client = new HttpClient();

        public State()
        {
            BaseEndpointAddress = new Uri("http://100.0.0.50/");
        }

        public State(string baseEndpointAddress)
        {
            BaseEndpointAddress = new Uri(baseEndpointAddress);
        }
        
        public void ReadState()
        {
            string TempActualRegex = @"Actual&nbsp;:&nbsp;<\/b><font size='5'>([.0-9\-]+)";
            string TempSettingRegex = @"Set&nbsp;:&nbsp;<\/b><font size='4'>([.0-9\-]+)";
            string IsHeatingOnRegex = @"Hot Water:<\/b><font size='4'>([A-O]+)";
            string IsWaterOnRegex = @"Timer :<\/b><font size='4'>([A-O]+)";
            
            var page = client.GetStringAsync(BaseEndpointAddress + "right.htm").Result;

            Match match = Regex.Match(page, TempActualRegex, RegexOptions.IgnoreCase);
            TempActual = decimal.Parse(match.Groups[1].Value);

            match = Regex.Match(page, TempSettingRegex, RegexOptions.IgnoreCase);
            TempSetting = decimal.Parse(match.Groups[1].Value);

            match = Regex.Match(page, IsHeatingOnRegex, RegexOptions.IgnoreCase);
            IsHeatingOn = match.Groups[1].Value.Equals("ON");

            match = Regex.Match(page, IsWaterOnRegex, RegexOptions.IgnoreCase);
            IsWaterOn = match.Groups[1].Value.Equals("ON");
        }

        
        public void SetWater(bool waterOn)
        {
            bool IsHotWater = waterOn;

            // onff=0
            client.BaseAddress = BaseEndpointAddress;
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("onff", Convert.ToInt32(IsHotWater).ToString())
            });
            var result = client.PostAsync("/basicset.htm", content).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;
        }

        

        public void SetHeating(int temp, int? hours = default(int?), int? minutes = default(int?))
        {
            if ((hours == null) && (minutes == null))
            {
                // Setting just the heating 
                // ovdt=1&tvrd=21

                bool IsOverride = true;
                int OverrideTemp = temp;

                client.BaseAddress = BaseEndpointAddress;
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("ovdt", Convert.ToInt32(IsOverride).ToString()),
                    new KeyValuePair<string, string>("tvrd", OverrideTemp.ToString())
                });
                var result = client.PostAsync("/basicset.htm", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;

                return;
            }

            if (hours != null && minutes != null)
            {
                // ovca=2&hdca=1&hldt=1&hdSt=15&hdhr=03&hdmi=13

                int TempHoldTemp = temp;
                int TempHoldHours = hours.Value;
                int TempHoldMinutes = minutes.Value;
                
                client.BaseAddress = BaseEndpointAddress;
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("ovca", "2"),
                    new KeyValuePair<string, string>("hdca", "1"),
                    new KeyValuePair<string, string>("hldt", "1"),
                    new KeyValuePair<string, string>("hdSt", TempHoldTemp.ToString()),
                    new KeyValuePair<string, string>("hdhr", TempHoldHours.ToString("00")),
                    new KeyValuePair<string, string>("hdmi", TempHoldMinutes.ToString("00"))
                });
                var result = client.PostAsync("/basicset.htm", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;

                return;
            }

            throw new ArgumentException("Specify both or neither hours and minutes");
        }
    }
}
