using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KviatkovskyiLab3
{
    internal class PersonModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string? _email;
        private DateTime? _birthdate;
        private int? _age;
        private readonly bool? _isAdult;
        private readonly string? _zodiacWestern;
        private readonly string? _zodiacChinese;
        private readonly bool? _isBirthday;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PersonModel(string firstName, string lastName)
        {
            _firstName = firstName;
            _lastName = lastName;
        }

        public PersonModel(string firstName, string lastName, string email) : this(firstName, lastName)
        {
            _email = email;
        }

        public PersonModel(string firstName, string lastName, DateTime birthdate) : this(firstName, lastName)
        {
            _birthdate = birthdate;
        }


        public PersonModel(string firstName, string lastName, string email, DateTime birthdate) : this(firstName, lastName, email)
        {
            _birthdate = birthdate;
            if (!_birthdate.HasValue) return;
            _age = CalculateAge();
            _isAdult = CalculateIsAdult();
            _isBirthday = CalculateIsBirthday();
            _zodiacWestern = CalculateZodiacWestern();
            _zodiacChinese = CalculateZodiacChinese();
        }


        public string FirstName
        {
            get { return _firstName; }
            set { 
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string? Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public DateTime? Birthdate
        {
            get { return _birthdate; }
            set { _birthdate = value; }
        }

        public int? Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public bool? IsAdult
        {
            get { return _isAdult; }
        }

        public string? ZodiacWestern
        {
            get { return _zodiacWestern; }
        }

        public string? ZodiacChinese
        {
            get { return _zodiacChinese; }
        }

        public bool? IsBirthday
        {
            get { return _isBirthday; }
        }


        private int? CalculateAge()
        {
            DateTime current = DateTime.Now;
            if (!_birthdate.HasValue) return null;
            int age = current.Year - _birthdate.Value.Year;

            if (current.Month < _birthdate.Value.Month || (current.Month == _birthdate.Value.Month && current.Day < _birthdate.Value.Day))
            {
                age--;
            }

            return age;
        }

        private bool? CalculateIsAdult()
        {
            if (!_age.HasValue) return null;
            if (_age.Value >= 18) return true;
            return false;
        }

        private bool? CalculateIsBirthday()
        {
            if (!_birthdate.HasValue) return null;
            DateTime current = DateTime.Now;
            return (current.Month == _birthdate.Value.Month && current.Day == _birthdate.Value.Day);
        }



        private string? CalculateZodiacWestern()
        {
            if (!_birthdate.HasValue) return null;
            DateTime[] zodiacDates = new DateTime[]
            {
                new DateTime(_birthdate.Value.Year, 1, 20),      // Aquarius
                new DateTime(_birthdate.Value.Year, 2, 19),      // Pisces
                new DateTime(_birthdate.Value.Year, 3, 21),      // Aries
                new DateTime(_birthdate.Value.Year, 4, 20),      // Taurus
                new DateTime(_birthdate.Value.Year, 5, 21),      // Gemini
                new DateTime(_birthdate.Value.Year, 6, 21),      // Cancer
                new DateTime(_birthdate.Value.Year, 7, 23),      // Leo
                new DateTime(_birthdate.Value.Year, 8, 23),      // Virgo
                new DateTime(_birthdate.Value.Year, 9, 23),      // Libra
                new DateTime(_birthdate.Value.Year, 10, 23),     // Scorpio
                new DateTime(_birthdate.Value.Year, 11, 22),     // Sagittarius
                new DateTime(_birthdate.Value.Year, 12, 22),     // Capricorn

            };

            string[] zodiacSigns = { "Capricorn", "Aquarius", "Pisces", "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Sagittarius" };

            for (int i = 0; i < zodiacDates.Length; i++)
            {
                if (_birthdate < zodiacDates[i]) return zodiacSigns[i];
            }

            return zodiacSigns[0];
        }

        private string? CalculateZodiacChinese()
        {
            if (!_birthdate.HasValue) return null;

            int yearInCycle = _birthdate.Value.Year % 12;

            string[] zodiacSigns = { "Monkey", "Rooster", "Dog", "Pig", "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat" };

            return zodiacSigns[yearInCycle];
        }
    }
}
