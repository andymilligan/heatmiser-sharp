using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace heatmiser_sharp
{
    public class State
    {

        public Decimal? TempActual { get; set; }
        public Decimal? TempSetting { get; set; }
        public Boolean IsHeatingOn { get; set; }
        public Boolean IsWaterOn { get; set; }

        public State (string endpointAddress = ("http://100.0.0.50/"))
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

    }
}
