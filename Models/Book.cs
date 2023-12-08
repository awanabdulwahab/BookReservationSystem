namespace BookReservationSystem.Models{
public class Book
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string? Author { get; set; }
        
        public bool IsReserved { get; set; } = false;
        public string? ReservationComment { get; set; }
    }
}
