using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGLib
{
    public class RNG
    {
        private static Random random = new Random();

        //int min = 1; // Minimum value for the random number
        //int max = 100; // Maximum value for the random number

        //int randomNumber = GenerateRandomNumber(min, max);

        public static int GenerateRandomNumber(int min, int max)
        {
            return random.Next(min, max + 1);
        }
    }

}
