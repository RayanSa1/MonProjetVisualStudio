﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkLibrary.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Btw { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string RegDate { get; set; }
        public string AcceptDate { get; set; }
        public string LastModified { get; set; }
        public string Status { get; set; }
        public string Language { get; set; }
        public byte[] Logo { get; set; }
        public string Nacecode { get; set; }
    }
} 
