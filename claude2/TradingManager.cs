using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockAutoTrade2
{
    public class TradingManager
    {
        public MainForm gMainForm = MainForm.GetInstance();
        private static TradingManager gTradingManager;
        private Timer _orderCheckTimer;
        public TradingManager()
        {
            gTradingManager = this;
            //TR 수신 이벤트 등록
            gMainForm.KiwoomAPI.OnReceiveTrData += OnReceiveTrData;
            // 서버에서 보내주는 메시지 이벤트 등록
            gMainForm.KiwoomAPI.OnReceiveMsg += onReceiveServerMessage;
            // 실시간 시세 데이터 이벤트 등록
            gMainForm.KiwoomAPI.OnReceiveRealData += OnReceiveRealData;
            // 종목접수, 체결, 잔고 관련 이벤트 등록
            gMainForm.KiwoomAPI.OnReceiveChejanData += OnReceiveChejanData;
            // 타이머 초기화 및 설정
            //InitializeOrderCheckTimer();

            // WinForms Timer 생성
            _orderCheckTimer = new Timer();
            // 타이머 간격 설정 (예: 1000ms = 1초)
            // 실제 사용 시 환경에 따라 조정 가능
            _orderCheckTimer.Interval = 300;
            // 타이머 Tick 이벤트에 메서드 연결
            // Tick 이벤트는 Interval마다 한 번씩 호출된다.
            _orderCheckTimer.Tick += OrderCheckTimer_Tick;
            // 타이머 중지(최초에는 중지상태)
            _orderCheckTimer.Stop();
        }
        //인스턴스생성 메서드
        public static TradingManager GetInstance()
        {
            if (gTradingManager == null)
                gTradingManager = new TradingManager();

            return gTradingManager;
        }
        // conditionDataList의 인덱스번호를 넣을시 conditionName을 받아오는 method
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
        

        /// <summary>
        /// 타이머의 Tick 이벤트 핸들러
        /// 여기서 일정 주기마다 CheckUnconcludedOrders() 메서드를 호출한다.
        /// </summary>
        /// 
        
        private void OrderCheckTimer_Tick(object sender, EventArgs e)
        {
            
            /*
            // 일정 주기(1초마다)로 미체결 종목 체크 메서드 실행
            CheckUnconcludedCancelOrders();
            //CheckUnconcludedMarketOrders();
            CheckUnconcludedDesignationOrders();
            */

            Task task1 = new Task(() => CheckUnconcludedCancelOrders());
            Task task2 = new Task(() => CheckUnconcludedDesignationOrders());

            gMainForm.gCheckRequestManager.sendTaskData(task1);
            gMainForm.gCheckRequestManager.sendTaskData(task2);

        }
        

        // TR 수신 이벤트 매서드
        public void OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            if(e.sRQName.Contains("예수금상세현황")) // opw00001
            {
                long orderAmount = long.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "예수금"));
                long deposit_D1 = long.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "d+1추정예수금"));
                long deposit_D2 = long.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "d+2추정예수금"));

                if(deposit_D2 ==0)
                {
                    if(deposit_D1==0)
                    {
                        gMainForm.ConditionDig.depositAmountTextBoxlabel.Text = orderAmount.ToString("N0");
                        gMainForm.curOrderAmount = orderAmount;
                    }
                    else
                    {
                        gMainForm.ConditionDig.depositAmountTextBoxlabel.Text = deposit_D1.ToString("N0");
                        gMainForm.curOrderAmount = deposit_D1;
                    }
                }
                else
                {
                    gMainForm.ConditionDig.depositAmountTextBoxlabel.Text = deposit_D2.ToString("N0");
                    gMainForm.curOrderAmount = deposit_D2;
                }
                gMainForm.setLogText("예수금상세현황 수신 성공");
                gMainForm.setLogText("현재 예수금 : " + gMainForm.curOrderAmount.ToString("N0"));
                gMainForm.curOrderAmountLabel.Text = gMainForm.curOrderAmount.ToString("N0");
            }
            else if(e.sRQName.Contains("주식정보")) // opt10001
            {
                string _itemCode = gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "종목코드").Trim(); // 종목코드
                string _itemName = gMainForm.KiwoomAPI.GetMasterCodeName(_itemCode); // 종목명

                //Console.WriteLine(_itemName + " opt100001수신 완료");
                // 조건식이름을 구분해 낸다.
                string[] _rqName = e.sRQName.Split(';');
                string _conditionName = _rqName[1]; // 조건식이름

                //Console.WriteLine("편입종목: " + _itemName);

                // 데이터를 수신해온다
                int startPrice = Math.Abs(int.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "시가").Trim())); // 시가
                int highPrice = Math.Abs(int.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "고가").Trim())); // 고가
                int lowPrice = Math.Abs(int.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "저가").Trim())); // 저가
                int closingPrice = Math.Abs(int.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "현재가").Trim())); // 현재가 == 종가
                int itemVolume = Math.Abs(int.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "거래량").Trim())); // 거래량
                int upperLimitPrice = Math.Abs(int.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "상한가").Trim())); // 상한가
                int lowerLimitPrice = Math.Abs(int.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "하한가").Trim())); // 하한가
                string updownRate = gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "등락율").Trim(); //등락율

                // 조건식리스트에서 조건식 이름이 같은 것을 찾아낸다.
                MyTradingCondition _mtc = gMainForm.gMyTradingConditionList.Find(o => o.conditionData.conditionName.Equals(_conditionName));
                if(_mtc != null)
                {
                    // 서버체크
                    bool bCheckServer = true;
                    // 모의 투자 경우 1000원 이하 종목은 매매를 할 수 없음.
                    if(gMainForm.sServerGubun.Equals("1"))
                    {
                        if (closingPrice < 1000)
                        {
                            gMainForm.setLogText("1000원이하 종목 매수금지 : " + _itemName);
                            bCheckServer = false;
                        }
                    }
                    
                    if(bCheckServer)
                    {
                        bool isDuplicate = false;
                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                        {
                            // 빈 행이거나 아직 값이 없으면 넘어감
                            if (row.IsNewRow) continue;
                            if (row.Cells["매매진행_종목코드"].Value == null) continue;

                            // 이미 동일한 종목코드가 있다면 중복으로 간주
                            if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode))
                            {
                                isDuplicate = true;
                                break;
                            }
                        }

                        // 2) 중복이면 추가하지 않고 바로 리턴
                        if (isDuplicate)
                        {
                            Console.WriteLine($"[중복] 이미 등록된 종목코드: {_itemCode}");
                            return;
                        }

                        // MyTradingItem 객체 생성
                        MyTradingItem _mti = new MyTradingItem(_mtc, _conditionName, _mtc.conditionData.conditionIndex.ToString(), _itemCode, closingPrice, closingPrice, upperLimitPrice);
                        // MyTradingItemList에 추가
                        _mtc.MyTradingItemList.Add(_mti);
                        //Console.WriteLine("MyTradingItemList에 추가 후 개수: " + _mtc.MyTradingItemList.Count);
                        //Console.WriteLine("opt10001 주식코드: " + _itemCode + " 종목명: " + _itemName + " _mti.m_buyingTransferType: " + _mti.m_buyingTransferType + " _mti.m_rePurchaseArray[0]: " + _mti.m_rePurchaseArray[0]);
                        if (_mtc.remainingTransferItemCount > 0)
                        {
                            // tradingItemDataGridView에 추가하기
                            gMainForm.AddMyTradingItemToDataGridView(_mti, _mtc.itemInvestment);
                            _mtc.remainingTransferItemCount--;
                            // 실시간 시세등록
                            Task _task = new Task(() =>
                            {
                                string fidList = "10;11;12;13;15;20;228;302;9001";
                                gMainForm.KiwoomAPI.SetRealReg(_mtc.screenNumber, _itemCode, fidList, "1");
                            });
                            gMainForm.gCheckRequestManager.sendTaskData(_task);
                            //매수하기(매수사용여부 체크, 기본매수, 편입시 바로 매수)
                            if (_mtc.buyingUsing == 1 && _mtc.buyingType == 0)
                            {
                                if (_mti.m_buyingTransferType == 0 && !_mti.m_rePurchaseArray[0] && _mtc.remainingBuyingItemCount > 0)
                                {
                                    if(_mtc.mesuoption1 == 0) //시장가
                                    {
                                        gMainForm.setLogText(_mti.m_conditionName + " " + _itemName + " 시장가 매수 주문 시작");
                                        // 더이상 매수가 안되도록 _mti.m_bPurchased를 true로 설정한다.
                                        _mti.m_rePurchaseArray[0] = true;
                                        _mti.m_bPurchased = true;

                                        // 주문수량계산 : 시장가의 경우 상한가를 기준으로 수량을 계산한다.
                                        int _qnt = (int)(_mti.m_buyingPerInvestment[0] / upperLimitPrice); // 종목당투자금 / 상한가 = 주문수량
                                        //Task _marketBuyTask = new Task(() =>
                                        //{
                                            long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                        "편입종목시장가매수", // 사용자 구분명
                                                                        gMainForm.GetScreenNumber(), // 화면번호
                                                                        _mtc.account, // 계좌번호 10자리
                                                                        1, // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                        _itemCode, // 종목코드 6자리
                                                                        _qnt, // 주문수량
                                                                        0, // 주문가격: 시장가의 경우 0
                                                                        "03", // 거래구분 / 03은 시장가
                                                                        "" // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                                            );
                                        gMainForm.setLogText(_itemName + " 매수 주문 보냄." + _qnt + "_" + "03_"+ _mti.m_currentPrice);
                                        if (_ret == 0) //성공
                                        {

                                            //Console.WriteLine(_itemName + " 일반 시장가 매수주문성공");

                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                            {
                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode) &&
                                                    row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                {
                                                    row.Cells["매매진행_진행상황"].Value = "매수중";
                                                    //매수주문성공시 개수카운트차감
                                                    _mtc.remainingBuyingItemCount--;
                                                    //Console.WriteLine("주문성공 남은매수갯수: " + _mtc.remainingBuyingItemCount);
                                                    break;
                                                }
                                            }
                                            gMainForm.setLogText(_itemName + "시장가 매수주문 성공.");
                                            //Console.WriteLine(_itemName + " 편입가종목 시장가 매수 성공");
                                        }
                                        else // 실패
                                        {
                                            gMainForm.setLogText("편입종목 매수실패: " + _itemName);
                                        }
                                        //});
                                        //gMainForm.gOrderRequestManager.sendTaskData(_marketBuyTask);
                                    }
                                    if(_mtc.mesuoption1 ==1) // 지정가(현재가)
                                    {
                                        // 더이상 매수가 안되도록 _mti.m_bPurchased를 true로 설정한다.
                                        _mti.m_rePurchaseArray[0] = true;
                                        _mti.m_bPurchased = true;

                                        // 주문수량계산 : 시장가의 경우 상한가를 기준으로 수량을 계산한다.
                                        int ModifyPrice = gMainForm.RoundToTick(closingPrice); // 호가단위오류방지를 위한 계산
                                        int _qnt = (int)(_mti.m_buyingPerInvestment[0] / ModifyPrice); // 종목당투자금 / 상한가 = 주문수량
                                        Console.WriteLine("종목명: " + _itemName + " 이벤트에서 수신한 현재가: " + closingPrice + " 호가방지오류를 위해 수정한 현재가: " + ModifyPrice + " 주문수량: " + _qnt);
                                        //Task _limitBuyTask = new Task(() =>
                                        //{
                                            long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                "편입종목지정가매수", // 사용자 구분명
                                                                                gMainForm.GetScreenNumber(), // 화면번호
                                                                                _mtc.account, // 계좌번호 10자리
                                                                                1, // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                _itemCode, // 종목코드 6자리
                                                                                _qnt, // 주문수량
                                                                                ModifyPrice, // 주문가격: 시장가의 경우 0
                                                                                "00", // 거래구분 / 03은 시장가
                                                                                "" // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                                                   );
                                        if (_ret == 0) //성공
                                        {

                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                            {
                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode) &&
                                                    row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                {
                                                    row.Cells["매매진행_진행상황"].Value = "매수중";
                                                    //매수주문성공시 개수카운트차감
                                                    _mtc.remainingBuyingItemCount--;
                                                    //Console.WriteLine("주문성공 남은매수갯수: " + _mtc.remainingBuyingItemCount);
                                                    break;
                                                }
                                            }
                                            gMainForm.setLogText("편입종목 현재가 매수성공: " + _itemName);
                                            //Console.WriteLine(_itemName + " 편입가종목 현재가 매수 성공" + " 편입가격: " + ModifyPrice);
                                        }
                                        else // 실패
                                        {
                                            gMainForm.setLogText("편입종목 매수실패: " + _itemName);
                                        }
                                        //});
                                        //gMainForm.gOrderRequestManager.sendTaskData(_limitBuyTask);
                                    }

                                }
                                else if (_mti.m_buyingTransferType == 1 || _mti.m_buyingTransferType == 2)
                                {
                                    if(!_mti.m_rePurchaseArray[0])
                                    {
                                        gMainForm.getBunBongData(_itemCode, _conditionName, 0, gMainForm.GetScreenNumber());
                                        //Console.WriteLine(_itemName +  " 편입후상승하락체크 분봉데이터요청" + " _mti.m_volumeState: " + _mti.m_volumeState);
                                    }
                                }
                            }
                            else
                            {
                                gMainForm.getBunBongData(_itemCode, _conditionName, 0, gMainForm.GetScreenNumber());
                                //Console.WriteLine("기본매수이외 분봉데이터요청");
                            }
                        }
                    }
                }
            }
            else if(e.sRQName.Contains("계좌평가현황")) // OPW00004
            {
                // 조건식이름을 구분해 낸다.
                string[] _rqName = e.sRQName.Split(';');
                string _division= _rqName[1]; // 조건식이름
                //Console.WriteLine("_division: " + _division);
                string codeList = "";

                int _listCount = gMainForm.KiwoomAPI.GetRepeatCnt(e.sTrCode, e.sRQName); // 받아온 리스트갯수(멀티데이터 수신을 위해 받아오는것)
                for (int i = 0; i < _listCount; i++)
                {
                    string _itemCode = gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "종목코드").Trim();
                    string _itemName = gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "종목명").Trim();
                    int _qnt = Math.Abs(int.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "보유수량")));
                    double _avgPrice = Math.Abs(double.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "평균단가")));
                    long _endPrice = Math.Abs(long.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "현재가")));
                    long _evaluationPrice = Math.Abs(long.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "평가금액")));
                    //long _profitlossPrice  = Math.Abs(long.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "손익금액")));
                    double _updownRate = Math.Abs(double.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "손익율"))) / 10000.0;
                    int _purchaseAmount = Math.Abs(int.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "매입금액")));

                    long _profitlossPrice = _evaluationPrice - _purchaseAmount;

                    codeList += _itemCode;
                    if (i != _listCount - 1)
                        codeList += ";";
                    //Console.WriteLine("OPW00004 종목명: " + _itemName + " 손익율: " + _updownRate);

                    _itemCode = _itemCode.Replace("A", "");

                    // 보유종목과 검색식 종목을 구분하기위한 변수
                    bool isSearchCondition = false;
                    string _conditionName = string.Empty;
                    int _transferPrice = 0;
                    string _rePurcahseArray = string.Empty;
                    int _currentRebuyingStep = 0;

                    string[] _ItemListData = gMainForm.gFileIOInstance.getItemListData();
                    int _itemListCount = _ItemListData.Length / (int)Save.Item;

                    string[] _holdingItemOptionData = gMainForm.gFileIOInstance.getHoldingItemListData();
                    int _holdingItemOptionCount = _holdingItemOptionData.Length / (int)Save.holdingItem;


                    for (int j = 0; j < _itemListCount; j++)
                    {
                        string _tItemCode = _ItemListData[j * (int)Save.Item]; // 아이템코드

                        if (_tItemCode.Equals(_itemCode)) // 같은 종목이 있으면...
                        {
                            // 검색식메모장파일에있는 종목일경우 변수를 true로 변경.
                            isSearchCondition = true;
                            _conditionName = _ItemListData[j * (int)Save.Item + 1]; // 조건식이름
                            _transferPrice = int.Parse(_ItemListData[j * (int)Save.Item + 3]); // 편입가격
                            _rePurcahseArray = _ItemListData[j * (int)Save.Item + 4]; // 추매배열
                            _currentRebuyingStep = int.Parse(_ItemListData[j * (int)Save.Item + 5]); // 추매차수
                        }
                    }
                    if (isSearchCondition)
                    {
                        // 먼저 조건식을 찾아서 등록을 해 준다.
                        string[] _conditionListData = gMainForm.gFileIOInstance.getConditionListData();
                        int _conditionListCount = _conditionListData.Length / (int)Save.Condition; // 4

                        for (int k = 0; k < _conditionListCount; k++)
                        {
                            string _tConditionIndex = _conditionListData[k * (int)Save.Condition]; // 조건식이름
                            string _tConditionName = getConditionName(int.Parse(_tConditionIndex)); // 조건식인덱스
                            if (_tConditionName.Equals(_conditionName)) // 조건식저장파일에서 같은 조건식이 있으면...
                            {
                                // 조건식 데이타그리드뷰에 없을때만 등록을 한다.
                                ConditionData _conditionData = gMainForm.conditionDataList.Find(o => o.conditionName.Equals(_tConditionName));
                                MyTradingCondition _mtc = gMainForm.gMyTradingConditionList.Find(o => o.conditionData.conditionName.Equals(_tConditionName));
                                if (_mtc == null)
                                {
                                    string _conditionIndex2 = string.Empty;
                                    string _conditionName2 = string.Empty;
                                    string _strInvestment = string.Empty, _strInvestmentPrice = string.Empty, _strBuying = string.Empty, _strReBuying = string.Empty, _strTakeProfit = string.Empty, _strStopLoss = string.Empty, _strTsmedo = string.Empty;

                                    // 조건식인덱스
                                    _conditionIndex2 = _conditionListData[(k * (int)Save.Condition)];
                                    _conditionName2 = getConditionName(Int32.Parse(_conditionIndex2));
                                    _strInvestment = _conditionListData[(k * (int)Save.Condition) + 1];
                                    _strInvestmentPrice = _conditionListData[(k * (int)Save.Condition) + 2];
                                    _strBuying = _conditionListData[(k * (int)Save.Condition) + 3];
                                    _strReBuying = _conditionListData[(k * (int)Save.Condition) + 4];
                                    _strTakeProfit = _conditionListData[(k * (int)Save.Condition) + 5];
                                    _strStopLoss = _conditionListData[(k * (int)Save.Condition) + 6];
                                    _strTsmedo = _conditionListData[(k * (int)Save.Condition) + 7];

                                    string _screenNumber = gMainForm.GetConditionScreenNumber(); // 스크린번호를 받아온다.
                                                                                                    // bConditionSNUseOrNot 변수를 셋팅한다.
                                    gMainForm.setConditionListCreateCheck(int.Parse(_screenNumber) - (int)ConditionNumber.ConditionStartNum, true);
                                    MyTradingCondition _tmtc = new MyTradingCondition(_conditionIndex2, _strInvestment, _strInvestmentPrice, _strBuying, _strReBuying, _strTakeProfit, _strStopLoss, _screenNumber, _strTsmedo);

                                    // 생성된 MyTradingCondition을 gMyTradingConditionList에 추가한다.
                                    gMainForm.gMyTradingConditionList.Add(_tmtc);
                                    // 데이타그리드뷰에 출력하기
                                    gMainForm.AddmyTradingConditionToDataGridView(_tmtc);
                                }
                                // 종목을 생성시켜준다.
                                MyTradingCondition _mtc2 = gMainForm.gMyTradingConditionList.Find(o => o.conditionData.conditionName.Equals(_tConditionName));
                                MyTradingItem _mti = new MyTradingItem(_mtc2, _conditionName, _mtc2.conditionData.conditionIndex.ToString(), _itemCode, _transferPrice, _endPrice, 0);
                                _mtc2.MyTradingItemList.Add(_mti);
                                GoJumMedo goJumMedo = new GoJumMedo(_itemCode, _avgPrice, "");
                                gMainForm.gojumMedoList.Add(goJumMedo);
                                // 추매배열셋팅
                                _mti.setRepurchaseArray(_rePurcahseArray);
                                // tradingItemDataGridView에 추가하기
                                gMainForm.AddMyTradingItemToDataGridView(_mti, _mtc2.itemInvestment);
                                //Console.WriteLine("opw00004 에서 AddMyTradingItemToDataGridView 실행");
                                // 같은 종목 체크
                                gMainForm.transferSameCheckList.Add(_itemCode);

                                _mtc2.remainingBuyingItemCount--;
                                _mtc2.remainingTransferItemCount--;
                                if (_mtc2.remainingBuyingItemCount < 0)
                                    _mtc2.remainingBuyingItemCount = 0;

                                if (_mtc2.remainingTransferItemCount < 0)
                                    _mtc2.remainingTransferItemCount = 0;
                                // 데이타 입력
                                _mti.m_bCompletePurchase = true;
                                _mti.m_averagePrice = _avgPrice; // 평균단가
                                _mti.m_totalQnt = _qnt; // 매입량                                    
                                _mti.m_orderAvailableQnt = _mti.m_totalQnt; // 주문가능수량
                                _mti.m_totalPurchaseAmount = _purchaseAmount; // 총매입가
                                _mti.m_currentRebuyingStep = _currentRebuyingStep; // 추매차수
                                //Console.WriteLine("종목명: " + _mti.m_itemName + " 현재 추가매수 진행된차수: " + _mti.m_currentRebuyingStep);

                                foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                {
                                    if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode) && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc2.conditionData.conditionName))
                                    {
                                        row.Cells["매매진행_매입금"].Value = _mti.m_totalPurchaseAmount.ToString("N0");
                                        row.Cells["매매진행_보유수량"].Value = _mti.m_totalQnt.ToString("N0");
                                        row.Cells["매매진행_매입가"].Value = _mti.m_averagePrice.ToString("N0");
                                        row.Cells["매매진행_주문가능수량"].Value = _mti.m_orderAvailableQnt.ToString("N0");
                                        row.Cells["매매진행_진행상황"].Value = "매수완료";
                                        row.Cells["매매진행_추매"].Value = _mti.getRePurchaseCount().ToString();
                                        break;
                                    }
                                }

                                /*
                                // 실시간시세등록
                                Task _task2 = new Task(() =>
                                {
                                    string fidList = "10;11;12;13;15;20;228;302;9001";
                                    gMainForm.KiwoomAPI.SetRealReg(_mtc2.screenNumber, _itemCode, fidList, "1");
                                    gMainForm.setLogText("종목 실시간 감시 요청 : " + _itemName);
                                });
                                gMainForm.gRequestManager.sendTaskData(_task2);
                                */
                                // 1분봉데이타 요청하기
                                // 1분봉데이타 요청하기
                                //gMainForm.getBunBongData(_itemCode, _conditionName, 0, gMainForm.GetScreenNumber());
                                //Console.WriteLine("opw00004 에서 getBunBongData 실행");

                                //sendRequestLoadingItemInfoTR(_itemCode, _mtc2.conditionData.conditionName);
                                //gMainForm.setLogText("종목로딩 성공 : " + _itemName);
                            }
                        }
                    }
                    else
                    {
                        for (int a = 0; a < _holdingItemOptionCount; a++)
                        {
                            string _strInvestment = _holdingItemOptionData[a * (int)Save.holdingItem];
                            string _strReBuying = _holdingItemOptionData[a * (int)Save.holdingItem + 1];
                            string _strInvestmentPrice = _holdingItemOptionData[a * (int)Save.holdingItem + 2];
                            string _strTakeProfit = _holdingItemOptionData[a * (int)Save.holdingItem + 3];
                            string _strStopLoss = _holdingItemOptionData[a * (int)Save.holdingItem + 4];
                            string _strTsmedo = _holdingItemOptionData[a * (int)Save.holdingItem + 5];

                            MyHoldingItemOption mho = new MyHoldingItemOption(_strInvestment, _strReBuying, _strInvestmentPrice, _strTakeProfit, _strStopLoss, _strTsmedo, gMainForm.GetScreenNumber());
                            gMainForm.gMyHoldingItemOptionList.Add(mho);
                        }

                        MyHoldingItemOption _mho = gMainForm.gMyHoldingItemOptionList.Find(o => o.division.Equals(_division));
                        if (_mho != null)
                        {
                            MyHoldingItem _mhi = _mho.MyHoldingItemList.Find(o => o.m_itemName.Equals(_itemName));
                            if (_mhi == null)
                            {
                                MyHoldingItem mhi = new MyHoldingItem(_mho, _itemCode, _avgPrice, _endPrice, _purchaseAmount, _qnt);
                                _mho.MyHoldingItemList.Add(mhi);
                                string itemName = gMainForm.KiwoomAPI.GetMasterCodeName(_itemCode);
                                if(itemName != null)
                                Console.WriteLine("OPW0004수신중 보유종목으로 저장된 종목명: " + itemName);
                                else if(itemName == null)
                                Console.WriteLine("OPW0004수신중 보유종목으로 저장된 종목없음.");

                                GoJumMedo goJumMedo = new GoJumMedo(_itemCode, _avgPrice, "");
                                gMainForm.gojumMedoList.Add(goJumMedo);
                            }
                        }
                        /*
                        Console.WriteLine("잔고" + " 종목코드: " + _itemCode + " 종목명: " + _itemName + " 보유수량: " + _qnt + " 평균단가: " + _avgPrice + " 현재가: " + _endPrice +
                        " 평가금액: " + _evaluationPrice + "  손익금액: " + _profitlossPrice + " 손익율: " + _updownRate + " 매입금액: " + _purchaseAmount);*/

                        // rowIndex  = 몇 번째 줄인지 리턴
                        int rowIndex = gMainForm.tradingItemDataGridView.Rows.Add(); // 그리드뷰에 한 줄이 추가됨

                        gMainForm.tradingItemDataGridView["매매진행_종목명", rowIndex].Value = _itemName; // 종목명
                        gMainForm.tradingItemDataGridView["매매진행_종목코드", rowIndex].Value = _itemCode; // 종목코드
                        gMainForm.tradingItemDataGridView["매매진행_조건식", rowIndex].Value = "보유"; // 조건식
                        gMainForm.tradingItemDataGridView["매매진행_구분", rowIndex].Value = "보유"; // 조건식
                        gMainForm.tradingItemDataGridView["매매진행_현재가", rowIndex].Value = _endPrice.ToString("N0"); // 현재가
                        gMainForm.tradingItemDataGridView["매매진행_매입금", rowIndex].Value = _purchaseAmount.ToString("N0"); // 매입금
                        gMainForm.tradingItemDataGridView["매매진행_보유수량", rowIndex].Value = _qnt; // 보유수량            
                        gMainForm.tradingItemDataGridView["매매진행_매입가", rowIndex].Value = _avgPrice.ToString("N0"); // 매입가
                        gMainForm.tradingItemDataGridView["매매진행_평가손익", rowIndex].Value = gMainForm.setUpDownArrow(_profitlossPrice);
                        if (_profitlossPrice < 0)
                            gMainForm.tradingItemDataGridView["매매진행_수익률", rowIndex].Value = "-" + gMainForm.setUpDownRateOfReturn(_updownRate);//"0.00%"; // 수익률
                        else
                            gMainForm.tradingItemDataGridView["매매진행_수익률", rowIndex].Value = gMainForm.setUpDownRateOfReturn(_updownRate);//"0.00%"; // 수익률
                        gMainForm.tradingItemDataGridView["매매진행_진행상황", rowIndex].Value = "보유중"; // 진행상황
                        gMainForm.tradingItemDataGridView["매매진행_등락율", rowIndex].Value = "0.00%"; // 등락율

                        // 열의 높이지정
                        gMainForm.tradingItemDataGridView.Rows[rowIndex].Height = 26;

                        /*
                        Task _task = new Task(() =>
                        {
                            string fidList = "10;11;12;13;15;20;228;302;9001";
                            gMainForm.KiwoomAPI.SetRealReg(gMainForm.GetScreenNumber(), _itemCode, fidList, "1");
                            gMainForm.setLogText("종목 실시간 감시 요청 : " + _itemName);
                        });
                        gMainForm.gRequestManager.sendTaskData(_task);
                        */


                    }
                }
                gMainForm.KiwoomAPI.CommKwRqData(codeList, 0, _listCount, 0, "관심종목정보요청;보유", gMainForm.GetScreenNumber());
            }
            else if(e.sRQName.Contains("관심종목정보"))
            {
                // 조건식이름을 구분해 낸다.
                string[] _rqName = e.sRQName.Split(';');
                string _division = _rqName[1]; // 조건식이름
                //Console.WriteLine("관심종목정보요청 수신 완료");

                int _listCount = gMainForm.KiwoomAPI.GetRepeatCnt(e.sTrCode, e.sRQName); // 받아온 리스트갯수(멀티데이터 수신을 위해 받아오는것)
                for (int i = 0; i < _listCount; i++)
                {
                    string _itemCode = gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "종목코드").Trim().Replace("A", "");
                    string _itemName = gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "종목명").Trim();
                    long _currentPrice = Math.Abs(long.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "현재가")));
                    long _standardPrice = long.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "기준가"));
                    long _upperLimitPrice = long.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "상한가"));
                    double _upDownRate = double.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "등락율"));

                    MyHoldingItemOption _mho = gMainForm.gMyHoldingItemOptionList.Find(o => o.division.Equals(_division));
                    if (_mho != null)
                    {
                        MyHoldingItem _mhi = _mho.MyHoldingItemList.Find(o => o.m_itemName.Equals(_itemName));
                        if (_mhi != null)
                        {
                            _mhi.m_upperLimitPrice = _upperLimitPrice;
                            //Console.WriteLine("종목명: " + _itemName + " 상한가: " + _mhi.m_upperLimitPrice);
                            HoldingItemRealRateOfReturnAndEvaluationProfitLoss(_mhi, _currentPrice, _upDownRate);
                            TotalRateOfReturnAndEvaluationProfitLoss();
                            //Console.WriteLine("종목명: " + _mhi.m_itemName + " 수익률: " + _mhi.m_rateOfReturn);
                        }
                    }

                    string[] _ItemListData = gMainForm.gFileIOInstance.getItemListData();
                    int _itemListCount = _ItemListData.Length / (int)Save.Item;
                    //Console.WriteLine("관심종목정보요청 _itemListCount: " + _itemListCount);
                    for (int j = 0; j < _itemListCount; j++)
                    {
                        string _tItemCode = _ItemListData[j * (int)Save.Item]; // 아이템코드
                        string _conditionName = _ItemListData[j * (int)Save.Item + 1]; // 조건식이름
                        string _transferPrice = _ItemListData[j * (int)Save.Item + 3]; // 편입가격
                        string _rePurcahseArray = _ItemListData[j * (int)Save.Item + 4]; // 추매배열
                        string _tItemName = gMainForm.KiwoomAPI.GetMasterCodeName(_tItemCode);
                        //Console.WriteLine("종목명: " + _tItemName + " 종목코드: " + _tItemCode + " 조건식이름: " + _conditionName + " 편입가격: " + _transferPrice);

                        if (_tItemCode.Equals(_itemCode)) // 같은 종목이 있으면...
                        {
                            MyTradingCondition _mtc = gMainForm.gMyTradingConditionList.Find(o => o.conditionData.conditionName.Equals(_conditionName));
                            if (_mtc != null)
                            {
                                MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(_itemCode));
                                if(_mti != null)
                                {
                                    //Console.WriteLine("관심종목정보요청: " + _mti.m_itemName + " 상한가: " + _upperLimitPrice);
                                    _mti.m_upperLimitPrice = _upperLimitPrice;
                                    //Console.WriteLine("종목명: " + _mti.m_itemName + " _mti.m_upperLimitPrice: " + _mti.m_upperLimitPrice);
                                    // 수익률 계산
                                    RealRateOfReturnAndEvaluationProfitLoss(_mti, _currentPrice, _upDownRate);
                                    // 총수익, 총매수, 총손익등등 계산
                                    TotalRateOfReturnAndEvaluationProfitLoss();

                                    //Console.WriteLine("종목명: " + _mti.m_itemName + " 수익률: " + _mti.m_rateOfReturn);
                                }

                            }

                        }
                    }

                    Task _task = new Task(() =>
                    {
                        string fidList = "10;11;12;13;15;20;228;302;9001";
                        gMainForm.KiwoomAPI.SetRealReg(gMainForm.GetScreenNumber(), _itemCode, fidList, "1");
                        gMainForm.setLogText("종목 실시간 감시 요청 : " + _itemName);
                    });
                    gMainForm.gCheckRequestManager.sendTaskData(_task);
                }
            }
            else if(e.sRQName.Contains("1분봉5")) // 1분봉 5번 받아오기 -> MainForm에서 getBunBongData 호출시 api로 데이터 송신
            {
                string _itemCode = gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "종목코드").Trim();
                string _itemName = gMainForm.KiwoomAPI.GetMasterCodeName(_itemCode);
                int _listCount = gMainForm.KiwoomAPI.GetRepeatCnt(e.sTrCode, e.sRQName); // 받아온 리스트갯수(멀티데이터 수신을 위해 받아오는것)
                string[] _rqName = e.sRQName.Split(';'); // 기본적으로 rqName이 1분봉5;conditionName으로 수신해오기때문에, 이걸 분리하기위한 작업
                string _conditionName = _rqName[1]; //조건식 이름
                //Console.WriteLine("_listcount: " + _listCount);

                //조건식리스트에서 조건식 이름이 같은 것을 찾아낸다
                MyTradingCondition _mtc = gMainForm.gMyTradingConditionList.Find(o => o.conditionData.conditionName.Equals(_conditionName));
                if(_mtc != null)
                {
                    //매매중인 종목을찾음
                    MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(_itemCode));
                    if(_mti != null)
                    {
                        //Console.WriteLine("상태확인: " + _mti.m_volumeState);
                        if (_mti.m_volumeState == (int)trading.wait)
                            _mti.m_volumeState = (int)trading.doing; // 거래량보정을위해 m_volumeState을 doing으로 변환

                        _mti.m_totalBunBongCount += _listCount; // 가져온 분봉의 총 갯수누적
                        //Console.WriteLine("종목명: " + _itemName + " _mti.m_totalBunBongCount " + _mti.m_totalBunBongCount);
                        //1분봉 멀티데이터 저장
                        for (int i= 0; i<_listCount; i++)
                        {
                            long _startPrice = Math.Abs(long.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "시가")));
                            long _endPrice = Math.Abs(long.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "현재가")));
                            long _highPrice = Math.Abs(long.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "고가")));
                            long _lowPrice = Math.Abs(long.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "저가")));
                            int _tradingVolume = Math.Abs(int.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "거래량")));
                            string _tradingTime = gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "체결시간").Trim(); // 년 월 일 시간 분 초(20240821145400)으로 데이터 전달
                            string _curDate = _tradingTime.Substring(0, 8); // 년 월 일(20240821)만 데이터 빼내기(시작점인덱스 + 총길이(인덱스포함)

                            //Console.WriteLine("i : " + i + " _mti.m_totalBunBongCount : " + _mti.m_totalBunBongCount + " _curDate: " + _curDate + " _mti.m_getBunBongDataCount: " + _mti.m_getBunBongDataCount);

                            //종목변수에 넣어준다 ([현재 분봉데이터 받은갯수 * 900 + i)]
                            _mti.m_startPrice[(_mti.m_getBunBongDataCount * (int)bongCount.bunbong + i)] = _startPrice;
                            _mti.m_endPrice[(_mti.m_getBunBongDataCount * (int)bongCount.bunbong + i)] = _endPrice;
                            _mti.m_highPrice[(_mti.m_getBunBongDataCount * (int)bongCount.bunbong + i)] = _highPrice;
                            _mti.m_lowPrice[(_mti.m_getBunBongDataCount * (int)bongCount.bunbong + i)] = _lowPrice;
                            _mti.m_tradingVolume[(_mti.m_getBunBongDataCount * (int)bongCount.bunbong + i)] = _tradingVolume;
                            _mti.m_strDay[(_mti.m_getBunBongDataCount * (int)bongCount.bunbong + i)] = _tradingTime.Substring(4, 4); // 날짜 저장
                            _mti.m_strHM[(_mti.m_getBunBongDataCount * (int)bongCount.bunbong + i)] = _tradingTime.Substring(8, 4); // 날짜 저장

                            //Console.WriteLine("i(0): " + _mti.m_strDay[0] + _mti.m_strHM[0]);
                            //Console.WriteLine("i(1): " + _mti.m_strDay[1] + _mti.m_strHM[1]);
                            //Console.WriteLine("i(4499): " + _mti.m_strDay[4499] + _mti.m_strHM[4499]);
                            //Console.WriteLine("_mti.m_strDay : " + _mti.m_strDay[(_mti.m_getBunBongDataCount * (int)bongCount.bunbong + i)] + " : " + (_mti.m_getBunBongDataCount * (int)bongCount.bunbong + i) + " _mti.m_getBunBongDataCount: " + _mti.m_getBunBongDataCount +  " i : " + i + "e.sPrevNext: "  +  e.sPrevNext);
                            
                            // 수신 데이터를 기반으로 현재가, 편입가, 상한가 계산
                            if (!_mti.m_bCheckIpperLimit)
                            {
                                //상한가 계산하기
                                string strToday = DateTime.Today.ToString("yyyyMMdd");
                                if (!strToday.Equals(_curDate)) // 현재 날짜와 분봉날짜가 다르면 전날(예를들어 오늘이 20240821이면 그 이전 날짜들 분봉데이터)
                                {
                                    _mti.m_prevEndPrice = _endPrice; // 전날 종가
                                    _mti.m_upperLimitPrice = gMainForm.getUpperLimitPrice((int)_mti.m_prevEndPrice);
                                    _mti.m_transferPrice = _mti.m_endPrice[0]; // 편입가
                                    _mti.m_currentPrice = _mti.m_endPrice[0]; // 현재가
                                    _mti.m_bCheckIpperLimit = true;

                                    // 현재가, 편입가, 상한가 처리가 완료되었으면 그리드뷰에 데이터를 추가해준다
                                    foreach(DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                    {
                                        if(row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode) && row.Cells["매매진행_조건식"].Value.ToString().Equals(_conditionName))
                                        {
                                            row.Cells["매매진행_편입가격"].Value = _mti.m_transferPrice.ToString("N0");
                                            row.Cells["매매진행_현재가"].Value = _mti.m_currentPrice.ToString("N0");
                                            break;
                                        }
                                    }

                                }
                            }
                        }
                        // 이전날 분봉이 없다면 신규상장주임
                        // 신규상장주의 경우 최초종가를 넣어준다
                        if(!_mti.m_bCheckIpperLimit)
                        {
                            _mti.m_prevEndPrice = _mti.m_endPrice[_listCount - 1]; // 오늘 1분봉 최초종가 // [_listCount -1]은 해당 리스트의 마지막 인덱스 값을 의미함.
                            _mti.m_upperLimitPrice = gMainForm.getUpperLimitPrice((int)_mti.m_prevEndPrice);
                            _mti.m_transferPrice = _mti.m_endPrice[0]; // 편입가
                            _mti.m_currentPrice = _mti.m_endPrice[0]; // 현재가
                            _mti.m_bCheckIpperLimit = true;
                            //Console.WriteLine(_itemName + " 오늘최초종가: " + _mti.m_endPrice[_listCount - 1] + " 상한가: " + _mti.m_upperLimitPrice);
                            // 현재가, 편입가, 상한가 처리가 완료되었으면 그리드뷰에 데이터를 추가해준다
                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                            {
                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode) && row.Cells["매매진행_조건식"].Value.ToString().Equals(_conditionName))
                                {
                                    row.Cells["매매진행_편입가격"].Value = _mti.m_transferPrice.ToString("N0");
                                    row.Cells["매매진행_현재가"].Value = _mti.m_currentPrice.ToString("N0");
                                    break;
                                }
                            }
                        }
                        if(e.sPrevNext.Equals("2")) //다음에 받을 데이터가 존재한다면
                        {
                            _mti.m_getBunBongDataCount++; // 분봉데이터를 가져온 횟수를 추가
                            if(_mti.m_getBunBongDataCount ==5) // 5회를 다 받아왔다면 -> 5회인이유는 분봉데이터는 한번에 900개를 받아올수있고 4500개가있기때문에, 5회 카운트를 채우면 전부다 수신이 완료
                            {
                                //Console.WriteLine("데이터를 5번다 받아옴. sPrevNext = 2 ");
                                // call_GlobalTimer메서드에서 g_InsertItemList에 추가적인 종목있으면 종목그리드뷰 추가 및 분봉요청
                                gMainForm.g_bGetBunBong = false; // 1분봉데이터를 전부다 요청완료해서 false값으로 다시 변환
                                _mti.m_volumeState = (int)trading.stop;// 1분봉데이터를 전부다 요청완료해서 거래량 보정 중지
                                _mti.m_bGetBunBongSuccess = true; // 1분봉데이터를 전부다 수신완료했으므로 true로 변환
                                _mti.CalculateAnotherBunBongWithOneBunBong();
                                //Console.WriteLine("_mti.CalculateAnotherBunBongWithOneBunBong(); 실행완료");
                                _mti.setBunBongModifyData();
                                if (gMainForm.g_selectItemCode == String.Empty)
                                {
                                    gMainForm.g_selectItemCode = _itemCode;
                                }
                                DrawIndicateData(gMainForm.g_selectItemCode);
                                //Console.WriteLine("_mti.setBunBongModifyData(); 실행완료");
                                //gMainForm.setLogText(_itemName + " : 1분봉데이터 받은 횟수 : " + _mti.m_getBunBongDataCount.ToString());
                                //gMainForm.setLogText(_itemName + " : 1분봉데이터 받기 완료");
                                // 분봉데이터를 전부 다 수신한 이후에 일봉데이터를 요청한다.
                                gMainForm.setLogText(_itemName + " : 1분봉데이터 받기 완료");
                                gMainForm.getDayBongData(_itemCode, _conditionName, 0, gMainForm.GetScreenNumber());
                                //gMainForm.setLogText(_itemName + " : 일봉데이터 요청");
                            }
                            else // 5회를 수신하지 못했다면 데이터 추가요청
                            {
                                gMainForm.getBunBongData(_itemCode, _conditionName, 2, gMainForm.GetScreenNumber());
                                //gMainForm.setLogText(_itemName + " : 1분봉데이터 받은 횟수 : " + _mti.m_getBunBongDataCount.ToString());
                            }
                        }
                        else // 더 이상 받을 데이터가 없다면
                        {
                            //Console.WriteLine("데이터를 전부다 받아옴. 4500개 이하야 ");
                            // call_GlobalTimer메서드에서 g_InsertItemList에 종목
                            // 있으면 종목그리드뷰 추가 및 분봉요청
                            gMainForm.g_bGetBunBong = false; // 1분봉데이터를 전부다 요청완료해서 false값으로 다시 변환
                            _mti.m_volumeState = (int)trading.stop; // 1분봉데이터를 전부다 요청완료해서 실시간거래량 보정 중지(onreceiverealdata에서 거래량, 고가, 저가에대한 값을 그만 받아와)
                            _mti.m_bGetBunBongSuccess = true;// 1분봉데이터를 전부다 수신완료했으므로 true로 변환
                            _mti.CalculateAnotherBunBongWithOneBunBong();
                            //Console.WriteLine("_mti.CalculateAnotherBunBongWithOneBunBong(); 실행완료");
                            _mti.setBunBongModifyData();
                            if (gMainForm.g_selectItemCode == String.Empty)
                            {
                                gMainForm.g_selectItemCode = _itemCode;
                            }
                            DrawIndicateData(gMainForm.g_selectItemCode);
                            //Console.WriteLine("_mti.setBunBongModifyData(); 실행완료");
                            //gMainForm.setLogText(_itemName + " : 1분봉데이터 받은 횟수 : " + _mti.m_getBunBongDataCount.ToString());
                            gMainForm.setLogText(_itemName + " : 1분봉데이터 받기 완료");
                            // 분봉데이터를 전부 다 수신한 이후에 일봉데이터를 요청한다.
                            gMainForm.getDayBongData(_itemCode, _conditionName, 0, gMainForm.GetScreenNumber());
                            //gMainForm.setLogText(_itemName + " : 일봉데이터 요청");
                        }

                    }
                }
            }
            else if(e.sRQName.Contains("1일봉1"))
            {
                int _listCount = gMainForm.KiwoomAPI.GetRepeatCnt(e.sTrCode, e.sRQName);
                string _itemCode = gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, 0, "종목코드").Trim();
                string _itemName = gMainForm.KiwoomAPI.GetMasterCodeName(_itemCode);
                string[] _rqName = e.sRQName.Split(';');
                string _conditionName = _rqName[1];

                //조건식리스트에서 조건식 이름이 같은 것을 찾아낸다.
                MyTradingCondition _mtc = gMainForm.gMyTradingConditionList.Find(o => o.conditionData.conditionName.Equals(_conditionName));
                if(_mtc != null)
                {
                    // 매매중인 종목코드를 비교해서 찾는다.
                    MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(_itemCode));
                    {
                        if(_mti != null)
                        {
                            _mti.m_totalDayBongCount += _listCount;
                            //일봉데이터를 저장한다.
                            for(int i=0; i < _listCount; i++)
                            {
                                _mti.db_endPrice[i] = Math.Abs(double.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "현재가")));
                                _mti.db_startPrice[i] = Math.Abs(double.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "시가")));
                                _mti.db_highPrice[i] = Math.Abs(double.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "고가")));
                                _mti.db_lowPrice[i] = Math.Abs(double.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "저가")));
                                _mti.db_tradingVolume[i] = Math.Abs(int.Parse(gMainForm.KiwoomAPI.GetCommData(e.sTrCode, e.sRQName, i, "거래량")));
                            }
                            _mti.m_bGetDayBongSuccess = true;
                            _mti.getDayBongMovingAveragePrice(0);
                            _mti.getDayBongBollingerPrice(0);
                            _mti.getDayBongEnvelopePrice(0);
                            _mti.getDayBongStochasticsSlowPrice(0);
                            _mti.CalculateIndicator();
                            DrawIndicateData(gMainForm.g_selectItemCode);

                            //gMainForm.setLogText(_itemName + ": 일봉데이타 받기 완료");
                            gMainForm.gFileIOInstance.WriteLogFile(_itemName + " : 일봉데이타 받기 완료");
                        }
                    }
                }
            }
        }
        // 주식정보요청 TR
        public void sendRequestItemInfoTR(string itemCode, string conditionName)
        {
            gMainForm.KiwoomAPI.SetInputValue("종목코드", itemCode); // opt10001 의 통합종목조회?
            gMainForm.KiwoomAPI.CommRqData("주식정보;" + conditionName, "opt10001", 0, gMainForm.GetScreenNumber());
        }
        //서버에서 보내 주는 메시지 수신 매서드
        private void onReceiveServerMessage(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEvent e)
        {
            gMainForm.setLogText("ServerMSG - ScreenNum : " + e.sScrNo + ". 사용자구분명 : " + e.sRQName);
            gMainForm.setLogText("ServerMSG - TR이름 : " + e.sTrCode + ", MSG : " + e.sMsg);
        }
        // 실시간시세 데이터 수신 메서드
        private void OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            string itemCode = e.sRealKey;
            string itemName = gMainForm.KiwoomAPI.GetMasterCodeName(itemCode);

            if(e.sRealType.Equals("주식체결")) // 해당 종목이 체결될때 마다 데이터를 받아서 계속 모니터링 한다.
            {
                long closingPrice = Math.Abs(long.Parse(gMainForm.KiwoomAPI.GetCommRealData(itemCode, 10))); // 현재가, 체결가
                double upDownRate = double.Parse(gMainForm.KiwoomAPI.GetCommRealData(itemCode, 12)); // 등락율
                long tradingVolume = long.Parse(gMainForm.KiwoomAPI.GetCommRealData(itemCode, 13)); // 누적거래량
                int volume = Math.Abs(int.Parse(gMainForm.KiwoomAPI.GetCommRealData(itemCode, 15))); // 거래량
                long startPrice = Math.Abs(long.Parse(gMainForm.KiwoomAPI.GetCommRealData(itemCode, 16))); // 시가
                long highPrice = Math.Abs(long.Parse(gMainForm.KiwoomAPI.GetCommRealData(itemCode, 17))); // 고가
                long lowPrice = Math.Abs(long.Parse(gMainForm.KiwoomAPI.GetCommRealData(itemCode, 18))); // 저가
                string tradingTime = gMainForm.KiwoomAPI.GetCommRealData(itemCode, 20).Trim();
                double conclusionStrength = double.Parse(gMainForm.KiwoomAPI.GetCommRealData(itemCode, 228)); // 체결강도

                foreach(PassitiveBuyingItem _pbi in gMainForm.gMyPassitiveBuyingItemList)
                {
                    if(_pbi.itemCode.Equals(itemCode))
                    {
                        _pbi.currentPrice = closingPrice;
                        PassitiveBuyingItemRealRateOfReturnAndEvaluationProfitLoss(_pbi, closingPrice, upDownRate);
                        TotalRateOfReturnAndEvaluationProfitLoss();
                    }
                }

                // 보유종목 검색
                foreach(MyHoldingItemOption _mho in gMainForm.gMyHoldingItemOptionList)
                {
                    MyHoldingItem _mhi = _mho.MyHoldingItemList.Find(o => o.m_itemCode.Equals(itemCode));

                    if (_mhi == null)
                        continue;

                    HoldingItemRealRateOfReturnAndEvaluationProfitLoss(_mhi, closingPrice, upDownRate);
                    TotalRateOfReturnAndEvaluationProfitLoss();

                    if (_mho.m_holdingitemState == HoldingItemState.Playing) // 전체보유매도가 켜져있을때
                    {
                        if (!_mhi.m_bSold && _mhi.m_rePurchaseNumber == -1 && _mhi.m_upperLimitPrice > 0) // 매도가 되지 않았을때 익절, 손절을 체크한다.
                        {
                            ////추매/////
                            if (_mhi.m_reBuyingUsing == 1)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    // 현재 차수보다 높은 추가매수는 진행하지 않도록 차수 제한
                                    if (i != _mhi.m_currentRebuyingStep)
                                        continue;

                                    if (_mhi.m_rePurchaseArray[i]) // 추매되었다면...
                                        continue;

                                    if (_mhi.m_reBuyingInvestment[i] == 0) // 추매금액이 0이면...
                                        continue;

                                    if (_mhi.m_rateOfReturn <= _mhi.m_reBuyingPer[i])
                                    {
                                        // 예수금 부족 확인
                                        if (gMainForm.curOrderAmount < _mhi.m_reBuyingInvestment[i])
                                        {
                                            gMainForm.setLogText("예수금이 부족하여 추매를 할 수 없습니다.");
                                            return;
                                        }

                                        _mhi.m_brePurchased = true; // 추가매수 성공시 true로 변환
                                        _mhi.m_rePurchaseArray[i] = true;
                                        _mhi.m_rePurchaseNumber = i;

                                        // 총매입금액에 대한 %를 계산하기위함
                                        int _qnt0 = (int)(_mhi.m_totalPurchaseAmount * (int)_mhi.m_reBuyingInvestment[i] / 100);
                                        // 주문수량계산 : 시장가의 경우 상한가를 기준으로 수량을 계산한다.
                                        int _qnt = (int)(_qnt0 / _mhi.m_upperLimitPrice);
                                        //Task _addBuyTask = new Task(() =>
                                        //{
                                            long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                    "추가매수",                                // 사용자 구분명
                                                                    gMainForm.GetScreenNumber(),   // 화면번호
                                                                    _mho.account,                                // 계좌번호 10자리
                                                                    1,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                    itemCode,                                     // 종목코드 6자리                                                        
                                                                    _qnt,                                              // 주문수량
                                                                    0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                    "03", // 시장가                                  // 거래구분
                                                                    "");                                               // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                                                                                       //Console.WriteLine("추가매수 결과값: " + _ret);

                                        if (_ret == 0) // 성공
                                        {
                                            //Console.WriteLine(itemName + " 일반추가매수주문성공");

                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                            {
                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode)
                                                    && row.Cells["매매진행_구분"].Value.ToString().Equals(_mho.division))
                                                {
                                                    row.Cells["매매진행_진행상황"].Value = "매수중";
                                                    break;
                                                }
                                            }
                                            gMainForm.setLogText("보유종목 추가 매수 성공 : " + itemName);
                                            gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 보유종목 추가 매수 주문 성공");

                                            //Console.WriteLine("_mti.m_currentRebuyingStep: " + _mti.m_currentRebuyingStep);
                                            // 현재 차수의 추가매수가 완료되었으므로 다음 차수로 넘어갑니다.
                                        }
                                        else // 실패
                                        {
                                            gMainForm.setLogText("추가 매수 실패 : " + itemName);
                                            gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 추가 매수 주문 실패");
                                        }
                                        //});
                                        //gMainForm.gOrderRequestManager.sendTaskData(_addBuyTask);
                                    }
                                }
                            }
                            ////추매진행중이 아니라면
                            if (_mhi.m_rePurchaseNumber == -1) // 추매진행중이 아니면...
                            {
                                bool _bTakeProfit = false;
                                bool _bStopLoss = false;
                                string _str = "";
                                int _arrayNumber = -1;

                                ///////////////////////////////////////////////////////// 익절 ///////////////////////////////////////////////////////
                                // 현재 수익률이 조건식에서 설정된 익절 %보다 높으면
                                for (int i = 0; i < 5; i++)
                                {
                                    if (i != _mhi.m_currentTakeProfitStep)
                                        continue;
                                    if (_mhi.m_takeProfitArray[i])
                                        continue;
                                    if (_mhi.m_takeProfitProportion[i] == 0)
                                        continue;

                                    if (_mhi.m_takeProfitUsing == 1)
                                    {
                                        if (_mhi.m_rateOfReturn >= _mhi.m_takeProfitBuyingPricePer[i])
                                        {
                                            _bTakeProfit = true;
                                            _arrayNumber = i;
                                            _str = _arrayNumber + 1 + "차 익절 : " + itemName;

                                            if (_bTakeProfit)
                                            {
                                                _mhi.m_bSold = true;

                                                int _qnt = _mhi.m_totalQnt * (int)_mhi.m_takeProfitProportion[_arrayNumber] / 100; // 보유수량
                                                //Task _takeProfitTask = new Task(() =>
                                                //{
                                                    long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                            "익절매도",                                     // 사용자 구분명
                                                                            gMainForm.GetScreenNumber(),   // 화면번호
                                                                            _mho.account,                                // 계좌번호 10자리
                                                                            2,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                            itemCode,                                     // 종목코드 6자리                                                        
                                                                            _qnt,                                              // 주문수량
                                                                            0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                            "03", // 시장가                                  // 거래구분
                                                                            "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                                                                                                //Console.WriteLine("기본매수후 익절 결과값: " + _ret);
                                                if (_ret == 0) // 익절매도 주문 성공
                                                {
                                                    foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                    {
                                                        if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode)
                                                            && row.Cells["매매진행_구분"].Value.ToString().Equals(_mho.division))
                                                        {
                                                            row.Cells["매매진행_진행상황"].Value = _arrayNumber + 1 + "차 익절";
                                                            break;
                                                        }
                                                    }
                                                    _mhi.m_currentTakeProfitStep++;
                                                    //Console.WriteLine("현재 익절 차수: " + _mti.m_currentTakeProfitStep);
                                                    gMainForm.setLogText(_arrayNumber + 1 + "차 익절 매도 주문 성공 : " + itemName);
                                                    gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 익절 매도 주문 성공");
                                                }
                                                else
                                                {
                                                    gMainForm.setLogText(_arrayNumber + 1 + "차 익절 매도 실패 : " + itemName);
                                                    gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 익절 매도 주문 실패");
                                                }
                                                //});
                                                //gMainForm.gOrderRequestManager.sendTaskData(_takeProfitTask);
                                            }
                                        }
                                    }
                                }
                                ////////////////////////////////////////////////////////////// 손절 /////////////////////////////////////////////////////////////////
                                // 현재 수익률이 조건식에서 설정된 손절 %보다 낮으면
                                if (!_bTakeProfit)
                                {
                                    for (int i = 0; i < 5; i++)
                                    {
                                        if (i != _mhi.m_currentStopLossStep)
                                            continue;
                                        if (_mhi.m_stopLossArray[i])
                                            continue;
                                        if (_mhi.m_stopLossProportion[i] == 0)
                                            continue;

                                        if (_mhi.m_stopLossUsing == 1)
                                        {
                                            if (_mhi.m_rateOfReturn <= _mhi.m_stopLossBuyingPricePer[i])
                                            {
                                                _bStopLoss = true;
                                                _arrayNumber = i;
                                                _str = _arrayNumber + 1 + "차 손절 : " + itemName;

                                                if (_bStopLoss)
                                                {
                                                    _mhi.m_bSold = true;

                                                    int _qnt = _mhi.m_totalQnt * (int)_mhi.m_stopLossProportion[_arrayNumber] / 100; // 보유수량
                                                    //Task _stopLossTask = new Task(() =>
                                                    //{

                                                        long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                "손절매도",                                     // 사용자 구분명
                                                                                gMainForm.GetScreenNumber(),   // 화면번호
                                                                                _mho.account,                                // 계좌번호 10자리
                                                                                2,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                itemCode,                                     // 종목코드 6자리                                                        
                                                                                _qnt,                                              // 주문수량
                                                                                0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                                "03", // 시장가                                  // 거래구분
                                                                                "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                                                                                                    //Console.WriteLine("기본매수후 손절 결과값: " + _ret);
                                                    if (_ret == 0) // 손절매도 주문 성공
                                                    {
                                                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                        {
                                                            if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode)
                                                                && row.Cells["매매진행_구분"].Value.ToString().Equals(_mho.division))
                                                            {
                                                                row.Cells["매매진행_진행상황"].Value = _arrayNumber + 1 + "차 손절";
                                                                break;
                                                            }
                                                        }
                                                        _mhi.m_currentStopLossStep++;
                                                        //Console.WriteLine("현재 손절 차수: " + _mti.m_currentStopLossStep);
                                                        gMainForm.setLogText(_arrayNumber + 1 + "차 손절 매도 주문 성공 : " + itemName);
                                                        gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 손절 매도 주문 성공");
                                                    }
                                                    else
                                                    {
                                                        gMainForm.setLogText("손절 매도 주문 실패 : " + itemName);
                                                        gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 손절 매도 주문 실패");
                                                    }
                                                    //});
                                                    //gMainForm.gOrderRequestManager.sendTaskData(_stopLossTask);
                                                }
                                            }
                                        }
                                    }
                                }
                                //ts매도
                                if (_mhi.m_tsMedoUsing == 1)
                                {
                                    foreach (GoJumMedo _gmd in gMainForm.gojumMedoList)
                                    {
                                        // gojumMedoList 에서 동일한 종목코드인지를 확인함
                                        GoJumMedo _goJumMedo = gMainForm.gojumMedoList.Find(o => o.itemCode.Equals(itemCode));

                                        if (_goJumMedo != null)
                                        {
                                            //ts매도 변수
                                            bool _bTsmedoCheck = false;

                                            //3번의 ts매도를 확인함
                                            for (int i = 0; i < 3; i++)
                                            {
                                                if (i != _mhi.m_currentTsmedoStep)
                                                    continue;

                                                if (_mhi.m_tsMedoArray[i]) // ts매도 여부 확인
                                                    continue;

                                                if (_mhi.m_tsMedoProportion[i] == 0) // 비중이 0이라면
                                                    continue;

                                                if (_mhi.m_tsMedoUsingType[i] == 0) //목표가ts
                                                {
                                                    if (_mhi.m_currentTsmedoStep == 0 && _mhi.m_rateOfReturn >= _mhi.m_tsMedoAchievedPer[i]) // 목표가와 수익률이 동일해졋을때,
                                                    {
                                                        if (closingPrice > _goJumMedo.highPrice[i] && closingPrice > _gmd.purchasePrice)
                                                        {
                                                            _goJumMedo.highPrice[i] = closingPrice;
                                                            //Console.WriteLine("1차 목표가ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);
                                                        }
                                                    }
                                                    if (_mhi.m_currentTsmedoStep == 1 && _mhi.m_rateOfReturn >= _mhi.m_tsMedoAchievedPer[i]) // 목표가와 수익률이 동일해졋을때,
                                                    {
                                                        if (closingPrice > _goJumMedo.highPrice[i])
                                                        {
                                                            _goJumMedo.highPrice[i] = closingPrice;
                                                            //Console.WriteLine("2차 목표가ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);
                                                        }
                                                    }
                                                    if (_mhi.m_currentTsmedoStep == 2 && _mhi.m_rateOfReturn >= _mhi.m_tsMedoAchievedPer[i]) // 목표가와 수익률이 동일해졋을때,
                                                    {
                                                        if (closingPrice > _goJumMedo.highPrice[i])
                                                        {
                                                            _goJumMedo.highPrice[i] = closingPrice;
                                                            //Console.WriteLine("3차 목표가ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);
                                                        }
                                                    }
                                                    double dropPercent = _mho.tsMedoPercent[i] / 100;
                                                    double dropPrice = _goJumMedo.highPrice[i] * (1 - dropPercent);
                                                    //Console.WriteLine("종목명: " + itemName + " 목표가ts사용중" + " 현재가: " + closingPrice + " 고점가격: " + _goJumMedo.highPrice + " 하락률: " + dropPercent + " 고점대비 하락가격: " + dropPrice);
                                                    if (_mhi.m_tsProfitPreservation1 == 1 && _mhi.m_currentTsmedoStep == 0)
                                                    {
                                                        if (closingPrice <= dropPrice && _mhi.m_rateOfReturn > 0)
                                                        {
                                                            //Console.WriteLine("1차 목표가 ts 이익구간작동");
                                                            //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                            _bTsmedoCheck = true;
                                                            _arrayNumber = i;
                                                            _str = "목표가ts매도 : " + itemName;
                                                            break;
                                                        }
                                                    }
                                                    if (_mhi.m_tsProfitPreservation1 == 0 && _mhi.m_currentTsmedoStep == 0)
                                                    {
                                                        if (closingPrice <= dropPrice)
                                                        {
                                                            //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                            _bTsmedoCheck = true;
                                                            _arrayNumber = i;
                                                            _str = "목표가ts매도 : " + itemName;
                                                            break;
                                                        }
                                                    }
                                                    if (_mhi.m_tsProfitPreservation2 == 1 && _mhi.m_currentTsmedoStep == 1)
                                                    {
                                                        if (closingPrice <= dropPrice && _mhi.m_rateOfReturn > 0)
                                                        {
                                                            //Console.WriteLine("2차 목표가 ts 이익구간작동");
                                                            //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                            _bTsmedoCheck = true;
                                                            _arrayNumber = i;
                                                            _str = "목표가ts매도 : " + itemName;
                                                            break;
                                                        }
                                                    }
                                                    if (_mhi.m_tsProfitPreservation2 == 0 && _mhi.m_currentTsmedoStep == 1)
                                                    {
                                                        if (closingPrice <= dropPrice)
                                                        {
                                                            //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                            _bTsmedoCheck = true;
                                                            _arrayNumber = i;
                                                            _str = "목표가ts매도 : " + itemName;
                                                            break;
                                                        }
                                                    }
                                                    if (_mhi.m_tsProfitPreservation3 == 1 && _mhi.m_currentTsmedoStep == 2)
                                                    {
                                                        if (closingPrice <= dropPrice && _mhi.m_rateOfReturn > 0)
                                                        {
                                                            //Console.WriteLine("3차 목표가 ts 이익구간작동");
                                                            //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                            _bTsmedoCheck = true;
                                                            _arrayNumber = i;
                                                            _str = "목표가ts매도 : " + itemName;
                                                            break;
                                                        }
                                                    }
                                                    if (_mhi.m_tsProfitPreservation3 == 0 && _mhi.m_currentTsmedoStep == 2)
                                                    {
                                                        if (closingPrice <= dropPrice)
                                                        {
                                                            //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                            _bTsmedoCheck = true;
                                                            _arrayNumber = i;
                                                            _str = "목표가ts매도 : " + itemName;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (_mhi.m_tsMedoUsingType[i] == 1) //고점ts
                                                {
                                                    if (_mhi.m_currentTsmedoStep == 0 && closingPrice > _goJumMedo.highPrice[i] && closingPrice >= _gmd.purchasePrice)
                                                    {
                                                        _goJumMedo.highPrice[i] = closingPrice;
                                                        //Console.WriteLine("1차 고점ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);
                                                    }
                                                    // 현재가가 고점보다 높을경우 고점가격에 대한 갱신
                                                    if (_mhi.m_currentTsmedoStep == 1 && _mhi.m_tsMedoArray[_mhi.m_currentTsmedoStep - 1] && closingPrice > _goJumMedo.highPrice[i])
                                                    {
                                                        _goJumMedo.highPrice[i] = closingPrice;
                                                        //Console.WriteLine("2차 고점ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);

                                                    }
                                                    if (_mhi.m_currentTsmedoStep == 2 && _mhi.m_tsMedoArray[_mhi.m_currentTsmedoStep - 1] && closingPrice > _goJumMedo.highPrice[i])
                                                    {
                                                        _goJumMedo.highPrice[i] = closingPrice;
                                                        //Console.WriteLine("3차 고점ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);

                                                    }
                                                    double dropPercent = _mho.tsMedoPercent[i] / 100;
                                                    double dropPrice = _goJumMedo.highPrice[i] * (1 - dropPercent);
                                                    //Console.WriteLine("종목명: " + itemName + " 고점ts사용중"  +" 현재가: " + closingPrice + " 고점가격: " + _goJumMedo.highPrice + " 하락률: " + dropPercent + " 고점대비 하락가격: " + dropPrice);
                                                    if (_mhi.m_tsProfitPreservation1 == 1 && _mhi.m_currentTsmedoStep == 0)
                                                    {
                                                        if (closingPrice <= dropPrice && _mhi.m_rateOfReturn > 0)
                                                        {
                                                            //Console.WriteLine("1차 고점 ts 이익구간작동");
                                                            //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                            _bTsmedoCheck = true;
                                                            _arrayNumber = i;
                                                            _str = "고점ts매도 : " + itemName;
                                                            break;
                                                        }
                                                    }
                                                    if (_mhi.m_tsProfitPreservation1 == 0 && _mhi.m_currentTsmedoStep == 0)
                                                    {
                                                        if (closingPrice <= dropPrice)
                                                        {
                                                            //Console.WriteLine("1차 고점ts 이익구간작동안함");
                                                            _bTsmedoCheck = true;
                                                            _arrayNumber = i;
                                                            _str = "고점ts매도 : " + itemName;
                                                            break;
                                                        }
                                                    }
                                                    if (_mhi.m_tsProfitPreservation2 == 1 && _mhi.m_currentTsmedoStep == 1)
                                                    {
                                                        if (closingPrice <= dropPrice && _mhi.m_rateOfReturn > 0)
                                                        {
                                                            //Console.WriteLine("2차 고점 ts 이익구간작동");
                                                            //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                            _bTsmedoCheck = true;
                                                            _arrayNumber = i;
                                                            _str = "고점ts매도 : " + itemName;
                                                            break;
                                                        }
                                                    }
                                                    if (_mhi.m_tsProfitPreservation2 == 0 && _mhi.m_currentTsmedoStep == 1)
                                                    {
                                                        if (closingPrice <= dropPrice)
                                                        {
                                                            //Console.WriteLine("2차 고점ts 이익구간작동안함");
                                                            _bTsmedoCheck = true;
                                                            _arrayNumber = i;
                                                            _str = "고점ts매도 : " + itemName;
                                                            break;
                                                        }
                                                    }
                                                    if (_mhi.m_tsProfitPreservation3 == 1 && _mhi.m_currentTsmedoStep == 2)
                                                    {
                                                        if (closingPrice <= dropPrice && _mhi.m_rateOfReturn > 0)
                                                        {
                                                            //Console.WriteLine("3차 고점 ts 이익구간작동");
                                                            //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                            _bTsmedoCheck = true;
                                                            _arrayNumber = i;
                                                            _str = "고점ts매도 : " + itemName;
                                                            break;
                                                        }
                                                    }
                                                    if (_mhi.m_tsProfitPreservation3 == 0 && _mhi.m_currentTsmedoStep == 2)
                                                    {
                                                        if (closingPrice <= dropPrice)
                                                        {
                                                            //Console.WriteLine("3차 고점ts 이익구간작동안함");
                                                            _bTsmedoCheck = true;
                                                            _arrayNumber = i;
                                                            _str = "고점ts매도 : " + itemName;
                                                            break;
                                                        }
                                                    }

                                                }
                                            }
                                            if (_bTsmedoCheck) //매도주문 코딩
                                            {
                                                _mhi.m_bSold = true;
                                                _mhi.m_tsMedoArray[_arrayNumber] = true;
                                                _mhi.m_tsMedoNumber = _arrayNumber;

                                                // 정해진 비중만큼 계산을 해야한다. 이때 배열이있으니깐 for문을 돌려야할듯?
                                                //Console.WriteLine("_arrayNumber: " + _arrayNumber);
                                                int _qnt = _mhi.m_totalQnt * (int)_mhi.m_tsMedoProportion[_arrayNumber] / 100;
                                                //Console.WriteLine("ts매도수량계산: " + _qnt);
                                                Task _tsMedoTask = new Task(() =>
                                                {
                                                    long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                            "ts매도",                  // 사용자구분명
                                                                                            gMainForm.GetScreenNumber(), // 화면번호
                                                                                            _mho.account,                // 계좌번호 10자리
                                                                                            2,                           // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                            itemCode,                    // 종목코드 6자리
                                                                                            _qnt,                        // 주문수량
                                                                                            0,                           // 주문가격 : 시장가일때는 0이다.
                                                                                            "03",                        // 거래구분 : 시장가는 "03"
                                                                                            "");                         // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                if (_ret == 0) // ts매도성공
                                                {
                                                    foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                    {
                                                        if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) &&
                                                            row.Cells["매매진행_구분"].Value.ToString().Equals(_mho.division))
                                                        {
                                                            row.Cells["매매진행_진행상황"].Value = _arrayNumber + 1 + "차ts매도";
                                                            break;
                                                        }
                                                    }
                                                    gMainForm.setLogText(_arrayNumber + 1 + "차ts 매도 성공: " + itemName);

                                                    _mhi.m_currentTsmedoStep++;
                                                    //Console.WriteLine("현재 ts매도 차수: " + _mti.m_currentTsmedoStep);
                                                }
                                                else
                                                {
                                                    gMainForm.setLogText(_arrayNumber + 1 + "차ts 매도 실패: " + itemName);
                                                }
                                                });
                                                gMainForm.gOrderRequestManager.sendTaskData(_tsMedoTask);
                                            }
                                            /*
                                            // 현재가가 고점보다 높을경우 고점가격에 대한 갱신
                                            if (closingPrice > _goJumMedo.highPrice && closingPrice > _gmd.purchasePrice)
                                            {
                                                _goJumMedo.highPrice = closingPrice;
                                                //Console.WriteLine("시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice);
                                            }
                                            //double dropPercent = _mtc.tsMedoPercent[0] / 100;
                                            //double dropPrice = _goJumMedo.highPrice * (1 - dropPercent);
                                            //Console.WriteLine("고점가격: " + _goJumMedo.highPrice + " 하락률: " + dropPercent + " 고점대비 하락가격: " + dropPrice);
                                            if (closingPrice <= _goJumMedo.highPrice * (1 - dropPercent))
                                            {
                                                int _qnt = _mti.m_totalQnt; //보유수량
                                                long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                "ts매도",                  // 사용자구분명
                                                                                gMainForm.GetScreenNumber(), // 화면번호
                                                                                _mtc.account,                // 계좌번호 10자리
                                                                                2,                           // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                itemCode,                    // 종목코드 6자리
                                                                                _qnt,                        // 주문수량
                                                                                0,                           // 주문가격 : 시장가일때는 0이다.
                                                                                "03",                        // 거래구분 : 시장가는 "03"
                                                                                "");                         // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                if (_ret == 0) // ts매도성공
                                                {
                                                    _mti.m_bSold = true;
                                                    foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                    {
                                                        if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) &&
                                                            row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                        {
                                                            row.Cells["매매진행_진행상황"].Value = "ts매도중";
                                                            break;
                                                        }
                                                    }
                                                    gMainForm.setLogText("ts 매도 성공: " + itemCode);
                                                }
                                                else
                                                {
                                                    gMainForm.setLogText("ts 매도 실패: " + itemCode);
                                                }
                                            }*/
                                        }
                                        else
                                        {
                                            // _goJumMedo가 null일 경우 로그를 남기거나 적절한 조치를 취합니다.
                                            //Console.WriteLine("매수체결이 완료되지않음.");
                                        }
                                    }

                                }
                            }
                        }
                    }
                }

                // 조건식 리스트 검색
                foreach (MyTradingCondition _mtc in gMainForm.gMyTradingConditionList)
                {
                    //종목리스트에서 같은 종목 검색
                    MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(itemCode)); // 매매중인 종목 찾음

                    if (_mti == null) // 같은 종목이 없으면 아래 코드들은 건너뛰고 다음 종목으로 넘어가
                        continue;
                    if (_mti.m_volumeState == (int)trading.wait)
                    {
                        // 현재가
                        _mti.m_currentPrice = closingPrice;
                        // 종목 그리드뷰에 현재가를 넣어준다.
                        RealRateOfReturnAndEvaluationProfitLoss(_mti, closingPrice,upDownRate);
                        // 총수익, 총매수, 총손익등등 계산
                        TotalRateOfReturnAndEvaluationProfitLoss();

                        if (_mti.m_bCompletePurchase && _mtc.m_conditionState == ConditionState.Playing) //매수완료시
                        {
                            //Console.WriteLine("종목명: " + _mti.m_itemName + " _mti.m_bSold: " + _mti.m_bSold + " _mti.m_rePurchaseNumber: " + _mti.m_rePurchaseNumber + " _mti.m_upperLimitPrice: " + _mti.m_upperLimitPrice);
                            if (!_mti.m_bSold && _mti.m_rePurchaseNumber == -1 && _mti.m_upperLimitPrice > 0) // 매도가 되지 않았을때 익절, 손절을 체크한다.
                            {
                                //Console.WriteLine("조건식명: " + _mtc.conditionData.conditionName + " 종목명: " + _mti.m_itemName + " _mti.m_bCompletePurchase: " + _mti.m_bCompletePurchase + " _mtc.m_conditionState: " + _mtc.m_conditionState + " _mti.m_bSold: " + _mti.m_bSold + " _mti.m_rePurchaseNumber: " + _mti.m_rePurchaseNumber + " 상한가: " + _mti.m_upperLimitPrice);
                                if (_mti.m_reBuyingType == 0)
                                {
                                    //Console.WriteLine("_mti.m_reBuyingType: " + _mti.m_reBuyingType);
                                    /////////////////////////////////////////////////////////// 추매 ////////////////////////////////////////////////////////
                                    
                                    for (int i = 0; i < 5; i++)
                                    {
                                        //추가매수변수
                                        int _arrayNumber = -1;

                                        // 현재 차수보다 높은 추가매수는 진행하지 않도록 차수 제한
                                        if (i != _mti.m_currentRebuyingStep)
                                            continue;

                                        if (_mti.m_rePurchaseArray[i + 1]) // 추매되었다면...
                                            continue;

                                        if (_mti.m_buyingPerInvestment[i + 1] == 0) // 추매금액이 0이면...
                                            continue;

                                        if (_mti.m_rateOfReturn <= _mti.m_reBuyingPer[i])
                                        {
                                            _arrayNumber = i + 1;
                                            // 예수금 부족 확인
                                            if (gMainForm.curOrderAmount < _mti.m_buyingPerInvestment[_arrayNumber])
                                            {
                                                gMainForm.setLogText("예수금이 부족하여 추매를 할 수 없습니다.");
                                                return;
                                            }

                                            _mti.m_brePurchased = true; // 추가매수 성공시 true로 변환
                                            _mti.m_rePurchaseArray[_arrayNumber] = true;
                                            _mti.m_rePurchaseNumber = _arrayNumber;

                                            // 주문수량계산 : 시장가의 경우 상한가를 기준으로 수량을 계산한다.
                                            int _qnt = (int)(_mti.m_buyingPerInvestment[_arrayNumber] / _mti.m_upperLimitPrice);
                                            Task _addBuyTask = new Task(() =>
                                            {
                                                long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                        "추가매수",                                // 사용자 구분명
                                                                        gMainForm.GetScreenNumber(),   // 화면번호
                                                                        _mtc.account,                                // 계좌번호 10자리
                                                                        1,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                        itemCode,                                     // 종목코드 6자리                                                        
                                                                        _qnt,                                              // 주문수량
                                                                        0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                        "03", // 시장가                                  // 거래구분
                                                                        "");                                               // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                            //Console.WriteLine("추가매수 결과값: " + _ret);

                                            if (_ret == 0) // 성공
                                            {
                                                //Console.WriteLine(itemName + " 일반추가매수주문성공");

                                                foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                {
                                                    if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode)
                                                        && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                    {
                                                        row.Cells["매매진행_진행상황"].Value = "매수중";
                                                        break;
                                                    }
                                                }
                                                gMainForm.setLogText("추가 매수 성공 : " + itemName);
                                                gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 추가 매수 주문 성공");

                                                //Console.WriteLine("_mti.m_currentRebuyingStep: " + _mti.m_currentRebuyingStep);
                                                // 현재 차수의 추가매수가 완료되었으므로 다음 차수로 넘어갑니다.
                                                _mti.m_currentRebuyingStep++;
                                            }
                                            else // 실패
                                            {
                                                gMainForm.setLogText("추가 매수 실패 : " + itemName);
                                                gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 추가 매수 주문 실패");
                                            }
                                            });
                                            gMainForm.gOrderRequestManager.sendTaskData(_addBuyTask);
                                        }
                                    }
                                }
                                if (_mti.m_rePurchaseNumber == -1) // 추매진행중이 아니면...
                                {
                                    bool _bTakeProfit = false;
                                    bool _bStopLoss = false;
                                    string _str = "";
                                    string _str1 = "";
                                    int _arrayNumber = -1;

                                    ///////////////////////////////////////////////////////// 익절 ///////////////////////////////////////////////////////
                                    // 현재 수익률이 조건식에서 설정된 익절 %보다 높으면
                                    for(int i =0; i<5; i++)
                                    {
                                        if (i != _mti.m_currentTakeProfitStep)
                                            continue;
                                        if (_mti.m_takeProfitArray[i])
                                            continue;
                                        if (_mti.m_takeProfitProportion[i] == 0)
                                            continue;

                                        if (_mti.m_takeProfitUsing == 1 && _mti.m_takeProfitType == 0)
                                        {
                                            //Console.WriteLine("종목명: " + itemName + " 수익률: " + _mti.m_rateOfReturn + " 차수: " + i + " 차수도달 수익률: " +_mti.m_takeProfitBuyingPricePer[i]);
                                            if (_mti.m_rateOfReturn >= _mti.m_takeProfitBuyingPricePer[i])
                                            {
                                                _bTakeProfit = true;
                                                _arrayNumber = i;
                                                _str = _arrayNumber + 1 + "차 익절 : " + itemName;

                                                if (_bTakeProfit && !_mti.m_isMedoOrderPlaced)
                                                {
                                                    if(_arrayNumber ==0)
                                                    {
                                                        _mti.m_isMedoOrderPlaced = true;
                                                        _mti.m_bSold = true;

                                                        double calculated = _mti.m_totalQnt * _mti.m_takeProfitProportion[_arrayNumber] / 100.0;
                                                        int _qnt = (calculated > 0 && calculated < 1) ? 1 : (int)Math.Round(calculated, MidpointRounding.AwayFromZero);
                                                        Console.WriteLine("_arraynum = 0: " + "종목명: " + _mti.m_itemName + " 주문가능수량: " + _mti.m_orderQnt + " 총수량: " + _mti.m_totalQnt + " " + _arrayNumber + "차 익절주문시 수량: " + _qnt + " arraynumber: " + _arrayNumber + " 매도비율: " + (int)_mti.m_takeProfitProportion[_arrayNumber] + " 수익률: " + _mti.m_rateOfReturn + " 목표수익률: " + _mti.m_takeProfitBuyingPricePer[i]);
                                                        Task _takeProfitTask = new Task(() =>
                                                        {
                                                            long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                    "익절매도",                                     // 사용자 구분명
                                                                                    gMainForm.GetScreenNumber(),   // 화면번호
                                                                                    _mtc.account,                                // 계좌번호 10자리
                                                                                    2,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                    itemCode,                                     // 종목코드 6자리                                                        
                                                                                    _qnt,                                              // 주문수량
                                                                                    0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                                    "03", // 시장가                                  // 거래구분
                                                                                    "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                                                                                                        //Console.WriteLine("기본매수후 익절 결과값: " + _ret);
                                                        if (_ret == 0) // 익절매도 주문 성공
                                                        {

                                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                            {
                                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode)
                                                                    && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                                {
                                                                    row.Cells["매매진행_진행상황"].Value = _arrayNumber + 1 + "차 익절";
                                                                    break;
                                                                }
                                                            }
                                                            _mti.m_currentTakeProfitStep++;
                                                            //Console.WriteLine("현재 익절 차수: " + _mti.m_currentTakeProfitStep);
                                                            Console.WriteLine(itemName + " 익절 주문 성공");
                                                            gMainForm.setLogText(_arrayNumber + 1 + "차 익절 매도 주문 성공 : " + itemName);
                                                            gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 익절 매도 주문 성공");
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine(itemName + " 익절 주문 실패");
                                                            gMainForm.setLogText(_arrayNumber + 1 + "차 익절 매도 실패 : " + itemName);
                                                            gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 익절 매도 주문 실패");
                                                        }
                                                        });
                                                        gMainForm.gOrderRequestManager.sendTaskData(_takeProfitTask);
                                                    }
                                                    if(_arrayNumber > 0)
                                                    {
                                                        _mti.m_isMedoOrderPlaced = true;
                                                        _mti.m_bSold = true;

                                                        double calculated = _mti.m_totalQnt * _mti.m_takeProfitProportion[_arrayNumber] / 100.0;
                                                        int _qnt = (calculated > 0 && calculated < 1) ? 1 : (int)Math.Round(calculated, MidpointRounding.AwayFromZero);
                                                        Console.WriteLine("_arraynum > 0: " + "종목명: " + _mti.m_itemName + " 주문가능수량: " + _mti.m_orderQnt + " 총수량: " + _mti.m_totalQnt + " " + _arrayNumber + "차 익절주문시 수량: " + _qnt + " arraynumber: " + _arrayNumber + " 매도비율: " + (int)_mti.m_takeProfitProportion[_arrayNumber] + " 수익률: " + _mti.m_rateOfReturn + " 목표수익률: " + _mti.m_takeProfitBuyingPricePer[i]);
                                                        Task _takeProfitTask2 = new Task(() =>
                                                        {
                                                            long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                    "익절매도",                                     // 사용자 구분명
                                                                                    gMainForm.GetScreenNumber(),   // 화면번호
                                                                                    _mtc.account,                                // 계좌번호 10자리
                                                                                    2,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                    itemCode,                                     // 종목코드 6자리                                                        
                                                                                    _qnt,                                              // 주문수량
                                                                                    0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                                    "03", // 시장가                                  // 거래구분
                                                                                    "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                                                                                                        //Console.WriteLine("기본매수후 익절 결과값: " + _ret);
                                                        if (_ret == 0) // 익절매도 주문 성공
                                                        {

                                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                            {
                                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode)
                                                                    && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                                {
                                                                    row.Cells["매매진행_진행상황"].Value = _arrayNumber + 1 + "차 익절";
                                                                    break;
                                                                }
                                                            }
                                                            _mti.m_currentTakeProfitStep++;
                                                            //Console.WriteLine("현재 익절 차수: " + _mti.m_currentTakeProfitStep);
                                                            Console.WriteLine(itemName + " 익절 주문 성공");
                                                            gMainForm.setLogText(_arrayNumber + 1 + "차 익절 매도 주문 성공 : " + itemName);
                                                            gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 익절 매도 주문 성공");
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine(itemName + " 익절 주문 실패");
                                                            gMainForm.setLogText(_arrayNumber + 1 + "차 익절 매도 실패 : " + itemName);
                                                            gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 익절 매도 주문 실패");
                                                        }
                                                        });
                                                        gMainForm.gOrderRequestManager.sendTaskData(_takeProfitTask2);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    ////////////////////////////////////////////////////////////// 손절 /////////////////////////////////////////////////////////////////
                                    // 현재 수익률이 조건식에서 설정된 손절 %보다 낮으면
                                    if (!_bTakeProfit)
                                    {
                                        for (int i = 0; i < 5; i++)
                                        {
                                            if (i != _mti.m_currentStopLossStep)
                                                continue;
                                            if (_mti.m_stopLossArray[i])
                                                continue;
                                            if (_mti.m_stopLossProportion[i] == 0)
                                                continue;

                                            if (_mti.m_stopLossUsing == 1 && _mti.m_stopLossType == 0)
                                            {
                                                if (_mti.m_rateOfReturn <= _mti.m_stopLossBuyingPricePer[i])
                                                {
                                                    _bStopLoss = true;
                                                    _arrayNumber = i;
                                                    _str1 = _arrayNumber + 1 + "차 손절";

                                                    if (_bStopLoss && !_mti.m_isMedoOrderPlaced)
                                                    {
                                                        if(_arrayNumber == 0)
                                                        {
                                                            _mti.m_isMedoOrderPlaced = true;
                                                            _mti.m_bSold = true;

                                                            double calculated = _mti.m_totalQnt * _mti.m_stopLossProportion[_arrayNumber] / 100.0;
                                                            int _qnt = (calculated > 0 && calculated < 1) ? 1 : (int)Math.Round(calculated, MidpointRounding.AwayFromZero);

                                                            gMainForm.setLogText(itemName + " : " + _str1 + " 가능 : " + _mti.m_rateOfReturn.ToString("N2") + "%" + " 현재가: " + closingPrice);
                                                            Task _stopLossTask = new Task(() =>
                                                            {

                                                                long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                        "손절매도",                                     // 사용자 구분명
                                                                                        gMainForm.GetScreenNumber(),   // 화면번호
                                                                                        _mtc.account,                                // 계좌번호 10자리
                                                                                        2,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                        itemCode,                                     // 종목코드 6자리                                                        
                                                                                        _qnt,                                              // 주문수량
                                                                                        0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                                        "03", // 시장가                                  // 거래구분
                                                                                        "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                                                                                                            //Console.WriteLine("기본매수후 손절 결과값: " + _ret);
                                                                gMainForm.setLogText(itemName + " : " + _str1 + " 주문 보냄.");

                                                                if (_ret == 0) // 손절매도 주문 성공
                                                            {
                                                                foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                                {
                                                                    if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode)
                                                                        && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                                    {
                                                                        row.Cells["매매진행_진행상황"].Value = _arrayNumber + 1 + "차 손절";
                                                                        break;
                                                                    }
                                                                }
                                                                _mti.m_currentStopLossStep++;
                                                                //Console.WriteLine("현재 손절 차수: " + _mti.m_currentStopLossStep);
                                                                gMainForm.setLogText(_arrayNumber + 1 + "차 손절 매도 주문 성공 : " + itemName);
                                                                gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 손절 매도 주문 성공");
                                                            }
                                                            else
                                                            {
                                                                gMainForm.setLogText("손절 매도 주문 실패 : " + itemName);
                                                                gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 손절 매도 주문 실패");
                                                            }
                                                            });
                                                            gMainForm.gOrderRequestManager.sendTaskData(_stopLossTask);
                                                        }
                                                        if (_arrayNumber > 0)
                                                        {
                                                            _mti.m_isMedoOrderPlaced = true;
                                                            _mti.m_bSold = true;

                                                            double calculated = _mti.m_totalQnt * _mti.m_stopLossProportion[_arrayNumber] / 100.0;
                                                            int _qnt = (calculated > 0 && calculated < 1) ? 1 : (int)Math.Round(calculated, MidpointRounding.AwayFromZero);

                                                            gMainForm.setLogText(itemName + " : " + _str + " 가능 : " + _mti.m_rateOfReturn.ToString("N2") + "%" + " 현재가: " + closingPrice);
                                                            Task _stopLossTask2 = new Task(() =>
                                                            {
                                                                long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                        "손절매도",                                     // 사용자 구분명
                                                                                        gMainForm.GetScreenNumber(),   // 화면번호
                                                                                        _mtc.account,                                // 계좌번호 10자리
                                                                                        2,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                        itemCode,                                     // 종목코드 6자리                                                        
                                                                                        _qnt,                                              // 주문수량
                                                                                        0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                                        "03", // 시장가                                  // 거래구분
                                                                                        "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                                                                                                            //Console.WriteLine("기본매수후 손절 결과값: " + _ret);
                                                                gMainForm.setLogText(itemName + " : " + _str1 + " 주문 보냄.");

                                                            if (_ret == 0) // 손절매도 주문 성공
                                                            {

                                                                foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                                {
                                                                    if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode)
                                                                        && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                                    {
                                                                        row.Cells["매매진행_진행상황"].Value = _arrayNumber + 1 + "차 손절";
                                                                        break;
                                                                    }
                                                                }
                                                                _mti.m_currentStopLossStep++;
                                                                //Console.WriteLine("현재 손절 차수: " + _mti.m_currentStopLossStep);
                                                                gMainForm.setLogText(_arrayNumber + 1 + "차 손절 매도 주문 성공 : " + itemName);
                                                                gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 손절 매도 주문 성공");
                                                            }
                                                            else
                                                            {
                                                                gMainForm.setLogText("손절 매도 주문 실패 : " + itemName);
                                                                gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 손절 매도 주문 실패");
                                                            }
                                                            });
                                                            gMainForm.gOrderRequestManager.sendTaskData(_stopLossTask2);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //ts매도
                                    if (_mtc.tsMedoUsing == 1)
                                    {
                                        foreach (GoJumMedo _gmd in gMainForm.gojumMedoList)
                                        {
                                            // gojumMedoList 에서 동일한 종목코드인지를 확인함
                                            GoJumMedo _goJumMedo = gMainForm.gojumMedoList.Find(o => o.itemCode.Equals(itemCode));

                                            if (_goJumMedo != null)
                                            {
                                                //ts매도 변수
                                                bool _bTsmedoCheck = false;

                                                //3번의 ts매도를 확인함
                                                for (int i = 0; i < 3; i++)
                                                {
                                                    if (i != _mti.m_currentTsmedoStep)
                                                        continue;

                                                    if (_mti.m_tsMedoArray[i]) // ts매도 여부 확인
                                                        continue;

                                                    if (_mti.m_tsMedoProportion[i] == 0) // 비중이 0이라면
                                                        continue;

                                                    if (_mti.m_tsMedoUsingType[i] == 0) //목표가ts
                                                    {
                                                        if (_mti.m_currentTsmedoStep == 0 && _mti.m_rateOfReturn >= _mti.m_tsMedoArchievedPer[i]) // 목표가와 수익률이 동일해졋을때,
                                                        {
                                                            if (closingPrice > _goJumMedo.highPrice[i] && closingPrice > _gmd.purchasePrice)
                                                            {
                                                                _goJumMedo.highPrice[i] = closingPrice;
                                                                if(!_mti._bFirstTsmedologCheck)
                                                                {
                                                                    gMainForm.setLogText("종목명: " + itemName + " 1차 목표가ts도달 - " + _mti.m_tsMedoArchievedPer[i] + " % " + " 현재수익률: " + _mti.m_rateOfReturn.ToString("N2") + "%" + " 현재가격: " + _mti.m_currentPrice);
                                                                    _mti._bFirstTsmedologCheck = true;
                                                                }
                                                                
                                                                //Console.WriteLine("1차 목표가ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);
                                                            }
                                                        }
                                                        if (_mti.m_currentTsmedoStep == 1 && _mti.m_rateOfReturn >= _mti.m_tsMedoArchievedPer[i]) // 목표가와 수익률이 동일해졋을때,
                                                        {
                                                            if (closingPrice > _goJumMedo.highPrice[i])
                                                            {
                                                                _goJumMedo.highPrice[i] = closingPrice;
                                                                if (!_mti._bSecondTsmedologCheck)
                                                                {
                                                                    gMainForm.setLogText("종목명: " + itemName + " 2차 목표가ts도달 - " + _mti.m_tsMedoArchievedPer[i] + " % " + " 현재수익률: " + _mti.m_rateOfReturn.ToString("N2") + "%" + " 현재가격: " + _mti.m_currentPrice);
                                                                    _mti._bSecondTsmedologCheck = true;
                                                                }
                                                                //Console.WriteLine("2차 목표가ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);
                                                            }
                                                        }
                                                        if (_mti.m_currentTsmedoStep == 2 && _mti.m_rateOfReturn >= _mti.m_tsMedoArchievedPer[i]) // 목표가와 수익률이 동일해졋을때,
                                                        {
                                                            if (closingPrice > _goJumMedo.highPrice[i])
                                                            {
                                                                _goJumMedo.highPrice[i] = closingPrice;
                                                                if (!_mti._bThirdTsmedologCheck)
                                                                {
                                                                    gMainForm.setLogText("종목명: " + itemName + " 3차 목표가ts도달 - " + _mti.m_tsMedoArchievedPer[i] + " % " + " 현재수익률: " + _mti.m_rateOfReturn.ToString("N2") + "%" + " 현재가격: " + _mti.m_currentPrice);
                                                                    _mti._bThirdTsmedologCheck = true;
                                                                }
                                                                //Console.WriteLine("3차 목표가ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);
                                                            }
                                                        }
                                                        double dropPercent = _mtc.tsMedoPercent[i] / 100;
                                                        double dropPrice = _goJumMedo.highPrice[i] * (1 - dropPercent);
                                                        //Console.WriteLine("종목명: " + itemName + " 목표가ts사용중" + " 현재가: " + closingPrice + " 고점가격: " + _goJumMedo.highPrice + " 하락률: " + dropPercent + " 고점대비 하락가격: " + dropPrice);
                                                        if(_mti.m_tsProfitPreservation1 == 1 && _mti.m_currentTsmedoStep == 0)
                                                        {
                                                            if(closingPrice <= dropPrice && _mti.m_rateOfReturn > 0 )
                                                            {
                                                                //Console.WriteLine("1차 목표가 ts 이익구간작동");
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber = i;
                                                                _str = "목표가ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation1 == 0 && _mti.m_currentTsmedoStep == 0)
                                                        {
                                                            if (closingPrice <= dropPrice)
                                                            {
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber = i;
                                                                _str = "목표가ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation2 == 1 && _mti.m_currentTsmedoStep == 1)
                                                        {
                                                            if (closingPrice <= dropPrice && _mti.m_rateOfReturn > 0)
                                                            {
                                                                //Console.WriteLine("2차 목표가 ts 이익구간작동");
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber = i;
                                                                _str = "목표가ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation2 == 0 && _mti.m_currentTsmedoStep == 1)
                                                        {
                                                            if (closingPrice <= dropPrice)
                                                            {
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber = i;
                                                                _str = "목표가ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation3 == 1 && _mti.m_currentTsmedoStep == 2)
                                                        {
                                                            if (closingPrice <= dropPrice && _mti.m_rateOfReturn > 0)
                                                            {
                                                                //Console.WriteLine("3차 목표가 ts 이익구간작동");
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber = i;
                                                                _str = "목표가ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation3 == 0 && _mti.m_currentTsmedoStep == 2)
                                                        {
                                                            if (closingPrice <= dropPrice)
                                                            {
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber = i;
                                                                _str = "목표가ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (_mti.m_tsMedoUsingType[i] == 1) //고점ts
                                                    {
                                                        if (_mti.m_currentTsmedoStep == 0 && closingPrice > _goJumMedo.highPrice[i] && closingPrice >= _gmd.purchasePrice)
                                                        {
                                                            _goJumMedo.highPrice[i] = closingPrice;
                                                            //Console.WriteLine("1차 고점ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);
                                                        }
                                                        // 현재가가 고점보다 높을경우 고점가격에 대한 갱신
                                                        if (_mti.m_currentTsmedoStep == 1 && _mti.m_tsMedoArray[_mti.m_currentTsmedoStep - 1] && closingPrice > _goJumMedo.highPrice[i])
                                                        {
                                                            _goJumMedo.highPrice[i] = closingPrice;
                                                            //Console.WriteLine("2차 고점ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);

                                                        }
                                                        if (_mti.m_currentTsmedoStep == 2 && _mti.m_tsMedoArray[_mti.m_currentTsmedoStep - 1] && closingPrice > _goJumMedo.highPrice[i])
                                                        {
                                                            _goJumMedo.highPrice[i] = closingPrice;
                                                            //Console.WriteLine("3차 고점ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);

                                                        }
                                                        double dropPercent = _mtc.tsMedoPercent[i] / 100;
                                                        double dropPrice = _goJumMedo.highPrice[i] * (1 - dropPercent);
                                                        //Console.WriteLine("종목명: " + itemName + " 고점ts사용중"  +" 현재가: " + closingPrice + " 고점가격: " + _goJumMedo.highPrice + " 하락률: " + dropPercent + " 고점대비 하락가격: " + dropPrice);
                                                        if (_mti.m_tsProfitPreservation1 == 1 && _mti.m_currentTsmedoStep == 0)
                                                        {
                                                            if (closingPrice <= dropPrice && _mti.m_rateOfReturn > 0)
                                                            {
                                                                //Console.WriteLine("1차 고점 ts 이익구간작동");
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber = i;
                                                                _str = "고점ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if(_mti.m_tsProfitPreservation1 == 0 && _mti.m_currentTsmedoStep == 0)
                                                        {
                                                            if (closingPrice <= dropPrice)
                                                            {
                                                                //Console.WriteLine("1차 고점ts 이익구간작동안함");
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber = i;
                                                                _str = "고점ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation2 == 1 && _mti.m_currentTsmedoStep == 1)
                                                        {
                                                            if (closingPrice <= dropPrice && _mti.m_rateOfReturn > 0)
                                                            {
                                                                //Console.WriteLine("2차 고점 ts 이익구간작동");
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber = i;
                                                                _str = "고점ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation2 == 0 && _mti.m_currentTsmedoStep == 1)
                                                        {
                                                            if (closingPrice <= dropPrice)
                                                            {
                                                                //Console.WriteLine("2차 고점ts 이익구간작동안함");
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber = i;
                                                                _str = "고점ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation3 == 1 && _mti.m_currentTsmedoStep == 2)
                                                        {
                                                            if (closingPrice <= dropPrice && _mti.m_rateOfReturn > 0)
                                                            {
                                                                //Console.WriteLine("3차 고점 ts 이익구간작동");
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber = i;
                                                                _str = "고점ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation3 == 0 && _mti.m_currentTsmedoStep == 2)
                                                        {
                                                            if (closingPrice <= dropPrice)
                                                            {
                                                                //Console.WriteLine("3차 고점ts 이익구간작동안함");
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber = i;
                                                                _str = "고점ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }

                                                    }
                                                }
                                                if (_bTsmedoCheck && !_mti.m_isMedoOrderPlaced) //매도주문 코딩
                                                {
                                                    if(_arrayNumber ==0)
                                                    {
                                                        _mti.m_isMedoOrderPlaced = true;
                                                        _mti.m_bSold = true;
                                                        _mti.m_tsMedoArray[_arrayNumber] = true;
                                                        _mti.m_tsMedoNumber = _arrayNumber;

                                                        // 정해진 비중만큼 계산을 해야한다. 이때 배열이있으니깐 for문을 돌려야할듯?
                                                        //Console.WriteLine("_arrayNumber: " + _arrayNumber);
                                                        double highRateOfReturn = GetRateOfReturnAtHighPrice(_mti, _mti.m_averagePrice, _mti.m_totalQnt, _goJumMedo.highPrice[_arrayNumber]);
                                                        double calculated = _mti.m_totalQnt * _mti.m_tsMedoProportion[_arrayNumber] / 100.0;
                                                        int _qnt = (calculated > 0 && calculated < 1) ? 1 : (int)Math.Round(calculated, MidpointRounding.AwayFromZero);
                                                        gMainForm.setLogText(itemName + " 1차 ts매도수량계산: " + _qnt + " 고점가격: " + _goJumMedo.highPrice[_arrayNumber] + " 고점수익률: " + highRateOfReturn.ToString("N2") +"%" +" 주문가격: " + closingPrice + " 수익률: " + _mti.m_rateOfReturn.ToString("N2") + "%");
                                                        Task _tsMedoTask = new Task(() =>
                                                        {


                                                            long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                                    "ts매도",                  // 사용자구분명
                                                                                                    gMainForm.GetScreenNumber(), // 화면번호
                                                                                                    _mtc.account,                // 계좌번호 10자리
                                                                                                    2,                           // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                                    itemCode,                    // 종목코드 6자리
                                                                                                    _qnt,                        // 주문수량
                                                                                                    0,                           // 주문가격 : 시장가일때는 0이다.
                                                                                                    "03",                        // 거래구분 : 시장가는 "03"
                                                                                                    "");                         // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                        if (_ret == 0) // ts매도성공
                                                        {
                                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                            {
                                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) &&
                                                                    row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                                {
                                                                    row.Cells["매매진행_진행상황"].Value = _arrayNumber + 1 + "차ts매도";
                                                                    break;
                                                                }
                                                            }
                                                            gMainForm.setLogText(_arrayNumber + 1 + "차ts 매도 성공: " + itemName);

                                                            _mti.m_currentTsmedoStep++;
                                                            //Console.WriteLine("현재 ts매도 차수: " + _mti.m_currentTsmedoStep);
                                                        }
                                                        else
                                                        {
                                                            gMainForm.setLogText(_arrayNumber + 1 + "차ts 매도 실패: " + itemName);
                                                        }
                                                        });
                                                        gMainForm.gOrderRequestManager.sendTaskData(_tsMedoTask);
                                                    }
                                                    if(_arrayNumber >0)
                                                    {
                                                        _mti.m_isMedoOrderPlaced = true;
                                                        _mti.m_bSold = true;
                                                        _mti.m_tsMedoArray[_arrayNumber] = true;
                                                        _mti.m_tsMedoNumber = _arrayNumber;

                                                        // 정해진 비중만큼 계산을 해야한다. 이때 배열이있으니깐 for문을 돌려야할듯?
                                                        //Console.WriteLine("_arrayNumber: " + _arrayNumber);
                                                        double calculated = _mti.m_totalQnt * _mti.m_tsMedoProportion[_arrayNumber] / 100.0;
                                                        int _qnt = (calculated > 0 && calculated < 1) ? 1 : (int)Math.Round(calculated, MidpointRounding.AwayFromZero);
                                                        double highRateOfReturn = GetRateOfReturnAtHighPrice(_mti, _mti.m_averagePrice, _mti.m_totalQnt, _goJumMedo.highPrice[_arrayNumber]);
                                                        int arraynumber2 = _arrayNumber + 1;
                                                        gMainForm.setLogText(itemName+ " " + arraynumber2 + " ts매도수량계산: " + _qnt + " 고점가격: " + _goJumMedo.highPrice[_arrayNumber] + " 고점수익률: " + highRateOfReturn.ToString("N2") + "%" + " 주문가격: " + closingPrice + " 수익률: " + _mti.m_rateOfReturn.ToString("N2")+ "%");
                                                        Task _tsMedoTask = new Task(() =>
                                                        {
                                                            long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                                    "ts매도",                  // 사용자구분명
                                                                                                    gMainForm.GetScreenNumber(), // 화면번호
                                                                                                    _mtc.account,                // 계좌번호 10자리
                                                                                                    2,                           // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                                    itemCode,                    // 종목코드 6자리
                                                                                                    _qnt,                        // 주문수량
                                                                                                    0,                           // 주문가격 : 시장가일때는 0이다.
                                                                                                    "03",                        // 거래구분 : 시장가는 "03"
                                                                                                    "");                         // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                        if (_ret == 0) // ts매도성공
                                                        {
                                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                            {
                                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) &&
                                                                    row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                                {
                                                                    row.Cells["매매진행_진행상황"].Value = _arrayNumber + 1 + "차ts매도";
                                                                    break;
                                                                }
                                                            }
                                                            gMainForm.setLogText(_arrayNumber + 1 + "차ts 매도 성공: " + itemName);

                                                            _mti.m_currentTsmedoStep++;
                                                            //Console.WriteLine("현재 ts매도 차수: " + _mti.m_currentTsmedoStep);
                                                        }
                                                        else
                                                        {
                                                            gMainForm.setLogText(_arrayNumber + 1 + "차ts 매도 실패: " + itemName);
                                                        }
                                                        });
                                                        gMainForm.gOrderRequestManager.sendTaskData(_tsMedoTask);
                                                    }

                                                }
                                                /*
                                                // 현재가가 고점보다 높을경우 고점가격에 대한 갱신
                                                if (closingPrice > _goJumMedo.highPrice && closingPrice > _gmd.purchasePrice)
                                                {
                                                    _goJumMedo.highPrice = closingPrice;
                                                    //Console.WriteLine("시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice);
                                                }
                                                //double dropPercent = _mtc.tsMedoPercent[0] / 100;
                                                //double dropPrice = _goJumMedo.highPrice * (1 - dropPercent);
                                                //Console.WriteLine("고점가격: " + _goJumMedo.highPrice + " 하락률: " + dropPercent + " 고점대비 하락가격: " + dropPrice);
                                                if (closingPrice <= _goJumMedo.highPrice * (1 - dropPercent))
                                                {
                                                    int _qnt = _mti.m_totalQnt; //보유수량
                                                    long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                    "ts매도",                  // 사용자구분명
                                                                                    gMainForm.GetScreenNumber(), // 화면번호
                                                                                    _mtc.account,                // 계좌번호 10자리
                                                                                    2,                           // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                    itemCode,                    // 종목코드 6자리
                                                                                    _qnt,                        // 주문수량
                                                                                    0,                           // 주문가격 : 시장가일때는 0이다.
                                                                                    "03",                        // 거래구분 : 시장가는 "03"
                                                                                    "");                         // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                    if (_ret == 0) // ts매도성공
                                                    {
                                                        _mti.m_bSold = true;
                                                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                        {
                                                            if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) &&
                                                                row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                            {
                                                                row.Cells["매매진행_진행상황"].Value = "ts매도중";
                                                                break;
                                                            }
                                                        }
                                                        gMainForm.setLogText("ts 매도 성공: " + itemCode);
                                                    }
                                                    else
                                                    {
                                                        gMainForm.setLogText("ts 매도 실패: " + itemCode);
                                                    }
                                                }*/
                                            }
                                            else
                                            {
                                                // _goJumMedo가 null일 경우 로그를 남기거나 적절한 조치를 취합니다.
                                                //Console.WriteLine("매수체결이 완료되지않음.");
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        break;
                    }
                     // 거래량 보정을 위한 계산
                     // 1분봉 데이터를 받는 동안 발생하는 거래량을 배열2개(1분간격으로)로 처리를 해놓는다.
                    if (_mti.m_volumeState == (int)trading.doing) // m_volumeState의 기본값은 (int)trading.wait임
                    {
                        string _time = tradingTime.Substring(0, 4); // 실시간 체결되는 시간값을 받아옴 (시간 분)
                        //Console.WriteLine("_time: " + _time + " _mti.m_tradingTime[0]: " + _mti.m_tradingTime[0]);
                        if(_mti.m_tradingTime[0] == string.Empty) // 최초 시작시 시간, 거래량[0]번에 저장
                        {
                            _mti.m_tradingTime[0] = _time;
                            _mti.m_tradeVolume[0] += volume;
                            _mti.m_tradeHighPrice[0] = closingPrice;
                            _mti.m_tradeLowPrice[0] = closingPrice;
                            //Console.WriteLine("_mti.m_tradingTime[0]: " + _mti.m_tradingTime[0] + " _mti.m_tradeVolume[0]: " + _mti.m_tradeVolume[0]);
                        }
                        else
                        {
                            if(_mti.m_tradingTime[0].Equals(_time)) //0번시간과 동일한 경우 거래량은 누적
                            {
                                _mti.m_tradeVolume[0] += volume;
                                if (_mti.m_tradeHighPrice[0] < closingPrice) // 고가계산
                                    _mti.m_tradeHighPrice[0] = closingPrice;
                                if (_mti.m_tradeLowPrice[0] > closingPrice) //저가 계산
                                    _mti.m_tradeLowPrice[0] = closingPrice;
                                //Console.WriteLine("equaltime _mti.m_tradingTime[0]: " + _mti.m_tradingTime[0] + " _mti.m_tradeVolume[0]: " + _mti.m_tradeVolume[0]);
                            }
                            else // [0] 시간과 다른 경우
                            {
                                if(_mti.m_tradingTime[1] == string.Empty) // 시간, 거래량 [1]번에 저장
                                {
                                    _mti.m_tradingTime[1] = _time;
                                    _mti.m_tradeVolume[1] += volume;
                                    _mti.m_tradeHighPrice[1] = closingPrice;
                                    _mti.m_tradeLowPrice[1] = closingPrice;
                                    //Console.WriteLine("_mti.m_tradingTime[1]: " + _mti.m_tradingTime[1] + " _mti.m_tradeVolume[1]: " + _mti.m_tradeVolume[1]);
                                }
                                else //[1번]시간과 동일한 경우 거래량 누적
                                {
                                    _mti.m_tradeVolume[1] += volume;
                                    if (_mti.m_tradeHighPrice[1] < closingPrice) // 고가계산
                                        _mti.m_tradeHighPrice[1] = closingPrice;
                                    if (_mti.m_tradeLowPrice[1] > closingPrice) //저가 계산
                                        _mti.m_tradeLowPrice[1] = closingPrice;
                                    //Console.WriteLine("_mti.m_tradingTime[1]: " + _mti.m_tradingTime[1] + " _mti.m_tradeVolume[1]: " + _mti.m_tradeVolume[1]);
                                }
                            }
                        }
                    }
                    if (_mti.m_volumeState == (int)trading.stop)
                    {

                        //Console.WriteLine("m_bGetBunBongSuccess : " + _mti.m_bGetBunBongSuccess + " m_bGetDayBongSuccess: " + _mti.m_bGetDayBongSuccess);
                        //1분봉 데이터와 일봉 데이터를 다 가져오기전에는 다른 계산을 하지 않는다.
                        if (!_mti.m_bGetBunBongSuccess && !_mti.m_bGetDayBongSuccess)
                            return;

                        // 실시간 분봉 및 일봉 계산
                        string _tradingtime = tradingTime.Substring(0, 4); //체결시간
                        _mti.setBunBongRealCalculate(closingPrice, _tradingtime, volume); //분봉계산
                        _mti.setDayBongRealCalculate(closingPrice, volume); // 일봉계산
                        _mti.CalculateIndicator();
                        DrawIndicateData(gMainForm.g_selectItemCode);
                        // 현재가
                        _mti.m_currentPrice = closingPrice;
                        //종목 그리드뷰에 현재가를 넣는 메소드
                        RealRateOfReturnAndEvaluationProfitLoss(_mti, closingPrice, upDownRate);

                        // 보유종목 수익률, 총매입, 총평가, 총손익 계산
                        TotalRateOfReturnAndEvaluationProfitLoss();

                        // 현재가
                        _mti.m_currentPrice = closingPrice;

                        if (!_mti.m_bCompletePurchase) // 매수가 완료되지 않았다면
                        {
                            // m_bPurchased가 false일때
                            if (!_mti.m_bPurchased)
                            {
                                if (_mti.m_buyingUsing == 1) // 매수 사용 - 1 , 매수 사용안함 - 0 
                                {
                                    // 매수 변수
                                    bool _bPurchaseCheck = false;
                                    bool _bReboundChcek = false;
                                    string _str = "";
                                    
                                    if (_mti.m_buyingType == 0) ////////////////////////// 기본매수
                                    {
                                        if (_mti.m_buyingTransferType == 1) // 편입대비 -n% 매수
                                        {
                                            double _perPrice = getPercentPrice(_mti.m_transferPrice, _mti.m_buyingTransferPer);
                                            
                                            if (_mti.m_buyingTransferUpdown == 0) // n%이하
                                            {
                                                if (_mti.m_currentPrice < _perPrice)
                                                    _bPurchaseCheck = true;
                                            }
                                            else if (_mti.m_buyingTransferUpdown == 1) // n%이상
                                            {
                                                if (_mti.m_currentPrice > _perPrice)
                                                    _bPurchaseCheck = true;
                                            }
                                            if (_bPurchaseCheck && _mtc.remainingBuyingItemCount > 0)
                                            {
                                                _str = "편입 대비 매수 : " + itemName;
                                                gMainForm.setLogText(_str);
                                            }
                                        }
                                        if(_mti.m_buyingTransferType == 2) //저점 00% 도달후 00% 반등시 매수
                                        {
                                            double profitRate = ((closingPrice - _mti.m_transferPrice) / _mti.m_transferPrice) * 100; // 편입가와 현재가를 비교한 수익률
                                            double _perPrice = getPercentPrice(_mti.m_transferPrice, _mti.m_buyingTransferPer2); // 목표저점가
                                            
                                            if(_mti.m_lowlowprice == 0)
                                            {
                                                _mti.m_lowlowprice = _perPrice; // 저점 가격을 처음에는 편입가로 계산한 기준치 저점
                                            }

                                            if (profitRate < _mti.m_buyingTransferPer2) // 수익률이 지정한 저점%보다 낮아질경우
                                            {
                                                //Console.WriteLine("종목명: " + itemName + " 목표저점가격: " + _perPrice + " 현재가: " + closingPrice);

                                                if (closingPrice < _mti.m_lowlowprice)
                                                {
                                                    _mti.m_lowlowprice = closingPrice;//저점갱신 = closingprice , 현재가를 저점으로 지정한다.
                                                    gMainForm.setLogText(" 종목명: " + itemName + " 목표저점가: " + _perPrice +  " 계산된저점가격: " + _mti.m_lowlowprice);
                                                }
                                                _bReboundChcek = true;
                                            }

                                            double reboundPrice = _mti.m_lowlowprice * (1 + _mti.m_riseTransferPer2 / 100);

                                            if (_mti.m_currentPrice >= reboundPrice && _bReboundChcek)
                                            {
                                                _bPurchaseCheck = true;
                                                gMainForm.setLogText("종목명: " + itemName + " 저점후 반등시 매수주문들어가는가격: " + reboundPrice);
                                            }

                                            if (_bPurchaseCheck && _mtc.remainingBuyingItemCount > 0)
                                            {
                                                _str = "저점 대비 매수 : " + itemName;
                                                gMainForm.setLogText(_str);
                                            }
                                        }
                                    }
                                    else if (_mti.m_buyingType == 1) //////////////////////// 이동평균선 매수
                                    {
                                        // moving_PriceDayCur[0] 현재 일봉 매수이평선 가겨
                                        // moving_PriceCur[0] 현재 분봉 매수이평선 가격
                                        // m_buyingBongType 일봉인지 분봉인지                                                
                                        // m_buyingMinuteLineAccessPer n%
                                        // m_buyingDistance 0:근접 1:돌파
                                        double _standardPrice = 0;
                                        if (_mti.m_buyingBongType == 0) // 일봉
                                        {
                                            if (_mti.m_buyingMovePriceKindType == 0) // 단순
                                                _standardPrice = _mti.moving_PriceDayCur[0];
                                            else // 지수.
                                                _standardPrice = _mti.moving_PriceDayCurE[0];
                                        }
                                        else if (_mti.m_buyingBongType == 1) // 분봉
                                        {
                                            if (_mti.m_buyingMovePriceKindType == 0) // 단순
                                                _standardPrice = _mti.moving_PriceCur[0];
                                            else // 지수
                                                _standardPrice = _mti.moving_PriceCurE[0];
                                        }

                                        if (_mti.m_buyingDistance == 0) // 근접
                                        {
                                            // 기준가격 위 아래 계산해서 그 사이에 들어왔을때 매수
                                            double _perUpPrice = getPercentPrice(_standardPrice, _mti.m_buyingMinuteLineAccessPer);
                                            double _perDownPrice = getPercentPrice(_standardPrice, _mti.m_buyingMinuteLineAccessPer * -1);
                                            if (_perUpPrice >= _mti.m_currentPrice && _mti.m_currentPrice >= _perDownPrice)
                                            {
                                                _bPurchaseCheck = true;
                                            }
                                        }
                                        else if (_mti.m_buyingDistance == 1) // 돌파
                                        {
                                            if (_mti.m_currentPrice > _standardPrice)
                                            {
                                                _bPurchaseCheck = true;
                                            }
                                        }
                                        if (_bPurchaseCheck)
                                        {
                                            _str = "이동평균선 매수 : " + itemName;
                                            gMainForm.setLogText(_str);
                                        }
                                    }
                                    else if (_mti.m_buyingType == 2) //////////////////////// 스토캐스틱 슬로우 매수
                                    {
                                        // stochastics_KPriceDayCur[0] 현재 일봉 k값
                                        // stochastics_DPriceDayCur[0] 현재 일봉 d값
                                        // stochastics_KPriceCur[0] 현재 분봉 k값
                                        // stochastics_DPriceCur[0] 현재 분봉 d값
                                        // m_buyingBongType 일봉인지 분봉인지                                                
                                        // m_buyingMinuteLineAccessPer K값
                                        // m_buyingDistance 0:이상 1:이하
                                        double _standardPriceK = 0;
                                        double _standardPriceD = 0;
                                        if (_mti.m_buyingBongType == 0) // 일봉
                                        {
                                            _standardPriceK = _mti.stochastics_KPriceDayCur[0];
                                            _standardPriceD = _mti.stochastics_DPriceDayCur[0];
                                        }
                                        else if (_mti.m_buyingBongType == 1) // 분봉
                                        {
                                            _standardPriceK = _mti.stochastics_KPriceCur[0];
                                            _standardPriceD = _mti.stochastics_DPriceCur[0];
                                        }

                                        if (_mti.m_buyingDistance == 0) // k값 이상
                                        {
                                            if (_standardPriceK > _mti.m_buyingMinuteLineAccessPer)
                                            {
                                                _bPurchaseCheck = true;
                                            }
                                        }
                                        else if (_mti.m_buyingDistance == 1) // k값 이하
                                        {
                                            if (_standardPriceK < _mti.m_buyingMinuteLineAccessPer)
                                            {
                                                _bPurchaseCheck = true;
                                            }
                                        }
                                        if (_bPurchaseCheck)
                                        {
                                            _str = "스토캐스틱 매수 : " + itemName;
                                            gMainForm.setLogText(_str);
                                        }
                                    }
                                    else if (_mti.m_buyingType == 3) //////////////////////// 볼린저밴드 매수
                                    {
                                        // bollinger_upPriceDayCur[0] 상한선값
                                        // bollinger_centerPriceDayCur[0] 중심선 값
                                        // bollinger_downPriceDayCur[0] 하한선 값
                                        // bollinger_upPriceCur[0] 상한선 값
                                        // bollinger_centerPriceCur[0] 중심선 값
                                        // bollinger_downPriceCur[0] 하한선 값
                                        // m_buyingBongType 일봉인지 분봉인지
                                        // m_buyingLine3Type 상,중,하선
                                        // m_buyingMinuteLineAccessPer n%
                                        // m_buyingDistance 0:근접, 1:돌파, 2:이탈
                                        double _standardPrice = 0;
                                        if (_mti.m_buyingBongType == 0) // 일봉
                                        {
                                            if (_mti.m_buyingLine3Type == 0) // 상
                                                _standardPrice = _mti.bollinger_upPriceDayCur[0];
                                            else if (_mti.m_buyingLine3Type == 1) // 중
                                                _standardPrice = _mti.bollinger_centerPriceDayCur[0];
                                            else if (_mti.m_buyingLine3Type == 2) // 하
                                                _standardPrice = _mti.bollinger_downPriceDayCur[0];
                                        }
                                        else if (_mti.m_buyingBongType == 1) // 분봉
                                        {
                                            if (_mti.m_buyingLine3Type == 0) // 상
                                                _standardPrice = _mti.bollinger_upPriceCur[0];
                                            else if (_mti.m_buyingLine3Type == 1) // 중
                                                _standardPrice = _mti.bollinger_centerPriceCur[0];
                                            else if (_mti.m_buyingLine3Type == 2) // 하
                                                _standardPrice = _mti.bollinger_downPriceCur[0];
                                        }
                                        if (_mti.m_buyingDistance == 0) // 근접
                                        {
                                            // 기준가격 위 아래 계산해서 그 사이에 들어왔을때 매수
                                            double _perUpPrice = getPercentPrice(_standardPrice, _mti.m_buyingMinuteLineAccessPer);
                                            double _perDownPrice = getPercentPrice(_standardPrice, _mti.m_buyingMinuteLineAccessPer * -1);
                                            if (_perUpPrice >= _mti.m_currentPrice && _mti.m_currentPrice >= _perDownPrice)
                                            {
                                                _bPurchaseCheck = true;
                                            }
                                        }
                                        else if (_mti.m_buyingDistance == 1) // 돌파
                                        {
                                            if (_mti.m_currentPrice > _standardPrice)
                                            {
                                                _bPurchaseCheck = true;
                                            }
                                        }
                                        else if (_mti.m_buyingDistance == 2) // 이탈
                                        {
                                            if (_mti.m_currentPrice < _standardPrice)
                                            {
                                                _bPurchaseCheck = true;
                                            }
                                        }
                                        if (_bPurchaseCheck)
                                        {
                                            _str = "볼린저밴드 매수 : " + itemName;
                                            gMainForm.setLogText(_str);
                                        }
                                    }
                                    else if (_mti.m_buyingType == 4) //////////////////////// 엔벨로프 매수
                                    {
                                        // envelope_upPriceDayCur[0] 상한선 값
                                        // envelope_centerPriceDayCur[0] 중심선 값
                                        // envelope_downPriceDayCur[0] 하한선 값
                                        // envelope_upPriceCur[0] 상한선 값
                                        // envelope_centerPriceCur[0] 중심선 값
                                        // envelope_downPriceCur[0] 하한선 값
                                        // m_buyingBongType 일봉인지 분봉인지                  
                                        // m_buyingLine3Type 상,중,하선
                                        // m_buyingMinuteLineAccessPer n%
                                        // m_buyingDistance 0:근접, 1:돌파, 2:이탈
                                        double _standardPrice = 0;
                                        if (_mti.m_buyingBongType == 0) // 일봉
                                        {
                                            if (_mti.m_buyingLine3Type == 0) // 상
                                                _standardPrice = _mti.envelope_upPriceDayCur[0];
                                            else if (_mti.m_buyingLine3Type == 1) // 중
                                                _standardPrice = _mti.envelope_centerPriceDayCur[0];
                                            else if (_mti.m_buyingLine3Type == 2) // 하
                                                _standardPrice = _mti.envelope_downPriceDayCur[0];
                                        }
                                        else if (_mti.m_buyingBongType == 1) // 분봉
                                        {
                                            if (_mti.m_buyingLine3Type == 0) // 상
                                                _standardPrice = _mti.envelope_upPriceCur[0];
                                            else if (_mti.m_buyingLine3Type == 1) // 중
                                                _standardPrice = _mti.envelope_centerPriceCur[0];
                                            else if (_mti.m_buyingLine3Type == 2) // 하
                                                _standardPrice = _mti.envelope_downPriceCur[0];
                                        }
                                        if (_mti.m_buyingDistance == 0) // 근접
                                        {
                                            // 기준가격 위 아래 계산해서 그 사이에 들어왔을때 매수
                                            double _perUpPrice = getPercentPrice(_standardPrice, _mti.m_buyingMinuteLineAccessPer);
                                            double _perDownPrice = getPercentPrice(_standardPrice, _mti.m_buyingMinuteLineAccessPer * -1);
                                            if (_perUpPrice >= _mti.m_currentPrice && _mti.m_currentPrice >= _perDownPrice)
                                            {
                                                _bPurchaseCheck = true;
                                            }
                                        }
                                        else if (_mti.m_buyingDistance == 1) // 돌파
                                        {
                                            if (_mti.m_currentPrice > _standardPrice)
                                            {
                                                _bPurchaseCheck = true;
                                            }
                                        }
                                        else if (_mti.m_buyingDistance == 2) // 이탈
                                        {
                                            if (_mti.m_currentPrice < _standardPrice)
                                            {
                                                _bPurchaseCheck = true;
                                            }
                                        }
                                        if (_bPurchaseCheck)
                                        {
                                            _str = "엔벨로프 매수 : " + itemName;
                                            gMainForm.setLogText(_str);
                                        }
                                    }

                                    // 매수를 한다.
                                    if (_bPurchaseCheck && _mtc.remainingBuyingItemCount > 0)
                                    {
                                        // 예수금 부족 확인
                                        if (gMainForm.curOrderAmount < _mti.m_buyingPerInvestment[0])
                                        {
                                            gMainForm.setLogText("예수금이 부족하여 주문을 할 수 없습니다.");
                                            return;
                                        }
                                        //Console.WriteLine(" 주문가격: " + _mti.m_currentPrice);
                                        if (_mtc.mesuoption1 == 0) // 시장가
                                        {
                                            Console.WriteLine(itemName + " 편입대비매수 시장가매수 " + _mti.m_volumeState);
                                            // 주문수량계산 : 시장가의 경우 상한가를 기준으로 수량을 계산한다.
                                            int _qnt = (int)(_mti.m_buyingPerInvestment[0] / _mti.m_upperLimitPrice);
                                            long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                        "종목매수",                                // 사용자 구분명
                                                                        gMainForm.GetScreenNumber(),   // 화면번호
                                                                        _mtc.account,                                // 계좌번호 10자리
                                                                        1,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                        itemCode,                                     // 종목코드 6자리                                                        
                                                                        _qnt,                                              // 주문수량
                                                                        0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                        "03", // 시장가                                  // 거래구분
                                                                        "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                            if (_ret == 0) // 성공
                                            {
                                                //Console.WriteLine(itemName +" 기타 시장가 매수주문 성공");
                                                // 더이상 매수가 안되도록 _mti.m_bPurchased를 true로 설정한다.
                                                _mti.m_rePurchaseArray[0] = true;
                                                _mti.m_bPurchased = true;

                                                foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                {
                                                    if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                    {
                                                        row.Cells["매매진행_진행상황"].Value = "매수중";
                                                        _mtc.remainingBuyingItemCount--;
                                                        //Console.WriteLine("주문성공 남은매수갯수: " + _mtc.remainingBuyingItemCount);
                                                        break;
                                                    }
                                                }
                                                gMainForm.setLogText("종목 매수 성공 : " + itemName);
                                            }
                                            else // 실패
                                            {
                                                gMainForm.setLogText("종목 매수 실패 : " + itemName);
                                            }
                                        }
                                        if (_mtc.mesuoption1 == 1) // 지정가(현재가)
                                        {
                                            Console.WriteLine(itemName + " 편입대비매수 지정가매수 " + _mti.m_volumeState); // 해당 state가 일정하게 일단 stop이어야함
                                            // 주문수량계산 : 시장가의 경우 상한가를 기준으로 수량을 계산한다.
                                            int _qnt1 = (int)(_mti.m_buyingPerInvestment[0] / closingPrice); // 종목당투자금 / 상한가 = 주문수량
                                            long _ret1 = gMainForm.KiwoomAPI.SendOrder(
                                                                                    "편입종목지정가매수", // 사용자 구분명
                                                                                    gMainForm.GetScreenNumber(), // 화면번호
                                                                                    _mtc.account, // 계좌번호 10자리
                                                                                    1, // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                    itemCode, // 종목코드 6자리
                                                                                    _qnt1, // 주문수량
                                                                                    (int)closingPrice, // 주문가격: 시장가의 경우 0
                                                                                    "00", // 거래구분 / 03은 시장가
                                                                                    "" // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                                                       );
                                            if (_ret1 == 0) //성공
                                            {
                                                //Console.WriteLine(itemName + " 기타 지정가 매수주문 성공");
                                                // 더이상 매수가 안되도록 _mti.m_bPurchased를 true로 설정한다.
                                                _mti.m_rePurchaseArray[0] = true;
                                                _mti.m_bPurchased = true;

                                                foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                {
                                                    if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) &&
                                                        row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                    {
                                                        row.Cells["매매진행_진행상황"].Value = "매수중";
                                                        //매수주문성공시 개수카운트차감
                                                        _mtc.remainingBuyingItemCount--;
                                                        //Console.WriteLine("주문성공 남은매수갯수: " + _mtc.remainingBuyingItemCount);
                                                        break;
                                                    }
                                                }
                                                gMainForm.setLogText("편입종목 현재가 매수성공: " + itemName);
                                            }
                                            else // 실패
                                            {
                                                gMainForm.setLogText("편입종목 매수실패: " + itemName);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else // 매수가 완료되었으면
                        {
                            if (!_mti.m_bSold && _mti.m_rePurchaseNumber == -1 && _mti.m_upperLimitPrice > 0) // 매도가 되지 않았을때 익절, 손절을 체크한다.
                            {
                                /////////////////////////////////////////////////////////// 추매 ////////////////////////////////////////////////////////
                                ///// 추가매수 변수
                                bool _bRepurchaseCheck = false;
                                string _str = "";
                                int _arrayNumber = -1;

                                // 5번 추매를 확인한다.
                                for (int i = 0; i < 5; i++)
                                {
                                    if (_mti.m_rePurchaseArray[i + 1]) // 추매되었다면...
                                        continue;

                                    if (_mti.m_buyingPerInvestment[i + 1] == 0) // 추매금액이 0이면...
                                        continue;

                                    if (_mti.m_reBuyingType == 0) ////////////////////////////////////////// 일반추매
                                    {
                                        if (_mti.m_rateOfReturn <= _mti.m_reBuyingPer[i])
                                        {
                                            _bRepurchaseCheck = true;
                                            _arrayNumber = i + 1;
                                            _str = "일반 추가 매수 : " + itemName;
                                            gMainForm.setLogText(_str);
                                            break;
                                        }
                                    }
                                    else if (_mti.m_reBuyingType == 1) ////////////////////////////////////////// 이동평균선 추매
                                    {
                                        // m_reBuyingBongType 일봉, 분봉
                                        // reBuyingMoving_PriceDayCur[_arrayNum] 일봉지수이평가격
                                        // reBuyingMoving_PriceCur[_arrayNum] 분봉지수이평가격
                                        // m_reBuyingPer[] n% 근접
                                        double _standardPrice = 0;
                                        if (_mti.m_reBuyingBongType == 0) // 일봉
                                        {
                                            _standardPrice = _mti.reBuyingMoving_PriceDayCur[i];
                                        }
                                        else if (_mti.m_reBuyingBongType == 1) // 분봉
                                        {
                                            _standardPrice = _mti.reBuyingMoving_PriceCur[i];
                                        }

                                        double _perUpPrice = getPercentPrice(_standardPrice, _mti.m_reBuyingPer[i]);
                                        double _perDownPrice = getPercentPrice(_standardPrice, _mti.m_reBuyingPer[i] * -1);
                                        if (_perUpPrice >= _mti.m_currentPrice && _mti.m_currentPrice >= _perDownPrice)
                                        {
                                            _bRepurchaseCheck = true;
                                            _arrayNumber = i + 1;
                                            _str = "일반 추가 매수 : " + itemName;
                                            gMainForm.setLogText(_str);
                                            break;
                                        }
                                    }
                                }

                                if (_bRepurchaseCheck)
                                {
                                    // 예수금 부족 확인
                                    if (gMainForm.curOrderAmount < _mti.m_buyingPerInvestment[_arrayNumber])
                                    {
                                        gMainForm.setLogText("예수금이 부족하여 추매를 할 수 없습니다.");
                                        return;
                                    }
                                    // 주문수량계산 : 시장가의 경우 상한가를 기준으로 수량을 계산한다.
                                    int _qnt = (int)(_mti.m_buyingPerInvestment[_arrayNumber] / _mti.m_upperLimitPrice);
                                    long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                "추가매수",                                // 사용자 구분명
                                                                gMainForm.GetScreenNumber(),   // 화면번호
                                                                _mtc.account,                                // 계좌번호 10자리
                                                                1,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                itemCode,                                     // 종목코드 6자리                                                        
                                                                _qnt,                                              // 주문수량
                                                                0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                "03", // 시장가                                  // 거래구분
                                                                "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                    if (_ret == 0) // 성공
                                    {
                                        _mti.m_brePurchased = true; // 추가매수 성공시 true로 변환
                                        _mti.m_rePurchaseArray[_arrayNumber] = true;
                                        _mti.m_rePurchaseNumber = _arrayNumber;
                                        //Console.WriteLine(itemName + " 일반매수아닐경우 주문성공 _mti.m_rePurchaseNumber: " + _mti.m_rePurchaseNumber);

                                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                        {
                                            if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                            {
                                                row.Cells["매매진행_진행상황"].Value = "매수중";
                                                break;
                                            }
                                        }
                                        gMainForm.setLogText("추가 매수 성공 : " + itemName);
                                    }
                                    else // 실패
                                    {
                                        gMainForm.setLogText("추가 매수 실패 : " + itemName);
                                    }
                                }

                                if (_mti.m_rePurchaseNumber == -1) // 추매진행중이 아니면...익절, 손절
                                {
                                    ///////////////////////////////////////////////////////// 익절 ///////////////////////////////////////////////////////
                                    bool _bTakeProfit = false;
                                    bool _bTakeProfitBasic = false;
                                    _str = "";

                                    if (_mti.m_takeProfitUsing == 1) // 익절사용
                                    {
                                        if (_mti.m_takeProfitType == 0) // 기본 익절
                                        {
                                            for (int i = 0; i < 5; i++)
                                            {
                                                if (i != _mti.m_currentTakeProfitStep)
                                                    continue;
                                                if (_mti.m_takeProfitArray[i])
                                                    continue;
                                                if (_mti.m_takeProfitProportion[i] == 0)
                                                    continue;

                                                if (_mti.m_rateOfReturn >= _mti.m_takeProfitBuyingPricePer[i])
                                                {
                                                    _bTakeProfitBasic = true;
                                                    _arrayNumber = i;
                                                    _str = "기본 익절 : " + itemName;
                                                    gMainForm.setLogText(_str);
                                                }
                                            }
                                        }
                                        else if (_mti.m_takeProfitType == 1) // 이동평균선
                                        {
                                            // moving_PriceDayCur[2] 현재 일봉 매수이평선 가겨
                                            // moving_PriceCur[2] 현재 분봉 매수이평선 가격
                                            // m_takeProfitBongType 일봉,분봉
                                            // m_takeProfitLineAccessPer n%
                                            // m_takeProfitDistance 근접, 돌파
                                            double _standardPrice = 0;
                                            if (_mti.m_takeProfitBongType == 0) // 일봉
                                            {
                                                if (_mti.m_takeProfitMovePriceKindType == 0) // 단순
                                                    _standardPrice = _mti.moving_PriceDayCur[2];
                                                else
                                                    _standardPrice = _mti.moving_PriceDayCurE[2];
                                            }
                                            else if (_mti.m_takeProfitBongType == 1) // 분봉
                                            {
                                                if (_mti.m_takeProfitMovePriceKindType == 0) // 단순
                                                    _standardPrice = _mti.moving_PriceCur[2];
                                                else
                                                    _standardPrice = _mti.moving_PriceCurE[2];
                                            }

                                            if (_mti.m_takeProfitDistance == 0) // 근접
                                            {
                                                double _perUpPrice = getPercentPrice(_standardPrice, _mti.m_takeProfitLineAccessPer);
                                                double _perDownPrice = getPercentPrice(_standardPrice, _mti.m_takeProfitLineAccessPer * -1);
                                                if (_perUpPrice >= _mti.m_currentPrice && _mti.m_currentPrice >= _perDownPrice)
                                                {
                                                    _bTakeProfit = true;
                                                }
                                            }
                                            else if (_mti.m_takeProfitDistance == 1) // 돌파
                                            {
                                                if (_mti.m_currentPrice > _standardPrice)
                                                {
                                                    _bTakeProfit = true;
                                                }
                                            }
                                            if (_bTakeProfit)
                                            {
                                                _str = "이동평균선 익절 : " + itemName;
                                                gMainForm.setLogText(_str);
                                            }
                                        }
                                        else if (_mti.m_takeProfitType == 2) // 스토캐스틱SLOW
                                        {
                                            // stochastics_KPriceDayCur[2] 현재 일봉 k값
                                            // stochastics_DPriceDayCur[2] 현재 일봉 d값
                                            // stochastics_KPriceCur[2] 현재 분봉 k값
                                            // stochastics_DPriceCur[2] 현재 분봉 d값
                                            // m_takeProfitBongType 일봉인지 분봉인지                                                
                                            // m_takeProfitLineAccessPer n%
                                            // m_takeProfitDistance 0:이상 1:이하
                                            double _standardPriceK = 0;
                                            double _standardPriceD = 0;
                                            if (_mti.m_takeProfitBongType == 0) // 일봉
                                            {
                                                _standardPriceK = _mti.stochastics_KPriceDayCur[2];
                                                _standardPriceD = _mti.stochastics_DPriceDayCur[2];
                                            }
                                            else if (_mti.m_takeProfitBongType == 1) // 분봉
                                            {
                                                _standardPriceK = _mti.stochastics_KPriceCur[2];
                                                _standardPriceD = _mti.stochastics_DPriceCur[2];
                                            }

                                            if (_mti.m_takeProfitDistance == 0) // k값 이상
                                            {
                                                if (_standardPriceK > _mti.m_takeProfitLineAccessPer)
                                                {
                                                    _bTakeProfit = true;
                                                }
                                            }
                                            else if (_mti.m_takeProfitDistance == 1) // k값 이하
                                            {
                                                if (_standardPriceK < _mti.m_takeProfitLineAccessPer)
                                                {
                                                    _bTakeProfit = true;
                                                }
                                            }
                                            if (_bTakeProfit)
                                            {
                                                _str = "스토캐스틱SLOW 익절 : " + itemName;
                                                gMainForm.setLogText(_str);
                                            }
                                        }
                                        else if (_mti.m_takeProfitType == 3) // 볼린저밴드
                                        {
                                            // bollinger_upPriceDayCur[2] 상한선값
                                            // bollinger_centerPriceDayCur[2] 중심선 값
                                            // bollinger_downPriceDayCur[2] 하한선 값
                                            // bollinger_upPriceCur[2] 상한선 값
                                            // bollinger_centerPriceCur[2] 중심선 값
                                            // bollinger_downPriceCur[2] 하한선 값
                                            // m_takeProfitBongType 일봉인지 분봉인지                
                                            // m_takeProfitLine3Type 상,중,하선
                                            // m_takeProfitLineAccessPer n%
                                            // m_takeProfitDistance 0:근접, 1:돌파, 2:이탈
                                            double _standardPrice = 0;
                                            if (_mti.m_takeProfitBongType == 0) // 일봉
                                            {
                                                if (_mti.m_takeProfitLine3Type == 0) // 상
                                                    _standardPrice = _mti.bollinger_upPriceDayCur[2];
                                                else if (_mti.m_takeProfitLine3Type == 1) // 중
                                                    _standardPrice = _mti.bollinger_centerPriceDayCur[2];
                                                else if (_mti.m_takeProfitLine3Type == 2) // 하
                                                    _standardPrice = _mti.bollinger_downPriceDayCur[2];
                                            }
                                            else if (_mti.m_takeProfitBongType == 1) // 분봉
                                            {
                                                if (_mti.m_takeProfitLine3Type == 0) // 상
                                                    _standardPrice = _mti.bollinger_upPriceCur[2];
                                                else if (_mti.m_takeProfitLine3Type == 1) // 중
                                                    _standardPrice = _mti.bollinger_centerPriceCur[2];
                                                else if (_mti.m_takeProfitLine3Type == 2) // 하
                                                    _standardPrice = _mti.bollinger_downPriceCur[2];
                                            }

                                            if (_mti.m_takeProfitDistance == 0) // 근접
                                            {
                                                // 기준가격 위 아래 계산해서 그 사이에 들어왔을때 매수
                                                double _perUpPrice = getPercentPrice(_standardPrice, _mti.m_takeProfitLineAccessPer);
                                                double _perDownPrice = getPercentPrice(_standardPrice, _mti.m_takeProfitLineAccessPer * -1);
                                                if (_perUpPrice >= _mti.m_currentPrice && _mti.m_currentPrice >= _perDownPrice)
                                                {
                                                    _bTakeProfit = true;
                                                }
                                            }
                                            else if (_mti.m_takeProfitDistance == 1) // 돌파
                                            {
                                                if (_mti.m_currentPrice > _standardPrice)
                                                {
                                                    _bTakeProfit = true;
                                                }
                                            }
                                            else if (_mti.m_takeProfitDistance == 2) // 이탈
                                            {
                                                if (_mti.m_currentPrice < _standardPrice)
                                                {
                                                    _bTakeProfit = true;
                                                }
                                            }
                                            if (_bTakeProfit)
                                            {
                                                _str = "볼린저밴드 익절 : " + itemName;
                                                gMainForm.setLogText(_str);
                                            }
                                        }
                                        else if (_mti.m_takeProfitType == 4) // 엔벨로프
                                        {
                                            // envelope_upPriceDayCur[2] 상한선 값
                                            // envelope_centerPriceDayCur[2] 중심선 값
                                            // envelope_downPriceDayCur[2] 하한선 값
                                            // envelope_upPriceCur[2] 상한선 값
                                            // envelope_centerPriceCur[2] 중심선 값
                                            // envelope_downPriceCur[2] 하한선 값
                                            // m_takeProfitBongType 일봉인지 분봉인지                                                
                                            // m_takeProfitLine3Type 상,중,하선
                                            // m_takeProfitLineAccessPer n%
                                            // m_takeProfitDistance 0:근접, 1:돌파, 2:이탈
                                            double _standardPrice = 0;
                                            if (_mti.m_takeProfitBongType == 0) // 일봉
                                            {
                                                if (_mti.m_takeProfitLine3Type == 0) // 상
                                                    _standardPrice = _mti.envelope_upPriceDayCur[2];
                                                else if (_mti.m_takeProfitLine3Type == 1) // 중
                                                    _standardPrice = _mti.envelope_centerPriceDayCur[2];
                                                else if (_mti.m_takeProfitLine3Type == 2) // 하
                                                    _standardPrice = _mti.envelope_downPriceDayCur[2];
                                            }
                                            else if (_mti.m_takeProfitBongType == 1) // 분봉
                                            {
                                                if (_mti.m_takeProfitLine3Type == 0) // 상
                                                    _standardPrice = _mti.envelope_upPriceCur[2];
                                                else if (_mti.m_takeProfitLine3Type == 1) // 중
                                                    _standardPrice = _mti.envelope_centerPriceCur[2];
                                                else if (_mti.m_takeProfitLine3Type == 2) // 하
                                                    _standardPrice = _mti.envelope_downPriceCur[2];
                                            }
                                            if (_mti.m_takeProfitDistance == 0) // 근접
                                            {
                                                // 기준가격 위 아래 계산해서 그 사이에 들어왔을때 매수
                                                double _perUpPrice = getPercentPrice(_standardPrice, _mti.m_takeProfitLineAccessPer);
                                                double _perDownPrice = getPercentPrice(_standardPrice, _mti.m_takeProfitLineAccessPer * -1);
                                                if (_perUpPrice >= _mti.m_currentPrice && _mti.m_currentPrice >= _perDownPrice)
                                                {
                                                    _bTakeProfit = true;
                                                }
                                            }
                                            else if (_mti.m_takeProfitDistance == 1) // 돌파
                                            {
                                                if (_mti.m_currentPrice > _standardPrice)
                                                {
                                                    _bTakeProfit = true;
                                                }
                                            }
                                            else if (_mti.m_takeProfitDistance == 2) // 이탈
                                            {
                                                if (_mti.m_currentPrice < _standardPrice)
                                                {
                                                    _bTakeProfit = true;
                                                }
                                            }
                                            if (_bTakeProfit)
                                            {
                                                _str = "엔벨로프 익절 : " + itemName;
                                                gMainForm.setLogText(_str);
                                            }
                                        }

                                        if (_bTakeProfit)
                                        {
                                            int _qnt = _mti.m_totalQnt; // 보유수량
                                            long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                        "익절매도",                                     // 사용자 구분명
                                                                        gMainForm.GetScreenNumber(),   // 화면번호
                                                                        _mtc.account,                                // 계좌번호 10자리
                                                                        2,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                        itemCode,                                     // 종목코드 6자리                                                        
                                                                        _qnt,                                              // 주문수량
                                                                        0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                        "03", // 시장가                                  // 거래구분
                                                                        "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                            if (_ret == 0) // 익절매도 주문 성공
                                            {
                                                _mti.m_bSold = true;
                                                foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                {
                                                    if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                    {
                                                        row.Cells["매매진행_진행상황"].Value = "매도중";
                                                        break;
                                                    }
                                                }
                                                gMainForm.setLogText("익절 매도 성공 : " + itemName);
                                            }
                                            else
                                            {
                                                gMainForm.setLogText("익절 매도 실패 : " + itemName);
                                            }
                                        }
                                        if(_bTakeProfitBasic)
                                        {
                                            int _qnt = _mti.m_totalQnt * (int)_mti.m_takeProfitProportion[_arrayNumber] / 100; // 보유수량
                                            long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                        "익절매도",                                     // 사용자 구분명
                                                                        gMainForm.GetScreenNumber(),   // 화면번호
                                                                        _mtc.account,                                // 계좌번호 10자리
                                                                        2,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                        itemCode,                                     // 종목코드 6자리                                                        
                                                                        _qnt,                                              // 주문수량
                                                                        0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                        "03", // 시장가                                  // 거래구분
                                                                        "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                            
                                            if (_ret == 0) // 익절매도 주문 성공
                                            {
                                                _mti.m_bSold = true;
                                                foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                {
                                                    if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode)
                                                        && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                    {
                                                        row.Cells["매매진행_진행상황"].Value = _arrayNumber + 1 + "차 익절";
                                                        break;
                                                    }
                                                }
                                                _mti.m_currentTakeProfitStep++;
                                                //Console.WriteLine("보조지표관련 익절시차수" + _mti.m_currentTakeProfitStep);
                                                gMainForm.setLogText(_arrayNumber + 1 + "차 익절 매도 주문 성공 : " + itemName);
                                                gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 익절 매도 주문 성공");
                                            }
                                            else
                                            {
                                                gMainForm.setLogText(_arrayNumber + 1 + "차 익절 매도 실패 : " + itemName);
                                                gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 익절 매도 주문 실패");
                                            }
                                        }
                                    }
                                    if (!_bTakeProfit)
                                    {
                                        //////////////////////////////////////////////////////////////// 손절 /////////////////////////////////////////////////////////////////
                                        bool _bStopLoss = false;
                                        bool _bStopLossBasic = false;
                                        _str = "";
                                        if (_mti.m_stopLossUsing == 1) // 손절사용
                                        {
                                            if (_mti.m_stopLossType == 0) // 기본 손절
                                            {
                                                for (int i = 0; i < 5; i++)
                                                {
                                                    if (i != _mti.m_currentStopLossStep)
                                                        continue;
                                                    if (_mti.m_stopLossArray[i])
                                                        continue;
                                                    if (_mti.m_stopLossProportion[i] == 0)
                                                        continue;

                                                    if (_mti.m_rateOfReturn <= _mti.m_stopLossBuyingPricePer[i])
                                                    {
                                                        _bStopLossBasic = true;
                                                        _arrayNumber = i;
                                                        _str = "기본 손절 : " + itemName;
                                                        gMainForm.setLogText(_str);
                                                    }
                                                }
                                            }
                                            else if (_mti.m_stopLossType == 1) // 이동평균선 손절
                                            {
                                                // moving_PriceDayCur[3] 현재 일봉 매수이평선 가겨
                                                // moving_PriceCur[3] 현재 분봉 매수이평선 가격
                                                // m_stopLossBongType 일봉, 분봉
                                                // m_stopLossLineAccessPer n%
                                                // m_stopLossDistance 0:근접 1:이탈
                                                double _standardPrice = 0;
                                                if (_mti.m_stopLossBongType == 0) // 일봉
                                                {
                                                    if (_mti.m_stopLossMovePriceKindType == 0) // 단수
                                                        _standardPrice = _mti.moving_PriceDayCur[3];
                                                    else
                                                        _standardPrice = _mti.moving_PriceDayCurE[3];
                                                }
                                                else if (_mti.m_stopLossBongType == 1) // 분봉
                                                {
                                                    if (_mti.m_stopLossMovePriceKindType == 0) // 단수
                                                        _standardPrice = _mti.moving_PriceCur[3];
                                                    else
                                                        _standardPrice = _mti.moving_PriceCurE[3];
                                                }

                                                if (_mti.m_stopLossDistance == 0) // 근접
                                                {
                                                    double _perUpPrice = getPercentPrice(_standardPrice, _mti.m_stopLossLineAccessPer);
                                                    double _perDownPrice = getPercentPrice(_standardPrice, _mti.m_stopLossLineAccessPer * -1);
                                                    if (_perUpPrice >= _mti.m_currentPrice && _mti.m_currentPrice >= _perDownPrice)
                                                    {
                                                        _bStopLoss = true;
                                                    }
                                                }
                                                else if (_mti.m_stopLossDistance == 1) // 이탈
                                                {
                                                    if (_mti.m_currentPrice < _standardPrice)
                                                    {
                                                        _bStopLoss = true;
                                                    }
                                                }
                                                if (_bStopLoss)
                                                {
                                                    _str = "이동평균선 손절 : " + itemName;
                                                    gMainForm.setLogText(_str);
                                                }
                                            }
                                            else if (_mti.m_stopLossType == 2) // 볼린저벤드 손절
                                            {
                                                // bollinger_upPriceDayCur[3] 상한선값
                                                // bollinger_centerPriceDayCur[3] 중심선 값
                                                // bollinger_downPriceDayCur[3] 하한선 값
                                                // bollinger_upPriceCur[3] 상한선 값
                                                // bollinger_centerPriceCur[3] 중심선 값
                                                // bollinger_downPriceCur[3] 하한선 값
                                                // m_stopLossBongType 일봉인지 분봉인지
                                                // m_stopLossLine3Type 상,중,하선
                                                // m_stopLossLineAccessPer n%
                                                // m_stopLossDistance 0:근접, 1:돌파, 2:이탈
                                                double _standardPrice = 0;
                                                if (_mti.m_stopLossBongType == 0) // 일봉
                                                {
                                                    if (_mti.m_stopLossLine3Type == 0) // 상
                                                        _standardPrice = _mti.bollinger_upPriceDayCur[3];
                                                    else if (_mti.m_stopLossLine3Type == 1) // 중
                                                        _standardPrice = _mti.bollinger_centerPriceDayCur[3];
                                                    else if (_mti.m_stopLossLine3Type == 2) // 하
                                                        _standardPrice = _mti.bollinger_downPriceDayCur[3];
                                                }
                                                else if (_mti.m_stopLossBongType == 1) // 분봉
                                                {
                                                    if (_mti.m_stopLossLine3Type == 0) // 상
                                                        _standardPrice = _mti.bollinger_upPriceCur[3];
                                                    else if (_mti.m_stopLossLine3Type == 1) // 중
                                                        _standardPrice = _mti.bollinger_centerPriceCur[3];
                                                    else if (_mti.m_stopLossLine3Type == 2) // 하
                                                        _standardPrice = _mti.bollinger_downPriceCur[3];
                                                }
                                                if (_mti.m_stopLossDistance == 0) // 근접
                                                {
                                                    // 기준가격 위 아래 계산해서 그 사이에 들어왔을때 매수
                                                    double _perUpPrice = getPercentPrice(_standardPrice, _mti.m_stopLossLineAccessPer);
                                                    double _perDownPrice = getPercentPrice(_standardPrice, _mti.m_stopLossLineAccessPer * -1);
                                                    if (_perUpPrice >= _mti.m_currentPrice && _mti.m_currentPrice >= _perDownPrice)
                                                    {
                                                        _bStopLoss = true;
                                                    }
                                                }
                                                else if (_mti.m_stopLossDistance == 1) // 이탈
                                                {
                                                    if (_mti.m_currentPrice < _standardPrice)
                                                    {
                                                        _bStopLoss = true;
                                                    }
                                                }
                                                if (_bStopLoss)
                                                {
                                                    _str = "볼린저벤드 손절 : " + itemName;
                                                    gMainForm.setLogText(_str);
                                                }
                                            }
                                            else if (_mti.m_stopLossType == 3) // 엔벨로프 손절
                                            {
                                                // envelope_upPriceDayCur[3] 상한선 값
                                                // envelope_centerPriceDayCur[3] 중심선 값
                                                // envelope_downPriceDayCur[3] 하한선 값
                                                // envelope_upPriceCur[3] 상한선 값
                                                // envelope_centerPriceCur[3] 중심선 값
                                                // envelope_downPriceCur[3] 하한선 값
                                                // m_stopLossBongType 일봉인지 분봉인지                
                                                // m_stopLossLine3Type 상,중,하선
                                                // m_stopLossLineAccessPer n%
                                                // m_stopLossDistance 0:근접, 1:돌파, 2:이탈
                                                double _standardPrice = 0;
                                                if (_mti.m_stopLossBongType == 0) // 일봉
                                                {
                                                    if (_mti.m_stopLossLine3Type == 0) // 상
                                                        _standardPrice = _mti.envelope_upPriceDayCur[3];
                                                    else if (_mti.m_stopLossLine3Type == 1) // 중
                                                        _standardPrice = _mti.envelope_centerPriceDayCur[3];
                                                    else if (_mti.m_stopLossLine3Type == 2) // 하
                                                        _standardPrice = _mti.envelope_downPriceDayCur[3];
                                                }
                                                else if (_mti.m_stopLossBongType == 1) // 분봉
                                                {
                                                    if (_mti.m_stopLossLine3Type == 0) // 상
                                                        _standardPrice = _mti.envelope_upPriceCur[3];
                                                    else if (_mti.m_stopLossLine3Type == 1) // 중
                                                        _standardPrice = _mti.envelope_centerPriceCur[3];
                                                    else if (_mti.m_stopLossLine3Type == 2) // 하
                                                        _standardPrice = _mti.envelope_downPriceCur[3];
                                                }
                                                if (_mti.m_stopLossDistance == 0) // 근접
                                                {
                                                    // 기준가격 위 아래 계산해서 그 사이에 들어왔을때 매수
                                                    double _perUpPrice = getPercentPrice(_standardPrice, _mti.m_stopLossLineAccessPer);
                                                    double _perDownPrice = getPercentPrice(_standardPrice, _mti.m_stopLossLineAccessPer * -1);
                                                    if (_perUpPrice >= _mti.m_currentPrice && _mti.m_currentPrice >= _perDownPrice)
                                                    {
                                                        _bStopLoss = true;
                                                    }
                                                }
                                                else if (_mti.m_stopLossDistance == 1) // 이탈
                                                {
                                                    if (_mti.m_currentPrice < _standardPrice)
                                                    {
                                                        _bStopLoss = true;
                                                    }
                                                }
                                            }
                                            if (_bStopLoss)
                                            {
                                                int _qnt = _mti.m_totalQnt; // 보유수량
                                                long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                            "손절매도",                                     // 사용자 구분명
                                                                            gMainForm.GetScreenNumber(),   // 화면번호
                                                                            _mtc.account,                                // 계좌번호 10자리
                                                                            2,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                            itemCode,                                     // 종목코드 6자리                                                        
                                                                            _qnt,                                              // 주문수량
                                                                            0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                            "03", // 시장가                                  // 거래구분
                                                                            "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                if (_ret == 0) // 손절매도 주문 성공
                                                {
                                                    _mti.m_bSold = true;
                                                    foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                    {
                                                        if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                        {
                                                            row.Cells["매매진행_진행상황"].Value = "매도중";
                                                            break;
                                                        }
                                                    }
                                                    gMainForm.setLogText("손절 매도 성공 : " + itemName);
                                                }
                                                else
                                                {
                                                    gMainForm.setLogText("손절 매도 실패 : " + itemName);
                                                }
                                            }
                                            if (_bStopLossBasic)
                                            {
                                                int _qnt = _mti.m_totalQnt * (int)_mti.m_stopLossProportion[_arrayNumber] / 100; // 보유수량
                                                long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                            "손절매도",                                     // 사용자 구분명
                                                                            gMainForm.GetScreenNumber(),   // 화면번호
                                                                            _mtc.account,                                // 계좌번호 10자리
                                                                            2,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                            itemCode,                                     // 종목코드 6자리                                                        
                                                                            _qnt,                                              // 주문수량
                                                                            0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                            "03", // 시장가                                  // 거래구분
                                                                            "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                
                                                if (_ret == 0) // 손절매도 주문 성공
                                                {
                                                    _mti.m_bSold = true;
                                                    foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                    {
                                                        if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode)
                                                            && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                        {
                                                            row.Cells["매매진행_진행상황"].Value = _arrayNumber + 1 + "차 손절";
                                                            break;
                                                        }
                                                    }
                                                    _mti.m_currentStopLossStep++;
                                                    //Console.WriteLine("기본매수가 아닐시 손절차수: " + _mti.m_currentStopLossStep);
                                                    gMainForm.setLogText(_arrayNumber + 1 + "차 손절 매도 주문 성공 : " + itemName);
                                                    gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 손절 매도 주문 성공");
                                                }
                                                else
                                                {
                                                    gMainForm.setLogText(_arrayNumber + 1 + "차 손절 매도 주문 실패 : " + itemName);
                                                    gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 손절 매도 주문 실패");
                                                }
                                            }
                                        }
                                    }
                                    //ts매도
                                    if (_mtc.tsMedoUsing == 1)
                                    {
                                        foreach (GoJumMedo _gmd in gMainForm.gojumMedoList)
                                        {
                                            // gojumMedoList 에서 동일한 종목코드인지를 확인함
                                            GoJumMedo _goJumMedo = gMainForm.gojumMedoList.Find(o => o.itemCode.Equals(itemCode));

                                            if (_goJumMedo != null)
                                            {
                                                //ts매도 변수
                                                bool _bTsmedoCheck = false;
                                                string _str1 = "";
                                                int _arrayNumber1 = -1;

                                                //3번의 ts매도를 확인함
                                                for (int i = 0; i < 3; i++)
                                                {
                                                    if (i != _mti.m_currentTsmedoStep)
                                                        continue;

                                                    if (_mti.m_tsMedoArray[i]) // ts매도 여부 확인
                                                        continue;

                                                    if (_mti.m_tsMedoProportion[i] == 0) // 비중이 0이라면
                                                        continue;

                                                    if (_mti.m_tsMedoUsingType[i] == 0) //목표가ts
                                                    {
                                                        if (_mti.m_currentTsmedoStep == 0 && _mti.m_rateOfReturn >= _mti.m_tsMedoArchievedPer[i]) // 목표가와 수익률이 동일해졋을때,
                                                        {
                                                            if (closingPrice > _goJumMedo.highPrice[i] && closingPrice > _gmd.purchasePrice)
                                                            {
                                                                _goJumMedo.highPrice[i] = closingPrice;
                                                                //Console.WriteLine("1차 목표가ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);
                                                            }
                                                        }
                                                        if (_mti.m_currentTsmedoStep == 1 && _mti.m_rateOfReturn >= _mti.m_tsMedoArchievedPer[i]) // 목표가와 수익률이 동일해졋을때,
                                                        {
                                                            if (closingPrice > _goJumMedo.highPrice[i])
                                                            {
                                                                _goJumMedo.highPrice[i] = closingPrice;
                                                                //Console.WriteLine("2차 목표가ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);
                                                            }
                                                        }
                                                        if (_mti.m_currentTsmedoStep == 2 && _mti.m_rateOfReturn >= _mti.m_tsMedoArchievedPer[i]) // 목표가와 수익률이 동일해졋을때,
                                                        {
                                                            if (closingPrice > _goJumMedo.highPrice[i])
                                                            {
                                                                _goJumMedo.highPrice[i] = closingPrice;
                                                                //Console.WriteLine("3차 목표가ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);
                                                            }
                                                        }
                                                        double dropPercent = _mtc.tsMedoPercent[i] / 100;
                                                        double dropPrice = _goJumMedo.highPrice[i] * (1 - dropPercent);
                                                        //Console.WriteLine("종목명: " + itemName + " 목표가ts사용중" + " 현재가: " + closingPrice + " 고점가격: " + _goJumMedo.highPrice + " 하락률: " + dropPercent + " 고점대비 하락가격: " + dropPrice);
                                                        if (_mti.m_tsProfitPreservation1 == 1 && _mti.m_currentTsmedoStep == 0)
                                                        {
                                                            if (closingPrice <= dropPrice && _mti.m_rateOfReturn > 0)
                                                            {
                                                                //Console.WriteLine("1차 목표가 ts 이익구간작동");
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber1 = i;
                                                                _str = "목표가ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation1 == 0 && _mti.m_currentTsmedoStep == 0)
                                                        {
                                                            if (closingPrice <= dropPrice)
                                                            {
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber1 = i;
                                                                _str = "목표가ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation2 == 1 && _mti.m_currentTsmedoStep == 1)
                                                        {
                                                            if (closingPrice <= dropPrice && _mti.m_rateOfReturn > 0)
                                                            {
                                                                //Console.WriteLine("2차 목표가 ts 이익구간작동");
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber1 = i;
                                                                _str = "목표가ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation2 == 0 && _mti.m_currentTsmedoStep == 1)
                                                        {
                                                            if (closingPrice <= dropPrice)
                                                            {
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber1 = i;
                                                                _str = "목표가ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation3 == 1 && _mti.m_currentTsmedoStep == 2)
                                                        {
                                                            if (closingPrice <= dropPrice && _mti.m_rateOfReturn > 0)
                                                            {
                                                                //Console.WriteLine("3차 목표가 ts 이익구간작동");
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber1 = i;
                                                                _str = "목표가ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation3 == 0 && _mti.m_currentTsmedoStep == 2)
                                                        {
                                                            if (closingPrice <= dropPrice)
                                                            {
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber1 = i;
                                                                _str = "목표가ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (_mti.m_tsMedoUsingType[i] == 1) //고점ts
                                                    {
                                                        if (_mti.m_currentTsmedoStep == 0 && closingPrice > _goJumMedo.highPrice[i] && closingPrice >= _gmd.purchasePrice)
                                                        {
                                                            _goJumMedo.highPrice[i] = closingPrice;
                                                            //Console.WriteLine("1차 고점ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);
                                                        }
                                                        // 현재가가 고점보다 높을경우 고점가격에 대한 갱신
                                                        if (_mti.m_currentTsmedoStep == 1 && _mti.m_tsMedoArray[_mti.m_currentTsmedoStep - 1] && closingPrice > _goJumMedo.highPrice[i])
                                                        {
                                                            _goJumMedo.highPrice[i] = closingPrice;
                                                            //Console.WriteLine("2차 고점ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);

                                                        }
                                                        if (_mti.m_currentTsmedoStep == 2 && _mti.m_tsMedoArray[_mti.m_currentTsmedoStep - 1] && closingPrice > _goJumMedo.highPrice[i])
                                                        {
                                                            _goJumMedo.highPrice[i] = closingPrice;
                                                            //Console.WriteLine("3차 고점ts 시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice[i]);

                                                        }
                                                        double dropPercent = _mtc.tsMedoPercent[i] / 100;
                                                        double dropPrice = _goJumMedo.highPrice[i] * (1 - dropPercent);
                                                        //Console.WriteLine("종목명: " + itemName + " 고점ts사용중"  +" 현재가: " + closingPrice + " 고점가격: " + _goJumMedo.highPrice + " 하락률: " + dropPercent + " 고점대비 하락가격: " + dropPrice);
                                                        if (_mti.m_tsProfitPreservation1 == 1 && _mti.m_currentTsmedoStep == 0)
                                                        {
                                                            if (closingPrice <= dropPrice && _mti.m_rateOfReturn > 0)
                                                            {
                                                                //Console.WriteLine("1차 고점 ts 이익구간작동");
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber1 = i;
                                                                _str = "고점ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation1 == 0 && _mti.m_currentTsmedoStep == 0)
                                                        {
                                                            if (closingPrice <= dropPrice)
                                                            {
                                                                //Console.WriteLine("1차 고점ts 이익구간작동안함");
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber1 = i;
                                                                _str = "고점ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation2 == 1 && _mti.m_currentTsmedoStep == 1)
                                                        {
                                                            if (closingPrice <= dropPrice && _mti.m_rateOfReturn > 0)
                                                            {
                                                                //Console.WriteLine("2차 고점 ts 이익구간작동");
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber1 = i;
                                                                _str = "고점ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation2 == 0 && _mti.m_currentTsmedoStep == 1)
                                                        {
                                                            if (closingPrice <= dropPrice)
                                                            {
                                                                //Console.WriteLine("2차 고점ts 이익구간작동안함");
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber1 = i;
                                                                _str = "고점ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation3 == 1 && _mti.m_currentTsmedoStep == 2)
                                                        {
                                                            if (closingPrice <= dropPrice && _mti.m_rateOfReturn > 0)
                                                            {
                                                                //Console.WriteLine("3차 고점 ts 이익구간작동");
                                                                //Console.WriteLine(itemName + " 하락시 매도가격: " + dropPrice);
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber1 = i;
                                                                _str = "고점ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                        if (_mti.m_tsProfitPreservation3 == 0 && _mti.m_currentTsmedoStep == 2)
                                                        {
                                                            if (closingPrice <= dropPrice)
                                                            {
                                                                //Console.WriteLine("3차 고점ts 이익구간작동안함");
                                                                _bTsmedoCheck = true;
                                                                _arrayNumber1 = i;
                                                                _str = "고점ts매도 : " + itemName;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                }
                                                if (_bTsmedoCheck) //매도주문 코딩
                                                {
                                                    // 정해진 비중만큼 계산을 해야한다. 이때 배열이있으니깐 for문을 돌려야할듯?
                                                    //Console.WriteLine("_arrayNumber: " + _arrayNumber1);
                                                    int _qnt = _mti.m_totalQnt * (int)_mti.m_tsMedoProportion[_arrayNumber1] / 100;
                                                    //Console.WriteLine("ts매도수량계산: " + _qnt);
                                                    long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                                "ts매도",                  // 사용자구분명
                                                                                                gMainForm.GetScreenNumber(), // 화면번호
                                                                                                _mtc.account,                // 계좌번호 10자리
                                                                                                2,                           // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                                itemCode,                    // 종목코드 6자리
                                                                                                _qnt,                        // 주문수량
                                                                                                0,                           // 주문가격 : 시장가일때는 0이다.
                                                                                                "03",                        // 거래구분 : 시장가는 "03"
                                                                                                "");                         // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                    if (_ret == 0) // ts매도성공
                                                    {
                                                        _mti.m_bSold = true;
                                                        _mti.m_tsMedoArray[_arrayNumber1] = true;
                                                        _mti.m_tsMedoNumber = _arrayNumber1;
                                                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                        {
                                                            if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) &&
                                                                row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                            {
                                                                row.Cells["매매진행_진행상황"].Value = _arrayNumber1 + 1 + "차ts매도";
                                                                break;
                                                            }
                                                        }
                                                        gMainForm.setLogText(_arrayNumber1 + 1 + "차ts 매도 성공: " + itemName);

                                                        _mti.m_currentTsmedoStep++;
                                                        //Console.WriteLine("현재 ts매도 차수: " + _mti.m_currentTsmedoStep);
                                                    }
                                                    else
                                                    {
                                                        gMainForm.setLogText(_arrayNumber1 + 1 + "차ts 매도 실패: " + itemName);
                                                    }
                                                }
                                                /*
                                                // 현재가가 고점보다 높을경우 고점가격에 대한 갱신
                                                if (closingPrice > _goJumMedo.highPrice && closingPrice > _gmd.purchasePrice)
                                                {
                                                    _goJumMedo.highPrice = closingPrice;
                                                    //Console.WriteLine("시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice);
                                                }
                                                //double dropPercent = _mtc.tsMedoPercent[0] / 100;
                                                //double dropPrice = _goJumMedo.highPrice * (1 - dropPercent);
                                                //Console.WriteLine("고점가격: " + _goJumMedo.highPrice + " 하락률: " + dropPercent + " 고점대비 하락가격: " + dropPrice);
                                                if (closingPrice <= _goJumMedo.highPrice * (1 - dropPercent))
                                                {
                                                    int _qnt = _mti.m_totalQnt; //보유수량
                                                    long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                    "ts매도",                  // 사용자구분명
                                                                                    gMainForm.GetScreenNumber(), // 화면번호
                                                                                    _mtc.account,                // 계좌번호 10자리
                                                                                    2,                           // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                    itemCode,                    // 종목코드 6자리
                                                                                    _qnt,                        // 주문수량
                                                                                    0,                           // 주문가격 : 시장가일때는 0이다.
                                                                                    "03",                        // 거래구분 : 시장가는 "03"
                                                                                    "");                         // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                    if (_ret == 0) // ts매도성공
                                                    {
                                                        _mti.m_bSold = true;
                                                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                        {
                                                            if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) &&
                                                                row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                            {
                                                                row.Cells["매매진행_진행상황"].Value = "ts매도중";
                                                                break;
                                                            }
                                                        }
                                                        gMainForm.setLogText("ts 매도 성공: " + itemCode);
                                                    }
                                                    else
                                                    {
                                                        gMainForm.setLogText("ts 매도 실패: " + itemCode);
                                                    }
                                                }*/
                                            }
                                            else
                                            {
                                                // _goJumMedo가 null일 경우 로그를 남기거나 적절한 조치를 취합니다.
                                                //Console.WriteLine("매수체결이 완료되지않음.");
                                            }
                                        }

                                    }
                                }
                            }
                            /*
                            if (!_mti.m_bSold && _mti.m_rePurchaseNumber == -1) // 매도가 되지않고, m_rePurchaseNumber가 -1인지 체크
                            {
                                //////////////추매///////////////////
                                if(_mtc.rePurchase)
                                {
                                    for(int i=0; i<6; i++)
                                    {
                                        if (_mti.m_rePurchaseArray[i]) // 1차부터 6차추매까지검사 / m_rePurchaseArray[i]값이 true라면 이미 추매가진행됨
                                            continue;
                                        if (_mtc.rePurchaseMoney[i] == 0) // 추매금액을 검사 / 0값이면 다음 순서로 넘김
                                            continue;

                                        if (_mti.m_rateOfReturn <= _mtc.rePurchaseRate[i] && _mti.m_upperLimitPrice > 0) // 수익률이 추매 %보다 낮다면 && 상한가가 0보다 크다면
                                        {
                                            //주문수량 계산
                                            int _qnt = (int)(_mtc.rePurchaseMoney[i] / _mti.m_upperLimitPrice);
                                            long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                     "추가매수",
                                                                                     gMainForm.GetScreenNumber(),
                                                                                     _mtc.account,
                                                                                     1,
                                                                                     itemCode,
                                                                                     _qnt,
                                                                                     0,
                                                                                     "03",
                                                                                     "");
                                            if(_ret == 0) //성공
                                            {
                                                _mti.m_rePurchaseArray[i] = true;
                                                _mti.m_rePurchaseNumber = i;

                                                foreach(DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                {
                                                    if(row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                    {
                                                        row.Cells["매매진행_진행상황"].Value = "매수중";
                                                        break;
                                                    }
                                                }
                                                gMainForm.setLogText("추가 매수 성공 : " + itemName);
                                                gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 추가 매수 주문 성공");
                                            }
                                            else //실패
                                            {
                                                gMainForm.setLogText("추가 매수 실패 : " + itemName);
                                                gMainForm.gFileIOInstance.WriteLogFile(itemName + " : 추가 매수 주문 실패");
                                            }    
                                        }
                                    }
                                }
                                if(_mti.m_rePurchaseNumber == -1) // 추매진행중이 아니면
                                {
                                    /////////////익절////////////////////
                                    // 현재 수익률이 조건식에서 설정된 익절 %보다 높으면
                                    if (_mti.m_rateOfReturn >= _mtc.takeProfitBuyingPricePercent)
                                    {
                                        int _qnt = _mti.m_totalQnt; //보유수량
                                        long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                        "익절매도",                  // 사용자구분명
                                                                        gMainForm.GetScreenNumber(), // 화면번호
                                                                        _mtc.account,                // 계좌번호 10자리
                                                                        2,                           // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                        itemCode,                    // 종목코드 6자리
                                                                        _qnt,                        // 주문수량
                                                                        0,                           // 주문가격 : 시장가일때는 0이다.
                                                                        "03",                        // 거래구분 : 시장가는 "03"
                                                                        "");                         // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                        if (_ret == 0) // 익절매도성공
                                        {
                                            _mti.m_bSold = true;
                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                            {
                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) &&
                                                    row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                {
                                                    row.Cells["매매진행_진행상황"].Value = "익절매도중";
                                                    break;
                                                }
                                            }
                                            gMainForm.setLogText("익절 매도 성공: " + itemCode);
                                        }
                                        else
                                        {
                                            gMainForm.setLogText("익절 매도 실패: " + itemCode);
                                        }
                                    }
                                    /////////////손절////////////////////
                                    // 현재 수익률이 조건식에서 설정된 익절 %보다 높으면
                                    if (_mti.m_rateOfReturn <= _mtc.stopLossBuyingPricePercent)
                                    {
                                        int _qnt = _mti.m_totalQnt; //보유수량
                                        long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                        "손절매도",                  // 사용자구분명
                                                                        gMainForm.GetScreenNumber(), // 화면번호
                                                                        _mtc.account,                // 계좌번호 10자리
                                                                        2,                           // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                        itemCode,                    // 종목코드 6자리
                                                                        _qnt,                        // 주문수량
                                                                        0,                           // 주문가격 : 시장가일때는 0이다.
                                                                        "03",                        // 거래구분 : 시장가는 "03"
                                                                        "");                         // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                        if (_ret == 0) // 손절매도성공
                                        {
                                            _mti.m_bSold = true;
                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                            {
                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) &&
                                                    row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                {
                                                    row.Cells["매매진행_진행상황"].Value = "손절매도중";
                                                    break;
                                                }
                                            }
                                            gMainForm.setLogText("손절 매도 성공: " + itemCode);
                                        }
                                        else
                                        {
                                            gMainForm.setLogText("손절 매도 실패: " + itemCode);
                                        }
                                    }
                                    //ts매도
                                    if(_mtc.tsMedoUsing == 1 && _mti.m_rePurchaseNumber == -1)
                                    {
                                        foreach (GoJumMedo _gmd in gMainForm.gojumMedoList)
                                        {
                                            // gojumMedoList 에서 동일한 종목코드인지를 확인함
                                            GoJumMedo _goJumMedo = gMainForm.gojumMedoList.Find(o => o.itemCode.Equals(itemCode));
                                            if (_goJumMedo != null)
                                            {
                                                // 현재가가 고점보다 높을경우 고점가격에 대한 갱신
                                                if (closingPrice > _goJumMedo.highPrice && closingPrice > _gmd.purchasePrice)
                                                {
                                                    _goJumMedo.highPrice = closingPrice;
                                                    //Console.WriteLine("시간: " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + itemName + " 현재가: " + closingPrice + " 매수가:" + _goJumMedo.purchasePrice + " 고점가격갱신중: " + _goJumMedo.highPrice);
                                                }
                                                double dropPercent = _mtc.tsMedoPercent / 100;
                                                double dropPrice = _goJumMedo.highPrice * (1 - dropPercent);
                                                //Console.WriteLine("하락률: " + dropPercent + " 고점대비 하락가격: " + dropPrice);
                                                if (closingPrice <= _goJumMedo.highPrice * (1 - dropPercent))
                                                {
                                                    int _qnt = _mti.m_totalQnt; //보유수량
                                                    long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                                    "ts매도",                  // 사용자구분명
                                                                                    gMainForm.GetScreenNumber(), // 화면번호
                                                                                    _mtc.account,                // 계좌번호 10자리
                                                                                    2,                           // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                                    itemCode,                    // 종목코드 6자리
                                                                                    _qnt,                        // 주문수량
                                                                                    0,                           // 주문가격 : 시장가일때는 0이다.
                                                                                    "03",                        // 거래구분 : 시장가는 "03"
                                                                                    "");                         // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                                    if (_ret == 0) // ts매도성공
                                                    {
                                                        _mti.m_bSold = true;
                                                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                        {
                                                            if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(itemCode) &&
                                                                row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                            {
                                                                row.Cells["매매진행_진행상황"].Value = "ts매도중";
                                                                break;
                                                            }
                                                        }
                                                        gMainForm.setLogText("ts 매도 성공: " + itemCode);
                                                    }
                                                    else
                                                    {
                                                        gMainForm.setLogText("ts 매도 실패: " + itemCode);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // _goJumMedo가 null일 경우 로그를 남기거나 적절한 조치를 취합니다.
                                                //Console.WriteLine("매수체결이 완료되지않음.");
                                            }
                                        }

                                    }
                                }
                            }*/
                        }

                        /*
                        // 종목 그리드뷰에 현재가를 넣어준다.
                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                        {
                            // 아이템코드가 같을 경우
                            if (row.Cells["매매진행_종목코드"].Value.ToString().Contains(itemCode))
                            {
                                //현재가
                                row.Cells["매매진행_현재가"].Value = closingPrice.ToString("N0");
                                //수익률
                                int totalEvaluationPrice = 0; // 평가금액
                                int totalEvalutationProfitLoss = 0; // 평가손익
                                double totalPurchaseAmount = _mti.m_totalPurchaseAmount; // 총매입금
                                double totalQnt = _mti.m_totalQnt; // 총매수량
                                double purchaseFee = 0; // 매수가계산 수수료
                                double sellFee = 0;  //매도가계산 수수료
                                double itemTax = 0; //제세금
                                double rateOfReturn = 0; //수익률

                                string _ret = gMainForm.KiwoomAPI.KOA_Functions("GetStockMarketKind", _mti.m_itemCode); //  종목코드 입력으로 해당 종목이 어느 시장에 포함되어 있는지 구하는 기능

                                // 매수가계산 수수료
                                if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                                    purchaseFee = totalPurchaseAmount * 0.0035;
                                else//실서버
                                    purchaseFee = totalPurchaseAmount * 0.00015;
                                purchaseFee = purchaseFee - (purchaseFee % 10);

                                // 매도가계산 수수료
                                if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                                    sellFee = _mti.m_currentPrice * totalQnt * 0.0035;
                                else//실서버
                                    sellFee = _mti.m_currentPrice * totalQnt * 0.00015;
                                sellFee = sellFee - (sellFee % 10);

                                // 제세금
                                if (_ret.Equals("0")) // 코스피
                                {
                                    double _tax1 = _mti.m_currentPrice * totalQnt * 0.0005;
                                    _tax1 = _tax1 - (_tax1 % 1);
                                    double _tax2 = _mti.m_currentPrice * totalQnt * 0.0015;
                                    _tax2 = _tax2 - (_tax2 % 1);
                                    itemTax = _tax1 + _tax2;
                                }
                                else //코스닥
                                {
                                    itemTax = _mti.m_currentPrice * totalQnt * 0.002;
                                    itemTax = itemTax - (itemTax % 1);
                                }

                                //평가금액
                                totalEvaluationPrice = ((int)_mti.m_currentPrice * (int)totalQnt) - (int)purchaseFee - (int)sellFee - (int)itemTax;
                                //평가손익
                                totalEvalutationProfitLoss = (int)totalEvaluationPrice - (int)totalPurchaseAmount;
                                //수익률
                                if (totalPurchaseAmount > 0)
                                    rateOfReturn = (double)totalEvalutationProfitLoss / totalPurchaseAmount * 100;
                                else
                                    rateOfReturn = 0;
                                // 수익률 변수에 저장
                                _mti.m_rateOfReturn = rateOfReturn;

                                if (totalEvalutationProfitLoss != 0)
                                {
                                    row.Cells["매매진행_수익률"].Value = rateOfReturn.ToString("N2") + "%";
                                    //Console.WriteLine("수익률: " + row.Cells["매매진행_수익률"].Value);
                                    // 평가손익
                                    row.Cells["매매진행_평가손익"].Value = gMainForm.setUpDownArrow(totalEvalutationProfitLoss);
                                }
                                break;
                            }
                        }*/
                        break;
                    }
                }
            }
        }
        // 종목접수, 체결, 잔고 관련 메서드
        private void OnReceiveChejanData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            if(e.sGubun.Equals("0")) // 주문접수와 체결시
            {
                string _account = gMainForm.KiwoomAPI.GetChejanData(9201); // 계좌번호
                string _currentPrice = gMainForm.KiwoomAPI.GetChejanData(10); // 현재가
                string _itemName = gMainForm.KiwoomAPI.GetChejanData(302).Trim(); // 종목명
                string _orderQnt = gMainForm.KiwoomAPI.GetChejanData(900); // 주문수량
                string _orderPrice = gMainForm.KiwoomAPI.GetChejanData(901); // 주문가격
                string _outstandingQnt = gMainForm.KiwoomAPI.GetChejanData(902); // 미체결수량
                string _orderType = gMainForm.KiwoomAPI.GetChejanData(905); // 주문구분
                string _tradingType = gMainForm.KiwoomAPI.GetChejanData(906); // 매매구분
                string _tradingTime = gMainForm.KiwoomAPI.GetChejanData(908); // 주문, 체결시간
                string _conclusionPrice = gMainForm.KiwoomAPI.GetChejanData(910); // 체결가
                string _conclusionQnt = gMainForm.KiwoomAPI.GetChejanData(911); // 체결량
                string _orderState = gMainForm.KiwoomAPI.GetChejanData(913); // 주문상태 "접수", "체결" 2개로 전달되서 이것으로 0일때 구분을 한다.
                string _unitConclusionPrice = gMainForm.KiwoomAPI.GetChejanData(914); // 단위체결가
                string _unitConclusionQnt = gMainForm.KiwoomAPI.GetChejanData(915); // 단위체결량
                string _itemCode = gMainForm.KiwoomAPI.GetChejanData(9001).Replace("A", ""); // 종목코드
                string _orderNumber = gMainForm.KiwoomAPI.GetChejanData(9203); // 주문번호
                string _originOrderNumber = gMainForm.KiwoomAPI.GetChejanData(904); // 원주문번호

                // _orderType의 +, -, 공백을 제거해 준다.
                _orderType = _orderType.Replace("+", "").Replace("-", "").Trim();

                if(_orderState.Equals("접수"))
                {
                    //Console.WriteLine("종목명: " + _itemName + " ordertype: " + _orderType);
                    bool found = false;
                    //조건식매매 기준
                    //Console.WriteLine("종목명: " + _itemName + " 매매구분: " + _tradingType + " 주문구분: " + _orderType);
                    foreach (MyTradingCondition _mtc in gMainForm.gMyTradingConditionList) // 조건식데이터 검색
                    {
                        MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(_itemCode)); // 같은 종목이 있는지 찾아낸다
                        if (_mti != null) // 매매중인 종목을 찾은 경우
                        {
                            if (_orderType.Equals("매수"))
                            {
                                //Console.WriteLine("매수접수 " + _itemName + " 주문수량개수: " + _orderQnt + " 미체결수량: " + _outstandingQnt + " 주문번호: " + _orderNumber + " 원주문번호: " + _originOrderNumber + " 주문가격: " + _orderPrice);
                                //orderDataGridView에 추가해 준다.
                                if(int.Parse(_outstandingQnt) > 0) // 주문이 취소되고나서 미체결이 0인상태로 데이터를 한번 더 수신하기 때문에 이를 방지하고자 추가한 부분
                                {
                                    if(_mti.m_mesuoption1 == 1) // 지정가일때
                                    {
                                        if (_mti.m_mesuoption2 == 2)  // 일괄취소 
                                        {
                                            Console.WriteLine(_itemName + " 일괄취소 타이머 start");
                                            _mti._orderCancellationTimer.Start();
                                        }
                                        if (_mti.m_mesuoption2 == 0) // 시장가로정정이전에 일괄취소
                                        {
                                            if (!_mti.m_bCanceled)
                                            {
                                                Console.WriteLine(_itemName + " 시장가로 정정 이전 일괄취소 타이머 start");
                                                _mti._orderCancellationTimer.Start();
                                            }
                                        }
                                    }
                                    gMainForm.AddOrderToDataGridView(_itemName, _mti.m_conditionName, _orderNumber, _tradingTime, int.Parse(_orderQnt), int.Parse(_orderPrice), _orderType, _tradingType);
                                }
                                //Console.WriteLine("체잔에서 접수 " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + _itemName + " 주문수량: " + _orderQnt + " 주문가격: " + _orderPrice + " 주문구분: " + _orderType + " 매매구분: " + _tradingType + " 주문번호: " + _orderNumber +" 원주문번호: " + _originOrderNumber + " 체결가: " + _conclusionPrice + " 체결랑: " + _conclusionQnt);
                                //주문번호 저장
                                ProfitLossCalculate _profitLossCalculate = new ProfitLossCalculate(_orderNumber, _mti.m_rePurchaseNumber, "", -1);
                                _mti.m_profitLossCalculateList.Add(_profitLossCalculate);
                                _mti.m_rePurchaseNumber = -1;
                                _mti.m_buyingOrderNumber = _orderNumber; // 매수주문번호 부여(시장가정정, 일괄정정, 일괄취소 위함)
                                _mti.m_orderQnt = int.Parse(_orderQnt); // 주문수량
                                _mti.m_orderReceivedTime = DateTime.Now;
                                if (_mti.m_mesuoption2 == 1) // 일괄정정
                                {
                                    if(_mti._orderModificationTimer != null)
                                    {
                                        Console.WriteLine(_itemName + " 일괄정정 타이머 start");
                                        _mti._orderModificationTimer.Start();
                                    }
                                }
                                found = true;
                                if(int.Parse(_outstandingQnt) ==0) // 시장가 및 지정가로 정정주문
                                {
                                    if(_mti.m_mesuoption2 == 2) // 주문전체 일괄취소시 개수 count 설정
                                    {
                                        foreach (DataGridViewRow row in gMainForm.tradingConditionDataGridView.Rows)
                                        {
                                            string _conditionName = row.Cells["매매조건식_조건식"].Value.ToString();
                                            int _count = 0;
                                            foreach (DataGridViewRow item in gMainForm.tradingItemDataGridView.Rows)
                                            {
                                                string _itemConditionName = item.Cells["매매진행_조건식"].Value.ToString();

                                                if (_conditionName.Equals(_itemConditionName) && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("매도완료") && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("매도중") && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("주문취소") && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("대기중"))
                                                {
                                                    _count++;
                                                }
                                            }
                                            row.Cells["매매조건식_실매수종목수"].Value = _count;
                                        }
                                    }
                                    if(_mti.m_mesuoption2 == 0)
                                    {
                                        Console.WriteLine("종목명: " + _itemName + " 전체 미체결되서 시장가로 정정주문 진행");
                                        CheckUnconcludedMarketOrders();
                                    }
                                }
                                //Console.WriteLine(_itemName + " 주문받은시각: " + _mti.m_orderReceivedTime);
                                //Console.WriteLine(_itemName + " 매수접수 m_rePurchaseNumber: " + _mti.m_rePurchaseNumber);
                            }
                            else if (_orderType.Equals("매수취소"))
                            {
                                //orderDataGridView에 추가해 준다.
                                gMainForm.AddOrderToDataGridView(_itemName, _mti.m_conditionName, _orderNumber, _tradingTime, int.Parse(_orderQnt), int.Parse(_orderPrice), _orderType, _tradingType);
                                //Console.WriteLine(_itemName + " 주문취소 접수완료" + " 체결가: " + _conclusionPrice + " 미체결수량: " + _outstandingQnt + " 주문수량: " + _orderQnt);
                            }
                            else if(_orderType.Equals("매수정정"))
                            {
                                //주문번호 저장
                                ProfitLossCalculate _profitLossCalculate = new ProfitLossCalculate(_orderNumber, _mti.m_rePurchaseNumber, "", -1);
                                _mti.m_profitLossCalculateList.Add(_profitLossCalculate);
                                _mti.m_rePurchaseNumber = -1;
                                _mti.m_modifyOrderNumber = _orderNumber; // 매수주문번호 부여(시장가정정, 일괄정정, 일괄취소 위함)
                                //_mti.m_orderQnt = int.Parse(_orderQnt); // 주문수량
                                
                                Console.WriteLine("매수정정 " + _itemName + " 주문수량개수: " + _orderQnt + " 미체결수량: " + _outstandingQnt + " 주문번호: " + _orderNumber + " 원주문번호: " + _originOrderNumber + " 주문가격: " + _orderPrice);
                                //orderDataGridView에 추가해 준다.
                                gMainForm.AddOrderToDataGridView(_itemName, _mti.m_conditionName, _orderNumber, _tradingTime, int.Parse(_orderQnt), int.Parse(_orderPrice), _orderType, _tradingType);
                            }
                            else if (_orderType.Equals("매도"))
                            {
                                //orderDataGridView에 추가해 준다.
                                gMainForm.AddOrderToDataGridView(_itemName, _mti.m_conditionName, _orderNumber, _tradingTime, int.Parse(_orderQnt), int.Parse(_orderPrice), _orderType, _tradingType);
                                //주문번호 및 평균단가 저장
                                //_mti.m_sellOrderNumber = _orderNumber; //sellordernumber를 사용안함
                                ProfitLossCalculate profitLossCalculate = new ProfitLossCalculate("", -1, _orderNumber, (double)_mti.m_totalPurchaseAmount / (double)_mti.m_totalQnt);
                                _mti.m_profitLossCalculateList.Add(profitLossCalculate);
                                _mti.m_tsMedoNumber = -1;
                                _mti.m_orderQnt = _mti.m_totalQnt - int.Parse(_orderQnt);
                                //Console.WriteLine("종목명: " + _itemName + " 전체보유수량에서 주문수량만큼 뺀 갯수: " + _mti.m_orderQnt);
                            }
                            else if (_orderType.Equals("매도취소"))
                            {

                            }
                        }
                    }
                    if(!found)
                    {
                        //보유매도 기준
                        foreach (MyHoldingItemOption _mho in gMainForm.gMyHoldingItemOptionList) // 조건식데이터 검색
                        {
                            MyHoldingItem _mhi = _mho.MyHoldingItemList.Find(o => o.m_itemCode.Equals(_itemCode));
                            {
                                if (_mhi != null)
                                {
                                    if (_orderType.Equals("매수"))
                                    {
                                        //orderDataGridView에 추가해 준다.
                                        gMainForm.AddOrderToDataGridView(_itemName, "보유", _orderNumber, _tradingTime, int.Parse(_orderQnt), int.Parse(_orderPrice), _orderType, _tradingType);
                                        //Console.WriteLine("체잔에서 접수 " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + _itemName + " 주문수량: " + _orderQnt + " 주문가격: " + _orderPrice + " 주문구분: " + _orderType + " 매매구분: " + _tradingType + " 주문번호: " + _orderNumber + " 원주문번호: " + _originOrderNumber + " 체결가: " + _conclusionPrice + " 체결랑: " + _conclusionQnt);
                                        //주문번호 저장
                                        ProfitLossCalculate _profitLossCalculate = new ProfitLossCalculate(_orderNumber, _mhi.m_rePurchaseNumber, "", -1);
                                        _mhi.m_profitLossCalculateList.Add(_profitLossCalculate);
                                        _mhi.m_rePurchaseNumber = -1;
                                        _mhi.m_buyingOrderNumber = _orderNumber; // 매수주문번호 부여(시장가정정, 일괄정정, 일괄취소 위함)
                                        _mhi.m_orderQnt = int.Parse(_orderQnt); // 주문수량
                                                                                //_mhi.m_orderReceivedTime = DateTime.Now;
                                                                                //_orderCheckTimer.Start(); // 타이머실행(미체결 상태 감지를 위해) 
                                                                                //Console.WriteLine(_itemName + " 주문받은시각: " + _mti.m_orderReceivedTime);
                                                                                //Console.WriteLine(_itemName + " 매수접수 m_rePurchaseNumber: " + _mti.m_rePurchaseNumber);
                                        found = true;
                                    }
                                    else if (_orderType.Equals("매수취소"))
                                    {
                                        //orderDataGridView에 추가해 준다.
                                        gMainForm.AddOrderToDataGridView(_itemName, "보유", _orderNumber, _tradingTime, int.Parse(_orderQnt), int.Parse(_orderPrice), _orderType, _tradingType);
                                    }
                                    else if (_orderType.Equals("매도"))
                                    {
                                        //orderDataGridView에 추가해 준다.
                                        gMainForm.AddOrderToDataGridView(_itemName, "보유", _orderNumber, _tradingTime, int.Parse(_orderQnt), int.Parse(_orderPrice), _orderType, _tradingType);
                                        //주문번호 및 평균단가 저장
                                        //_mti.m_sellOrderNumber = _orderNumber; //sellordernumber를 사용안함
                                        ProfitLossCalculate profitLossCalculate = new ProfitLossCalculate("", -1, _orderNumber, (double)_mhi.m_totalPurchaseAmount / (double)_mhi.m_totalQnt);
                                        _mhi.m_profitLossCalculateList.Add(profitLossCalculate);
                                        _mhi.m_tsMedoNumber = -1;
                                    }
                                    else if (_orderType.Equals("매도취소"))
                                    {

                                    }
                                }
                            }
                        }
                    }
                    if(!found)
                    {
                        if (_orderType.Equals("매수"))
                        {
                            PassitiveBuyingItem passitiveBuyingItem = new PassitiveBuyingItem(_itemCode, _orderNumber);
                            gMainForm.gMyPassitiveBuyingItemList.Add(passitiveBuyingItem);
                            PassitiveBuyingItem _pbi = gMainForm.gMyPassitiveBuyingItemList.Find(o => o.itemCode.Equals(_itemCode));

                            _currentPrice = _currentPrice.Replace("+", " ").Replace("-", " ");

                            if(_pbi.itemCode.Equals(_itemCode) && !_pbi.buyingcheck)
                            {
                                int rowIndex = gMainForm.tradingItemDataGridView.Rows.Add(); // 그리드뷰에 한 줄이 추가됨

                                gMainForm.tradingItemDataGridView["매매진행_구분", rowIndex].Value = "영웅문매수"; // 조건식
                                gMainForm.tradingItemDataGridView["매매진행_종목명", rowIndex].Value = _itemName; // 종목명
                                gMainForm.tradingItemDataGridView["매매진행_종목코드", rowIndex].Value = _itemCode; // 종목코드
                                gMainForm.tradingItemDataGridView["매매진행_조건식", rowIndex].Value = "영웅문매수"; // 조건식
                                //gMainForm.tradingItemDataGridView["매매진행_총투자금", rowIndex].Value = _itemInvestment.ToString("N0"); // 총투자금
                                //gMainForm.tradingItemDataGridView["매매진행_편입가격", rowIndex].Value = mti.m_currentPrice.ToString("N0"); // 편입가격
                                //gMainForm.tradingItemDataGridView["매매진행_편입대비수익률", rowIndex].Value = "0.00%"; // 편입대비수익률
                                gMainForm.tradingItemDataGridView["매매진행_현재가", rowIndex].Value = int.Parse(_currentPrice).ToString("N0"); // 현재가
                                gMainForm.tradingItemDataGridView["매매진행_매입금", rowIndex].Value = "0"; // 매입금
                                gMainForm.tradingItemDataGridView["매매진행_보유수량", rowIndex].Value = "0"; // 보유수량            
                                //gMainForm.tradingItemDataGridView["매매진행_주문가능수량", rowIndex].Value = "0"; // 주문가능수량
                                gMainForm.tradingItemDataGridView["매매진행_매입가", rowIndex].Value = "0"; // 매입가
                                gMainForm.tradingItemDataGridView["매매진행_평가손익", rowIndex].Value = "0"; // 평가손익
                                gMainForm.tradingItemDataGridView["매매진행_수익률", rowIndex].Value = "0.00%"; // 수익률
                                gMainForm.tradingItemDataGridView["매매진행_진행상황", rowIndex].Value = "매수완료"; // 진행상황
                                gMainForm.tradingItemDataGridView["매매진행_등락율", rowIndex].Value = "0.00%"; // 등락율

                                _pbi.buyingcheck = true;

                                Task _task = new Task(() =>
                                {
                                    string fidList = "10;11;12;13;15;20;228;302;9001";
                                    gMainForm.KiwoomAPI.SetRealReg(gMainForm.GetScreenNumber(), _itemCode, fidList, "1");
                                    //gMainForm.setLogText("영웅문 매수 종목 실시간 감시 요청 : " + _itemName);
                                });
                                gMainForm.gCheckRequestManager.sendTaskData(_task);

                                Console.WriteLine("영웅문매수 접수: " + _itemName + " 체결가: " + _conclusionPrice + " 체결량: " + _conclusionQnt + " 주문번호: " + _orderNumber);
                            }



                        }
                        else if (_orderType.Equals("매도"))
                        {

                        }

                    }
                }
                else if(_orderState.Equals("체결")) // 접수된 종목이 체결이 되는 경우
                {
                    //Console.WriteLine("체결: " + _itemName + " ordertype: " + _orderType);
                    //체결될때마다 체결된 내용을 condlusionDataGridView에 추가
                    if (!string.IsNullOrWhiteSpace(_conclusionPrice))
                    {
                        gMainForm.AddConclusionToDataGridView(_itemName, _orderNumber, _tradingTime, int.Parse(_orderQnt), int.Parse(_conclusionQnt), int.Parse(_unitConclusionQnt), int.Parse(_conclusionPrice), _orderType);
                    }
                    //Console.WriteLine(_itemName + ": int.Parse(_orderQnt): " + int.Parse(_orderQnt) + " int.Parse(_conclusionQnt): " + int.Parse(_conclusionQnt) + " int.Parse(_unitConclusionQnt): " + int.Parse(_unitConclusionQnt) + " int.Parse(_conclusionPrice: " + int.Parse(_conclusionPrice));
                    //Console.WriteLine("체결데이터수신 : " + _itemName + " 미체결수량: " + _outstandingQnt + " 주문타입: " + _orderType + " 체결가: " + _conclusionPrice);

                    //체결될때마다 평가손익 계산
                    if (_orderType.Contains("매도"))
                    {
                        //조건식
                        foreach (MyTradingCondition _mtc in gMainForm.gMyTradingConditionList) //조건식데이터검색
                        {
                            MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(_itemCode)); // 종목코드가 동일한 종목을 찾아낸다
                            if(_mti != null)
                            {
                                ProfitLossCalculate _plc = _mti.m_profitLossCalculateList.Find(o => o.m_sellOrderNumber.Equals(_orderNumber)); //매도주문번호가 같은 것을 찾아냄
                                if(_plc != null)
                                {
                                    //매도체결이 될때마다 평가손익을 계산한다
                                    double[] _ret = {};
                                    _ret = GetRateOfReturnAndEvaluationProfitLoss(_mti, _plc.m_realAveragePrice, int.Parse(_unitConclusionQnt), double.Parse(_unitConclusionPrice));
                                    // 단위체결가로 수익계산
                                    _plc.m_totalEvaluationProfitLoss += _ret[1];
                                    gMainForm.AddsoldToDataGridview(_itemName, _tradingTime, int.Parse(_unitConclusionQnt), double.Parse(_unitConclusionPrice), _ret[0], _ret[1]);
                                    break;
                                }
                            }
                        }
                        //보유매도
                        foreach(MyHoldingItemOption _mho in gMainForm.gMyHoldingItemOptionList) // 보유매도 설정검색
                        {
                            MyHoldingItem _mhi = _mho.MyHoldingItemList.Find(o => o.m_itemCode.Equals(_itemCode));
                            {
                                if(_mhi != null)
                                {
                                    ProfitLossCalculate _plc = _mhi.m_profitLossCalculateList.Find(o => o.m_sellOrderNumber.Equals(_orderNumber)); //매도주문번호가 같은 것을 찾아냄
                                    if (_plc != null)
                                    {
                                        //Console.WriteLine(_itemName + "익절매도 차수: " + _mhi.m_currentTakeProfitStep + " 손절매도 차수: " + _mhi.m_currentStopLossStep);
                                        //매도체결이 될때마다 평가손익을 계산한다
                                        double[] _ret = { };
                                        _ret = GetRateOfReturnAndEvaluationProfitLossHoldingItem(_mhi, _plc.m_realAveragePrice, int.Parse(_unitConclusionQnt), double.Parse(_unitConclusionPrice));
                                        // 단위체결가로 수익계산
                                        _plc.m_totalEvaluationProfitLoss += _ret[1];
                                        gMainForm.AddsoldToDataGridview(_itemName, _tradingTime, int.Parse(_unitConclusionQnt), double.Parse(_unitConclusionPrice), _ret[0], _ret[1]);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if(_orderType.Equals("매수"))
                    {
                        //Console.WriteLine("체잔에서 매수체결 " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + _itemName + " 주문수량: " + _orderQnt + " 주문가격: " + _orderPrice + " 주문구분: " + _orderType + " 매매구분: " + _tradingType + " 주문번호: " + _orderNumber + " 원주문번호: " + _originOrderNumber + " 체결가: " + _conclusionPrice + " 단위체결가: " + _unitConclusionPrice + " 체결랑: " + _conclusionQnt + " 단위체결량: " + _unitConclusionQnt);
                        foreach (MyTradingCondition _mtc in gMainForm.gMyTradingConditionList)//조건식데이터검색
                        {
                            MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(_itemCode)); // 종목코드가 동일한 종목을 찾아낸다
                            if (_mti != null)
                            {
                                _mti.m_outstandingQnt = int.Parse(_outstandingQnt);
                                //sendRequestItemOutstandingInfoTR(_mtc.account, "1" , "2", _mti.m_itemCode,"0", "0");
                                //Console.WriteLine("종목명: "+ _itemName + " 주문수량: " + _orderQnt + " 체결수량: " + _conclusionQnt + " 미체결된수량: " + _mti.m_outstandingQnt);
                                Console.WriteLine(_itemName + " 매수체결데이터 수신, 미체결량: " + _outstandingQnt);
                                //Console.WriteLine("매수체결되서 타이머 종료");
                            }
                        }
                        //보유종목
                        foreach (MyHoldingItemOption _mho in gMainForm.gMyHoldingItemOptionList) // 보유매도 설정검색
                        {
                            MyHoldingItem _mhi = _mho.MyHoldingItemList.Find(o => o.m_itemCode.Equals(_itemCode));
                            if (_mhi != null)
                            {
                                //Console.WriteLine(_itemName + " 매수체결데이터 수신, 미체결량: " + _outstandingQnt);
                                //Console.WriteLine("매수체결되서 타이머 종료");
                            }
                        }
                    }

                    //미체결수량(_outstandingQnt)이 0 이면 매수, 매도가 끝이 난 상태
                    if(int.Parse(_outstandingQnt) == 0)
                    {
                        if (_orderType.Equals("매수"))
                        {
                            //Console.WriteLine("체잔에서 미체결0일때 매수체결 " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + _itemName + " 주문수량: " + _orderQnt + " 주문가격: " + _orderPrice + " _orderType: " + _orderType + " 매매구분: " + _tradingType + " 주문번호: " + _orderNumber + " 원주문번호: " + _originOrderNumber + " 체결가: " + _conclusionPrice + " 단위체결가: " + _unitConclusionPrice + " 체결랑: " + _conclusionQnt + " 단위체결량: " + _unitConclusionQnt);
                            bool found = false;

                            /////////////////////////검색식 종목 매수완료//////////////////////////////////////
                            foreach (MyTradingCondition _mtc in gMainForm.gMyTradingConditionList) // 조건식데이터검색
                            {
                                MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(_itemCode)); // 종목코드가 동일한 종목을 찾아낸다
                                if (_mti != null) // 매매중인 종목을 찾은 경우...
                                {
                                    if (!string.IsNullOrWhiteSpace(_conclusionPrice)) // 주문취소시 conclusionprice가 공백으로 올경우 예외처리함
                                    {   /*
                                        if(_mti.m_mesuoption2 == 2)
                                        {
                                            _mti._orderCancellationTimer.Stop();
                                        }
                                        if(_mti.m_mesuoption2 == 1)
                                        {
                                            _mti._orderModificationTimer.Stop();
                                        }
                                        */
                                        //_mti.m_completeConclusion = true;
                                        double mesuPrice = double.Parse(_conclusionPrice);
                                        //ts매도를 위한 주문번호 저장
                                        GoJumMedo goJumMedo = new GoJumMedo(_itemCode, mesuPrice, _orderNumber);
                                        gMainForm.gojumMedoList.Add(goJumMedo);

                                        //Console.WriteLine("체잔에서 미체결0일때 매수체결 " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + _itemName + " 주문수량: " + _orderQnt + " 주문가격: " + _orderPrice + " 주문구분: " + _orderType + " 매매구분: " + _tradingType + " 주문번호: " + _orderNumber + " 원주문번호: " + _originOrderNumber + " 체결가: " + _conclusionPrice + " 단위체결가: " + _unitConclusionPrice + " 체결랑: " + _conclusionQnt + " 단위체결량: " + _unitConclusionQnt);

                                        ProfitLossCalculate _plc = _mti.m_profitLossCalculateList.Find(o => o.m_buyOrderNumber.Equals(_orderNumber)); // 매수주문번호가 동일한 종목을 찾는다.
                                        if (_plc != null)
                                        {
                                            // 매수상태변수를 true로 변경
                                            //_mti.m_bCompletePurchase = true;

                                            // 저장파일에 종목이 없을때만 종목을 저장한다.
                                            if (gMainForm.gFileIOInstance.checkSameItemCode(_itemCode) == -1) // 처음 매수
                                            {
                                                gMainForm.gFileIOInstance.saveItem(_itemCode, _mtc.conditionData.conditionName, _mtc.conditionData.conditionIndex, _mti.m_transferPrice, _mti.getRePurchaseArray(), _mti.m_currentRebuyingStep);
                                                //gMainForm.setLogText("매수 종목 데이타 저장 완료 : " + _itemName);
                                            }
                                            else // 추매
                                            {
                                                gMainForm.gFileIOInstance.editItem(_itemCode, _mtc.conditionData.conditionName, _mtc.conditionData.conditionIndex, _mti.m_transferPrice, _mti.getRePurchaseArray());
                                                //gMainForm.setLogText("추매 종목 데이타 수정 완료 : " + _itemName);
                                            }

                                            // 진행상황 매수완료로 처리
                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                            {
                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode) && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                {
                                                    row.Cells["매매진행_진행상황"].Value = "매수완료";
                                                    row.Cells["매매진행_추매"].Value = _mti.getRePurchaseCount().ToString();
                                                    break;
                                                }
                                            }
                                            // 리스트 삭제
                                            _mti.m_profitLossCalculateList.Remove(_plc);
                                            // 예수금현황
                                            gMainForm.gLoginInstance.getAccountDetailDataTR(_account);
                                            //Console.WriteLine("getAccountDetailDataTR 작동완료");
                                            //Console.WriteLine("상태체크: " + _mti.m_volumeState);
                                            //Console.WriteLine("추매: " + _mti.m_reBuyingType + " 익절: " + _mti.m_takeProfitType + " 손절: " + _mti.m_stopLossType + " true일경우에 추가매수: " + _mti.m_brePurchased);
                                            //Console.WriteLine("_mtc.buyingUsing: " + _mtc.buyingUsing + " _mtc.buyingType: " + _mtc.buyingType + " _mti.m_buyingTransferType: " + _mti.m_buyingTransferType + " _mti.m_rePurchaseArray[0]: " + _mti.m_rePurchaseArray[0]);
                                            // 미체결이 0이고 매수완료가되었는데, 기본매수일경우에만 분봉데이터 요청(기본매수만을위함)
                                            if (_mtc.buyingUsing == 1 && _mtc.buyingType == 0)
                                            {
                                                if (_mti.m_buyingTransferType == 0 && _mti.m_rePurchaseArray[0])
                                                {
                                                    if (_mti.m_reBuyingType != 0 || _mti.m_takeProfitType != 0 || _mti.m_stopLossType != 0)
                                                    {
                                                        // 추가매수 여부에대한 변수를 추가해서 추가매수가 되면 true로 변경되기떄문에 true일경우에는 추가로 분봉요청을 하지않음.
                                                        if (!_mti.m_brePurchased)
                                                        {
                                                            gMainForm.getBunBongData(_itemCode, _mti.m_conditionName, 0, gMainForm.GetScreenNumber());
                                                            //Console.WriteLine("미체결0 매수완료 분봉데이터요청");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        found = true;
                                        gMainForm.setLogText("매수 완료 : " + _itemName);
                                        gMainForm.gFileIOInstance.WriteLogFile(_itemName + " : 매수 완료");
                                        break;
                                    }
                                    if (string.IsNullOrWhiteSpace(_conclusionPrice))
                                    {
                                        if (_mti.m_mesuoption2 == 2)
                                        {
                                            //_mti.m_completeConclusion = true;
                                            _orderCheckTimer.Stop();

                                            //Console.WriteLine("체잔에서 미체결0일때 매수체결 " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + _itemName + " 주문수량: " + _orderQnt + " 주문가격: " + _orderPrice + " 주문구분: " + _orderType + " 매매구분: " + _tradingType + " 주문번호: " + _orderNumber + " 원주문번호: " + _originOrderNumber + " 체결가: " + _conclusionPrice + " 단위체결가: " + _unitConclusionPrice + " 체결랑: " + _conclusionQnt + " 단위체결량: " + _unitConclusionQnt);

                                            ProfitLossCalculate _plc = _mti.m_profitLossCalculateList.Find(o => o.m_buyOrderNumber.Equals(_orderNumber)); // 매수주문번호가 동일한 종목을 찾는다.
                                            if (_plc != null)
                                            {
                                                // 매수상태변수를 true로 변경
                                                //_mti.m_bCompletePurchase = true;

                                                // 저장파일에 종목이 없을때만 종목을 저장한다.
                                                if (gMainForm.gFileIOInstance.checkSameItemCode(_itemCode) == -1) // 처음 매수
                                                {
                                                    gMainForm.gFileIOInstance.saveItem(_itemCode, _mtc.conditionData.conditionName, _mtc.conditionData.conditionIndex, _mti.m_transferPrice, _mti.getRePurchaseArray(), _mti.m_currentRebuyingStep);
                                                    gMainForm.setLogText("매수 종목 데이타 저장 완료 : " + _itemName);
                                                }
                                                else // 추매
                                                {
                                                    gMainForm.gFileIOInstance.editItem(_itemCode, _mtc.conditionData.conditionName, _mtc.conditionData.conditionIndex, _mti.m_transferPrice, _mti.getRePurchaseArray());
                                                    gMainForm.setLogText("추매 종목 데이타 수정 완료 : " + _itemName);
                                                }

                                                // 진행상황 매수완료로 처리
                                                foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                {
                                                    if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode) && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                    {
                                                        row.Cells["매매진행_진행상황"].Value = "매수완료";
                                                        row.Cells["매매진행_추매"].Value = _mti.getRePurchaseCount().ToString();
                                                        break;
                                                    }
                                                }
                                                // 리스트 삭제
                                                _mti.m_profitLossCalculateList.Remove(_plc);
                                                // 예수금현황
                                                gMainForm.gLoginInstance.getAccountDetailDataTR(_account);
                                                //Console.WriteLine("getAccountDetailDataTR 작동완료");
                                                //Console.WriteLine("상태체크: " + _mti.m_volumeState);
                                                //Console.WriteLine("추매: " + _mti.m_reBuyingType + " 익절: " + _mti.m_takeProfitType + " 손절: " + _mti.m_stopLossType + " true일경우에 추가매수: " + _mti.m_brePurchased);
                                                //Console.WriteLine("_mtc.buyingUsing: " + _mtc.buyingUsing + " _mtc.buyingType: " + _mtc.buyingType + " _mti.m_buyingTransferType: " + _mti.m_buyingTransferType + " _mti.m_rePurchaseArray[0]: " + _mti.m_rePurchaseArray[0]);
                                                // 미체결이 0이고 매수완료가되었는데, 기본매수일경우에만 분봉데이터 요청(기본매수만을위함)
                                                if (_mtc.buyingUsing == 1 && _mtc.buyingType == 0)
                                                {
                                                    if (_mti.m_buyingTransferType == 0 && _mti.m_rePurchaseArray[0])
                                                    {
                                                        if (_mti.m_reBuyingType != 0 || _mti.m_takeProfitType != 0 || _mti.m_stopLossType != 0)
                                                        {
                                                            // 추가매수 여부에대한 변수를 추가해서 추가매수가 되면 true로 변경되기떄문에 true일경우에는 추가로 분봉요청을 하지않음.
                                                            if (!_mti.m_brePurchased)
                                                            {
                                                                gMainForm.getBunBongData(_itemCode, _mti.m_conditionName, 0, gMainForm.GetScreenNumber());
                                                                //Console.WriteLine("미체결0 매수완료 분봉데이터요청");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            found = true;
                                            gMainForm.setLogText("매수 완료 : " + _itemName);
                                            gMainForm.gFileIOInstance.WriteLogFile(_itemName + " : 매수 완료");
                                            Console.WriteLine(_itemName + "미체결 일부취소후 기존 매수한 개수만 매수완료.");
                                            break;
                                        }
                                        if (_mti.m_mesuoption2 == 0)
                                        {
                                            Console.WriteLine("종목명: " + _itemName + " 일부만 체결되서 시장가로 정정주문 진행");
                                            CheckUnconcludedMarketOrders();
                                        }
                                    }
                                }
                            }
                            if (!found)
                            {
                                //////////////////////////보유 종목 매수완료(추매만 해당될듯)//////////////////////
                                foreach (MyHoldingItemOption _mho in gMainForm.gMyHoldingItemOptionList) // 조건식데이터검색
                                {
                                    MyHoldingItem _mhi = _mho.MyHoldingItemList.Find(o => o.m_itemCode.Equals(_itemCode)); // 종목코드가 동일한 종목을 찾아낸다
                                    if (_mhi != null) // 매매중인 종목을 찾은 경우...
                                    {
                                        _mhi.m_completeConclusion = true;
                                        double mesuPrice = double.Parse(_conclusionPrice);
                                        //ts매도를 위한 주문번호 저장
                                        GoJumMedo goJumMedo = new GoJumMedo(_itemCode, mesuPrice, _orderNumber);
                                        gMainForm.gojumMedoList.Add(goJumMedo);

                                        //Console.WriteLine("체잔에서 미체결0일때 매수체결 " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + _itemName + " 주문수량: " + _orderQnt + " 주문가격: " + _orderPrice + " 주문구분: " + _orderType + " 매매구분: " + _tradingType + " 주문번호: " + _orderNumber + " 원주문번호: " + _originOrderNumber + " 체결가: " + _conclusionPrice + " 단위체결가: " + _unitConclusionPrice + " 체결랑: " + _conclusionQnt + " 단위체결량: " + _unitConclusionQnt);

                                        ProfitLossCalculate _plc = _mhi.m_profitLossCalculateList.Find(o => o.m_buyOrderNumber.Equals(_orderNumber)); // 매수주문번호가 동일한 종목을 찾는다.
                                        if (_plc != null)
                                        {
                                            // 매수상태변수를 true로 변경
                                            //_mhi.m_bCompletePurchase = true;

                                            // 진행상황 매수완료로 처리
                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                            {
                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode) && row.Cells["매매진행_구분"].Value.ToString().Equals("보유"))
                                                {
                                                    row.Cells["매매진행_진행상황"].Value = "매수완료";
                                                    row.Cells["매매진행_추매"].Value = _mhi.getRePurchaseCount().ToString();
                                                    break;
                                                }
                                            }
                                            // 리스트 삭제
                                            _mhi.m_profitLossCalculateList.Remove(_plc);
                                            // 예수금현황
                                            gMainForm.gLoginInstance.getAccountDetailDataTR(_account);
                                            //Console.WriteLine("getAccountDetailDataTR 작동완료");
                                            //Console.WriteLine("상태체크: " + _mti.m_volumeState);
                                            //Console.WriteLine("추매: " + _mti.m_reBuyingType + " 익절: " + _mti.m_takeProfitType + " 손절: " + _mti.m_stopLossType + " true일경우에 추가매수: " + _mti.m_brePurchased);
                                            //Console.WriteLine("_mtc.buyingUsing: " + _mtc.buyingUsing + " _mtc.buyingType: " + _mtc.buyingType + " _mti.m_buyingTransferType: " + _mti.m_buyingTransferType + " _mti.m_rePurchaseArray[0]: " + _mti.m_rePurchaseArray[0]);
                                            // 미체결이 0이고 매수완료가되었는데, 기본매수일경우에만 분봉데이터 요청(기본매수만을위함)
                                        }
                                        found = true;
                                        gMainForm.setLogText("보유종목 추가매수 완료 : " + _itemName);
                                        gMainForm.gFileIOInstance.WriteLogFile(_itemName + " : 보유종목 추가매수 완료");
                                        break;
                                    }
                                }
                            }
                            if (!found)
                            {
                                PassitiveBuyingItem _pbi = gMainForm.gMyPassitiveBuyingItemList.Find(o => o.orderNumber.Equals(_orderNumber));
                                if (_pbi != null)
                                {
                                    Console.WriteLine("영웅문 매수 미체결0: " + _pbi.itemCode + " " + _pbi.orderNumber);
                                    Console.WriteLine("영웅문 매수 미체결0: " + _itemName + " 체결가: " + _conclusionPrice + " 체결량: " + _conclusionQnt + " 주문번호: " + _orderNumber);
                                    gMainForm.setLogText("영웅문 수동매수 완료 - " + _itemName);
                                }
                            }
                        }
                        else if (_orderType.Equals("매수정정"))
                        {
                            Console.WriteLine("체결에서 미체결0일때 매수정정 " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + _itemName + " 주문수량: " + _orderQnt + " 주문가격: " + _orderPrice + " _orderType: " + _orderType + " 매매구분: " + _tradingType + " 주문번호: " + _orderNumber + " 원주문번호: " + _originOrderNumber + " 체결가: " + _conclusionPrice + " 단위체결가: " + _unitConclusionPrice + " 체결랑: " + _conclusionQnt + " 단위체결량: " + _unitConclusionQnt);
                            foreach (MyTradingCondition _mtc in gMainForm.gMyTradingConditionList) // 조건식데이터검색
                            {
                                MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(_itemCode)); // 종목코드가 동일한 종목을 찾아낸다
                                if (_mti != null) // 매매중인 종목을 찾은 경우...
                                {
                                    if(_conclusionPrice != null)
                                    {
                                        //_mti.m_completeConclusion = true;
                                        double mesuPrice = double.Parse(_conclusionPrice);
                                        //ts매도를 위한 주문번호 저장
                                        GoJumMedo goJumMedo = new GoJumMedo(_itemCode, mesuPrice, _orderNumber);
                                        gMainForm.gojumMedoList.Add(goJumMedo);

                                        //Console.WriteLine("체잔에서 미체결0일때 매수체결 " + DateTime.Now.ToString("HH:mm:ss") + " 종목명: " + _itemName + " 주문수량: " + _orderQnt + " 주문가격: " + _orderPrice + " 주문구분: " + _orderType + " 매매구분: " + _tradingType + " 주문번호: " + _orderNumber + " 원주문번호: " + _originOrderNumber + " 체결가: " + _conclusionPrice + " 단위체결가: " + _unitConclusionPrice + " 체결랑: " + _conclusionQnt + " 단위체결량: " + _unitConclusionQnt);

                                        ProfitLossCalculate _plc = _mti.m_profitLossCalculateList.Find(o => o.m_buyOrderNumber.Equals(_orderNumber)); // 매수주문번호가 동일한 종목을 찾는다.
                                        Console.WriteLine("종목명: " + _itemName + " 리스트에 저장된 ordernumber: " + _plc.m_buyOrderNumber + " _ordernumber: " + _orderNumber + " 원주문번호: " + _originOrderNumber);
                                        if (_plc != null)
                                        {
                                            // 매수상태변수를 true로 변경
                                            //_mti.m_bCompletePurchase = true;

                                            // 저장파일에 종목이 없을때만 종목을 저장한다.
                                            if (gMainForm.gFileIOInstance.checkSameItemCode(_itemCode) == -1) // 처음 매수
                                            {
                                                gMainForm.gFileIOInstance.saveItem(_itemCode, _mtc.conditionData.conditionName, _mtc.conditionData.conditionIndex, _mti.m_transferPrice, _mti.getRePurchaseArray(), _mti.m_currentRebuyingStep);
                                                gMainForm.setLogText("매수 종목 데이타 저장 완료 : " + _itemName);
                                            }
                                            else // 추매
                                            {
                                                gMainForm.gFileIOInstance.editItem(_itemCode, _mtc.conditionData.conditionName, _mtc.conditionData.conditionIndex, _mti.m_transferPrice, _mti.getRePurchaseArray());
                                                gMainForm.setLogText("추매 종목 데이타 수정 완료 : " + _itemName);
                                            }

                                            // 진행상황 매수완료로 처리
                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                            {
                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode) && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                {
                                                    row.Cells["매매진행_진행상황"].Value = "매수완료";
                                                    row.Cells["매매진행_추매"].Value = _mti.getRePurchaseCount().ToString();
                                                    break;
                                                }
                                            }
                                            // 리스트 삭제
                                            _mti.m_profitLossCalculateList.Remove(_plc);
                                            // 예수금현황
                                            gMainForm.gLoginInstance.getAccountDetailDataTR(_account);
                                            //Console.WriteLine("getAccountDetailDataTR 작동완료");
                                            //Console.WriteLine("상태체크: " + _mti.m_volumeState);
                                            //Console.WriteLine("추매: " + _mti.m_reBuyingType + " 익절: " + _mti.m_takeProfitType + " 손절: " + _mti.m_stopLossType + " true일경우에 추가매수: " + _mti.m_brePurchased);
                                            //Console.WriteLine("_mtc.buyingUsing: " + _mtc.buyingUsing + " _mtc.buyingType: " + _mtc.buyingType + " _mti.m_buyingTransferType: " + _mti.m_buyingTransferType + " _mti.m_rePurchaseArray[0]: " + _mti.m_rePurchaseArray[0]);
                                            // 미체결이 0이고 매수완료가되었는데, 기본매수일경우에만 분봉데이터 요청(기본매수만을위함)
                                            if (_mtc.buyingUsing == 1 && _mtc.buyingType == 0)
                                            {
                                                if (_mti.m_buyingTransferType == 0 && _mti.m_rePurchaseArray[0])
                                                {
                                                    if (_mti.m_reBuyingType != 0 || _mti.m_takeProfitType != 0 || _mti.m_stopLossType != 0)
                                                    {
                                                        // 추가매수 여부에대한 변수를 추가해서 추가매수가 되면 true로 변경되기떄문에 true일경우에는 추가로 분봉요청을 하지않음.
                                                        if (!_mti.m_brePurchased)
                                                        {
                                                            gMainForm.getBunBongData(_itemCode, _mti.m_conditionName, 0, gMainForm.GetScreenNumber());
                                                            //Console.WriteLine("미체결0 매수완료 분봉데이터요청");
                                                        }
                                                    }
                                                }
                                            }
                                            gMainForm.setLogText("매수 완료 : " + _itemName);
                                            gMainForm.gFileIOInstance.WriteLogFile(_itemName + " : 매수 완료");
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (_orderType.Contains("매도"))
                        {
                            //////////////////////////검색식 종목 매도완료//////////////////////
                            foreach (MyTradingCondition _mtc in gMainForm.gMyTradingConditionList) // 조건식 데이터 검색
                            {
                                MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(_itemCode)); // 종목코드가 동일한 종목을 찾아낸다
                                if (_mti != null)// 매매중인 종목을 찾은 경우...
                                {
                                    ProfitLossCalculate _plc = _mti.m_profitLossCalculateList.Find(o => o.m_sellOrderNumber.Equals(_orderNumber)); //주문번호로 한번더 체크
                                    if (_plc != null)
                                    {/*
                                        //진행상황 매도 완료로 처리
                                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                        {
                                            if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode)
                                                && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                            {
                                                row.Cells["매매진행_진행상황"].Value = "매도완료";
                                                break;
                                            }
                                        }*/

                                        // 주문가능수량이 0일때 종목을 저장한다.
                                        if (_mti.m_orderAvailableQnt == 0)
                                        {
                                            /*gMainForm.gFileIOInstance.deleteItem(_itemCode);
                                            gMainForm.setLogText("종목 데이타 삭제 완료 : " + _itemName); */
                                        }

                                        //수익률, 평가손익 매도데이터그리드뷰에 추가
                                        //수익률 계산
                                        //Console.WriteLine(_itemName + " 미체결0시 체결가: " + _conclusionPrice);
                                        double _rateOfReturn = _plc.m_totalEvaluationProfitLoss / (_plc.m_realAveragePrice * double.Parse(_conclusionQnt)) * 100;
                                        gMainForm.AddsoldToDataGridview(_itemName, _tradingTime, int.Parse(_conclusionQnt), int.Parse(_conclusionPrice), _rateOfReturn, (double)_plc.m_totalEvaluationProfitLoss);

                                        //종목데이터그리드뷰에 수익률, 평가손익 수정
                                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                        {
                                            //아이템 코드가 같을 경우
                                            if (row.Cells["매매진행_종목코드"].Value.ToString().Contains(_itemCode))
                                            {
                                                //수익률
                                                row.Cells["매매진행_수익률"].Value = _rateOfReturn.ToString("N2") + "%";
                                                //평가손익
                                                row.Cells["매매진행_평가손익"].Value = gMainForm.setUpDownArrow(_plc.m_totalEvaluationProfitLoss);
                                            }
                                        }
                                        Console.WriteLine(_itemName + " 주문 후 미체결 0 매도완료");
                                        _mti.m_isMedoOrderPlaced = false;
                                        _mti.m_bSold = false;
                                        //리스트삭제
                                        _mti.m_profitLossCalculateList.Remove(_plc);
                                        // 예수금현황
                                        gMainForm.gLoginInstance.getAccountDetailDataTR(_account);
                                    }
                                    gMainForm.setLogText("검색식 종목 매도 완료: " + _itemName);
                                    gMainForm.gFileIOInstance.WriteLogFile(_itemName + ": 매도 완료");
                                    break;
                                }
                            }

                            //////////////////////////보유종목 매도완료//////////////////////
                            foreach (MyHoldingItemOption _mho in gMainForm.gMyHoldingItemOptionList) // 조건식 데이터 검색
                            {
                                MyHoldingItem _mhi = _mho.MyHoldingItemList.Find(o => o.m_itemCode.Equals(_itemCode)); // 종목코드가 동일한 종목을 찾아낸다
                                if (_mhi != null)// 매매중인 종목을 찾은 경우...
                                {
                                    Console.WriteLine("보유종목 미체결0매도 수신시 종목명: " + _itemName);
                                    ProfitLossCalculate _plc = _mhi.m_profitLossCalculateList.Find(o => o.m_sellOrderNumber.Equals(_orderNumber)); //주문번호로 한번더 체크
                                    if (_plc != null)
                                    {/*
                                        //진행상황 매도 완료로 처리
                                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                        {
                                            if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode)
                                                && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                            {
                                                row.Cells["매매진행_진행상황"].Value = "매도완료";
                                                break;
                                            }
                                        }*/
                                        //수익률, 평가손익 매도데이터그리드뷰에 추가
                                        //수익률 계산
                                        //Console.WriteLine(_itemName + " 미체결0시 체결가: " + _conclusionPrice);
                                        double _rateOfReturn = _plc.m_totalEvaluationProfitLoss / (_plc.m_realAveragePrice * double.Parse(_conclusionQnt)) * 100;
                                        gMainForm.AddsoldToDataGridview(_itemName, _tradingTime, int.Parse(_conclusionQnt), int.Parse(_conclusionPrice), _rateOfReturn, (double)_plc.m_totalEvaluationProfitLoss);

                                        //종목데이터그리드뷰에 수익률, 평가손익 수정
                                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                        {
                                            //아이템 코드가 같을 경우
                                            if (row.Cells["매매진행_종목코드"].Value.ToString().Contains(_itemCode))
                                            {
                                                //수익률
                                                row.Cells["매매진행_수익률"].Value = _rateOfReturn.ToString("N2") + "%";
                                                //평가손익
                                                row.Cells["매매진행_평가손익"].Value = gMainForm.setUpDownArrow(_plc.m_totalEvaluationProfitLoss);
                                            }
                                        }
                                        //Console.WriteLine(_itemName + "_mti.m_currentTsmedoStep: " + _mti.m_currentTsmedoStep);
                                        _mhi.m_bSold = false;
                                        //리스트삭제
                                        _mhi.m_profitLossCalculateList.Remove(_plc);
                                        // 예수금현황
                                        gMainForm.gLoginInstance.getAccountDetailDataTR(_account);
                                    }
                                    gMainForm.setLogText("보유종목 매도 완료: " + _itemName);
                                    gMainForm.gFileIOInstance.WriteLogFile(_itemName + ": 매도 완료");
                                    break;
                                }
                            }
                        }
                    }
                }

            }
            else if (e.sGubun.Equals("1")) //국내주식 잔고 변경
            {
                string _itemName = gMainForm.KiwoomAPI.GetChejanData(302).Trim(); // 종목명
                string _balanceQnt = gMainForm.KiwoomAPI.GetChejanData(930); // 보유수량
                string _buyingPrice = gMainForm.KiwoomAPI.GetChejanData(931); // 매입단가
                string _totalPurchasePrice = gMainForm.KiwoomAPI.GetChejanData(932); // 총매입가
                string _orderAvailableQnt = gMainForm.KiwoomAPI.GetChejanData(933); // 주문가능수량
                string _profitRate = gMainForm.KiwoomAPI.GetChejanData(8019);//손익률
                string _itemCode = gMainForm.KiwoomAPI.GetChejanData(9001).Replace("A", ""); //종목코드
                string _orderNumber = gMainForm.KiwoomAPI.GetChejanData(9203); // 주문번호
                string _currentPrice = gMainForm.KiwoomAPI.GetChejanData(10); // 현재가
                string _buyAndSell = gMainForm.KiwoomAPI.GetChejanData(946); // 매도 / 매수구분

                bool found = false;

                //Console.WriteLine("_mtc검색전 조건식종목 잔고변경: " + _itemName + " 매입량: " + _balanceQnt + " 주문가능수량: " + _orderAvailableQnt + " 매도/매수구분: " + _buyAndSell);


                // 조건검색식 종목데이터, 종목데이터그리드뷰 갱신
                foreach (MyTradingCondition _mtc in gMainForm.gMyTradingConditionList) // 조건식데이터 검색
                {
                    MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(_itemCode)); // 종목코드가 동일한 종목을 찾아낸다
                    if(_mti != null)
                    {
                        _mti.m_averagePrice = double.Parse(_buyingPrice); // 평균단가
                        _mti.m_totalQnt = int.Parse(_balanceQnt); //매입량
                        _mti.m_orderAvailableQnt = int.Parse(_orderAvailableQnt); // 주문가능수량
                        _mti.m_totalPurchaseAmount = int.Parse(_totalPurchasePrice); // 총매입가

                        //Console.WriteLine("조건식종목 잔고변경: " + _itemName + " 매입량: " + _balanceQnt + " 주문가능수량(접수시): " + _mti.m_orderQnt + " 주문가능수량(잔고에서): " + _mti.m_orderAvailableQnt + " 매도/매수구분: " + _buyAndSell + " _mti.m_bSold: " + _mti.m_bSold + " _mti.m_bCompletePurchase: " + _mti.m_bCompletePurchase);

                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                        {
                            if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode) &&
                                row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                            {
                                row.Cells["매매진행_매입금"].Value = _mti.m_totalPurchaseAmount.ToString("N0");
                                row.Cells["매매진행_보유수량"].Value = _mti.m_totalQnt.ToString("N0");
                                row.Cells["매매진행_매입가"].Value = _mti.m_averagePrice.ToString("N0");
                                row.Cells["매매진행_주문가능수량"].Value = _mti.m_orderAvailableQnt.ToString("N0");
                            }
                        }

                        if (_buyAndSell.Equals("2"))
                        {
                            if(_mti.m_orderQnt == _mti.m_totalQnt)
                            {
                                _mti.m_bCompletePurchase = true;
                                _mti.m_completeConclusion = true;
                            }
                        }

                        if (_mti.m_totalQnt == 0)
                        {
                            //진행상황 매도 완료로 처리
                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                            {
                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode)
                                    && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                {
                                    row.Cells["매매진행_진행상황"].Value = "매도완료";
                                    row.Cells["매매진행_즉시매도"].Value = "종목삭제";

                                    gMainForm.gFileIOInstance.deleteItem(_itemCode);
                                    //gMainForm.setLogText("종목 데이타 삭제 완료 : " + _itemName);
                                    // 편입 매수 증가 시킴
                                    _mtc.remainingBuyingItemCount++;
                                    //Console.WriteLine("잔고변경 남은매수갯수: " + _mtc.remainingBuyingItemCount);
                                    _mti.m_bSold = true;
                                }
                            }
                            // 조건식에 실제 매수된 종목 갯수 갱신하기
                            // 조건식그리드뷰에 현재 매매되고 있는 종목갯수저장
                            foreach (DataGridViewRow row in gMainForm.tradingConditionDataGridView.Rows)
                            {
                                string _conditionName = row.Cells["매매조건식_조건식"].Value.ToString();
                                int _count = 0;
                                foreach (DataGridViewRow item in gMainForm.tradingItemDataGridView.Rows)
                                {
                                    string _itemConditionName = item.Cells["매매진행_조건식"].Value.ToString();

                                    if (_conditionName.Equals(_itemConditionName) && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("매도완료") && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("매도중") && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("주문취소") && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("대기중"))
                                    {
                                        _count++;
                                    }
                                }
                                row.Cells["매매조건식_실매수종목수"].Value = _count;
                            }
                        }
                        else if(_mti.m_totalQnt > 0)
                        {
                            foreach (DataGridViewRow row in gMainForm.tradingConditionDataGridView.Rows)
                            {
                                string _conditionName = row.Cells["매매조건식_조건식"].Value.ToString();
                                int _count = 0;
                                foreach (DataGridViewRow item in gMainForm.tradingItemDataGridView.Rows)
                                {
                                    string _itemConditionName = item.Cells["매매진행_조건식"].Value.ToString();

                                    if (_conditionName.Equals(_itemConditionName) && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("매도완료") && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("매도중") && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("주문취소") && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("대기중"))
                                    {
                                        _count++;
                                    }
                                }
                                row.Cells["매매조건식_실매수종목수"].Value = _count;
                            }
                        }
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    //보유종목
                    foreach (MyHoldingItemOption _mho in gMainForm.gMyHoldingItemOptionList) // 조건식데이터 검색
                    {
                        MyHoldingItem _mhi = _mho.MyHoldingItemList.Find(o => o.m_itemCode.Equals(_itemCode)); // 종목코드가 동일한 종목을 찾아낸다
                        if (_mhi != null)
                        {
                            //Console.WriteLine("보유종목 잔고변경: " + _itemName + " 종목코드: " + _itemCode + " 매입단가: " + _buyingPrice + " 매입량: " + _balanceQnt + "  총매입가: " + _totalPurchasePrice);
                            _mhi.m_buyingAvgPrice = double.Parse(_buyingPrice); // 평균단가
                            _mhi.m_totalQnt = int.Parse(_balanceQnt); //매입량
                                                                      //_mhi.m_orderAvailableQnt = int.Parse(_orderAvailableQnt); // 주문가능수량
                            _mhi.m_totalPurchaseAmount = int.Parse(_totalPurchasePrice); // 총매입가

                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                            {
                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode) &&
                                    row.Cells["매매진행_구분"].Value.ToString().Equals("보유"))
                                {
                                    row.Cells["매매진행_매입금"].Value = _mhi.m_totalPurchaseAmount.ToString("N0");
                                    row.Cells["매매진행_보유수량"].Value = _mhi.m_totalQnt.ToString("N0");
                                    row.Cells["매매진행_매입가"].Value = _mhi.m_buyingAvgPrice.ToString("N0");
                                    //row.Cells["매매진행_주문가능수량"].Value = _mti.m_orderAvailableQnt.ToString("N0");
                                }
                            }

                            if (_mhi.m_totalQnt == 0)
                            {
                                //진행상황 매도 완료로 처리
                                foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                {
                                    if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode)
                                        && row.Cells["매매진행_구분"].Value.ToString().Equals("보유"))
                                    {
                                        row.Cells["매매진행_진행상황"].Value = "매도완료";
                                        row.Cells["매매진행_즉시매도"].Value = "종목삭제";
                                        // 편입 매수 증가 시킴
                                        //_mtc.remainingBuyingItemCount++;
                                        //Console.WriteLine("잔고변경 남은매수갯수: " + _mtc.remainingBuyingItemCount);
                                        _mhi.m_bSold = true;
                                    }
                                }
                                /*
                                // 조건식에 실제 매수된 종목 갯수 갱신하기
                                // 조건식그리드뷰에 현재 매매되고 있는 종목갯수저장
                                foreach (DataGridViewRow row in gMainForm.tradingConditionDataGridView.Rows)
                                {
                                    string _conditionName = row.Cells["매매조건식_조건식"].Value.ToString();
                                    int _count = 0;
                                    foreach (DataGridViewRow item in gMainForm.tradingItemDataGridView.Rows)
                                    {
                                        string _itemConditionName = item.Cells["매매진행_조건식"].Value.ToString();

                                        if (_conditionName.Equals(_itemConditionName) && item.Cells["매매진행_진행상황"].Value.ToString().Equals("매수완료"))
                                        {
                                            _count++;
                                        }
                                    }
                                    row.Cells["매매조건식_실매수종목수"].Value = _count;
                                }*/
                            }
                            /*
                            else if (_mhi.m_totalQnt > 0)
                            {
                                foreach (DataGridViewRow row in gMainForm.tradingConditionDataGridView.Rows)
                                {
                                    string _conditionName = row.Cells["매매조건식_조건식"].Value.ToString();
                                    int _count = 0;
                                    foreach (DataGridViewRow item in gMainForm.tradingItemDataGridView.Rows)
                                    {
                                        string _itemConditionName = item.Cells["매매진행_조건식"].Value.ToString();

                                        if (_conditionName.Equals(_itemConditionName) && item.Cells["매매진행_진행상황"].Value.ToString().Equals("매수완료"))
                                        {
                                            _count++;
                                        }
                                    }
                                    row.Cells["매매조건식_실매수종목수"].Value = _count;
                                }
                            }*/
                            found = true;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    PassitiveBuyingItem _pbi = gMainForm.gMyPassitiveBuyingItemList.Find(o => o.itemCode.Equals(_itemCode));
                    if (_pbi != null)
                    {
                        _pbi.totalPurchasePrice = int.Parse(_totalPurchasePrice); // 매입금
                        _pbi.balanceQnt = int.Parse(_balanceQnt); // 보유수량
                        _pbi.buyingAvgPrice = double.Parse(_buyingPrice); // 매입가

                        foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                        {

                            if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode)
                                && row.Cells["매매진행_구분"].Value.ToString().Equals("영웅문매수"))
                            {
                                row.Cells["매매진행_매입금"].Value = _pbi.totalPurchasePrice.ToString("N0"); // 매입금
                                row.Cells["매매진행_보유수량"].Value = _pbi.balanceQnt.ToString("N0"); // 보유수량            
                                row.Cells["매매진행_매입가"].Value = _pbi.buyingAvgPrice.ToString("N0"); // 매입가
                            }
                        }

                        Console.WriteLine("영웅문 매수 잔고 변경: " + _itemName + " 매입금액: " + _totalPurchasePrice + " 보유수량: " + _balanceQnt + " 매입가: " + _buyingPrice + " 현재가: " + _currentPrice);

                        if (_pbi.balanceQnt == 0)
                        {
                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                            {
                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode)
                                    && row.Cells["매매진행_구분"].Value.ToString().Equals("영웅문매수"))
                                {
                                    row.Cells["매매진행_진행상황"].Value = "매도완료";
                                    row.Cells["매매진행_즉시매도"].Value = "종목삭제";
                                    // 편입 매수 증가 시킴
                                    //_mtc.remainingBuyingItemCount++;
                                    //Console.WriteLine("잔고변경 남은매수갯수: " + _mtc.remainingBuyingItemCount);
                                    _pbi.bsold = true;
                                }
                            }
                        }
                    }

                }
            }
            else if (e.sGubun.Equals("4")) //파생 잔고 변경
            {

            }
        }
        public void CheckUnconcludedCancelOrders() // 미체결주문에 대한 취소
        {
            // 주식 주문에서 정정주문이나 취소주문은 주문수량을 0으로 지정하면 전량 정정 혹은 취소 주문할수있습니다.
            foreach (MyTradingCondition _mtc in gMainForm.gMyTradingConditionList) // 조건식데이터 검색
            {
                foreach(MyTradingItem _mti in _mtc.MyTradingItemList)
                {
                    if (_mti._orderCancellationTimer != null)
                    {
                        if (_mti.m_itemCode != null && !_mti.m_bCompletePurchase) // 매수가 완료되지않은경우
                        {
                            if (_mti.m_mesuoption1 == 1 && !_mti.m_bRetryOrder) // 지정가 + 정정주문이 들어갔는지 여부가 false;
                            {
                                //Console.WriteLine(DateTime.Now.ToString("HHmmss") + " 종목명: " + _mti.m_itemName + " 체결여부: " + _mti.m_completeConclusion + " 주문들어간시간: " + _mti.m_orderReceivedTime);
                                if (!_mti.m_completeConclusion && _mti.m_orderReceivedTime != DateTime.MinValue)
                                {
                                    double elapsedSeconds = (DateTime.Now - _mti.m_orderReceivedTime).TotalSeconds;

                                    if (elapsedSeconds >= _mti.m_nontradingtime)
                                    {
                                        if (_mti.m_mesuoption2 == 2 || _mti.m_mesuoption2 == 0) // 일괄취소
                                        {
                                            if (!_mti.m_bCanceled)
                                            {
                                                Task _buyCancelTask = new Task(() =>
                                                {
                                                    long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                "일괄취소",
                                                gMainForm.GetScreenNumber(),
                                                _mtc.account,
                                                3,
                                                _mti.m_itemCode,
                                                0,
                                                0,
                                                "00",
                                                _mti.m_buyingOrderNumber
                                                );
                                                    Console.WriteLine(_mti.m_itemName + " 주문취소 시작 " + " elapsedSeconds: " + elapsedSeconds + " _mti.m_nontradingtime: " + _mti.m_nontradingtime);


                                                    if (_ret == 0) //성공
                                                    {
                                                        //Console.WriteLine(_mtc.conditionData.conditionName + " " + _mti.m_itemName + " 일괄취소 주문 성공");
                                                        if (_mti._orderCancellationTimer != null)
                                                        {
                                                            _mti._orderCancellationTimer.Stop();
                                                            _mti._orderCancellationTimer.Dispose();
                                                            _mti._orderCancellationTimer = null;
                                                            Console.WriteLine("일괄취소 타이머 종료및 데이터 삭제");

                                                            _mti.m_bCanceled = true;
                                                            //Console.WriteLine("일괄취소 성공하여 타이머 종료");
                                                            // 더이상 매수가 안되도록 _mti.m_bPurchased를 true로 설정한다.
                                                            //_mti.m_rePurchaseArray[0] = true;
                                                            //_mti.m_bPurchased = true;
                                                            //Console.WriteLine(_itemName + " 일반매수주문성공 m_rePurchaseNumber: " + _mti.m_rePurchaseNumber);
                                                            _mtc.remainingBuyingItemCount++; // 취소의경우 다시 개수를 더한다.
                                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                            {
                                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_mti.m_itemCode) &&
                                                                    row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName) &&
                                                                    Convert.ToInt32(row.Cells["매매진행_보유수량"].Value) == 0)
                                                                {
                                                                    row.Cells["매매진행_진행상황"].Value = "주문취소";
                                                                    row.Cells["매매진행_즉시매도"].Value = "종목삭제";
                                                                    //매수주문성공시 개수카운트차감
                                                                    //_mtc.remainingBuyingItemCount--;
                                                                    //Console.WriteLine("주문성공 남은매수갯수: " + _mtc.remainingBuyingItemCount);
                                                                    break;
                                                                }
                                                            }
                                                            Console.WriteLine(_mti.m_itemName + " 주문취소 성공");
                                                            gMainForm.setLogText("편입종목 일괄 취소주문 성공: " + _mti.m_itemName);
                                                        }
                                                    }
                                                    else // 실패
                                                    {
                                                        gMainForm.setLogText("편입종목 취소주문실패: " + _mti.m_itemName);
                                                    }
                                                });
                                                gMainForm.gOrderRequestManager.sendTaskData(_buyCancelTask);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void CheckUnconcludedMarketOrders() // 미체결주문 취소 이후 시장가로 정정 주문
        {
            foreach (MyTradingCondition _mtc in gMainForm.gMyTradingConditionList) // 조건식데이터 검색
            {
                foreach (MyTradingItem _mti in _mtc.MyTradingItemList)
                {
                    if (_mti.m_itemCode != null && !_mti.m_bCompletePurchase) // 매수가 완료되지않은경우
                    {
                        if(_mti.m_bCanceled)
                        {
                            if (_mti.m_mesuoption2 == 0 && !_mti.m_bRetryOrder) // 시장가로정정
                            {
                                //Console.WriteLine(_mti.m_itemName + " 시장가로 정정 sendorder 시작");
                                int price = (int)(_mti.m_buyingPerInvestment[0] - _mti.m_totalPurchaseAmount);
                                //Console.WriteLine("남은 매수금액: " + price);
                                int _qnt = (int)((_mti.m_buyingPerInvestment[0] - _mti.m_totalPurchaseAmount) / _mti.m_upperLimitPrice); // 종목당투자금 / 상한가 = 주문수량
                                                                                                                                         //Console.WriteLine("주문수량: " + _qnt);
                                Task _buyMarketPricelTask = new Task(() =>
                                {
                                    long _ret1 = gMainForm.KiwoomAPI.SendOrder(
                                    "시장가로 정정",
                                    gMainForm.GetScreenNumber(),
                                    _mtc.account,
                                    1,
                                    _mti.m_itemCode,
                                    _qnt,
                                    0,
                                    "03",
                                    _mti.m_buyingOrderNumber
                                    );

                                if (_ret1 == 0) //성공
                                {
                                    //Console.WriteLine(_mtc.conditionData.conditionName + " " + _mti.m_itemName + " 시장가로 정정 주문 성공");
                                    // 더이상 매수가 안되도록 _mti.m_bPurchased를 true로 설정한다.
                                    _mti.m_rePurchaseArray[0] = true;
                                    _mti.m_bPurchased = true;
                                    _mti.m_bRetryOrder = true; // 정정주문이 들어갔는지 여부
                                                               //Console.WriteLine("시장가 정정 성공하여 타이머 종료");
                                                               //Console.WriteLine(_itemName + " 일반매수주문성공 m_rePurchaseNumber: " + _mti.m_rePurchaseNumber);
                                    Console.WriteLine(_mti.m_itemName + " 시장가로 정정 sendorder 완료");
                                    foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                    {
                                        if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_mti.m_itemCode) &&
                                            row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                        {
                                            row.Cells["매매진행_진행상황"].Value = "시장가로 정정";
                                            row.Cells["매매진행_즉시매도"].Value = "즉시매도";
                                            //매수주문성공시 개수카운트차감
                                            _mtc.remainingBuyingItemCount--;
                                            //Console.WriteLine("주문성공 남은매수갯수: " + _mtc.remainingBuyingItemCount);
                                            break;
                                        }
                                    }
                                    gMainForm.setLogText("편입종목 시장가로 정정 매수주문 성공: " + _mti.m_itemName);
                                }
                                else // 실패
                                {
                                    gMainForm.setLogText("편입종목 시장가로 정정 매수주문실패: " + _mti.m_itemName);
                                }
                                });
                                gMainForm.gOrderRequestManager.sendTaskData(_buyMarketPricelTask);
                            }
                        }
                    }
                }
            }
        }
        public void CheckUnconcludedDesignationOrders() // 미체결주문 지정가로 정정 주문
        {
            foreach (MyTradingCondition _mtc in gMainForm.gMyTradingConditionList) // 조건식데이터 검색
            {
                foreach (MyTradingItem _mti in _mtc.MyTradingItemList)
                {
                    if (_mti._orderModificationTimer != null)
                    {
                        if (_mti.m_itemCode != null && !_mti.m_bCompletePurchase) // 매수가 완료되지않은경우
                        {
                            if (!_mti.m_completeConclusion && _mti.m_orderReceivedTime != DateTime.MinValue)
                            {
                                double elapsedSeconds = (DateTime.Now - _mti.m_orderReceivedTime).TotalSeconds;

                                if (elapsedSeconds >= _mti.m_nontradingtime)
                                {
                                    if (_mti.m_mesuoption2 == 1 && !_mti.m_bRetryOrder) // 일괄 정정
                                    {
                                        if (_mti._orderModificationTimer != null)
                                        {
                                            _mti._orderModificationTimer.Stop();
                                            _mti._orderModificationTimer.Dispose();
                                            _mti._orderModificationTimer = null;
                                            Console.WriteLine(_mti.m_itemName + " 일괄정정 타이머 종료 및 데이터 삭제");
                                        }

                                        int price = gMainForm.PlusToTick(_mti.m_currentPrice);
                                        Console.WriteLine(_mti.m_itemName + " 일괄정정되는 수량: " + _mti.m_outstandingQnt);
                                        Task _buyLimitPricelTask = new Task(() =>
                                        {
                                            long _ret2 = gMainForm.KiwoomAPI.SendOrder(
                                            "현재가로 정정",
                                            gMainForm.GetScreenNumber(),
                                            _mtc.account,
                                            5,
                                            _mti.m_itemCode,
                                            0,
                                            price,
                                            "00",
                                            _mti.m_buyingOrderNumber
                                            );
                                            Console.WriteLine(_mti.m_itemName + " 현재가로정정 sendorder 시작" + " 원주문가격: " + _mti.m_currentPrice + " 1틱더한주문가격: " + price + " elapsedSeconds: " + elapsedSeconds + " _mti.m_nontradingtime: " + _mti.m_nontradingtime);

                                            if (_ret2 == 0) //성공
                                            {
                                                //Console.WriteLine(_mtc.conditionData.conditionName + " " + _mti.m_itemName + " 현재가로 일괄정정 주문 성공");
                                                // 더이상 매수가 안되도록 _mti.m_bPurchased를 true로 설정한다.
                                                _mti.m_rePurchaseArray[0] = true;
                                                _mti.m_bPurchased = true;
                                                _mti.m_bRetryOrder = true; // 정정주문이 들어갔는지 여부 

                                                //Console.WriteLine("일괄정정 성공하여 타이머 종료");
                                                //Console.WriteLine(_itemName + " 일반매수주문성공 m_rePurchaseNumber: " + _mti.m_rePurchaseNumber);

                                                foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                {
                                                    if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_mti.m_itemCode) &&
                                                        row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                    {
                                                        row.Cells["매매진행_진행상황"].Value = "현재가로 일괄정정";
                                                        row.Cells["매매진행_즉시매도"].Value = "즉시매도";
                                                        //매수주문성공시 개수카운트차감
                                                        //_mtc.remainingBuyingItemCount--;
                                                        //Console.WriteLine("주문성공 남은매수갯수: " + _mtc.remainingBuyingItemCount);
                                                        break;
                                                    }
                                                }
                                                Console.WriteLine(_mti.m_itemName + " 현재가로정정 sendorder 성공" + " 주문가격: " + _mti.m_currentPrice);
                                                gMainForm.setLogText("편입종목 현재가로 일괄정정 매수주문: " + _mti.m_itemName);
                                            }
                                            else // 실패
                                            {
                                                gMainForm.setLogText("편입종목 현재가로 일괄정정 매수주문실패: " + _mti.m_itemName);
                                            }
                                        });
                                        gMainForm.gOrderRequestManager.sendTaskData(_buyLimitPricelTask);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //보유종목 수익률, 총매입, 총평가, 총손익 계산
        private void TotalRateOfReturnAndEvaluationProfitLoss()
        {
            int allTotalPurchaseAmount = 0; // 총매입금액
            int allTotalEvaluationPrice = 0; // 총평가금액
            int allTotalEvaluationProfitLoss = 0; // 총평가손익
            double allRateOfReturn = 0; // 총수익률

            foreach(DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
            {
                //매매완료가 되지않은 종목
                if (!row.Cells["매매진행_진행상황"].Value.Equals("매도완료"))
                {
                    string _itemCode = row.Cells["매매진행_종목코드"].Value.ToString(); // 종목코드
                    long _closingPrice = long.Parse(row.Cells["매매진행_현재가"].Value.ToString().Replace(",", "")); // 현재가
                    double totalPurchaseAmount = double.Parse(row.Cells["매매진행_매입금"].Value.ToString().Replace(",", "")); // 총매입금
                    double totalQnt = double.Parse(row.Cells["매매진행_보유수량"].Value.ToString().Replace(",", "")); // 총매수량
                    double purchaseFee = 0; // 매수가 계산수수료
                    double sellFee = 0; //매도가 계산수수료
                    double itemTax = 0; //제세금

                    if (totalQnt == 0) // 매수된 수량이 없으면
                        continue;

                    string _ret = gMainForm.KiwoomAPI.KOA_Functions("GetStockMarketKind", _itemCode); //  종목코드 입력으로 해당 종목이 어느 시장에 포함되어 있는지 구하는 기능

                    // 매수가계산 수수료
                    if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                        purchaseFee = totalPurchaseAmount * 0.0035;
                    else//실서버
                        purchaseFee = totalPurchaseAmount * 0.00015;
                    purchaseFee = purchaseFee - (purchaseFee % 10);

                    // 매도가계산 수수료
                    if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                        sellFee = _closingPrice * totalQnt * 0.0035;
                    else//실서버
                        sellFee = _closingPrice * totalQnt * 0.00015;
                    sellFee = sellFee - (sellFee % 10);

                    // 제세금
                    if (_ret.Equals("0")) // 코스피
                    {
                        itemTax = _closingPrice * totalQnt * 0.0015;
                        itemTax = itemTax - (itemTax % 1);
                    }
                    else //코스닥
                    {
                        itemTax = _closingPrice * totalQnt * 0.0015;
                        itemTax = itemTax - (itemTax % 1);
                    }

                    //총매입금액 할당
                    allTotalPurchaseAmount += (int)totalPurchaseAmount;
                    //총평가금액 할당
                    allTotalEvaluationPrice += ((int)_closingPrice * (int)totalQnt) - (int)purchaseFee - (int)sellFee - (int)itemTax;
                }
            }

            //총손익 할당
            allTotalEvaluationProfitLoss += (int)allTotalEvaluationPrice - (int)allTotalPurchaseAmount;

            //총매입금액 라벨 바인딩
            gMainForm.totalPurchaseAmountLabel.Text = allTotalPurchaseAmount.ToString("N0");
            //총평가금액 라벨 바인딩
            gMainForm.totalEvalutionAmountLabel.Text = allTotalEvaluationPrice.ToString("N0");
            //총손익 라벨 바인딩
            gMainForm.totalProfitLossLabel.Text = allTotalEvaluationProfitLoss.ToString("N0");

            // 총손익 글자색 변경
            if (allTotalEvaluationProfitLoss < 0)
                gMainForm.totalProfitLossLabel.ForeColor = Color.Blue;
            else if (allTotalEvaluationProfitLoss > 0)
                gMainForm.totalProfitLossLabel.ForeColor = Color.Red;
            else
                gMainForm.totalProfitLossLabel.ForeColor = Color.Black;

            // 총수익률
            if (allTotalPurchaseAmount > 0)
                allRateOfReturn = (double)allTotalEvaluationProfitLoss / allTotalPurchaseAmount * 100;
            else
                allRateOfReturn = 0;

            // 총수익률 라벨 바인딩
            gMainForm.totalRateOfReturnLabel.Text = allRateOfReturn.ToString("N2") + "%";

            // 총수익률 글자색 변경
            if (allRateOfReturn < 0)
                gMainForm.totalRateOfReturnLabel.ForeColor = Color.Blue;
            else if (allRateOfReturn > 0)
                gMainForm.totalRateOfReturnLabel.ForeColor = Color.Red;
            else
                gMainForm.totalRateOfReturnLabel.ForeColor = Color.Black;
        }
        //조건검색식 종목 바인딩
        private void RealRateOfReturnAndEvaluationProfitLoss(MyTradingItem _mti, long _closingPrice, double _updownRate)
        {
            // 종목 그리드뷰에 현재가를 넣어준다.
            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
            {
                // 아이템코드가 같을 경우
                if (row.Cells["매매진행_종목코드"].Value.ToString().Contains(_mti.m_itemCode) 
                    && !row.Cells["매매진행_진행상황"].Value.ToString().Equals("매도완료"))
                {
                    //현재가
                    row.Cells["매매진행_현재가"].Value = _closingPrice.ToString("N0");
                    //등락율
                    row.Cells["매매진행_등락율"].Value = _updownRate.ToString("N2") + "%";
                    //수익률
                    int totalEvaluationPrice = 0; // 평가금액
                    int totalEvalutationProfitLoss = 0; // 평가손익
                    double totalPurchaseAmount = _mti.m_totalPurchaseAmount; // 총매입금
                    double totalQnt = _mti.m_totalQnt; // 총매수량
                    double purchaseFee = 0; // 매수가계산 수수료
                    double sellFee = 0;  //매도가계산 수수료
                    double itemTax = 0; //제세금
                    double rateOfReturn = 0; //수익률

                    //  종목코드 입력으로 해당 종목이 어느 시장에 포함되어 있는지 구하는 기능
                    string _ret = gMainForm.KiwoomAPI.KOA_Functions("GetStockMarketKind", _mti.m_itemCode); 

                    // 매수가계산 수수료
                    if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                        purchaseFee = totalPurchaseAmount * 0.0035;
                    else//실서버
                        purchaseFee = totalPurchaseAmount * 0.00015;
                    purchaseFee = purchaseFee - (purchaseFee % 10);

                    // 매도가계산 수수료
                    if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                        sellFee = _mti.m_currentPrice * totalQnt * 0.0035;
                    else//실서버
                        sellFee = _mti.m_currentPrice * totalQnt * 0.00015;
                    sellFee = sellFee - (sellFee % 10);

                    // 제세금
                    if (_ret.Equals("0")) // 코스피
                    {
                        itemTax = _mti.m_currentPrice * totalQnt * 0.0015;
                        itemTax = itemTax - (itemTax % 1);
                    }
                    else //코스닥
                    {
                        itemTax = _mti.m_currentPrice * totalQnt * 0.0015;
                        itemTax = itemTax - (itemTax % 1);
                    }

                    //평가금액
                    totalEvaluationPrice = ((int)_closingPrice * (int)totalQnt) - (int)purchaseFee - (int)sellFee - (int)itemTax;
                    //평가손익
                    totalEvalutationProfitLoss = (int)totalEvaluationPrice - (int)totalPurchaseAmount;
                    //수익률
                    if (totalPurchaseAmount > 0)
                        rateOfReturn = (double)totalEvalutationProfitLoss / totalPurchaseAmount * 100;
                    else
                        rateOfReturn = 0;
                    // 수익률 변수에 저장
                    _mti.m_rateOfReturn = rateOfReturn;

                    if (totalEvalutationProfitLoss != 0)
                    {
                        row.Cells["매매진행_수익률"].Value = rateOfReturn.ToString("N2") + "%";
                        //Console.WriteLine("수익률: " + row.Cells["매매진행_수익률"].Value);
                        // 평가손익
                        row.Cells["매매진행_평가손익"].Value = gMainForm.setUpDownArrow(totalEvalutationProfitLoss);
                    }
                    break;
                }
            }
        }
        //보유종목
        private void HoldingItemRealRateOfReturnAndEvaluationProfitLoss(MyHoldingItem _mhi, long _closingPrice, double _updownRate)
        {
            /*
            if (_mhi == null)
            {
                Debug.WriteLine("HoldingItemRealRateOfReturnAndEvaluationProfitLoss: _mhi is null.");
                // 로그 남기거나 예외 처리
                return;
            }
            else
            {
                Debug.WriteLine("HoldingItemRealRateOfReturnAndEvaluationProfitLoss: _mhi is not null. _mhi.m_itemCode = " + _mhi.m_itemCode);
            }
            */

            // 종목 그리드뷰에 현재가를 넣어준다.
            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
            {
                // 아이템코드가 같을 경우
                if (//row.Cells["매매진행_종목코드"].Value != null &&
                    //row.Cells["매매진행_진행상황"].Value != null &&
                    row.Cells["매매진행_종목코드"].Value.ToString().Contains(_mhi.m_itemCode)
                    && !row.Cells["매매진행_진행상황"].Value.ToString().Equals("매도완료"))
                {
                    //현재가
                    row.Cells["매매진행_현재가"].Value = _closingPrice.ToString("N0");
                    //등락율
                    row.Cells["매매진행_등락율"].Value = _updownRate.ToString("N2") + "%";
                    //수익률
                    int totalEvaluationPrice = 0; // 평가금액
                    int totalEvalutationProfitLoss = 0; // 평가손익
                    double totalPurchaseAmount = _mhi.m_totalPurchaseAmount; // 총매입금
                    double totalQnt = _mhi.m_totalQnt; // 총매수량
                    double purchaseFee = 0; // 매수가계산 수수료
                    double sellFee = 0;  //매도가계산 수수료
                    double itemTax = 0; //제세금
                    double rateOfReturn = 0; //수익률

                    //Console.WriteLine("보유 종목명: " + _mhi.m_itemName + " 현재가: " + _mhi.m_currentPrice);

                    //  종목코드 입력으로 해당 종목이 어느 시장에 포함되어 있는지 구하는 기능
                    string _ret = gMainForm.KiwoomAPI.KOA_Functions("GetStockMarketKind", _mhi.m_itemCode);

                    // 매수가계산 수수료
                    if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                        purchaseFee = totalPurchaseAmount * 0.0035;
                    else//실서버
                        purchaseFee = totalPurchaseAmount * 0.00015;
                    purchaseFee = purchaseFee - (purchaseFee % 10);

                    // 매도가계산 수수료
                    if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                        sellFee = _mhi.m_currentPrice * totalQnt * 0.0035;
                    else//실서버
                        sellFee = _mhi.m_currentPrice * totalQnt * 0.00015;
                    sellFee = sellFee - (sellFee % 10);

                    // 제세금
                    if (_ret.Equals("0")) // 코스피
                    {
                        itemTax = _mhi.m_currentPrice * totalQnt * 0.0015;
                        itemTax = itemTax - (itemTax % 1);
                    }
                    else //코스닥
                    {
                        itemTax = _mhi.m_currentPrice * totalQnt * 0.0015;
                        itemTax = itemTax - (itemTax % 1);
                    }

                    //평가금액
                    totalEvaluationPrice = ((int)_closingPrice * (int)totalQnt) - (int)purchaseFee - (int)sellFee - (int)itemTax;
                    //평가손익
                    totalEvalutationProfitLoss = (int)totalEvaluationPrice - (int)totalPurchaseAmount;
                    //수익률
                    if (totalPurchaseAmount > 0)
                        rateOfReturn = (double)totalEvalutationProfitLoss / totalPurchaseAmount * 100;
                    else
                        rateOfReturn = 0;
                    // 수익률 변수에 저장
                    _mhi.m_rateOfReturn = rateOfReturn;

                    if (totalEvalutationProfitLoss != 0)
                    {
                        row.Cells["매매진행_수익률"].Value = rateOfReturn.ToString("N2") + "%";
                        //Console.WriteLine("수익률: " + row.Cells["매매진행_수익률"].Value);
                        // 평가손익
                        row.Cells["매매진행_평가손익"].Value = gMainForm.setUpDownArrow(totalEvalutationProfitLoss);
                    }
                    break;
                }
            }
        }
        //수동매수
        private void PassitiveBuyingItemRealRateOfReturnAndEvaluationProfitLoss(PassitiveBuyingItem _pbi, long _closingPrice, double _updownRate)
        {
            // 종목 그리드뷰에 현재가를 넣어준다.
            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
            {
                // 아이템코드가 같을 경우
                if (row.Cells["매매진행_종목코드"].Value.ToString().Contains(_pbi.itemCode)
                    && !row.Cells["매매진행_진행상황"].Value.ToString().Equals("매도완료"))
                {
                    //현재가
                    row.Cells["매매진행_현재가"].Value = _closingPrice.ToString("N0");
                    //등락율
                    row.Cells["매매진행_등락율"].Value = _updownRate.ToString("N2") + "%";
                    //수익률
                    int totalEvaluationPrice = 0; // 평가금액
                    int totalEvalutationProfitLoss = 0; // 평가손익
                    double totalPurchaseAmount = _pbi.totalPurchasePrice; // 총매입금
                    double totalQnt = _pbi.balanceQnt; // 총매수량
                    double purchaseFee = 0; // 매수가계산 수수료
                    double sellFee = 0;  //매도가계산 수수료
                    double itemTax = 0; //제세금
                    double rateOfReturn = 0; //수익률

                    //  종목코드 입력으로 해당 종목이 어느 시장에 포함되어 있는지 구하는 기능
                    string _ret = gMainForm.KiwoomAPI.KOA_Functions("GetStockMarketKind", _pbi.itemCode);

                    // 매수가계산 수수료
                    if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                        purchaseFee = totalPurchaseAmount * 0.0035;
                    else//실서버
                        purchaseFee = totalPurchaseAmount * 0.00015;
                    purchaseFee = purchaseFee - (purchaseFee % 10);

                    // 매도가계산 수수료
                    if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                        sellFee = _pbi.currentPrice * totalQnt * 0.0035;
                    else//실서버
                        sellFee = _pbi.currentPrice * totalQnt * 0.00015;
                    sellFee = sellFee - (sellFee % 10);

                    // 제세금
                    if (_ret.Equals("0")) // 코스피
                    {
                        itemTax = _pbi.currentPrice * totalQnt * 0.0015;
                        itemTax = itemTax - (itemTax % 1);
                    }
                    else //코스닥
                    {
                        itemTax = _pbi.currentPrice * totalQnt * 0.0015;
                        itemTax = itemTax - (itemTax % 1);
                    }

                    //평가금액
                    totalEvaluationPrice = ((int)_closingPrice * (int)totalQnt) - (int)purchaseFee - (int)sellFee - (int)itemTax;
                    //평가손익
                    totalEvalutationProfitLoss = (int)totalEvaluationPrice - (int)totalPurchaseAmount;
                    //수익률
                    if (totalPurchaseAmount > 0)
                        rateOfReturn = (double)totalEvalutationProfitLoss / totalPurchaseAmount * 100;
                    else
                        rateOfReturn = 0;
                    // 수익률 변수에 저장
                    _pbi.rateOfReturn = rateOfReturn;

                    if (totalEvalutationProfitLoss != 0)
                    {
                        row.Cells["매매진행_수익률"].Value = rateOfReturn.ToString("N2") + "%";
                        //Console.WriteLine("수익률: " + row.Cells["매매진행_수익률"].Value);
                        // 평가손익
                        row.Cells["매매진행_평가손익"].Value = gMainForm.setUpDownArrow(totalEvalutationProfitLoss);
                    }
                    break;
                }
            }
        }
        // 현재가와 고점가격을 비교해서 수익률을 계산.
        public double GetRateOfReturnAtHighPrice(MyTradingItem _mti, double realAveragePrice, int qnt, double highPrice)
        {
            double ret = 0;

            // 총매입금액과 총수량
            double totalPurchaseAmount = realAveragePrice * qnt;
            double totalQnt = qnt;

            double purchaseFee = 0; // 매수 시 수수료
            double sellFee = 0;     // 매도 시 수수료
            double itemTax = 0;     // 제세금
            double rateOfReturn = 0; // 수익률(%)

            // 종목이 어느 시장에 속하는지 확인 (예: "0"이면 코스피, 그 외는 코스닥)
            string market = gMainForm.KiwoomAPI.KOA_Functions("GetStockMarketKind", _mti.m_itemCode);

            // 매수 수수료 계산
            if (gMainForm.sServerGubun.Equals("1")) // 모의서버: 0.35%
                purchaseFee = totalPurchaseAmount * 0.0035;
            else // 실서버: 0.015%
                purchaseFee = totalPurchaseAmount * 0.00015;
            purchaseFee = purchaseFee - (purchaseFee % 10); // 10원 단위로 내림

            // 매도 수수료는 고점가격(highPrice)을 기준으로 계산
            if (gMainForm.sServerGubun.Equals("1"))
                sellFee = highPrice * totalQnt * 0.0035;
            else
                sellFee = highPrice * totalQnt * 0.00015;
            sellFee = sellFee - (sellFee % 10);

            // 제세금 계산 (코스피, 코스닥 모두 동일하게 0.15% 적용)
            itemTax = highPrice * totalQnt * 0.0015;
            itemTax = itemTax - (itemTax % 1); // 1원 단위로 내림

            // 평가금액 = (매도가격 * 수량) - (매수수수료 + 매도수수료 + 제세금)
            double totalEvaluationPrice = (highPrice * totalQnt) - purchaseFee - sellFee - itemTax;

            // 평가손익 = 평가금액 - 총매입금액
            double totalEvaluationProfitLoss = totalEvaluationPrice - totalPurchaseAmount;

            // 수익률 = (평가손익 / 총매입금액) * 100
            if (totalPurchaseAmount > 0)
                rateOfReturn = (totalEvaluationProfitLoss / totalPurchaseAmount) * 100;
            else
                rateOfReturn = 0;

            ret = rateOfReturn;

            return ret;
        }

        private double[] GetRateOfReturnAndEvaluationProfitLoss(MyTradingItem _mti, double realAveragePrice, int qnt, double sellPrice)
        {
            double[] _ret = new double[2];

            // 수익률
            double totalEvaluationPrice = 0; // 평가금액
            double totalEvaluationProfitLoss = 0; //평가손익
            double totalPurchaseAmount = realAveragePrice * qnt; // 총매입금
            double totalQnt = qnt; //총매수량
            double purchaseFee = 0; // 매수가계산 수수료
            double sellFee = 0; //매도가계산 수수료
            double itemTax = 0; //제세금
            double rateOfReturn = 0; //수익률

            string _market = gMainForm.KiwoomAPI.KOA_Functions("GetStockMarketKind", _mti.m_itemCode); //  종목코드 입력으로 해당 종목이 어느 시장에 포함되어 있는지 구하는 기능

            // 매수가계산 수수료
            if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                purchaseFee = totalPurchaseAmount * 0.0035;
            else//실서버
                purchaseFee = totalPurchaseAmount * 0.00015;
            purchaseFee = purchaseFee - (purchaseFee % 10);

            // 매도가계산 수수료
            if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                sellFee = _mti.m_currentPrice * totalQnt * 0.0035;
            else//실서버
                sellFee = _mti.m_currentPrice * totalQnt * 0.00015;
            sellFee = sellFee - (sellFee % 10);

            // 제세금
            if (_market.Equals("0")) // 코스피
            {
                itemTax = _mti.m_currentPrice * totalQnt * 0.0015;
                itemTax = itemTax - (itemTax % 1);
            }
            else //코스닥
            {
                itemTax = _mti.m_currentPrice * totalQnt * 0.0015;
                itemTax = itemTax - (itemTax % 1);
            }

            //평가금액
            totalEvaluationPrice = ((int)_mti.m_currentPrice * (int)totalQnt) - (int)purchaseFee - (int)sellFee - (int)itemTax;
            //평가손익
            totalEvaluationProfitLoss = (int)totalEvaluationPrice - (int)totalPurchaseAmount;
            //수익률
            if (totalPurchaseAmount > 0)
                rateOfReturn = (double)totalEvaluationProfitLoss / totalPurchaseAmount * 100;
            else
                rateOfReturn = 0;

            _ret[0] = rateOfReturn; //수익률
            _ret[1] = totalEvaluationProfitLoss; // 평가손익

            return _ret;
        }
        private double[] GetRateOfReturnAndEvaluationProfitLossHoldingItem(MyHoldingItem _mhi, double realAveragePrice, int qnt, double sellPrice)
        {
            double[] _ret = new double[2];

            // 수익률
            double totalEvaluationPrice = 0; // 평가금액
            double totalEvaluationProfitLoss = 0; //평가손익
            double totalPurchaseAmount = realAveragePrice * qnt; // 총매입금
            double totalQnt = qnt; //총매수량
            double purchaseFee = 0; // 매수가계산 수수료
            double sellFee = 0; //매도가계산 수수료
            double itemTax = 0; //제세금
            double rateOfReturn = 0; //수익률

            string _market = gMainForm.KiwoomAPI.KOA_Functions("GetStockMarketKind", _mhi.m_itemCode); //  종목코드 입력으로 해당 종목이 어느 시장에 포함되어 있는지 구하는 기능

            // 매수가계산 수수료
            if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                purchaseFee = totalPurchaseAmount * 0.0035;
            else//실서버
                purchaseFee = totalPurchaseAmount * 0.00015;
            purchaseFee = purchaseFee - (purchaseFee % 10);

            // 매도가계산 수수료
            if (gMainForm.sServerGubun.Equals("1")) // 모의서버
                sellFee = _mhi.m_currentPrice * totalQnt * 0.0035;
            else//실서버
                sellFee = _mhi.m_currentPrice * totalQnt * 0.00015;
            sellFee = sellFee - (sellFee % 10);

            // 제세금
            if (_market.Equals("0")) // 코스피
            {
                itemTax = _mhi.m_currentPrice * totalQnt * 0.0015;
                itemTax = itemTax - (itemTax % 1);
            }
            else //코스닥
            {
                itemTax = _mhi.m_currentPrice * totalQnt * 0.0015;
                itemTax = itemTax - (itemTax % 1);
            }

            //평가금액
            totalEvaluationPrice = ((int)_mhi.m_currentPrice * (int)totalQnt) - (int)purchaseFee - (int)sellFee - (int)itemTax;
            //평가손익
            totalEvaluationProfitLoss = (int)totalEvaluationPrice - (int)totalPurchaseAmount;
            //수익률
            if (totalPurchaseAmount > 0)
                rateOfReturn = (double)totalEvaluationProfitLoss / totalPurchaseAmount * 100;
            else
                rateOfReturn = 0;

            _ret[0] = rateOfReturn; //수익률
            _ret[1] = totalEvaluationProfitLoss; // 평가손익

            return _ret;
        }
        public double getPercentPrice(double price, double per)
        {
            return price + ((price * per) / 100);
        }
        // 분봉 데이터 그리드뷰에 바인딩(종목을 찾아서)
        public void DrawIndicateData(String itemCode)
        {
            foreach (MyTradingCondition _mtc in gMainForm.gMyTradingConditionList)
            {
                // 종목리스트에서 같은 종목 검색
                MyTradingItem mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(itemCode)); // 매매중인 종목 찾음
                if (mti == null) // 같은 종목이 없으면...
                    continue;

                gMainForm.bunBongDataGridView["분봉_상태", 0].Value = mti.m_itemName; // 데이터그리드뷰 '종목명' 부분

                string _str = "";
                string _str2 = "";
                string[] _num = { "1", "3", "5", "10", "15", "30", "45", "60" };
                // 분봉_1,분봉_3,분봉_5,분봉_10,
                //////////////////////////////////////////////////////////// 매수
                if (mti.m_buyingUsing == 1) // 매수사용여부(사용시)
                {
                    if (mti.m_buyingType == 0) ////////////////////////// 기본매수
                    {
                        if (mti.m_buyingTransferType == 0) // 편입대비 즉시매수
                        {
                            _str = "편입즉시매수";
                        }
                        else // 편입대비 n% 매수
                        {
                            _str = "편입대비매수";
                            double _perPrice = getPercentPrice(mti.m_transferPrice, mti.m_buyingTransferPer); // 편입가격, 편입가격대비 매수시% 를 넣은 메소드를 통해 편입대비가격을 계산.
                            if (mti.m_buyingTransferUpdown == 0) // 0은 편입가격대비 하락시 매수 
                            {
                                _str2 = "현재가 < " + _perPrice.ToString("N0");
                            }
                            else
                            {
                                _str2 = "현재가 > " + _perPrice.ToString("N0");
                            }
                        }
                    }
                    else if (mti.m_buyingType == 1) // 이동평균선
                    {
                        double curPrice = 0, prevPrice = 0;
                        _str = "- 이동평균선 매수 -\n";

                        if (mti.m_buyingBongType == 0) // 일봉
                        {
                            if (mti.m_buyingMovePriceKindType == 0)
                            {
                                _str += "일봉 - 단순\n";
                                curPrice = mti.moving_PriceDayCur[0];
                                prevPrice = mti.moving_PriceDayPrev[0];
                            }
                            else
                            {
                                _str += "일봉 - 지수\n";
                                curPrice = mti.moving_PriceDayCurE[0];
                                prevPrice = mti.moving_PriceDayPrevE[0];
                            }
                        }
                        else // 분봉
                        {
                            if (mti.m_buyingMovePriceKindType == 0)
                            {
                                _str += _num[mti.m_buyingMinuteType] + " 분봉 - 단순\n";
                                curPrice = mti.moving_PriceCur[0];
                                prevPrice = mti.moving_PricePrev[0];
                            }
                            else
                            {
                                _str += _num[mti.m_buyingMinuteType] + " 분봉 - 지수\n";
                                curPrice = mti.moving_PriceCurE[0];
                                prevPrice = mti.moving_PricePrevE[0];
                            }
                        }

                        _str += mti.m_buyingMinuteLineType + " 이평/";
                        _str += "현재:" + curPrice.ToString("N3") + " / " + "이전:" + prevPrice.ToString("N3");

                        if (mti.m_buyingDistance == 0) // 근접
                        {
                            double _perUpPrice = getPercentPrice(curPrice, mti.m_buyingMinuteLineAccessPer);
                            double _perDownPrice = getPercentPrice(curPrice, mti.m_buyingMinuteLineAccessPer * -1);

                            _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                        }
                        else if (mti.m_buyingDistance == 1) // 돌파
                        {
                            _str2 = "현재가 > " + curPrice.ToString("N0");
                        }
                    }
                    else if (mti.m_buyingType == 2) // 스토캐스틱 슬로우
                    {
                        double _standardPriceK = 0, _standardPriceKPrev = 0;
                        double _standardPriceD = 0, _standardPriceDPrev = 0;
                        _str = "- 스토캐스틱슬로우 매수 -\n";
                        if (mti.m_buyingBongType == 0) // 일봉
                        {
                            _str += "일봉";
                            _standardPriceK = mti.stochastics_KPriceDayCur[0];
                            _standardPriceKPrev = mti.stochastics_KPriceDayPrev[0];
                            _standardPriceD = mti.stochastics_DPriceDayCur[0];
                            _standardPriceDPrev = mti.stochastics_DPriceDayPrev[0];
                        }
                        else
                        {
                            _str += _num[mti.m_buyingMinuteType] + " 분봉\n";
                            _standardPriceK = mti.stochastics_KPriceCur[0];
                            _standardPriceKPrev = mti.stochastics_KPricePrev[0];
                            _standardPriceD = mti.stochastics_DPriceCur[0];
                            _standardPriceDPrev = mti.stochastics_DPricePrev[0];
                        }
                        _str += "sto1:" + mti.m_buyingStocPeriod1 + " / " + "sto2:" + mti.m_buyingStocPeriod2 + " / " + "sto3:" + mti.m_buyingStocPeriod3 + "\n";
                        _str += "현재K값:" + _standardPriceK.ToString("N2") + " / " + "이전K값:" + _standardPriceKPrev.ToString("N2") + "\n";
                        _str += "현재D값:" + _standardPriceD.ToString("N2") + " / " + "이전D값:" + _standardPriceDPrev.ToString("N2");
                        if (mti.m_buyingDistance == 0) // k값 이상
                        {
                            _str2 = "K값 " + _standardPriceK.ToString("N2") + " > " + mti.m_buyingMinuteLineAccessPer.ToString("N2");
                        }
                        else if (mti.m_buyingDistance == 1) // k값 이하
                        {
                            _str2 = "K값 " + _standardPriceK.ToString("N2") + " < " + mti.m_buyingMinuteLineAccessPer.ToString("N2");
                        }
                    }
                    else if (mti.m_buyingType == 3) //////////////////////// 볼린저밴드
                    {
                        double _bollUp = 0, _bollCenter = 0, _bollDown = 0;
                        double _bollUpPrev = 0, _bollCenterPrev = 0, _bollDownPrev = 0;

                        _str = "- 볼린저밴드 매수 -\n";
                        if (mti.m_buyingBongType == 0) // 일봉
                        {
                            _str += "일봉";
                            _bollUp = mti.bollinger_upPriceDayCur[0];
                            _bollCenter = mti.bollinger_centerPriceDayCur[0];
                            _bollDown = mti.bollinger_downPriceDayCur[0];
                            _bollUpPrev = mti.bollinger_upPriceDayPrev[0];
                            _bollCenterPrev = mti.bollinger_centerPriceDayPrev[0];
                            _bollDownPrev = mti.bollinger_downPriceDayPrev[0];
                        }
                        else
                        {
                            _str += _num[mti.m_buyingMinuteType] + " 분봉\n";
                            _bollUp = mti.bollinger_upPriceCur[0];
                            _bollCenter = mti.bollinger_centerPriceCur[0];
                            _bollDown = mti.bollinger_downPriceCur[0];
                            _bollUpPrev = mti.bollinger_upPricePrev[0];
                            _bollCenterPrev = mti.bollinger_centerPricePrev[0];
                            _bollDownPrev = mti.bollinger_downPricePrev[0];
                        }
                        _str += "Period:" + mti.m_buyingStocPeriod1 + " / " + "D1:" + mti.m_buyingBollPeriod + "\n";
                        _str += "현재중심:" + _bollCenter.ToString("N3") + " / " + "이전중심:" + _bollCenterPrev.ToString("N3") + "\n";
                        _str += "현재상한:" + _bollUp.ToString("N3") + " / " + "이전상한:" + _bollUpPrev.ToString("N3") + "\n";
                        _str += "현재하한:" + _bollDown.ToString("N3") + " / " + "이전하한:" + _bollDownPrev.ToString("N3");

                        if (mti.m_buyingLine3Type == 0) // 상한선
                        {
                            if (mti.m_buyingDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollUp, mti.m_buyingMinuteLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollUp, mti.m_buyingMinuteLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_buyingDistance == 1) // 돌파
                            {
                                _str2 = "현재가 > " + "상한선 " + _bollUp.ToString("N3");
                            }
                            else if (mti.m_buyingDistance == 2) // 이탈
                            {
                                _str2 = "현재가 < " + "상한선 " + _bollUp.ToString("N3");
                            }
                        }
                        else if (mti.m_buyingLine3Type == 1) // 중심선
                        {
                            if (mti.m_buyingDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollCenter, mti.m_buyingMinuteLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollCenter, mti.m_buyingMinuteLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_buyingDistance == 1) // 돌파
                            {
                                _str2 = "현재가 > " + "중심선 " + _bollCenter.ToString("N3");
                            }
                            else if (mti.m_buyingDistance == 2) // 이탈
                            {
                                _str2 = "현재가 < " + "중심선 " + _bollCenter.ToString("N3");
                            }
                        }
                        else if (mti.m_buyingLine3Type == 2) // 하한선
                        {
                            if (mti.m_buyingDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollDown, mti.m_buyingMinuteLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollDown, mti.m_buyingMinuteLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_buyingDistance == 1) // 돌파
                            {
                                _str2 = "현재가 > " + "하한선 " + _bollDown.ToString("N3");
                            }
                            else if (mti.m_buyingDistance == 2) // 이탈
                            {
                                _str2 = "현재가 < " + "하한선 " + _bollDown.ToString("N3");
                            }
                        }
                    }
                    else if (mti.m_buyingType == 4) //////////////////////// 엔벨로프
                    {
                        double _bollUp = 0, _bollCenter = 0, _bollDown = 0;
                        double _bollUpPrev = 0, _bollCenterPrev = 0, _bollDownPrev = 0;

                        _str = "- 엔벨로프 매수 -\n";
                        if (mti.m_buyingBongType == 0) // 일봉
                        {
                            _str += "일봉";
                            _bollUp = mti.envelope_upPriceDayCur[0];
                            _bollCenter = mti.envelope_centerPriceDayCur[0];
                            _bollDown = mti.envelope_downPriceDayCur[0];
                            _bollUpPrev = mti.envelope_upPriceDayPrev[0];
                            _bollCenterPrev = mti.envelope_centerPriceDayPrev[0];
                            _bollDownPrev = mti.envelope_downPriceDayPrev[0];
                        }
                        else
                        {
                            _str += _num[mti.m_buyingMinuteType] + " 분봉\n";
                            _bollUp = mti.envelope_upPriceCur[0];
                            _bollCenter = mti.envelope_centerPriceCur[0];
                            _bollDown = mti.envelope_downPriceCur[0];
                            _bollUpPrev = mti.envelope_upPricePrev[0];
                            _bollCenterPrev = mti.envelope_centerPricePrev[0];
                            _bollDownPrev = mti.envelope_downPricePrev[0];
                        }
                        _str += "Period:" + mti.m_buyingStocPeriod1 + " / " + "Percent:" + mti.m_buyingBollPeriod + "\n";
                        _str += "현재중심:" + _bollCenter.ToString("N3") + " / " + "이전중심:" + _bollCenterPrev.ToString("N3") + "\n";
                        _str += "현재저항:" + _bollUp.ToString("N3") + " / " + "이전저항:" + _bollUpPrev.ToString("N3") + "\n";
                        _str += "현재지지:" + _bollDown.ToString("N3") + " / " + "이전지지:" + _bollDownPrev.ToString("N3");

                        if (mti.m_buyingLine3Type == 0) // 상한선
                        {
                            if (mti.m_buyingDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollUp, mti.m_buyingMinuteLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollUp, mti.m_buyingMinuteLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_buyingDistance == 1) // 돌파
                            {
                                _str2 = "현재가 > " + "저항선 " + _bollUp.ToString("N3");
                            }
                            else if (mti.m_buyingDistance == 2) // 이탈
                            {
                                _str2 = "현재가 < " + "저항선 " + _bollUp.ToString("N3");
                            }
                        }
                        else if (mti.m_buyingLine3Type == 1) // 중심선
                        {
                            if (mti.m_buyingDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollCenter, mti.m_buyingMinuteLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollCenter, mti.m_buyingMinuteLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_buyingDistance == 1) // 돌파
                            {
                                _str2 = "현재가 > " + "중심선 " + _bollCenter.ToString("N3");
                            }
                            else if (mti.m_buyingDistance == 2) // 이탈
                            {
                                _str2 = "현재가 < " + "중심선 " + _bollCenter.ToString("N3");
                            }
                        }
                        else if (mti.m_buyingLine3Type == 2) // 하한선
                        {
                            if (mti.m_buyingDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollDown, mti.m_buyingMinuteLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollDown, mti.m_buyingMinuteLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_buyingDistance == 1) // 돌파
                            {
                                _str2 = "현재가 > " + "지지선 " + _bollDown.ToString("N3");
                            }
                            else if (mti.m_buyingDistance == 2) // 이탈
                            {
                                _str2 = "현재가 < " + "지지선 " + _bollDown.ToString("N3");
                            }
                        }
                    }
                    gMainForm.bunBongDataGridView["분봉_매수", 0].Value = _str;
                    gMainForm.bunBongDataGridView["분봉_매수", 1].Value = _str2;
                }
                else
                {
                    _str = "매수사용안함";
                    gMainForm.bunBongDataGridView["분봉_매수", 0].Value = _str;
                }
                ///////////////////////////////////////////////////////// 추매
                if (mti.m_reBuyingType == 0) ////////////////////////////////////////// 일반추매
                {
                    _str = "일반 추매";
                    _str2 = "현재수익률 " + mti.m_rateOfReturn.ToString("N2") + " < " + mti.m_reBuyingPer[0].ToString("N2");
                }
                else if (mti.m_reBuyingType == 1) /////////////////////////////////// 이동평균선 추매
                {
                    double curPrice = 0, prevPrice = 0;
                    _str = "- 이동평균선 추매 -\n";
                    if (mti.m_reBuyingBongType == 0) // 일봉
                    {
                        if (mti.m_reBuyingMovePriceKindType == 0)
                            _str += "일봉 - (단순)\n";
                        else
                            _str += "일봉 - (지수)\n";
                    }
                    else
                    {
                        if (mti.m_reBuyingMovePriceKindType == 0)
                            _str += _num[mti.m_reBuyingMinuteType] + " 분봉 - (단순)\n";
                        else
                            _str += _num[mti.m_reBuyingMinuteType] + " 분봉 - (지수)\n";
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        if (mti.m_buyingPerInvestment[i + 1] > 0)
                        {
                            if (mti.m_reBuyingBongType == 0) // 일봉                        
                            {
                                curPrice = mti.reBuyingMoving_PriceDayCur[i];
                                prevPrice = mti.reBuyingMoving_PriceDayPrev[i];
                            }
                            else
                            {
                                curPrice = mti.reBuyingMoving_PriceCur[i];
                                prevPrice = mti.reBuyingMoving_PricePrev[i];
                            }
                            if (i == 4)
                                _str += mti.m_reBuyingMinuteLineType[i] + " 이평/" + "현재:" + curPrice.ToString("N3") + "/" + "이전:" + prevPrice.ToString("N3");
                            else
                                _str += mti.m_reBuyingMinuteLineType[i] + " 이평/" + "현재:" + curPrice.ToString("N3") + "/" + "이전:" + prevPrice.ToString("N3") + "\n";
                        }
                    }
                    if (mti.m_reBuyingBongType == 0) // 일봉                        
                    {
                        curPrice = mti.reBuyingMoving_PriceDayCur[0];
                    }
                    else
                    {
                        curPrice = mti.reBuyingMoving_PriceCur[0];
                    }

                    double _perUpPrice = getPercentPrice(curPrice, mti.m_reBuyingPer[0]);
                    double _perDownPrice = getPercentPrice(curPrice, mti.m_reBuyingPer[0] * -1);
                    _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                }
                gMainForm.bunBongDataGridView["분봉_추매", 0].Value = _str;
                gMainForm.bunBongDataGridView["분봉_추매", 1].Value = _str2;

                /////////////////////////////////////////////////////// 익절
                if (mti.m_takeProfitUsing == 1) // 익절사용
                {
                    if (mti.m_takeProfitType == 0) // 기본 익절
                    {
                        _str = "일반 익절";
                        _str2 = "현재수익률 " + mti.m_rateOfReturn.ToString("N2") + " >= " + mti.m_takeProfitBuyingPricePer[0].ToString("N2");
                    }
                    else if (mti.m_takeProfitType == 1) // 이동평균선
                    {
                        double curPrice = 0, prevPrice = 0;
                        _str = "- 이동평균선 익절 -\n";

                        if (mti.m_takeProfitBongType == 0) // 일봉
                        {
                            if (mti.m_takeProfitMovePriceKindType == 0)
                            {
                                _str += "일봉 - 단순\n";
                                curPrice = mti.moving_PriceDayCur[2];
                                prevPrice = mti.moving_PriceDayPrev[2];
                            }
                            else
                            {
                                _str += "일봉 - 지수\n";
                                curPrice = mti.moving_PriceDayCurE[2];
                                prevPrice = mti.moving_PriceDayPrevE[2];
                            }
                        }
                        else
                        {
                            if (mti.m_takeProfitMovePriceKindType == 0)
                            {
                                _str += _num[mti.m_takeProfitMinuteType] + " 분봉 - 단순\n";
                                curPrice = mti.moving_PriceCur[2];
                                prevPrice = mti.moving_PricePrev[2];
                            }
                            else
                            {
                                _str += _num[mti.m_takeProfitMinuteType] + " 분봉 - 지수\n";
                                curPrice = mti.moving_PriceCurE[2];
                                prevPrice = mti.moving_PricePrevE[2];
                            }
                        }

                        _str += mti.m_takeProfitMinuteLineType + " 이평/";
                        _str += "현재:" + curPrice.ToString("N3") + " / " + "이전:" + prevPrice.ToString("N3");

                        if (mti.m_takeProfitDistance == 0) // 근접
                        {
                            double _perUpPrice = getPercentPrice(curPrice, mti.m_takeProfitLineAccessPer);
                            double _perDownPrice = getPercentPrice(curPrice, mti.m_takeProfitLineAccessPer * -1);

                            _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                        }
                        else if (mti.m_takeProfitDistance == 1) // 돌파
                        {
                            _str2 = "현재가 > " + curPrice.ToString("N0");
                        }
                    }
                    else if (mti.m_takeProfitType == 2) // 스토캐스틱SLOW
                    {
                        double _standardPriceK = 0, _standardPriceKPrev = 0;
                        double _standardPriceD = 0, _standardPriceDPrev = 0;
                        _str = "- 스토캐스틱슬로우 익절 -\n";
                        if (mti.m_takeProfitBongType == 0) // 일봉
                        {
                            _str += "일봉";
                            _standardPriceK = mti.stochastics_KPriceDayCur[2];
                            _standardPriceKPrev = mti.stochastics_KPriceDayPrev[2];
                            _standardPriceD = mti.stochastics_DPriceDayCur[2];
                            _standardPriceDPrev = mti.stochastics_DPriceDayPrev[2];
                        }
                        else
                        {
                            _str += _num[mti.m_takeProfitMinuteType] + " 분봉\n";
                            _standardPriceK = mti.stochastics_KPriceCur[2];
                            _standardPriceKPrev = mti.stochastics_KPricePrev[2];
                            _standardPriceD = mti.stochastics_DPriceCur[2];
                            _standardPriceDPrev = mti.stochastics_DPricePrev[2];
                        }
                        _str += "sto1:" + mti.m_takeProfitStocPeriod1 + " / " + "sto2:" + mti.m_takeProfitStocPeriod2 + " / " + "sto3:" + mti.m_takeProfitStocPeriod3 + "\n";
                        _str += "현재K값:" + _standardPriceK.ToString("N2") + " / " + "이전K값:" + _standardPriceKPrev.ToString("N2") + "\n";
                        _str += "현재D값:" + _standardPriceD.ToString("N2") + " / " + "이전D값:" + _standardPriceDPrev.ToString("N2");

                        if (mti.m_takeProfitDistance == 0) // k값 이상
                        {
                            _str2 = "K값 " + _standardPriceK.ToString("N2") + " > " + mti.m_takeProfitLineAccessPer.ToString("N2");
                        }
                        else if (mti.m_takeProfitDistance == 1) // k값 이하
                        {
                            _str2 = "K값 " + _standardPriceK.ToString("N2") + " < " + mti.m_takeProfitLineAccessPer.ToString("N2");
                        }
                    }
                    else if (mti.m_takeProfitType == 3) // 볼린저밴드
                    {
                        double _bollUp = 0, _bollCenter = 0, _bollDown = 0;
                        double _bollUpPrev = 0, _bollCenterPrev = 0, _bollDownPrev = 0;

                        _str = "- 볼린저밴드 익절 -\n";
                        if (mti.m_takeProfitBongType == 0) // 일봉
                        {
                            _str += "일봉";
                            _bollUp = mti.bollinger_upPriceDayCur[2];
                            _bollCenter = mti.bollinger_centerPriceDayCur[2];
                            _bollDown = mti.bollinger_downPriceDayCur[2];
                            _bollUpPrev = mti.bollinger_upPriceDayPrev[2];
                            _bollCenterPrev = mti.bollinger_centerPriceDayPrev[2];
                            _bollDownPrev = mti.bollinger_downPriceDayPrev[2];
                        }
                        else
                        {
                            _str += _num[mti.m_takeProfitMinuteType] + " 분봉\n";
                            _bollUp = mti.bollinger_upPriceCur[2];
                            _bollCenter = mti.bollinger_centerPriceCur[2];
                            _bollDown = mti.bollinger_downPriceCur[2];
                            _bollUpPrev = mti.bollinger_upPricePrev[2];
                            _bollCenterPrev = mti.bollinger_centerPricePrev[2];
                            _bollDownPrev = mti.bollinger_downPricePrev[2];
                        }
                        _str += "Period:" + mti.m_takeProfitStocPeriod1 + " / " + "D1:" + mti.m_takeProfitBollPeriod + "\n";
                        _str += "현재중심:" + _bollCenter.ToString("N3") + " / " + "이전중심:" + _bollCenterPrev.ToString("N3") + "\n";
                        _str += "현재상한:" + _bollUp.ToString("N3") + " / " + "이전상한:" + _bollUpPrev.ToString("N3") + "\n";
                        _str += "현재하한:" + _bollDown.ToString("N3") + " / " + "이전하한:" + _bollDownPrev.ToString("N3");

                        if (mti.m_takeProfitLine3Type == 0) // 상한선
                        {
                            if (mti.m_takeProfitDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollUp, mti.m_takeProfitLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollUp, mti.m_takeProfitLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_takeProfitDistance == 1) // 돌파
                            {
                                _str2 = "현재가 > " + "상한선 " + _bollUp.ToString("N3");
                            }
                            else if (mti.m_takeProfitDistance == 2) // 이탈
                            {
                                _str2 = "현재가 < " + "상한선 " + _bollUp.ToString("N3");
                            }
                        }
                        else if (mti.m_takeProfitLine3Type == 1) // 중심선
                        {
                            if (mti.m_takeProfitDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollCenter, mti.m_takeProfitLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollCenter, mti.m_takeProfitLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_takeProfitDistance == 1) // 돌파
                            {
                                _str2 = "현재가 > " + "중심선 " + _bollCenter.ToString("N3");
                            }
                            else if (mti.m_takeProfitDistance == 2) // 이탈
                            {
                                _str2 = "현재가 < " + "중심선 " + _bollCenter.ToString("N3");
                            }
                        }
                        else if (mti.m_takeProfitLine3Type == 2) // 하한선
                        {
                            if (mti.m_takeProfitDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollDown, mti.m_takeProfitLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollDown, mti.m_takeProfitLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_takeProfitDistance == 1) // 돌파
                            {
                                _str2 = "현재가 > " + "하한선 " + _bollDown.ToString("N3");
                            }
                            else if (mti.m_takeProfitDistance == 2) // 이탈
                            {
                                _str2 = "현재가 < " + "하한선 " + _bollDown.ToString("N3");
                            }
                        }
                    }
                    else if (mti.m_takeProfitType == 4) // 엔벨로프
                    {
                        double _bollUp = 0, _bollCenter = 0, _bollDown = 0;
                        double _bollUpPrev = 0, _bollCenterPrev = 0, _bollDownPrev = 0;

                        _str = "- 엔벨로프 익절 -\n";
                        if (mti.m_takeProfitBongType == 0) // 일봉
                        {
                            _str += "일봉";
                            _bollUp = mti.envelope_upPriceDayCur[2];
                            _bollCenter = mti.envelope_centerPriceDayCur[2];
                            _bollDown = mti.envelope_downPriceDayCur[2];
                            _bollUpPrev = mti.envelope_upPriceDayPrev[2];
                            _bollCenterPrev = mti.envelope_centerPriceDayPrev[2];
                            _bollDownPrev = mti.envelope_downPriceDayPrev[2];
                        }
                        else
                        {
                            _str += _num[mti.m_takeProfitMinuteType] + " 분봉\n";
                            _bollUp = mti.envelope_upPriceCur[2];
                            _bollCenter = mti.envelope_centerPriceCur[2];
                            _bollDown = mti.envelope_downPriceCur[2];
                            _bollUpPrev = mti.envelope_upPricePrev[2];
                            _bollCenterPrev = mti.envelope_centerPricePrev[2];
                            _bollDownPrev = mti.envelope_downPricePrev[2];
                        }
                        _str += "Period:" + mti.m_takeProfitStocPeriod1 + " / " + "Percent:" + mti.m_takeProfitBollPeriod + "\n";
                        _str += "현재중심:" + _bollCenter.ToString("N3") + " / " + "이전중심:" + _bollCenterPrev.ToString("N3") + "\n";
                        _str += "현재저항:" + _bollUp.ToString("N3") + " / " + "이전저항:" + _bollUpPrev.ToString("N3") + "\n";
                        _str += "현재지지:" + _bollDown.ToString("N3") + " / " + "이전지지:" + _bollDownPrev.ToString("N3");

                        if (mti.m_takeProfitLine3Type == 0) // 상한선
                        {
                            if (mti.m_takeProfitDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollUp, mti.m_takeProfitLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollUp, mti.m_takeProfitLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_takeProfitDistance == 1) // 돌파
                            {
                                _str2 = "현재가 > " + "저항선 " + _bollUp.ToString("N3");
                            }
                            else if (mti.m_takeProfitDistance == 2) // 이탈
                            {
                                _str2 = "현재가 < " + "저항선 " + _bollUp.ToString("N3");
                            }
                        }
                        else if (mti.m_takeProfitLine3Type == 1) // 중심선
                        {
                            if (mti.m_takeProfitDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollCenter, mti.m_takeProfitLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollCenter, mti.m_takeProfitLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_takeProfitDistance == 1) // 돌파
                            {
                                _str2 = "현재가 > " + "중심선 " + _bollCenter.ToString("N3");
                            }
                            else if (mti.m_takeProfitDistance == 2) // 이탈
                            {
                                _str2 = "현재가 < " + "중심선 " + _bollCenter.ToString("N3");
                            }
                        }
                        else if (mti.m_takeProfitLine3Type == 2) // 하한선
                        {
                            if (mti.m_takeProfitDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollDown, mti.m_takeProfitLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollDown, mti.m_takeProfitLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_takeProfitDistance == 1) // 돌파
                            {
                                _str2 = "현재가 > " + "지지선 " + _bollDown.ToString("N3");
                            }
                            else if (mti.m_takeProfitDistance == 2) // 이탈
                            {
                                _str2 = "현재가 < " + "지지선 " + _bollDown.ToString("N3");
                            }
                        }
                    }
                }
                else
                {
                    _str = "익절사용안함";
                }

                gMainForm.bunBongDataGridView["분봉_익절", 0].Value = _str;
                gMainForm.bunBongDataGridView["분봉_익절", 1].Value = _str2;

                ////////////////////////////////////////////////////////////////// 손절
                if (mti.m_stopLossUsing == 1) // 손절사용
                {
                    if (mti.m_stopLossType == 0) // 기본 손절
                    {
                        _str = "일반 손절";
                        _str2 = "현재수익률 " + mti.m_rateOfReturn.ToString("N2") + " <= " + mti.m_stopLossBuyingPricePer[0].ToString("N2");
                    }
                    else if (mti.m_stopLossType == 1) // 이동평균선 손절
                    {
                        double curPrice = 0, prevPrice = 0;
                        _str = "- 이동평균선 손절 -\n";

                        if (mti.m_stopLossBongType == 0) // 일봉
                        {
                            if (mti.m_stopLossMovePriceKindType == 0)
                            {
                                _str += "일봉 - 단순\n";
                                curPrice = mti.moving_PriceDayCur[3];
                                prevPrice = mti.moving_PriceDayPrev[3];
                            }
                            else
                            {
                                _str += "일봉 - 지수\n";
                                curPrice = mti.moving_PriceDayCurE[3];
                                prevPrice = mti.moving_PriceDayPrevE[3];
                            }
                        }
                        else // 분봉
                        {
                            if (mti.m_stopLossMovePriceKindType == 0)
                            {
                                _str += _num[mti.m_stopLossMinuteType] + " 분봉 - 단순\n";
                                curPrice = mti.moving_PriceCur[3];
                                prevPrice = mti.moving_PricePrev[3];
                            }
                            else
                            {
                                _str += _num[mti.m_stopLossMinuteType] + " 분봉 - 지수\n";
                                curPrice = mti.moving_PriceCurE[3];
                                prevPrice = mti.moving_PricePrevE[3];
                            }
                        }

                        _str += mti.m_stopLossMinuteLineType + " 이평/";
                        _str += "현재:" + curPrice.ToString("N3") + " / " + "이전:" + prevPrice.ToString("N3");

                        if (mti.m_stopLossDistance == 0) // 근접
                        {
                            double _perUpPrice = getPercentPrice(curPrice, mti.m_stopLossLineAccessPer);
                            double _perDownPrice = getPercentPrice(curPrice, mti.m_stopLossLineAccessPer * -1);

                            _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                        }
                        else if (mti.m_stopLossDistance == 1) // 이탈
                        {
                            _str2 = "현재가 < " + curPrice.ToString("N0");
                        }
                    }
                    else if (mti.m_stopLossType == 2) // 볼린저벤드 손절
                    {
                        double _bollUp = 0, _bollCenter = 0, _bollDown = 0;
                        double _bollUpPrev = 0, _bollCenterPrev = 0, _bollDownPrev = 0;

                        _str = "- 볼린저밴드 손절 -\n";
                        if (mti.m_stopLossBongType == 0) // 일봉
                        {
                            _str += "일봉";
                            _bollUp = mti.bollinger_upPriceDayCur[3];
                            _bollCenter = mti.bollinger_centerPriceDayCur[3];
                            _bollDown = mti.bollinger_downPriceDayCur[3];
                            _bollUpPrev = mti.bollinger_upPriceDayPrev[3];
                            _bollCenterPrev = mti.bollinger_centerPriceDayPrev[3];
                            _bollDownPrev = mti.bollinger_downPriceDayPrev[3];
                        }
                        else
                        {
                            _str += _num[mti.m_stopLossMinuteType] + " 분봉\n";
                            _bollUp = mti.bollinger_upPriceCur[3];
                            _bollCenter = mti.bollinger_centerPriceCur[3];
                            _bollDown = mti.bollinger_downPriceCur[3];
                            _bollUpPrev = mti.bollinger_upPricePrev[3];
                            _bollCenterPrev = mti.bollinger_centerPricePrev[3];
                            _bollDownPrev = mti.bollinger_downPricePrev[3];
                        }
                        _str += "Period:" + mti.m_stopLossStocPeriod1 + " / " + "D1:" + mti.m_stopLossBollPeriod + "\n";
                        _str += "현재중심:" + _bollCenter.ToString("N3") + " / " + "이전중심:" + _bollCenterPrev.ToString("N3") + "\n";
                        _str += "현재상한:" + _bollUp.ToString("N3") + " / " + "이전상한:" + _bollUpPrev.ToString("N3") + "\n";
                        _str += "현재하한:" + _bollDown.ToString("N3") + " / " + "이전하한:" + _bollDownPrev.ToString("N3");

                        if (mti.m_stopLossLine3Type == 0) // 상한선
                        {
                            if (mti.m_stopLossDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollUp, mti.m_stopLossLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollUp, mti.m_stopLossLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_stopLossDistance == 1) // 이탈
                            {
                                _str2 = "현재가 < " + "상한선 " + _bollUp.ToString("N3");
                            }
                        }
                        else if (mti.m_stopLossLine3Type == 1) // 중심선
                        {
                            if (mti.m_stopLossDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollCenter, mti.m_stopLossLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollCenter, mti.m_stopLossLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_stopLossDistance == 1) // 이탈
                            {
                                _str2 = "현재가 < " + "중심선 " + _bollCenter.ToString("N3");
                            }
                        }
                        else if (mti.m_stopLossLine3Type == 2) // 하한선
                        {
                            if (mti.m_stopLossDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollDown, mti.m_stopLossLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollDown, mti.m_stopLossLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_stopLossDistance == 1) // 이탈
                            {
                                _str2 = "현재가 < " + "하한선 " + _bollDown.ToString("N3");
                            }
                        }
                    }
                    else if (mti.m_stopLossType == 3) // 엔벨로프 손절
                    {
                        double _bollUp = 0, _bollCenter = 0, _bollDown = 0;
                        double _bollUpPrev = 0, _bollCenterPrev = 0, _bollDownPrev = 0;

                        _str = "- 엔벨로프 손절 -\n";
                        if (mti.m_stopLossBongType == 0) // 일봉
                        {
                            _str += "일봉";
                            _bollUp = mti.envelope_upPriceDayCur[3];
                            _bollCenter = mti.envelope_centerPriceDayCur[3];
                            _bollDown = mti.envelope_downPriceDayCur[3];
                            _bollUpPrev = mti.envelope_upPriceDayPrev[3];
                            _bollCenterPrev = mti.envelope_centerPriceDayPrev[3];
                            _bollDownPrev = mti.envelope_downPriceDayPrev[3];
                        }
                        else
                        {
                            _str += _num[mti.m_stopLossMinuteType] + " 분봉\n";
                            _bollUp = mti.envelope_upPriceCur[3];
                            _bollCenter = mti.envelope_centerPriceCur[3];
                            _bollDown = mti.envelope_downPriceCur[3];
                            _bollUpPrev = mti.envelope_upPricePrev[3];
                            _bollCenterPrev = mti.envelope_centerPricePrev[3];
                            _bollDownPrev = mti.envelope_downPricePrev[3];
                        }
                        _str += "Period:" + mti.m_stopLossStocPeriod1 + " / " + "Percent:" + mti.m_stopLossBollPeriod + "\n";
                        _str += "현재중심:" + _bollCenter.ToString("N3") + " / " + "이전중심:" + _bollCenterPrev.ToString("N3") + "\n";
                        _str += "현재저항:" + _bollUp.ToString("N3") + " / " + "이전저항:" + _bollUpPrev.ToString("N3") + "\n";
                        _str += "현재지지:" + _bollDown.ToString("N3") + " / " + "이전지지:" + _bollDownPrev.ToString("N3");

                        if (mti.m_stopLossLine3Type == 0) // 상한선
                        {
                            if (mti.m_stopLossDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollUp, mti.m_stopLossLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollUp, mti.m_stopLossLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_stopLossDistance == 1) // 이탈
                            {
                                _str2 = "현재가 < " + "저항선 " + _bollUp.ToString("N3");
                            }
                        }
                        else if (mti.m_stopLossLine3Type == 1) // 중심선
                        {
                            if (mti.m_stopLossDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollCenter, mti.m_stopLossLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollCenter, mti.m_stopLossLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_stopLossDistance == 1) // 이탈
                            {
                                _str2 = "현재가 < " + "중심선 " + _bollCenter.ToString("N3");
                            }
                        }
                        else if (mti.m_stopLossLine3Type == 2) // 하한선
                        {
                            if (mti.m_stopLossDistance == 0) // 근접
                            {
                                double _perUpPrice = getPercentPrice(_bollDown, mti.m_stopLossLineAccessPer);
                                double _perDownPrice = getPercentPrice(_bollDown, mti.m_stopLossLineAccessPer * -1);

                                _str2 = _perDownPrice.ToString("N0") + " <= 현재가 <= " + _perUpPrice.ToString("N0");
                            }
                            else if (mti.m_stopLossDistance == 1) // 이탈
                            {
                                _str2 = "현재가 < " + "지지선 " + _bollDown.ToString("N3");
                            }
                        }
                    }

                }
                else
                {
                    _str = "손절사용안함";
                }
                gMainForm.bunBongDataGridView["분봉_손절", 0].Value = _str;
                gMainForm.bunBongDataGridView["분봉_손절", 1].Value = _str2;
            }
        }
    }
}
