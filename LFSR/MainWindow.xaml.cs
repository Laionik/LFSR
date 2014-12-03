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
        private static BackgroundWorker _bw;
        private delegate void GetTextDelegate(string txt);
        private int registry_lenght;
        private string text, path;
        private int i_generate, i_encrypt;
        public MainWindow()
        {
            InitializeComponent();
            CB_options.ItemsSource = File.ReadAllLines("generatory.txt");
            IMG_help.ToolTip = "1. Select option from dropdown list\n2. Enter number of repeats\n3. Enter your registry data (or use generated one)\n4. Press \"Generate\"";
            IMG_help2.ToolTip = "1. Select file\n2. Select option\n3. Enter your key\n4. Press \"Start\"";

            i_generate = 0;
            i_encrypt = 0;
        }

        /*Obsługa zdarzeń w oknie
         Zmiana opcji w dropie
         */
        private void CB_options_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BT_generate.IsEnabled = true;
            TB_input.IsEnabled = true;
            string[] values = GetValues();
            registry_lenght = int.Parse(values[0]) + 1;
            TB_generate_output.Text = "";
            input_example(registry_lenght);
        }

        /*obsługa zmiany tekstu w input*/
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
                if(entered.Length > 0)
                    TB_amount.Text = entered.Remove(entered.Length - 1);
                MessageBox.Show("You can entering only numerics here");
            }
        }

        /*wciśnięcie klawisza */
        private void BT_generate_Click(object sender, RoutedEventArgs e)
        {
            BT_generate.IsEnabled = false;

            _bw = new BackgroundWorker
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };
            _bw.DoWork += bw_Generate;

            _bw.RunWorkerAsync();
        }

        /*pobieranie wartiści z dropa*/
        public string[] GetValues()
        {
            string from_cb = "";
            CB_options.Dispatcher.Invoke(new Action(() =>
            {
                from_cb = (string)CB_options.SelectedItem;
            }));
            string[] values = from_cb.Split(',');
            return values;
        }

        /*pobieranie wartiści amount*/
        public int GetAmount()
        {
            int amount = 0;
            CB_options.Dispatcher.Invoke(new Action(() =>
            {
                amount = int.Parse(TB_amount.Text);
            }));
            return amount;
        }

        /*metoda wstawiająca przykładową wartość w input*/
        private void input_example(int registry_lenght)
        {
            string registry_value = "";
            Random rnd = new Random();
            for (int i = 0; i < registry_lenght; i++)
            {
                registry_value += rnd.Next(2).ToString();
            }
            TB_input.Text = registry_value;
            LBL_input.Content = "Input. You have entered " + registry_lenght.ToString() + " bytes";
        }

        /*Przeliczanie rejestru*/
        private string registry_calculate(string[] values, string registry_value, int amount)
        {
            int temp, temp2;
            string wynik = "";
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
                wynik += temp.ToString();
            }

            return wynik;
        }

        /*Główny proces BACKGROUNDWORKER*/
        public void bw_Generate(object sender, DoWorkEventArgs e)
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

                File.WriteAllText("LFSR_generated" + i_generate + ".txt", text);
                i_generate++;
            }
            catch (Exception x)
            {
                MessageBox.Show("Something went wrong\n" + x.Message);
            }
        }

        /* SZYFRATOR */
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

        string getBits(string txt)
        {
            string temp = "";
            byte[] bytes = Encoding.ASCII.GetBytes(txt);
            int i = 0;
            foreach (byte b in bytes)
            {
                if (Convert.ToString(b, 2).Length != 8)
                {
                    for(int x = Convert.ToString(b, 2).Length; x < 8; x++)
                    {
                        temp += '0';
                    }
                }
                temp += Convert.ToString(b, 2);
                

                i++;
            }
            return temp;
        }

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

            File.WriteAllText("LFSR_encrypt" + i_encrypt + ".txt", output);
            return output;
        }


        public Byte[] GetBytesFromBinaryString(String binary)
        {
            var list = new List<Byte>();

            for (int i = 0; i < binary.Length; i += 8)
            {
                String t = binary.Substring(i, 8);

                list.Add(Convert.ToByte(t, 2));
            }
            return list.ToArray();
        }

        private string decrypt(string key)
        {
            string txt = File.ReadAllText(path);
            string temp = "";
            int i = 0;
            int key_length = key.Length;

            Byte[] data = GetBytesFromBinaryString(txt);
            string output = Encoding.ASCII.GetString(data);

            foreach (char ch in output)
            {
                if (i == key_length)
                    i = 0;
                temp += (Convert.ToInt32(ch) - '0') ^ (Convert.ToInt32(key[i]) - '0');
                i++;
            }
            File.WriteAllText("LFSR_decrypt" + i_encrypt + ".txt", temp);
            return temp;
        }

        public void bw_Encrypt(object sender, DoWorkEventArgs e)
        {
            try
            {
                string key;
                this.Dispatcher.Invoke(new Action(() =>
                {
                    key = TB_key.Text;
                    if (RB_encrypt.IsChecked == true)
                        TB_encrypt_output.Text = encrypt(key);
                    else
                        TB_encrypt_output.Text = decrypt(key);
                    i_encrypt++;
                }));
            }
            catch (Exception x)
            {
                MessageBox.Show("Something went wrong\n" + x.Message);
            }
        }

        private void BT_encrypt_Click(object sender, RoutedEventArgs e)
        {
            _bw = new BackgroundWorker
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };
            _bw.DoWork += bw_Encrypt;

            _bw.RunWorkerAsync();
        }

        private void TB_select_file_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TB_key.Text != null)
            {
                BT_encrypt.IsEnabled = true;
            }
        }

        private void TB_key_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TB_select_file.Text != null)
            {
                BT_encrypt.IsEnabled = true;
            }
        }
    }
}
