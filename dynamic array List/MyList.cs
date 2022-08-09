using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dynamic_array_List
{
    internal class MyList<T>
    {
        private T[] unit;
        public int Count { get; set; }

        public int index = 0;

        private int size = 0;
        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        public MyList()
        {
            IncreaseSize();
            this.unit = new T[this.size];
        }

        private int IncreaseSize()
        { return this.size = this.size + 1; }

        public void Add(T value)
        {
            if(this.index == this.size) Array.Resize<T>(ref this.unit, IncreaseSize());
            this.unit[index] = value;
            this.index++;
        }

        public void Delete(int i)
        {
            T[] newUnit = new T[this.size];
            Array.ConstrainedCopy(this.unit, 0, newUnit, 0, i);
            Array.ConstrainedCopy(this.unit, i+1, newUnit, i, this.size-(i+1));
            Array.Resize<T>(ref newUnit, Size - 1);
            this.unit = newUnit;
            this.index = this.index - 1;
            this.size = this.size - 1;
        }
    }
}
