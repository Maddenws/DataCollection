using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DataCollectionApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Units measureUnits;
        private MeasuringLengthDevice measuringLengthDevice;
        public MainPage()
        {
            this.InitializeComponent();
            //creating a new instance of the measurelength device obj. calls checkradio to pass in the radiobutton selection 
            this.measuringLengthDevice = new MeasuringLengthDevice(CheckRadioButton());
        }
        //method to check radio button selection.
        public Units CheckRadioButton()
        {

            if (rd_ImperialValue.IsChecked == true)
            {
                measureUnits = Units.Imperial;
                return measureUnits;
            }
            else
            {
                measureUnits = Units.Metric;
                return measureUnits;
            }
        }

        //start button event click.
        private void Button_Start(object sender, RoutedEventArgs e)
        {
            //Setting the texts of Textblocks to White Spaces
            tbl_MRV.Text = " ";
            tbl_TimeStamp.Text = " ";
            tbl_UnitConversion.Text = " ";
            textBlock4.Text = " ";
            CheckRadioButton();
            //Setting the selected Units by checking the value of Radio Boxes
            if (CheckRadioButton() == Units.Imperial)
            {


                //Disabling the other control
                rd_MetricValue.IsEnabled = false;
                //Assigning the value to textblock
                tbl_UnitConversion.Text = "From inches to Cm";
            }
            //Checking the other case
            else if (CheckRadioButton() == Units.Metric)
            {

                //Disabling the other radio box
                rd_ImperialValue.IsEnabled = false;
                //Assigning the value to textblock
                tbl_UnitConversion.Text = "From Cm To Inches";
            }
            //disabling the controls
            Button_DisplayHistory.IsEnabled = false;
            btn_Start.IsEnabled = false;
            //Start collecting the data
            measuringLengthDevice.StartCollecting();
        }
        //stop button event click.
        private void Button_Stop(object sender, RoutedEventArgs e)
        {
            //enables buttons for user use again and calls stopcollecting to timeout its timers.
            Button_DisplayHistory.IsEnabled = true;
            btn_Start.IsEnabled = true;
            rd_MetricValue.IsEnabled = true;
            rd_ImperialValue.IsEnabled = true;
            measuringLengthDevice.StopCollecting();
        }

        private void ButtonDisplayHistory(object sender, RoutedEventArgs e)
        {
            //making a variable to check radiovalue.
            Units testRadio = CheckRadioButton();
            //changing the measuring devices units to use based on users radio button selection.
            measuringLengthDevice.UnitToUse = testRadio;
            //pulling information from raw data.
            int[] passedIntArray = measuringLengthDevice.GetRawData();

            //using the length of the array we will copy into a decimal array.
            decimal[] accurateArray = new decimal[passedIntArray.Length];

            for (int i = 0; i < passedIntArray.Length; i++)
            {
                decimal x = passedIntArray[i];
                //if imperial, do proper math to convert cm into a inch
                if (testRadio == Units.Imperial)
                {
                    //mathround with a 3 decimal placement on the passed info.
                    x = Math.Round(x / (decimal)2.54, 3);
                }
                //if it is metric, do proper math to go inches to cm.
                else if (testRadio == Units.Metric)
                {
                    x = Math.Round(x * (decimal)2.54, 3);

                }
                accurateArray[i] = x;
            }


            //generates a fresh view when refreshed.
            listBox.Items.Clear();
            //each index in the accurateArray will be displayed based on unit. this will be added to the listbox with appropraite conversion type.
            foreach (var item in accurateArray)
            {

                if (testRadio == Units.Imperial)
                {
                    listBox.Items.Add($"{item} inches");

                }
                else if (testRadio == Units.Metric)
                {
                    listBox.Items.Add($"{item} cm");
                }
            }

        }
    }
}
