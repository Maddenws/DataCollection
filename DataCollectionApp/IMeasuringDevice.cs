using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollectionApp
{
    //Interface for providing methods need to be used in MeasuringLengthDevice
    interface IMeasuringDevice
    {
        //Will return the most recent measure in metric unit
        decimal MetricValue();
        //Will return the most recent measure in imperial unit
        decimal ImperialValue();
        //This will start the timers to collect measurements
        void StartCollecting();
        //It will stop the tmers
        void StopCollecting();
        //This method will return the most recent data from data array with their units and values
        int[] GetRawData();
    }
}
