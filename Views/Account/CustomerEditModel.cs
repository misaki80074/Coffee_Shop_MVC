namespace Coffee.Views.Account
{
    public class CustomerEditModel
    {
        public string CustomerId { get; set; } = null!;

        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public string? Password { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Name { get; set; }

        public bool? Gender { get; set; }

        public DateOnly? Birthday { get; set; }

        public string? ImgSrc { get; set; }

        public string? Language { get; set; }

        public string? ReceiverAddress { get; set; }
    }
}
