namespace SIMAPI.Data.Entities
{
    public partial class SimActivation
    {
        public int SimActivationId { get; set; }

        public int SimId { get; set; }

        public bool IsActive { get; set; }
        public bool IsSpam { get; set; }
        public string ActivationType { get; set; }

        public DateTime ActivatedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }
}
