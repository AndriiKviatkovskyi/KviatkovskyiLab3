using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KviatkovskyiLab3
{
    internal class PersonViewModel : INotifyPropertyChanged
    {

        private MainWindow _mainWindow;
        private PersonModel _person;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ProceedCommand { get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() => handler.Invoke(this, new PropertyChangedEventArgs(propertyName)));
                }
            }
        }

        public PersonViewModel(MainWindow mainWindow)
        {
            _person = new PersonModel("", "");
            _mainWindow = mainWindow;
            ProceedCommand = new RelayCommand(Proceed, CanProceed);
        }

        public string FirstName
        {
            get { return _person.FirstName; }
            set {
                _person.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get { return _person.LastName; }
            set {
                _person.LastName = value; 
                OnPropertyChanged(nameof(LastName));
                }

        }

        public string Email
        {
            get { return _person.Email ?? ""; }
            set { 
                _person.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public DateTime? Birthdate
        {
            get { return _person.Birthdate; }
            set {
                _person.Birthdate = value;
                OnPropertyChanged(nameof(Birthdate));
            }
        }

        public string BirthdateString
        {
            get { return Birthdate.HasValue ? Birthdate.Value.ToShortDateString() : ""; }
            set { OnPropertyChanged(nameof(BirthdateString)); }
        }

        public int Age
        {
            get { return _person.Age ?? -1; }
        }

        public string AgeString
        {
            get { return Age == -1 ? "" : Age.ToString(); }
            set { OnPropertyChanged(nameof(AgeString)); }
        }

        public bool? IsAdult
        {
            get { return _person.IsAdult; }
        }

        public string AdultString
        {
            get { return IsAdult.HasValue ? (IsAdult.Value ? "Adult" : "Child") : ""; }
            set { OnPropertyChanged(nameof(AdultString)); }
        }

        public string ZodiacWestern
        {
            get { return _person.ZodiacWestern ?? string.Empty; }
            set { OnPropertyChanged(nameof(ZodiacWestern)); }
        }

        public string ZodiacChinese
        {
            get { return _person.ZodiacChinese ?? string.Empty; }
            set { OnPropertyChanged(nameof(ZodiacChinese)); }
        }

        public bool IsBirthday
        {
            get { return _person.IsBirthday ?? false; }
        }

        public string BirthdayString
        {
            get { return IsBirthday ? "HAPPY BIRTHDAY!" : ""; }
            set { OnPropertyChanged(nameof(BirthdayString)); }
        }

        public PersonModel Person
        {
            get { return _person ?? new PersonModel("", ""); }
            set { _person = value; }
        }

        private bool CanProceed(object parameter)
        {

            return !string.IsNullOrWhiteSpace(_mainWindow.EnterFirstName.Text) &&
                   !string.IsNullOrWhiteSpace(_mainWindow.EnterLastName.Text) &&
                   !string.IsNullOrWhiteSpace(_mainWindow.EnterEmail.Text) &&
                   _mainWindow.PickedBirthday.SelectedDate.HasValue;
        }

        private async void Proceed(object parameter)
        {
            foreach (UIElement element in _mainWindow.MainGrid.Children)
            {
                element.IsEnabled = false;
            }
            _mainWindow.MainWindowName.IsEnabled = false;
            try
            {
                string tempName = _mainWindow.EnterFirstName.Text;
                string tempSurname = _mainWindow.EnterLastName.Text;
                string tempEmail = _mainWindow.EnterEmail.Text;
                DateTime tempDate = DateTime.Now;
                if (_mainWindow.PickedBirthday.SelectedDate.HasValue)
                {
                    tempDate = _mainWindow.PickedBirthday.SelectedDate.Value;
                }
                await ProcessAsync(tempName, tempSurname, tempEmail, tempDate);

               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
                _person = new PersonModel("", "");
                activateChanges();
            }
            foreach (UIElement element in _mainWindow.MainGrid.Children)
            {
                element.IsEnabled = true;
            }
            _mainWindow.MainWindowName.IsEnabled = true;
        }

        public async Task ProcessAsync(string tempName, string tempSurname, string tempEmail, DateTime tempDate)
        {
            int age = GetAge(tempDate);
            if (age < 0) throw new NegativeAgeException();
            if (age > 135) throw new BigAgeException();
            if (!checkEmail(tempEmail)) throw new InvalidEmailException();
            await Task.Run(() =>
            {
                _person = new PersonModel(tempName, tempSurname, tempEmail, tempDate);
            });
            activateChanges();
        }

        private int GetAge(DateTime date)
        {
            DateTime current = DateTime.Now;
            int age = current.Year - date.Year;

            if (current.Month < date.Month || (current.Month == date.Month && current.Day < date.Day))
            {
                age--;
            }
            return age;
        }

        private bool checkEmail(string email)
        {
            if (!email.Contains('@')) return false;
            string[] parts = email.Split('@');
            if (parts.Length != 2) return false;
            if (!parts[1].Contains('.')) return false;
            string[] newParts = parts[1].Split(".");
            if (newParts.Length != 2) return false;
            return true;
        }

        private void activateChanges()
        {
            FirstName = FirstName;
            LastName = LastName;
            Email = Email;
            Birthdate = Birthdate;
            BirthdateString = BirthdateString;
            AgeString = AgeString;
            AdultString = AdultString;
            BirthdayString = BirthdayString;
            ZodiacWestern = ZodiacWestern;
            ZodiacChinese = ZodiacChinese;
        }



        public class NegativeAgeException : Exception
        {
            public NegativeAgeException() : base("Age cannot be under 0.") { }
        }

        
        public class BigAgeException : Exception
        {
            public BigAgeException() : base("Age cannot be over 135.") { }
        }

      
        public class InvalidEmailException : Exception
        {
            public InvalidEmailException() : base("Invalid email address.") { }
        }
    }
}
