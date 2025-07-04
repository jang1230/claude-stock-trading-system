namespace StockAutoTrade2
{
    public enum ConditionNumber
    {
        NormalStartNum = 1000, //화면 시작 번호
        ConditionStartNum = 1191, //실시간 종목 등록 시작 번호
        TotalConditionCount = 5, //총 조건식 사용갯수
    }
    public enum ConditionState
    {
        Stop = 0, // 조건식 실행 중지
        Playing, // 조건식 실행
    }
    public enum HoldingItemState
    {
        Stop = 0, Playing,
        // 전체보유매도 실행 및 중지
    }
    public enum Save
    {
        Condition = 8,
        Item = 6,
        trading = 7,
        lastCondition = 8,
        holdingItem = 6,
    }
    public enum bongCount
    {
        bunbong = 900,
        daybong = 600,
    }
    public enum trading
    {
        wait = 0,
        doing,
        stop,
    }
    public class ProfitLossCalculate
    {
        public string m_buyOrderNumber; //매수주문번호
        public int m_rePurchaseNumber; // 추가매수 회차 번호
        public string m_sellOrderNumber; // 매도주문번호(향후 분할매도에 사용)
        public double m_totalEvaluationProfitLoss; // 매도시 각각의 평가손익 합산
        public double m_realAveragePrice; // 매도시점의 평균단가(총매입금/보유수량)

        public ProfitLossCalculate(string m_buyOrderNumber, int m_rePurchaseNumber, string m_sellOrderNumber, double m_realAveragePrice)
        {
            this.m_buyOrderNumber = m_buyOrderNumber;
            this.m_rePurchaseNumber = m_rePurchaseNumber;
            this.m_sellOrderNumber = m_sellOrderNumber;
            this.m_realAveragePrice = m_realAveragePrice;
            this.m_totalEvaluationProfitLoss = 0;
        }
    }
    //ts매도 고점용
    public class GoJumMedo
    {
        public string itemCode;
        public double purchasePrice;
        public double[] highPrice = new double[3];
        public string orderNumber;

        public GoJumMedo(string itemCode, double purchasePrice, string orderNumber)
        {
            this.itemCode = itemCode;
            this.purchasePrice = purchasePrice;
            this.orderNumber = orderNumber;

            for (int i = 0; i < 3; i++) // 배열 초기화
            {
                this.highPrice[i] = 0; // 초기값을 0으로 설정
            }
        }
    }
    // 수동매수용
    public class PassitiveBuyingItem
    {
        public string division = "영웅문매수";
        public string itemCode; // 종목코드
        public string orderNumber; //주문번호(사실상 무쓸모같고)
        public bool buyingcheck = false; // 매수 주문완료 체크(데이터그리딩뷰 바인딩용)
        public int totalPurchasePrice; //총매입금
        public int balanceQnt; // 총매수량
        public double currentPrice; // 현재가 
        public double rateOfReturn; // 수익률
        public double buyingAvgPrice; // 매입가(평균단가)
        public bool bsold = false; // 매도 완료체크 여부

        public PassitiveBuyingItem(string itemCode, string orderNumber)
        {
            this.itemCode = itemCode;
            this.orderNumber = orderNumber;
        }
    }
}
