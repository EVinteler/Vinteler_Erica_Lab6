using System.Windows.Controls;

namespace Vinteler_Erica_Lab6
{
    //validator pentru camp required
    public class StringNotEmpty : ValidationRule
    {
        //mostenim din clasa ValidationRule
        //suprascriem metoda Validate ce returneaza un ValidationResult
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureinfo)
        {
            string aString = value.ToString();
            if (aString == "")
                return new ValidationResult(false, "String cannot be empty");
            return new ValidationResult(true, null);
        }
    }
    //validator pentru lungime minima a string-ului
    public class StringMinLengthValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureinfo)
        {
            string aString = value.ToString();
            if (aString.Length < 3)
                return new ValidationResult(false, "String must have at least 3 characters!");
            return new ValidationResult(true, null);
        }
    }
    private void SetValidationBinding()
    {
        Binding firstNameValidationBinding = new Binding();
        firstNameValidationBinding.Source = customerViewSource;
        firstNameValidationBinding.Path = new PropertyPath("FirstName");
        firstNameValidationBinding.NotifyOnValidationError = true;
        firstNameValidationBinding.Mode = BindingMode.TwoWay;
        firstNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        // string required
        firstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
        firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameValidationBinding);

        Binding lastNameValidationBinding = new Binding();
        lastNameValidationBinding.Source = customerViewSource;
        lastNameValidationBinding.Path = new PropertyPath("LastName");
        lastNameValidationBinding.NotifyOnValidationError = true;
        lastNameValidationBinding.Mode = BindingMode.TwoWay;
        lastNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        // string min length validator
        lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValid());
        lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameValidationBinding);
    }
}