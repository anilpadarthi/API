namespace SIMAPI.Data.Dto
{
    public class BulkUpdateOrderRequest
    {
        public int[] orderIds { get; set; }
        public int orderStatusId { get; set; }
        public int loggedInUserId { get; set; }
    }
}
