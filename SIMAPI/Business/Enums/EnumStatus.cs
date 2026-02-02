namespace SIMAPI.Business.Enums
{
    public enum EnumStatus
    {
        Active = 1,
        InActive = 0,
        Deleted = 2,
        Hold = 3

    }

    public enum EnumOrderStatus
    {
        Pending = 1,
        Shipped = 2,
        Cancelled = 3,
        Paid = 4,
        Returned = 5,
        PPA = 6,
        PPM = 7,
        PPS = 8,
        Hide = 9,
        Hold = 10,
        CCA = 13,
        CCM = 14,
        Delivered = 11,
        Defaulted = 12,
        Received = 15,
    }

    public enum EnumOrderPaymentMethod
    {
        COD = 1,
        AC = 2,
        Bonus = 3,
        Free = 4,
        SaleOrReturn = 5,
        ReturnOrDamaged = 6,
        MC = 7,
        NewShopPromo = 9,
        Cash = 12,
        BT = 13,
        BankCheque = 14

    }
}
