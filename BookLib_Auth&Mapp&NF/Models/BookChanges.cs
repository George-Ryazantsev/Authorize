namespace BookLib_Auth_Mapp_NF.Models
{
    public class BookChanges
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ChangeType { get; set; } 
        public DateTime ChangeDate { get; set; }
    }
}
/*USE BookLibAuth 
CREATE TABLE BookChanges (
  Id INT PRIMARY KEY IDENTITY(1,1),
  Title VARCHAR(255) NOT NULL,
  Author VARCHAR(255) NOT NULL,
  ChangeType VARCHAR(255) NOT NULL,
  ChangeDate DATETIME2 NOT NULL
);*/