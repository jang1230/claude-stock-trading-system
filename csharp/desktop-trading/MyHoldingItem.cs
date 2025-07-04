using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAutoTrade2
{
    public class MyHoldingItem
    {
        public MainForm gMainForm = MainForm.GetInstance();

        public string m_itemCode; // 종목코드
        public string m_itemName; // 종목명
        public double m_buyingAvgPrice; // 매수평균가격
        public double m_upperLimitPrice; // 상한가
        public double m_currentPrice; // 현재가
        public int m_totalQnt; // 매수량
        public double m_rateOfReturn; // 수익률
        public int m_totalPurchaseAmount; // 총매입금
        //public double m_lowlowprice = 0; //저점가격
        public bool m_completeConclusion = false; // 주문접수: false, 체결데이터 수신시: true,
        public int m_orderQnt = 0; // 주문접수 수량
        public bool m_bRetryOrder = false; // 정정주문들어갔는지 여부

        // 매수 관련
        public bool m_bCompletePurchase = false; // 매수상태(true: 매수O, false: 매수X)
        //public string m_sellOrderNumber; // 매도주문번호 -> 사용안함
        public string m_buyingOrderNumber; // 매수주문번호 -> 
        //매도주문번호 및 수익률계산
        public List<ProfitLossCalculate> m_profitLossCalculateList = new List<ProfitLossCalculate>();
        //public double m_averagePrice; // 평균단가
        //public int m_orderAvailableQnt; // 주문가능수량
        public bool m_bPurchased = false; // 현재 매수중인지 확인 변수
        public bool m_brePurchased = false; // 추가매수 들어갔는지 확인 변수
        public bool m_bSold = false; //현재 매도중인지 확인 뱐수
        public double m_finalRateOfReturn = 0; //
        public double m_purchasePerPriceCalciuate = 0; // 매수때 마다 평단계산

        //추매관련배열
        public bool[] m_rePurchaseArray = new bool[5];
        public int m_rePurchaseNumber = -1;

        //ts매도관련배열
        public bool[] m_tsMedoArray = new bool[3]; // ts매도가 이루어졋을때 값을 변환하기위함
        public int m_tsMedoNumber = -1;

        //기본익절관련배열
        public bool[] m_takeProfitArray = new bool[5];
        public int m_takeProfitNumber = -1;
        //기본손절관련배열
        public bool[] m_stopLossArray = new bool[5];
        public int m_stopLossNumber = -1;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////// 전체보유매도에서 저장된 변수 세팅
        // 추가매수 관련 부분
        public int m_reBuyingUsing = 1; // 추가매수 사용 여부
        public int m_reBuyingCount = 0; // 추가매수 횟수
        public double[] m_reBuyingInvestment = new double[5];// 회차별 투자금
        public double[] m_reBuyingPer = new double[5]; // 추매 %

        // 익절
        public int m_takeProfitUsing = 1; // 익절 사용 여부
        public int m_takeProfitCount = 0; // 익절매도횟수(최대5회)
        public double[] m_takeProfitBuyingPricePer = new double[5]; // 익절도달 %
        public double[] m_takeProfitProportion = new double[5]; // 익절매도비중 %

        // 손절
        public int m_stopLossUsing = 1; // 손절 사용 여부
        public int m_stopLossCount = 0; // 손절매도횟수(최대5회)
        public double[] m_stopLossBuyingPricePer = new double[5]; // 손절도달 %
        public double[] m_stopLossProportion = new double[5]; // 손절매도비중 %

        //ts매도
        public int m_tsMedoUsing = 1; // ts매도사용여부
        public int m_tsMedoCount = 0; // ts매도횟수(최대3회)
        public int[] m_tsMedoUsingType = new int[3]; // 0:목표가,1:고점
        public double[] m_tsMedoAchievedPer = new double[3]; // 목표가일경우 달성한% 
        public double[] m_tsMedoPercent = new double[3]; // ts매도하락%
        public double[] m_tsMedoProportion = new double[3]; // ts매도비중%
        public int m_tsProfitPreservation1 = 1;//1차 ts매도 이익보존 사용여부
        public int m_tsProfitPreservation2 = 1;//2차 ts매도 이익보존 사용여부
        public int m_tsProfitPreservation3 = 1;//3차 ts매도 이익보존 사용여부

        public int m_currentRebuyingStep = 0; // 현재 진행 중인 추가매수 차수를 저장하는 변수 (1차부터 시작)
        public int m_currentTsmedoStep = 0; // 현재 진행중인 ts매도 차수를 저장하는 변수
        public int m_currentTakeProfitStep = 0; //현재 진행중인 익절매도 차수 저장변수
        public int m_currentStopLossStep = 0;//현재 진행중인 손절매도 차수 저장변수

        public MyHoldingItem(MyHoldingItemOption mho, string m_itemCode, double m_buyingAvgPrice, double m_currentPrice, int m_totalPurchaseAmount, int m_totalQnt)
        {
            this.m_itemCode = m_itemCode;
            this.m_itemName = gMainForm.KiwoomAPI.GetMasterCodeName(m_itemCode);
            this.m_buyingAvgPrice = m_buyingAvgPrice;
            this.m_currentPrice = m_currentPrice;
            this.m_totalPurchaseAmount = m_totalPurchaseAmount;
            this.m_totalQnt = m_totalQnt;

            //설정값을 세팅한다.
            m_reBuyingUsing = mho.reBuyingUsing;
            m_reBuyingCount = mho.reBuyingCount;
            for(int i =0; i<5; i++)
            {
                m_reBuyingInvestment[i] = mho.reBuyingInvestment[i];
                m_reBuyingPer[i] = mho.reBuyingPer[i];
            }

            m_takeProfitUsing = mho.takeProfitUsing;
            m_takeProfitCount = mho.takeProfitCount;
            for(int i=0; i<5; i++)
            {
                m_takeProfitBuyingPricePer[i] = mho.takeProfitBuyingPricePer[i];
                m_takeProfitProportion[i] = mho.takeProfitProportion[i];
            }

            m_stopLossUsing = mho.stopLossUsing;
            m_stopLossCount = mho.stopLossCount;
            for(int i=0; i<5; i++)
            {
                m_stopLossBuyingPricePer[i] = mho.stopLossBuyingPricePer[i];
                m_stopLossProportion[i] = mho.stopLossProportion[i];
            }

            m_tsMedoUsing = mho.tsMedoUsing;
            m_tsMedoCount = mho.tsMedoCount;
            for(int i=0; i<3; i++)
            {
                m_tsMedoUsingType[i] = mho.tsMedoUsingType[i];
                m_tsMedoAchievedPer[i] = mho.tsMedoAchievedPer[i];
                m_tsMedoPercent[i] = mho.tsMedoPercent[i];
                m_tsMedoProportion[i] = mho.tsMedoProportion[i];
            }
            m_tsProfitPreservation1 = mho.tsProfitPreservation1;
            m_tsProfitPreservation2 = mho.tsProfitPreservation2;
            m_tsProfitPreservation3 = mho.tsProfitPreservation3;
        }
        public void reSetOptionData(MyHoldingItemOption mho)
        {
            m_reBuyingUsing = mho.reBuyingUsing;
            m_reBuyingCount = mho.reBuyingCount;
            for (int i = 0; i < 5; i++)
            {
                m_reBuyingInvestment[i] = mho.reBuyingInvestment[i];
                m_reBuyingPer[i] = mho.reBuyingPer[i];
            }

            m_takeProfitUsing = mho.takeProfitUsing;
            m_takeProfitCount = mho.takeProfitCount;
            for (int i = 0; i < 5; i++)
            {
                m_takeProfitBuyingPricePer[i] = mho.takeProfitBuyingPricePer[i];
                m_takeProfitProportion[i] = mho.takeProfitProportion[i];
            }

            m_stopLossUsing = mho.stopLossUsing;
            m_stopLossCount = mho.stopLossCount;
            for (int i = 0; i < 5; i++)
            {
                m_stopLossBuyingPricePer[i] = mho.stopLossBuyingPricePer[i];
                m_stopLossProportion[i] = mho.stopLossProportion[i];
            }

            m_tsMedoUsing = mho.tsMedoUsing;
            m_tsMedoCount = mho.tsMedoCount;
            for (int i = 0; i < 3; i++)
            {
                m_tsMedoUsingType[i] = mho.tsMedoUsingType[i];
                m_tsMedoAchievedPer[i] = mho.tsMedoAchievedPer[i];
                m_tsMedoPercent[i] = mho.tsMedoPercent[i];
                m_tsMedoProportion[i] = mho.tsMedoProportion[i];
            }
            m_tsProfitPreservation1 = mho.tsProfitPreservation1;
            m_tsProfitPreservation2 = mho.tsProfitPreservation2;
            m_tsProfitPreservation3 = mho.tsProfitPreservation3;

            m_currentRebuyingStep = 0; // 현재 진행 중인 추가매수 차수를 저장하는 변수 (1차부터 시작)
            m_currentTsmedoStep = 0; // 현재 진행중인 ts매도 차수를 저장하는 변수
            m_currentTakeProfitStep = 0; //현재 진행중인 익절매도 차수 저장변수
            m_currentStopLossStep = 0;//현재 진행중인 손절매도 차수 저장변수
    }

        public int getRePurchaseCount()
        {
            int _count = 0;
            for (int i = 0; i < 5; i++)
            {
                if (m_rePurchaseArray[i])
                    _count++;
            }
            return _count;
        }
    }
}
