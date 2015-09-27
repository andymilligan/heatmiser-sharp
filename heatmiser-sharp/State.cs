using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace heatmiser_sharp
{
    public class State : IState
    {

        public Decimal? TempActual { get; set; }
        public Decimal? TempSetting { get; set; }
        public Boolean IsHeatingOn { get; set; }
        public Boolean IsWaterOn { get; set; }

        public State(string endpointAddress = ("http://100.0.0.50/"))
        {
            string TempActualRegex = @"Actual&nbsp;:&nbsp;<\/b><font size='5'>([.0-9\-]+)";
            string TempSettingRegex = @"Set&nbsp;:&nbsp;<\/b><font size='4'>([.0-9\-]+)";
            string IsHeatingOnRegex = @"Hot Water:<\/b><font size='4'>([A-O]+)";
            string IsWaterOnRegex = @"Timer :<\/b><font size='4'>([A-O]+)";

            var client = new HttpClient();
            var page = client.GetStringAsync(endpointAddress + "right.htm").Result;

            Match match = Regex.Match(page, TempActualRegex, RegexOptions.IgnoreCase);
            TempActual = Decimal.Parse(match.Groups[1].Value);

            match = Regex.Match(page, TempSettingRegex, RegexOptions.IgnoreCase);
            TempSetting = Decimal.Parse(match.Groups[1].Value);

            match = Regex.Match(page, IsHeatingOnRegex, RegexOptions.IgnoreCase);
            IsHeatingOn = match.Groups[1].Value.Equals("ON");

            match = Regex.Match(page, IsWaterOnRegex, RegexOptions.IgnoreCase);
            IsWaterOn = match.Groups[1].Value.Equals("ON");
        }

        public void Set(int temp)
        {
            bool IsOverride = false;
            int OverrideTemp = 22;
            bool IsTempHold = true;
            int TempHoldTemp = 18;
            int TempHoldHours = 0;
            int TempHoldMinutes = 30;
            bool IsKeyLock = false;
            bool IsHotWater = true;

            var client = new HttpClient();
            // ovca=2&hdca=1&hldt=1&hdSt=22&hdhr=02&hdmi=00&kylk=0&ofbtn=0
            // ovca=2&hdca=1&hldt=1&hdSt=22&hdhr=02&hdmi=00&kylk=0&ofbtn=0
            // ovca=1&hdca=2&ovdt=1&tvrd=24                &kylk=1&ofbtn=1
            // ovca=2&hdca=1&hldt=0&hdSt=21&hdhr=1&hdmi=30&kylk=0&ofbtn=0
            // ovca=2&hdca=1&hldt=1&hdSt=21&hdhr=1&hdmi=30&kylk=0&ofbtn=0
            // ovca=2&hdca=1&hldt=1&hdSt=17&hdhr=04&hdmi=30&kylk=0&ofbtn=0
            client.BaseAddress = new Uri("http://100.0.0.50/");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("ovca", "2"),
                new KeyValuePair<string, string>("hdca", "1"),
                new KeyValuePair<string, string>("hldt", Convert.ToInt32(IsTempHold).ToString()),
                new KeyValuePair<string, string>("hdSt", TempHoldTemp.ToString()),
                new KeyValuePair<string, string>("hdhr", TempHoldHours.ToString("00")),
                new KeyValuePair<string, string>("hdmi", TempHoldMinutes.ToString("00")),
                new KeyValuePair<string, string>("kylk", Convert.ToInt32(IsKeyLock).ToString()),
                new KeyValuePair<string, string>("ofbtn", Convert.ToInt32(IsHotWater).ToString())
            });
            var result = client.PostAsync("/basicset.htm", content).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;

        }

        public void SetHotWater(bool waterState)
        {
            bool IsHotWater = waterState;

            var client = new HttpClient();
            // onff=0
            client.BaseAddress = new Uri("http://100.0.0.50/");
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


                var client = new HttpClient();

                client.BaseAddress = new Uri("http://100.0.0.50/");
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
                return;
            }

            throw new ArgumentException("Specify both or neither hours and minutes");
        }
    }
}
