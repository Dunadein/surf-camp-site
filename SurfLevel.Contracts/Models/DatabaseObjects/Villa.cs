using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Villa : IPrimaryKeyObject
    {
        public Villa()
        {
            Rooms = new HashSet<Room>();
        }

        #region Columns
        [Column("v_Id")]
        public int Id { get; set; }
        [Column("v_Name")]
        public string Name { get; set; }
        [Column("v_Folder")]
        public string FolderName { get; set; }
        [Column("v_MinPax")]
        public int? MinPaxForRent { get; set; }
        [Column("v_Default")]
        public bool IsDefault { get; set; }
        [Column("v_Enabled")]
        public bool IsEnabled { get; set; }
        #endregion

        public virtual ICollection<Room> Rooms { get; set; } 
    }
}
