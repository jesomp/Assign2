using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;

namespace Assign2
{
    public class RandomNumberGenerator
    {
        private const uint ARRAYSIZE = 256;
        private const uint START = 13;
        private const uint TRAIL = 0;
        private const uint LAG = (START - TRAIL);
        private const uint BITSIZE = 32;

        private uint[] UnsignedIntArray;
        private uint start;
        private uint trail;

        public RandomNumberGenerator(uint encryptkey)
        {
            //set start and trail to 13 and 0
            this.start = START;
            this.trail = TRAIL;

            UnsignedIntArray = new uint[ARRAYSIZE];
            //filling in the array with cube values of index
            for (uint index = 0; index < ARRAYSIZE; index++)
            {
                UnsignedIntArray[index] = Convert.ToUInt32(Math.Pow(index, 3));
            }
            //Assign encryption key from textbox to index[0] of array
            UnsignedIntArray[0] = encryptkey;

            //call the Next method 256 * lag
            for (uint i = 0; i < (ARRAYSIZE * LAG); i++)
            {
                uint randomNumber = Next();
            }
        }

        //method to generate random numbers
        public uint Next()
        {
            uint val_s = UnsignedIntArray[start];
            uint val_t = UnsignedIntArray[trail];

            //generate spins to shift bits 
            int spin = (int)(val_t % BITSIZE);
            int spin2 = (int)BITSIZE - spin;

            //move left most bits to right most postion
            uint new_val_s = (val_s << spin | val_s >> spin2);
            uint random = new_val_s ^ val_t;
            UnsignedIntArray[start] = random;

            //increment start and trail by 1
            start++;
            trail++;

            //reset start and trail to 0 if it reaches 255
            if (start > ARRAYSIZE - 1)
                start = 0;
            if (trail > ARRAYSIZE - 1)
                trail = 0;

            return random;
        }
    }

    [TestFixture]

    public class RandomNumber
    {
        [Test]
        public void TestRandomNumber()
        {
            RandomNumberGenerator rng = new RandomNumberGenerator(3412);
            
            //validate random number matches
            Assert.AreEqual(1896325970, rng.Next());
            
            //validate random number does not match
            Assert.AreNotEqual(0, rng.Next());
        }
    }
}
