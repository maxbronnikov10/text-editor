using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    class ReadonlyListWithIndex<T>
    {
        private int index;
        public readonly IEnumerable<T> List;

        public ReadonlyListWithIndex(int index, IEnumerable<T> list) 
        {
            this.index = index;
            List = list;
        }

        public void MovePrevious()
        {
            if(List.Count() >= 0) index--;
        }

        public void MoveNext()
        {
            if (index + 1 < List.Count()) index++;
        }

        public T GetItem()
        {
            return List.ElementAt(index);
        }

    }
}
