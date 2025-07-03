using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAutoTrade2
{
    public class ConditionData
    {
        public int conditionIndex; //조건식 인덱스
        public string conditionName; //조건식 이름
        
        public ConditionData(int index, string name)
        {
            conditionIndex = index;
            conditionName = name;
        }
    }
}
