﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C42G02Demo.DAL.Model
{
	public class Email
	{
        public int Id { get; set; }
		public string To { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
    }
}
