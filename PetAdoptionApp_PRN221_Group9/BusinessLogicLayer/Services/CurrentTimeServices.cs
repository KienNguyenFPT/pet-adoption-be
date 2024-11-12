using BusinessLogicLayer.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class CurrentTimeServices : ICurrentTimeServices
    {
        public DateTime GetCurrentTime()
        {
            var time = DateTime.Now;

            return  DateTime.Parse(time.ToString("yyyy-MM-dd"));
        }
    }
}
