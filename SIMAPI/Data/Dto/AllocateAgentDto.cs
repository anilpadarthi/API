namespace SIMAPI.Data.Dto
{
    public class AllocateAgentDto
    {
        public int managerId { get; set; }
        public DateTime? fromDate { get; set; }
        public int[] agentIds { get; set; }      
    }
}
