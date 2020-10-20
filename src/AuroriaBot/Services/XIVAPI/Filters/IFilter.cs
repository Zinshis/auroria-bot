using System;
using System.Collections.Generic;
using System.Text;

namespace AuroriaBot.Services.XIVAPI.Filters
{
    interface IFilter
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
