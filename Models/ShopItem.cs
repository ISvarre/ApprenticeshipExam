using EPiServer.Data.Dynamic;
using System;
using EPiServer.Data;

namespace ApprenticeshipExam.Models
{
    [EPiServerDataStore(AutomaticallyCreateStore = true, AutomaticallyRemapStore = true)]
    public class ShopItems
    {
        public Identity Id { get; set; }
        public string Name { get; set; }
        public bool Bought { get; set; }
    }
}