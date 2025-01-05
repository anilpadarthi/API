namespace SIMAPI.Data.Dto
{
    public class AllocateAreaDto
    {
        public int agentId { get; set; }
        public DateTime? fromDate { get; set; }

        public int[] areaIds { get; set; }        

    }
}
