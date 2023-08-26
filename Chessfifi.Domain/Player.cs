namespace Chessfifi.Domain
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; } // ایمیل یا شماره تماس بازیکن
        public bool IsTurn { get; set; } // نوبت بازی بازیکن
    }

}