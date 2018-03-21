using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;

namespace realD31
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static int err = 0;

        private delegate Decimal Round(Decimal value, Int32 N);
        private String rouding { get; set; }

        private struct sign
        {
            public static String sb1 { get; set; }
            public static String sb2 { get; set; }
            public static String sb3 { get; set; }
            public static String sb4 { get; set; }
        }

        private void getAns_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String[] s_on =
                {
                    tb1.GetLineText(0),
                    tb2.GetLineText(0),
                    tb3.GetLineText(0),
                    tb4.GetLineText(0),
                    tb5.GetLineText(0)
                };

                var d_on = new Decimal[s_on.Length];


                for (var i = 0; i < s_on.Length; ++i)
                {
                    if (Decimal.TryParse(s_on[i], NumberStyles.Float, CultureInfo.GetCultureInfo("ru-RU"), out var vr)) d_on[i] = vr;
                    else if (Decimal.TryParse(s_on[i], NumberStyles.Float, CultureInfo.GetCultureInfo("en-US"), out var ve)) d_on[i] = ve;
                    else throw new Exception("input err");
                }

                Round round;
                if (rouding == "math") round = mRound;
                else if (rouding == "banker") round = bRound;
                else round = tRound;


                int getPriority(String s)
                {
                    return s == "+" || s == "-" ? 1 : 2;
                }
                bool getLog(String s1, String s2)
                {
                    return getPriority(s1) >= getPriority(s2);
                }
                Decimal getResult(String z, Decimal a, Decimal b)
                {
                    Decimal res = -1;
                    switch (z)
                    {
                        case "+": res = a + b;
                            break;
                        case "-": res = a - b;
                            break;
                        case "*": res = a * b;
                            break;
                        case "/":
                            if (b == 0)
                                throw new Exception("ноль, ужас");
                            res = a / b;
                            break;
                    }
                    decimal max = 1000000000000000;
                    if (res > max || res < -max)
                        throw new Exception("превышен порог чисел");
                    return round(res, 6);
                }

                Decimal mRound(Decimal value, Int32 N)
                {
                    return Decimal.Round(value, N, MidpointRounding.AwayFromZero);
                }
                Decimal bRound(Decimal value, Int32 N)
                {
                    return Decimal.Round(value, N, MidpointRounding.ToEven);
                }
                Decimal tRound(Decimal value, Int32 N)
                {
                    Decimal st = 10m;
                    for (var i = 0; i < N - 1; ++i) st *= 10m;
                    return decimal.Truncate(value * st) / st;
                }

                

                Decimal v1_intermediate, v2_intermediate;

                if (getLog(sign.sb2, sign.sb3))
                {
                    v1_intermediate = getResult(sign.sb2,d_on[1],d_on[2]);
                    v1_intermediate = getResult(sign.sb3, v1_intermediate, d_on[3]);
                }
                else
                {
                    v1_intermediate = getResult(sign.sb3, d_on[2], d_on[3]);
                    v1_intermediate = getResult(sign.sb2, d_on[1], v1_intermediate);
                }
                if (getLog(sign.sb1, sign.sb4))
                {
                    v2_intermediate = getResult(sign.sb1, d_on[0],v1_intermediate);
                    v2_intermediate = getResult(sign.sb4, v2_intermediate, d_on[4]);
                }
                else
                {
                    v2_intermediate = getResult(sign.sb4,v1_intermediate,d_on[4]);
                    v2_intermediate = getResult(sign.sb1,d_on[0],v2_intermediate);
                }

                lanswer.Content = (round(v2_intermediate, 3)).ToString("#,0.###");
               

            }
            catch(Exception ex)
            {

                lerr.Content = ex.Message + (err++).ToString();
            }
            finally
            { }
        }


        private void rounding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            rouding = ((TextBlock)(((ComboBox)sender).SelectedItem)).Text;
        }

        private void sb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sign.sb1 = ((TextBlock)(((ComboBox)sender).SelectedItem)).Text;
        }
        private void sb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sign.sb2 = ((TextBlock)(((ComboBox)sender).SelectedItem)).Text;
        }
        private void sb3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sign.sb3 = ((TextBlock)(((ComboBox)sender).SelectedItem)).Text;
        }
        private void sb4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sign.sb4 = ((TextBlock)(((ComboBox)sender).SelectedItem)).Text;
        }

    }
}
