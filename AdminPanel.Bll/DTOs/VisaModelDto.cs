namespace AdminPanel.Bll.DTOs;
public class VisaModelDto
{
    public string Holder { get; set; } = string.Empty;

    public string CardNumber { get; set; } = string.Empty;

    public int MonthExpire { get; set; }

    public int YearExpire { get; set; }

    public int Cvv2 { get; set; }
}