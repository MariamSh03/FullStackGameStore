namespace AdminPanel.Bll.DTOs;
public class PaymentRequestDto
{
    public string Method { get; set; } = string.Empty;

    public VisaModelDto? Model { get; set; }
}
