using System.Collections.Generic;

namespace SurfLevel.Domain.Options
{
    public class SystemEmails
    {
        public string SystemEmail { get; set; }

        public IEnumerable<string> AdminEmails { get; set; }
    }
}
