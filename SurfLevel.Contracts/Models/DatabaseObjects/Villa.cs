using System.Collections.Generic;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Villa
    {
        public Villa()
        {
            Rooms = new HashSet<Room>();
        }

        #region Columns
        public int Id { get; set; }
        public string Name { get; set; }
        public string FolderName { get; set; }
        public int? MinPaxForRent { get; set; }
        public bool IsDefault { get; set; }
        public bool IsEnabled { get; set; }
        #endregion

        public virtual ICollection<Room> Rooms { get; set; } 
    }
}
