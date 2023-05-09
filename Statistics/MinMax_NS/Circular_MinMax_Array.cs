using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickStatistics.Net.MinMax_NS
{
    internal class Circular_MinMax_Array<T> where T : struct
    {
        internal Circular_MinMax_Array(int length)
        {
            Array = new T[length];
            Clear();
        }
        internal T[] Array { get; set; }
        internal int HeadIndex { get; private set; }
        internal int TailIndex { get; private set; }
        internal int Length { get; private set; }
        internal void Clear()
        {
            HeadIndex = 0;
            TailIndex = 0;
            Length = 0;
        }
        internal T GetValueAt(int index)
        {
            return Array[ConvertIndex(index)];
        }
        internal int ConvertIndex(int index)
        {
            if (index > Length)
            {
                throw new IndexOutOfRangeException($"index {index} is larger than array Length {Length} (max {Length - 1})!");
            }
            int indexConversion = HeadIndex + index;
            if (indexConversion >= Array.Length)
            {
                indexConversion -= Array.Length;
            }
            return indexConversion;
        }
        // appends a data point and returns value if a point gets overwritten
        internal void NewMinMaxFound(T input)
        {
            Length = 1;
            TailIndex = HeadIndex;
            Array[HeadIndex] = input;
        }
        internal Nullable<T> AppendPoint(T input) 
        {
            Nullable<T> result = null; // Type T must be a non nullable value type...
            if (Length == Array.Length)
            {
                result = Array[TailIndex];
                TailIndex = IncrementIndexByOne(TailIndex);
                Length--;
            }
            Length++;
            HeadIndex = IncrementIndexByOne(HeadIndex);
            Array[HeadIndex] = input;
            return result;
        }
        /// <summary>
        /// at which point to cut the snake. the cut tasil is discarded
        /// </summary>
        /// <remarks>
        /// cut index is inclusive and will not be discarded. The cut effectively takes place after the cutIndex
        /// </remarks>
        /// <param name="cutIndex"></param>
        internal void CutTail(int cutIndex)
        {
            TailIndex = ConvertIndex(cutIndex);
        }
        private int IncrementIndexByOne(int input)
        {
            input++;
            if (input >= Array.Length)
            {
                return 0;
            }
            return input;
        }
    }
}
