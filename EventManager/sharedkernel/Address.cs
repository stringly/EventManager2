using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.sharedkernel
{
    public class Address : IEquatable<Address>
    {
        public static Address Create(string addressLine1, string addressLine2, string city, string state, string zip)
        {
            return new Address(addressLine1, addressLine2, city, state, zip);
        }
        private Address() { }
        public static Address Empty()
        {
            return new Address(null, null, null, null, null);
        }
        public bool IsEmpty()
        {
            if (string.IsNullOrEmpty(AddressLine1) && string.IsNullOrEmpty(AddressLine2) && string.IsNullOrEmpty(City) && string.IsNullOrEmpty(State) && string.IsNullOrEmpty(Zip))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private Address(string addressLine1, string addressLine2, string city, string state, string zip)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            Zip = zip;
        }
        public string AddressLine1 { get; private set; }
        public string AddressLine2 { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Zip { get; private set; }

        public string StreetAddress => AddressLine1 + " " + AddressLine2;
        public string FullAddress => $"{AddressLine1}{(string.IsNullOrEmpty(AddressLine2) ? "" : $" {AddressLine2}")} {City}, {State} {Zip}";
        public override bool Equals(object obj)
        {
            return Equals(obj as Address);
        }
        public bool Equals(Address other)
        {
            return !(other is null) && AddressLine1 == other.AddressLine1 && AddressLine2 == other.AddressLine2 && City == other.City && State == other.State && Zip == other.Zip;
        }
        public static bool operator ==(Address x, Address y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Address x, Address y)
        {
            return !(x == y);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
