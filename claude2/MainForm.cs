using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;


namespace StockAutoTrade2
{
    public partial class MainForm : Form
    {
        private static MainForm gMainForm;
        public LoginManager gLoginInstance; // 로그인 매니져
        public TradingManager gTradingManager; // 매매 매니져

        public string sServerGubun = string.Empty; // 서버구분 변수 선언
        // 조건식 관리 리스트
        public List<ConditionData> conditionDataList = new List<ConditionData>();
        public List<GoJumMedo> gojumMedoList = new List<GoJumMedo>();
        // ConditionSettingDialog 변수
        public ConditionSettingDialog ConditionDig;
        public ConditionLoadDialog ConditionLoadDig;
        public ProfitLossDialog ProfitLossDig;
        public HoldingItemSettingDialog HolditemDig;
        public EntireLog EntireLog;
        // 화면 번호 1000 부터 9999번까지
        private int screenNum; // = 1000번부터 시작
        // 조건식 감시 요청을 위한 스크린번호 사용유무
        public bool[] bConditionSNUserOrNot; // = new bool[5];
        // 추정 예수금
        public long curOrderAmount;
        //나의 매매 조건식 관리 리스트
        public List<MyTradingCondition> gMyTradingConditionList = new List<MyTradingCondition>();
        // 나의 보유 종목 관리 리스트
        public List<MyHoldingItemOption> gMyHoldingItemOptionList = new List<MyHoldingItemOption>();
        // 수동 매수(영웅문매수 종목 관리 리스트
        public List<PassitiveBuyingItem> gMyPassitiveBuyingItemList = new List<PassitiveBuyingItem>();
        //편입중복체크 리스트
        public List<string> transferSameCheckList = new List<string>();
        // 조회 요청 매니져 생성
        public CheckRequestManager gCheckRequestManager;
        // 주문 접수 요청 매니져 생성
        public OrderRequestManager gOrderRequestManager;
        //글로벌 로그 저장소(예: MainForm에 정의) -> 전체로그와 종목별 로그를 구분하기위해 만든 리스트.
        public List<string> globalLogs = new List<string>();
        // ObservableCollection으로 선언하면 CollectionChanged 이벤트를 사용할 수 있다.
        public ObservableCollection<string> globalLogs2 = new ObservableCollection<string>();


        // logListBox 다른 스레드 호출시 처리(기본적으로 logListBox 메서드는 UI스레드에서 사용한다고 가정해)
        private delegate void SafeCallLogListBoxDelegate(string _slog);

        public FileIO gFileIOInstance; // FileIO 변수

        // 편입 종목 저장 리스트(특정 종목의 봉차트를 다 받고 새로운 종목을 받기위해 임시저장)
        public List<string> g_InsertItemList = new List<string>();
        // 편입 종목의 1분봉차트를 가져오기 위한 타이머
        public System.Threading.Timer g_GlobalTimer;
        // 종목이 분봉데이터를 서버에서 가지고 오는 중인지 확인
        public bool g_bGetBunBong = false;
        // AddMyTradingItemToDataGridView 크르스오류(쓰레드충돌) 방지 
        private delegate void SafeCallAddMyTradingItemToDataGridView(MyTradingItem _mti, double itemInvestment);

        // 시작 시간이 10시인 날짜 확인
        public string[] _startTime10 = { "0102" };
        public string g_curDay;

        // 테스트 변수
        public string g_selectItemCode = String.Empty;
        public string g_selectItemName = String.Empty;

        public bool g_bIndicatroview = true;

        public bool g_medohidden = true;

        public bool g_holditemactivate = false;

        private readonly object _lock = new object();

        private System.Windows.Forms.Timer internetCheckTimer;
        public MainForm()
        {
            InitializeComponent();

            gMainForm = this;
            gLoginInstance = LoginManager.GetInstance();

            // 폼 closing 이벤트등록
            FormClosing += MainForm_Closing;

            // 로그인 버튼 이벤트 등록
            LoginButton.Click += ButtonClickEvent;
            // 비밀번호 입력창 버튼 이벤트 등록
            passwordTextBox.Click += ButtonClickEvent;
            // 전체 로그보기 버튼 클릭 이벤트 등록
            showEntireLogButton.Click += ButtonClickEvent;
            // 조건식설정창 버튼 이벤트 등록
            conditionSettingButton.Click += ButtonClickEvent;
            // 보유종목설정창 버튼 이벤트 등록
            holdingitemsettingButton.Click += ButtonClickEvent;
            // 지표보기 버튼 이벤트 등록
            indicatorViewButton.Click += ButtonClickEvent;
            // 매도종목숨기기 버튼 이벤트 등록
            medoHiddenButton.Click += ButtonClickEvent;
            // 전체보유매도 활성화버튼
            holdingItemActivateButton.Click += ButtonClickEvent;
            // 로그 리스트박스 DrawItem 이벤트 등록
            logListBox.DrawItem += LogListBoxDrawItemEvent;
            // ObservableCollection의 변경 이벤트 핸들러 등록
            globalLogs2.CollectionChanged += GlobalLogs2_CollectionChanged;

            // ConditionSettingDialog 객체생성
            ConditionDig = new ConditionSettingDialog();
            ConditionLoadDig = new ConditionLoadDialog();
            ProfitLossDig = new ProfitLossDialog();
            HolditemDig = new HoldingItemSettingDialog();
            EntireLog = new EntireLog();
            //화면 번호 초기화
            screenNum = (int)ConditionNumber.NormalStartNum;
            bConditionSNUserOrNot = new bool[(int)ConditionNumber.TotalConditionCount];
            // 실시간 조건식 감시에 사용되는 스크린번호 사용 확인용
            for (int i = 0; i < (int)ConditionNumber.TotalConditionCount; i++)
                bConditionSNUserOrNot[i] = false;

            gTradingManager = TradingManager.GetInstance();

            //tradingConditionDataGridView 초기 설정하기
            //머리글 중앙 정렬
            tradingConditionDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //더블버퍼 설정
            tradingConditionDataGridView.DoubleBuffered(true);
            //각각의 열 정렬
            foreach(DataGridViewColumn item in tradingConditionDataGridView.Columns)
            {
                item.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //중앙 정렬
            }
            //조건식 데이터그리드뷰 셀클릭 이벤트 등록
            tradingConditionDataGridView.CellClick += tradingConditionCellsClickEvent;
            // 조건식 데이타그리드뷰 셀더블클릭 이벤트 등록
            tradingConditionDataGridView.CellDoubleClick += tradingConditionCellDoubleClickEvent;

            //실시간 조건검색 요청에 대한 수신 이벤트 등록
            KiwoomAPI.OnReceiveRealCondition += OnReceiveRealConditionEvent;

            /*
            // transferItemDataGridView 초기 설정하기
            // 머리글 중앙정렬
            transferItemDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //더블버퍼 설정
            transferItemDataGridView.DoubleBuffered(true);
            //각각의 열 정렬
            foreach (DataGridViewColumn item in transferItemDataGridView.Columns)
            {
                item.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //중앙 정렬
            }
            */
            // tradingItemDataGridView 초기 설정하기
            // 머리글 중앙 정렬
            tradingItemDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //더블버퍼 설정
            tradingItemDataGridView.DoubleBuffered(true);
            //각각의 열 정렬
            for(int i = 0; i < tradingItemDataGridView.Columns.Count; i++)
            {
                if (i < 3 || i == tradingItemDataGridView.Columns.Count - 1 || i == tradingItemDataGridView.Columns.Count - 2)// columns[0],[1],[2] -> 종목명, 종목코드, 조건식 // 마지막 두개 columns 진행상황 , 즉시매도
                    tradingItemDataGridView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // 중앙정렬
                else // 숫자 표시 부분은 우측정렬 -> 나머지 columns
                    tradingItemDataGridView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // 우측정렬
            }

            tradingItemDataGridView.CellFormatting += tradingItemDataGridView_CellFormatting;
            tradingItemDataGridView.CellClick += tradingItemDataGridView_CellClick;

            // 요청 매니저 객체생성
            gCheckRequestManager = new CheckRequestManager();
            gOrderRequestManager = new OrderRequestManager();

            //orderDataGridView 초기 설정하기
            // 머리글 중앙 정렬
            orderDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //더블버퍼 설정
            orderDataGridView.DoubleBuffered(true);
            //각각의 열 정렬
            for (int i = 0; i < orderDataGridView.Columns.Count; i++)
            {
                if (i < 3) // columns[0],[1],[2] -> 종목명, 종목코드, 조건식
                    orderDataGridView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // 중앙정렬
                else // 숫자 표시 부분은 우측정렬 -> 나머지 columns
                    orderDataGridView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // 우측정렬
            }
            //conclusionDataGridView 초기 설정하기
            // 머리글 중앙 정렬
            conclusionDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //더블버퍼 설정
            conclusionDataGridView.DoubleBuffered(true);
            //각각의 열 정렬
            for (int i = 0; i < conclusionDataGridView.Columns.Count; i++)
            {
                if (i < 3) // columns[0],[1],[2] -> 종목명, 종목코드, 조건식
                    conclusionDataGridView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // 중앙정렬
                else // 숫자 표시 부분은 우측정렬 -> 나머지 columns
                    conclusionDataGridView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // 우측정렬
            }

            // soldDataGridView 초기 설정하기
            // 머리글 중앙 정렬
            soldDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            // 더블버퍼 설정
            soldDataGridView.DoubleBuffered(true);
            // 각각의 열 정렬
            foreach (DataGridViewColumn item in soldDataGridView.Columns)
            {
                for (int i = 0; i < soldDataGridView.Columns.Count; i++)
                {
                    if (i < 2)
                        soldDataGridView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // 중앙정렬
                    else
                        soldDataGridView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // 우측정렬
                }
            }
            soldDataGridView.CellFormatting += soldDataGridView_CellFormatting;

            // 매매종목 데이타그리드뷰 셀클릭 이벤트 등록
            tradingItemDataGridView.CellClick += tradingItemCellClickEvent;

            // 조건식 매매 방식 저장
            gFileIOInstance = FileIO.GetInstance(); // 인스턴스 리턴
            gFileIOInstance.setConditionFileCreate(); // 조건식 저장 파일 유무확인
            gFileIOInstance.setItemFileCreate(); // 종목저장파일생성
            gFileIOInstance.setlastHoldingItemFileCreate(); // 전체보유매도 파일 생성
            gFileIOInstance.setlastConditionFileCreate(); // 폼이꺼졋을때 설정값 저장 파일 유무확인
            gFileIOInstance.setLogFileCreate(); // 로그파일생성

            // 0.5초 간격으로 call_GlobalTimer메서드를 호출하는 타이머 생성
            g_GlobalTimer = new System.Threading.Timer(call_GlobalTimer, null, 0, 500); // 마지막 두 인자는 타이머가 처음 실행될때까지 대기시간 , 주기적으로 실행되는 시간간격을 의미 // 이 타이머는 주기적으로(500ms?)마다 call_globaltimer라는 메서드를 자동으로 불러오게됨
            g_curDay = DateTime.Now.ToString("MMdd");

            // bunBongDataGridView 초기 설정하기
            bunBongDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            // 더블버퍼 설정
            bunBongDataGridView.DoubleBuffered(true);
            // 각각의 열 정렬
            int count = 0;
            foreach (DataGridViewColumn item in bunBongDataGridView.Columns)
            {
                if (count == 0)
                    item.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // 중앙정렬
                else
                    item.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; // 중앙정렬
                count++;
            }

            int rowIndex = bunBongDataGridView.Rows.Add(); // 그리드뷰에 한 줄이 추가됨
            bunBongDataGridView["분봉_상태", rowIndex].Value = ""; // 종목명
            bunBongDataGridView["분봉_매수", rowIndex].Value = ""; // 종목명
            bunBongDataGridView["분봉_추매", rowIndex].Value = ""; // 조건식
            bunBongDataGridView["분봉_익절", rowIndex].Value = ""; // 주문번호
            bunBongDataGridView["분봉_손절", rowIndex].Value = ""; // 주문시간            
            bunBongDataGridView.Rows[rowIndex].Height = 85;
            rowIndex = bunBongDataGridView.Rows.Add(); // 그리드뷰에 한 줄이 추가됨
            bunBongDataGridView["분봉_상태", rowIndex].Value = "가격대"; // 종목명
            bunBongDataGridView["분봉_매수", rowIndex].Value = ""; // 종목명
            bunBongDataGridView["분봉_추매", rowIndex].Value = ""; // 조건식
            bunBongDataGridView["분봉_익절", rowIndex].Value = ""; // 주문번호
            bunBongDataGridView["분봉_손절", rowIndex].Value = ""; // 주문시간
            bunBongDataGridView.Rows[rowIndex].Height = 20;

            InitializeInternetMonitoring();
            DisplayUserInfo();

        }
        private void InitializeInternetMonitoring()
        {
            internetCheckTimer = new System.Windows.Forms.Timer();
            internetCheckTimer.Interval = 30000; // 30초마다 체크
            internetCheckTimer.Tick += InternetCheckTimer_Tick;
            internetCheckTimer.Start();

            // 초기 연결 상태 확인
            CheckInternetConnection();
        }
        private void InternetCheckTimer_Tick(object sender, EventArgs e)
        {
            CheckInternetConnection();
        }
        private void CheckInternetConnection()
        {
            var authManager = UserAuthManager.GetInstance();
            if (!authManager.IsInternetConnected())
            {
                MessageBox.Show("인터넷 연결이 끊어졌습니다.\n프로그램을 종료합니다.",
                               "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
        }
        private void DisplayUserInfo()
        {
            var authManager = UserAuthManager.GetInstance();
            if (!string.IsNullOrEmpty(authManager.CurrentUserEmail))
            {
                // 제목 표시줄에 사용자 정보 추가
                this.Text = $"조건식 자동매매 프로그램 - 사용자: {authManager.CurrentUserEmail}";

                // 로그에 사용자 정보 표시
                setLogText($"사용자 '{authManager.CurrentUserEmail}'로 로그인됨");
            }
        }


        public static MainForm GetInstance()
        {
            if (gMainForm == null)
                gMainForm = new MainForm();

            return gMainForm;
        }
        private void MainForm_Closing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(gMainForm.gFileIOInstance.ItemPath, string.Empty);

            foreach (DataGridViewRow row in tradingItemDataGridView.Rows)
            {
                string _conditionName = row.Cells["매매진행_조건식"].Value.ToString();
                string _itemCode = row.Cells["매매진행_종목코드"].Value.ToString();
                string _Gubun = row.Cells["매매진행_구분"].Value.ToString();
                string _holdingQnt = row.Cells["매매진행_보유수량"].Value.ToString();

                MyTradingCondition _mtc = gMainForm.gMyTradingConditionList.Find(o => o.conditionData.conditionName.Equals(_conditionName));

                if(_mtc != null && _Gubun != "보유" && _holdingQnt != "0")
                {
                    MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(_itemCode));

                    if(_mti != null)
                    {
                        gMainForm.gFileIOInstance.mainForm_saveItem(_itemCode, _conditionName, _mtc.conditionData.conditionIndex, 0 , _mti.getRePurchaseArray(), _mti.m_currentRebuyingStep);
                    }
                }
            }
            //gMainForm.gFileIOInstance.saveItem()

            gCheckRequestManager.Stop();
            gOrderRequestManager.Stop();
            gCheckRequestManager = null;
            gOrderRequestManager = null;

            // 리소스 정리
            internetCheckTimer?.Stop();
            internetCheckTimer?.Dispose();

            var authManager = UserAuthManager.GetInstance();
            authManager?.Dispose();
        }

        private void ButtonClickEvent(object sender, EventArgs e)
        {
            // 키움 서버 로그인
            if (sender.Equals(LoginButton))
            {
                gLoginInstance.KiWoomServerLogin();
            }
            else if(sender.Equals(passwordTextBox))
            {
                if(KiwoomAPI.GetConnectState() == 0)
                {
                    MessageBox.Show("로그인을 해주세요.");
                }
                else if (gMainForm.KiwoomAPI.GetConnectState() == 1)
                {
                    gMainForm.KiwoomAPI.KOA_Functions(("ShowAccountWindow"), (""));
                }
            }
            else if(sender.Equals(showEntireLogButton))
            {
                if (gMainForm.KiwoomAPI.GetConnectState() == 1)
                {
                    //전체 로그 화면 출력
                    EntireLog.ShowDialog();
                }
                else
                    MessageBox.Show("로그인을 해주세요.");
            }
            else if(sender.Equals(conditionSettingButton))
            {
                if (gMainForm.KiwoomAPI.GetConnectState() == 1)
                {
                    //조건식설정 화면 출력
                    ConditionDig.ShowDialog();
                }
                else
                    MessageBox.Show("로그인을 해주세요.");
            }
            else if(sender.Equals(holdingitemsettingButton))
            {
                if(gMainForm.KiwoomAPI.GetConnectState() ==1 )
                {
                    //보유종목설정 화면 출력
                    HolditemDig.ShowDialog();
                }
                else
                    MessageBox.Show("로그인을 해주세요.");
            }
            else if(sender.Equals(profitLossButton))
            {
                if(gMainForm.KiwoomAPI.GetConnectState()==1)
                {
                    ProfitLossDig.getProfitLossFileDataToDataGridView();
                    ProfitLossDig.ShowDialog();
                }
                else
                    MessageBox.Show("로그인을 해주세요.");
            }
            // 지표보기 버튼 클릭이벤트
            else if(sender.Equals(indicatorViewButton))
            {
                if(g_bIndicatroview)
                {
                    g_bIndicatroview = false;
                    indicatorLabel.Visible = false;
                    indicatorPictureBox.Visible = false;
                    bunBongDataGridView.Visible = false;
                    tradingItemDataGridView.Size = new Size(1167, 334);
                    indicatorViewButton.Text = "지표계산보기 ▲";
                }
                else
                {
                    g_bIndicatroview = true;
                    indicatorLabel.Visible = true;
                    indicatorPictureBox.Visible = true;
                    bunBongDataGridView.Visible = true;
                    tradingItemDataGridView.Size = new Size(1167, 175);
                    indicatorViewButton.Text = "지표계산보기 ▼";
                }
            }
            //매도종목 숨기기 버튼 클릭이벤트
            else if(sender.Equals(medoHiddenButton))
            {
                if(g_medohidden)
                {
                    g_medohidden = false;
                    medoHiddenButton.Text = "매도종목 보기";
                    foreach(DataGridViewRow row in tradingItemDataGridView.Rows)
                    {
                        if (row.Cells["매매진행_진행상황"].Value.ToString().Equals("매도완료"))
                        {
                            row.Visible = true; // 매도 완료된 항목을 다시 보이도록 설정

                        }
                    }
                }
                else
                {
                    g_medohidden = true;
                    medoHiddenButton.Text = "매도종목 숨기기";
                    foreach (DataGridViewRow row in tradingItemDataGridView.Rows)
                    {
                        if (row.Cells["매매진행_진행상황"].Value.ToString().Equals("매도완료"))
                        {
                            row.Visible = false; // 매도 완료된 항목을 숨김
                        }
                    }
                }
            }
            else if(sender.Equals(holdingItemActivateButton))
            {
                if (gMainForm.KiwoomAPI.GetConnectState() == 1)
                {
                    MyHoldingItemOption mho = gMyHoldingItemOptionList.Find(o => o.CheckFirstRun.Equals(g_holditemactivate));
                    {
                        if(mho != null)
                        {
                            if (!mho.CheckFirstRun)
                            {
                                mho.CheckFirstRun = true;
                                mho.m_holdingitemState = HoldingItemState.Playing;
                                g_holditemactivate = true;
                                holdingItemActivateButton.Text = "진행중";
                            }
                            else
                            {
                                if (mho.m_holdingitemState == HoldingItemState.Playing) // 현재 진행중이면
                                {
                                    mho.m_holdingitemState = HoldingItemState.Stop;
                                    holdingItemActivateButton.Text = "일시정지";
                                }
                                else // 현재 일시정지상태라면
                                {
                                    mho.m_holdingitemState = HoldingItemState.Playing;
                                    holdingItemActivateButton.Text = "진행중";
                                }

                            }
                        }
                        else if(mho == null)
                        {
                            MessageBox.Show("현재 보유 종목이 없습니다.");
                        }
                    }
                }
                else
                    MessageBox.Show("로그인을 해주세요.");

            }
        }

        //텍스트를 인자로 받아서 현재시간과 함께 출력을 해준다.
        public void setLogText(string _slog)
        {
            string _curTime = DateTime.Now.ToString("HH:mm:ss");
            string newLogEntry = "[" + _curTime + "] " + _slog;

            // 글로벌 로그 리스트에 추가
            globalLogs2.Add(newLogEntry);

            if (EntireLog.logTextBox.InvokeRequired)
            {
                EntireLog.logTextBox.Invoke(new Action(() => {
                    EntireLog.logTextBox.AppendText(newLogEntry + Environment.NewLine + Environment.NewLine);
                }));
            }
            else
            {
               EntireLog.logTextBox.AppendText(newLogEntry + Environment.NewLine + Environment.NewLine);
            }
            /*
            if (logListBox.InvokeRequired) // 다른 스레드에서 호출시 처리 delegate사용 -> UI스레드가 아니라면 UI스레드에서 setlogText 메서드를 호출
            {
                SafeCallLogListBoxDelegate _b = new SafeCallLogListBoxDelegate(setLogText);
                logListBox.Invoke(_b, new object[] { _slog }); // UI 스레드에서 델리게이트 실행
            }
            else // setLogText가 UI스레드에서 사용된다면 /  UI스레드에서 setLogText메서드를 호출
            {
                // 현재 시간 + 로그 출력 (UI 스레드에서 실행됨)
                logListBox.Items.Add("[" + _curTime + "]" + _slog);
                // 리스트박스 아이템 맨 아래로 선택 인덱스 셋팅 (UI 스레드에서 실행됨)
                logListBox.SelectedIndex = logListBox.Items.Count - 1;
            }*/

        }
        private void GlobalLogs2_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // 선택된 종목이 있다면 필터링하여 ListBox를 업데이트
            if (!string.IsNullOrEmpty(g_selectItemName))
            {
                List<string> snapshot;
                lock (_lock)
                {
                    snapshot = globalLogs2.ToList();
                }

                var filteredLogs = snapshot.Where(log => log != null && log.Contains(g_selectItemName)).ToList();

                if (logListBox.InvokeRequired)
                {
                    logListBox.Invoke(new Action(() =>
                    {
                        UpdateLogListBox(filteredLogs);
                    }));
                }
                else
                {
                    UpdateLogListBox(filteredLogs);
                }
            }
        }
        private void UpdateLogListBox(System.Collections.Generic.List<string> logs)
        {
            logListBox.Items.Clear();
            foreach (var log in logs)
            {
                logListBox.Items.Add(log);
            }
            if (logListBox.Items.Count > 0)
            {
                logListBox.SelectedIndex = logListBox.Items.Count - 1;
            }
        }
        // 텍스트 색상 변경을 위한 이벤트
        // 텍스트가 추가될 때마다 자동으로 호출이 된다
        private void LogListBoxDrawItemEvent(object sender, DrawItemEventArgs e)
        {
            //리스트에 아무것도 없으면 return
            if (e.Index < 0 || logListBox.Items.Count == 0)
                return;

            //텍스트에 성공, 실패 단어가 들어 있을때 색상을 다르게 처리를 한다.
            Brush _brush;
            string _curStr = logListBox.Items[e.Index].ToString();
            if (_curStr.Contains("성공") == true)
                _brush = Brushes.Blue; //파란색
            else if (_curStr.Contains("실패") == true)
                _brush = Brushes.Red; //빨간색
            else if (_curStr.Contains("ServerMSG") == true)
                _brush = Brushes.Green; // green
            else
                _brush = Brushes.Black; // 검은색
            e.Graphics.DrawString(logListBox.Items[e.Index].ToString(), e.Font, _brush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        // 스크린번호 얻어오기
        public String GetScreenNumber()
        {
            if (screenNum >= (int)ConditionNumber.ConditionStartNum - 1) // 번호가 1190 넘어가면 다시 1000번부터 시작
                screenNum = (int)ConditionNumber.NormalStartNum;

            screenNum++; // 호출될 때마다 번호 1씩 증가 시킴

            return screenNum.ToString(); //문자열로 리턴
        }
        // 조건검색식의 화면번호를 가져온다
        public String GetConditionScreenNumber()
        {
            int screenNum = 0;
            for(int i=0; i<(int)ConditionNumber.TotalConditionCount; i++) //현재 총 5개의 조건식 사용
            {
                if(!bConditionSNUserOrNot[i]) // 현재 사용하지 않는 배열 찾기
                {
                    screenNum = i + (int)ConditionNumber.ConditionStartNum; // 1191+i
                    return screenNum.ToString();
                }
            }
            return screenNum.ToString();
        }
        //조건식 5개가 전부 사용되었는지 확인하기위함 / 하나라도 빈 배열공간이있다면(ex: 조건식을 4개사용하여 1개를 더 사용할수있음)의 경우 true를 반환
        public bool GetConditionListCreateCheck()
        {
            bool ret = false;
            for(int i = 0; i<(int)ConditionNumber.TotalConditionCount; i++) // 현재 총 5개의 조건식 사용
            {
                if(!bConditionSNUserOrNot[i]) // 사용하지 않는 것이 있다면 리턴 true
                {
                    ret = true;
                }
            }
            return ret;
        }
        //조건검색식 등록시 배열 셋팅
        public void setConditionListCreateCheck(int arraynum, bool onoff)
        {
            bConditionSNUserOrNot[arraynum] = onoff; //조건식 등록할때 true로 설정
        }

        // 등록하기 버튼클릭시 데이터그리드뷰에 고정
        public void AddmyTradingConditionToDataGridView(MyTradingCondition mtc)
        {
            // rowIndex = 몇 번째 줄인지 리턴
            int rowIndex = tradingConditionDataGridView.Rows.Add(); // 그리드뷰에 한줄이 추가됨

            tradingConditionDataGridView["매매조건식_조건식", rowIndex].Value = mtc.conditionData.conditionName; // 조건식이름
            tradingConditionDataGridView["매매조건식_종목투자금", rowIndex].Value = mtc.itemInvestment.ToString("N0"); // 종목투자금
            tradingConditionDataGridView["매매조건식_매수종목수", rowIndex].Value = mtc.buyingItemCount.ToString("N0"); //매수종목수
            tradingConditionDataGridView["매매조건식_실매수종목수", rowIndex].Value = "0"; //매수종목수
            
            //매수
            if(mtc.buyingUsing == 0)
            {
                tradingConditionDataGridView["매매조건식_매수타입", rowIndex].Value = "사용안함";
            }
            else
            {
                if(mtc.buyingType ==0) // 기본매수
                {
                    if (mtc.buyingTransferType == 0)
                        tradingConditionDataGridView["매매조건식_매수타입", rowIndex].Value = "편입즉시매수";
                    else
                        tradingConditionDataGridView["매매조건식_매수타입", rowIndex].Value = "편입가대비매수";
                }
                else
                {
                    string[] _str = { "이동평균선", "스토캐스틱", "볼린저밴드", " 엔벨로프" };
                    tradingConditionDataGridView["매매조건식_매수타입", rowIndex].Value = _str[mtc.buyingType - 1];
                }
            }
            //추매
            if (mtc.buyingCount == 1)
                tradingConditionDataGridView["매매조건식_추매", rowIndex].Value = "사용안함";
            else
            {
                if (mtc.reBuyingType == 0)
                    tradingConditionDataGridView["매매조건식_추매", rowIndex].Value = "기본추매";
                else
                    tradingConditionDataGridView["매매조건식_추매", rowIndex].Value = "이동평균선";
            }
            //익절
            if(mtc.takeProfitUsing ==0)
            {
                tradingConditionDataGridView["매매조건식_익절", rowIndex].Value = "사용안함";
            }
            else
            {
                string[] _str = { "기본익절", "이동평균선", "스토캐스틱", "볼린저밴드", "엔벨로프" };
                tradingConditionDataGridView["매매조건식_익절", rowIndex].Value = _str[mtc.takeProfitType];
            }
            // 손절
            if (mtc.stopLossUsing == 0)
            {
                tradingConditionDataGridView["매매조건식_손절", rowIndex].Value = "사용안함";
            }
            else
            {
                string[] _str = { "기본손절", "이동평균선", "볼린저밴드", "엔벨로프" };
                tradingConditionDataGridView["매매조건식_손절", rowIndex].Value = _str[mtc.stopLossType];
            }

            tradingConditionDataGridView["매매조건식_상태", rowIndex].Value = "시작하기"; // 상태버튼
            tradingConditionDataGridView["매매조건식_삭제", rowIndex].Value = "삭제하기"; // 삭제버튼
            // 열의 높이지정
            gMainForm.tradingConditionDataGridView.Rows[rowIndex].Height = 30;
        }

        // 수정하기 버튼클릭시 데이터그리드뷰 내용 수정
        public void reSetAddMyTradingConditionToDataGridView(MyTradingCondition mtc, int rowIndex)
        {
            tradingConditionDataGridView["매매조건식_종목투자금", rowIndex].Value = mtc.itemInvestment.ToString("N0"); // 종목투자금
            tradingConditionDataGridView["매매조건식_매수종목수", rowIndex].Value = mtc.buyingItemCount.ToString("N0"); // 매수종목수
            tradingConditionDataGridView["매매조건식_실매수종목수", rowIndex].Value = "0"; // 매수종목수

            // 매수
            if (mtc.buyingUsing == 0)
            {
                tradingConditionDataGridView["매매조건식_매수", rowIndex].Value = "사용안함";
            }
            else
            {
                if (mtc.buyingType == 0) // 기본매수
                {
                    if (mtc.buyingTransferType == 0)
                        tradingConditionDataGridView["매매조건식_매수타입", rowIndex].Value = "편입즉시매수";
                    else
                        tradingConditionDataGridView["매매조건식_매수타입", rowIndex].Value = "조건매수";
                }
                else
                {
                    string[] _str = { "이동평균선", "스토캐스틱", "볼린저밴드", "엔벨로프" };
                    tradingConditionDataGridView["매매조건식_매수타입", rowIndex].Value = _str[mtc.buyingType - 1];
                }
            }
            // 추매
            if (mtc.buyingCount == 1)
                tradingConditionDataGridView["매매조건식_추매", rowIndex].Value = "사용안함";
            else
            {
                if (mtc.reBuyingType == 0)
                    tradingConditionDataGridView["매매조건식_추매", rowIndex].Value = "기본추매";
                else
                    tradingConditionDataGridView["매매조건식_추매", rowIndex].Value = "이동평균선";
            }
            // 익절
            if (mtc.takeProfitUsing == 0)
            {
                tradingConditionDataGridView["매매조건식_익절", rowIndex].Value = "사용안함";
            }
            else
            {
                string[] _str = { "기본익절", "이동평균선", "스토캐스틱", "볼린저밴드", "엔벨로프" };
                tradingConditionDataGridView["매매조건식_익절", rowIndex].Value = _str[mtc.takeProfitType];
            }
            // 손절
            if (mtc.stopLossUsing == 0)
            {
                tradingConditionDataGridView["매매조건식_손절", rowIndex].Value = "사용안함";
            }
            else
            {
                string[] _str = { "기본손절", "이동평균선", "볼린저밴드", "엔벨로프" };
                tradingConditionDataGridView["매매조건식_손절", rowIndex].Value = _str[mtc.stopLossType];
            }
        }

        public void tradingConditionCellsClickEvent(object sender, DataGridViewCellEventArgs e)
        {
            //조건식 데이터그리드뷰 셀클릭
            if(sender.Equals(tradingConditionDataGridView))
            {
                //행이 없가나 행의 인덱스가 0보다 작으면
                if (tradingConditionDataGridView.RowCount == 0 || e.RowIndex < 0)
                    return;
                //현재 선택한 행의 조건식 이름을 가져온다.
                string _conditionName = tradingConditionDataGridView["매매조건식_조건식", e.RowIndex].Value.ToString();

                //열 인덱스가 0보다 크고, 열의 갯수가 열열이 인덱스 보다 크거나 같을때
                if(0 <= e.ColumnIndex && e.ColumnIndex <= tradingConditionDataGridView.Columns.Count)
                {
                    if(tradingConditionDataGridView.Columns["매매조건식_상태"].Index == e.ColumnIndex) //시작하기 버튼을 클릭했을시
                    {
                        //gMyTradingConditionList과 같은 조건식 이름을 찾아낸다 
                        MyTradingCondition _mtc = gMyTradingConditionList.Find(o => o.conditionData.conditionName.Equals(_conditionName));
                        //최초 sendCondition()실행체크
                        if(!_mtc.m_CheckFirstRun) // 최초 실행
                        {
                            int _result = KiwoomAPI.SendCondition(_mtc.screenNumber, _mtc.conditionData.conditionName, _mtc.conditionData.conditionIndex, 1);
                            if(_result ==1) // 조건식호출요청 성공시
                            {
                                _mtc.m_conditionState = ConditionState.Playing; //EtcData 참고
                                _mtc.m_CheckFirstRun = true;

                                //버튼 텍스트 진행으로 수정 
                                tradingConditionDataGridView["매매조건식_상태", e.RowIndex].Value = "진행중";
                                setLogText("실시간 검색요청 성공 : " + _mtc.conditionData.conditionName);
                            }
                            else //실패
                            {
                                setLogText("실시간 검색요청 실패 : " + _mtc.conditionData.conditionName);
                                setLogText("잠시 후 다시 요청해주세요.");
                            }
                        }
                        else
                        {
                            if(_mtc.m_conditionState == ConditionState.Playing) // 현재 진행중이면
                            {
                                _mtc.m_conditionState = ConditionState.Stop;
                                tradingConditionDataGridView["매매조건식_상태", e.RowIndex].Value = "일시정지";
                            }
                            else // 현재 일시정지상태라면
                            {
                                _mtc.m_conditionState = ConditionState.Playing;
                                tradingConditionDataGridView["매매조건식_상태", e.RowIndex].Value = "진행중";
                            }

                        }
                    }
                    if (tradingConditionDataGridView.Columns["매매조건식_삭제"].Index == e.ColumnIndex) //삭제하기 버튼을 클릭했을시
                    {
                        //gMyTradingConditionList과 같은 조건식 이름을 찾아낸다 
                        MyTradingCondition _mtc = gMyTradingConditionList.Find(o => o.conditionData.conditionName.Equals(_conditionName));

                        if (tradingConditionDataGridView.Columns["매매조건식_삭제"].Index == e.ColumnIndex)//시작하기 버튼을 클릭했을시
                        {
                            if (_mtc != null)
                            {
                                // 스크린 번호를 받아옴
                                int screenIndex = int.Parse(_mtc.screenNumber) - (int)ConditionNumber.ConditionStartNum;

                                //조건검색식 등록시 배열 셋팅 -> 사용하지않는 스크린넘버로 false로변경
                                gMainForm.setConditionListCreateCheck(screenIndex, false);

                                // gMyTradingConditionList에서 해당 조건식 삭제
                                gMyTradingConditionList.Remove(_mtc);

                                // DataGridView에서 해당 Row 삭제
                                tradingConditionDataGridView.Rows.RemoveAt(e.RowIndex);

                                // 만약 필요한 경우, 삭제된 상태를 로그에 기록
                                gMainForm.setLogText($"조건식 '{_conditionName}' 삭제 완료.");
                            }
                        }
                    }
                }
            }
        }
        //실시간 조건검색 요청에 대한 수신 메서드
        private void OnReceiveRealConditionEvent(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            string _itemCode = e.sTrCode; // 종목코드
            string _type = e.strType; // 편입 "I" , 이탈 "D"
            string _conditionName = e.strConditionName; // 조건식 이름
            string _conditionIndex = e.strConditionIndex; // 조건식 번호
            string _itemName = gMainForm.KiwoomAPI.GetMasterCodeName(_itemCode);

            // 편입, 이탈한 조건식 클래스 찾는다.
            MyTradingCondition _mtc = gMyTradingConditionList.Find(o => o.conditionData.conditionName.Equals(_conditionName));
            if(_mtc != null)
            {
                if(_type.Equals("I")) // 편입
                {
                    //Console.WriteLine(_conditionName + " 종목편입: " + _itemName + " 남은 편입갯수: " + _mtc.remainingTransferItemCount + " 남은 매수갯수: " + _mtc.remainingBuyingItemCount);
                    // 현재 조건식검색이 일시정지 일 경우 return
                    if (_mtc.m_conditionState == ConditionState.Stop)
                        return;

                    // 매수할수 있는 남은 종목 갯수가 0보다 클때
                    // 매수할때마다 remainingBuyingItemCount를 차감
                    if(_mtc.remainingBuyingItemCount >0 && _mtc.remainingTransferItemCount>0)
                    {
                        //매매중인 종목 체크
                        bool _sameCheck = false;
                        foreach(string _s in transferSameCheckList)
                        {
                            if((_s.Equals(_itemCode))) //같은 종목이 있으면...
                            {
                                _sameCheck = true;
                                break;
                            }
                        }
                        if((!_sameCheck)) // 같은 종목이 없는 경우 리스트에 추가
                        {
                            transferSameCheckList.Add(_itemCode);
                            // 종목이름 GetMasterCodeName : 종목이름을 반환
                            // 매수종목수 차감(여기서 차감하지말자 -> 동전주때문에)
                            //_mtc.remainingBuyingItemCount--;
                            // 종목 주식기본정보(opt10001)TR을 요청한다.
                            gTradingManager.sendRequestItemInfoTR(_itemCode, _conditionName);
                            if(_mtc.remainingTransferItemCount > 0)
                            {
                                gMainForm.setLogText("주식기본정보 TR 요청: " + _itemName);
                            }
                            // 편입 종목을 리스트에 추가한다.(종목코드;조건식)
                            string _str = _itemCode + ";" + _mtc.conditionData.conditionName;
                            g_InsertItemList.Add(_str);
                        }
                        else // 같은 종목이 있는 경우 return
                        {
                            return;
                        }

                        /*
                        // transferItemDataGridView에서 같은 조건식에 같은 종목이 있는지 확인한다.
                        bool _checkItem = false;
                        foreach(DataGridViewRow row in transferItemDataGridView.Rows)
                        {
                            if(row.Cells["편입_종목코드"].Value != null)
                            {
                                //종목코드와 조건식이 동일한 경우 편입 시간만 수정을 해준다
                                if(row.Cells["편입_종목코드"].Value.ToString().Equals(_itemCode) && row.Cells["편입_조건식"].Value.ToString().Equals(_conditionName))
                                {
                                    string _itemName = KiwoomAPI.GetMasterCodeName(_itemCode);
                                    _checkItem = true;
                                    row.Cells["편입_시간"].Value = DateTime.Now.ToString("HH:mm:ss");
                                    gMainForm.setLogText("종목 편입 : " + _itemName);
                                    break;
                                }
                            }
                        }
                        // 동일한 종목이 없는 경우 AddTransferItemToDataGridView메서드 호출
                        if(!_checkItem)
                        {
                            //종목이름 GetMasterCodeName : 종목이름을 반환해준다.
                            string _itemName = KiwoomAPI.GetMasterCodeName(_itemCode);
                            AddTransferItemToDataGridView(_itemName, _itemCode, _conditionName);
                            gMainForm.setLogText("종목 편입 : " + _itemName);

                            //매수종목수를 차감한다.
                            _mtc.remainingBuyingItemCount--;
                            // 종목 주식기본정보(opt10001)TR을 요청한다.
                            gTradingManger.sendRequestItemInfoTR(_itemCode, _conditionName);
                            gMainForm.setLogText("주식기본정보 TR 요청: " + _itemName);
                        }*/
                    }
                }
                else if(_type.Equals("D")) //이탈
                {
                    /*
                    // 이탈하게 되면 transferItemDataGridView에서 삭제를 시킨다.
                    foreach(DataGridViewRow row in transferItemDataGridView.Rows)
                    {
                        if(row.Cells["편입_종목코드"].Value != null)
                        {
                            //종목코드와 조건식이 동일한 경우 편입 시간만 수정을 해준다.
                            if (row.Cells["편입_종목코드"].Value.ToString().Equals(_itemCode) && row.Cells["편입_조건식"].Value.ToString().Equals(_conditionName))
                            {
                                string _itemName = KiwoomAPI.GetMasterCodeName(_itemCode);
                                transferItemDataGridView.Rows.Remove(row);
                                gMainForm.setLogText("종목 이탈 : " + _itemName);
                                break;
                            }
                        }
                    }*/
                }
            }
        }

        //즉시매도 버튼 클릭시 수동매도 이벤트
        private void tradingItemCellClickEvent(object sender, DataGridViewCellEventArgs e)
        {
            // 행이 없거나 행의 인덱스가 0보다 작으면
            if (tradingItemDataGridView.RowCount == 0 || e.RowIndex < 0)
                return;
            //구분
            string _division = tradingItemDataGridView["매매진행_구분", e.RowIndex].Value.ToString();
            //종목코드
            string _itemCode = tradingItemDataGridView["매매진행_종목코드", e.RowIndex].Value.ToString();
            //조건식
            string _conditionName = tradingItemDataGridView["매매진행_조건식", e.RowIndex].Value.ToString();
            //진행상황
            string _state = tradingItemDataGridView["매매진행_진행상황", e.RowIndex].Value.ToString();

            // 열 인덱스가 0보다 크고, 열의 갯수가 열의 인덱스보다 크거나 같을때
            if (0<= e.ColumnIndex && e.ColumnIndex <= tradingItemDataGridView.Columns.Count)
            {
                if(tradingItemDataGridView.Columns["매매진행_즉시매도"].Index == e.ColumnIndex) //매도하기 버튼클릭
                {
                    if(_division == "보유")
                    {
                        MyHoldingItemOption _mho = gMainForm.gMyHoldingItemOptionList.Find(o => o.division.Equals(_division));
                        if (_mho != null)
                        {
                            MyHoldingItem _mhi = _mho.MyHoldingItemList.Find(o => o.m_itemCode.Equals(_itemCode));
                            if (_mhi != null)
                            {
                                if (_state.Equals("매도완료") && _mhi.m_bSold) // 종목삭제 클릭시 종목을 삭제
                                {
                                    string itemName = gMainForm.KiwoomAPI.GetMasterCodeName(_itemCode); // 종목명
                                    string Message = "종목명 : " + itemName + "\n" + "종목을 삭제하시겠습니까?";
                                    DialogResult Result = MessageBox.Show(Message, "종목삭제하기", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (Result == DialogResult.Yes)
                                    {
                                        // 리스트 삭제하기
                                        _mho.MyHoldingItemList.Remove(_mhi);
                                        // 그리드뷰 삭제하기
                                        tradingItemDataGridView.Rows.RemoveAt(e.RowIndex);
                                    }
                                    return;
                                }
                                else
                                {
                                    if (_mhi.m_bSold)
                                    {
                                        MessageBox.Show("매도가 완료된 종목입니다.");
                                        return;
                                    }
                                    string _itemName = gMainForm.KiwoomAPI.GetMasterCodeName(_itemCode);
                                    string message = "종목명 : " + _itemName + "\n" + "종목을 매도하시겠습니까?";
                                    DialogResult result = MessageBox.Show(message, "종목매도하기", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                    if (result == DialogResult.Yes)
                                    {
                                        //매도하기
                                        int _qnt = _mhi.m_totalQnt; // 보유수량
                                        Task _task = new Task(() =>
                                        {
                                            long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                    "수동매도",                                     // 사용자 구분명
                                                                    gMainForm.GetScreenNumber(),   // 화면번호
                                                                    _mho.account,                                // 계좌번호 10자리
                                                                    2,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                    _itemCode,                                     // 종목코드 6자리                                                        
                                                                    _qnt,                                              // 주문수량
                                                                    0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                    "03", // 시장가                                  // 거래구분
                                                                    "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  

                                        if (_ret == 0) //수동매도 주문 성공
                                        {
                                            _mhi.m_bSold = true;
                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                            {
                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode)
                                                    && row.Cells["매매진행_구분"].Value.ToString().Equals(_mho.division))
                                                {
                                                    row.Cells["매매진행_진행상황"].Value = "매도중";
                                                    break;

                                                }
                                            }
                                        }

                                        });
                                        gMainForm.gOrderRequestManager.sendTaskData(_task);
                                    }
                                }
                            }
                        }
                    }
                    if(_division == "영웅문매수")
                    {
                        PassitiveBuyingItem _pbi2 = gMainForm.gMyPassitiveBuyingItemList.Find(o => o.division.Equals(_division));
                        {
                            if(_pbi2 != null)
                            {
                                PassitiveBuyingItem _pbi = gMainForm.gMyPassitiveBuyingItemList.Find(o => o.itemCode.Equals(_itemCode));

                                if(_pbi != null)
                                {
                                    if (_state.Equals("매도완료") && _pbi.bsold) // 종목삭제 클릭시 종목을 삭제
                                    {
                                        string itemName = gMainForm.KiwoomAPI.GetMasterCodeName(_itemCode); // 종목명
                                        string Message = "종목명 : " + itemName + "\n" + "종목을 삭제하시겠습니까?";
                                        DialogResult Result = MessageBox.Show(Message, "종목삭제하기", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        if (Result == DialogResult.Yes)
                                        {
                                            // 리스트 삭제하기
                                            gMainForm.gMyPassitiveBuyingItemList.Remove(_pbi);
                                            // 그리드뷰 삭제하기
                                            tradingItemDataGridView.Rows.RemoveAt(e.RowIndex);
                                        }
                                        return;
                                    }
                                    else
                                    {
                                        if (_pbi.bsold)
                                        {
                                            MessageBox.Show("매도가 완료된 종목입니다.");
                                            return;
                                        }
                                        string _itemName = gMainForm.KiwoomAPI.GetMasterCodeName(_itemCode);
                                        string message = "종목명 : " + _itemName + "\n" + "종목을 매도하시겠습니까?";
                                        DialogResult result = MessageBox.Show(message, "종목매도하기", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                        if (result == DialogResult.Yes)
                                        {
                                            //매도하기
                                            int _qnt = _pbi.balanceQnt; // 보유수량
                                            Task _task = new Task(() =>
                                            {
                                                long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                        "수동매도",                                     // 사용자 구분명
                                                                        gMainForm.GetScreenNumber(),   // 화면번호
                                                                        gMainForm.myAccountComboBox.Text,                                // 계좌번호 10자리
                                                                        2,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                        _itemCode,                                     // 종목코드 6자리                                                        
                                                                        _qnt,                                              // 주문수량
                                                                        0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                        "03", // 시장가                                  // 거래구분
                                                                        "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  

                                            if (_ret == 0) //수동매도 주문 성공
                                            {
                                                _pbi.bsold = true;
                                                foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                                {
                                                    if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode)
                                                        && row.Cells["매매진행_구분"].Value.ToString().Equals(_pbi2.division))
                                                    {
                                                        row.Cells["매매진행_진행상황"].Value = "매도중";
                                                        break;

                                                    }
                                                }
                                            }
                                            });
                                            gMainForm.gOrderRequestManager.sendTaskData(_task);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //조건식과 종목코드가 같은 종목을 찾아낸다.
                        MyTradingCondition _mtc = gMainForm.gMyTradingConditionList.Find(o => o.conditionData.conditionName.Equals(_conditionName));
                        if (_mtc != null)
                        {
                            MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(_itemCode)); // 종목코드가 같은 종목 찾음
                            if (_mti != null)
                            {
                                if (_state.Equals("주문취소") && !_mti.m_bSold) // 종목삭제 클릭시 종목을 삭제
                                {
                                    string itemName = gMainForm.KiwoomAPI.GetMasterCodeName(_itemCode); // 종목명
                                    string Message = "종목명 : " + itemName + "\n" + "종목을 삭제하시겠습니까?";
                                    DialogResult Result = MessageBox.Show(Message, "종목삭제하기", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (Result == DialogResult.Yes)
                                    {
                                        // 리스트 삭제하기
                                        _mtc.MyTradingItemList.Remove(_mti);
                                        // 그리드뷰 삭제하기
                                        tradingItemDataGridView.Rows.RemoveAt(e.RowIndex);
                                    }
                                    return;
                                }

                                if (_state.Equals("매도완료") && _mti.m_bSold) // 종목삭제 클릭시 종목을 삭제
                                {
                                    string itemName = gMainForm.KiwoomAPI.GetMasterCodeName(_itemCode); // 종목명
                                    string Message = "종목명 : " + itemName + "\n" + "종목을 삭제하시겠습니까?";
                                    DialogResult Result = MessageBox.Show(Message, "종목삭제하기", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (Result == DialogResult.Yes)
                                    {
                                        // 리스트 삭제하기
                                        _mtc.MyTradingItemList.Remove(_mti);
                                        // 그리드뷰 삭제하기
                                        tradingItemDataGridView.Rows.RemoveAt(e.RowIndex);
                                    }
                                    return;
                                }
                                else
                                {
                                    //매수가 되었고, 아직 매도가 안된 종목인지 확인
                                    if (!_mti.m_bCompletePurchase)
                                    {
                                        MessageBox.Show("매수가 완료되지 않은 종목입니다.");
                                        return;
                                    }

                                    if (_mti.m_bSold)
                                    {
                                        MessageBox.Show("매도가 완료된 종목입니다.");
                                        return;
                                    }
                                    string _itemName = gMainForm.KiwoomAPI.GetMasterCodeName(_itemCode);
                                    string message = "종목명 : " + _itemName + "\n" + "종목을 매도하시겠습니까?";
                                    DialogResult result = MessageBox.Show(message, "종목매도하기", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                    if (result == DialogResult.Yes)
                                    {
                                        //매도하기
                                        int _qnt = _mti.m_totalQnt; // 보유수량
                                        Task _task = new Task(() =>
                                        {
                                            long _ret = gMainForm.KiwoomAPI.SendOrder(
                                                                    "수동매도",                                     // 사용자 구분명
                                                                    gMainForm.GetScreenNumber(),   // 화면번호
                                                                    _mtc.account,                                // 계좌번호 10자리
                                                                    2,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                    _itemCode,                                     // 종목코드 6자리                                                        
                                                                    _qnt,                                              // 주문수량
                                                                    0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                    "03", // 시장가                                  // 거래구분
                                                                    "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  

                                        if (_ret == 0) //수동매도 주문 성공
                                        {
                                            _mti.m_bSold = true;
                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                            {
                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode)
                                                    && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mti.m_conditionName))
                                                {
                                                    row.Cells["매매진행_진행상황"].Value = "매도중";
                                                    break;

                                                }
                                            }
                                        }
                                        });
                                        gMainForm.gOrderRequestManager.sendTaskData(_task);
                                    }

                                }
                            }
                        }
                    }
                }    
                else if(tradingItemDataGridView.Columns["매매진행_수동매수"].Index == e.ColumnIndex)//수동매수버튼클릭
                {
                    MyTradingCondition _mtc = gMyTradingConditionList.Find(o => o.conditionData.conditionName.Equals(_conditionName));
                    if (_mtc != null)
                    {
                        MyTradingItem _mti = _mtc.MyTradingItemList.Find(o => o.m_itemCode.Equals(_itemCode));
                        if (_mti != null)
                        {
                            if(!_mti.m_bCompletePurchase) //매수 완료여부확인
                            {
                                //조건식설정창에 편입가격대비매수이고, m_bPurchased가 false일때
                                if (_mtc.buyingTransferType == 1 && !_mti.m_bPurchased)
                                {
                                    string _itemName = KiwoomAPI.GetMasterCodeName(_itemCode); //종목명
                                    string message = "종목명 : " + _itemName + "\n" + "종목을 매수하시겠습니까?";
                                    DialogResult result = MessageBox.Show(message, "종목매수하기", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (result == DialogResult.Yes)
                                    {
                                        if (_mti.m_bCompletePurchase)
                                        {
                                            MessageBox.Show("매수가 완료된 종목입니다.");
                                            return;
                                        }
                                        if(_mti.m_bPurchased)
                                        {
                                            MessageBox.Show("매수주문이 들어간 종목입니다.");
                                            return;
                                        }
                                        // 주문수량계산 : 시장가의 경우 상한가를 기준으로 수량을 계산한다.
                                        int _qnt = (int)(_mtc.itemInvestment / _mti.m_upperLimitPrice);
                                        Task _task = new Task(() =>
                                        {
                                            long _ret = KiwoomAPI.SendOrder(
                                                                    "수동매수",                                // 사용자 구분명
                                                                    gMainForm.GetScreenNumber(),   // 화면번호
                                                                    _mtc.account,                                // 계좌번호 10자리
                                                                    1,                                                  // 주문유형 1:신규매수,2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
                                                                    _itemCode,                                     // 종목코드 6자리                                                        
                                                                    _qnt,                                              // 주문수량
                                                                    0,                                                   // 주문가격 : 시장가일때는 0이다.
                                                                    "03", // 시장가                                  // 거래구분
                                                                    "");                                                // 원주문번호: 정장, 취소시 사용, 신규주문은 공백으로 처리  
                                        if (_ret == 0) // 성공
                                        {
                                            // 더이상 매수가 안되도록 _mti.m_bPurchased를 true로 설정한다.
                                            _mti.m_bPurchased = true;

                                            foreach (DataGridViewRow row in gMainForm.tradingItemDataGridView.Rows)
                                            {
                                                if (row.Cells["매매진행_종목코드"].Value.ToString().Equals(_itemCode) && row.Cells["매매진행_조건식"].Value.ToString().Equals(_mtc.conditionData.conditionName))
                                                {
                                                    row.Cells["매매진행_진행상황"].Value = "매수중";
                                                    break;
                                                }
                                            }
                                            gMainForm.setLogText("수동매수 성공 : " + _itemCode);
                                        }
                                        else // 실패
                                        {
                                            gMainForm.setLogText("수동매수 실패 : " + _itemCode);
                                        }
                                        });
                                        gMainForm.gOrderRequestManager.sendTaskData(_task);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("매수중인 종목입니다.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("매수된 종목입니다.");
                            }
                        }
                    }
                }
            }
        }

        public void tradingConditionCellDoubleClickEvent(object sender, DataGridViewCellEventArgs e)
        {
            if (tradingConditionDataGridView.RowCount == 0 || e.RowIndex < 0)
                return;
            
        }
        /*
        // 편입 종목 추가 메서드
        private void AddTransferItemToDataGridView(string itemName, string itemCode, string conditionName)
        {
            // rowIndex  = 몇 번째 줄인지 리턴
            int rowindex = transferItemDataGridView.Rows.Add(); // 그리드뷰에 한 줄이 추가됨

            transferItemDataGridView["편입_종목명", rowindex].Value = itemName; //종목명
            transferItemDataGridView["편입_종목코드", rowindex].Value = itemCode; // 종목코드
            transferItemDataGridView["편입_조건식", rowindex].Value = conditionName; // 조건식이름
            transferItemDataGridView["편입_시간", rowindex].Value = DateTime.Now.ToString("HH:mm:ss"); // 편입시간
            // 열의 높이지정
            transferItemDataGridView.Rows[rowindex].Height = 26;
            // 맨 아래열로 이동
            transferItemDataGridView.FirstDisplayedScrollingRowIndex = transferItemDataGridView.Rows.Count - 1;
        }
        */
        // tradingItemDataGridView 추가 메서드
        public void AddMyTradingItemToDataGridView(MyTradingItem mti, double _itemInvestment)
        {
            if (tradingItemDataGridView.InvokeRequired) // 다른쓰레드에서 호출시 처리(AddMyTradingItemToDataGridView가 TradingManager 클래스에서 실행될때 처리하기위함)
            {
                SafeCallAddMyTradingItemToDataGridView d = new SafeCallAddMyTradingItemToDataGridView(AddMyTradingItemToDataGridView);
                tradingItemDataGridView.Invoke(d, new object[] { mti, _itemInvestment });
            }
            else
            {
                // rowIndex  = 몇 번째 줄인지 리턴

                int rowIndex = tradingItemDataGridView.Rows.Add(); // 그리드뷰에 한 줄이 추가됨

                string _itemName = KiwoomAPI.GetMasterCodeName(mti.m_itemCode);
                tradingItemDataGridView["매매진행_구분", rowIndex].Value = mti.m_conditionName; // 조건식
                tradingItemDataGridView["매매진행_종목명", rowIndex].Value = _itemName; // 종목명
                tradingItemDataGridView["매매진행_종목코드", rowIndex].Value = mti.m_itemCode; // 종목코드
                tradingItemDataGridView["매매진행_조건식", rowIndex].Value = mti.m_conditionName; // 조건식
                tradingItemDataGridView["매매진행_총투자금", rowIndex].Value = _itemInvestment.ToString("N0"); // 총투자금
                tradingItemDataGridView["매매진행_편입가격", rowIndex].Value = mti.m_currentPrice.ToString("N0"); // 편입가격
                tradingItemDataGridView["매매진행_편입대비수익률", rowIndex].Value = "0.00%"; // 편입대비수익률
                tradingItemDataGridView["매매진행_현재가", rowIndex].Value = mti.m_currentPrice.ToString("N0"); // 현재가
                tradingItemDataGridView["매매진행_매입금", rowIndex].Value = "0"; // 매입금
                tradingItemDataGridView["매매진행_보유수량", rowIndex].Value = "0"; // 보유수량            
                tradingItemDataGridView["매매진행_주문가능수량", rowIndex].Value = "0"; // 주문가능수량
                tradingItemDataGridView["매매진행_매입가", rowIndex].Value = "0"; // 매입가
                tradingItemDataGridView["매매진행_평가손익", rowIndex].Value = "0"; // 평가손익
                tradingItemDataGridView["매매진행_수익률", rowIndex].Value = "0.00%"; // 수익률
                tradingItemDataGridView["매매진행_진행상황", rowIndex].Value = "대기중"; // 진행상황
                tradingItemDataGridView["매매진행_등락율", rowIndex].Value = "0.00%"; // 등락율

                // 열의 높이지정
                tradingItemDataGridView.Rows[rowIndex].Height = 26;

                // 조건식으로 실제 매수된 종목 갯수 갱신하기
                //tradingConditionDataGridView의 [매매조건식_실매수종목수]부분
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
        }
        //주문상태 데이터그리드뷰
        public void AddOrderToDataGridView(string itemName, string conditionName, string orderNumber, string orderTime, int qnt, int price, string orderType, string priceType)
        {
            // rowIndex = 몇 번째 줄인지 리턴
            int rowIndex = orderDataGridView.Rows.Add(); // 그리드뷰에 한줄이 추가됨

            orderDataGridView["주문_종목명", rowIndex].Value = itemName; // 종목명
            orderDataGridView["주문_조건식", rowIndex].Value = conditionName; // 조건식
            orderDataGridView["주문_주문번호", rowIndex].Value = orderNumber; // 주문번호
            orderDataGridView["주문_주문시간", rowIndex].Value = orderTime; // 주문시간
            orderDataGridView["주문_주문량", rowIndex].Value = qnt; // 주문량
            orderDataGridView["주문_주문가격", rowIndex].Value = price; // 주문가격
            orderDataGridView["주문_매매구분", rowIndex].Value = orderType; // 매매구분
            orderDataGridView["주문_가격구분", rowIndex].Value = priceType; //가격구분

            // 열의 높이지정
            orderDataGridView.Rows[rowIndex].Height = 26;
            orderDataGridView.FirstDisplayedScrollingRowIndex = orderDataGridView.Rows.Count - 1;
        }

        //체결상태 데이터그리드뷰
        public void AddConclusionToDataGridView(string itemName, string orderNumber, string orderTime, int orderQnt, int conclusionQnt, int unitConclusionQnt, int conclusionPrice, string orderType)
        {
            // rowIndex = 몇 번째 줄인지 리턴
            int rowIndex = conclusionDataGridView.Rows.Add(); // 그리드뷰에 한줄이 추가됨

            conclusionDataGridView["체결_종목명", rowIndex].Value = itemName; // 종목명
            conclusionDataGridView["체결_주문번호", rowIndex].Value = orderNumber; // 주문번호
            conclusionDataGridView["체결_주문시간", rowIndex].Value = orderTime; // 주문시간
            conclusionDataGridView["체결_주문량", rowIndex].Value = orderQnt.ToString("N0"); //주문량
            conclusionDataGridView["체결_체결량", rowIndex].Value = conclusionQnt.ToString("N0"); //체결량
            conclusionDataGridView["체결_단위체결량", rowIndex].Value = unitConclusionQnt.ToString("N0"); //단위체결량
            conclusionDataGridView["체결_체결가", rowIndex].Value = conclusionPrice.ToString("N0"); //체결가
            conclusionDataGridView["체결_매매구분", rowIndex].Value = orderType; // 매매구분

            //열의 높이지정
            conclusionDataGridView.Rows[rowIndex].Height = 26;
            conclusionDataGridView.FirstDisplayedScrollingRowIndex = conclusionDataGridView.Rows.Count - 1;
        }

        // 매도수익률및평가손익 데이터그리드뷰
        public void AddsoldToDataGridview(string itemName, string soldTime, int conclusionQnt, double conclusionPrice ,double rateOfReturn, double evaluationProfitLoss)
        {
            //rowIndex = 몇번째 줄인지 리턴
            int rowIndex = soldDataGridView.Rows.Add(); // 그리드뷰에 한줄을 추가함

            soldDataGridView["매도_종목명", rowIndex].Value = itemName; //종목명
            soldDataGridView["매도_매도시간", rowIndex].Value = soldTime; //매도시간
            soldDataGridView["매도_매도량", rowIndex].Value = conclusionQnt.ToString("N0"); ; //매도량
            soldDataGridView["매도_매도가격", rowIndex].Value = conclusionPrice.ToString("N0"); ; //매도가격
            soldDataGridView["매도_평가손익", rowIndex].Value = gMainForm.setUpDownArrow(evaluationProfitLoss); //평가손익
            soldDataGridView["매도_수익률", rowIndex].Value = rateOfReturn.ToString("N2") +"%" ; // 수익률

            // 열의 높이지정
            soldDataGridView.Rows[rowIndex].Height = 26;
            soldDataGridView.FirstDisplayedScrollingRowIndex = soldDataGridView.Rows.Count - 1;
        }

        // 텍스트 앞 화살표 모양 출력
        public string setUpDownArrow(double num)
        {
            string _ret = string.Empty;
            double _number = Math.Abs(num);

            if (num == 0)
                _ret = _number.ToString("N0");
            else if (num < 0)
                _ret = "▼ " + _number.ToString("N0");
            else if (num > 0)
                _ret = "▲ " + _number.ToString("N0");

            return _ret;
        }
        //수익률 표기
        public string setUpDownRateOfReturn(double num)
        {
            string _ret = string.Empty;
            double _number = Math.Abs(num);

            if (num == 0)
                _ret = _number.ToString("N2") + "%";
            else if (num < 0)
                _ret = "- " + _number.ToString("N2") + "%";
            else if (num > 0)
                _ret = _number.ToString("N2") + "%";

            return _ret;
        }

        // 셀 내용의 서식을 변경하는 메서드
        public void tradingItemDataGridView_CellFormatting(object sender , DataGridViewCellFormattingEventArgs e)
        {
            if (tradingItemDataGridView.Columns[e.ColumnIndex].Name == "매매진행_평가손익")
            {
                if (e.Value != null)
                {
                    if (e.Value.ToString() == "0")
                    {
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        if (e.Value.ToString().Contains("▼"))
                            e.CellStyle.ForeColor = Color.Blue;
                        else
                            e.CellStyle.ForeColor = Color.Red;
                    }
                }
            }
            if (tradingItemDataGridView.Columns[e.ColumnIndex].Name == "매매진행_수익률" || tradingItemDataGridView.Columns[e.ColumnIndex].Name == "매매진행_편입대비수익률")
            {
                if (e.Value.ToString() == "0.00%")
                {
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    if (e.Value.ToString().Contains("-"))
                        e.CellStyle.ForeColor = Color.Blue;
                    else
                        e.CellStyle.ForeColor = Color.Red;
                }
            }
        }
        private void soldDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (soldDataGridView.Columns[e.ColumnIndex].Name == "매도_평가손익")
            {
                if (e.Value != null)
                {
                    if (e.Value.ToString() == "0")
                    {
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        if (e.Value.ToString().Contains("▼"))
                            e.CellStyle.ForeColor = Color.Blue;
                        else
                            e.CellStyle.ForeColor = Color.Red;
                    }
                }
            }
            if (soldDataGridView.Columns[e.ColumnIndex].Name == "매도_수익률")
            {
                if (e.Value.ToString() == "0.00%")
                {
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    if (e.Value.ToString().Contains("-"))
                        e.CellStyle.ForeColor = Color.Blue;
                    else
                        e.CellStyle.ForeColor = Color.Red;
                }
            }
        }

        // OnReceiveRealConditionEvent에서 실시간으로 들어온 종목과 조건식을 분리한 뒤에 setInsertItemSetting 메서드를 호출해서 그리드뷰 추가및 1분봉 데이터 요청
        public void call_GlobalTimer(object o)
        {
            // 1분봉 데이터 받고 있는지 확인 -> 최초값은 false이고 true일경우 작업이 완료될때까지 대기함.
            if (g_bGetBunBong)
                return;

            // 리스트 갯수
            int _count = g_InsertItemList.Count;
            if(_count != 0) // 편입 종목 저장 리스트의 갯수가 0이 아니면
            {
                // 리스트 첫번째 데이터
                string _str = g_InsertItemList[0];
                string[] _rqName = _str.Split(';');
                string _itemCode = _rqName[0]; // 종목코드
                string _conditionName = _rqName[1]; // 편입 조건식

                //1분봉 데이터 요청하기
                //getBunBongData(_itemCode, _conditionName, 0, GetScreenNumber());
                g_InsertItemList.Remove(_str); // 리스트에 삭제한다.
                // 1분봉데이터 처리중 변수 true
                g_bGetBunBong = true;
            }
        }
        // 그리드뷰 추가
        public void setInsertItemSetting(string itemCode, string conditionName)
        {
            //조건식 리스트에서 조건식 이름이 같은 것을 찾아낸다
            MyTradingCondition _mtc = gMainForm.gMyTradingConditionList.Find(o => o.conditionData.conditionName.Equals(conditionName));
            if(_mtc != null)
            {
                // MyTradingItem생성
                MyTradingItem _mti = new MyTradingItem(_mtc,conditionName, _mtc.conditionData.conditionIndex.ToString(), itemCode, 0, 0, 0); //편입가, 현재가, 상한가는 0으로 세팅
                // MyTradingItemList에 추가
                _mtc.MyTradingItemList.Add(_mti);
                //tradingItemDataGridView에 추가하기
                AddMyTradingItemToDataGridView(_mti, _mtc.itemInvestment);
                //실시간시세등록
                Task _task = new Task(() =>
                {
                    string fidList = "10;11;12;13;15;20;228;302;9001";
                    gMainForm.KiwoomAPI.SetRealReg(_mtc.screenNumber, itemCode, fidList, "1");
                });
                gMainForm.gCheckRequestManager.sendTaskData(_task);
                //1분봉 데이터 요청하기
                //getBunBongData(itemCode, conditionName, 0, GetScreenNumber());
            }
        }
        // 서버에 1분봉 데이터 요청
        public void getBunBongData(string ItemCode, string conditionName, int searchOption, string screenNumber)
        {
            string _itemName = KiwoomAPI.GetMasterCodeName(ItemCode);

            Task _task = new Task(() =>
            {
                KiwoomAPI.SetInputValue("종목코드", ItemCode);
                KiwoomAPI.SetInputValue("틱범위", "1");
                KiwoomAPI.SetInputValue("수정주가구분", "1");
                KiwoomAPI.CommRqData("1분봉5;" + conditionName, "opt10080", searchOption, screenNumber);
                //setLogText("1분봉차트조회 : " + _itemName);
            });
            gCheckRequestManager.sendTaskData(_task);
        }

        //서버에 일봉 데이터 요청
        public void getDayBongData(string itemCode, string conditionName, int searchOption, string screenNumber)
        {
            string _itemName = KiwoomAPI.GetMasterCodeName(itemCode);

            Task _task = new Task(() =>
            {
                string strToday = DateTime.Today.ToString("yyyyMMdd");
                gMainForm.KiwoomAPI.SetInputValue("종목코드", itemCode);
                gMainForm.KiwoomAPI.SetInputValue("기준일자", strToday);
                gMainForm.KiwoomAPI.SetInputValue("수정주가구분", "1");
                gMainForm.KiwoomAPI.CommRqData("1일봉1;" + conditionName, "opt10081", searchOption, screenNumber);
                //setLogText("일봉차트조회 : " + _itemName);
            });
            gCheckRequestManager.sendTaskData(_task);
        }
        // 호가 단위 오류수정을 위한 코드
        public int RoundToTick(int price)
        {
            int tickSize;
            if(price >= 5000 && price < 20000)          // 5000원 이상 20000원 미만 -> 10원 단위 반올림
                tickSize = 10;                        
            else if (price >= 20000 && price < 50000)   // 20000원 이상 50000원 미만 → 50원 단위 반올림
                tickSize = 50;
            else if (price >= 50000 && price < 500000) // 50000원 이상 500000원 미만 → 100원 단위 반올림
                tickSize = 100;
            else if (price >= 500000)                  // 500000원 이상 → 1000원 단위 반올림
                tickSize = 1000;
            else                                        // 5000원 미만은 조정 없이 그대로 반환
                return price;

            return (int)Math.Round(price / (double)tickSize) * tickSize;
        }
        // 지정가 매수 정정을 위한 임시
        public int PlusToTick(double price)
        {
            int tickSize;
            if (price < 2000)
                tickSize = 1;
            else if (price >= 2000 && price < 5000)
                tickSize = 5;
            else if (price >= 5000 && price < 20000)          // 5000원 이상 20000원 미만 -> 10원 단위 +
                tickSize = 10;
            else if (price >= 20000 && price < 50000)   // 20000원 이상 50000원 미만 → 50원 단위 +
                tickSize = 50;
            else if (price >= 50000 && price < 200000) // 50000원 이상 500000원 미만 → 100원 단위 +
                tickSize = 100;
            else if (price >= 200000 && price < 500000)
                tickSize = 500;
            else if (price >= 500000)                  // 500000원 이상 → 1000원 단위 +
                tickSize = 1000;
            else                                        // 5000원 미만은 조정 없이 그대로 반환
                return (int)price;

            return (int)(price + tickSize);
        }

        // 호가 단위
        public int getHogaUnitPrice(int price)
        {
            int _unit = 0;
            if (price >= 2000 && price < 5000) // 5원 단위
                _unit = 5;
            else if (price >= 5000 && price < 20000) // 10원 단위
                _unit = 10;
            else if (price >= 20000 && price < 50000) // 50원 단위
                _unit = 50;
            else if (price >= 50000 && price < 200000) // 100원 단위
                _unit = 100;
            else if (price >= 200000 && price < 500000) // 500원 단위
                _unit = 500;
            else if (price >= 500000) // 1000원 단위
                _unit = 1000;
            else
                _unit = 1; // 1원 단위
            return _unit;
        }
        // 상한가 계산
        public int getUpperLimitPrice(int lastprice)
        {
            int _unit1 = 0, _unit2 = 0; ;
            int _calPrice = (int)(lastprice * 0.3);
            int _sum = 0;

            _unit1 = getHogaUnitPrice(_calPrice);  // 계산된 가격의 호가 단위
            if (_unit1 > 1)
            {
                _calPrice = _calPrice - (_calPrice % _unit1); // 호가단위 가격 미만 절사
                /*
                    (_calPrice % _unit1) 의 예시계산법, 
                    몫이 아닌 나머지를 계산하는거임
                    ex1) 15 % 2 는 몫이 7 나머지가 1 이므로 1값이 나옴    

                    ex2)_calPrice가 43,710 _unit1이 50이라고 가정
                    1. 43,710 ÷ 50을 합니다. 몫은 874입니다.
                    2. 그 다음에 이 몫을 다시 50과 곱합니다: 874 × 50 = 43,700.
                    3. 마지막으로, 원래 숫자에서 이 값을 뺍니다: 43,710 - 43,700 = 10.
                */
            }
            _sum = lastprice + _calPrice; // 전날종가 + _calPrice
            _unit2 = getHogaUnitPrice(_sum);  // 계산된 가격의 호가 단위
            if (_unit2 > 1)
            {
                _sum = _sum - (_sum % _unit2); // 호가단위 가격 미만 절사
            }
            return _sum;
        }
        private void tradingItemDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            gMainForm.g_selectItemCode = tradingItemDataGridView["매매진행_종목코드", e.RowIndex].Value.ToString();
            gMainForm.g_selectItemName = tradingItemDataGridView["매매진행_종목명", e.RowIndex].Value.ToString();
            gTradingManager.DrawIndicateData(gMainForm.g_selectItemCode);

            var filteredLogs = globalLogs2.Where(log => log != null && log.Contains(g_selectItemName)).ToList();
            UpdateLogListBox(filteredLogs);
            /*
            logListBox.Items.Clear();
            foreach (var log in filteredLogs)
            {
                logListBox.Items.Add(log);
            }*/
        }
        private void bunBongDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            bunBongDataGridView.CurrentCell = null;
            bunBongDataGridView.ClearSelection();
        }
        private void orderDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            orderDataGridView.CurrentCell = null;
            orderDataGridView.ClearSelection();
        }

        private void conclusionDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            conclusionDataGridView.CurrentCell = null;
            conclusionDataGridView.ClearSelection();
        }

        private void soldDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            soldDataGridView.CurrentCell = null;
            soldDataGridView.ClearSelection();
        }
    }
}
