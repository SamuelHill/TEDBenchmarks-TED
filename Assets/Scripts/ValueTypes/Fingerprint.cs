using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Random = TED.Utilities.Random;

namespace Scripts.ValueTypes
{
    public readonly struct Fingerprint
    {
        static Fingerprint()
        {
            InitPopCountArray();
        }

        private Fingerprint(uint bits)
        {
            _bitVector = bits;
        }

        public static Fingerprint Random() => new(RandomUint());

        public static Fingerprint Mood() => new(RandomMood());

        private readonly uint _bitVector;

        public float Affinity(Fingerprint other, Fingerprint mood)
        {
            return BitCountToAffinity(PopCount(_bitVector ^ other._bitVector ^ mood._bitVector));
        }

        public float Affinity(Fingerprint other)
        {
            return BitCountToAffinity(PopCount(_bitVector ^ other._bitVector));
        }

        private static float BitCountToAffinity(int count) => (count - 10) * (100f / 22);

        private static readonly System.Random Rng = TED.Utilities.Random.MakeRng();

        private static uint RandomUint() => unchecked((uint)Rng.Next());

        private const int FiveBits = 0x1f; 

        /// <summary>
        /// Choose 6 random bits from a 32 bit word and return a mask of just them
        /// The same bit can be chosen more than once, in which case the mask will have fewer bits.
        /// </summary>
        /// <returns></returns>
        private static uint RandomMood()
        {
            var bits = Rng.Next();
            var bit1 = bits & FiveBits;
            var bit2 = (bits >> 5) & FiveBits;
            var bit3 = (bits >> 10) & FiveBits;
            var bit4 = (bits >> 15) & FiveBits;
            var bit5 = (bits >> 20) & FiveBits;
            var bit6 = (bits >> 25) & FiveBits;
            return (1u << bit1) | (1u << bit2) | (1u << bit3) | (1u << bit4) | (1u << bit5) | (1u << bit6);
        }

        #region Population count implementation, since Unity's version of .NET doesn't have BitOperations.PopCount()
        // Cribbed from our friend wikipedia
        static int PopCount(uint x)
        {
            return (int)(Wordbits[x & 0xFFFF] + Wordbits[x >> 16]);
        }

        private static readonly uint[] Wordbits = new uint[65536]; /* bit counts of integers 0 through 65535, inclusive */
        static void InitPopCountArray()
        {
            uint i;
            uint x;
            uint count;
            for (i=0; i <= 0xFFFF; i++)
            {
                x = i;
                for (count=0; x != 0; count++) // borrowed from popcount64d() in wikipedia
                    x &= x - 1;
                Wordbits[i] = count;
            }
        }
        #endregion
    }
}
