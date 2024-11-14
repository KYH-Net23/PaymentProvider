using System.ComponentModel.DataAnnotations;

namespace PaymentProvider.Models
{
    public class EmailSession
    {
        [Key]
        public int Id { get; set; }
        public string SessionId { get; set; } = null!;
        public int OrderId { get; set; }
        public bool Sent { get; set; } = false;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
