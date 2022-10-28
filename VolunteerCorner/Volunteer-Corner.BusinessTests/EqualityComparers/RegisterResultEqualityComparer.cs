using Volunteer_Corner.Business.Models.Results;

namespace Volunteer_Corner.BusinessTests.EqualityComparers;

// For comparing register results without ids because each time new user is instantiated, it has new Guid
public class RegisterResultEqualityComparer : IEqualityComparer<RegisterResult>
{
    public bool Equals(RegisterResult x, RegisterResult y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.FirstName == y.FirstName && x.LastName == y.LastName && x.Patronymic == y.Patronymic;
    }

    public int GetHashCode(RegisterResult obj)
    {
        return HashCode.Combine(obj.FirstName, obj.LastName, obj.Patronymic);
    }
}