namespace Entities.DTOs.Params
{
    public class PasswordForChangeDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordAgain { get; set; }
    }
}
