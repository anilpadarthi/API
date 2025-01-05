namespace SIMAPI.Data.Dto
{
    public class GetPagedOrderListRequest
    {
        public string? requestType { get; set; }
        public int? pageNo { get; set; }
        public int? pageSize { get; set; }
        public int? orderStatusId { get; set; }
        public int? paymentMethodId { get; set; }
        public int? shippingModeId { get; set; }
        public int? agentId { get; set; }
        public int? managerId { get; set; }
        public int? areaId { get; set; }
        public long? shopId { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string? trackingNumber { get; set; }
        public long? orderId { get; set; }
        public short? isVat { get; set; }
        public int? loggedInUserId { get; set; }
        public int? loggedInUserRoleId { get; set; }
    }
}
