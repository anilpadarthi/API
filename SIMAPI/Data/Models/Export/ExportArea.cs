﻿
namespace SIMAPI.Data.Models.Export
{
    public class ExportArea
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string? Status { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime? FromDate { get; set; }
    }
}
