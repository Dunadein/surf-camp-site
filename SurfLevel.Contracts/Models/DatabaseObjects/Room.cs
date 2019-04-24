using System.Collections.Generic;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Room
    {
        public Room()
        {
            Prices = new HashSet<AccommodationPrice>();
        }

        #region Columns
        public int Id { get; set; }
        public int VillaId { get; set; }
        public string Name { get; set; }
        public string DescriptionFolder { get; set; }
        #endregion

        public virtual ICollection<AccommodationPrice> Prices { get; set; }
    }
}
