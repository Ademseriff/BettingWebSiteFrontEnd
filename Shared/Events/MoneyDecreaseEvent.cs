﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class MoneyDecreaseEvent
    {
        public string Tc { get; set; }

        public string Money { get; set; }
    }
}
