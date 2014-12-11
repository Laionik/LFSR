using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace LFSR
{


    public partial class MainWindow : Window
    {
        private int registry_lenght;
        private string text, path;
        private Encoding encoding;
        public MainWindow()
        {
            InitializeComponent();
            CB_options.ItemsSource = File.ReadAllLines("generatory.txt");
            IMG_generator.ToolTip = "1. Select option from dropdown list\n2. Enter number of repeats\n3. Enter your registry data (or use generated one)\n4. Press \"Generate\"";
            IMG_cipher.ToolTip = "1. Select file\n2. Select option\n3. Enter your key or generate by pressing \"Generate key\"\n4. Press \"Start\"";
            IMG_test.ToolTip = "1. Select test\n2. Enter your key or generate by pressing \"Generate key\"\n3. Press \"Test\"";
            encoding = Encoding.GetEncoding("windows-1250");
            RB_encrypt.IsChecked = true;
        }

        /* Generator */

        /*Pobieranie wartiści z droplisty*/
        public string[] GetValues()
        {
            string from_cb = "";
            this.Dispatcher.Invoke(new Action(() =>
            {
                from_cb = (string)CB_options.SelectedItem;

            }));
            string[] values = from_cb.Split(',');
            return values;
        }

        /*Pobieranie wartości amount*/
        public int GetAmount()
        {
            int amount = 0;
            CB_options.Dispatcher.Invoke(new Action(() =>
            {
                amount = int.Parse(TB_amount.Text);
            }));
            return amount;
        }

        /*Generowanie rejestru*/
        private string Registry_Generate(int x)
        {
            string registry_value = "";
            Random rnd = new Random();
            for (int i = 0; i < x; i++)
            {
                registry_value += rnd.Next(2).ToString();
            }
            return registry_value;
        }

        /*Generowanie przykładowej wartości wejściowej*/
        private void input_example(int x)
        {
            string registry_value = Registry_Generate(x);
            TB_input.Text = registry_value;
            LBL_input.Content = "Input. You have entered " + x.ToString() + " bits";
        }

        /*Generowanie klucza*/
        private string registry_calculate(string[] values, string registry_value, int amount)
        {
            int temp, temp2;
            string result = "";
            for (int i = 0; i < amount; i++)
            {
                temp = Convert.ToInt32(registry_value[0]) - '0';
                foreach (string val_int in values)
                {
                    if (int.Parse(val_int) != 0)
                    {
                        temp2 = Convert.ToInt32(registry_value[int.Parse(val_int)]) - '0';
                        temp = temp ^ temp2;
                    }
                }
                registry_value = temp.ToString() + registry_value.Remove(registry_value.Length - 1);
                result += temp.ToString();
            }

            return result;
        }

        /*Main Generate*/
        public void Generate()
        {
            try
            {
                string[] values = GetValues();
                int registry_lenght = int.Parse(values[0]) + 1;
                string registry_value = "";
                int amount = GetAmount();

                this.Dispatcher.Invoke(new Action(() =>
                {
                    registry_value = TB_input.Text;
                    LBL_output.Content = "Output. Amount = " + amount;
                    TB_generate_output.Text = TB_key.Text = text = registry_calculate(values, registry_value, amount);
                    TB_generate_output.IsEnabled = true;
                }));

                File.WriteAllText("LFSR_key.txt", text);
            }
            catch (Exception x)
            {
                MessageBox.Show("Something went wrong\n" + x.Message);
            }
        }

        /*Obsługa kontrolek*/
        private void CB_options_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BT_generate.IsEnabled = true;
            TB_input.IsEnabled = true;
            string[] values = GetValues();
            registry_lenght = int.Parse(values[0]) + 1;
            TB_generate_output.Text = "";
            input_example(registry_lenght);
        }

        private void TB_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            BT_generate.IsEnabled = false;
            TB_generate_output.Text = "";
            string entered = TB_input.Text;
            Regex _regexp_registry = new Regex("^[0-1]+$");
            if (_regexp_registry.IsMatch(entered) || entered == "")
            {
                LBL_input.Content = "Input. You have entered " + entered.Length + " bits";
                if (registry_lenght == entered.Length)
                {
                    BT_generate.IsEnabled = true;
                }
            }
            else
            {
                TB_input.Text = entered.Remove(entered.Length - 1);
                MessageBox.Show("You can entering only 0 and 1 here");
            }
        }

        private void TB_amount_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex _regexp_amount = new Regex("^[0-9]+$");
            string entered = TB_amount.Text;
            if (!_regexp_amount.IsMatch(entered))
            {
                if (entered.Length > 0)
                    TB_amount.Text = entered.Remove(entered.Length - 1);
                MessageBox.Show("You can entering only numerics here");
            }
        }

        private void BT_generate_Click(object sender, RoutedEventArgs e)
        {
            Thread Tgenerate = new Thread(Generate);
            Tgenerate.Start();
        }

        /* SZYFRATOR */

        /*Konwertowanie stringów do binarek*/
        public byte[] ConvertToByteArray(string str)
        {
            return encoding.GetBytes(str);
        }

        public String byteToBinary(Byte[] data)
        {
            return string.Join("", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }

        string getBits(string txt)
        {
            byte[] bytes = encoding.GetBytes(txt);
            return byteToBinary(bytes);
        }

        /*Szyfrowanie*/
        private string encrypt(string key)
        {
            string txt = File.ReadAllText(path);
            int key_length = key.Length;
            int i = 0;
            string toEncrypt = getBits(txt);
            string output = "";
            foreach (char ch in toEncrypt)
            {
                if (i == key_length)
                    i = 0;
                output += (Convert.ToInt32(ch) - '0') ^ (Convert.ToInt32(key[i]) - '0');
                i++;
            }

            File.WriteAllText("XOR_encrypt.txt", output);
            return output;
        }

        /*Konwertowanie binrek do stringów*/
        private byte[] binarytoBytes(string bitString)
        {
            return Enumerable.Range(0, bitString.Length / 8).
                Select(pos => Convert.ToByte(
                    bitString.Substring(pos * 8, 8),
                    2)
                ).ToArray();
        }

        private String binaryToString(String binary)
        {
            byte[] bArr = binarytoBytes(binary);
            return encoding.GetString(bArr);
        }

        /*Deszyfrator*/
        private string decrypt(string key)
        {
            string txt = File.ReadAllText(path);
            string temp = "";
            int i = 0;
            foreach (char ch in txt)
            {
                if (i == key.Length)
                    i = 0;
                temp += (Convert.ToInt32(ch) - '0') ^ (Convert.ToInt32(key[i]) - '0');
                i++;
            }
            string output = binaryToString(temp);
            File.WriteAllText("XOR_decrypt.txt", output);
            return output;
        }

        /*Main Generator*/
        public void Cipher()
        {
            try
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    string key = TB_key.Text;
                    if (RB_encrypt.IsChecked == true)
                        TB_encrypt_output.Text = encrypt(key);
                    else
                        TB_encrypt_output.Text = decrypt(key);
                    TB_select_file.Text = "";
                    BT_cipher.IsEnabled = false;

                }));
            }
            catch (Exception x)
            {
                MessageBox.Show("Something went wrong\n" + x.Message);
            }
        }

        /*Obsługa kontrolek*/
        /*Menu wyboru pliku*/
        private void BT_select_file_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document 
                path = dlg.FileName;
                TB_select_file.Text = path;
            }
        }

        private void BT_cipher_Click(object sender, RoutedEventArgs e)
        {
            Thread Tgenerate = new Thread(Cipher);
            Tgenerate.Start();
        }

        private void TB_select_file_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TB_key.Text.Length > 1)
            {
                BT_cipher.IsEnabled = true;
            }
        }

        private void TB_key_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TB_select_file.Text.Length > 1)
            {
                BT_cipher.IsEnabled = true;
            }
        }

        private void BT_generate_key_Click(object sender, RoutedEventArgs e)
        {
            string cutOff = "30,6,4,1,0";
            string[] values = cutOff.Split(',');
            int amount = 10000;
            string registry_value = Registry_Generate(int.Parse(values[0]) + 1);
            string result = registry_calculate(values, registry_value, amount);
            TB_key.Text = result;
            File.WriteAllText("XOR_key.txt", result);
        }


        /*TESTS*/
        /*Obsługa kontrolek*/
        private void BT_test_Click(object sender, RoutedEventArgs e)
        {
            Thread Ttest = new Thread(Testing);
            Ttest.Start();
        }

        /*Metody testów*/
        /*Test pojedynczych bitów*/
        private void Individual_bits(string key)
        {
            int n0 = 0, n1 = 0; //liczba zer w ciągu, liczba jedynek w ciągu
            foreach (char c in key)
            {
                if (c == '0')
                    n0++;
            }
            n1 = key.Length - n0;
            int test = n0 - n1;
            double result = Math.Pow(test, 2) / key.Length;

            if (test < 150 && test > -150)
                TB_test_output.Text = "Individual bits test passed\nStatistic S = " + result;
            else
                TB_test_output.Text = "Individual bits test failed\nStatistic S = " + result;
        }
        /*Test pary bitów*/
        private void Pair_bits(string key)
        {
            int n0 = 0, n00 = 0, n01 = 0, n10 = 0, n11 = 0; //liczba 00 wciągu, liczba 01 wciągu, liczba 10 wciągu, liczba 11 wciągu
            string temp = "";
            for (int i = 0; i < key.Length - 1; i++)
            {
                temp = key[i].ToString() + key[i + 1].ToString();
                if (temp == "00")
                {
                    n00++;
                    n0++;
                }
                else if (temp == "01")
                {
                    n0++;
                    n01++;
                }
                else if (temp == "10")
                    n10++;
                else
                    n11++;
            }
            double result = (4.0 / (key.Length- 1.0)) * (Math.Pow(n00, 2) + Math.Pow(n01, 2) + Math.Pow(n10, 2) + Math.Pow(n11, 2)) - (double)(2.0 / key.Length) * (Math.Pow(n0, 2) + Math.Pow((key.Length - n0), 2)) + 1;
            int test1 = n00 - n01, test2 = n00 - n10, test3 = n00 - n11;
            if ((test1 < 150 && test1 > -150) && (test2 < 150 && test2 > -150) && (test3 < 150 && test3 > -150))
                TB_test_output.Text = "Pair bits test passed\nS = " + Math.Round(result, 2).ToString();
            else
                TB_test_output.Text = "Pair bits test failed\nS = " + Math.Round(result, 2).ToString();
        }
        /*Test pokerowy*/
        private void Poker(string key)
        {
            Dictionary<string, int> d_segments = new Dictionary<string, int>();
            d_segments.Add("0000", 0);
            d_segments.Add("0001", 0);
            d_segments.Add("0011", 0);
            d_segments.Add("0101", 0);
            d_segments.Add("1001", 0);
            d_segments.Add("0010", 0);
            d_segments.Add("0110", 0);
            d_segments.Add("1010", 0);
            d_segments.Add("0100", 0);
            d_segments.Add("1100", 0);
            d_segments.Add("0111", 0);
            d_segments.Add("1101", 0);
            d_segments.Add("1011", 0);
            d_segments.Add("1110", 0);
            d_segments.Add("1000", 0);
            d_segments.Add("1111", 0);
            string temp;
            double x = 0, result = 0;
            for (int i = 0; i < key.Length; i += 4)
            {
                temp = key[i].ToString() + key[i + 1].ToString() + key[i + 2].ToString() + key[i + 3].ToString();
                d_segments[temp] = d_segments[temp] + 1;
            }
            Dictionary<string, int>.ValueCollection values = d_segments.Values;
            foreach (int val in values)
            {
                x += Math.Pow(val, 2);
            }
            result = 0.0032 * x - 5000;
            if (Math.Round(result, 2) > 2.16 && Math.Round(result, 2) < 46.17)
                TB_test_output.Text = "Poker test passed\nResult = " + Math.Round(result, 2).ToString();
            else
                TB_test_output.Text = "Poker test failed\nResult = " + Math.Round(result, 2).ToString();

        }
        /*Test długiej serii*/
        private void Long_series(string key)
        {
            int serie = 0;
            char compare = key[0];
            foreach (char c in key)
            {
                if (compare == c)
                    serie++;
                else
                {
                    compare = c;
                    serie = 0;
                }
                if (serie >= 26)
                    break;
            }
            if (serie >= 26)
                TB_test_output.Text = "Long series test failed";
            else
                TB_test_output.Text = "Long series test passed";


        }
        /*Generowanie klucza do testów*/
        private string key_generate()
        {
            string cutOff = "30,6,4,1,0";
            string[] values = cutOff.Split(',');
            int amount = 20000;
            string registry_value = Registry_Generate(int.Parse(values[0]) + 1);
            return registry_calculate(values, registry_value, amount); ;
        }
        private void Testing()
        {
            try
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    String key = TB_key_test.Text;
                    if (RB_individual_bits.IsChecked == true)
                        Individual_bits(key);
                    else if (RB_pair_bits.IsChecked == true)
                        Pair_bits(key);
                    else if (RB_poker.IsChecked == true)
                        Poker(key);
                    else
                        Long_series(key);
                }));
            }
            catch (Exception x)
            {
                MessageBox.Show("Something went wrong\n" + x.Message);
            }
        }

    }
}
