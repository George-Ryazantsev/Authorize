namespace BookLib_Auth_Mapp_NF.Models
{
    public class UserModel
    {     
        public int Id { get; set; }
        public string UserName { get; set; }
        public string HashPassword { get; set; }            
        public string Role { get; set; }        
    }
}
