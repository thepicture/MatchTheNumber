using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NumberGuesser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Matcher matcher;
        public MainWindow()
        {
            InitializeComponent();

            MessageBlock.Text = "The program guesses the number from 1 to n, where n is a supremum.\nPlease enter the inclusive supremum up to " + ulong.MaxValue + ":";
            SupremumBox.Focus();
        }

        public int GetNumberOfDigits(ulong number)
        {
            int sum = 0;
            while (number > 10)
            {
                sum ++;
                number /= 10;
            }
            return sum;
        }

        private void GreaterThanButton_Click(object sender, RoutedEventArgs e)
        {
            matcher.SayGreater();
            MessageBlock.Text = matcher.GetMessage();
        }

        private void LessThanButton_Click(object sender, RoutedEventArgs e)
        {
            matcher.SayLess();
            MessageBlock.Text = matcher.GetMessage();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Exit confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                App.Current.Shutdown();
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void SupremumSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            matcher = new Matcher(ulong.Parse(SupremumBox.Text));
            SupremumSubmitButton.Visibility = Visibility.Collapsed;
            SupremumBox.Visibility = Visibility.Collapsed;

            ButtonList.Visibility = Visibility.Visible;
            MessageBlock.Text = matcher.GetMessage();
        }

        private void SupremumBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (new Regex(@"^[1-9][0-9]{0," + GetNumberOfDigits(ulong.MaxValue) + @"}$").IsMatch(textBox.Text))
            {
                if (ulong.Parse(textBox.Text) > 1000000000)
                {
                    SupremumSubmitButton.IsEnabled = false;
                    return;
                }
                textBox.Foreground = Brushes.Black;
                SupremumSubmitButton.IsEnabled = true;
            }
            else
            {
                textBox.Foreground = Brushes.Red;
                SupremumSubmitButton.IsEnabled = false;
            }
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            matcher.SayYes();
            MessageBlock.Text = matcher.GetMessage();
            ButtonList.Visibility = Visibility.Collapsed;
            RestartButton.Visibility = Visibility.Visible;
        }
    }
}
