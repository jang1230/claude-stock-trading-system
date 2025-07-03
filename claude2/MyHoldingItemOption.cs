using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAutoTrade2
{
    public class MyHoldingItemOption
    {
        // mainform 인스턴스
        public MainForm gMainForm = MainForm.GetInstance();

        public string division = "보유"; // 구분값.
        public bool CheckFirstRun = false; // 최초 실행여부

        public string account; // 매매 계좌번호

        // 추가매수 관련 부분
        public int reBuyingUsing = 1; // 추가매수 사용 여부
        public int reBuyingCount = 0; // 추가매수 횟수
        public double[] reBuyingInvestment = new double[5];// 회차별 투자%
        public double[] reBuyingPer = new double[5]; // 추매 %

        // 익절
        public int takeProfitUsing = 1; // 익절 사용 여부
        public int takeProfitCount = 0; // 익절매도횟수(최대5회)
        public double[] takeProfitBuyingPricePer = new double[5]; // 익절도달 %
        public double[] takeProfitProportion = new double[5]; // 익절매도비중 %

        // 손절
        public int stopLossUsing = 1; // 손절 사용 여부
        public int stopLossCount = 0; // 손절매도횟수(최대5회)
        public double[] stopLossBuyingPricePer = new double[5]; // 손절도달 %
        public double[] stopLossProportion = new double[5]; // 손절매도비중 %

        //ts매도
        public int tsMedoUsing = 1; // ts매도사용여부
        public int tsMedoCount = 0; // ts매도횟수(최대3회)
        public int[] tsMedoUsingType = new int[3]; // 0:목표가,1:고점
        public double[] tsMedoAchievedPer = new double[3]; // 목표가일경우 달성한% 
        public double[] tsMedoPercent = new double[3]; // ts매도하락%
        public double[] tsMedoProportion = new double[3]; // ts매도비중%
        public int tsProfitPreservation1 = 1;//1차 ts매도 이익보존 사용여부
        public int tsProfitPreservation2 = 1;//2차 ts매도 이익보존 사용여부
        public int tsProfitPreservation3 = 1;//3차 ts매도 이익보존 사용여부

        // 현재 진행상황
        public HoldingItemState holdingitemState;  // ConditionState(Playint, Stop) 진행중, 일시정지
        public string screenNumber; // 화면번호(고유번호)

        // 현재 조건검색 state
        public HoldingItemState m_holdingitemState = HoldingItemState.Stop;
        // 보유종목 저장 리스트
        public List<MyHoldingItem> MyHoldingItemList = new List<MyHoldingItem>();
        
        public MyHoldingItemOption(string strInvestment, string strInvestmentPrice, string strRebuying, string strTakeProfit, string strStopLoss, string strTsmedo, string screenNumber)
        {

            string[] _strInvestment, _strInvestmentPrice, _strRebuying, _strTakeProfit, _strStopLoss, _strTsmedo;

            _strInvestment = strInvestment.Split(';');
            //_strInvestment[0] = account
            account = _strInvestment[0]; // 계좌번호

            ////////////////////////////////////////////////////////////////////////////// 추가매수 설정 //////////////////////////////////////////////////////////////////////////////
            _strRebuying = strRebuying.Split(';');
            //_strRebuying[0] = reBuyingUsing
            //_strRebuying[1] = reBuyingCount
            //_strRebuying[2] = reBuyingInvestment[0]
            //_strRebuying[3] = reBuyingInvestment[1]
            //_strRebuying[4] = reBuyingInvestment[2]
            //_strRebuying[5] = reBuyingInvestment[3]
            //_strRebuying[6] = reBuyingInvestment[4]
            //_strRebuying[8] = reBuyingPer[0]
            //_strRebuying[9] = reBuyingPer[1]
            //_strRebuying[10] = reBuyingPer[2]
            //_strRebuying[11] = reBuyingPer[3]
            //_strRebuying[12] = reBuyingPer[4]

            reBuyingUsing = int.Parse(_strRebuying[0]);
            reBuyingCount = int.Parse(_strRebuying[1]);
            for(int i=0; i<5; i++)
            {
                reBuyingPer[i] = double.Parse(_strRebuying[2 + i]);
            }
            ////////////////////////////////////////
            _strInvestmentPrice = strInvestmentPrice.Split(';');
            //_strInvestmentPrice[0] = reBuyingPer[0]
            //_strInvestmentPrice[1] = reBuyingPer[1]
            //_strInvestmentPrice[2] = reBuyingPer[2]
            //_strInvestmentPrice[3] = reBuyingPer[3]
            //_strInvestmentPrice[4] = reBuyingPer[4]
            for (int i=0; i<5; i++)
            {
                reBuyingInvestment[i] = double.Parse(_strInvestmentPrice[i]);
            }

            //////////////////////////////////////////////////////////////////////////////// 익절 설정 //////////////////////////////////////////////////////////////////////////////
            _strTakeProfit = strTakeProfit.Split(';');
            takeProfitUsing = int.Parse(_strTakeProfit[0]); // 익절 사용 여부            
            takeProfitCount = int.Parse(_strTakeProfit[1]); // 익절횟수

            // 1차 익절 %
            takeProfitBuyingPricePer[0] = double.Parse(_strTakeProfit[2]);
            // 1차 익절매도 비중 %
            takeProfitProportion[0] = double.Parse(_strTakeProfit[3]);
            // 2차 익절 %
            takeProfitBuyingPricePer[1] = double.Parse(_strTakeProfit[4]);
            // 2차 익절매도 비중 %
            takeProfitProportion[1] = double.Parse(_strTakeProfit[5]);
            // 3차 익절 %
            takeProfitBuyingPricePer[2] = double.Parse(_strTakeProfit[6]);
            // 3차 익절매도 비중 %
            takeProfitProportion[2] = double.Parse(_strTakeProfit[7]);
            // 4차 익절 %
            takeProfitBuyingPricePer[3] = double.Parse(_strTakeProfit[8]);
            // 4차 익절매도 비중 %
            takeProfitProportion[3] = double.Parse(_strTakeProfit[9]);
            // 5차 익절 %
            takeProfitBuyingPricePer[4] = double.Parse(_strTakeProfit[10]);
            // 5차 익절매도 비중 %
            takeProfitProportion[4] = double.Parse(_strTakeProfit[11]);

            //////////////////////////////////////////////////////////////////////////////// 손절 설정 //////////////////////////////////////////////////////////////////////////////
            _strStopLoss = strStopLoss.Split(';');
            stopLossUsing = int.Parse(_strStopLoss[0]); // 손절 사용 여부
            stopLossCount = int.Parse(_strStopLoss[1]);//손절횟수(최대5회)

            // 1차 손절 %
            stopLossBuyingPricePer[0] = double.Parse(_strStopLoss[2]);
            // 1차 손절매도 비중 %
            stopLossProportion[0] = double.Parse(_strStopLoss[3]);
            // 2차 손절 %
            stopLossBuyingPricePer[1] = double.Parse(_strStopLoss[4]);
            // 2차 손절매도 비중 %
            stopLossProportion[1] = double.Parse(_strStopLoss[5]);
            // 3차 손절 %
            stopLossBuyingPricePer[2] = double.Parse(_strStopLoss[6]);
            // 3차 손절매도 비중 %
            stopLossProportion[2] = double.Parse(_strStopLoss[7]);
            // 4차 손절 %
            stopLossBuyingPricePer[3] = double.Parse(_strStopLoss[8]);
            // 4차 손절매도 비중 %
            stopLossProportion[3] = double.Parse(_strStopLoss[9]);
            // 5차 손절 %
            stopLossBuyingPricePer[4] = double.Parse(_strStopLoss[10]);
            // 5차 손절매도 비중 %
            stopLossProportion[4] = double.Parse(_strStopLoss[11]);

            //////////////////////////////////////////////////////////////////////////////// ts매도 설정 //////////////////////////////////////////////////////////////////////////////
            _strTsmedo = strTsmedo.Split(';');
            // tsmedo[0] tsMedoUsing -> ts매도 사용여부
            // tsmedo[1] tsMedoCount -> ts매도횟수(최대3회)
            // tsmedo[2] tsMedoUsingtype ->  0:목표가, 1:고점
            // tsMedoAchievedPer = new double[2]; // 목표가일경우 달성한%
            // tsmedo[3] tsmedoPercent -> ts매도 하락%
            // tsmedo[4] tsmedoProportion -> ts매도비중
            tsMedoUsing = int.Parse(_strTsmedo[0]); // ts매도사용여부
            tsMedoCount = int.Parse(_strTsmedo[1]); // ts매도횟수(최대3회) 

            tsMedoUsingType[0] = int.Parse(_strTsmedo[2]); // 1차 0:목표가, 1:고점
            tsMedoAchievedPer[0] = double.Parse(_strTsmedo[3]); // 목표가일경우 1차 달성한%
            tsMedoPercent[0] = double.Parse(_strTsmedo[4]); // ts매도 1회 하락%
            tsMedoProportion[0] = double.Parse(_strTsmedo[5]); // ts매도 1회 매도비중

            tsMedoUsingType[1] = int.Parse(_strTsmedo[6]); // 1차 0:목표가, 1:고점
            tsMedoAchievedPer[1] = double.Parse(_strTsmedo[7]); // 목표가일경우 1차 달성한%
            tsMedoPercent[1] = double.Parse(_strTsmedo[8]); // ts매도 2회 하락%
            tsMedoProportion[1] = double.Parse(_strTsmedo[9]); // ts매도 2회 매도비중

            tsMedoUsingType[2] = int.Parse(_strTsmedo[10]); // 1차 0:목표가, 1:고점
            tsMedoAchievedPer[2] = double.Parse(_strTsmedo[11]); // 목표가일경우 1차 달성한%
            tsMedoPercent[2] = double.Parse(_strTsmedo[12]); // ts매도 3회 하락%
            tsMedoProportion[2] = double.Parse(_strTsmedo[13]); // ts매도 3회 매도비중

            tsProfitPreservation1 = int.Parse(_strTsmedo[14]); // 1차 ts매도 이익보존여부
            tsProfitPreservation2 = int.Parse(_strTsmedo[15]); // 2차 ts매도 이익보존여부
            tsProfitPreservation3 = int.Parse(_strTsmedo[16]); // 3차 ts매도 이익보존여부

            this.screenNumber = screenNumber; // 화면번호(고유번호)
        }
        // 저장하기를 눌럿을때 새로 설정을 덮어씌운다.
        public void reSetMyHoldingItemOption(string strInvestment, string strInvestmentPrice, string strRebuying, string strTakeProfit, string strStopLoss, string strTsmedo)
        {

            string[] _strInvestment, _strInvestmentPrice, _strRebuying, _strTakeProfit, _strStopLoss, _strTsmedo;

            _strInvestment = strInvestment.Split(';');
            //_strInvestment[0] = account
            account = _strInvestment[0]; // 계좌번호

            ////////////////////////////////////////////////////////////////////////////// 추가매수 설정 //////////////////////////////////////////////////////////////////////////////
            _strRebuying = strRebuying.Split(';');
            //_strRebuying[0] = reBuyingUsing
            //_strRebuying[1] = reBuyingCount
            //_strRebuying[2] = reBuyingInvestment[0]
            //_strRebuying[3] = reBuyingInvestment[1]
            //_strRebuying[4] = reBuyingInvestment[2]
            //_strRebuying[5] = reBuyingInvestment[3]
            //_strRebuying[6] = reBuyingInvestment[4]
            //_strRebuying[8] = reBuyingPer[0]
            //_strRebuying[9] = reBuyingPer[1]
            //_strRebuying[10] = reBuyingPer[2]
            //_strRebuying[11] = reBuyingPer[3]
            //_strRebuying[12] = reBuyingPer[4]

            reBuyingUsing = int.Parse(_strRebuying[0]);
            reBuyingCount = int.Parse(_strRebuying[1]);
            for (int i = 0; i < 5; i++)
            {
                reBuyingPer[i] = double.Parse(_strRebuying[2 + i]);
            }
            ////////////////////////////////////////
            _strInvestmentPrice = strInvestmentPrice.Split(';');
            //_strInvestmentPrice[0] = reBuyingPer[0]
            //_strInvestmentPrice[1] = reBuyingPer[1]
            //_strInvestmentPrice[2] = reBuyingPer[2]
            //_strInvestmentPrice[3] = reBuyingPer[3]
            //_strInvestmentPrice[4] = reBuyingPer[4]
            for (int i = 0; i < 5; i++)
            {
                reBuyingInvestment[i] = double.Parse(_strInvestmentPrice[i]);
            }

            //////////////////////////////////////////////////////////////////////////////// 익절 설정 //////////////////////////////////////////////////////////////////////////////
            _strTakeProfit = strTakeProfit.Split(';');
            takeProfitUsing = int.Parse(_strTakeProfit[0]); // 익절 사용 여부            
            takeProfitCount = int.Parse(_strTakeProfit[1]); // 익절횟수

            // 1차 익절 %
            takeProfitBuyingPricePer[0] = double.Parse(_strTakeProfit[2]);
            // 1차 익절매도 비중 %
            takeProfitProportion[0] = double.Parse(_strTakeProfit[3]);
            // 2차 익절 %
            takeProfitBuyingPricePer[1] = double.Parse(_strTakeProfit[4]);
            // 2차 익절매도 비중 %
            takeProfitProportion[1] = double.Parse(_strTakeProfit[5]);
            // 3차 익절 %
            takeProfitBuyingPricePer[2] = double.Parse(_strTakeProfit[6]);
            // 3차 익절매도 비중 %
            takeProfitProportion[2] = double.Parse(_strTakeProfit[7]);
            // 4차 익절 %
            takeProfitBuyingPricePer[3] = double.Parse(_strTakeProfit[8]);
            // 4차 익절매도 비중 %
            takeProfitProportion[3] = double.Parse(_strTakeProfit[9]);
            // 5차 익절 %
            takeProfitBuyingPricePer[4] = double.Parse(_strTakeProfit[10]);
            // 5차 익절매도 비중 %
            takeProfitProportion[4] = double.Parse(_strTakeProfit[11]);

            //////////////////////////////////////////////////////////////////////////////// 손절 설정 //////////////////////////////////////////////////////////////////////////////
            _strStopLoss = strStopLoss.Split(';');
            stopLossUsing = int.Parse(_strStopLoss[0]); // 손절 사용 여부
            stopLossCount = int.Parse(_strStopLoss[1]);//손절횟수(최대5회)

            // 1차 손절 %
            stopLossBuyingPricePer[0] = double.Parse(_strStopLoss[2]);
            // 1차 손절매도 비중 %
            stopLossProportion[0] = double.Parse(_strStopLoss[3]);
            // 2차 손절 %
            stopLossBuyingPricePer[1] = double.Parse(_strStopLoss[4]);
            // 2차 손절매도 비중 %
            stopLossProportion[1] = double.Parse(_strStopLoss[5]);
            // 3차 손절 %
            stopLossBuyingPricePer[2] = double.Parse(_strStopLoss[6]);
            // 3차 손절매도 비중 %
            stopLossProportion[2] = double.Parse(_strStopLoss[7]);
            // 4차 손절 %
            stopLossBuyingPricePer[3] = double.Parse(_strStopLoss[8]);
            // 4차 손절매도 비중 %
            stopLossProportion[3] = double.Parse(_strStopLoss[9]);
            // 5차 손절 %
            stopLossBuyingPricePer[4] = double.Parse(_strStopLoss[10]);
            // 5차 손절매도 비중 %
            stopLossProportion[4] = double.Parse(_strStopLoss[11]);

            //////////////////////////////////////////////////////////////////////////////// ts매도 설정 //////////////////////////////////////////////////////////////////////////////
            _strTsmedo = strTsmedo.Split(';');
            // tsmedo[0] tsMedoUsing -> ts매도 사용여부
            // tsmedo[1] tsMedoCount -> ts매도횟수(최대3회)
            // tsmedo[2] tsMedoUsingtype ->  0:목표가, 1:고점
            // tsMedoAchievedPer = new double[2]; // 목표가일경우 달성한%
            // tsmedo[3] tsmedoPercent -> ts매도 하락%
            // tsmedo[4] tsmedoProportion -> ts매도비중
            tsMedoUsing = int.Parse(_strTsmedo[0]); // ts매도사용여부
            tsMedoCount = int.Parse(_strTsmedo[1]); // ts매도횟수(최대3회) 

            tsMedoUsingType[0] = int.Parse(_strTsmedo[2]); // 1차 0:목표가, 1:고점
            tsMedoAchievedPer[0] = double.Parse(_strTsmedo[3]); // 목표가일경우 1차 달성한%
            tsMedoPercent[0] = double.Parse(_strTsmedo[4]); // ts매도 1회 하락%
            tsMedoProportion[0] = double.Parse(_strTsmedo[5]); // ts매도 1회 매도비중

            tsMedoUsingType[1] = int.Parse(_strTsmedo[6]); // 1차 0:목표가, 1:고점
            tsMedoAchievedPer[1] = double.Parse(_strTsmedo[7]); // 목표가일경우 1차 달성한%
            tsMedoPercent[1] = double.Parse(_strTsmedo[8]); // ts매도 2회 하락%
            tsMedoProportion[1] = double.Parse(_strTsmedo[9]); // ts매도 2회 매도비중

            tsMedoUsingType[2] = int.Parse(_strTsmedo[10]); // 1차 0:목표가, 1:고점
            tsMedoAchievedPer[2] = double.Parse(_strTsmedo[11]); // 목표가일경우 1차 달성한%
            tsMedoPercent[2] = double.Parse(_strTsmedo[12]); // ts매도 3회 하락%
            tsMedoProportion[2] = double.Parse(_strTsmedo[13]); // ts매도 3회 매도비중

            tsProfitPreservation1 = int.Parse(_strTsmedo[14]); // 1차 ts매도 이익보존여부
            tsProfitPreservation2 = int.Parse(_strTsmedo[15]); // 2차 ts매도 이익보존여부
            tsProfitPreservation3 = int.Parse(_strTsmedo[16]); // 3차 ts매도 이익보존여부

            
        }
    }
}
