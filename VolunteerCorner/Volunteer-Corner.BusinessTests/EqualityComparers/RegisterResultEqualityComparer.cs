using Volunteer_Corner.Business.Models.Results;

namespace Volunteer_Corner.BusinessTests.EqualityComparers;

// For comparing register results without ids because each time new user is instantiated, it has new Guid
public class RegisterResultEqualityComparer : IEqualityComparer<UserResult>
{
    public bool Equals(UserResult x, UserResult y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.FirstName == y.FirstName && x.LastName == y.LastName && x.Email == y.Email && x.PhoneNumber == y.PhoneNumber;
    }

    public int GetHashCode(UserResult obj)
    {
        return HashCode.Combine(obj.FirstName, obj.LastName, obj.Email, obj.PhoneNumber);
    }
}