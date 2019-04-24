using System.Collections.Generic;

namespace SurfLevel.Domain.ViewModels.Search
{
    public class ViewVilla
    {
        public ViewVilla()
        {
            Rooms = new List<ViewRoom>();
        }

        public List<ViewRoom> Rooms { get; set; }

        public string Locale { get; set; }

        public string FolderName { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public int? MinPax { get; set; }
    }
}
