namespace EcoPark.Domain.Aggregates.Client;

public class ClientAggregateRoot
{
    private List<Car> _cars = new();

    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Password { get; set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public ClientAggregateRoot(Guid id, string email, string password, string firstName, string lastName)
    {
        Id = id;
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
    }

    public ClientAggregateRoot()
    {

    }

    public ClientAggregateRoot(ClientModel client)
    {
        Id = client.Id;
        Email = client.Email;
        Password = client.Password;
        FirstName = client.FirstName;
        LastName = client.LastName;
    }

    public IReadOnlyCollection<Car> Cars => new ReadOnlyCollection<Car>(_cars);

    public void AddCar(Car car) => _cars.Add(car);

    public bool RemoveCar(Car car) => _cars?.Remove(car) ?? false;

    public bool RemoveCarById(Guid carId)
    {
        var car = _cars?.FirstOrDefault(c => c.Id == carId);

        return car != null && (_cars?.Remove(car) ?? false);
    }

    public void UpdateCar(Guid carId, string? plate, ECarType? type, string? model, string? color, string? brand, int? year)
    {
        var existingCar = _cars?.FirstOrDefault(c => c.Id == carId);

        if (existingCar == null)
            return;

        if (!string.IsNullOrWhiteSpace(plate))
            existingCar.UpdatePlate(plate);

        if (type != null)
            existingCar.UpdateType(type.Value);

        if (!string.IsNullOrWhiteSpace(model))
            existingCar.UpdateModel(model);

        if (!string.IsNullOrWhiteSpace(color))
            existingCar.UpdateColor(color);

        if (!string.IsNullOrWhiteSpace(brand))
            existingCar.UpdateBrand(brand);

        if (year != null)
            existingCar.UpdateYear(year.Value);
    }

    public void UpdateFirstName(string firstName)
    {
        if (!string.IsNullOrWhiteSpace(firstName) && !FirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase))
            FirstName = firstName;
    }

    public void UpdateLastName(string lastName)
    {
        if (!string.IsNullOrWhiteSpace(lastName) && !LastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
            LastName = lastName;
    }

    public void UpdateEmail(string email)
    {
        if (!string.IsNullOrWhiteSpace(email) && !Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
            Email = email.ToLower();
    }

    public void UpdatePassword(string password)
    {
        if (!string.IsNullOrWhiteSpace(password) && !Password.Equals(password, StringComparison.InvariantCultureIgnoreCase))
            Password = password;
    }

    public string GetFullName() => $"{FirstName} {LastName}";
}