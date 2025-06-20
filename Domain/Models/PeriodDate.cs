namespace Domain.Models
{
    public class PeriodDate
    {
        public DateOnly InitDate { get; set; }
        public DateOnly FinalDate { get; set; }

        public PeriodDate() { }

        public PeriodDate(DateOnly init, DateOnly final)
        {
            InitDate = init;
            FinalDate = final;
        }
    }
}
