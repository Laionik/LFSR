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
        private Regex _regexp;
        public MainWindow()
        {
            InitializeComponent();
            CB_options.ItemsSource = File.ReadAllLines("generatory.txt");
            IMG_help.ToolTip = "1. Select option from dropdown list\n2. Enter your registry data (or use generated one)\n3. Press button Start";
            _regexp = new Regex("^[0-1]+$");
        }

        /*Obsługa zdarzeń w oknie
         Zmiana opcji w dropie
         */
        private void CB_options_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BT_do.IsEnabled = true;
            TB_input.IsEnabled = true;
            string[] values = GetText();
            registry_lenght = int.Parse(values[0]) * 8;
            test_input(registry_lenght);
        }


        /*obsługa zmiany tekstu w input*/
        private void TB_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            BT_do.IsEnabled = false;
            string entered = TB_input.Text;
            if (_regexp.IsMatch(entered) || entered == "")
            {
                LBL_input.Content = "Input. You have entered " + entered.Length + " bytes";
                if (registry_lenght == entered.Length)
                {
                    BT_do.IsEnabled = true;
                }
            }
            else
            {
                TB_input.Text = entered.Remove(entered.Length - 1);
                MessageBox.Show("You can entering only 0 and 1 here");
                }
        }

        /*wciśnięcie klawisza */
        private void BT_do_Click(object sender, RoutedEventArgs e)
        {
            BT_do.IsEnabled = false;

            _bw = new BackgroundWorker
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };
            _bw.DoWork += bw_DoWork;

            _bw.RunWorkerAsync();

        }

        /*pobieranie wartiści z dopa*/
        public string[] GetText()
        {
            string from_cb = "";
            CB_options.Dispatcher.Invoke(new Action(() =>
            {
                from_cb = (string)CB_options.SelectedItem;
            }));
            string[] values = from_cb.Split(',');
            return values;
        }

        /*metoda testowa*/
        private void test_input(int registry_lenght)
        {
            string registry_value = "";
            Random rnd = new Random();
            for (int i = 0; i < registry_lenght; i++)
            {
                registry_value += rnd.Next(2).ToString();
            }
            TB_input.Text = registry_value;
            LBL_input.Content = "Input. You have entered " + registry_lenght.ToString() + " bytes)";
        }

        /*xorowanie bajtu*/
        private char xor(char a, char b)
        {
            if (Convert.ToInt32(a) == Convert.ToInt32(b))
                return '0';
            else
                return '1';
        }

        /*Przesuwa rejestr o jedną jednostkę w lewo*/
        private string shift(string x, char val)
        {
            return x.Remove(0, 1) + val.ToString();
        }
    
        /*Przeliczanie rejestru*/
        private string registry_calculate(string[] values, string registry_value)
        {
            for (int i = 0; i < registry_value.Length; i++)
            {
                char temp = registry_value[registry_value.Length - 1];
                foreach (string val_int in values)
                {
                    temp = xor(temp, registry_value[int.Parse(val_int)]);
                }
                registry_value = shift(registry_value, temp);
            }
            return registry_value;
        }

        /*Główny proces BACKGROUNDWORKER*/
        public void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string[] values = GetText();
                int registry_lenght = int.Parse(values[0]) * 8;
                string registry_value = "";
                TB_input.Dispatcher.Invoke(new Action(() =>
                {
                    registry_value = TB_input.Text;
                }));

                TB_output.Dispatcher.Invoke(new Action(() =>
                {
                    TB_output.Text = registry_calculate(values, registry_value);
                    TB_output.IsEnabled = true;
                }));

            }
            catch (Exception x)
            {
                MessageBox.Show("Something went wrong\n" + x.Message);
            }
        }
    }
}
