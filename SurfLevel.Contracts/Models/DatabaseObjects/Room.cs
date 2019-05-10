using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Room : IPrimaryKeyObject
    {
        public Room()
        {
            Prices = new HashSet<AccommodationPrice>();
        }

        #region Columns
        [Column("r_Id")]
        public int Id { get; set; }
        [Column("r_vId")]
        public int VillaId { get; set; }
        [Column("r_Name")]
        public string Name { get; set; }
        [Column("r_Description")]
        public string DescriptionFolder { get; set; }
        #endregion

        public virtual ICollection<AccommodationPrice> Prices { get; set; }
    }
}
