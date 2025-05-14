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
        Pendig = 1,
        Shipped = 2,
        Cancelled = 3,
        Paid = 4,
        Returned = 5,
        PPA = 6,
        PPM = 7,
        PPS = 8,
        Hide = 9,
        Hold = 10
    }

    public enum EnumOrderPaymentMethod
    {
        COD = 1,
        BT = 2,
        AC = 3,
        Bonus = 4,
        Free = 5,
        SaleOrReturn = 6,
        ReturnOrDamaged = 7,
        MC = 8

    }
}
