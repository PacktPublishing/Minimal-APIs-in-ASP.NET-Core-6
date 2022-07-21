namespace Chapter06.Entities;

public class PersonEntity
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public AddressEntity Address { get; set; }
}
