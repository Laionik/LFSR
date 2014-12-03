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
        private string toWrite;
        private int i;
        public MainWindow()
        {
            InitializeComponent();
            CB_options.ItemsSource = File.ReadAllLines("generatory.txt");
            IMG_help.ToolTip = "1. Select option from dropdown list\n2. Enter number of repeats\n3. Enter your registry data (or use generated one)\n4. Press button Start";
            i = 0;
        }

        /*Obsługa zdarzeń w oknie
         Zmiana opcji w dropie
         */
        private void CB_options_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BT_do.IsEnabled = true;
            TB_input.IsEnabled = true;
            string[] values = GetValues();
            registry_lenght = int.Parse(values[0]) + 1;
            TB_output.Text = "";
            input_example(registry_lenght);
        }


        /*obsługa zmiany tekstu w input*/
        private void TB_input_TextChanged(object sender, TextChangedEventArgs e)
        {
            BT_do.IsEnabled = false;
            TB_output.Text = "";
            string entered = TB_input.Text;
            Regex _regexp_registry = new Regex("^[0-1]+$");
            if (_regexp_registry.IsMatch(entered) || entered == "")
            {
                LBL_input.Content = "Input. You have entered " + entered.Length + " bits";
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

        private void TB_amount_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex _regexp_amount = new Regex("^[0-9]+$");
            string entered = TB_amount.Text;
            if (!_regexp_amount.IsMatch(entered))
            {
                TB_amount.Text = entered.Remove(entered.Length - 1);
                MessageBox.Show("You can entering only numerics here");
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
        public void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string[] values = GetValues();
                int registry_lenght = int.Parse(values[0]) + 1;
                string registry_value = "";
                int amount = GetAmount();
                TB_input.Dispatcher.Invoke(new Action(() =>
                {
                    registry_value = TB_input.Text;
                }));

                LBL_output.Dispatcher.Invoke(new Action(() =>
                {

                        LBL_output.Content = "Output. Amount = " + amount;
                }));

                TB_output.Dispatcher.Invoke(new Action(() =>
                {
                    TB_output.Text = toWrite = registry_calculate(values, registry_value, amount);
                    TB_output.IsEnabled = true;
                }));
               
                File.WriteAllText("LFSR_output" + i +".txt", toWrite);
                i++;
            }
            catch (Exception x)
            {
                MessageBox.Show("Something went wrong\n" + x.Message);
            }
        }
    }
}
