using System.ComponentModel.DataAnnotations;

namespace Online_Shop.ViewModels
{
    public class RegisterViewModels
    {
        
        public string username { get; set; }
        
        public string password { get; set; }
        [Compare("password",ErrorMessage = "Password and Repeat Password are not mathched")]
        public string repeatPassword { get; set; }
    }
}
