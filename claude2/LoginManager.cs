using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockAutoTrade2
{
    public class LoginManager
    {
        private static LoginManager gLoginManagerInstance;
        public MainForm gMainForm = MainForm.GetInstance();
        
        private LoginManager()
        {
            gLoginManagerInstance = this;

            // 키움 로그인 이벤트 등록
            gMainForm.KiwoomAPI.OnEventConnect += onKiWoomLoginEvent;
            // 사용자 조건식 여청 결과 이벤트 등록
            gMainForm.KiwoomAPI.OnReceiveConditionVer += onReceiveConditionListEvent;
        }
        public static LoginManager GetInstance()
        {
            if (gLoginManagerInstance == null)
                gLoginManagerInstance = new LoginManager();

            return gLoginManagerInstance;
        }

        //키움 서버 로그인
        public void KiWoomServerLogin()
        {
            // KiwoomAPi : 키움 APi 컨트롤 변수명
            // GetConnectState() : 서버와 현재 접속 상태(0: 연결안됨 , 1: 연결됨)
            // CommConnect(): 키움 로그인 창 출력 매서드
            if (gMainForm.KiwoomAPI.GetConnectState() == 0)
            {
                gMainForm.setLogText("키움 로그인 시작");
                gMainForm.KiwoomAPI.CommConnect();
            }
        }
        // 키움 로그인 이벤트 처리 함수
        private void onKiWoomLoginEvent(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            if (e.nErrCode == 0) // 로그인 성공
            {
                // 계좌비밀번호 입력창을 출력(4자리 비밀번호)
                // KOA_Function 매서드 : OpenAPI기본 기능외에 기능을 사용하기 쉽도록 만든 함수
                gMainForm.setLogText("키움 로그인 성공");
                // 계좌비밀번호 입력창에서 닫기 버튼을 누른 후에 처리 할 일이 있으면 여기서 한다.
                /*
                 

                 */
                // 접속서버구분하기(1: 모의투자서버, 나머지: 실서버)
                gMainForm.sServerGubun = gMainForm.KiwoomAPI.GetLoginInfo("GetServerGubun");
                // 나의 보유계좌 확인
                // 계좌정보가 ;(세미콜론)으로 구분되어 반환됩니다((예) 00000001;00000002)
                string myAccountList = gMainForm.KiwoomAPI.GetLoginInfo("ACCLIST");
                // 계좌정보를 split을 사용하여 문자열 배열에 저장을 합니다.
                string[] myAccountArrayList = myAccountList.Split(';');

                foreach (string _account in myAccountArrayList)
                {
                    if (_account.Length > 0)
                    {
                        // 콤보박스에 나의계좌를 추가해 준다.
                        gMainForm.myAccountComboBox.Items.Add(_account);
                    }
                }
                //콤보박스 텍스트에 첫번쨰 계좌를 넣어준다.
                gMainForm.myAccountComboBox.Text = myAccountArrayList[0];

                //계좌가 2개 이상일 때 화면에 메시지박스를 출력해 준다.
                int _accountCount = gMainForm.myAccountComboBox.Items.Count;
                if (_accountCount > 1)
                {
                    string _str = "현재 나의 계좌입니다. 자동매매에서는\n사용하는 계좌를 잘 선택해 주세요.\n"
                        + myAccountArrayList[0] + "\n" + myAccountArrayList[1];
                    MessageBox.Show(_str);
                }
                gMainForm.setLogText("계좌비밀번호 입력창 닫기 버튼 클릭");

                // 사용자가 만들어 놓은 조건식 요청하기
                if (gMainForm.KiwoomAPI.GetConditionLoad() == 1)
                {
                    gMainForm.setLogText("사용자 조건식 요청 성공");
                    string _account = gMainForm.myAccountComboBox.Text; // 계좌번호

                    // 예수금 상세정보 요청
                    getAccountDetailDataTR(_account);
                    // 이때 Holdingitem.txt파일을 가져와서 gmainform.gmyholdingitemoption리스트에 추가를 해야한다.
                    // 잔고정보요청
                    gMainForm.setLogText("잔고 데이터 요청 성공");
                    Task _task = new Task(() =>
                    {
                        // 예수금 상세정보 요청
                        gMainForm.KiwoomAPI.SetInputValue("계좌번호", _account);
                        gMainForm.KiwoomAPI.SetInputValue("비밀번호", "");
                        gMainForm.KiwoomAPI.SetInputValue("상장폐지조회구분", "0");
                        gMainForm.KiwoomAPI.SetInputValue("비밀번호입력매체구분", "00");
                        gMainForm.KiwoomAPI.SetInputValue("거래소구분", "");
                        gMainForm.KiwoomAPI.CommRqData("계좌평가현황요청;보유" , "OPW00004", 0, gMainForm.GetScreenNumber());
                    });
                    gMainForm.gCheckRequestManager.sendTaskData(_task);

                }
                else
                {
                    gMainForm.setLogText("사용자 조건식 요청 실패");
                }
            }
            else if (e.nErrCode == -100) // 사용자 정보 교환 실패
            {
                gMainForm.setLogText("사용자 정보 교환 실패");
            }
            else if (e.nErrCode == -101) // 서버 접속 실패
            {
                gMainForm.setLogText("서버 접속 실패");
            }
            else if (e.nErrCode == -102) // 버전 처리 실패
            {
                gMainForm.setLogText("버전 처리 실패");
            }
            else // 기타 접속 실패
            {
                gMainForm.setLogText("알 수 없는 에러");
            }
        }

        public void getAccountDetailDataTR(string _account)
        {
            //string _account = gMainForm.myAccountComboBox.Text; // 계좌번호

            Task _task = new Task(() =>
            {
                // 예수금 상세정보 요청
                gMainForm.KiwoomAPI.SetInputValue("계좌번호", _account);
                gMainForm.KiwoomAPI.SetInputValue("비밀번호", "");
                gMainForm.KiwoomAPI.SetInputValue("비밀번호입력매체구분", "00");
                gMainForm.KiwoomAPI.SetInputValue("조회구분", "3");
                gMainForm.KiwoomAPI.CommRqData("예수금상세현황", "opw00001", 0, gMainForm.GetScreenNumber());
                //Console.WriteLine("(gMainForm.KiwoomAPI.CommRqData(예수금상세현황, opw00001, 0, gMainForm.GetScreenNumber()) " + gMainForm.KiwoomAPI.CommRqData("예수금상세현황", "opw00001", 0, gMainForm.GetScreenNumber()));
            });
            gMainForm.gCheckRequestManager.sendTaskData(_task);

            gMainForm.setLogText("예수금 상세 정보 요청");

        }
        private void onReceiveConditionListEvent(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
            //GetConditionNameList매서드를 사용하여 고유번호와 조건식 이름으로 구성된 문자열을 전달받는다
            //(예) 1^조건식1;2^조건식2;3^조건식3;........
            string sConditionData = gMainForm.KiwoomAPI.GetConditionNameList();
            //split매서드를 사용하여 문자열 배열에 저장을 한다
            string[] sConditionArray = sConditionData.Split(';');

            foreach (string _condition in sConditionArray)
            {
                if (_condition.Length > 0) // 마지막 공백이 들어가 있는것은 저장하지 않기 위함
                {
                    //split('^')매서드를 사용하여 고유번호와 이름을 분리합니다.AxKHOpenAPI_OnReceiveConditionVer
                    string[] conditionSplit = _condition.Split('^');
                    string index = conditionSplit[0];
                    string name = conditionSplit[1];

                    //분리한 고유번호와 이름을 conditionData객체를 생성
                    ConditionData conditionData = new ConditionData(int.Parse(index), name);
                    //conditiomDataList에 추가를 해 줍니다.
                    gMainForm.conditionDataList.Add(conditionData);
                    //조건식 설정창에 있는 conditionComboBox에 조건식을 추가해 준다
                    gMainForm.ConditionDig.conditionComboBox.Items.Add(name); 
                }
            }
            //첫번쨰조건식이름을 넣어준다.
            gMainForm.ConditionDig.conditionComboBox.Text = gMainForm.conditionDataList[0].conditionName;
            gMainForm.setLogText("조건식이 conditionDataList에 모두 저장되었습니다.");
        }
    }
}
