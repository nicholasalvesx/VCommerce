namespace VCommerce.Api.Models;

public class Customer
{
    
    public int Id { get; set; }
    
    public string? Email { get; set; }
    
    public string? Name { get; set; }

    public string? Password { get; set; }
    
    public string? ConfirmPassword { get; set; }

    public Customer(string? email, string? name, string? password, string? confirmPassword)
    {
        Email = email;
        Name = name;
        Password = password;
        ConfirmPassword = confirmPassword;
    }
}