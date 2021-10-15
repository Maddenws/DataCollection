using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataCollectionApp
{
    /// <summary>
    /// Measuring Length Device that is doing all the calculations. It has implemented IMeasuringDevice and INotifyPropertyChanged Interface.
    /// INotifyPropertyChanged Interface is prerequisite for data binding as it raises the notification of property being changed
    /// </summary>
    /// 
    class MeasuringLengthDevice : Device, IMeasuringDevice, INotifyPropertyChanged
    {
        //Declaring various variables as requirement with some extras need to use based on needs.
        private int MostRecentMeasure;
        //creating timer to ticks.
        private Timer timerForMeasuringDevice;
        //int array.
        private int[] dataCaptured;
        //event for notifying changes.
        public event PropertyChangedEventHandler PropertyChanged;
        //variable for post time of capture.
        private string timeStamp;
        //a new queue named myQueue.
        Queue<int> myQueue = new Queue<int>(10);
        //enum
        private Units unitToUse;
        //accumulator.
        private int totalCount = 0;
        //A Full property for capturing the value that will be converted from Metric to Imperial Unit and Vice Versa
        private decimal valueConvertedToAnotherUnit;

        //A Full property for units.
        public Units UnitToUse
        {
            get { return unitToUse; }
            set { unitToUse = value; }
        }

        //A Full property with Getters and setters for TimeStamp
        public string TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; this.OnPropertyChanged(); }
        }
        //A full property with Getters and Setters for MostRecentMeasure, calls onpropertychange to update when changed. 
        public int mostRecentMeasure
        {
            get { return MostRecentMeasure; }
            set { MostRecentMeasure = value; this.OnPropertyChanged(); }
        }

        public decimal ValueConvertedToAnotherUnit
        {
            get { return valueConvertedToAnotherUnit; }
            set { valueConvertedToAnotherUnit = value; this.OnPropertyChanged(); }
        }

        public int TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; this.OnPropertyChanged(); }
        }

        //Implementing the method of INotifyPropertyChanged Event. this will alow an update on any changes it made to the get/set property variable.
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        //Constructor for initializing the values for array dataCapture, Enum unitsToUse, Also the int MostRecentMeasure.
        public MeasuringLengthDevice(Units units)
        {
            dataCaptured = new int[10];
            this.unitToUse = units;
            MostRecentMeasure = 0;


        }

        //Implementing the GetRawData method of IMeasuringDevice Interface
        public int[] GetRawData()
        {

            //return data;
            int arraySize = 0;
            //creating an array that copys the current items in queue.
            int[] copyQueue = myQueue.ToArray();
            //checking the size of array.
            for (int i = 0; i < copyQueue.Length; i++)
                if (copyQueue[i] != 0)
                    //add to array size.
                    arraySize++;

            //New formated size so we dont return zeros if array is not stacked to max.
            int[] ReturnData = new int[arraySize];
            for (int i = 0; i < arraySize; i++)
                //adding index of the queue to the data array of each capture.
                ReturnData[i] = copyQueue[i];
            //returning the new array for calling.
            return ReturnData;
        }

        //Implementing GetMostRecentValue to return the most recent measure
        public int GetMostRecentValue
        {
            get { return mostRecentMeasure; }
            set { mostRecentMeasure = value; }
        }

        //The value we are supposing is in inches
        public decimal ImperialValue()
        {

            // Conver the centimeters to inches
            //using math.round to round the number within 3 decimal places.'

            decimal x = (decimal)(mostRecentMeasure / 2.54);
            return Math.Round(x, 3);

        }
        //The metric value is in centimeters
        public decimal MetricValue()
        {

            // Converting inches into cm.
            //using math.round to round the number within 3 decimal places.
            decimal x = (decimal)(mostRecentMeasure * 2.54);
            return Math.Round(x, 3);
        }

        //Implementing the Start Collecting method
        public void StartCollecting()
        {
            //Initializing the timers with passing params
            timerForMeasuringDevice = new Timer(timer_tickEnqueu, null, 15000, 15000);
        }

        //Callback method for TimeMeasuringDevice that will capture number after every 15 seconds
        private async void timer_tickEnqueu(object state)
        {

            //Checking to see the unit selected, if so this gets executed.
            if (unitToUse == Units.Imperial)
            {
                //Calling the RunAsync method of timer
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    //Calling the method AddingIntoStackAndCalculatingTimeStamp method
                    AddingIntoStackAndCalculatingTimeStamp();
                    //calling our getMeasurement from device class.
                    mostRecentMeasure = GetMeasurement();
                    //Assigning the value to valueConvertedToAnotherUnit after value conversion.
                    ValueConvertedToAnotherUnit = this.MetricValue();
                    //adding to a accumulator so user can see how many times this has collected data.
                    TotalCount += 1;
                    //using Queue to add the most recent measurement into a que of data.
                    myQueue.Enqueue(mostRecentMeasure);
                    //checking the que count. At 10 it will begin to remove the oldest from the que using dequeue.
                    if (myQueue.Count >= 11)
                    {
                        myQueue.Dequeue();
                    }

                });
            }
            //Checking to see if the unit selected is imperial, if so this gets executed.
            else if (unitToUse == Units.Metric)
            {
                //Calling the RunAsync method of timer
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    //If Generated number is not zero as 0 doesn't count a real entry

                    //Calling the method AddingIntoStackAndCalculatingTimeStamp method
                    AddingIntoStackAndCalculatingTimeStamp();
                    //Adding the measurement and unit into Custom Queue
                    ValueConvertedToAnotherUnit = this.ImperialValue();
                    //giving mostrecent measure a value from the device class.
                    mostRecentMeasure = GetMeasurement();
                    //adding to accumulator.
                    TotalCount += 1;
                    //adding most recent measure to a queue.
                    myQueue.Enqueue(mostRecentMeasure);
                    //once we reaches 10 values / limit. dequeue oldest item.
                    if (myQueue.Count >= 11)
                    {
                        myQueue.Dequeue();
                    }

                });
            }
        }

        //Implementing the method called above
        private void AddingIntoStackAndCalculatingTimeStamp()
        {
            //Calculating the time stamp which will be displayed in the UI
            TimeStamp = new DateTimeOffset(DateTime.Now).ToString();
        }

        //Method for StopCollecting
        public void StopCollecting()
        {
            //Setting the timer restart value to infinity which means cancelling the timers
            timerForMeasuringDevice.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
