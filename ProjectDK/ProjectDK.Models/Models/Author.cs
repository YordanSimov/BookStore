namespace ProjectDK.Models.Models
{
    public class Author : Person
    {
        public Author(int id, string name, int age, string nickname) : base(id, name, age)
        {
            DateOfBirth = DateTime.Now;
            Nickname = nickname;
        }

        public string Nickname { get; set; }
    }
}
