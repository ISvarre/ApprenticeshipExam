using EPiServer.Data.Dynamic;
using System;
using EPiServer.Data;

namespace ApprenticeshipExam.Models
{
    [EPiServerDataStore(AutomaticallyCreateStore = true, AutomaticallyRemapStore = true)]
    public class LunchOptions
    {
        public Identity Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int People { get; set; }
        public bool Selected { get; set; }
    }
}