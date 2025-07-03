using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockAutoTrade2
{
    public class MyTradingItem
    {
        public MainForm gMainForm = MainForm.GetInstance();

        public string m_conditionName; // 조건식이름
        public string m_conditionNumber; // 조건식번호
        public string m_itemCode; // 종목코드
        public string m_itemName; // 종목명
        public double m_transferPrice; // 편입가격
        public double m_upperLimitPrice; // 상한가
        public double m_currentPrice; // 현재가
        public double m_rateOfReturn; // 수익률
        public double m_lowlowprice = 0; //저점가격
        public bool m_completeConclusion = false; // 주문접수: false, 체결데이터 수신시: true,
        public int m_orderQnt = 0; // 주문접수 수량
        public bool m_bRetryOrder = false; // 정정주문들어갔는지 여부
        public bool m_bCanceled = false; // 취소주문들어갔는지 여부
        public bool m_isMedoOrderPlaced = false; // 매도주문 들어갔는지 여부
        public int m_outstandingQnt = 0; // 미체결수량

        // 매수 관련
        public bool m_bCompletePurchase = false; // 매수상태(true: 매수O, false: 매수X)
        //public string m_sellOrderNumber; // 매도주문번호 -> 사용안함
        public string m_buyingOrderNumber; // 매수주문번호 -> 
        public string m_modifyOrderNumber; // 매수정정주문번호
        //매도주문번호 및 수익률계산
        public List<ProfitLossCalculate> m_profitLossCalculateList = new List<ProfitLossCalculate>();
        public double m_averagePrice; // 평균단가
        public int m_orderAvailableQnt; // 주문가능수량
        public int m_totalPurchaseAmount; // 총매입금
        public int m_totalQnt; // 매수량
        public bool m_bPurchased = false; // 현재 매수중인지 확인 변수
        public bool m_brePurchased = false; // 추가매수 들어갔는지 확인 변수
        public bool m_bSold = false; //현재 매도중인지 확인 뱐수
        public double m_finalRateOfReturn = 0; //
        public double m_purchasePerPriceCalciuate = 0; // 매수때 마다 평단계산

        //추매관련배열
        public bool[] m_rePurchaseArray = new bool[6];
        public int m_rePurchaseNumber = -1;

        //ts매도관련배열
        public bool[] m_tsMedoArray = new bool[3]; // ts매도가 이루어졋을때 값을 변환하기위함
        public int m_tsMedoNumber = -1;

        //기본익절관련배열
        public bool[] m_takeProfitArray = new bool[5];
        //기본손절관련배열
        public bool[] m_stopLossArray = new bool[5];

        //분봉데이터 관련
        public int m_getBunBongDataCount = 0; // 분봉데이터를 가져온 횟수
        public int m_totalBunBongCount = 0; // 가져온 분봉의 총 갯수
        public bool m_bGetBunBongSuccess = false; // 분봉 데이터를 다 가져왔는지 확인

        //1분봉데이터 변수 선언
        public double[] m_startPrice = new double[5000]; // 시가
        public double[] m_endPrice = new double[5000]; // 종가 == 현재가
        public double[] m_highPrice = new double[5000]; // 고가
        public double[] m_lowPrice = new double[5000]; // 저가
        public int[] m_tradingVolume = new int[5000]; // 거래량

        public string[] m_strHM = new string[5000];
        public string[] m_strDay = new string[5000];

        public bool m_bCheckIpperLimit = false; // 상한가 계산 체크
        public double m_prevEndPrice = 0; // 전날 종가

        public int[] intervalArrayCount = { 382, 129, 78, 39, 27, 15, 10, 8 }; // 분봉당 개수(1분,3분,5분,10분,15분,30분,45분,60분)의 갯수 +1(계산을위해+1함, 인덱스로 치면0부터 있기떄문에)
        public int[,,] bb_interval = new int[8, 400, 2]; // 분봉별 시간 저장 [분봉종류8개 , 분봉갯수(최대값 382를 넉넉하게400), 현재값과 다음값2개] //값을 직접 넣은게 아니고 최대로 들어갈수있는 값을 넣어놓은거.
        public int[,] bb_exceptionInterval = new int[400, 2]; // 45분봉 장이 10시 시작시 예외처리
        public int[] calBongNumber = {-1,-1,-1,-1}; 
        // 기존에는 0,1,2,3,4,5,6,7(1,3,5,10,15,30,45,60) -> 계산할 분봉의 배열변수(index번호를 뜻하는거같음) -> 1분봉은 0번째 배열, 3분봉은 1번째 배열
        // 현재 변형된거는 -1로 4개의 1차원적 배열을 가지게되는데, calbongnumber를 사용하는데는 총 4곳이 존재함
        // 매수, 추가매수, 익절, 손절을 세팅에 따라 각자 다른 분봉을 사용할 수 있기때문에 그 분봉에 대해서만 계산하도록 4배열을 가짐
        // 기존것을 사용시 모든 데이터를 계산하기때문에 오래걸릴수 있는 단점을 필요한것만 해서 속도적으로 조금 더빨리 처리하기 위함으로 보임
        public int[] calBongCount = new int[8]; // 분봉당 총 분봉갯수

        public double[,] bb_highPrice = new double[8, 5000]; // 계산된 고가
        public double[,] bb_lowPrice = new double[8, 5000]; // 계산된 저가
        public double[,] bb_startPrice = new double[8, 5000]; // 계산된 시가
        public double[,] bb_endPrice = new double[8, 5000]; // 계산된 종가
        public int[,] bb_tradingVolume = new int[8, 5000]; // 계산된 거래량
        public string[,] bb_strDayhm = new string[8, 5000]; // 날짜 저장

        // 1분봉데이터 얻어 오기전 거래량 보정을 위한 변수
        public int m_volumeState = (int)trading.wait; // 0 진행전, 1 진행중 , 2 진행끝 (EtcData.cs 참조)
        public string[] m_tradingTime = new string[2]; //시간저장
        public int[] m_tradeVolume = new int[2]; // 거래량
        public double[] m_tradeHighPrice = new double[2];
        public double[] m_tradeLowPrice = new double[2];

        // 분봉 이동평균선
        public double[] moving_PriceCur = new double[4]; // [4]인 이유는 매수,추매, 익절,손절 순서이다.
        public double[] moving_PricePrev = new double[4];
        // 분봉 지수이동평균선
        public double[] moving_PriceCurE = new double[4]; // 0123 매수,추매,익절,손절
        public double[] moving_PricePrevE = new double[4];

        // 분봉볼린저밴드
        public double[] bollinger_upPriceCur = new double[4];
        public double[] bollinger_upPricePrev = new double[4];
        public double[] bollinger_centerPriceCur = new double[4];
        public double[] bollinger_centerPricePrev = new double[4];
        public double[] bollinger_downPriceCur = new double[4];
        public double[] bollinger_downPricePrev = new double[4];

        // 분봉엔벨로프
        public double[] envelope_upPriceCur = new double[4];
        public double[] envelope_upPricePrev = new double[4];
        public double[] envelope_centerPriceCur = new double[4];
        public double[] envelope_centerPricePrev = new double[4];
        public double[] envelope_downPriceCur = new double[4];
        public double[] envelope_downPricePrev = new double[4];

        // 분봉스토캐스틱 슬로우
        public double[] stochastics_KPriceCur = new double[4];
        public double[] stochastics_KPricePrev = new double[4];
        public double[] stochastics_DPriceCur = new double[4];
        public double[] stochastics_DPricePrev = new double[4];

        // 일봉저장변수
        public double[] db_highPrice = new double[1000]; // 고가
        public double[] db_lowPrice = new double[1000]; // 저가
        public double[] db_startPrice = new double[1000]; // 시가
        public double[] db_endPrice = new double[1000]; // 종가
        public int[] db_tradingVolume = new int[1000]; // 거래량

        public int m_totalDayBongCount = 0; // 받아온 일본 데이터 총 갯수
        public bool m_bGetDayBongSuccess = false; // 일봉 데이터를 다 가져왔는지 확인

        // 일봉 이동평균선
        public double[] moving_PriceDayCur = new double[4]; // 0123 매수,추매,익절,손절
        public double[] moving_PriceDayPrev = new double[4];
        // 분봉 지수이동평균선
        public double[] moving_PriceDayCurE = new double[4]; // 0123 매수,추매,익절,손절
        public double[] moving_PriceDayPrevE = new double[4];

        // 일봉 볼린저밴드
        public double[] bollinger_upPriceDayCur = new double[4];
        public double[] bollinger_upPriceDayPrev = new double[4];
        public double[] bollinger_centerPriceDayCur = new double[4];
        public double[] bollinger_centerPriceDayPrev = new double[4];
        public double[] bollinger_downPriceDayCur = new double[4];
        public double[] bollinger_downPriceDayPrev = new double[4];

        // 일봉 엔벨로프
        public double[] envelope_upPriceDayCur = new double[4];
        public double[] envelope_centerPriceDayCur = new double[4];
        public double[] envelope_downPriceDayCur = new double[4];
        public double[] envelope_upPriceDayPrev = new double[4];
        public double[] envelope_centerPriceDayPrev = new double[4];
        public double[] envelope_downPriceDayPrev = new double[4];

        // 일봉 스토캐스틱
        public double[] stochastics_KPriceDayCur = new double[4];
        public double[] stochastics_KPriceDayPrev = new double[4];
        public double[] stochastics_DPriceDayCur = new double[4];
        public double[] stochastics_DPriceDayPrev = new double[4];

        // 추매 이동평균
        public double[] reBuyingMoving_PriceCur = new double[5];
        public double[] reBuyingMoving_PricePrev = new double[5];
        public double[] reBuyingMoving_PriceDayCur = new double[5];
        public double[] reBuyingMoving_PriceDayPrev = new double[5];

        // 매매조건식 데이터 처리 관련 변수
        ///////////////////////////////////////// 매매설정에서 저장된 변수 셋팅 /////////////////////////////////////////
        ///// 매수
        public int m_buyingUsing = 1; // 매수 사용 여부
        public int m_buyingType = 0; // 0:기본매수, 1:이동평균,2:스토케스틱,3:볼린저밴드,4:엔벨로프
        public int m_buyingTransferType = 0; // 0:편입시 바로 매수,1:편입가격대비매수
        public double m_buyingTransferPer = 0; // 편입가격대비매수시 %
        public int m_buyingTransferUpdown = 0; // 편입가격대비매수시 %에 대한 0: 이하, 1: 이상
        public double m_buyingTransferPer2 = 0; // 저점대비 도달 %
        public double m_riseTransferPer2 = 0; // 저점대비 도달후 상승%
        public int m_mesuoption1 = 0; // 0: 시장가 , 1: 현재가(지정가)
        public DateTime m_orderReceivedTime; // 지정가 주문이 시작된 시간
        public int m_nontradingtime = 0; // 지정가시 주문후 대기시간
        public int m_mesuoption2 = 0; // 0: 시장가로정정 , 1: 일괄정정 , 2: 일괄취소
        public int m_buyingBongType = 0; // 0:일봉, 1:분봉
        public int m_buyingMinuteType = 0; // 1,3,5분봉 등등 입력
        public int m_buyingMinuteLineType = 0;// 1,3,5,20이평 등등
        public int m_buyingMovePriceKindType = 0; //0:단수, 1:지수
        public double m_buyingMinuteLineAccessPer = 0;// 근접 %
        public int m_buyingDistance = 0; // 0:근접, 1:돌파, 2:이탈, 0:이상, 1:이하
        public double m_buyingStocPeriod1 = 0; //기간
        public double m_buyingStocPeriod2 = 0; //%K
        public double m_buyingStocPeriod3 = 0; //%D
        public double m_buyingBollPeriod = 0; //승수, 엔벨%
        public int m_buyingLine3Type = 0; // 0:상한선,1:중심선,2:하한선
        // 추매
        public int m_reBuyingType = 0; //0:기본추매, 1:이동평균 근접
        public double[] m_reBuyingPer = new double[5]; // 추매 %
        // 이동평균근접
        public int m_reBuyingMovePriceKindType = 0; //0:단수, 1:지수
        public int m_reBuyingBongType = 0; // 0:월,1:주,2:일,3:분
        public int m_reBuyingMinuteType = 0; // 1,3,5분봉 등등 입력
        public int[] m_reBuyingMinuteLineType = new int[5];// 1,3,5,20이평 등등
        // 익절
        public int m_takeProfitUsing = 1; // 익절 사용 여부
        public int m_takeProfitType = 0; // 0:기본익절,1:이동평균 근접
        public double[] m_takeProfitBuyingPricePer = new double[5]; // 익절도달 %
        public double[] m_takeProfitProportion = new double[5]; // 익절매도비중 %
        // 이동평균 근접, 돌파 공통 사용        
        public int m_takeProfitMovePriceKindType = 0; //0:단수, 1:지수        
        public int m_takeProfitBongType = 0; // 0:월,1:주,2:일,3:분
        public int m_takeProfitMinuteType = 0; // 1,3,5분봉 등등 입력
        public int m_takeProfitMinuteLineType = 0;// 1,3,5,20이평 등등        
        public double m_takeProfitLineAccessPer = 0;// 근접 %
        public int m_takeProfitDistance = 0; // 0:근접, 1:돌파,2:이탈, 0:이상, 1:이하
        public double m_takeProfitStocPeriod1 = 0; //기간
        public double m_takeProfitStocPeriod2 = 0; //%K
        public double m_takeProfitStocPeriod3 = 0; //%D
        public double m_takeProfitBollPeriod = 0; //승수, 엔벨%
        public int m_takeProfitLine3Type = 0; // 0:상한선,1:중심선,2:하한선
        // 손절
        public int m_stopLossUsing = 1; // 손절 사용 여부
        public int m_stopLossType = 0; // 0:기본손절,1:이동평균 근접
        public double[] m_stopLossBuyingPricePer = new double[5]; // 손절도달 %
        public double[] m_stopLossProportion = new double[5]; // 손절매도비중 %
        // 이동평균 근접, 이탈 공통 사용        
        public int m_stopLossMovePriceKindType = 0; //0:단수, 1:지수        
        public int m_stopLossBongType = 0; // 0:월,1:주,2:일,3:분
        public int m_stopLossMinuteType = 0; // 1,3,5분봉 등등 입력
        public int m_stopLossMinuteLineType = 0;// 1,3,5,20이평 등등        
        public double m_stopLossLineAccessPer = 0;// 근접 %
        public int m_stopLossDistance = 0; // 0:근접, 1:돌파,2:이탈, 0:이상, 1:이하
        public double m_stopLossStocPeriod1 = 0; //기간
        public double m_stopLossStocPeriod2 = 0; //%K
        public double m_stopLossStocPeriod3 = 0; //%D
        public double m_stopLossBollPeriod = 0; //승수, 엔벨%
        public int m_stopLossLine3Type = 0; // 0:상한선,1:중심선,2:하한선
        // ts매도
        public int m_tsmedoUsing = 1; //ts매도 사용여부
        public int[] m_tsMedoUsingType = new int[3]; // 0:목표가,1:고점
        public double[] m_tsMedoArchievedPer = new double[3]; // 목표가일경우 달성한 %
        public double[] m_tsMedoPercent = new double[3]; // ts매도하락%
        public double[] m_tsMedoProportion = new double[3]; // ts매도비중%
        public int m_tsProfitPreservation1 = 1;//1차 ts매도 이익보존 사용여부
        public int m_tsProfitPreservation2 = 1;//2차 ts매도 이익보존 사용여부
        public int m_tsProfitPreservation3 = 1;//3차 ts매도 이익보존 사용여부

        //목표가ts매도 로그를 위한 변수
        public bool _bFirstTsmedologCheck = false;
        public bool _bSecondTsmedologCheck = false;
        public bool _bThirdTsmedologCheck = false;

        public int m_currentRebuyingStep = 0; // 현재 진행 중인 추가매수 차수를 저장하는 변수 (1차부터 시작)
        public int m_currentTsmedoStep = 0; // 현재 진행중인 ts매도 차수를 저장하는 변수
        public int m_currentTakeProfitStep = 0; //현재 진행중인 익절매도 차수 저장변수
        public int m_currentStopLossStep = 0;//현재 진행중인 손절매도 차수 저장변수

        public int m_buyingCount = 0; // 처음매수 + 추매 횟수
        public int m_tsMedoCount = 0; // ts매도 횟수
        public int m_takeProfitCount = 0; // 익절매도횟수(최대5회)
        public int m_stopLossCount = 0; //손절횟수
        public double[] m_buyingPerInvestment = new double[6]; // %별로 계산을 한다.
        public int[,] calINdicatorNumber = { { -1, -1 }, { -1, -1 }, { -1, -1 }, { -1, -1 } };
        //calINdicatorNumber[0, 0] = -1
        //calINdicatorNumber[0, 1] = -1
        //calINdicatorNumber[1, 0] = -1
        //calINdicatorNumber[1, 1] = -1
        //calINdicatorNumber[2, 0] = -1
        //calINdicatorNumber[2, 1] = -1
        //calINdicatorNumber[3, 0] = -1
        //calINdicatorNumber[3, 1] = -1
        public int[] calBongTypeNumber = { 0, 0, 0, 0 };

        public Timer _orderCancellationTimer;
        public Timer _orderModificationTimer;

        public MyTradingItem(MyTradingCondition mtc, string m_conditionName, string m_conditionNumber, string m_itemCode, double m_transferPrice, double m_currentPrice, double m_upperLimitPrice)
        {
            this.m_conditionName = m_conditionName;
            this.m_conditionNumber = m_conditionNumber;
            this.m_itemCode = m_itemCode;
            this.m_itemName = gMainForm.KiwoomAPI.GetMasterCodeName(m_itemCode);
            this.m_transferPrice = m_transferPrice;
            this.m_currentPrice = m_currentPrice;
            this.m_upperLimitPrice = m_upperLimitPrice;

            for (int i = 0; i < 6; i++)
                m_rePurchaseArray[i] = false;

            m_tradingTime[0] = string.Empty;
            m_tradingTime[1] = string.Empty;

            // 조건식 내용을 셋팅한다.
            // 매수횟수 
            m_buyingCount = mtc.buyingCount;
            m_tsMedoCount = mtc.tsMedoCount;
            m_takeProfitCount = mtc.takeProfitCount;
            m_stopLossCount = mtc.stopLossCount;
            // 매수금액
            for (int j = 0; j < 6; j++)
            {
                m_buyingPerInvestment[j] = mtc.buyingInvestment[j];
            }
            ///////////////////////////////////////////////// 매수 /////////////////////////////////////////////////            
            m_buyingUsing = mtc.buyingUsing;// 매수사용체크박스
            m_buyingType = mtc.buyingType;
            if (m_buyingType == 0) // 기본매수
            {
                m_mesuoption1 = mtc.mesuoption1;
                m_buyingTransferType = mtc.buyingTransferType;
                if (m_buyingTransferType == 0) // 편입즉시매수
                {
                    if(m_mesuoption1 == 1) // 지정가
                    {
                        m_nontradingtime = mtc.nontradingtime;
                        m_mesuoption2 = mtc.mesuoption2;
                    }
                }
                else if(m_buyingTransferType ==1) // 편입후 n% 매수
                {
                    // 대비 n%
                    m_buyingTransferPer = mtc.buyingTransferPer;
                    // 이하, 이상
                    m_buyingTransferUpdown = mtc.buyingTransferUpdown;

                    if (m_mesuoption1 == 1) // 지정가
                    {
                        m_nontradingtime = mtc.nontradingtime;
                        m_mesuoption2 = mtc.mesuoption2;
                    }
                }
                else if(m_buyingTransferType == 2)
                {
                    m_buyingTransferPer2 = mtc.buyingTransferPer2;
                    m_riseTransferPer2 = mtc.riseTransferPer2;

                    if (m_mesuoption1 == 1) // 지정가
                    {
                        m_nontradingtime = mtc.nontradingtime;
                        m_mesuoption2 = mtc.mesuoption2;
                    }
                }
            }
            else if (m_buyingType == 1) // 이동평균선
            {
                // 이평종류 0:단순, 1:지수                
                m_buyingMovePriceKindType = mtc.buyingMovePriceKindType;
                // 일,분봉(0,1)
                m_buyingBongType = mtc.buyingBongType;
                // 1,3,5,10,20 등등 분봉
                m_buyingMinuteType = mtc.buyingMinuteType;
                // 1,3,5,20 이평종류
                m_buyingMinuteLineType = mtc.buyingMinuteLineType;
                // n%
                m_buyingMinuteLineAccessPer = mtc.buyingMinuteLineAccessPer;
                // 0:근접, 1:돌파
                m_buyingDistance = mtc.buyingDistance;
            }
            else if (m_buyingType == 2) // 스토캐스틱
            {
                // 기간1
                m_buyingStocPeriod1 = mtc.buyingStocPeriod1;
                // 기간2
                m_buyingStocPeriod2 = mtc.buyingStocPeriod2;
                // 기간3
                m_buyingStocPeriod3 = mtc.buyingStocPeriod3;
                // 일,분봉(0,1)
                m_buyingBongType = mtc.buyingBongType;
                // 1,3,5,10,20 등등 분봉
                m_buyingMinuteType = mtc.buyingMinuteType;
                // n%
                m_buyingMinuteLineAccessPer = mtc.buyingMinuteLineAccessPer;
                // 0:이상, 1:이하
                m_buyingDistance = mtc.buyingDistance;
            }
            else if (m_buyingType == 3) // 볼린져밴드
            {
                // 기간
                m_buyingStocPeriod1 = mtc.buyingStocPeriod1;
                // 승수
                m_buyingBollPeriod = mtc.buyingBollPeriod;
                // 일,분봉(0,1)
                m_buyingBongType = mtc.buyingBongType;
                // 1,3,5,10,20 등등 분봉
                m_buyingMinuteType = mtc.buyingMinuteType;
                // 상한선, 중심선, 하한선(0, 1, 2)
                m_buyingLine3Type = mtc.buyingLine3Type;
                // n%
                m_buyingMinuteLineAccessPer = mtc.buyingMinuteLineAccessPer;
                // 0:근접, 1:돌파, 2:이탈
                m_buyingDistance = mtc.buyingDistance;
            }
            else if (m_buyingType == 4) // 엔벨로프
            {
                // 기간
                m_buyingStocPeriod1 = mtc.buyingStocPeriod1;
                // %
                m_buyingBollPeriod = mtc.buyingBollPeriod;
                // 일,분봉(0,1)
                m_buyingBongType = mtc.buyingBongType;
                // 1,3,5,10,20 등등 분봉
                m_buyingMinuteType = mtc.buyingMinuteType;
                // 상한선, 중심선, 하한선(0, 1, 2)
                m_buyingLine3Type = mtc.buyingLine3Type;
                // n%
                m_buyingMinuteLineAccessPer = mtc.buyingMinuteLineAccessPer;
                // 0:근접, 1:돌파, 2:이탈
                m_buyingDistance = mtc.buyingDistance;
            }
            calINdicatorNumber[0, 0] = m_buyingType;
            calBongTypeNumber[0] = m_buyingBongType;
            // 분봉 저장
            if (m_buyingType != 0 && m_buyingBongType == 1) // 기본매수가 아니면서 분봉을 사용하면
                setBunBongType(m_buyingMinuteType);
            ///////////////////////////////////////////////// 추매 /////////////////////////////////////////////////            
            m_reBuyingType = mtc.reBuyingType;

            if (m_reBuyingType == 0)
            {
                for (int i = 0; i < 5; i++)
                    m_reBuyingPer[i] = mtc.reBuyingPer[i];
            }
            else if (m_reBuyingType == 1) // 이동평균선 추매
            {
                //0:단수, 1:지수
                m_reBuyingMovePriceKindType = mtc.reBuyingMovePriceKindType;
                // 일,분봉(0,1)
                m_reBuyingBongType = mtc.reBuyingBongType;
                // 1,3,5,10,20 등등 분봉
                m_reBuyingMinuteType = mtc.reBuyingMinuteType;
                for (int i = 0; i < 5; i++)
                {
                    // 1,3,5,20이평 등등
                    m_reBuyingMinuteLineType[i] = mtc.reBuyingMinuteLineType[i];
                    // 추매 %
                    m_reBuyingPer[i] = mtc.reBuyingPer[i];
                }
            }
            calINdicatorNumber[1, 0] = m_reBuyingType;
            calBongTypeNumber[1] = m_reBuyingBongType;
            // 분봉 저장
            if (m_reBuyingType != 0 && m_reBuyingBongType == 1) // 분봉을 사용하면
                setBunBongType(m_reBuyingMinuteType);
            ///////////////////////////////////////////////// 익절 /////////////////////////////////////////////////
            m_takeProfitUsing = mtc.takeProfitUsing; // 익절 사용 여부            
            m_takeProfitType = mtc.takeProfitType;

            if (m_takeProfitType == 0) // 기본 익절
            {
                for (int i = 0; i < 5; i++)
                {
                    m_takeProfitBuyingPricePer[i] = mtc.takeProfitBuyingPricePer[i];
                    m_takeProfitProportion[i] = mtc.takeProfitProportion[i];
                }
            }
            else if (m_takeProfitType == 1) // 이동평균선
            {
                // 이평종류 0:단순, 1:지수                
                m_takeProfitMovePriceKindType = mtc.takeProfitMovePriceKindType;
                // 월,주,일,분봉(0,1,2,3)
                m_takeProfitBongType = mtc.takeProfitBongType;
                // 1,3,5,10,20 등등 분봉
                m_takeProfitMinuteType = mtc.takeProfitMinuteType;
                // 1,3,5,20 이평종류
                m_takeProfitMinuteLineType = mtc.takeProfitMinuteLineType;
                // n%
                m_takeProfitLineAccessPer = mtc.takeProfitLineAccessPer;
                // 0:근접, 1:돌파
                m_takeProfitDistance = mtc.takeProfitDistance;
            }
            else if (m_takeProfitType == 2) // 스토캐스틱SLOW
            {
                // 기간
                m_takeProfitStocPeriod1 = mtc.takeProfitStocPeriod1;
                // %K
                m_takeProfitStocPeriod2 = mtc.takeProfitStocPeriod2;
                // %D
                m_takeProfitStocPeriod3 = mtc.takeProfitStocPeriod3;
                // 월,주,일,분봉(0,1,2,3)
                m_takeProfitBongType = mtc.takeProfitBongType;
                // 1,3,5,10,20 등등 분봉
                m_takeProfitMinuteType = mtc.takeProfitMinuteType;
                // k값
                m_takeProfitLineAccessPer = mtc.takeProfitLineAccessPer;
                // 0:이상, 1:이하
                m_takeProfitDistance = mtc.takeProfitDistance;
            }
            else if (m_takeProfitType == 3) // 볼린저밴드
            {
                // 기간
                m_takeProfitStocPeriod1 = mtc.takeProfitStocPeriod1;
                // 승수
                m_takeProfitBollPeriod = mtc.takeProfitBollPeriod;
                // 월,주,일,분봉(0,1,2,3)
                m_takeProfitBongType = mtc.takeProfitBongType;
                // 1,3,5,10,20 등등 분봉
                m_takeProfitMinuteType = mtc.takeProfitMinuteType;
                // 상한선, 중심선, 하한선(0, 1, 2)
                m_takeProfitLine3Type = mtc.takeProfitLine3Type;
                // n%
                m_takeProfitLineAccessPer = mtc.takeProfitLineAccessPer;
                // 0:근접, 1:돌파, 2:이탈
                m_takeProfitDistance = mtc.takeProfitDistance;
            }
            else if (m_takeProfitType == 4) // 엔벨로프
            {
                // 기간
                m_takeProfitStocPeriod1 = mtc.takeProfitStocPeriod1;
                // 승수
                m_takeProfitBollPeriod = mtc.takeProfitBollPeriod;
                // 일,분봉(0,1)
                m_takeProfitBongType = mtc.takeProfitBongType;
                // 1,3,5,10,20 등등 분봉
                m_takeProfitMinuteType = mtc.takeProfitMinuteType;
                // 상한선, 중심선, 하한선(0, 1, 2)
                m_takeProfitLine3Type = mtc.takeProfitLine3Type;
                // n%
                m_takeProfitLineAccessPer = mtc.takeProfitLineAccessPer;
                // 0:근접, 1:돌파, 2:이탈
                m_takeProfitDistance = mtc.takeProfitDistance;
            }
            calINdicatorNumber[2, 0] = m_takeProfitType;
            calBongTypeNumber[2] = m_takeProfitBongType;

            // 분봉 저장
            if (m_takeProfitType != 0 && m_takeProfitBongType == 1) // 분봉을 사용하면
                setBunBongType(m_takeProfitMinuteType);

            ///////////////////////////////////////////////// 손절 /////////////////////////////////////////////////
            m_stopLossUsing = mtc.stopLossUsing; // 손절 사용 여부            
            m_stopLossType = mtc.stopLossType;

            if (m_stopLossType == 0) // 기본손절
            {
                for(int i=0; i<5; i++)
                {
                    m_stopLossBuyingPricePer[i] = mtc.stopLossBuyingPricePer[i];
                    m_stopLossProportion[i] = mtc.stopLossProportion[i];
                }
            }
            else if (m_stopLossType == 1) // 이동평균선 손절
            {
                // 이평종류 0:단순, 1:지수                
                m_stopLossMovePriceKindType = mtc.stopLossMovePriceKindType;
                // 월,주,일,분봉(0,1,2,3)
                m_stopLossBongType = mtc.stopLossBongType;
                // 1,3,5,10,20 등등 분봉
                m_stopLossMinuteType = mtc.stopLossMinuteType;
                // 1,3,5,20 이평종류
                m_stopLossMinuteLineType = mtc.stopLossMinuteLineType;
                // n%
                m_stopLossLineAccessPer = mtc.stopLossLineAccessPer;
                // 0:근접, 1:돌파
                m_stopLossDistance = mtc.stopLossDistance;
            }
            else if (m_stopLossType == 2) // 볼린저벤드
            {
                // 기간
                m_stopLossStocPeriod1 = mtc.stopLossStocPeriod1;
                // 승수
                m_stopLossBollPeriod = mtc.stopLossBollPeriod;
                // 월,주,일,분봉(0,1,2,3)
                m_stopLossBongType = mtc.stopLossBongType;
                // 1,3,5,10,20 등등 분봉
                m_stopLossMinuteType = mtc.stopLossMinuteType;
                // 상한선, 중심선, 하한선(0, 1, 2)
                m_stopLossLine3Type = mtc.stopLossLine3Type;
                // n%
                m_stopLossLineAccessPer = mtc.stopLossLineAccessPer;
                // 0:근접, 1:돌파, 2:이탈
                m_stopLossDistance = mtc.stopLossDistance;
            }
            else if (m_stopLossType == 3) // 엔벨로프
            {
                // 기간
                m_stopLossStocPeriod1 = mtc.stopLossStocPeriod1;
                // 승수
                m_stopLossBollPeriod = mtc.stopLossBollPeriod;
                // 월,주,일,분봉(0,1,2,3)
                m_stopLossBongType = mtc.stopLossBongType;
                // 1,3,5,10,20 등등 분봉
                m_stopLossMinuteType = mtc.stopLossMinuteType;
                // 상한선, 중심선, 하한선(0, 1, 2)
                m_stopLossLine3Type = mtc.stopLossLine3Type;
                // n%
                m_stopLossLineAccessPer = mtc.stopLossLineAccessPer;
                // 0:근접, 1:돌파, 2:이탈
                m_stopLossDistance = mtc.stopLossDistance;
            }
            calINdicatorNumber[3, 0] = m_stopLossType;
            calBongTypeNumber[3] = m_stopLossBongType;
            if (m_stopLossType != 0 && m_stopLossBongType == 1) // 분봉을 사용하면
                setBunBongType(m_stopLossMinuteType);
            ///////////////////////////////////////////////// ts매도 /////////////////////////////////////////////////
            m_tsmedoUsing = mtc.tsMedoUsing;
            for (int i = 0; i < 3; i++)
            {
                m_tsMedoUsingType[i] = mtc.tsMedoUsingType[i];
                m_tsMedoArchievedPer[i] = mtc.tsMedoAchievedPer[i];
                m_tsMedoPercent[i] = mtc.tsMedoPercent[i];
                m_tsMedoProportion[i] = mtc.tsMedoProportion[i];
            }
            m_tsProfitPreservation1 = mtc.tsProfitPreservation1;
            m_tsProfitPreservation2 = mtc.tsProfitPreservation2;
            m_tsProfitPreservation3 = mtc.tsProfitPreservation3;

            // WinForms Timer 생성
            _orderCancellationTimer = new Timer();
            // 타이머 간격 설정 (예: 1000ms = 1초)
            // 실제 사용 시 환경에 따라 조정 가능
            _orderCancellationTimer.Interval = 300;
            // 타이머 Tick 이벤트에 메서드 연결
            // Tick 이벤트는 Interval마다 한 번씩 호출된다.
            _orderCancellationTimer.Tick += _orderCancellationTimer_TIck; //일괄취소전용
            // 타이머 중지(최초에는 중지상태)
            _orderCancellationTimer.Stop();

            // WinForms Timer 생성
            _orderModificationTimer = new Timer();
            // 타이머 간격 설정 (예: 1000ms = 1초)
            // 실제 사용 시 환경에 따라 조정 가능
            _orderModificationTimer.Interval = 300;
            // 타이머 Tick 이벤트에 메서드 연결
            // Tick 이벤트는 Interval마다 한 번씩 호출된다.
            _orderModificationTimer.Tick += _orderModificationTimer_TIck; //일괄정정전용
            // 타이머 중지(최초에는 중지상태)
            _orderModificationTimer.Stop();

            // 분봉의 간격을 계산해서 넣는것
            // bb_interva[]값을 넣는 과정이라고 생각하면될듯.
            // 예를들어 1분봉의 경우 bb_interval[0,0,0] = 900 , bb_interval[0,0,1] =901 . . .bb_interval[0,381,0] = 1535, bb_interval[0,381,1] = 1536
            int[] intervalBong = { 1, 3, 5, 10, 15, 30, 45, 60 }; //계산하는 분봉 리스트(1분봉,3분봉,...60분봉)
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 400; j++)
                {
                    if (j < intervalArrayCount[i] - 2)
                    {
                        int cal = j * intervalBong[i]; //분봉간격증가(이건 분이기때문에 시간단위로 다시계산해야해 -> ex)i가3 j가36인경우 360이나옴(이건360분 ->6시간)이므로 600으로 계산
                        int cal2 = j * intervalBong[i] + intervalBong[i]; //다음 분봉간격 증가

                        // 시간 간격계산
                        if (cal >= 60 - intervalBong[i])
                            cal = ((cal / 60) * 100) + (cal % 60);

                        if (cal2 >= 60 - intervalBong[i])
                            cal2 = ((cal2 / 60) * 100) + (cal2 % 60);

                        bb_interval[i, j, 0] = 900 + cal; //현재시간
                        bb_interval[i, j, 1] = 900 + cal2; //다음시간
                    }
                    else if (j == intervalArrayCount[i] - 2) // 끝에서 2번째 봉
                    {
                        int[,] timelist = { {1530,1535},
                                            {1530,1533},
                                            {1530,1535},
                                            {1510,1530},
                                            {1515,1530},
                                            {1500,1530},
                                            {1415,1500},
                                            {1400,1500}
                                            };
                        bb_interval[i, j, 0] = timelist[i, 0];
                        bb_interval[i, j, 1] = timelist[i, 1];
                    }
                    else if (j == intervalArrayCount[i] - 1) // 마지막봉
                    {
                        int[,] timelist = { {1535,1536},
                                            {1533,1536},
                                            {1535,1540},
                                            {1530,1540},
                                            {1530,1545},
                                            };
                        if (i < 5)
                        {
                            bb_interval[i, j, 0] = timelist[i, 0];
                            bb_interval[i, j, 1] = timelist[i, 1];
                        }
                        break;
                    }
                }
            }
            // 45분봉 interval -> 10시에 장 시작하는 경우에 대한 예외처리 배열 // 10시장 시작인경우 45분봉은 총8개(원래는9개)
            for (int i = 0; i < 400; i++)
            {
                if (i < 7)
                {
                    int cal = i * 45;
                    int cal2 = i * 45 + 45;

                    if (cal >= 60 - 45)
                        cal = ((cal / 60) * 100) + (cal & 60);
                    if (cal2 >= 60 - 45)
                        cal2 = ((cal2 / 60) * 100) + (cal2 & 60);

                    bb_exceptionInterval[i, 0] = 1000 + cal;
                    bb_exceptionInterval[i, 1] = 1000 + cal2;
                }
                else if (i == 7)
                {
                    bb_exceptionInterval[i, 0] = 1515;
                    bb_exceptionInterval[i, 1] = 1600;
                    break;
                }
            }

        }
        public void _orderCancellationTimer_TIck(object sender, EventArgs e)
        {
            gMainForm.gTradingManager.CheckUnconcludedCancelOrders();
        }

        public void _orderModificationTimer_TIck(object sender, EventArgs e)
        {
            gMainForm.gTradingManager.CheckUnconcludedDesignationOrders();
        }

        //조건식을 수정하는 경우 다시 설정하기
        public void reSetConditionData(MyTradingCondition mtc)
        {
            m_buyingCount = mtc.buyingCount;
            m_tsMedoCount = mtc.tsMedoCount;
            m_takeProfitCount = mtc.takeProfitCount;
            m_stopLossCount = mtc.stopLossCount;
            // 매수금액
            for (int j = 0; j < 6; j++)
            {
                m_buyingPerInvestment[j] = mtc.buyingInvestment[j];
            }
            ///////////////////////////////////////////////// 매수 /////////////////////////////////////////////////            
            m_buyingUsing = mtc.buyingUsing;// 매수사용체크박스
            m_buyingType = mtc.buyingType;
            m_mesuoption1 = mtc.mesuoption1;
            if (m_buyingType == 0) // 기본매수
            {
                if(m_mesuoption1 == 0)
                {
                    m_buyingTransferType = mtc.buyingTransferType;
                    if (m_buyingTransferType == 0) // 편입즉시매수
                    {

                    }
                    else // 편입후 n% 매수
                    {
                        // 대비 n%
                        m_buyingTransferPer = mtc.buyingTransferPer;
                        // 이하, 이상
                        m_buyingTransferUpdown = mtc.buyingTransferUpdown;
                    }
                }
                else if(m_mesuoption1 == 1)
                {
                    m_nontradingtime = mtc.nontradingtime;
                    m_mesuoption2 = mtc.mesuoption2;
                }

            }
            else if (m_buyingType == 1) // 이동평균선
            {
                // 이평종류 0:단순, 1:지수                
                m_buyingMovePriceKindType = mtc.buyingMovePriceKindType;
                // 일,분봉(0,1)
                m_buyingBongType = mtc.buyingBongType;
                // 1,3,5,10,20 등등 분봉
                m_buyingMinuteType = mtc.buyingMinuteType;
                // 1,3,5,20 이평종류
                m_buyingMinuteLineType = mtc.buyingMinuteLineType;
                // n%
                m_buyingMinuteLineAccessPer = mtc.buyingMinuteLineAccessPer;
                // 0:근접, 1:돌파
                m_buyingDistance = mtc.buyingDistance;
            }
            else if (m_buyingType == 2) // 스토캐스틱
            {
                // 기간
                m_buyingStocPeriod1 = mtc.buyingStocPeriod1;
                // %K
                m_buyingStocPeriod2 = mtc.buyingStocPeriod2;
                // %D
                m_buyingStocPeriod3 = mtc.buyingStocPeriod3;
                // 일,분봉(0,1)
                m_buyingBongType = mtc.buyingBongType;
                // 1,3,5,10,20 등등 분봉
                m_buyingMinuteType = mtc.buyingMinuteType;
                // n%
                m_buyingMinuteLineAccessPer = mtc.buyingMinuteLineAccessPer;
                // 0:이상, 1:이하
                m_buyingDistance = mtc.buyingDistance;
            }
            else if (m_buyingType == 3) // 볼린져밴드
            {
                // 기간
                m_buyingStocPeriod1 = mtc.buyingStocPeriod1;
                // 승수
                m_buyingBollPeriod = mtc.buyingBollPeriod;
                // 일,분봉(0,1)
                m_buyingBongType = mtc.buyingBongType;
                // 1,3,5,10,20 등등 분봉
                m_buyingMinuteType = mtc.buyingMinuteType;
                // 상한선, 중심선, 하한선(0, 1, 2)
                m_buyingLine3Type = mtc.buyingLine3Type;
                // n%
                m_buyingMinuteLineAccessPer = mtc.buyingMinuteLineAccessPer;
                // 0:근접, 1:돌파, 2:이탈
                m_buyingDistance = mtc.buyingDistance;
            }
            else if (m_buyingType == 4) // 엔벨로프
            {
                // 기간
                m_buyingStocPeriod1 = mtc.buyingStocPeriod1;
                // %
                m_buyingBollPeriod = mtc.buyingBollPeriod;
                // 일,분봉(0,1)
                m_buyingBongType = mtc.buyingBongType;
                // 1,3,5,10,20 등등 분봉
                m_buyingMinuteType = mtc.buyingMinuteType;
                // 상한선, 중심선, 하한선(0, 1, 2)
                m_buyingLine3Type = mtc.buyingLine3Type;
                // n%
                m_buyingMinuteLineAccessPer = mtc.buyingMinuteLineAccessPer;
                // 0:근접, 1:돌파, 2:이탈
                m_buyingDistance = mtc.buyingDistance;
            }

            calINdicatorNumber[0, 0] = m_buyingType;
            calBongTypeNumber[0] = m_buyingBongType;
            // 분봉 저장
            if (m_buyingType != 0 && m_buyingBongType == 1) // 분봉을 사용하면
                setBunBongType(m_buyingMinuteType);
            ///////////////////////////////////////////////// 추매 /////////////////////////////////////////////////            
            m_reBuyingType = mtc.reBuyingType;

            if (m_reBuyingType == 0)
            {
                for (int i = 0; i < 5; i++)
                    m_reBuyingPer[i] = mtc.reBuyingPer[i];
            }
            else if (m_reBuyingType == 1) // 이동평균선 추매
            {
                //0:단수, 1:지수
                m_reBuyingMovePriceKindType = mtc.reBuyingMovePriceKindType;
                // 일,분봉(0,1)
                m_reBuyingBongType = mtc.reBuyingBongType;
                // 1,3,5,10,20 등등 분봉
                m_reBuyingMinuteType = mtc.reBuyingMinuteType;
                for (int i = 0; i < 5; i++)
                {
                    // 1,3,5,20이평 등등
                    m_reBuyingMinuteLineType[i] = mtc.reBuyingMinuteLineType[i];
                    // 추매 %
                    m_reBuyingPer[i] = mtc.reBuyingPer[i];
                }
            }
            calINdicatorNumber[1, 0] = m_reBuyingType;
            calBongTypeNumber[1] = m_reBuyingBongType;
            // 분봉 저장
            if (m_reBuyingType != 0 && m_reBuyingBongType == 1) // 분봉을 사용하면
                setBunBongType(m_reBuyingMinuteType);
            ///////////////////////////////////////////////// 익절 /////////////////////////////////////////////////
            m_takeProfitUsing = mtc.takeProfitUsing; // 익절 사용 여부            
            m_takeProfitType = mtc.takeProfitType;

            if (m_takeProfitType == 0) // 기본 익절
            {
                // 매수단가대비 n%
                m_takeProfitBuyingPricePer = mtc.takeProfitBuyingPricePer;
                m_takeProfitProportion = mtc.takeProfitProportion;
            }
            else if (m_takeProfitType == 1) // 이동평균선
            {
                // 이평종류 0:단순, 1:지수                
                m_takeProfitMovePriceKindType = mtc.takeProfitMovePriceKindType;
                // 월,주,일,분봉(0,1,2,3)
                m_takeProfitBongType = mtc.takeProfitBongType;
                // 1,3,5,10,20 등등 분봉
                m_takeProfitMinuteType = mtc.takeProfitMinuteType;
                // 1,3,5,20 이평종류
                m_takeProfitMinuteLineType = mtc.takeProfitMinuteLineType;
                // n%
                m_takeProfitLineAccessPer = mtc.takeProfitLineAccessPer;
                // 0:근접, 1:돌파
                m_takeProfitDistance = mtc.takeProfitDistance;
            }
            else if (m_takeProfitType == 2) // 스토캐스틱SLOW
            {
                // 기간
                m_takeProfitStocPeriod1 = mtc.takeProfitStocPeriod1;
                // %K
                m_takeProfitStocPeriod2 = mtc.takeProfitStocPeriod2;
                // %D
                m_takeProfitStocPeriod3 = mtc.takeProfitStocPeriod3;
                // 월,주,일,분봉(0,1,2,3)
                m_takeProfitBongType = mtc.takeProfitBongType;
                // 1,3,5,10,20 등등 분봉
                m_takeProfitMinuteType = mtc.takeProfitMinuteType;
                // k값
                m_takeProfitLineAccessPer = mtc.takeProfitLineAccessPer;
                // 0:이상, 1:이하
                m_takeProfitDistance = mtc.takeProfitDistance;
            }
            else if (m_takeProfitType == 3) // 볼린저밴드
            {
                // 기간
                m_takeProfitStocPeriod1 = mtc.takeProfitStocPeriod1;
                // 승수
                m_takeProfitBollPeriod = mtc.takeProfitBollPeriod;
                // 월,주,일,분봉(0,1,2,3)
                m_takeProfitBongType = mtc.takeProfitBongType;
                // 1,3,5,10,20 등등 분봉
                m_takeProfitMinuteType = mtc.takeProfitMinuteType;
                // 상한선, 중심선, 하한선(0, 1, 2)
                m_takeProfitLine3Type = mtc.takeProfitLine3Type;
                // n%
                m_takeProfitLineAccessPer = mtc.takeProfitLineAccessPer;
                // 0:근접, 1:돌파, 2:이탈
                m_takeProfitDistance = mtc.takeProfitDistance;
            }
            else if (m_takeProfitType == 4) // 엔벨로프
            {
                // 기간
                m_takeProfitStocPeriod1 = mtc.takeProfitStocPeriod1;
                // 승수
                m_takeProfitBollPeriod = mtc.takeProfitBollPeriod;
                // 일,분봉(0,1)
                m_takeProfitBongType = mtc.takeProfitBongType;
                // 1,3,5,10,20 등등 분봉
                m_takeProfitMinuteType = mtc.takeProfitMinuteType;
                // 상한선, 중심선, 하한선(0, 1, 2)
                m_takeProfitLine3Type = mtc.takeProfitLine3Type;
                // n%
                m_takeProfitLineAccessPer = mtc.takeProfitLineAccessPer;
                // 0:근접, 1:돌파, 2:이탈
                m_takeProfitDistance = mtc.takeProfitDistance;
            }
            calINdicatorNumber[2, 0] = m_takeProfitType;
            calBongTypeNumber[2] = m_takeProfitBongType;

            // 분봉 저장
            if (m_takeProfitType != 0 && m_takeProfitBongType == 1) // 분봉을 사용하면
                setBunBongType(m_takeProfitMinuteType); // m_takeProfitMinuteType: 0:1분봉 , 1:3분봉, 2: 5분봉

            ///////////////////////////////////////////////// 손절 /////////////////////////////////////////////////
            m_stopLossUsing = mtc.stopLossUsing; // 손절 사용 여부            
            m_stopLossType = mtc.stopLossType;

            if (m_stopLossType == 0) // 기본손절
            {
                // 매수단가대비 n%
                m_stopLossBuyingPricePer = mtc.stopLossBuyingPricePer;
                m_stopLossProportion = mtc.stopLossProportion;
            }
            else if (m_stopLossType == 1) // 이동평균선 손절
            {
                // 이평종류 0:단순, 1:지수                
                m_stopLossMovePriceKindType = mtc.stopLossMovePriceKindType;
                // 월,주,일,분봉(0,1,2,3)
                m_stopLossBongType = mtc.stopLossBongType;
                // 1,3,5,10,20 등등 분봉
                m_stopLossMinuteType = mtc.stopLossMinuteType;
                // 1,3,5,20 이평종류
                m_stopLossMinuteLineType = mtc.stopLossMinuteLineType;
                // n%
                m_stopLossLineAccessPer = mtc.stopLossLineAccessPer;
                // 0:근접, 1:돌파
                m_stopLossDistance = mtc.stopLossDistance;
            }
            else if (m_stopLossType == 2) // 볼린저벤드
            {
                // 기간
                m_stopLossStocPeriod1 = mtc.stopLossStocPeriod1;
                // 승수
                m_stopLossBollPeriod = mtc.stopLossBollPeriod;
                // 월,주,일,분봉(0,1,2,3)
                m_stopLossBongType = mtc.stopLossBongType;
                // 1,3,5,10,20 등등 분봉
                m_stopLossMinuteType = mtc.stopLossMinuteType;
                // 상한선, 중심선, 하한선(0, 1, 2)
                m_stopLossLine3Type = mtc.stopLossLine3Type;
                // n%
                m_stopLossLineAccessPer = mtc.stopLossLineAccessPer;
                // 0:근접, 1:돌파, 2:이탈
                m_stopLossDistance = mtc.stopLossDistance;
            }
            else if (m_stopLossType == 3) // 엔벨로프
            {
                // 기간
                m_stopLossStocPeriod1 = mtc.stopLossStocPeriod1;
                // 승수
                m_stopLossBollPeriod = mtc.stopLossBollPeriod;
                // 월,주,일,분봉(0,1,2,3)
                m_stopLossBongType = mtc.stopLossBongType;
                // 1,3,5,10,20 등등 분봉
                m_stopLossMinuteType = mtc.stopLossMinuteType;
                // 상한선, 중심선, 하한선(0, 1, 2)
                m_stopLossLine3Type = mtc.stopLossLine3Type;
                // n%
                m_stopLossLineAccessPer = mtc.stopLossLineAccessPer;
                // 0:근접, 1:돌파, 2:이탈
                m_stopLossDistance = mtc.stopLossDistance;
            }
            calINdicatorNumber[3, 0] = m_stopLossType;
            calBongTypeNumber[3] = m_stopLossBongType;
            if (m_stopLossType != 0 && m_stopLossBongType == 1) // 분봉을 사용하면
                setBunBongType(m_stopLossMinuteType);

            ///////////////////////////////////////////////// ts매도 /////////////////////////////////////////////////
            m_tsmedoUsing = mtc.tsMedoUsing;
            for (int i = 0; i < 3; i++)
            {
                m_tsMedoUsingType[i] = mtc.tsMedoUsingType[i];
                m_tsMedoArchievedPer[i] = mtc.tsMedoAchievedPer[i];
                m_tsMedoPercent[i] = mtc.tsMedoPercent[i];
                m_tsMedoProportion[i] = mtc.tsMedoProportion[i];
            }
            m_tsProfitPreservation1 = mtc.tsProfitPreservation1;
            m_tsProfitPreservation2 = mtc.tsProfitPreservation2;
            m_tsProfitPreservation3 = mtc.tsProfitPreservation3;

    }
        // 일봉 이동평균선 계산
        public void getDayBongMovingAveragePrice(int tradeType)
        {
            // tradeType 0:매수, 1:추매, 2:익절, 3:손절
            // 모두 150이상 가능하나, 45 분봉은 90, 60분봉은 70개
            // type으로 0은 단순, 1은 지수
            // 일봉상 - 600개로 550이평가능
            //3분봉 - 1500
            //5분봉 - 900
            //10분봉 - 450
            //15분봉 - 300
            //30분봉 - 150
            //45분봉 - 100
            //60분봉 - 75
            double[] evg_value = new double[600]; // 5이동 평균 가격을 계산한다.
            int calType = m_buyingMovePriceKindType;
            int movingKind = 0;

            if (tradeType == 0) // 매수
            {
                calType = m_buyingMovePriceKindType; // 단순, 지수
                movingKind = m_buyingMinuteLineType;
            }
            else if (tradeType == 2) // 익절
            {
                calType = m_takeProfitMovePriceKindType; // 단순, 지수
                movingKind = m_takeProfitMinuteLineType;
            }
            else if (tradeType == 3) // 손절
            {
                calType = m_stopLossMovePriceKindType; // 단순, 지수
                movingKind = m_stopLossMinuteLineType;
            }

            if (calType == 0) // 단순이평
            {
                if (movingKind > m_totalDayBongCount)
                    movingKind = m_totalDayBongCount;

                double totPrice1 = 0, totPrice2 = 0;
                for (int i = 0; i < movingKind; i++)
                {
                    totPrice1 += db_endPrice[i];
                    totPrice2 += db_endPrice[i + 1];
                }

                moving_PriceDayCur[tradeType] = totPrice1 / movingKind; // 현재이평
                moving_PriceDayPrev[tradeType] = totPrice2 / movingKind; // 이전이평      
            }
            else // 지수이평
            {
                if (movingKind > m_totalDayBongCount)
                    movingKind = m_totalDayBongCount;

                double _value = (double)2 / (double)(movingKind + 1);
                double _minus = (double)1 - _value;

                movingKind *= 6;
                if (movingKind > 600)
                    movingKind = 600;

                for (int i = movingKind - 1; i >= 0; i--)
                {
                    if (i == movingKind - 1)
                    {
                        evg_value[i] = db_endPrice[movingKind - 1];
                    }
                    else
                    {
                        evg_value[i] = db_endPrice[i] * _value + evg_value[i + 1] * _minus; // 5이평                        
                    }
                }
                moving_PriceDayCurE[tradeType] = evg_value[0]; // 현재이평
                moving_PriceDayPrevE[tradeType] = evg_value[1]; // 이전이평                           
            }
        }
        // 일봉 볼린저밴드 계산
        public void getDayBongBollingerPrice(int tradeType)
        {
            double[] bol_AvgThreePrice = new double[400];
            double[] bol_Avg30Price = new double[400];
            double[,] bol_DeviationTwice = new double[2, 400];
            double[] bol_DeviationTwiceAvgSqrt = new double[400];
            double[] bol_calHighPrice = new double[400]; // 최종 계산 가격
            double[] bol_calLowPrice = new double[400]; // 최종 계산 가격
            int _period = 0;
            double _k = 0;

            if (tradeType == 0) // 매수
            {
                _period = (int)m_buyingStocPeriod1;
                _k = m_buyingBollPeriod;
            }
            else if (tradeType == 2) // 익절
            {
                _period = (int)m_takeProfitStocPeriod1;
                _k = m_takeProfitBollPeriod;
            }
            else if (tradeType == 3) // 손절
            {
                _period = (int)m_stopLossStocPeriod1;
                _k = m_stopLossBollPeriod;
            }

            if (_period > m_totalDayBongCount)
                _period = m_totalDayBongCount;

            // 일별 평균가격 계산
            for (int i = 0; i < _period + 2; i++)
            {
                bol_AvgThreePrice[i] = (db_endPrice[i] + db_highPrice[i] + db_lowPrice[i]) / 3;
            }

            // 30일합의 평균
            int offset = 0;
            double total = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < _period; j++)
                {
                    total += bol_AvgThreePrice[j + i];
                }
                bol_Avg30Price[i] = total / _period;
                total = 0;
            }

            // 표준편차 구하기
            // 편차, 편차제곱 구하기            
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < _period; j++)
                {
                    bol_DeviationTwice[i, j] = bol_AvgThreePrice[j + i] - bol_Avg30Price[i];
                    bol_DeviationTwice[i, j] = Math.Abs(Math.Pow(bol_DeviationTwice[i, j], 2));
                }
            }
            // 편차제곱근의 평균
            total = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < _period; j++)
                {
                    total += bol_DeviationTwice[i, j];
                }
                bol_DeviationTwiceAvgSqrt[i] = total / _period;
                bol_DeviationTwiceAvgSqrt[i] = Math.Sqrt(bol_DeviationTwiceAvgSqrt[i]);
                // 최종 볼린져 상한가
                bol_calHighPrice[i] = bol_Avg30Price[i] + (_k * bol_DeviationTwiceAvgSqrt[i]);
                bol_calLowPrice[i] = bol_Avg30Price[i] - (_k * bol_DeviationTwiceAvgSqrt[i]);
                total = 0;
            }

            // 상한가
            bollinger_upPriceDayCur[tradeType] = bol_calHighPrice[0];
            bollinger_upPriceDayPrev[tradeType] = bol_calHighPrice[1];
            // 중심가
            bollinger_centerPriceDayCur[tradeType] = bol_Avg30Price[0];
            bollinger_centerPriceDayPrev[tradeType] = bol_Avg30Price[1];
            // 하한가
            bollinger_downPriceDayCur[tradeType] = bol_calLowPrice[0];
            bollinger_downPriceDayPrev[tradeType] = bol_calLowPrice[1];
        }
        // 일봉 엔벨로프 계산
        public void getDayBongEnvelopePrice(int tradeType)
        {
            double[] enve_AvgPrice = new double[400];
            double[] enve_UpPrice = new double[400];
            double[] enve_DownPrice = new double[400];

            int _period = 0;
            double _percent = 0;

            if (tradeType == 0) // 매수
            {
                _period = (int)m_buyingStocPeriod1;
                _percent = m_buyingBollPeriod;
            }
            else if (tradeType == 2) // 익절
            {
                _period = (int)m_takeProfitStocPeriod1;
                _percent = m_takeProfitBollPeriod;
            }
            else if (tradeType == 3) // 손절
            {
                _period = (int)m_stopLossStocPeriod1;
                _percent = m_stopLossBollPeriod;
            }

            if (_period > m_totalDayBongCount)
                _period = m_totalDayBongCount;

            for (int i = 0; i < 2; i++)
            {
                double _total = 0;
                for (int j = 0; j < _period; j++)
                {
                    _total += db_endPrice[j + i];
                }
                enve_AvgPrice[i] = _total / _period;
            }

            for (int i = 0; i < 2; i++)
            {
                enve_UpPrice[i] = enve_AvgPrice[i] + enve_AvgPrice[i] * _percent / 100;
                enve_DownPrice[i] = enve_AvgPrice[i] - enve_AvgPrice[i] * _percent / 100;
            }
            envelope_upPriceDayCur[tradeType] = enve_UpPrice[0];
            envelope_upPriceDayPrev[tradeType] = enve_UpPrice[1];
            envelope_centerPriceDayCur[tradeType] = enve_AvgPrice[0];
            envelope_centerPriceDayPrev[tradeType] = enve_AvgPrice[1];
            envelope_downPriceDayCur[tradeType] = enve_DownPrice[0];
            envelope_downPriceDayPrev[tradeType] = enve_DownPrice[1];
        }
        // 일봉 스토캐스틱 slow계산
        public void getDayBongStochasticsSlowPrice(int tradeType)
        {
            /////////////////////////////////// 스토캐스트 slow %k, %d //////////////////////////////
            double[] LowMinPrice = new double[2500];
            double[] HighMaxPrice = new double[2500];
            double[] EndMinusMinPrice = new double[2500];
            double[] MaxMinusMinPrice = new double[2500];
            double[] sumDayFirst = new double[2500];
            double[] sumDaySecond = new double[2500];
            double[] slowPerK = new double[2500];
            double[] slowPerD = new double[2500];

            int _sto1 = 0;
            int _sto2 = 0;
            double _sto3 = 0;

            if (tradeType == 0) // 매수
            {
                _sto1 = (int)m_buyingStocPeriod1;
                _sto2 = (int)m_buyingStocPeriod2;
                _sto3 = m_buyingStocPeriod3;
            }
            else if (tradeType == 2) // 익절
            {
                _sto1 = (int)m_takeProfitStocPeriod1;
                _sto2 = (int)m_takeProfitStocPeriod2;
                _sto3 = m_takeProfitStocPeriod3;
            }
            else if (tradeType == 3) // 손절
            {
                _sto1 = (int)m_stopLossStocPeriod1;
                _sto2 = (int)m_stopLossStocPeriod2;
                _sto3 = m_stopLossStocPeriod3;
            }

            int k_offset = _sto2;
            int d_offset = (int)_sto3 * 6;

            if (_sto1 > m_totalDayBongCount)
                _sto1 = m_totalDayBongCount;

            // _peroid일치 최저가 계산            
            for (int i = 0; i < k_offset + d_offset; i++) // 24 경우 _k값의 크기
            {
                double minPrice = 999999999;
                for (int j = 0; j < _sto1; j++) // 기간
                {
                    if (db_lowPrice[j + i] < minPrice)
                    {
                        minPrice = db_lowPrice[j + i];
                    }
                }
                LowMinPrice[i] = minPrice;
            }

            // _period일치 최고가 계산
            for (int i = 0; i < k_offset + d_offset; i++)
            {
                double maxPrice = 0;
                for (int j = 0; j < _sto1; j++) // 기간
                {
                    if (db_highPrice[j + i] > maxPrice)
                    {
                        maxPrice = db_highPrice[j + i];
                    }
                }
                HighMaxPrice[i] = maxPrice;
            }

            // 종가 - 12일치 최저가
            for (int i = 0; i < k_offset + d_offset; i++)
            {
                EndMinusMinPrice[i] = db_endPrice[i] - LowMinPrice[i];
            }
            // 12일 최고가-12일최저가
            for (int i = 0; i < k_offset + d_offset; i++)
            {
                MaxMinusMinPrice[i] = HighMaxPrice[i] - LowMinPrice[i];
            }

            // first, second 5일간 합계
            for (int i = 0; i < d_offset; i++) // 앞에 24개 이기 때문에 
            {
                double daySum1 = 0;
                double daySum2 = 0;
                for (int j = 0; j < _sto2; j++)
                {
                    daySum1 += EndMinusMinPrice[j + i];
                    daySum2 += MaxMinusMinPrice[j + i];
                }
                sumDayFirst[i] = daySum1;
                sumDaySecond[i] = daySum2;
            }
            // slow %K 값
            for (int i = 0; i < d_offset; i++) // 뒤에서 부터 계산을 한다.
            {
                if (sumDaySecond[i] == 0)
                    slowPerK[i] = 0;
                else
                    slowPerK[i] = (sumDayFirst[i] / sumDaySecond[i]) * 100;
            }
            double offset1 = ((double)2 / (double)(1 + _sto3));
            double offset2 = (double)1 - offset1;
            // slow %D 값은 뒤에서 부터 계산한다. 이동평균
            for (int i = d_offset - 1; i >= 0; i--) // 최소 15개필요하다.
            {
                if (i == d_offset - 1)
                {
                    slowPerD[i] = slowPerK[i];
                }
                else
                {
                    slowPerD[i] = (slowPerK[i] * offset1) + (slowPerD[i + 1] * offset2);
                }
            }
            stochastics_KPriceDayCur[tradeType] = slowPerK[0];
            stochastics_KPriceDayPrev[tradeType] = slowPerK[1];
            stochastics_DPriceDayCur[tradeType] = slowPerD[0];
            stochastics_DPriceDayPrev[tradeType] = slowPerD[1];
        }
        // 분봉 이동평균선 계산
        public void getBunBongMovingAveragePrice(int tradeType) // 단수지수, 분봉타입(1,3,5등등), 이평종류(1,3,5)
        {
            // 모두 150이상 가능하나, 45 분봉은 90, 60분봉은 70개
            // type으로 0은 단순, 1은 지수
            // 일봉상 - 600개로 550이평가능
            //3분봉 - 1500
            //5분봉 - 900
            //10분봉 - 450
            //15분봉 - 300
            //30분봉 - 150
            //45분봉 - 100
            //60분봉 - 75            
            // int[] calMovingNumber = { 3, 5, 10, 15 };
            //double[] moving_PriceCur = new double[7];
            //double[] moving_PricePrev = new double[7];
            // m_OneArrayCount
            // calBongCount[] 분봉갯수
            // %n이평값이 봉갯수보다 크면 봉갯수로 계산을 한다.
            double[] evg_value = new double[5000]; // 5이동 평균 가격을 계산한다.

            int calType = m_buyingMovePriceKindType; // 단순, 지수
            int bongMinuteType = m_buyingMinuteType;// 몇분봉인지 찾는다.            
            int movingKind = m_buyingMinuteLineType;

            if (tradeType == 0) // 매수
            {
                calType = m_buyingMovePriceKindType; // 단순, 지수
                bongMinuteType = m_buyingMinuteType;// 몇분봉인지 찾는다.            
                movingKind = m_buyingMinuteLineType;
            }
            else if (tradeType == 2) // 익절
            {
                calType = m_takeProfitMovePriceKindType; // 단순, 지수
                bongMinuteType = m_takeProfitMinuteType;// 몇분봉인지 찾는다.            
                movingKind = m_takeProfitMinuteLineType;
            }
            else if (tradeType == 3) // 손절
            {
                calType = m_stopLossMovePriceKindType; // 단순, 지수
                bongMinuteType = m_stopLossMinuteType;// 몇분봉인지 찾는다.            
                movingKind = m_stopLossMinuteLineType;
            }

            if (calType == 0) // 단순이평
            {
                if (movingKind > calBongCount[bongMinuteType])
                    movingKind = calBongCount[bongMinuteType];

                double totPrice1 = 0, totPrice2 = 0;
                for (int i = 0; i < movingKind; i++)
                {
                    totPrice1 += bb_endPrice[bongMinuteType, i];
                    totPrice2 += bb_endPrice[bongMinuteType, i + 1];
                }
                moving_PriceCur[tradeType] = totPrice1 / movingKind; // 현재이평
                moving_PricePrev[tradeType] = totPrice2 / movingKind; // 이전이평          
            }
            else // 지수이평
            {
                double _value = (double)2 / (double)(movingKind + 1);
                double _minus = (double)1 - _value;

                // 정확도를 높이기 위해 6배수 이전부터 계산한다.
                movingKind *= 6;

                if (movingKind > calBongCount[bongMinuteType])
                    movingKind = calBongCount[bongMinuteType];

                for (int i = movingKind - 1; i >= 0; i--)
                {
                    if (i == movingKind - 1)
                    {
                        evg_value[i] = bb_endPrice[bongMinuteType, movingKind - 1];
                    }
                    else
                    {
                        evg_value[i] = (bb_endPrice[bongMinuteType, i] * _value) + (evg_value[i + 1] * _minus); // 5이평                        
                    }
                }
                moving_PriceCurE[tradeType] = evg_value[0]; // 현재이평
                moving_PricePrevE[tradeType] = evg_value[1]; // 이전이평                               
            }
        }
        // 분봉 볼린저밴드 계산
        public void getBunBongBollingerPrice(int tradeType) // 상한선
        {
            double[] bol_AvgThreePrice = new double[400];
            double[] bol_Avg30Price = new double[400];
            double[,] bol_DeviationTwice = new double[2, 400];
            double[] bol_DeviationTwiceAvgSqrt = new double[400];
            double[] bol_calHighPrice = new double[400]; // 최종 계산 가격
            double[] bol_calLowPrice = new double[400]; // 최종 계산 가격

            int bongMinuteType = 0;
            int _period = 0;
            double _k = 0;

            if (tradeType == 0) // 매수
            {
                _period = (int)m_buyingStocPeriod1;
                _k = m_buyingBollPeriod;
                bongMinuteType = m_buyingMinuteType;// 몇분봉인지 찾는다.            
            }
            else if (tradeType == 2) // 익절
            {
                _period = (int)m_takeProfitStocPeriod1;
                _k = m_takeProfitBollPeriod;
                bongMinuteType = m_takeProfitMinuteType;// 몇분봉인지 찾는다.            
            }
            else if (tradeType == 3) // 손절
            {
                _period = (int)m_stopLossStocPeriod1;
                _k = m_stopLossBollPeriod;
                bongMinuteType = m_stopLossMinuteType;// 몇분봉인지 찾는다.            
            }

            if (_period > calBongCount[bongMinuteType])
                _period = calBongCount[bongMinuteType];

            // 일별 평균가격 계산
            for (int i = 0; i < _period + 2; i++)
            {
                bol_AvgThreePrice[i] = (bb_endPrice[bongMinuteType, i] + bb_highPrice[bongMinuteType, i] + bb_lowPrice[bongMinuteType, i]) / 3;
            }

            // 합의 평균
            double total = 0;
            for (int i = 0; i < 2; i++)
            {
                total = 0;
                for (int j = 0; j < _period; j++)
                {
                    total += bol_AvgThreePrice[j + i];
                }
                bol_Avg30Price[i] = total / _period;
            }

            // 표준편차 구하기
            // 편차, 편차제곱 구하기            
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < _period; j++)
                {
                    bol_DeviationTwice[i, j] = bol_AvgThreePrice[j + i] - bol_Avg30Price[i];
                    bol_DeviationTwice[i, j] = Math.Abs(Math.Pow(bol_DeviationTwice[i, j], 2));
                }
            }
            // 편차제곱근의 평균            
            for (int i = 0; i < 2; i++)
            {
                total = 0;
                for (int j = 0; j < _period; j++)
                {
                    total += bol_DeviationTwice[i, j];
                }
                bol_DeviationTwiceAvgSqrt[i] = total / _period;
                bol_DeviationTwiceAvgSqrt[i] = Math.Sqrt(bol_DeviationTwiceAvgSqrt[i]);
                // 최종 볼린져 상한가
                bol_calHighPrice[i] = bol_Avg30Price[i] + (_k * bol_DeviationTwiceAvgSqrt[i]);
                bol_calLowPrice[i] = bol_Avg30Price[i] - (_k * bol_DeviationTwiceAvgSqrt[i]);
            }

            // 상한가
            bollinger_upPriceCur[tradeType] = bol_calHighPrice[0];
            bollinger_upPricePrev[tradeType] = bol_calHighPrice[1];
            // 중심가
            bollinger_centerPriceCur[tradeType] = bol_Avg30Price[0];
            bollinger_centerPricePrev[tradeType] = bol_Avg30Price[1];
            // 하한가
            bollinger_downPriceCur[tradeType] = bol_calLowPrice[0];
            bollinger_downPricePrev[tradeType] = bol_calLowPrice[1];
        }
        // 분봉 엔벨로프 계산
        public void getBunBongEnvelopePrice(int tradeType)
        {
            double[] enve_AvgPrice = new double[400];
            double[] enve_UpPrice = new double[400];
            double[] enve_DownPrice = new double[400];

            int bongMinuteType = 0;
            int _period = 0;
            double _percent = 0;

            if (tradeType == 0) // 매수
            {
                _period = (int)m_buyingStocPeriod1;
                _percent = m_buyingBollPeriod;
                bongMinuteType = m_buyingMinuteType;// 몇분봉인지 찾는다.            
            }
            else if (tradeType == 2) // 익절
            {
                _period = (int)m_takeProfitStocPeriod1;
                _percent = m_takeProfitBollPeriod;
                bongMinuteType = m_takeProfitMinuteType;// 몇분봉인지 찾는다.            
            }
            else if (tradeType == 3) // 손절
            {
                _period = (int)m_stopLossStocPeriod1;
                _percent = m_stopLossBollPeriod;
                bongMinuteType = m_stopLossMinuteType;// 몇분봉인지 찾는다.            
            }

            if (_period > calBongCount[bongMinuteType])
                _period = calBongCount[bongMinuteType];

            for (int i = 0; i < 2; i++)
            {
                double _total = 0;
                for (int j = 0; j < _period; j++)
                {
                    _total += bb_endPrice[bongMinuteType, j + i];
                }
                enve_AvgPrice[i] = _total / _period; // 20일간의 종가평균
            }

            for (int i = 0; i < 2; i++)
            {
                enve_UpPrice[i] = enve_AvgPrice[i] + enve_AvgPrice[i] * _percent / 100;
                enve_DownPrice[i] = enve_AvgPrice[i] - enve_AvgPrice[i] * _percent / 100;
            }
            envelope_upPriceCur[tradeType] = enve_UpPrice[0];
            envelope_upPricePrev[tradeType] = enve_UpPrice[1];
            envelope_centerPriceCur[tradeType] = enve_AvgPrice[0];
            envelope_centerPricePrev[tradeType] = enve_AvgPrice[1];
            envelope_downPriceCur[tradeType] = enve_DownPrice[0];
            envelope_downPricePrev[tradeType] = enve_DownPrice[1];
        }
        // 분봉 스토캐스틱slow 계산
        public void getBunBongStochasticsSlowPrice(int tradeType)
        {
            /////////////////////////////////// 스토캐스트 slow %k, %d //////////////////////////////
            double[] LowMinPrice = new double[2500];
            double[] HighMaxPrice = new double[2500];
            double[] EndMinusMinPrice = new double[2500];
            double[] MaxMinusMinPrice = new double[2500];
            double[] sumDayFirst = new double[2100];
            double[] sumDaySecond = new double[2100];
            double[] slowPerK = new double[2100];
            double[] slowPerD = new double[2100];
            //1 - 200,200,200
            //3 - 200,200,200
            //5 - 200,200,200
            //10 - 150,150,150
            //15 - 100,100,100
            //30 - 50,50,50
            //45 - 30,30,30
            //60 - 25,25,25
            // (C - lowest(L, Period1)) / (highest(H, Period1) - lowest(L, Period1)) * 100
            //Stochasticsslow K: Sum(C - lowest(L, Period1), Period2) / Sum((highest(H, Period1) - lowest(L, Period1)), Period2) * 100
            //Stochasticsslow D: eavg(Stochasticsslow(sto1, sto2), sto3) 지수이동평균

            //int _period = 0;// _buyingStocPeriod1;
            //int _k = 0;//m_buyingStocPeriod2;
            //int _d = 0;//m_buyingStocPeriod3;
            int _sto1 = 0;
            int _sto2 = 0;
            int _sto3 = 0;
            int bongMinuteType = 0; // 몇분봉인지 찾는다.

            if (tradeType == 0) // 매수
            {
                _sto1 = (int)m_buyingStocPeriod1;
                _sto2 = (int)m_buyingStocPeriod2;
                _sto3 = (int)m_buyingStocPeriod3;
                bongMinuteType = m_buyingMinuteType;// 몇분봉인지 찾는다.            
            }
            else if (tradeType == 2) // 익절
            {
                _sto1 = (int)m_takeProfitStocPeriod1;
                _sto2 = (int)m_takeProfitStocPeriod2;
                _sto3 = (int)m_takeProfitStocPeriod3;
                bongMinuteType = m_takeProfitMinuteType;// 몇분봉인지 찾는다.

            }
            else if (tradeType == 3) // 손절
            {
                _sto1 = (int)m_stopLossStocPeriod1;
                _sto2 = (int)m_stopLossStocPeriod2;
                _sto3 = (int)m_stopLossStocPeriod3;
                bongMinuteType = m_stopLossMinuteType;// 몇분봉인지 찾는다. 
            }

            int k_offset = _sto2;
            int d_offset = _sto3 * 6;

            if (_sto1 > calBongCount[bongMinuteType])
                _sto1 = calBongCount[bongMinuteType];

            //_peroid일치 최저가 계산
            for (int i = 0; i < k_offset + d_offset; i++) // 24 경우 _k값의 크기
            {
                double minPrice = 999999999;
                for (int j = 0; j < _sto1; j++) // 기간
                {
                    if (bb_lowPrice[bongMinuteType, j + i] == 0)
                        continue;
                    if (bb_lowPrice[bongMinuteType, j + i] < minPrice)
                    {
                        minPrice = bb_lowPrice[bongMinuteType, j + i];
                    }
                }
                LowMinPrice[i] = minPrice;
            }

            //_period일치 최고가 계산
            for (int i = 0; i < k_offset + d_offset; i++)
            {
                double maxPrice = 0;
                for (int j = 0; j < _sto1; j++) // 기간
                {
                    if (bb_highPrice[bongMinuteType, j + i] == 0)
                        continue;
                    if (bb_highPrice[bongMinuteType, j + i] > maxPrice)
                    {
                        maxPrice = bb_highPrice[bongMinuteType, j + i];
                    }
                }
                HighMaxPrice[i] = maxPrice;
            }

            //종가 -  최저가
            for (int i = 0; i < k_offset + d_offset; i++)
            {
                EndMinusMinPrice[i] = bb_endPrice[bongMinuteType, i] - LowMinPrice[i];
            }
            // 최고가 - 최저가
            for (int i = 0; i < k_offset + d_offset; i++)
            {
                MaxMinusMinPrice[i] = HighMaxPrice[i] - LowMinPrice[i];
            }

            // 합계
            for (int i = 0; i < d_offset; i++)
            {
                double daySum1 = 0;
                double daySum2 = 0;
                for (int j = 0; j < _sto2; j++)
                {
                    daySum1 += EndMinusMinPrice[j + i];
                    daySum2 += MaxMinusMinPrice[j + i];
                }
                sumDayFirst[i] = daySum1;
                sumDaySecond[i] = daySum2;
            }
            //slow % K 값
            for (int i = 0; i < d_offset; i++)
            {
                if (sumDaySecond[i] == 0)
                    slowPerK[i] = 0;
                else
                    slowPerK[i] = (sumDayFirst[i] / sumDaySecond[i]) * 100;
            }
            //slow % D 이동평균            
            double offset1 = ((double)2 / (double)(1 + _sto3));
            double offset2 = (double)(1 - offset1);
            for (int i = d_offset - 1; i >= 0; i--)
            {
                if (i == d_offset - 1)
                {
                    slowPerD[i] = slowPerK[i];
                }
                else
                {
                    slowPerD[i] = (slowPerK[i] * offset1) + (slowPerD[i + 1] * offset2);
                }
            }

            stochastics_KPriceCur[tradeType] = slowPerK[0];
            stochastics_KPricePrev[tradeType] = slowPerK[1];
            stochastics_DPriceCur[tradeType] = slowPerD[0];
            stochastics_DPricePrev[tradeType] = slowPerD[1];
        }
        // 추매 일봉 이동평균선 계산
        public void getRebuyingDayBongMovingAveragePrice(int tradeType)
        {
            int type = m_reBuyingMovePriceKindType;
            double[] evg_value = new double[600]; // 5이동 평균 가격을 계산한다.

            for (int k = 0; k < m_buyingCount - 1; k++)
            {
                // %n이평값이 봉갯수보다 크면 봉갯수로 계산을 한다.
                int _calBongCount = m_reBuyingMinuteLineType[k];
                if (_calBongCount <= 0)
                    continue;

                if (type == 0) // 단순이평
                {
                    if (_calBongCount > m_totalDayBongCount)
                        _calBongCount = m_totalDayBongCount;

                    double totPrice1 = 0, totPrice2 = 0;
                    for (int i = 0; i < _calBongCount; i++)
                    {
                        totPrice1 += db_endPrice[i];
                        totPrice2 += db_endPrice[i + 1];
                    }

                    reBuyingMoving_PriceDayCur[k] = totPrice1 / _calBongCount; // 현재이평
                    reBuyingMoving_PriceDayPrev[k] = totPrice2 / _calBongCount; // 이전이평                
                }
                else if (type == 1) // 지수 이평
                {
                    double _value = (double)2 / (double)(_calBongCount + 1);
                    double _minus = (double)1 - _value;

                    _calBongCount *= 6;
                    if (_calBongCount > m_totalDayBongCount)
                        _calBongCount = m_totalDayBongCount;

                    for (int i = _calBongCount - 1; i >= 0; i--)
                    {
                        if (i == _calBongCount - 1)
                        {
                            evg_value[i] = db_endPrice[_calBongCount - 1];
                        }
                        else
                        {
                            evg_value[i] = db_endPrice[i] * _value + evg_value[i + 1] * _minus; // 5이평                        
                        }
                    }
                    reBuyingMoving_PriceDayCur[k] = evg_value[0]; // 현재이평
                    reBuyingMoving_PriceDayPrev[k] = evg_value[1]; // 이전이평           
                }
            }
        }
        // 추매 분봉 이동평균선 계산
        public void getRebuyingBunBongMovingAveragePrice(int tradeType) // 단수지수, 분봉타입(1,3,5등등), 이평종류(1,3,5
        {
            // 모두 150이상 가능하나, 45 분봉은 90, 60분봉은 70개
            // type으로 0은 단순, 1은 지수
            // 일봉상 - 600개로 550이평가능
            //3분봉 - 1500
            //5분봉 - 900
            //10분봉 - 450
            //15분봉 - 300
            //30분봉 - 150
            //45분봉 - 100
            //60분봉 - 75
            int type = m_reBuyingMovePriceKindType; // 단순, 지수
            int arrayNumber = m_reBuyingMinuteType;// getBunBongType(); // 몇분봉인지 찾는다.
            double[] evg_value = new double[5000]; // 5이동 평균 가격을 계산한다.
            // int[] calMovingNumber = { 3, 5, 10, 15 };
            //double[] moving_PriceCur = new double[7];
            //double[] moving_PricePrev = new double[7];
            // m_OneArrayCount
            // calBongCount[] 분봉갯수
            //m_reBuyingMinuteLineType[]

            for (int k = 0; k < m_buyingCount - 1; k++)
            {
                // %n이평값이 봉갯수보다 크면 봉갯수로 계산을 한다.
                int _calBongCount = m_reBuyingMinuteLineType[k];
                if (_calBongCount <= 0)
                    continue;
                if (m_buyingPerInvestment[k + 1] == 0)
                    continue;

                if (type == 0) // 단순이평
                {
                    if (_calBongCount > calBongCount[arrayNumber])
                        _calBongCount = calBongCount[arrayNumber];

                    double totPrice1 = 0, totPrice2 = 0;
                    for (int i = 0; i < _calBongCount; i++)
                    {
                        totPrice1 += bb_endPrice[arrayNumber, i];
                        totPrice2 += bb_endPrice[arrayNumber, i + 1];
                    }
                    reBuyingMoving_PriceCur[k] = totPrice1 / _calBongCount; // 현재이평
                    reBuyingMoving_PricePrev[k] = totPrice2 / _calBongCount; // 이전이평                               
                }
                else if (type == 1) // 지수 이평은 이전 3배수 정도 계산을 해야한다.
                {
                    double _value = (double)2 / (double)(_calBongCount + 1);
                    double _minus = (double)1 - _value;

                    _calBongCount *= 6;
                    if (_calBongCount > calBongCount[arrayNumber])
                        _calBongCount = calBongCount[arrayNumber];

                    for (int i = _calBongCount - 1; i >= 0; i--)
                    {
                        if (i == _calBongCount - 1)
                        {
                            evg_value[i] = bb_endPrice[arrayNumber, _calBongCount - 1];
                        }
                        else
                        {
                            evg_value[i] = bb_endPrice[arrayNumber, i] * _value + evg_value[i + 1] * _minus; // 5이평                        
                        }
                    }

                    reBuyingMoving_PriceCur[k] = evg_value[0]; // 현재이평
                    reBuyingMoving_PricePrev[k] = evg_value[1]; // 이전이평                       
                }
            }
        }
        // 매수, 추매, 익절, 손절에서 필요한 것(이동평균선, 스토캐스틱, 볼린저밴드, 엔벨로프)을 이용할시 계산에 대한부분임.
        public void CalculateIndicator()
        {
            // 0: 매수, 1: 추매, 2: 익절, 3: 손절
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int _typeNum = calINdicatorNumber[i, j];
                    //calINdicatorNumber[0, 0] = m_buyingType (public int m_buyingType = 0; // 0:기본매수, 1:이동평균,2:스토케스틱,3:볼린저밴드,4:엔벨로프)
                    //calINdicatorNumber[1, 0] = m_reBuyingType (public int m_reBuyingType = 0; //0:기본추매, 1:이동평균 근접)
                    //calINdicatorNumber[2, 0] = m_takeProfitType (public int m_takeProfitType = 0; // 0:기본익절,1:이동평균,2:스토케스틱,3:볼린저밴드,4:엔벨로프)
                    //calINdicatorNumber[3, 0] = m_stopLossType (public int m_stopLossType = 0; // 0:기본손절,1:이동평균 근접,2:스토케스틱,3:볼린저밴드,4:엔벨로프)
                    if (_typeNum == 0 || _typeNum == -1)  //0은 기본(분봉이나 일봉등으로 계산할 필요가없는 데이터) , -1은 해당기능을 사용하지않을경우라고 보임.
                        continue;

                    int _bongNum = calBongTypeNumber[i];
                    //calBongTypeNumber[0] = m_buyingType
                    //calBongTypeNumber[1] = m_reBuyingType
                    //calBongTypeNumber[2] = m_takeProfitType
                    //calBongTypeNumber[3] = m_stopLossType

                    if (i == 3) // 손절(손절을 스토캐스틱이 없다)
                    {
                        if (_bongNum == 0) // 일봉
                        {
                            if (m_bGetDayBongSuccess) // 일봉 데이타 로딩 후에 계산
                            {
                                if (_typeNum == 1) getDayBongMovingAveragePrice(i); // 이동평균선
                                else if (_typeNum == 2) getDayBongBollingerPrice(i); // 볼린져밴드
                                else if (_typeNum == 3) getDayBongEnvelopePrice(i); // 엔벨로프
                            }
                        }
                        else
                        {
                            if (_typeNum == 1) getBunBongMovingAveragePrice(i);
                            else if (_typeNum == 2) getBunBongBollingerPrice(i);
                            else if (_typeNum == 3) getBunBongEnvelopePrice(i);
                        }
                    }
                    else if (i == 1) // 추매는 함수가 다르다.
                    {
                        if (_bongNum == 0) // 일봉
                        {
                            if (m_bGetDayBongSuccess)
                                getRebuyingDayBongMovingAveragePrice(i);
                        }
                        else
                            getRebuyingBunBongMovingAveragePrice(i);
                    }
                    else // 매수, 익절
                    {
                        if (_bongNum == 0) // 일봉
                        {
                            if (m_bGetDayBongSuccess) // 일봉 데이타 로딩 후에 계산
                            {
                                if (_typeNum == 1) getDayBongMovingAveragePrice(i); // 이동평균선
                                else if (_typeNum == 2) getDayBongStochasticsSlowPrice(i); //스토캐스틱slow
                                else if (_typeNum == 3) getDayBongBollingerPrice(i); // 볼린져밴드
                                else if (_typeNum == 4) getDayBongEnvelopePrice(i); // 엔벨로프
                            }
                        }
                        else
                        {
                            if (_typeNum == 1) getBunBongMovingAveragePrice(i);
                            else if (_typeNum == 2) getBunBongStochasticsSlowPrice(i);
                            else if (_typeNum == 3) getBunBongBollingerPrice(i);
                            else if (_typeNum == 4) getBunBongEnvelopePrice(i);
                        }
                    }
                }
            }
        }
        // 해당 메서드를 사용하여 calBongNumber배열값을 각각{m_buyingMinuteType,m_reBuyingMinuteType,m_takeProfitMinuteType,m_stopLossMinuteType]값으로 채워나가는 과정 
        private void setBunBongType(int bunbong) //int bunbong 0:1분봉 , 1:3분봉, 2: 5분봉
        {
            bool _check = false;
            int _emptyArray = -1;
            for (int i = 0; i < 4; i++)
            {
                // public int[] calBongNumber = {-1, -1, -1, -1}; 이 배열값은
                // 0,1,2,3,4,5,6,7(1,3,5,10,15,30,45,60) -> 계산할 분봉의 배열변수(index번호를 뜻하는거같음) -> 1분봉은 0번째 배열, 3분봉은 1번째 배열
                if (calBongNumber[i] == bunbong)
                {
                    _check = true;
                    break;
                }
                if (calBongNumber[i] == -1)
                {
                    if (_emptyArray == -1)
                    {
                        _emptyArray = i;
                        break;
                    }
                }
            }
            if (!_check)
            {
                calBongNumber[_emptyArray] = bunbong;
            }
        }

        //위에서 계산된 1분봉을 기준으로 각 분봉(3,5,10분봉)의 시가,고가,저가,종가,거래량을 계산
        public void CalculateAnotherBunBongWithOneBunBong()
        {
            for (int k = 0; k<4; k++)
            {
                int _num = calBongNumber[k]; // 1분봉에서 60분봉까지 총 8개의봉중 매수,추매,익절,손절에 필요한 봉들만 계산
                //calBongNumber[0] = 0 / calBongNumber[1] = 1 / calBongNumber[2] = 2 / calBongNumber[3] =3 
                //public int[] calBongNumber = { 0, 1, 2, 3, 4, 5, 6, 7 }; // 0,1,2,3,4,5,6,7(1,3,5,10,15,30,45,60) -> 계산할 분봉의 배열변수(index번호를 뜻하는거같음) -> 1분봉은 0번째 배열, 3분봉은 1번째 배열
                if (_num == -1)
                    continue;

                int _offSet = 0; // 분봉 데이터를 저장할 위치를 나타내는 변수
                bool _bsCheck = false;
                int _selArray = 0; //현재 처리 중인 분봉의 시간 간격 인덱스
                double[] _bb_endPrice = Enumerable.Repeat<double>(0, 5000).ToArray<double>();
                double[] _bb_highPrice = Enumerable.Repeat<double>(0, 5000).ToArray<double>();
                double[] _bb_lowPrice = Enumerable.Repeat<double>(0, 5000).ToArray<double>();
                double[] _bb_startPrice= Enumerable.Repeat<double>(0, 5000).ToArray<double>();
                string[] _strDayhm = Enumerable.Repeat<string>(string.Empty, 5000).ToArray<string>();
                int[] _bb_tradingVolume = Enumerable.Repeat<int>(0, 5000).ToArray<int>();
                _bb_lowPrice[_offSet] = 100000000;

                // 서버에서 받아온 1분봉 총 갯수 만큼 계산(가장 먼 데이터부터 처리)
                //202408271000 -> 이게 멀티데이터 수신시 최신데이터(i=0)라면 202408010923 -> 이게 마지막데이터(i=4499)이거부터 처리하네

                // 가져온 분봉의 총 갯수 -> TradingManger.cs 의 onreceiveTR에서 갯수를 누적시킨다.

                //m_totalBunBongCount-1 = 4499라고 생각해도될듯

                for (int i = m_totalBunBongCount-1; i>=0; i--)
                {
                    //Console.WriteLine("i: " + i);
                    int _curTime = int.Parse(m_strHM[i]); //해당 1분봉시간(4500개의 일봉시간을 전부 가져오는것)
                    string _curDay = m_strDay[i]; // 해당 분봉날짜(이게 필요한 이유는 10시시작한날을 체크하기위해)
                    //Console.WriteLine("1. i: " + i + " k: " + k + " _curTime: " + _curTime + " _curDay: " + _curDay);

                    //intervalArrayCount[_num] -> 1,3,5,10분봉 분봉의 갯수인 382, 129, 78, 39

                    //intervalArrayCount[0] = 382 , intervalArrayCount[1] = 129 , intervalArrayCount[2] = 78 , intervalArrayCount[3] = 39
                    for (int j = 0; j<intervalArrayCount[_num]; j++)
                    {
                        int _firstTime = 0;
                        int _secondTime = 0;
                        bool _sameCheck = false;

                        if(_num == 6)// 45분봉인 경우 시작 시간 예외 처리
                        {
                            for(int ii =0; ii<gMainForm._startTime10.Length; ii++)
                            {
                                if(gMainForm._startTime10[ii].Equals(_curDay))
                                {
                                    _sameCheck = true;
                                    break;
                                }
                            }
                        }
                        if(!_sameCheck)
                        {
                            _firstTime = bb_interval[_num, j, 0];
                            //"k: " + k + " _num: " + _num + 
                            _secondTime = bb_interval[_num, j, 1];
                            //"k: " + k + " _num: " + _num +
                            //Console.WriteLine("j: " + j + " _firstTime: " + _firstTime + " _secondTime: " + _secondTime);
                        }
                        else
                        {
                            _firstTime = bb_exceptionInterval[j, 0];
                            _secondTime = bb_exceptionInterval[j, 1];
                        }
                        // 1분봉 시간이 계산해야될 시간에 포함이 되면
                        if(_firstTime <= _curTime && _curTime < _secondTime)
                        {
                           //Console.WriteLine("2. _firstTime: " + _firstTime + " _curTime: " + _curTime + " _secondTime: " + _secondTime + " " + _bsCheck + " j: " + j);
                            if (!_bsCheck) // 최초 계산해야될 시간대 배열번호 저장 및 시작가 저장(1분봉기준 4500번쨰전 월 일 시간 분 저장 + 시작가)
                            {
                                _bsCheck = true;
                                _selArray = j;
                                _bb_startPrice[_offSet] = m_startPrice[i];
                                //저장할 날짜와 시간저장
                                int b_interval;
                                if (_sameCheck)
                                    b_interval = bb_exceptionInterval[_selArray, 0];
                                else
                                    b_interval = bb_interval[_num, _selArray, 0];
                                string _dayTime;
                                if (b_interval.ToString().Length == 3)
                                    _dayTime = m_strDay[i] + "0" + b_interval.ToString();
                                else
                                    _dayTime = m_strDay[i] + b_interval.ToString();
                                _strDayhm[_offSet] = _dayTime;
                                //Console.WriteLine("3. !_bsCheck.." + " _offSet: " + _offSet + " _num: " + _num + " _selArray: " + _selArray + " _dayTime: " + _dayTime + " _bb_startPrice[_offSet]: " + _bb_startPrice[_offSet]);
                            }
                            else
                            {
                                //_selArray 값과 j가 다르면 저장 _offset++후 리셋(새로운시간대로 변경됨을 알림)
                                if(_selArray != j)
                                {
                                    _offSet++;
                                    _selArray = j;
                                    _bb_startPrice[_offSet] = m_startPrice[i]; // 새로운시간대라면 시가를 새롭게 저장
                                    _bb_lowPrice[_offSet] = 100000000;
                                    int b_interval;
                                    if (_sameCheck)
                                        b_interval = bb_exceptionInterval[_selArray, 0];
                                    else
                                        b_interval = bb_interval[_num, _selArray, 0];
                                    string _dayTime;
                                    if (b_interval.ToString().Length == 3)
                                        _dayTime = m_strDay[i] + "0" + b_interval.ToString();
                                    else
                                        _dayTime = m_strDay[i] + b_interval.ToString();
                                    _strDayhm[_offSet] = _dayTime;

                                    //Console.WriteLine("4. else.." + " _offSet: " + _offSet + " _num: " + _num + " _selArray: " + _selArray + " _dayTime: " + _dayTime + " _bb_startPrice[_offSet]: " + _bb_startPrice[_offSet]);
                                }
                            }
                            _bb_endPrice[_offSet] = m_endPrice[i]; //종가저장
                            if (_bb_highPrice[_offSet] < m_highPrice[i]) // 고가 계산
                                _bb_highPrice[_offSet] = m_highPrice[i];
                            if (_bb_lowPrice[_offSet] > m_lowPrice[i])
                                _bb_lowPrice[_offSet] = m_lowPrice[i];
                            //거래량 계산
                            _bb_tradingVolume[_offSet] += m_tradingVolume[i];
                            //Console.WriteLine("5. _bb_endPrice[_offSet]: " + _bb_endPrice[_offSet] + " _bb_highPrice[_offSet]: " + _bb_highPrice[_offSet] + " _bb_lowPrice[_offSet]: " + _bb_lowPrice[_offSet] + " _bb_tradingVolume[_offSet]: " + _bb_tradingVolume[_offSet]);
                        }
                    }
                }
                //Console.WriteLine("6. total offset: " + _offSet);
                //계산된 배열값을 다시 멤버변수에 넣어준다.(얘는 최신값부터 저장한다.)
                for(int i =0; i<_offSet + 1; i++)
                {
                    bb_endPrice[_num, i] = _bb_endPrice[_offSet - i]; //3번값이 나오고 4번이 나오기 전 5번의 i값의 endprice가 종가로 저장되는듯 // 마지막 두가지봉은 제외같은데?)
                    bb_highPrice[_num, i] = _bb_highPrice[_offSet - i];
                    bb_lowPrice[_num, i] = _bb_lowPrice[_offSet - i];
                    bb_startPrice[_num, i] = _bb_startPrice[_offSet - i];
                    bb_strDayhm[_num, i] = _strDayhm[_offSet - i];
                    bb_tradingVolume[_num, i] = _bb_tradingVolume[_offSet - i];
                    //Console.WriteLine("7. _num: " + _num + " i: " + i +  " _offSet: " + _offSet +  " bb_startPrice[_num, i]: " + bb_startPrice[_num, i]);
                    //+ " bb_endPrice[_num, i]: " + bb_endPrice[_num, i]
                }
                // 총 계산된 분봉 갯수 저장
                calBongCount[_num] = _offSet + 1;
                //Console.Write("8. calBongCount[_num]: " + calBongCount[_num]);
            }
            /*
            Console.WriteLine(" bb_tradingVolume[1분봉첫]: " + bb_tradingVolume[0, 0]);
            Console.WriteLine(" bb_tradingVolume[3분봉첫]: " + bb_tradingVolume[1, 0]);
            Console.WriteLine(" bb_tradingVolume[5분봉첫]: " + bb_tradingVolume[2, 0]);
            Console.WriteLine(" bb_tradingVolume[10분봉첫]: " + bb_tradingVolume[3, 0]);
            */
        }
        // 1분봉을 얻어오는 시간(onreceivetrdata)에 실시간 거래량을 저장(onreceiverealdata)하고 보정해준다.
        public void setBunBongModifyData()
        {
            //Console.WriteLine("m_tradeVolume[0]: " + m_tradeVolume[0] + " m_tradeVolume[1]: " + m_tradeVolume[1]);
            if (m_tradeVolume[0] == 0 && m_tradeVolume[1] == 0)
                return;

            for(int k =0; k<4; k++)
            {
                int num = calBongNumber[k];
                if (num == -1)
                    continue;

                for(int i=0; i<2; i++)
                {
                    //현재와 이전분봉까지만 계산해 준다.
                    string _curTime = bb_strDayhm[num, i].Substring(4, 4);

                    if(m_tradeVolume[0] != 0)
                    {
                        if(checkSameTime(num,_curTime, m_tradingTime[0])) // 현재시간과 같다면 거래량을 추가한다(추가되는거래량은 실시간거래량).
                        {
                            bb_tradingVolume[num, i] += m_tradeVolume[0];
                            if (bb_highPrice[num, i] < m_tradeHighPrice[0])
                                bb_highPrice[num, i] = m_tradeHighPrice[0];
                            if (bb_lowPrice[num, i] > m_tradeLowPrice[0])
                                bb_lowPrice[num, i] = m_tradeLowPrice[0];
                            Console.WriteLine("m_tradingTime[0] bb_tradingVolume[num, i]: " + bb_tradingVolume[num, i]);
                        }
                    }
                    if (m_tradeVolume[1] != 0)
                    {
                        if (checkSameTime(num, _curTime, m_tradingTime[1]))
                        {
                            bb_tradingVolume[num, i] += m_tradeVolume[1];
                            if (bb_highPrice[num, i] < m_tradeHighPrice[1])
                                bb_highPrice[num, i] = m_tradeHighPrice[1];
                            if (bb_lowPrice[num, i] > m_tradeLowPrice[1])
                                bb_lowPrice[num, i] = m_tradeLowPrice[1];
                            Console.WriteLine("m_tradingTime[1] bb_tradingVolume[num, i]: " + bb_tradingVolume[num, i]);
                        }
                    }
                    Console.WriteLine("_curTime: " + _curTime);
                    Console.WriteLine("i: " + i);
                    Console.WriteLine("num: " + num);
                }
            }
        }
        // 분봉 실시간 계산
        public void setBunBongRealCalculate(double curPrice, string time, int volume)
        {
            for(int k=3; k<4; k++)
            {
                int num = calBongNumber[k];
                if (num == -1)
                    continue;

                // 현재 체크하고 있는 시간(첫봉시간?)
                string _curTime = bb_strDayhm[num, 0].Substring(4, 4);
                if(checkSameTime(num, _curTime, time))
                {
                    // 현재 실시간가격과 고가,저가,거래량계산
                    bb_endPrice[num, 0] = curPrice;
                    if (bb_highPrice[num, 0] < curPrice)
                        bb_highPrice[num, 0] = curPrice;
                    if (bb_lowPrice[num, 0] > curPrice)
                        bb_lowPrice[num, 0] = curPrice;
                    bb_tradingVolume[num, 0] += volume;
                    Console.WriteLine("같은시간curprice: " + curPrice);
                    Console.WriteLine("bb_highPrice[num, 0]: " + bb_highPrice[num, 0]);
                    Console.WriteLine("bb_lowPrice[num, 0]: " + bb_lowPrice[num, 0]);
                    Console.WriteLine("bb_tradingVolume[num, 0]: " + bb_tradingVolume[num, 0]);
                }
                else
                {
                    int _offset = calBongCount[num];
                    if (_offset > 1500) // 1500까지만 계산
                        _offset = 1500;
                    Console.WriteLine("_offset: " + _offset);
                    //배열을 뒤로 하나씩 이동
                    for (int i = _offset; i >= 1; i--)
                    {
                        bb_endPrice[num, i] = bb_endPrice[num, i - 1];
                        bb_highPrice[num, i] = bb_highPrice[num, i - 1];
                        bb_lowPrice[num, i] = bb_lowPrice[num, i - 1];
                        bb_startPrice[num, i] = bb_startPrice[num, i - 1];
                        bb_strDayhm[num, i] = bb_strDayhm[num, i - 1];
                        bb_tradingVolume[num, i] = bb_tradingVolume[num, i - 1];
                    }
                    // 첫번째 배열에 현재 가격, 날짜, 거래량 셋팅
                    bb_endPrice[num, 0] = curPrice;
                    bb_highPrice[num, 0] = curPrice;
                    bb_lowPrice[num, 0] = curPrice;
                    bb_startPrice[num, 0] = curPrice;
                    bb_tradingVolume[num, 0] = 0;
                    Console.WriteLine("bb_tradingVolume[num, 0]: " + bb_tradingVolume[num, 0]);
                    bb_strDayhm[num, 0] = gMainForm.g_curDay + time; // gMainForm.g_curDay 는 오늘의 월,일(0824 -> 8월24일)
                    bb_tradingVolume[num, 0] += volume;
                    Console.WriteLine("bb_tradingVolume[num, 0]+=volume: " + bb_tradingVolume[num, 0]);
                    Console.WriteLine("curPrice: " + curPrice);
                    Console.WriteLine("첫번째 배열에 변경된시간; " + bb_strDayhm[num, 0].Substring(4, 4));
                }
            }
        }
        public bool checkSameTime(int num, string time1, string time2)
        {
            //num은 0~7까지(1,3,5,10,15,30,45,60분봉을 의미)
            int _firstTime = 0;
            int _secondTime = 0;
            int _time1 = int.Parse(time1);
            int _time2 = int.Parse(time2);
            int _time1Num = -1, _time2Num = -1;

            for (int j = 0; j < intervalArrayCount[num]; j++)
            {
                _firstTime = bb_interval[num, j, 0];
                _secondTime = bb_interval[num, j, 1];
                if (_firstTime <= _time1 && _secondTime > _time1)
                {
                    _time1Num = j;
                    break;
                }
            }
            for (int j = 0; j < intervalArrayCount[num]; j++)
            {
                _firstTime = bb_interval[num, j, 0];
                _secondTime = bb_interval[num, j, 1];
                if (_firstTime <= _time2 && _secondTime > _time2)
                {
                    _time2Num = j;
                    break;
                }
            }
            if (_time1Num == _time2Num)
                return true;

            return false;
        }
        // 일봉 실시간 계산
        public void setDayBongRealCalculate(double curPrice, int volume)
        {
            if (!m_bGetDayBongSuccess)
                return;

            db_endPrice[0] = curPrice; // 현재가 == 종가
            if (db_highPrice[0] < curPrice) // 고가
                db_highPrice[0] = curPrice;
            if (db_lowPrice[0] > curPrice) // 저가
                db_lowPrice[0] = curPrice;
            db_tradingVolume[0] += volume; // 거래량
        }

        //m_rePurchaseArray 배열의 모든 요소를 문자열로 변환하여 ;로 구분하여 반환
        public string getRePurchaseArray()
        {
            string _ret = string.Empty;
            for (int i = 0; i < 6; i++)
                _ret += m_rePurchaseArray[i].ToString() + ';';
            return _ret;
        }
        // m_rePurchaseArray 배열 내 true 값의 개수를 계산하여 반환
        public int getRePurchaseCount()
        {
            int _count = 0;
            for (int i = 1; i < 6; i++)
            {
                if (m_rePurchaseArray[i])
                    _count++;
            }
            return _count;
        }
        //입력된 문자열을 ;로 분리하여, 각 요소를 m_rePurchaseArray의 값으로 설정
        public void setRepurchaseArray(string _str)
        {
            string[] _temp = _str.Split(';');
            for(int i =0; i<6;i++)
            {
                if (_temp[i].Equals("False") || _temp[i].Equals("FALSE") || _temp[i].Equals("false"))
                    m_rePurchaseArray[i] = false;
                else
                    m_rePurchaseArray[i] = true;
            }
        }
    }
}
