﻿using SurfLevel.Contracts.Models.DTO;
using System.Collections.Generic;

namespace SurfLevel.Domain.ViewModels.Search
{
    public class SearchByDefaultResult
    {
        public List<PaxPrice> Prices { get; set; }

        public List<ViewVilla> Villas { get; set; }
    }
}
