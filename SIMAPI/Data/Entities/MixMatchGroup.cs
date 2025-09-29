using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{

    public partial class MixMatchGroup
    {
        public int? MixMatchGroupId { get; set; }

        public string GroupName { get; set; }
        public short? Status { get; set; }       
        public DateTime? CreatedDate { get; set; }       

    }
}
