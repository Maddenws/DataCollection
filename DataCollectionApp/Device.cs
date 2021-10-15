using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollectionApp
{
    public class Device
    {
        //This method will return an integer in between 1 and 10
        public int GetMeasurement()
        {
            //Generating Random number between 1 and 10.
            return new Random().Next(1, 11);
        }
    }
}