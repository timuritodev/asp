namespace ASP.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string SelectedCountry { get; set; }
        public string Gender { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
