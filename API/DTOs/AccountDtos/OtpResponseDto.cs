namespace API.DTOs.AccountDtos;

public class OtpResponseDto
{
    public Guid Guid { get; set; }
    public string Email { get; set; }
    public int OTP { get; set; }
}
