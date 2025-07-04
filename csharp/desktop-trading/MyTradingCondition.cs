using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAutoTrade2
{
    public class MyTradingCondition
    {
        // mainform 인스턴스
        public MainForm gMainForm = MainForm.GetInstance();

        public string account; // 매매 계좌번호
        public ConditionData conditionData; // 조건식 이름, 번호
        public double itemInvestment; // 종목당 투자금
        public int buyingItemCount; // 매수 종목수
        public int remainingBuyingItemCount; // 남은 매수 종목수
        public int transferItemCount; // 편입 종목수
        public int remainingTransferItemCount; // 남은 편입 종목수

        // 매수 금액 
        public int buyingCount = 0; // 매수횟수
        public double[] buyingInvestment = new double[6];// 회차별 투자금

        // 매수 전략
        public int buyingUsing = 1; // 매수 사용 여부
        public int buyingType = 0; // 0:기본매수, 1:이동평균,2:스토케스틱,3:볼린저밴드,4:엔벨로프
        public int mesuoption1 = 0; // 0: 시장가 , 1: 현재가(지정가)
        public int nontradingtime = 0; // 지정가시 주문후 대기시간
        public int mesuoption2 = 0; // 1: 시장가로정정 , 2: 일괄정정 , 3: 일괄취소
        public int buyingTransferType = 0; // 0:편입시 바로 매수,1:편입가격대비매수
        public double buyingTransferPer = 0; // 편입가격대비매수시 %
        public int buyingTransferUpdown = 0; // 편입가격대비매수시 %에 대한 0: 이하, 1: 이상
        public double buyingTransferPer2 = 0;// 저점대비 매수시 저점%
        public double riseTransferPer2 = 0; // 저점대비 매수시 상승%
        //공통 사용처리 변수
        public int buyingBongType = 0; // 0:월,1:주,2:일,3:분
        public int buyingMinuteType = 0; // 1,3,5분봉 등등 입력
        public int buyingMinuteLineType = 0;// 1,3,5,20이평 등등
        public int buyingMovePriceKindType = 0; //0:단순, 1:지수 
        public double buyingMinuteLineAccessPer = 0;// 근접 %
        public int buyingDistance = 0; // 0:근접, 1:돌파, 2:이탈, 0:이상, 1:이하
        public int buyingStocPeriod1 = 0; //기간
        public int buyingStocPeriod2 = 0; //%K
        public int buyingStocPeriod3 = 0; //%D
        public double buyingBollPeriod = 0; //승수, 엔벨%
        public int buyingLine3Type = 0; // 0:상한선,1:중심선,2:하한선

        // 추매
        // 기본추매
        public int reBuyingType = 0; //0:기본추매, 1:이동평균 근접
        public double[] reBuyingPer = new double[5]; // 추매 %
        // 이동평균근접
        public int reBuyingMovePriceKindType = 0; //0:단순, 1:지수 
        public int reBuyingBongType = 0; // 0:월,1:주,2:일,3:분
        public int reBuyingMinuteType = 0; // 1,3,5분봉 등등 입력
        public int[] reBuyingMinuteLineType = new int[5];// 1,3,5,20이평 등등
        //public double[] reBuyingMovePriceMinuteLineAccessPer = new double[5];// 근접 %        

        // 익절
        public int takeProfitUsing = 1; // 익절 사용 여부
        public int takeProfitType = 0; // 0:기본익절,1:이동평균 근접
        public int takeProfitCount = 0; // 익절매도횟수(최대5회)
        public double[] takeProfitBuyingPricePer = new double[5]; // 익절도달 %
        public double[] takeProfitProportion = new double[5]; // 익절매도비중 %

        // 이동평균 근접, 돌파 공통 사용        
        public int takeProfitMovePriceKindType = 0; //0:단순, 1:지수        
        public int takeProfitBongType = 0; // 0:월,1:주,2:일,3:분
        public int takeProfitMinuteType = 0; // 1,3,5분봉 등등 입력
        public int takeProfitMinuteLineType = 0;// 1,3,5,20이평 등등        
        public double takeProfitLineAccessPer = 0;// 근접 %
        public int takeProfitDistance = 0; // 0:근접,1:돌파,2:이탈, 0:이상, 1:이하
        public int takeProfitStocPeriod1 = 0; //기간
        public int takeProfitStocPeriod2 = 0; //%K
        public int takeProfitStocPeriod3 = 0; //%D
        public double takeProfitBollPeriod = 0; //승수, 엔벨%
        public int takeProfitLine3Type = 0; // 0:상한선,1:중심선,2:하한선
        // 저장값
        // takeProfitUsing, takeProfitType,takeProfitBuyingPricePre,
        // takeProfitMovePriceType, takeProfitMovePriceBongType, takeProfitMovePriceMinuteType, takeProfitMovePriceMinuteLineType, takeProfitMovePriceMinuteLineAccessPer

        // 손절
        public int stopLossUsing = 1; // 손절 사용 여부
        public int stopLossType = 0; // 0:기본손절,1:이동평균 근접
        public int stopLossCount = 0; // 손절매도횟수(최대5회)
        public double[] stopLossBuyingPricePer = new double[5]; // 손절도달 %
        public double[] stopLossProportion = new double[5]; // 손절매도비중 %
        // 이동평균 근접, 이탈 공통 사용        
        public int stopLossMovePriceKindType = 0; //0:단순, 1:지수     
        public int stopLossBongType = 0; // 0:월,1:주,2:일,3:분
        public int stopLossMinuteType = 0; // 1,3,5분봉 등등 입력
        public int stopLossMinuteLineType = 0;// 1,3,5,20이평 등등        
        public double stopLossLineAccessPer = 0;// 근접 %
        public int stopLossDistance = 0; // 0:근접,1:돌파,2:이탈, 0:이상, 1:이하
        public int stopLossStocPeriod1 = 0; //기간
        public int stopLossStocPeriod2 = 0; //%K
        public int stopLossStocPeriod3 = 0; //%D
        public double stopLossBollPeriod = 0; //승수, 엔벨%
        public int stopLossLine3Type = 0; // 0:상한선,1:중심선,2:하한선

        // 저장값
        // stopLossUsing,stopLossType,stopLossBuyingPricePre,
        // stopLossMovePriceType, stopLossMovePriceBongType, stopLossMovePriceMinuteType, stopLossMovePriceMinuteLineType, stopLossMovePriceMinuteLineAccessPer

        public bool usingTakeProfit; // 익절 사용 여부
        public double takeProfitRate; // 익절률

        public bool usingStopLoss; // 손절 사용 여부
        public bool stopLossRateBun5Check; // 5분봉 현재가가 눌렸을때 매수여부
        public double stopLossRate; // 손절률

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
        public ConditionState conditionState;  // ConditionState(Playint, Stop) 진행중, 일시정지
        public string screenNumber; // 화면번호(고유번호)

        // 종목 갯수 및 종목별 투자금 %
        public int itemCount;
        public double[] itemBuyingPer = new double[6];
        // 최초 SendCondition( ) 실행 체크
        public bool m_CheckFirstRun = false;
        // 현재 조건검색 state
        public ConditionState m_conditionState = ConditionState.Stop;
        // 매매종목 저장 리스트
        public List<MyTradingItem> MyTradingItemList = new List<MyTradingItem>();

        public string getConditionName(int num)
        {
            string _name = "";
            for (int i = 0; i < gMainForm.conditionDataList.Count; i++)
            {
                if (gMainForm.conditionDataList[i].conditionIndex.Equals(num))
                    return gMainForm.conditionDataList[i].conditionName;
            }
            return _name;
        }

        /*매수 설정
        //public bool buyingType; // true: 편입즉시매수 , false: 편입단가대비매수
        //public double transferPricePercent; // 편입단가대비 %
        //매도 익절
        public double takeProfitBuyingPricePercent; // 매수단가대비 %
        //매도 손절
        public double stopLossBuyingPricePercent; // 매수단가대비 %
        // 추매타입
        public bool rePurchase;
        // 추매금액
        public int[] rePurchaseMoney = new int[6];
        // 추매%
        public double[] rePurchaseRate = new double[6];
        */



        public MyTradingCondition(string conditionIndex, string strInvestment, 
                                   string strInvestmentPrice, string strBuying, 
                                   string strReBuying, string strTakeProfit, string strStopLoss, string screenNumber
                                   ,string tsmedo)
        {
            string[] _strInvestment, _strInvestmentPrice, _strBuying, _strReBuying, _strTakeProfit, _strStopLoss,_strTsmedo;
            // 조건식인덱스
            int _conditionIndex = -1;
            string conditionName = string.Empty;
            _conditionIndex = int.Parse(conditionIndex);

            //conditionName = gMainForm.gLoginInstance.conditionList[_conditionIndex].Name;
            conditionName = getConditionName(_conditionIndex);

            ConditionData _buyingCondition = new ConditionData(_conditionIndex, conditionName);

            conditionData = _buyingCondition;

            ////////////////////////////////////////////////////////////////////////////// 투자금 설정 //////////////////////////////////////////////////////////////////////////////
            _strInvestment = strInvestment.Split(';');
            //_strInvestment[0] account
            // _strInvestment[1] conditionName
            // _strInvestment[2] conditionIndex 
            // _strInvestment[3] buyingItmeCount 
            // _strInvestment[4] itemInvestment
            account = _strInvestment[0]; // 계좌번호
            buyingItemCount = int.Parse(_strInvestment[3]); // 매수 종목 수
            itemInvestment = double.Parse(_strInvestment[4]); // 종목당 투자금액
            transferItemCount = int.Parse(_strInvestment[5]); // 편입 종목 수
            remainingBuyingItemCount = buyingItemCount;
            remainingTransferItemCount = transferItemCount;

            ////////////////////////////////////////////////////////////////////////// 매수금액설정 ///////////////////////////////////////////////////////////////////////////////////
            _strInvestmentPrice = strInvestmentPrice.Split(';');
            //_strInvestmentPrice[0] buyingCount 
            //_strInvestmentPrice[1] investMoney_1 
            //_strInvestmentPrice[2] investMoney_2 
            //_strInvestmentPrice[3] investMoney_3 
            //_strInvestmentPrice[4] investMoney_4 
            //_strInvestmentPrice[5] investMoney_5 
            //_strInvestmentPrice[6] investMoney_6;
            buyingCount = Int32.Parse(_strInvestmentPrice[0]);
            for (int i = 0; i < 6; i++)
            {
                buyingInvestment[i] = double.Parse(_strInvestmentPrice[1 + i]);
            }

            //////////////////////////////////////////////////////////////////////////////// 매수 설정 //////////////////////////////////////////////////////////////////////////////            
            _strBuying = strBuying.Split(';');
            // _strBuying[0] buyingUsing.ToString() 
            // _strBuying[1] buyingType.ToString() 
            // _strBuying[2] buyingTransferType.ToString() 
            // _strBuying[3] buyingTransferPer.ToString() 
            // _strBuying[4] buyingTransferUpdown.ToString()
            // _strBuying[5] buyingMovePriceKindType.ToString() 
            // _strBuying[6] buyingBongType.ToString() 
            // _strBuying[7] buyingMinuteType.ToString() 
            // _strBuying[8] buyingMinuteLineType.ToString() 
            // _strBuying[9] buyingMinuteLineAccessPer.ToString() 
            // _strBuying[10] buyingDistance.ToString()
            // _strBuying[11] buyingStocPeriod1.ToString() 
            // _strBuying[12] buyingStocPeriod2.ToString() 
            // _strBuying[13] buyingStocPeriod3.ToString() 
            // _strBuying[14] buyingBollPeriod.ToString() 
            // _strBuying[15] buyingLine3Type.ToString();
            // _strBuying[16] buyingTransferPer2.ToString();
            // _strBuying[17] riseTransferPer2.ToString();
            // _strBuying[18] mesuoption1 .ToString();
            // _strBuying[19] nontradingtime.ToString();
            // _strBuying[20] mesuoption2.ToString();
            buyingUsing = Int32.Parse(_strBuying[0]);// 매수사용체크박스
            buyingType = Int32.Parse(_strBuying[1]);
            if (buyingType == 0) // 기본매수
            {
                mesuoption1 = int.Parse(_strBuying[18]);
                buyingTransferType = Int32.Parse(_strBuying[2]);
                if (buyingTransferType == 0) // 편입즉시매수
                {
                    if(mesuoption1 == 1) // 지정가
                    {
                        nontradingtime = int.Parse(_strBuying[19]);
                        mesuoption2 = int.Parse(_strBuying[20]);
                    }
                }
                else if (buyingTransferType == 1) // 편입후 n% 매수
                {
                    // 대비 n%
                    buyingTransferPer = double.Parse(_strBuying[3]);
                    // 이상, 이하
                    buyingTransferUpdown = Int32.Parse(_strBuying[4]);

                    if (mesuoption1 == 1) // 지정가
                    {
                        nontradingtime = int.Parse(_strBuying[19]);
                        mesuoption2 = int.Parse(_strBuying[20]);
                    }
                }
                else if (buyingTransferType == 2) // 저점대비 상승매수
                {
                    buyingTransferPer2 = double.Parse(_strBuying[16]);
                    riseTransferPer2 = double.Parse(_strBuying[17]);

                    if (mesuoption1 == 1) // 지정가
                    {
                        nontradingtime = int.Parse(_strBuying[19]);
                        mesuoption2 = int.Parse(_strBuying[20]);
                    }
                }
            }
            else if (buyingType == 1) // 이동평균선
            {
                // 이평종류 0:단순, 1:지수                
                buyingMovePriceKindType = Int32.Parse(_strBuying[5]);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = Int32.Parse(_strBuying[6]);
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = Int32.Parse(_strBuying[7]);
                // 1,3,5,20 이평종류
                buyingMinuteLineType = Int32.Parse(_strBuying[8]);
                // n%
                buyingMinuteLineAccessPer = double.Parse(_strBuying[9]);
                // 0:근접, 1:돌파
                buyingDistance = Int32.Parse(_strBuying[10]);
            }
            else if (buyingType == 2) // 스토캐스틱
            {
                // 기간
                buyingStocPeriod1 = Int32.Parse(_strBuying[11]);
                // %K
                buyingStocPeriod2 = Int32.Parse(_strBuying[12]);
                // %D
                buyingStocPeriod3 = Int32.Parse(_strBuying[13]);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = Int32.Parse(_strBuying[6]);
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = Int32.Parse(_strBuying[7]);
                // k값
                buyingMinuteLineAccessPer = double.Parse(_strBuying[9]);
                // 0:이상, 1:이하
                buyingDistance = Int32.Parse(_strBuying[10]);
            }
            else if (buyingType == 3) // 볼린져밴드
            {
                // 기간
                buyingStocPeriod1 = Int32.Parse(_strBuying[11]);
                // 승수
                buyingBollPeriod = Double.Parse(_strBuying[14]);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = Int32.Parse(_strBuying[6]);
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = Int32.Parse(_strBuying[7]);
                // 상한선, 중심선, 하한선(0, 1, 2)
                buyingLine3Type = Int32.Parse(_strBuying[15]);
                // n%
                buyingMinuteLineAccessPer = Double.Parse(_strBuying[9]);
                // 0:근접, 1:돌파, 2:이탈
                buyingDistance = Int32.Parse(_strBuying[10]);

            }
            else if (buyingType == 4) // 엔벨로프
            {
                // 기간
                buyingStocPeriod1 = Int32.Parse(_strBuying[11]);
                // %
                buyingBollPeriod = Double.Parse(_strBuying[14]);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = Int32.Parse(_strBuying[6]);
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = Int32.Parse(_strBuying[7]);
                // 상한선, 중심선, 하한선(0, 1, 2)
                buyingLine3Type = Int32.Parse(_strBuying[15]);
                // n%
                buyingMinuteLineAccessPer = Double.Parse(_strBuying[9]);
                // 0:근접, 1:돌파, 2:이탈
                buyingDistance = Int32.Parse(_strBuying[10]);
            }

            ///////////////////////////////////////////////////////////////////////////// 추매 설정 ////////////////////////////////////////////////////////////////////////////////
            _strReBuying = strReBuying.Split(';');
            // _strReBuying[0] reBuyingType
            // _strReBuying[1] reBuyingPer[0]
            // _strReBuying[2] reBuyingPer[1]
            // _strReBuying[3] reBuyingPer[2]
            // _strReBuying[4] reBuyingPer[3]
            // _strReBuying[5] reBuyingPer[4]
            // _strReBuying[6] reBuyingMovePriceKindType
            // _strReBuying[7] reBuyingBongType
            // _strReBuying[8] reBuyingMinuteType
            // _strReBuying[9] reBuyingMinuteLineType[0]
            // _strReBuying[10] reBuyingMinuteLineType[1]
            // _strReBuying[11] reBuyingMinuteLineType[2]
            // _strReBuying[12] reBuyingMinuteLineType[3] 
            // _strReBuying[13] reBuyingMinuteLineType[4];
            reBuyingType = Int32.Parse(_strReBuying[0]);
            if (reBuyingType == 0)
            {
                reBuyingPer[0] = Double.Parse(_strReBuying[1]);
                reBuyingPer[1] = Double.Parse(_strReBuying[2]);
                reBuyingPer[2] = Double.Parse(_strReBuying[3]);
                reBuyingPer[3] = Double.Parse(_strReBuying[4]);
                reBuyingPer[4] = Double.Parse(_strReBuying[5]);
            }
            else if (reBuyingType == 1) // 이동평균선 추매
            {
                //0:단수, 1:지수
                reBuyingMovePriceKindType = Int32.Parse(_strReBuying[6]);
                // 월,주,일,분봉(0,1,2,3)
                reBuyingBongType = Int32.Parse(_strReBuying[7]);
                // 1,3,5,10,20 등등 분봉
                reBuyingMinuteType = Int32.Parse(_strReBuying[8]);
                // 1,3,5,20이평 등등
                reBuyingMinuteLineType[0] = Int32.Parse(_strReBuying[9]);
                reBuyingMinuteLineType[1] = Int32.Parse(_strReBuying[10]);
                reBuyingMinuteLineType[2] = Int32.Parse(_strReBuying[11]);
                reBuyingMinuteLineType[3] = Int32.Parse(_strReBuying[12]);
                reBuyingMinuteLineType[4] = Int32.Parse(_strReBuying[13]);
                // 추매 %
                reBuyingPer[0] = Double.Parse(_strReBuying[1]);
                reBuyingPer[1] = Double.Parse(_strReBuying[2]);
                reBuyingPer[2] = Double.Parse(_strReBuying[3]);
                reBuyingPer[3] = Double.Parse(_strReBuying[4]);
                reBuyingPer[4] = Double.Parse(_strReBuying[5]);
            }

            //////////////////////////////////////////////////////////////////////////////// 익절 설정 //////////////////////////////////////////////////////////////////////////////
            _strTakeProfit = strTakeProfit.Split(';');
            // _strTakeProfit[0] takeProfitUsing.ToString()
            // _strTakeProfit[1] takeProfitType.ToString()
            // _strTakeProfit[2] takeProfitBuyingPricePer.ToString()
            // _strTakeProfit[3] takeProfitMovePriceKindType.ToString()
            // _strTakeProfit[4] takeProfitBongType.ToString() 
            // _strTakeProfit[5] takeProfitMinuteType.ToString()
            // _strTakeProfit[6] takeProfitMinuteLineType.ToString()
            // _strTakeProfit[7] takeProfitLineAccessPer.ToString()
            // _strTakeProfit[8] takeProfitDistance.ToString()
            // _strTakeProfit[9] takeProfitStocPeriod1.ToString() 
            // _strTakeProfit[10] takeProfitStocPeriod2.ToString() 
            // _strTakeProfit[11] takeProfitStocPeriod3.ToString()
            // _strTakeProfit[12] takeProfitBollPeriod.ToString()
            // _strTakeProfit[13] takeProfitLine3Type.ToString()
            takeProfitUsing = Int32.Parse(_strTakeProfit[0]); // 익절 사용 여부            
            takeProfitType = Int32.Parse(_strTakeProfit[1]);

            if (takeProfitType == 0) // 기본 익절
            {
                //익절횟수(최대5회)
                takeProfitCount = int.Parse(_strTakeProfit[2]);
                // 1차 익절 %
                takeProfitBuyingPricePer[0] = double.Parse(_strTakeProfit[3]);
                // 1차 익절매도 비중 %
                takeProfitProportion[0] = double.Parse(_strTakeProfit[4]);
                // 2차 익절 %
                takeProfitBuyingPricePer[1] = double.Parse(_strTakeProfit[5]);
                // 2차 익절매도 비중 %
                takeProfitProportion[1] = double.Parse(_strTakeProfit[6]);
                // 3차 익절 %
                takeProfitBuyingPricePer[2] = double.Parse(_strTakeProfit[7]);
                // 3차 익절매도 비중 %
                takeProfitProportion[2] = double.Parse(_strTakeProfit[8]);
                // 4차 익절 %
                takeProfitBuyingPricePer[3] = double.Parse(_strTakeProfit[9]);
                // 4차 익절매도 비중 %
                takeProfitProportion[3] = double.Parse(_strTakeProfit[10]);
                // 5차 익절 %
                takeProfitBuyingPricePer[4] = double.Parse(_strTakeProfit[11]);
                // 5차 익절매도 비중 %
                takeProfitProportion[4] = double.Parse(_strTakeProfit[12]);
            }
            else if (takeProfitType == 1) // 이동평균선
            {
                // 이평종류 0:단순, 1:지수                
                takeProfitMovePriceKindType = Int32.Parse(_strTakeProfit[13]);
                // 월,주,일,분봉(0,1,2,3)
                takeProfitBongType = Int32.Parse(_strTakeProfit[14]);
                // 1,3,5,10,20 등등 분봉
                takeProfitMinuteType = Int32.Parse(_strTakeProfit[15]);
                // 1,3,5,20 이평종류
                takeProfitMinuteLineType = Int32.Parse(_strTakeProfit[16]);;
                // n%
                takeProfitLineAccessPer = double.Parse(_strTakeProfit[17]);
                // 0:근접, 1:돌파
                takeProfitDistance = Int32.Parse(_strTakeProfit[18]);
            }
            else if (takeProfitType == 2) // 스토캐스틱SLOW
            {
                // 기간
                takeProfitStocPeriod1 = Int32.Parse(_strTakeProfit[19]);
                // %K
                takeProfitStocPeriod2 = Int32.Parse(_strTakeProfit[20]);
                // %D
                takeProfitStocPeriod3 = Int32.Parse(_strTakeProfit[21]);
                // 월,주,일,분봉(0,1,2,3)
                takeProfitBongType = Int32.Parse(_strTakeProfit[14]);
                // 1,3,5,10,20 등등 분봉
                takeProfitMinuteType = Int32.Parse(_strTakeProfit[15]);
                // k값
                takeProfitLineAccessPer = double.Parse(_strTakeProfit[17]);
                // 0:이상, 1:이하
                takeProfitDistance = Int32.Parse(_strTakeProfit[18]);
            }
            else if (takeProfitType == 3) // 볼린저밴드
            {
                // 기간
                takeProfitStocPeriod1 = Int32.Parse(_strTakeProfit[19]);
                // 승수
                takeProfitBollPeriod = double.Parse(_strTakeProfit[22]);
                // 월,주,일,분봉(0,1,2,3)
                takeProfitBongType = Int32.Parse(_strTakeProfit[14]);
                // 1,3,5,10,20 등등 분봉
                takeProfitMinuteType = Int32.Parse(_strTakeProfit[15]);
                // 상한선, 중심선, 하한선(0, 1, 2)
                takeProfitLine3Type = Int32.Parse(_strTakeProfit[23]);
                // n%
                takeProfitLineAccessPer = double.Parse(_strTakeProfit[17]);
                // 0:근접, 1:돌파, 2:이탈
                takeProfitDistance = Int32.Parse(_strTakeProfit[18]);
            }
            else if (takeProfitType == 4) // 엔벨로프
            {
                // 기간
                takeProfitStocPeriod1 = Int32.Parse(_strTakeProfit[19]);
                // 승수
                takeProfitBollPeriod = double.Parse(_strTakeProfit[22]);
                // 월,주,일,분봉(0,1,2,3)
                takeProfitBongType = Int32.Parse(_strTakeProfit[14]);
                // 1,3,5,10,20 등등 분봉
                takeProfitMinuteType = Int32.Parse(_strTakeProfit[15]);
                // 상한선, 중심선, 하한선(0, 1, 2)
                takeProfitLine3Type = Int32.Parse(_strTakeProfit[23]);
                // n%
                takeProfitLineAccessPer = double.Parse(_strTakeProfit[17]);
                // 0:근접, 1:돌파, 2:이탈
                takeProfitDistance = Int32.Parse(_strTakeProfit[18]);
            }

            //////////////////////////////////////////////////////////////////////////////// 손절 설정 //////////////////////////////////////////////////////////////////////////////
            _strStopLoss = strStopLoss.Split(';');
            // _strStopLoss[0] stopLossUsing.ToString() 
            // _strStopLoss[1] stopLossType.ToString() 
            // _strStopLoss[2] stopLossBuyingPricePer.ToString()
            // _strStopLoss[3] stopLossMovePriceKindType.ToString() 
            // _strStopLoss[4] stopLossBongType.ToString() 
            // _strStopLoss[5] stopLossMinuteType.ToString()
            // _strStopLoss[6] stopLossMinuteLineType.ToString() 
            // _strStopLoss[7] stopLossLineAccessPer.ToString()
            // _strStopLoss[8] stopLossDistance.ToString()
            // _strStopLoss[9] stopLossStocPeriod1.ToString() 
            // _strStopLoss[10] stopLossStocPeriod2.ToString() 
            // _strStopLoss[11] stopLossStocPeriod3.ToString() 
            // _strStopLoss[12] stopLossBollPeriod.ToString() 
            // _strStopLoss[13] stopLossLine3Type.ToString()
            stopLossUsing = Int32.Parse(_strStopLoss[0]); // 손절 사용 여부            
            stopLossType = Int32.Parse(_strStopLoss[1]);
            if (stopLossType == 0) // 기본손절
            {
                //손절횟수(최대5회)
                stopLossCount = int.Parse(_strStopLoss[2]);
                // 1차 손절 %
                stopLossBuyingPricePer[0] = double.Parse(_strStopLoss[3]);
                // 1차 손절매도 비중 %
                stopLossProportion[0] = double.Parse(_strStopLoss[4]);
                // 2차 손절 %
                stopLossBuyingPricePer[1] = double.Parse(_strStopLoss[5]);
                // 2차 손절매도 비중 %
                stopLossProportion[1] = double.Parse(_strStopLoss[6]);
                // 3차 손절 %
                stopLossBuyingPricePer[2] = double.Parse(_strStopLoss[7]);
                // 3차 손절매도 비중 %
                stopLossProportion[2] = double.Parse(_strStopLoss[8]);
                // 4차 손절 %
                stopLossBuyingPricePer[3] = double.Parse(_strStopLoss[9]);
                // 4차 손절매도 비중 %
                stopLossProportion[3] = double.Parse(_strStopLoss[10]);
                // 5차 손절 %
                stopLossBuyingPricePer[4] = double.Parse(_strStopLoss[11]);
                // 5차 손절매도 비중 %
                stopLossProportion[4] = double.Parse(_strStopLoss[12]);
            }
            else if (stopLossType == 1) // 이동평균선 손절
            {
                // 이평종류 0:단순, 1:지수                
                stopLossMovePriceKindType = Int32.Parse(_strStopLoss[13]);
                // 월,주,일,분봉(0,1,2,3)
                stopLossBongType = Int32.Parse(_strStopLoss[14]);
                // 1,3,5,10,20 등등 분봉
                stopLossMinuteType = Int32.Parse(_strStopLoss[15]);
                // 1,3,5,20 이평종류
                stopLossMinuteLineType = Int32.Parse(_strStopLoss[16]);
                // n%
                stopLossLineAccessPer = double.Parse(_strStopLoss[17]);
                // 0:근접, 1:돌파
                stopLossDistance = Int32.Parse(_strStopLoss[18]);
            }
            else if (stopLossType == 2) // 볼린저벤드
            {
                // 기간
                stopLossStocPeriod1 = Int32.Parse(_strStopLoss[19]);
                // 승수
                stopLossBollPeriod = double.Parse(_strStopLoss[20]);
                // 월,주,일,분봉(0,1,2,3)
                stopLossBongType = Int32.Parse(_strStopLoss[14]);
                // 1,3,5,10,20 등등 분봉
                stopLossMinuteType = Int32.Parse(_strStopLoss[15]);
                // 상한선, 중심선, 하한선(0, 1, 2)
                stopLossLine3Type = Int32.Parse(_strStopLoss[21]);
                // n%
                stopLossLineAccessPer = double.Parse(_strStopLoss[17]);
                // 0:근접, 1:돌파, 2:이탈
                stopLossDistance = Int32.Parse(_strStopLoss[18]);
            }
            else if (stopLossType == 3) // 엔벨로프
            {
                // 기간
                stopLossStocPeriod1 = Int32.Parse(_strStopLoss[19]);
                // 승수
                stopLossBollPeriod = double.Parse(_strStopLoss[20]);
                // 월,주,일,분봉(0,1,2,3)
                stopLossBongType = Int32.Parse(_strStopLoss[14]);
                // 1,3,5,10,20 등등 분봉
                stopLossMinuteType = Int32.Parse(_strStopLoss[15]);
                // 상한선, 중심선, 하한선(0, 1, 2)
                stopLossLine3Type = Int32.Parse(_strStopLoss[21]);
                // n%
                stopLossLineAccessPer = double.Parse(_strStopLoss[17]);
                // 0:근접, 1:돌파, 2:이탈
                stopLossDistance = Int32.Parse(_strStopLoss[18]);
            }

            //////////////////////////////////////////////////////////////////////////////// ts매도 설정 //////////////////////////////////////////////////////////////////////////////
            _strTsmedo = tsmedo.Split(';');
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
        public void reSetMyTradingCondition(string conditionIndex, string strInvestment, string strInvestmentPrice, 
                                            string strBuying, string strReBuying, string strTakeProfit, string strStopLoss, string tsmedo)
        {
            string[] _strInvestment, _strInvestmentPrice, _strBuying, _strReBuying, _strTakeProfit, _strStopLoss, _strTsmedo;
            // 조건식인덱스
            int _conditionIndex = -1;
            string conditionName = string.Empty;
            _conditionIndex = Int32.Parse(conditionIndex);

            //conditionName = gMainForm.gLoginInstance.conditionList[_conditionIndex].Name;
            conditionName = getConditionName(_conditionIndex);

            ConditionData _buyingCondition = new ConditionData(_conditionIndex, conditionName);

            conditionData = _buyingCondition;

            ////////////////////////////////////////////////////////////////////////////// 투자금 설정 //////////////////////////////////////////////////////////////////////////////
            _strInvestment = strInvestment.Split(';');
            //_strInvestment[0] account
            // _strInvestment[1] conditionName
            // _strInvestment[2] conditionIndex 
            // _strInvestment[3] buyingItmeCount 
            // _strInvestment[4] itemInvestment
            account = _strInvestment[0]; // 계좌번호
            buyingItemCount = Int32.Parse(_strInvestment[3]); // 매수 종목 수
            itemInvestment = double.Parse(_strInvestment[4]); // 종목당 투자금액
            transferItemCount = int.Parse(_strInvestment[5]); // 편입 종목 수
            remainingBuyingItemCount = buyingItemCount;
            remainingTransferItemCount = transferItemCount;

            ////////////////////////////////////////////////////////////////////////// 매수금액설정 ///////////////////////////////////////////////////////////////////////////////////
            _strInvestmentPrice = strInvestmentPrice.Split(';');
            //_strInvestmentPrice[0] buyingCount 
            //_strInvestmentPrice[1] investMoney_1 
            //_strInvestmentPrice[2] investMoney_2 
            //_strInvestmentPrice[3] investMoney_3 
            //_strInvestmentPrice[4] investMoney_4 
            //_strInvestmentPrice[5] investMoney_5 
            //_strInvestmentPrice[6] investMoney_6;
            buyingCount = Int32.Parse(_strInvestmentPrice[0]);
            for (int i = 0; i < 6; i++)
            {
                buyingInvestment[i] = double.Parse(_strInvestmentPrice[1 + i]);
            }

            //////////////////////////////////////////////////////////////////////////////// 매수 설정 //////////////////////////////////////////////////////////////////////////////            
            _strBuying = strBuying.Split(';');
            // _strBuying[0] buyingUsing.ToString() 
            // _strBuying[1] buyingType.ToString() 
            // _strBuying[2] buyingTransferType.ToString() 
            // _strBuying[3] buyingTransferPer.ToString() 
            // _strBuying[4] buyingTransferUpdown.ToString()
            // _strBuying[5] buyingMovePriceKindType.ToString() 
            // _strBuying[6] buyingBongType.ToString() 
            // _strBuying[7] buyingMinuteType.ToString() 
            // _strBuying[8] buyingMinuteLineType.ToString() 
            // _strBuying[9] buyingMinuteLineAccessPer.ToString() 
            // _strBuying[10] buyingDistance.ToString()
            // _strBuying[11] buyingStocPeriod1.ToString() 
            // _strBuying[12] buyingStocPeriod2.ToString() 
            // _strBuying[13] buyingStocPeriod3.ToString() 
            // _strBuying[14] buyingBollPeriod.ToString() 
            // _strBuying[15] buyingLine3Type.ToString();                
            buyingUsing = Int32.Parse(_strBuying[0]);// 매수사용체크박스
            buyingType = Int32.Parse(_strBuying[1]);
            if (buyingType == 0) // 기본매수
            {
                mesuoption1 = int.Parse(_strBuying[18]);
                buyingTransferType = Int32.Parse(_strBuying[2]);
                if (buyingTransferType == 0) // 편입즉시매수
                {
                    if (mesuoption1 == 1) // 지정가
                    {
                        nontradingtime = int.Parse(_strBuying[19]);
                        mesuoption2 = int.Parse(_strBuying[20]);
                    }
                }
                else if(buyingTransferType == 1) // 편입후 n% 매수
                {
                    // 대비 n%
                    buyingTransferPer = double.Parse(_strBuying[3]);
                    // 이상, 이하
                    buyingTransferUpdown = Int32.Parse(_strBuying[4]);

                    if (mesuoption1 == 1) // 지정가
                    {
                        nontradingtime = int.Parse(_strBuying[19]);
                        mesuoption2 = int.Parse(_strBuying[20]);
                    }
                }
                else if(buyingTransferType == 2) // 저점대비 상승매수
                {
                    buyingTransferPer2 = double.Parse(_strBuying[16]);
                    riseTransferPer2 = double.Parse(_strBuying[17]);


                    if (mesuoption1 == 1) // 지정가
                    {
                        nontradingtime = int.Parse(_strBuying[19]);
                        mesuoption2 = int.Parse(_strBuying[20]);
                    }
                }
            }
            else if (buyingType == 1) // 이동평균선
            {
                // 이평종류 0:단순, 1:지수                
                buyingMovePriceKindType = Int32.Parse(_strBuying[5]);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = Int32.Parse(_strBuying[6]);
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = Int32.Parse(_strBuying[7]);
                // 1,3,5,20 이평종류
                buyingMinuteLineType = Int32.Parse(_strBuying[8]);
                // n%
                buyingMinuteLineAccessPer = double.Parse(_strBuying[9]);
                // 0:근접, 1:돌파
                buyingDistance = Int32.Parse(_strBuying[10]);
            }
            else if (buyingType == 2) // 스토캐스틱
            {
                // 기간
                buyingStocPeriod1 = Int32.Parse(_strBuying[11]);
                // %K
                buyingStocPeriod2 = Int32.Parse(_strBuying[12]);
                // %D
                buyingStocPeriod3 = Int32.Parse(_strBuying[13]);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = Int32.Parse(_strBuying[6]);
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = Int32.Parse(_strBuying[7]);
                // k값
                buyingMinuteLineAccessPer = double.Parse(_strBuying[9]);
                // 0:이상, 1:이하
                buyingDistance = Int32.Parse(_strBuying[10]);
            }
            else if (buyingType == 3) // 볼린져밴드
            {
                // 기간
                buyingStocPeriod1 = Int32.Parse(_strBuying[11]);
                // 승수
                buyingBollPeriod = Double.Parse(_strBuying[14]);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = Int32.Parse(_strBuying[6]);
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = Int32.Parse(_strBuying[7]);
                // 상한선, 중심선, 하한선(0, 1, 2)
                buyingLine3Type = Int32.Parse(_strBuying[15]);
                // n%
                buyingMinuteLineAccessPer = Double.Parse(_strBuying[9]);
                // 0:근접, 1:돌파, 2:이탈
                buyingDistance = Int32.Parse(_strBuying[10]);

            }
            else if (buyingType == 4) // 엔벨로프
            {
                // 기간
                buyingStocPeriod1 = Int32.Parse(_strBuying[11]);
                // %
                buyingBollPeriod = Double.Parse(_strBuying[14]);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = Int32.Parse(_strBuying[6]);
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = Int32.Parse(_strBuying[7]);
                // 상한선, 중심선, 하한선(0, 1, 2)
                buyingLine3Type = Int32.Parse(_strBuying[15]);
                // n%
                buyingMinuteLineAccessPer = Double.Parse(_strBuying[9]);
                // 0:근접, 1:돌파, 2:이탈
                buyingDistance = Int32.Parse(_strBuying[10]);
            }

            ///////////////////////////////////////////////////////////////////////////// 추매 설정 ////////////////////////////////////////////////////////////////////////////////
            _strReBuying = strReBuying.Split(';');
            // _strReBuying[0] reBuyingType
            // _strReBuying[1] reBuyingPer[0]
            // _strReBuying[2] reBuyingPer[1]
            // _strReBuying[3] reBuyingPer[2]
            // _strReBuying[4] reBuyingPer[3]
            // _strReBuying[5] reBuyingPer[4]
            // _strReBuying[6] reBuyingMovePriceKindType
            // _strReBuying[7] reBuyingBongType
            // _strReBuying[8] reBuyingMinuteType
            // _strReBuying[9] reBuyingMinuteLineType[0]
            // _strReBuying[10] reBuyingMinuteLineType[1]
            // _strReBuying[11] reBuyingMinuteLineType[2]
            // _strReBuying[12] reBuyingMinuteLineType[3] 
            // _strReBuying[13] reBuyingMinuteLineType[4];
            reBuyingType = Int32.Parse(_strReBuying[0]);
            if (reBuyingType == 0)
            {
                reBuyingPer[0] = Double.Parse(_strReBuying[1]);
                reBuyingPer[1] = Double.Parse(_strReBuying[2]);
                reBuyingPer[2] = Double.Parse(_strReBuying[3]);
                reBuyingPer[3] = Double.Parse(_strReBuying[4]);
                reBuyingPer[4] = Double.Parse(_strReBuying[5]);
            }
            else if (reBuyingType == 1) // 이동평균선 추매
            {
                //0:단수, 1:지수
                reBuyingMovePriceKindType = Int32.Parse(_strReBuying[6]);
                // 월,주,일,분봉(0,1,2,3)
                reBuyingBongType = Int32.Parse(_strReBuying[7]);
                // 1,3,5,10,20 등등 분봉
                reBuyingMinuteType = Int32.Parse(_strReBuying[8]);
                // 1,3,5,20이평 등등
                reBuyingMinuteLineType[0] = Int32.Parse(_strReBuying[9]);
                reBuyingMinuteLineType[1] = Int32.Parse(_strReBuying[10]);
                reBuyingMinuteLineType[2] = Int32.Parse(_strReBuying[11]);
                reBuyingMinuteLineType[3] = Int32.Parse(_strReBuying[12]);
                reBuyingMinuteLineType[4] = Int32.Parse(_strReBuying[13]);
                // 추매 %
                reBuyingPer[0] = Double.Parse(_strReBuying[1]);
                reBuyingPer[1] = Double.Parse(_strReBuying[2]);
                reBuyingPer[2] = Double.Parse(_strReBuying[3]);
                reBuyingPer[3] = Double.Parse(_strReBuying[4]);
                reBuyingPer[4] = Double.Parse(_strReBuying[5]);
            }

            //////////////////////////////////////////////////////////////////////////////// 익절 설정 //////////////////////////////////////////////////////////////////////////////
            _strTakeProfit = strTakeProfit.Split(';');
            // _strTakeProfit[0] takeProfitUsing.ToString()
            // _strTakeProfit[1] takeProfitType.ToString()
            // _strTakeProfit[2] takeProfitBuyingPricePer.ToString()
            // _strTakeProfit[3] takeProfitMovePriceKindType.ToString()
            // _strTakeProfit[4] takeProfitBongType.ToString() 
            // _strTakeProfit[5] takeProfitMinuteType.ToString()
            // _strTakeProfit[6] takeProfitMinuteLineType.ToString()
            // _strTakeProfit[7] takeProfitLineAccessPer.ToString()
            // _strTakeProfit[8] takeProfitDistance.ToString()
            // _strTakeProfit[9] takeProfitStocPeriod1.ToString() 
            // _strTakeProfit[10] takeProfitStocPeriod2.ToString() 
            // _strTakeProfit[11] takeProfitStocPeriod3.ToString()
            // _strTakeProfit[12] takeProfitBollPeriod.ToString()
            // _strTakeProfit[13] takeProfitLine3Type.ToString()
            takeProfitUsing = Int32.Parse(_strTakeProfit[0]); // 익절 사용 여부            
            takeProfitType = Int32.Parse(_strTakeProfit[1]);

            if (takeProfitType == 0) // 기본 익절
            {
                //익절횟수(최대5회)
                takeProfitCount = int.Parse(_strTakeProfit[2]);
                // 1차 익절 %
                takeProfitBuyingPricePer[0] = double.Parse(_strTakeProfit[3]);
                // 1차 익절매도 비중 %
                takeProfitProportion[0] = double.Parse(_strTakeProfit[4]);
                // 2차 익절 %
                takeProfitBuyingPricePer[1] = double.Parse(_strTakeProfit[5]);
                // 2차 익절매도 비중 %
                takeProfitProportion[1] = double.Parse(_strTakeProfit[6]);
                // 3차 익절 %
                takeProfitBuyingPricePer[2] = double.Parse(_strTakeProfit[7]);
                // 3차 익절매도 비중 %
                takeProfitProportion[2] = double.Parse(_strTakeProfit[8]);
                // 4차 익절 %
                takeProfitBuyingPricePer[3] = double.Parse(_strTakeProfit[9]);
                // 4차 익절매도 비중 %
                takeProfitProportion[3] = double.Parse(_strTakeProfit[10]);
                // 5차 익절 %
                takeProfitBuyingPricePer[4] = double.Parse(_strTakeProfit[11]);
                // 5차 익절매도 비중 %
                takeProfitProportion[4] = double.Parse(_strTakeProfit[12]);
            }
            else if (takeProfitType == 1) // 이동평균선
            {
                // 이평종류 0:단순, 1:지수                
                takeProfitMovePriceKindType = Int32.Parse(_strTakeProfit[13]);
                // 월,주,일,분봉(0,1,2,3)
                takeProfitBongType = Int32.Parse(_strTakeProfit[14]);
                // 1,3,5,10,20 등등 분봉
                takeProfitMinuteType = Int32.Parse(_strTakeProfit[15]);
                // 1,3,5,20 이평종류
                takeProfitMinuteLineType = Int32.Parse(_strTakeProfit[16]); ;
                // n%
                takeProfitLineAccessPer = double.Parse(_strTakeProfit[17]);
                // 0:근접, 1:돌파
                takeProfitDistance = Int32.Parse(_strTakeProfit[18]);
            }
            else if (takeProfitType == 2) // 스토캐스틱SLOW
            {
                // 기간
                takeProfitStocPeriod1 = Int32.Parse(_strTakeProfit[19]);
                // %K
                takeProfitStocPeriod2 = Int32.Parse(_strTakeProfit[20]);
                // %D
                takeProfitStocPeriod3 = Int32.Parse(_strTakeProfit[21]);
                // 월,주,일,분봉(0,1,2,3)
                takeProfitBongType = Int32.Parse(_strTakeProfit[14]);
                // 1,3,5,10,20 등등 분봉
                takeProfitMinuteType = Int32.Parse(_strTakeProfit[15]);
                // k값
                takeProfitLineAccessPer = double.Parse(_strTakeProfit[17]);
                // 0:이상, 1:이하
                takeProfitDistance = Int32.Parse(_strTakeProfit[18]);
            }
            else if (takeProfitType == 3) // 볼린저밴드
            {
                // 기간
                takeProfitStocPeriod1 = Int32.Parse(_strTakeProfit[19]);
                // 승수
                takeProfitBollPeriod = double.Parse(_strTakeProfit[22]);
                // 월,주,일,분봉(0,1,2,3)
                takeProfitBongType = Int32.Parse(_strTakeProfit[14]);
                // 1,3,5,10,20 등등 분봉
                takeProfitMinuteType = Int32.Parse(_strTakeProfit[15]);
                // 상한선, 중심선, 하한선(0, 1, 2)
                takeProfitLine3Type = Int32.Parse(_strTakeProfit[23]);
                // n%
                takeProfitLineAccessPer = double.Parse(_strTakeProfit[17]);
                // 0:근접, 1:돌파, 2:이탈
                takeProfitDistance = Int32.Parse(_strTakeProfit[18]);
            }
            else if (takeProfitType == 4) // 엔벨로프
            {
                // 기간
                takeProfitStocPeriod1 = Int32.Parse(_strTakeProfit[19]);
                // 승수
                takeProfitBollPeriod = double.Parse(_strTakeProfit[22]);
                // 월,주,일,분봉(0,1,2,3)
                takeProfitBongType = Int32.Parse(_strTakeProfit[14]);
                // 1,3,5,10,20 등등 분봉
                takeProfitMinuteType = Int32.Parse(_strTakeProfit[15]);
                // 상한선, 중심선, 하한선(0, 1, 2)
                takeProfitLine3Type = Int32.Parse(_strTakeProfit[23]);
                // n%
                takeProfitLineAccessPer = double.Parse(_strTakeProfit[17]);
                // 0:근접, 1:돌파, 2:이탈
                takeProfitDistance = Int32.Parse(_strTakeProfit[18]);
            }

            //////////////////////////////////////////////////////////////////////////////// 손절 설정 //////////////////////////////////////////////////////////////////////////////
            _strStopLoss = strStopLoss.Split(';');
            // _strStopLoss[0] stopLossUsing.ToString() 
            // _strStopLoss[1] stopLossType.ToString() 
            // _strStopLoss[2] stopLossBuyingPricePer.ToString()
            // _strStopLoss[3] stopLossMovePriceKindType.ToString() 
            // _strStopLoss[4] stopLossBongType.ToString() 
            // _strStopLoss[5] stopLossMinuteType.ToString()
            // _strStopLoss[6] stopLossMinuteLineType.ToString() 
            // _strStopLoss[7] stopLossLineAccessPer.ToString()
            // _strStopLoss[8] stopLossDistance.ToString()
            // _strStopLoss[9] stopLossStocPeriod1.ToString() 
            // _strStopLoss[10] stopLossStocPeriod2.ToString() 
            // _strStopLoss[11] stopLossStocPeriod3.ToString() 
            // _strStopLoss[12] stopLossBollPeriod.ToString() 
            // _strStopLoss[13] stopLossLine3Type.ToString()
            stopLossUsing = Int32.Parse(_strStopLoss[0]); // 익절 사용 여부            
            stopLossType = Int32.Parse(_strStopLoss[1]);
            if (stopLossType == 0) // 기본손절
            {
                //손절횟수(최대5회)
                stopLossCount = int.Parse(_strStopLoss[2]);
                // 1차 손절 %
                stopLossBuyingPricePer[0] = double.Parse(_strStopLoss[3]);
                // 1차 손절매도 비중 %
                stopLossProportion[0] = double.Parse(_strStopLoss[4]);
                // 2차 손절 %
                stopLossBuyingPricePer[1] = double.Parse(_strStopLoss[5]);
                // 2차 손절매도 비중 %
                stopLossProportion[1] = double.Parse(_strStopLoss[6]);
                // 3차 손절 %
                stopLossBuyingPricePer[2] = double.Parse(_strStopLoss[7]);
                // 3차 손절매도 비중 %
                stopLossProportion[2] = double.Parse(_strStopLoss[8]);
                // 4차 손절 %
                stopLossBuyingPricePer[3] = double.Parse(_strStopLoss[9]);
                // 4차 손절매도 비중 %
                stopLossProportion[3] = double.Parse(_strStopLoss[10]);
                // 5차 손절 %
                stopLossBuyingPricePer[4] = double.Parse(_strStopLoss[11]);
                // 5차 손절매도 비중 %
                stopLossProportion[4] = double.Parse(_strStopLoss[12]);
            }
            else if (stopLossType == 1) // 이동평균선 손절
            {
                // 이평종류 0:단순, 1:지수                
                stopLossMovePriceKindType = Int32.Parse(_strStopLoss[13]);
                // 월,주,일,분봉(0,1,2,3)
                stopLossBongType = Int32.Parse(_strStopLoss[14]);
                // 1,3,5,10,20 등등 분봉
                stopLossMinuteType = Int32.Parse(_strStopLoss[15]);
                // 1,3,5,20 이평종류
                stopLossMinuteLineType = Int32.Parse(_strStopLoss[16]);
                // n%
                stopLossLineAccessPer = double.Parse(_strStopLoss[17]);
                // 0:근접, 1:돌파
                stopLossDistance = Int32.Parse(_strStopLoss[18]);
            }
            else if (stopLossType == 2) // 볼린저벤드
            {
                // 기간
                stopLossStocPeriod1 = Int32.Parse(_strStopLoss[19]);
                // 승수
                stopLossBollPeriod = double.Parse(_strStopLoss[20]);
                // 월,주,일,분봉(0,1,2,3)
                stopLossBongType = Int32.Parse(_strStopLoss[14]);
                // 1,3,5,10,20 등등 분봉
                stopLossMinuteType = Int32.Parse(_strStopLoss[15]);
                // 상한선, 중심선, 하한선(0, 1, 2)
                stopLossLine3Type = Int32.Parse(_strStopLoss[21]);
                // n%
                stopLossLineAccessPer = double.Parse(_strStopLoss[17]);
                // 0:근접, 1:돌파, 2:이탈
                stopLossDistance = Int32.Parse(_strStopLoss[18]);
            }
            else if (stopLossType == 3) // 엔벨로프
            {
                // 기간
                stopLossStocPeriod1 = Int32.Parse(_strStopLoss[19]);
                // 승수
                stopLossBollPeriod = double.Parse(_strStopLoss[20]);
                // 월,주,일,분봉(0,1,2,3)
                stopLossBongType = Int32.Parse(_strStopLoss[14]);
                // 1,3,5,10,20 등등 분봉
                stopLossMinuteType = Int32.Parse(_strStopLoss[15]);
                // 상한선, 중심선, 하한선(0, 1, 2)
                stopLossLine3Type = Int32.Parse(_strStopLoss[21]);
                // n%
                stopLossLineAccessPer = double.Parse(_strStopLoss[17]);
                // 0:근접, 1:돌파, 2:이탈
                stopLossDistance = Int32.Parse(_strStopLoss[18]);
            }
            //////////////////////////////////////////////////////////////////////////////// ts매도 설정 //////////////////////////////////////////////////////////////////////////////
            _strTsmedo = tsmedo.Split(';');
            // tsmedo[0] tsMedoUsing -> ts매도 사용여부
            // tsmedo[1] tsMedoCount -> ts매도횟수(최대3회)
            // tsmedo[2] tsMedoUsingtype ->  0:목표가, 1:고점
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
