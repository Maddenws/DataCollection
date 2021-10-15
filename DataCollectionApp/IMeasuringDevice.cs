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

        decimal MetricValue();

        decimal ImperialValue();

        void StartCollecting();

        void StopCollecting();

        int[] GetRawData();
    }
}
