using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FoWoSoft.Data.Model
{
  public  class Guid_id
    {
        /// <summary>
        /// ID
        /// </summary>
        [DisplayName("id")]
        public int id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DisplayName("GuidId")]
        public Guid GuidId { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [DisplayName("useId")]
        public string useId { get; set; }
    }
}
