﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APISTest.Models;

namespace APISTest.ViewModels
{
    public class CustomerViewModel
    {
        public Customer  customer {get;set;}
        public string CustomerTeamCode { get; set; }
    }
}