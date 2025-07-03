using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace StockAutoTrade2
{
    public class FileIO
    {
        // 인스턴스 객체
        private static FileIO gFileIOInstance;
        // 저장 위치 및 저장파일명
        private string conditionPath = @"condition\ConditionList.txt";
        private string HoldingitemPath = @"condition\Holdingitem.txt";
        private string itemPath = @"item\Item.txt";
        private string tradingPath = @"item\Trading.txt";
        private string lastconditionPath = @"lastcondition\lastConditionList.txt";

        public static FileIO GetInstance()
        {
            if (gFileIOInstance == null)
                gFileIOInstance = new FileIO();

            return gFileIOInstance;
        }

        public string ItemPath
        {
            get { return itemPath; }
        }

        //-------------------------------
        // 하단은 lastconditionPath(conditionsettingdialog폼이 꺼질때 저장하기 위한 파일)
        // 저장파일 존재 확인 후 없으면 파일 생성
        public bool setlastConditionFileCreate()
        {
            FileInfo fileinfo = null;
            fileinfo = new FileInfo(lastconditionPath);
            if (!fileinfo.Exists)
            {
                FileStream fs = fileinfo.Create();
                fs.Close();
                return true;
            }
            return false;
        }

        ////전체보유매도///////////

        public bool setlastHoldingItemFileCreate()
        {
            FileInfo fileinfo = null;
            fileinfo = new FileInfo(HoldingitemPath);
            if (!fileinfo.Exists)
            {
                FileStream fs = fileinfo.Create();
                fs.Close();
                return true;
            }
            return false;
        }

        // 전체보유매도 저장하기
        public void saveHoldingitem(string _strInvestment, string _strReBuying, string _strInvestmentPrice, string _strTakeProfit, string _strStopLoss, string _strTsmedo)
        {
            FileInfo fileInfo = new FileInfo(HoldingitemPath);
            //전체 텍스트를 불러온다.
            string[] tempConditionList = File.ReadAllLines(HoldingitemPath);
            //저장된 라인수 / 조건식 매매 방식 데이터 저장갯수 = 조건식 매매 방식 갯수
            int conditionListCount = tempConditionList.Length / (int)Save.holdingItem;

            if (fileInfo.Exists) // 파일이 존재한다면
            {
                if (conditionListCount != 0) //파일의 내용이 존재한다면
                {
                    // 덮어쓰기모드로오픈
                    using (StreamWriter file = new StreamWriter(HoldingitemPath, false))
                    {
                        // 각각의 저장 데이터를 WriteLine으로 한줄식 문자열로 쓴다.
                        file.WriteLine(_strInvestment);
                        file.WriteLine(_strReBuying);
                        file.WriteLine(_strInvestmentPrice);
                        file.WriteLine(_strTakeProfit);
                        file.WriteLine(_strStopLoss);
                        file.WriteLine(_strTsmedo);

                    }
                }
                if (conditionListCount == 0) //파일의 내용이 없다면
                {
                    //파일을 추가모드로 오픈(새로작성)
                    using (StreamWriter file = new StreamWriter(HoldingitemPath, true))
                    {
                        // 각각의 저장 데이터를 WriteLine으로 한줄식 문자열로 쓴다.
                        file.WriteLine(_strInvestment);
                        file.WriteLine(_strReBuying);
                        file.WriteLine(_strInvestmentPrice);
                        file.WriteLine(_strTakeProfit);
                        file.WriteLine(_strStopLoss);
                        file.WriteLine(_strTsmedo);

                    }
                }
                MessageBox.Show("설정이 저장 되었습니다.");
            }
        }
        public string[] getHoldingItemListData()
        {
            FileInfo fileInfo = new FileInfo(HoldingitemPath);
            string[] tempHoldingItemList = null;
            if(fileInfo.Exists)
            {
                tempHoldingItemList = File.ReadAllLines(HoldingitemPath);
            }
            return tempHoldingItemList;
        }
        
        // 조건식 매매방식 저장하기
        public void savelastConditionList(string _conditionIndex, string _strInvestment, string _strInvestmentPrice, string _strBuying,
                                                      string _strReBuying, string _strTakeProfit, string _strStopLoss, string _strTsmedo)
        {
            FileInfo fileInfo = new FileInfo(lastconditionPath);
            //전체 텍스트를 불러온다.
            string[] tempConditionList = File.ReadAllLines(lastconditionPath);
            //저장된 라인수 / 조건식 매매 방식 데이터 저장갯수 = 조건식 매매 방식 갯수
            int conditionListCount = tempConditionList.Length / (int)Save.lastCondition;

            if (fileInfo.Exists) // 파일이 존재한다면
            {
                if (conditionListCount != 0) //파일의 내용이 존재한다면
                {
                    // 덮어쓰기모드로오픈
                    using (StreamWriter file = new StreamWriter(lastconditionPath, false))
                    {
                        // 각각의 저장 데이터를 WriteLine으로 한줄식 문자열로 쓴다.
                        file.WriteLine(_conditionIndex);
                        file.WriteLine(_strInvestment);
                        file.WriteLine(_strInvestmentPrice);
                        file.WriteLine(_strBuying);
                        file.WriteLine(_strReBuying);
                        file.WriteLine(_strTakeProfit);
                        file.WriteLine(_strStopLoss);
                        file.WriteLine(_strTsmedo);

                    }
                }
                if (conditionListCount == 0) //파일의 내용이 없다면
                {
                    //파일을 추가모드로 오픈(새로작성)
                    using (StreamWriter file = new StreamWriter(lastconditionPath, true))
                    {
                        // 각각의 저장 데이터를 WriteLine으로 한줄식 문자열로 쓴다.
                        file.WriteLine(_conditionIndex);
                        file.WriteLine(_strInvestment);
                        file.WriteLine(_strInvestmentPrice);
                        file.WriteLine(_strBuying);
                        file.WriteLine(_strReBuying);
                        file.WriteLine(_strTakeProfit);
                        file.WriteLine(_strStopLoss);
                        file.WriteLine(_strTsmedo);

                    }
                }
            }
        }
        public string[] getlastConditionListData()
        {
            FileInfo fileInfo = new FileInfo(lastconditionPath);
            string[] tempConditionList = null;
            if (fileInfo.Exists)
            {
                tempConditionList = File.ReadAllLines(lastconditionPath);
            }
            return tempConditionList;
        }

        // 하단은 conditionPath(조건식설정파일)-------------------------------------------------------------------------------
        // 저장파일 존재 확인 후 없으면 파일 생성
        public bool setConditionFileCreate()
        {
            FileInfo fileinfo = null;
            fileinfo = new FileInfo(conditionPath);
            if (!fileinfo.Exists)
            {
                FileStream fs = fileinfo.Create();
                fs.Close();
                return true;
            }
            return false;
        }
        // 조건식 매매방식 저장하기
        public void saveConditionList(string _conditionIndex, string _strInvestment, string _strInvestmentPrice, string _strBuying,
                                                      string _strReBuying, string _strTakeProfit, string _strStopLoss, string _strTsmedo)
        {
            FileInfo fileInfo = new FileInfo(conditionPath);
            if (fileInfo.Exists) // 파일이 존재한다면
            {
                //전체 텍스트를 불러온다.
                string[] tempConditionList = File.ReadAllLines(conditionPath);
                //저장된 라인수 / 조건식 매매 방식 데이터 저장갯수 = 조건식 매매 방식 갯수
                int conditionListCount = tempConditionList.Length / (int)Save.Condition;
                // 조건식 이름과 비교해서 같은 것이 있으면 메시지박스 출력 후 return
                for (int i = 0; i < conditionListCount; i++)
                {
                    if (tempConditionList[i * (int)Save.Condition].Equals(_conditionIndex))
                    {
                        MessageBox.Show("동일한 조건식이 있습니다.");
                        return;
                    }
                }
            }
            //같은 것이 없는 경우 파일을 추가모드로 오픈
            using (StreamWriter file = new StreamWriter(conditionPath, true))
            {
                // 각각의 저장 데이터를 WriteLine으로 한줄식 문자열로 쓴다.
                file.WriteLine(_conditionIndex);
                file.WriteLine(_strInvestment);
                file.WriteLine(_strInvestmentPrice);
                file.WriteLine(_strBuying);
                file.WriteLine(_strReBuying);
                file.WriteLine(_strTakeProfit);
                file.WriteLine(_strStopLoss);
                file.WriteLine(_strTsmedo);

            }
            MessageBox.Show("조건식이 저장 되었습니다.");
        }
        //조건식 매매방식 삭제하기
        public void deleteConditionList(int deleteIndex)
        {
            FileInfo fileInfo = new FileInfo(conditionPath);
            if (fileInfo.Exists)
            {
                //전체 텍스트를 불러온다.
                string[] tempConditionList = File.ReadAllLines(conditionPath);
                //저장된 라인수 / 조건식 매매 방식 데이터 저장 갯수 = 조건식 매매 방식 갯수
                int conditionListCount = tempConditionList.Length / (int)Save.Condition;

                //덮어쓰기 모드로 파일을 오픈한다.
                using (StreamWriter file = new StreamWriter(conditionPath, false))
                {
                    for (int i = 0; i < conditionListCount; i++)
                    {
                        //삭제할 인덱스와 동일하면 파일에 쓰지 않는다. / 기존의 모든 텍스트를 불러와서 삭제 인덱스를 제외한 텍스트를 다시 작성하는개념 -> 삭제지만 삭제가아님
                        if (i != deleteIndex)
                        {
                            //이전에 받아온 데이터를 WriteLine으로 한줄씩 문자열로 쓴다. 
                            for (int j = 0; j < (int)Save.Condition; j++)
                            {
                                file.WriteLine(tempConditionList[(i * (int)Save.Condition) + j]);
                            }
                        }
                    }
                }
            }
        }
        //조건식 매매 방식 수정하기
        public void editConditionList(int editNumber, string _conditionIndex, string _strInvestment, string _strInvestmentPrice,
                                                     string _strBuying, string _strReBuying, string _strTakeProfit, string _strStopLoss, string _strTsmedo)
        {
            FileInfo fileInfo = new FileInfo(conditionPath);
            if (fileInfo.Exists)
            {
                //전체 텍스트를 불러온다.
                string[] tempConditionList = File.ReadAllLines(conditionPath);
                // 저장된 라인수 / 조건식 매매 방식 데이터 저장 갯수 = 조건식 매매 방식 갯수
                int conditionListCount = tempConditionList.Length / (int)Save.Condition;

                //덮어쓰기 모드를 파일로 오픈한다.
                using (StreamWriter file = new StreamWriter(conditionPath, false))
                {
                    for (int i = 0; i < conditionListCount; i++)
                    {
                        // 수정할 인덱스와 같으면 인자로 받은 데이터를 파일에 써준다.
                        if (i == editNumber)
                        {
                            file.WriteLine(_conditionIndex);
                            file.WriteLine(_strInvestment);
                            file.WriteLine(_strInvestmentPrice);
                            file.WriteLine(_strBuying);
                            file.WriteLine(_strReBuying);
                            file.WriteLine(_strTakeProfit);
                            file.WriteLine(_strStopLoss);
                            file.WriteLine(_strTsmedo);
                        }
                        else
                        {
                            //이전에 받아온 데이터를 WriteLine으로 한줄씩 문자열로 쓴다.
                            for (int j = 0; j < (int)Save.Condition; j++)
                            {
                                file.WriteLine(tempConditionList[(i * (int)Save.Condition) + j]);
                            }
                        }
                    }
                }
            }
        }
        // 저장 파일 있는지 확인
        public int getSameConditionIndex(string conditionName)
        {
            FileInfo fileInfo = new FileInfo(conditionPath);
            if (fileInfo.Exists) // 저장파일 있는지 확인
            {
                //전체텍스트를 불러온다
                string[] tempConditionList = File.ReadAllLines(conditionPath);
                //저장된 라인수 / 조건식 매매 방식 데이터 저장 개수 = 조건식 매매 방식 개수
                int conditionListCount = tempConditionList.Length / (int)Save.Condition;

                for (int i = 0; i < conditionListCount; i++)
                {
                    //같은 조건식 이름이 있으면 저장 위치(i)리턴
                    if (tempConditionList[i * (int)Save.Condition].Equals(conditionName))
                    {
                        return i;
                    }
                }
            }
            //없으면 -1 리턴
            return -1;
        }
        // ConditionList 파일 문자열 전부다 읽어오기
        public string[] getConditionListData()
        {
            FileInfo fileInfo = new FileInfo(conditionPath);
            string[] tempConditionList = null;
            if (fileInfo.Exists)
            {
                tempConditionList = File.ReadAllLines(conditionPath);
            }
            return tempConditionList;
        }

        // 하단은 tradingpath(매매수익률파일)-------------------------------------------------------------------------------
        // 매매수익률 파일 생성(파일이 없을경우 생성 , 있을경우 false값으로 return)
        public bool setTradingFileCreate()
        {
            FileInfo fileInfo = null;
            fileInfo = new FileInfo(tradingPath);
            if (!fileInfo.Exists)
            {
                FileStream fs = fileInfo.Create();
                fs.Close();
                return true;
            }
            return false;
        }

        //매매수익률 데이터 저장
        public void saveTradingList(string date, string itemCode, double totalPurchaseAmount, int totalQnt,
                                                    int sellPrice, double totalEvaluationProfitLoss, double rateOfReturn)
        {
            // 덮어쓰기모드로 Trading.txt 파일 열기
            using (StreamWriter file = new StreamWriter(tradingPath, true))
            {
                file.Write(date);
                file.Write(itemCode);
                file.Write(totalPurchaseAmount.ToString());
                file.Write(totalQnt.ToString());
                file.Write(sellPrice.ToString());
                file.Write(totalEvaluationProfitLoss.ToString());
                file.Write(rateOfReturn.ToString());
            }
        }
        //매매수익률 데이터 가져오기 -> Trading.txt 전체 문자열 읽어오기
        public string[] getTradingListData()
        {
            FileInfo fileInfo = new FileInfo(tradingPath);
            string[] tempTradingList = null;
            if (fileInfo.Exists)
            {
                tempTradingList = File.ReadAllLines(tradingPath);
            }
            return tempTradingList;
        }
        // 하단은 로그 파일 저장-------------------------------------------------------------------------------
        public bool setLogFileCreate()
        {
            string strToday = DateTime.Today.ToString("yyyyMMdd");
            string path = @"Log\log_" + strToday + ".txt";

            FileInfo fileInfo = null;
            fileInfo = new FileInfo(path);
            if(!fileInfo.Exists)
            {
                FileStream fs = fileInfo.Create();
                fs.Close();
                return true;
            }
            return false;
        }
        public void WriteLogFile(string txt)
        {
            string strToday = DateTime.Today.ToString("yyyyMMdd");
            string path = @"Log\log_" + strToday + ".txt";
            string cTime = DateTime.Now.ToString("HH:mm:ss.fff");
            string str = "[" + cTime + "]" + txt;
            if(File.Exists(path))
            {
                StreamWriter writer;
                writer = File.AppendText(path);
                writer.WriteLine(str);
                writer.Close();
            }
        }
        // 하단은 itemPath(종목이랑 검색식 매칭 -> 프로그램을 다시켯을때)-------------------------------------------------------------------------------
        public bool setItemFileCreate()
        {
            FileInfo fileInfo = null;
            fileInfo = new FileInfo(itemPath);
            if (!fileInfo.Exists)
            {
                FileStream fs = fileInfo.Create();
                fs.Close();
                return true;
            }
            return false;
        }

        public int checkSameItemCode(string itemCode)
        {
            FileInfo fileInfo = new FileInfo(itemPath);
            if (fileInfo.Exists)
            {
                string[] tempItemList = File.ReadAllLines(itemPath);
                int itemListCount = tempItemList.Length / (int)Save.Item;

                for (int i = 0; i < itemListCount; i++)
                {
                    if (tempItemList[(i * (int)Save.Item)].Equals(itemCode))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public string[] getItemListData()
        {
            FileInfo fileInfo = new FileInfo(itemPath);
            string[] tempItemList = null;
            if (fileInfo.Exists)
            {
                tempItemList = File.ReadAllLines(itemPath);
            }
            return tempItemList;
        }

        public void saveItem(string itemCode, string conditionName, int conditionIndex, double transferPrice, string rePruchaseArray, int _currentRebuyingStep)
        {
            using (StreamWriter file = new StreamWriter(itemPath, true))
            {
                file.WriteLine(itemCode);
                file.WriteLine(conditionName);
                file.WriteLine(conditionIndex.ToString());
                file.WriteLine(transferPrice.ToString());
                file.WriteLine(rePruchaseArray);
                file.WriteLine(_currentRebuyingStep);
            }
        }
        public void mainForm_saveItem(string itemCode, string conditionName, int conditionIndex, double transferPrice, string rePruchaseArray, int _currentRebuyingStep)
        {
            using (StreamWriter file = new StreamWriter(itemPath, true))
            {
                file.WriteLine(itemCode);
                file.WriteLine(conditionName);
                file.WriteLine(conditionIndex.ToString());
                file.WriteLine(transferPrice.ToString());
                file.WriteLine(rePruchaseArray);
                file.WriteLine(_currentRebuyingStep);
            }
        }

        public void editItem(string itemCode, string conditionName, int conditionIndex, double transferPrice, string rePruchaseArray)
        {
            FileInfo fileInfo = new FileInfo(itemPath);
            if (fileInfo.Exists)
            {
                string[] tempItemList = File.ReadAllLines(itemPath);
                int itemListCount = tempItemList.Length / (int)Save.Item;

                using (StreamWriter file = new StreamWriter(itemPath, false))
                {
                    for (int i = 0; i < itemListCount; i++)
                    {
                        if (tempItemList[(i * (int)Save.Item)].Equals(itemCode))
                        {
                            file.WriteLine(itemCode);
                            file.WriteLine(conditionName);
                            file.WriteLine(conditionIndex.ToString());
                            file.WriteLine(transferPrice.ToString());
                            file.WriteLine(rePruchaseArray);
                        }
                        else
                        {
                            for (int j = 0; j < (int)Save.Item; j++)
                            {
                                file.WriteLine(tempItemList[(i * (int)Save.Item) + j]);
                            }
                        }
                    }
                }
            }
        }
        public void deleteItem(string itemCode)
        {
            int deleteNum = -1;
            string[] tempItemList = File.ReadAllLines(itemPath);
            int itemListCount = tempItemList.Length / (int)Save.Item;
            FileInfo fileInfo = new FileInfo(itemPath);

            if (fileInfo.Exists)
            {
                for (int i = 0; i < itemListCount; i++)
                {
                    if (tempItemList[i * (int)Save.Item].Equals(itemCode))
                    {
                        deleteNum = i;
                        break;
                    }
                }

                if (deleteNum != -1)
                {
                    using (StreamWriter file = new StreamWriter(itemPath, false))
                    {
                        for (int i = 0; i < itemListCount; i++)
                        {
                            if (i != deleteNum)
                            {
                                for (int j = 0; j < (int)Save.Item; j++)
                                {
                                    file.WriteLine(tempItemList[(i * (int)Save.Item) + j]);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
