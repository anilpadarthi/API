namespace SIMAPI.Data.Models.CommissionStatement
{
    public class CommissionStatementModel
    {
        public string Network { get; set; }
        public int Conn1 { get; set; }
        public decimal Rate1 { get; set; }
        public decimal Comm1 { get; set; }

        public int Conn2 { get; set; }
        public decimal Rate2 { get; set; }
        public decimal Comm2 { get; set; }
    }
}
