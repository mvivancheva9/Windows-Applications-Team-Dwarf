﻿namespace JustQuest.Server.Web.Models
{
    using Contracts;
    using Data.Models;

    public class HintResponseModel : IMapFrom<Hint>
    {
        public string Description { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}