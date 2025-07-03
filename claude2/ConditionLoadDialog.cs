using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockAutoTrade2
{
    public partial class ConditionLoadDialog : Form
    {
        public MainForm gMainForm = MainForm.GetInstance();

        public string[] tempConditionListData = null;
        public int curSelectRowNumber; // 리스트에 클릭된 Row의 번호
        public ConditionLoadDialog()
        {
            InitializeComponent();
            //클릭이벤트 등록
            jogunLoadingButton.Click += Button_Click;
            jogunDeleteButton.Click += Button_Click;

            curSelectRowNumber = 0;
            jogunSaveLoadDataGridView.DoubleBuffered(true);
            jogunSaveLoadDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //머리글 중앙 정렬
            jogunSaveLoadDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            foreach(DataGridViewColumn item in jogunSaveLoadDataGridView.Columns)
            {
                item.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // 중앙정렬
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            if (sender.Equals(jogunLoadingButton)) //조건식 불러오기
            {
                string[] cListData = gMainForm.gFileIOInstance.getConditionListData();
                int cListDataLength = cListData.Length / (int)Save.Condition;
                if (cListDataLength == 0)
                {
                    MessageBox.Show("저장된 조건식이 없습니다.");
                    return;
                }

                tempConditionListData = gMainForm.gFileIOInstance.getConditionListData();

                string _tempStr = string.Empty;
                string _conditionIndex = string.Empty;
                string _conditionName = string.Empty;
                string[] _strInvestment, _strInvestmentPrice, _strBuying, _strReBuying, _strTakeProfit, _strStopLoss, _strTsmedo;

                // 조건식인덱스
                _conditionIndex = tempConditionListData[(curSelectRowNumber * (int)Save.Condition)];
                //_conditionName = gMainForm.gLoginInstance.conditionList[Int32.Parse(_conditionIndex)].Name;
                _conditionName = getConditionName(Int32.Parse(_conditionIndex));
                // 조건등록 다이얼로그 변수에 셋팅을 해 준다.
                gMainForm.ConditionDig.conditionComboBox.Text = _conditionName;
                ////////////////////////////////////////////////////////////////////////////// 투자금 설정 //////////////////////////////////////////////////////////////////////////////
                _tempStr = tempConditionListData[(curSelectRowNumber * (int)Save.Condition) + 1];
                _strInvestment = _tempStr.Split(';');
                //_strInvestment[0] account
                // _strInvestment[1] conditionName
                // _strInvestment[2] conditionIndex 
                // _strInvestment[3] buyingItmeCount 
                // _strInvestment[4] itemInvestment
                // UI에 셋팅을 한다.
                string conditionName = getConditionName(Int32.Parse(_strInvestment[2])); //gMainForm.gLoginInstance.conditionList[Int32.Parse(_strInvestment[2])].Name; // 조건식 이름
                gMainForm.ConditionDig.conditionComboBox.Text = conditionName;
                gMainForm.ConditionDig.buyingItemCountTextBox.Text = _strInvestment[3]; // 매수 종목수
                gMainForm.ConditionDig.investmentPerItemTextBox.Text = _strInvestment[4]; // 종목당 투자금
                gMainForm.ConditionDig.transferItemCountTextBox.Text = _strInvestment[5]; // 편입 종목수

                ////////////////////////////////////////////////////////////////////////// 매수금액설정 ///////////////////////////////////////////////////////////////////////////////////
                _tempStr = tempConditionListData[(curSelectRowNumber * (int)Save.Condition) + 2];
                _strInvestmentPrice = _tempStr.Split(';');
                //_strInvestmentPrice[0] buyingCount 
                //_strInvestmentPrice[1] investMoney_1 
                //_strInvestmentPrice[2] investMoney_2 
                //_strInvestmentPrice[3] investMoney_3 
                //_strInvestmentPrice[4] investMoney_4 
                //_strInvestmentPrice[5] investMoney_5 
                //_strInvestmentPrice[6] investMoney_6;
                string _vstr = "";
                double _value1 = 0, _value2 = 0, _value3 = 0, _value4 = 0, _value5 = 0, _value6 = 0;
                int _buyingCount = Int32.Parse(_strInvestmentPrice[0]);
                // ui설절
                setInvestMoneyTextBox(_buyingCount);
                gMainForm.ConditionDig.investBuyingCountNumeriUpDown.Value = Convert.ToDecimal(_buyingCount); // 매수회수                
                _value1 = double.Parse(_strInvestmentPrice[1]);
                _value2 = double.Parse(_strInvestmentPrice[2]);
                _value3 = double.Parse(_strInvestmentPrice[3]);
                _value4 = double.Parse(_strInvestmentPrice[4]);
                _value5 = double.Parse(_strInvestmentPrice[5]);
                _value6 = double.Parse(_strInvestmentPrice[6]);

                gMainForm.ConditionDig.investMoneyTextBox_1.Text = _value1.ToString("N0"); // 매수금1
                gMainForm.ConditionDig.investMoneyTextBox_2.Text = _value2.ToString("N0"); // 매수금2
                gMainForm.ConditionDig.investMoneyTextBox_3.Text = _value3.ToString("N0"); // 매수금3
                gMainForm.ConditionDig.investMoneyTextBox_4.Text = _value4.ToString("N0"); // 매수금4
                gMainForm.ConditionDig.investMoneyTextBox_5.Text = _value5.ToString("N0"); // 매수금5
                gMainForm.ConditionDig.investMoneyTextBox_6.Text = _value6.ToString("N0"); // 매수금6
                gMainForm.ConditionDig.investMoneyTotalTextBox.Text = (_value1 + _value2 + _value3 + _value4 + _value5 + _value6).ToString("N0"); // 총 투자금

                //////////////////////////////////////////////////////////////////////////////// 매수 설정 //////////////////////////////////////////////////////////////////////////////
                _tempStr = tempConditionListData[(curSelectRowNumber * (int)Save.Condition) + 3];
                _strBuying = _tempStr.Split(';');
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
                int buyingUsing = Int32.Parse(_strBuying[0]);// 매수사용체크박스
                if (buyingUsing == 1)
                {
                    gMainForm.ConditionDig.buyingCheckBox.Checked = true;
                    // 화면 ui 셋팅
                    gMainForm.ConditionDig.buyingNotUsedGroupBox.Visible = false;
                    gMainForm.ConditionDig.buyingTypeComboBox.Visible = true;
                }
                else
                {
                    gMainForm.ConditionDig.buyingCheckBox.Checked = false;
                    // 화면 ui 셋팅
                    gMainForm.ConditionDig.buyingNotUsedGroupBox.Visible = true;
                    gMainForm.ConditionDig.buyingNotUsedGroupBox.BringToFront();
                    gMainForm.ConditionDig.buyingNotUsedGroupBox.Location = new Point(20, 314);
                    gMainForm.ConditionDig.buyingTypeComboBox.Visible = false;
                }
                int buyingType = Int32.Parse(_strBuying[1]);
                gMainForm.ConditionDig.buyingTypeComboBox.SelectedIndex = buyingType;// 매수 타입 0:기본매수, 1:이동평균,2:스토케스틱,3:볼린저밴드,4:엔벨로프
                int mesuoption1 = int.Parse(_strBuying[18]);
                gMainForm.ConditionDig.mesuoption1comboBox.SelectedIndex = mesuoption1;

                setBuyingUI(buyingType);
                if (buyingType == 0) // 기본매수
                {
                    if(mesuoption1 == 0)
                    {
                        int buyingTransferType = Int32.Parse(_strBuying[2]);
                        if (buyingTransferType == 0) // 편입즉시매수
                        {
                            gMainForm.ConditionDig.buyingType1ImmediatelyBuyingRadioButton.Checked = true;
                            gMainForm.ConditionDig.buyingType1TransferPricePerRadioButton.Checked = false;
                            gMainForm.ConditionDig.buyingType2TransferPricePerRadioButton.Checked = false;
                        }
                        else if (buyingTransferType == 1) // 편입후 n% 매수
                        {
                            gMainForm.ConditionDig.buyingType1ImmediatelyBuyingRadioButton.Checked = false;
                            gMainForm.ConditionDig.buyingType1TransferPricePerRadioButton.Checked = true;
                            gMainForm.ConditionDig.buyingType2TransferPricePerRadioButton.Checked = false;
                            // 대비 n%
                            double buyingTransferPer = double.Parse(_strBuying[3]);
                            gMainForm.ConditionDig.buyingType1TransferPriceNumericUpDown.Value = Convert.ToDecimal(buyingTransferPer);
                            // 이상, 이하
                            int buyingTransferUpdown = Int32.Parse(_strBuying[4]);
                            gMainForm.ConditionDig.buyingType1TransferPriceUpDownNumericUpDown.SelectedIndex = buyingTransferUpdown;
                        }
                        else if (buyingTransferType == 2)
                        {
                            gMainForm.ConditionDig.buyingType1ImmediatelyBuyingRadioButton.Checked = false;
                            gMainForm.ConditionDig.buyingType1TransferPricePerRadioButton.Checked = false;
                            gMainForm.ConditionDig.buyingType2TransferPricePerRadioButton.Checked = true;

                            double buyingTransferPer2 = double.Parse(_strBuying[16]);
                            gMainForm.ConditionDig.buyingType2TransferPriceNumericUpDown.Value = Convert.ToDecimal(buyingTransferPer2);
                            double riseTransferPer2 = double.Parse(_strBuying[17]);
                            gMainForm.ConditionDig.buyingType2RisePriceNumericUpDown.Value = Convert.ToDecimal(riseTransferPer2);

                        }
                    }
                    else if(mesuoption1 == 1)
                    {
                        int nontradingtime = int.Parse(_strBuying[19]);
                        gMainForm.ConditionDig.nontradingtimeNumericUpDown.Value = nontradingtime;
                        int mesuoption2 = int.Parse(_strBuying[20]);
                        gMainForm.ConditionDig.mesuoption2comboBox.SelectedIndex = mesuoption2;

                        int buyingTransferType = Int32.Parse(_strBuying[2]);
                        if (buyingTransferType == 0) // 편입즉시매수
                        {
                            gMainForm.ConditionDig.buyingType1ImmediatelyBuyingRadioButton.Checked = true;
                            gMainForm.ConditionDig.buyingType1TransferPricePerRadioButton.Checked = false;
                            gMainForm.ConditionDig.buyingType2TransferPricePerRadioButton.Checked = false;
                        }
                        else if (buyingTransferType == 1) // 편입후 n% 매수
                        {
                            gMainForm.ConditionDig.buyingType1ImmediatelyBuyingRadioButton.Checked = false;
                            gMainForm.ConditionDig.buyingType1TransferPricePerRadioButton.Checked = true;
                            gMainForm.ConditionDig.buyingType2TransferPricePerRadioButton.Checked = false;
                            // 대비 n%
                            double buyingTransferPer = double.Parse(_strBuying[3]);
                            gMainForm.ConditionDig.buyingType1TransferPriceNumericUpDown.Value = Convert.ToDecimal(buyingTransferPer);
                            // 이상, 이하
                            int buyingTransferUpdown = Int32.Parse(_strBuying[4]);
                            gMainForm.ConditionDig.buyingType1TransferPriceUpDownNumericUpDown.SelectedIndex = buyingTransferUpdown;
                        }
                        else if (buyingTransferType == 2)
                        {
                            gMainForm.ConditionDig.buyingType1ImmediatelyBuyingRadioButton.Checked = false;
                            gMainForm.ConditionDig.buyingType1TransferPricePerRadioButton.Checked = false;
                            gMainForm.ConditionDig.buyingType2TransferPricePerRadioButton.Checked = true;

                            double buyingTransferPer2 = double.Parse(_strBuying[16]);
                            gMainForm.ConditionDig.buyingType2TransferPriceNumericUpDown.Value = Convert.ToDecimal(buyingTransferPer2);
                            double riseTransferPer2 = double.Parse(_strBuying[17]);
                            gMainForm.ConditionDig.buyingType2RisePriceNumericUpDown.Value = Convert.ToDecimal(riseTransferPer2);

                        }
                    }
                }
                else if (buyingType == 1) // 이동평균선
                {
                    // 이평종류 0:단순, 1:지수                
                    int buyingMovePriceKindType = Int32.Parse(_strBuying[5]);
                    gMainForm.ConditionDig.buyingType2MoveKindComboBox.SelectedIndex = buyingMovePriceKindType;
                    // 월,주,일,분봉(0,1,2,3)
                    int buyingBongType = Int32.Parse(_strBuying[6]);
                    gMainForm.ConditionDig.buyingType2DayComboBox.SelectedIndex = buyingBongType;
                    // 1,3,5,10,20 등등 분봉
                    int buyingMinuteType = Int32.Parse(_strBuying[7]);
                    //gMainForm.ConditionDig.buyingType2MoveBongKindTextBox.Text = buyingMinuteType.ToString();
                    gMainForm.ConditionDig.buyingType2MoveBongKindComboBox.SelectedIndex = buyingMinuteType;
                    // 1,3,5,20 이평종류
                    int buyingMinuteLineType = Int32.Parse(_strBuying[8]);
                    gMainForm.ConditionDig.buyingType2MoveLineTextBox.Text = buyingMinuteLineType.ToString();
                    // n%
                    double buyingMinuteLineAccessPer = double.Parse(_strBuying[9]);
                    gMainForm.ConditionDig.buyingType2MoveBongPerTextBox.Text = buyingMinuteLineAccessPer.ToString();
                    // 0:근접, 1:돌파
                    int buyingDistance = Int32.Parse(_strBuying[10]);
                    // UI셋팅
                    if (buyingDistance == 0) // 근접
                    {
                        gMainForm.ConditionDig.buyingType2MoveBongPerTextBox.Visible = true;
                        gMainForm.ConditionDig.buyingType2TextPercent.Visible = true;
                        gMainForm.ConditionDig.buyingType2DistanceComboBox.Location = new Point(193, 89);
                        gMainForm.ConditionDig.buyingType2TextBuying.Location = new Point(241, 93);
                    }
                    else // 돌파
                    {
                        gMainForm.ConditionDig.buyingType2MoveBongPerTextBox.Visible = false;
                        gMainForm.ConditionDig.buyingType2TextPercent.Visible = false;
                        gMainForm.ConditionDig.buyingType2DistanceComboBox.Location = new Point(193 - 55, 89);
                        gMainForm.ConditionDig.buyingType2TextBuying.Location = new Point(241 - 55, 93);
                    }

                    gMainForm.ConditionDig.buyingType2DistanceComboBox.SelectedIndex = buyingDistance;
                }
                else if (buyingType == 2) // 스토캐스틱
                {
                    // 기간
                    // 월,주,일,분봉(0,1,2,3)
                    int buyingBongType = Int32.Parse(_strBuying[6]);
                    gMainForm.ConditionDig.buyingType3DayComboBox.SelectedIndex = buyingBongType;
                    // 1,3,5,10,20 등등 분봉
                    int buyingMinuteType = Int32.Parse(_strBuying[7]);
                    //gMainForm.ConditionDig.buyingType3MoveBongKindTextBox.Text = buyingMinuteType.ToString();
                    gMainForm.ConditionDig.buyingType3MoveBongKindComboBox.SelectedIndex = buyingMinuteType;

                    int buyingStocPeriod1 = Int32.Parse(_strBuying[11]);
                    gMainForm.ConditionDig.buyingType3Period1.Text = buyingStocPeriod1.ToString();
                    // %K
                    int buyingStocPeriod2 = Int32.Parse(_strBuying[12]);
                    gMainForm.ConditionDig.buyingType3Period2.Text = buyingStocPeriod2.ToString();
                    // %D
                    int buyingStocPeriod3 = Int32.Parse(_strBuying[13]);
                    gMainForm.ConditionDig.buyingType3Period3.Text = buyingStocPeriod3.ToString();

                    // k값
                    double buyingMinuteLineAccessPer = double.Parse(_strBuying[9]);
                    gMainForm.ConditionDig.buyingType3StocValueTextBox.Text = buyingMinuteLineAccessPer.ToString();
                    // 0:이상, 1:이하
                    int buyingDistance = Int32.Parse(_strBuying[10]);
                    gMainForm.ConditionDig.buyingType3DistanceComboBox.SelectedIndex = buyingDistance;
                }
                else if (buyingType == 3) // 볼린져밴드
                {
                    // 기간
                    // 월,주,일,분봉(0,1,2,3)
                    int buyingBongType = Int32.Parse(_strBuying[6]);
                    gMainForm.ConditionDig.buyingType4DayComboBox.SelectedIndex = buyingBongType;
                    // 1,3,5,10,20 등등 분봉
                    int buyingMinuteType = Int32.Parse(_strBuying[7]);
                    //gMainForm.ConditionDig.buyingType4MoveBongKindTextBox.Text = buyingMinuteType.ToString();
                    gMainForm.ConditionDig.buyingType4MoveBongKindComboBox.SelectedIndex = buyingMinuteType;
                    int buyingStocPeriod1 = Int32.Parse(_strBuying[11]);
                    gMainForm.ConditionDig.buyingType4Period1.Text = buyingStocPeriod1.ToString();
                    // 승수
                    double buyingBollPeriod = Double.Parse(_strBuying[14]);
                    gMainForm.ConditionDig.buyingType4Period2.Text = buyingBollPeriod.ToString();
                    // 상한선, 중심선, 하한선(0, 1, 2)
                    int buyingLine3Type = Int32.Parse(_strBuying[15]);
                    gMainForm.ConditionDig.buyingType4LineComboBox.SelectedIndex = buyingLine3Type;
                    // n%
                    double buyingMinuteLineAccessPer = Double.Parse(_strBuying[9]);
                    gMainForm.ConditionDig.buyingType4MoveBongPerTextBox.Text = buyingMinuteLineAccessPer.ToString();
                    // 0:근접, 1:돌파, 2:이탈
                    int buyingDistance = Int32.Parse(_strBuying[10]);
                    // UI 셋팅
                    if (buyingDistance == 0) // 근접
                    {
                        gMainForm.ConditionDig.buyingType4MoveBongPerTextBox.Visible = true;
                        gMainForm.ConditionDig.buyingType4TextPercent.Visible = true;
                        gMainForm.ConditionDig.buyingType4DistanceComboBox.Location = new Point(168, 89);
                        gMainForm.ConditionDig.buyingType4TextBuying.Location = new Point(219, 93);
                    }
                    else // 돌파, 이탈
                    {
                        gMainForm.ConditionDig.buyingType4MoveBongPerTextBox.Visible = false;
                        gMainForm.ConditionDig.buyingType4TextPercent.Visible = false;
                        gMainForm.ConditionDig.buyingType4DistanceComboBox.Location = new Point(168 - 55, 89);
                        gMainForm.ConditionDig.buyingType4TextBuying.Location = new Point(219 - 55, 93);
                    }
                    gMainForm.ConditionDig.buyingType4DistanceComboBox.SelectedIndex = buyingDistance;
                }
                else if (buyingType == 4) // 엔벨로프
                {
                    // 기간
                    // 월,주,일,분봉(0,1,2,3)
                    int buyingBongType = Int32.Parse(_strBuying[6]);
                    gMainForm.ConditionDig.buyingType5DayComboBox.SelectedIndex = buyingBongType;
                    // 1,3,5,10,20 등등 분봉
                    int buyingMinuteType = Int32.Parse(_strBuying[7]);
                    //gMainForm.ConditionDig.buyingType5MoveBongKindTextBox.Text = buyingMinuteType.ToString();
                    gMainForm.ConditionDig.buyingType5MoveBongKindComboBox.SelectedIndex = buyingMinuteType;

                    int buyingStocPeriod1 = Int32.Parse(_strBuying[11]);
                    gMainForm.ConditionDig.buyingType5Period1.Text = buyingStocPeriod1.ToString();
                    // %
                    double buyingBollPeriod = Double.Parse(_strBuying[14]);
                    gMainForm.ConditionDig.buyingType5Period2.Text = buyingBollPeriod.ToString();

                    // 상한선, 중심선, 하한선(0, 1, 2)
                    int buyingLine3Type = Int32.Parse(_strBuying[15]);
                    gMainForm.ConditionDig.buyingType5LineComboBox.SelectedIndex = buyingLine3Type;
                    // n%
                    double buyingMinuteLineAccessPer = Double.Parse(_strBuying[9]);
                    gMainForm.ConditionDig.buyingType5MoveBongPerTextBox.Text = buyingMinuteLineAccessPer.ToString();
                    // 0:근접, 1:돌파, 2:이탈
                    int buyingDistance = Int32.Parse(_strBuying[10]);
                    // UI셋팅
                    if (buyingDistance == 0) // 근접
                    {
                        gMainForm.ConditionDig.buyingType5MoveBongPerTextBox.Visible = true;
                        gMainForm.ConditionDig.buyingType5TextPercent.Visible = true;
                        gMainForm.ConditionDig.buyingType5DistanceComboBox.Location = new Point(168, 89);
                        gMainForm.ConditionDig.buyingType5TextBuying.Location = new Point(219, 93);
                    }
                    else // 돌파, 이탈
                    {
                        gMainForm.ConditionDig.buyingType5MoveBongPerTextBox.Visible = false;
                        gMainForm.ConditionDig.buyingType5TextPercent.Visible = false;
                        gMainForm.ConditionDig.buyingType5DistanceComboBox.Location = new Point(168 - 55, 89);
                        gMainForm.ConditionDig.buyingType5TextBuying.Location = new Point(219 - 55, 93);
                    }
                    gMainForm.ConditionDig.buyingType5DistanceComboBox.SelectedIndex = buyingDistance;
                }
                ///////////////////////////////////////////////////////////////////////////// 추매 설정 ////////////////////////////////////////////////////////////////////////////////
                _tempStr = tempConditionListData[(curSelectRowNumber * (int)Save.Condition) + 4];
                _strReBuying = _tempStr.Split(';');
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
                int reBuyingType = Int32.Parse(_strReBuying[0]);
                int[] reBuyingMinuteLineType = new int[5];// 1,3,5,20이평 등등
                // UI셋팅
                setReBuyingUI(reBuyingType);
                gMainForm.ConditionDig.addBuyingTypeComboBox.SelectedIndex = reBuyingType; // 추매타입 0:기본추매, 1:이동평균선
                double[] reBuyingPer = new double[5]; // 추매 %
                if (reBuyingType == 0)
                {
                    reBuyingPer[0] = Double.Parse(_strReBuying[1]);
                    reBuyingPer[1] = Double.Parse(_strReBuying[2]);
                    reBuyingPer[2] = Double.Parse(_strReBuying[3]);
                    reBuyingPer[3] = Double.Parse(_strReBuying[4]);
                    reBuyingPer[4] = Double.Parse(_strReBuying[5]);
                    gMainForm.ConditionDig.reBuyingType1InvestPricePerTextBox_2.Text = reBuyingPer[0].ToString();
                    gMainForm.ConditionDig.reBuyingType1InvestPricePerTextBox_3.Text = reBuyingPer[1].ToString();
                    gMainForm.ConditionDig.reBuyingType1InvestPricePerTextBox_4.Text = reBuyingPer[2].ToString();
                    gMainForm.ConditionDig.reBuyingType1InvestPricePerTextBox_5.Text = reBuyingPer[3].ToString();
                    gMainForm.ConditionDig.reBuyingType1InvestPricePerTextBox_6.Text = reBuyingPer[4].ToString();
                }
                else if (reBuyingType == 1) // 이동평균선 추매
                {
                    //0:단수, 1:지수
                    int reBuyingMovePriceKindType = Int32.Parse(_strReBuying[6]);
                    gMainForm.ConditionDig.reBuyingType2MoveKindComboBox.SelectedIndex = reBuyingMovePriceKindType;
                    // 월,주,일,분봉(0,1,2,3)
                    int reBuyingBongType = Int32.Parse(_strReBuying[7]);
                    gMainForm.ConditionDig.reBuyingType2DayComboBox.SelectedIndex = reBuyingBongType;
                    // 1,3,5,10,20 등등 분봉
                    int reBuyingMinuteType = Int32.Parse(_strReBuying[8]);
                    //gMainForm.ConditionDig.reBuyingType2MoveBongKindTextBox.Text = reBuyingMinuteType.ToString();
                    gMainForm.ConditionDig.reBuyingType2MoveBongKindComboBox.SelectedIndex = reBuyingMinuteType;
                    // 1,3,5,20이평 등등
                    reBuyingMinuteLineType[0] = Int32.Parse(_strReBuying[9]);
                    reBuyingMinuteLineType[1] = Int32.Parse(_strReBuying[10]);
                    reBuyingMinuteLineType[2] = Int32.Parse(_strReBuying[11]);
                    reBuyingMinuteLineType[3] = Int32.Parse(_strReBuying[12]);
                    reBuyingMinuteLineType[4] = Int32.Parse(_strReBuying[13]);
                    gMainForm.ConditionDig.reBuyingType2MoveLineTextBox_2.Text = reBuyingMinuteLineType[0].ToString();
                    gMainForm.ConditionDig.reBuyingType2MoveLineTextBox_3.Text = reBuyingMinuteLineType[1].ToString();
                    gMainForm.ConditionDig.reBuyingType2MoveLineTextBox_4.Text = reBuyingMinuteLineType[2].ToString();
                    gMainForm.ConditionDig.reBuyingType2MoveLineTextBox_5.Text = reBuyingMinuteLineType[3].ToString();
                    gMainForm.ConditionDig.reBuyingType2MoveLineTextBox_6.Text = reBuyingMinuteLineType[4].ToString();
                    // 추매 %
                    reBuyingPer[0] = Double.Parse(_strReBuying[1]);
                    reBuyingPer[1] = Double.Parse(_strReBuying[2]);
                    reBuyingPer[2] = Double.Parse(_strReBuying[3]);
                    reBuyingPer[3] = Double.Parse(_strReBuying[4]);
                    reBuyingPer[4] = Double.Parse(_strReBuying[5]);
                    gMainForm.ConditionDig.reBuyingType2InvestPricePerTextBox_2.Text = reBuyingPer[0].ToString();
                    gMainForm.ConditionDig.reBuyingType2InvestPricePerTextBox_3.Text = reBuyingPer[1].ToString();
                    gMainForm.ConditionDig.reBuyingType2InvestPricePerTextBox_4.Text = reBuyingPer[2].ToString();
                    gMainForm.ConditionDig.reBuyingType2InvestPricePerTextBox_5.Text = reBuyingPer[3].ToString();
                    gMainForm.ConditionDig.reBuyingType2InvestPricePerTextBox_6.Text = reBuyingPer[4].ToString();
                }

                //////////////////////////////////////////////////////////////////////////////// 익절 설정 //////////////////////////////////////////////////////////////////////////////
                _tempStr = tempConditionListData[(curSelectRowNumber * (int)Save.Condition) + 5];
                _strTakeProfit = _tempStr.Split(';');
                double[] takeProfitBuyingPricePer = new double[5];
                double[] takeProfitProportion = new double[5];
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
                int takeProfitUsing = Int32.Parse(_strTakeProfit[0]); // 익절 사용 여부
                if (takeProfitUsing == 1)
                {
                    gMainForm.ConditionDig.takeProfitCheckBox.Checked = true;
                    // 화면 ui 셋팅
                    gMainForm.ConditionDig.takeProfitNotUsedGroupBox.Visible = false;
                    gMainForm.ConditionDig.takeProfitTypeComboBox.Visible = true;
                }
                else
                {
                    gMainForm.ConditionDig.takeProfitCheckBox.Checked = false;
                    // 화면 ui 셋팅
                    gMainForm.ConditionDig.takeProfitNotUsedGroupBox.Visible = true;
                    gMainForm.ConditionDig.takeProfitNotUsedGroupBox.BringToFront();
                    gMainForm.ConditionDig.takeProfitNotUsedGroupBox.Location = new Point(356, 314);
                    gMainForm.ConditionDig.takeProfitTypeComboBox.Visible = false;
                }
                int takeProfitType = Int32.Parse(_strTakeProfit[1]);
                gMainForm.ConditionDig.takeProfitTypeComboBox.SelectedIndex = takeProfitType; // 0:기본익절,1:이동평균 근접

                setTakeProfitUI(takeProfitType);

                if (takeProfitType == 0) // 기본 익절
                {
                    // 매수단가대비 n%
                    int takeProfitCount = int.Parse(_strTakeProfit[2]);
                    gMainForm.ConditionDig.takeProfitComboBox.SelectedIndex = takeProfitCount;

                    takeProfitBuyingPricePer[0] = double.Parse(_strTakeProfit[3]);
                    gMainForm.ConditionDig.takeProfitBuyingPricePerNumericUpDown1.Value = Convert.ToDecimal(takeProfitBuyingPricePer[0]);
                    takeProfitProportion[0] = double.Parse(_strTakeProfit[4]);
                    gMainForm.ConditionDig.takeProfitPerNumericUpDown1.Value = Convert.ToDecimal(takeProfitProportion[0]);

                    takeProfitBuyingPricePer[1] = double.Parse(_strTakeProfit[5]);
                    gMainForm.ConditionDig.takeProfitBuyingPricePerNumericUpDown2.Value = Convert.ToDecimal(takeProfitBuyingPricePer[1]);
                    takeProfitProportion[1] = double.Parse(_strTakeProfit[6]);
                    gMainForm.ConditionDig.takeProfitPerNumericUpDown2.Value = Convert.ToDecimal(takeProfitProportion[1]);

                    takeProfitBuyingPricePer[2] = double.Parse(_strTakeProfit[7]);
                    gMainForm.ConditionDig.takeProfitBuyingPricePerNumericUpDown3.Value = Convert.ToDecimal(takeProfitBuyingPricePer[2]);
                    takeProfitProportion[2] = double.Parse(_strTakeProfit[8]);
                    gMainForm.ConditionDig.takeProfitPerNumericUpDown3.Value = Convert.ToDecimal(takeProfitProportion[2]);

                    takeProfitBuyingPricePer[3] = double.Parse(_strTakeProfit[9]);
                    gMainForm.ConditionDig.takeProfitBuyingPricePerNumericUpDown4.Value = Convert.ToDecimal(takeProfitBuyingPricePer[3]);
                    takeProfitProportion[3] = double.Parse(_strTakeProfit[10]);
                    gMainForm.ConditionDig.takeProfitPerNumericUpDown4.Value = Convert.ToDecimal(takeProfitProportion[3]);

                    takeProfitBuyingPricePer[4] = double.Parse(_strTakeProfit[11]);
                    gMainForm.ConditionDig.takeProfitBuyingPricePerNumericUpDown5.Value = Convert.ToDecimal(takeProfitBuyingPricePer[4]);
                    takeProfitProportion[4] = double.Parse(_strTakeProfit[12]);
                    gMainForm.ConditionDig.takeProfitPerNumericUpDown5.Value = Convert.ToDecimal(takeProfitProportion[4]);

                }
                else if (takeProfitType == 1) // 이동평균선
                {
                    // 이평종류 0:단순, 1:지수                
                    int takeProfitMovePriceKindType = Int32.Parse(_strTakeProfit[13]);
                    gMainForm.ConditionDig.takeProfitType2MoveKindComboBox.SelectedIndex = takeProfitMovePriceKindType;
                    // 월,주,일,분봉(0,1,2,3)
                    int takeProfitBongType = Int32.Parse(_strTakeProfit[14]);
                    gMainForm.ConditionDig.takeProfitType2DayComboBox.SelectedIndex = takeProfitBongType;
                    // 1,3,5,10,20 등등 분봉
                    int takeProfitMinuteType = Int32.Parse(_strTakeProfit[15]);
                    //gMainForm.ConditionDig.takeProfitType2MoveBongKindTextBox.Text = takeProfitMinuteType.ToString();
                    gMainForm.ConditionDig.takeProfitType2MoveBongKindComboBox.SelectedIndex = takeProfitMinuteType;
                    // 1,3,5,20 이평종류
                    int takeProfitMinuteLineType = Int32.Parse(_strTakeProfit[16]);
                    gMainForm.ConditionDig.takeProfitType2MoveLineTextBox.Text = takeProfitMinuteLineType.ToString();
                    // n%
                    double takeProfitLineAccessPer = double.Parse(_strTakeProfit[17]);
                    gMainForm.ConditionDig.takeProfitType2MoveBongPerTextBox.Text = takeProfitLineAccessPer.ToString();
                    // 0:근접, 1:돌파
                    int takeProfitDistance = Int32.Parse(_strTakeProfit[18]);
                    gMainForm.ConditionDig.takeProfitType2DistanceComboBox.SelectedIndex = takeProfitDistance;
                }
                else if (takeProfitType == 2) // 스토캐스틱SLOW
                {
                    // 기간
                    int takeProfitStocPeriod1 = Int32.Parse(_strTakeProfit[19]);
                    gMainForm.ConditionDig.takeProfitType3Period1.Text = takeProfitStocPeriod1.ToString();
                    // %K
                    int takeProfitStocPeriod2 = Int32.Parse(_strTakeProfit[20]);
                    gMainForm.ConditionDig.takeProfitType3Period2.Text = takeProfitStocPeriod2.ToString();
                    // %D
                    int takeProfitStocPeriod3 = Int32.Parse(_strTakeProfit[21]);
                    gMainForm.ConditionDig.takeProfitType3Period3.Text = takeProfitStocPeriod3.ToString();
                    // 월,주,일,분봉(0,1,2,3)
                    int takeProfitBongType = Int32.Parse(_strTakeProfit[14]);
                    gMainForm.ConditionDig.takeProfitType3DayComboBox.SelectedIndex = takeProfitBongType;
                    // 1,3,5,10,20 등등 분봉
                    int takeProfitMinuteType = Int32.Parse(_strTakeProfit[15]);
                    //gMainForm.ConditionDig.takeProfitType3MoveBongKindTextBox.Text = takeProfitMinuteType.ToString();
                    gMainForm.ConditionDig.takeProfitType3MoveBongKindComboBox.SelectedIndex = takeProfitMinuteType;
                    // k값
                    double takeProfitLineAccessPer = double.Parse(_strTakeProfit[17]);
                    gMainForm.ConditionDig.takeProfitType3StocValueTextBox.Text = takeProfitLineAccessPer.ToString();
                    // 0:이상, 1:이하
                    int takeProfitDistance = Int32.Parse(_strTakeProfit[18]);
                    gMainForm.ConditionDig.takeProfitType3DistanceComboBox.SelectedIndex = takeProfitDistance;
                }
                else if (takeProfitType == 3) // 볼린저밴드
                {
                    // 기간
                    int takeProfitStocPeriod1 = Int32.Parse(_strTakeProfit[19]);
                    gMainForm.ConditionDig.takeProfitType4Period1.Text = takeProfitStocPeriod1.ToString();
                    // 승수
                    double takeProfitBollPeriod = double.Parse(_strTakeProfit[22]);
                    gMainForm.ConditionDig.takeProfitType4Period2.Text = takeProfitBollPeriod.ToString();
                    // 월,주,일,분봉(0,1,2,3)
                    int takeProfitBongType = Int32.Parse(_strTakeProfit[14]);
                    gMainForm.ConditionDig.takeProfitType4DayComboBox.SelectedIndex = takeProfitBongType;
                    // 1,3,5,10,20 등등 분봉
                    int takeProfitMinuteType = Int32.Parse(_strTakeProfit[15]);
                    //gMainForm.ConditionDig.takeProfitType4MoveBongKindTextBox.Text = takeProfitMinuteType.ToString();
                    gMainForm.ConditionDig.takeProfitType4MoveBongKindComboBox.SelectedIndex = takeProfitMinuteType;
                    // 상한선, 중심선, 하한선(0, 1, 2)
                    int takeProfitLine3Type = Int32.Parse(_strTakeProfit[23]);
                    gMainForm.ConditionDig.takeProfitType4LineComboBox.SelectedIndex = takeProfitLine3Type;
                    // n%
                    double takeProfitLineAccessPer = double.Parse(_strTakeProfit[17]);
                    gMainForm.ConditionDig.takeProfitType4MoveBongPerTextBox.Text = takeProfitLineAccessPer.ToString();
                    // 0:근접, 1:돌파, 2:이탈
                    int takeProfitDistance = Int32.Parse(_strTakeProfit[18]);
                    gMainForm.ConditionDig.takeProfitType4DistanceComboBox.SelectedIndex = takeProfitDistance;
                }
                else if (takeProfitType == 4) // 엔벨로프
                {
                    // 기간
                    int takeProfitStocPeriod1 = Int32.Parse(_strTakeProfit[19]);
                    gMainForm.ConditionDig.takeProfitType5Period1.Text = takeProfitStocPeriod1.ToString();
                    // 승수
                    double takeProfitBollPeriod = double.Parse(_strTakeProfit[22]);
                    gMainForm.ConditionDig.takeProfitType5Period2.Text = takeProfitBollPeriod.ToString();
                    // 월,주,일,분봉(0,1,2,3)
                    int takeProfitBongType = Int32.Parse(_strTakeProfit[14]);
                    gMainForm.ConditionDig.takeProfitType5DayComboBox.SelectedIndex = takeProfitBongType;
                    // 1,3,5,10,20 등등 분봉
                    int takeProfitMinuteType = Int32.Parse(_strTakeProfit[15]);
                    //gMainForm.ConditionDig.takeProfitType5MoveBongKindTextBox.Text = takeProfitMinuteType.ToString();
                    gMainForm.ConditionDig.takeProfitType5MoveBongKindComboBox.SelectedIndex = takeProfitMinuteType;
                    // 상한선, 중심선, 하한선(0, 1, 2)
                    int takeProfitLine3Type = Int32.Parse(_strTakeProfit[23]);
                    gMainForm.ConditionDig.takeProfitType5LineComboBox.SelectedIndex = takeProfitLine3Type;
                    // n%
                    double takeProfitLineAccessPer = double.Parse(_strTakeProfit[17]);
                    gMainForm.ConditionDig.takeProfitType5MoveBongPerTextBox.Text = takeProfitLineAccessPer.ToString();
                    // 0:근접, 1:돌파, 2:이탈
                    int takeProfitDistance = Int32.Parse(_strTakeProfit[18]);
                    gMainForm.ConditionDig.takeProfitType5DistanceComboBox.SelectedIndex = takeProfitDistance;
                }

                //////////////////////////////////////////////////////////////////////////////// 손절 설정 //////////////////////////////////////////////////////////////////////////////
                _tempStr = tempConditionListData[(curSelectRowNumber * (int)Save.Condition) + 6];
                _strStopLoss = _tempStr.Split(';');
                double[] stopLossBuyingPricePer = new double[5];
                double[] stopLossProportion = new double[5];
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
                int stopLossUsing = Int32.Parse(_strStopLoss[0]); // 익절 사용 여부
                if (stopLossUsing == 1)
                {
                    gMainForm.ConditionDig.stopLossCheckBox.Checked = true;
                    // 화면 ui 셋팅
                    gMainForm.ConditionDig.stopLossNotUsedGroupBox.Visible = false;
                    gMainForm.ConditionDig.stopLossTypeComboBox.Visible = true;
                }
                else
                {
                    gMainForm.ConditionDig.stopLossCheckBox.Checked = false;
                    // 화면 ui 셋팅
                    gMainForm.ConditionDig.stopLossNotUsedGroupBox.Visible = true;
                    gMainForm.ConditionDig.stopLossNotUsedGroupBox.BringToFront();
                    gMainForm.ConditionDig.stopLossNotUsedGroupBox.Location = new Point(356, 569);
                    gMainForm.ConditionDig.stopLossTypeComboBox.Visible = false;
                }
                int stopLossType = Int32.Parse(_strStopLoss[1]);
                gMainForm.ConditionDig.stopLossTypeComboBox.SelectedIndex = stopLossType; // 0:기본익절,1:이동평균 근접

                setStopLossUI(stopLossType);

                if (stopLossType == 0) // 기본익절
                {
                    int stopLossCount = int.Parse(_strStopLoss[2]);
                    gMainForm.ConditionDig.stopLossComboBox.SelectedIndex = stopLossCount;

                    stopLossBuyingPricePer[0] = double.Parse(_strStopLoss[3]);
                    gMainForm.ConditionDig.stopLossBuyingPricePerNumericUpDown1.Value = Convert.ToDecimal(stopLossBuyingPricePer[0]);
                    stopLossProportion[0] = double.Parse(_strStopLoss[4]);
                    gMainForm.ConditionDig.stopLossPerNumericUpDown1.Value = Convert.ToDecimal(stopLossProportion[0]);

                    stopLossBuyingPricePer[1] = double.Parse(_strStopLoss[5]);
                    gMainForm.ConditionDig.stopLossBuyingPricePerNumericUpDown2.Value = Convert.ToDecimal(stopLossBuyingPricePer[1]);
                    stopLossProportion[1] = double.Parse(_strStopLoss[6]);
                    gMainForm.ConditionDig.stopLossPerNumericUpDown2.Value = Convert.ToDecimal(stopLossProportion[1]);

                    stopLossBuyingPricePer[2] = double.Parse(_strStopLoss[7]);
                    gMainForm.ConditionDig.stopLossBuyingPricePerNumericUpDown3.Value = Convert.ToDecimal(stopLossBuyingPricePer[2]);
                    stopLossProportion[2] = double.Parse(_strStopLoss[8]);
                    gMainForm.ConditionDig.stopLossPerNumericUpDown3.Value = Convert.ToDecimal(stopLossProportion[2]);

                    stopLossBuyingPricePer[3] = double.Parse(_strStopLoss[9]);
                    gMainForm.ConditionDig.stopLossBuyingPricePerNumericUpDown4.Value = Convert.ToDecimal(stopLossBuyingPricePer[3]);
                    stopLossProportion[3] = double.Parse(_strStopLoss[10]);
                    gMainForm.ConditionDig.stopLossPerNumericUpDown4.Value = Convert.ToDecimal(stopLossProportion[3]);

                    stopLossBuyingPricePer[4] = double.Parse(_strStopLoss[11]);
                    gMainForm.ConditionDig.stopLossBuyingPricePerNumericUpDown5.Value = Convert.ToDecimal(stopLossBuyingPricePer[4]);
                    stopLossProportion[4] = double.Parse(_strStopLoss[12]);
                    gMainForm.ConditionDig.stopLossPerNumericUpDown5.Value = Convert.ToDecimal(stopLossProportion[4]);
                }
                else if (stopLossType == 1) // 이동평균선 손절
                {
                    // 이평종류 0:단순, 1:지수                
                    int stopLossMovePriceKindType = Int32.Parse(_strStopLoss[13]);
                    gMainForm.ConditionDig.stopLossType2MoveKindComboBox.SelectedIndex = stopLossMovePriceKindType;
                    // 월,주,일,분봉(0,1,2,3)
                    int stopLossBongType = Int32.Parse(_strStopLoss[14]);
                    gMainForm.ConditionDig.stopLossType2DayComboBox.SelectedIndex = stopLossBongType;
                    // 1,3,5,10,20 등등 분봉
                    int stopLossMinuteType = Int32.Parse(_strStopLoss[15]);
                    //gMainForm.ConditionDig.stopLossType2MoveBongKindTextBox.Text = stopLossMinuteType.ToString();
                    gMainForm.ConditionDig.stopLossType2MoveBongKindComboBox.SelectedIndex = stopLossMinuteType;
                    // 1,3,5,20 이평종류
                    int stopLossMinuteLineType = Int32.Parse(_strStopLoss[16]);
                    gMainForm.ConditionDig.stopLossType2MoveLineTextBox.Text = stopLossMinuteLineType.ToString();
                    // n%
                    double stopLossLineAccessPer = double.Parse(_strStopLoss[17]);
                    gMainForm.ConditionDig.stopLossType2MoveBongPerTextBox.Text = stopLossLineAccessPer.ToString();
                    // 0:근접, 1:돌파
                    int stopLossDistance = Int32.Parse(_strStopLoss[18]);
                    gMainForm.ConditionDig.stopLossType2DistanceComboBox.SelectedIndex = stopLossDistance;
                }
                else if (stopLossType == 2) // 볼린저벤드
                {
                    // 기간
                    int stopLossStocPeriod1 = Int32.Parse(_strStopLoss[19]);
                    gMainForm.ConditionDig.stopLossType3Period1.Text = stopLossStocPeriod1.ToString();
                    // 승수
                    double stopLossBollPeriod = double.Parse(_strStopLoss[20]);
                    gMainForm.ConditionDig.stopLossType3Period2.Text = stopLossBollPeriod.ToString();
                    // 월,주,일,분봉(0,1,2,3)
                    int stopLossBongType = Int32.Parse(_strStopLoss[14]);
                    gMainForm.ConditionDig.stopLossType3DayComboBox.SelectedIndex = stopLossBongType;
                    // 1,3,5,10,20 등등 분봉
                    int stopLossMinuteType = Int32.Parse(_strStopLoss[15]);
                    //gMainForm.ConditionDig.stopLossType3MoveBongKindTextBox.Text = stopLossMinuteType.ToString();
                    gMainForm.ConditionDig.stopLossType3MoveBongKindComboBox.SelectedIndex = stopLossMinuteType;
                    // 상한선, 중심선, 하한선(0, 1, 2)
                    int stopLossLine3Type = Int32.Parse(_strStopLoss[21]);
                    gMainForm.ConditionDig.stopLossType3LineComboBox.SelectedIndex = stopLossLine3Type;
                    // n%
                    double stopLossLineAccessPer = double.Parse(_strStopLoss[17]);
                    gMainForm.ConditionDig.stopLossType3MoveBongPerTextBox.Text = stopLossLineAccessPer.ToString();
                    // 0:근접, 1:돌파, 2:이탈
                    int stopLossDistance = Int32.Parse(_strStopLoss[18]);
                    gMainForm.ConditionDig.stopLossType3DistanceComboBox.SelectedIndex = stopLossDistance;
                }
                else if (stopLossType == 3) // 엔벨로프
                {
                    // 기간
                    int stopLossStocPeriod1 = Int32.Parse(_strStopLoss[19]);
                    gMainForm.ConditionDig.stopLossType4Period1.Text = stopLossStocPeriod1.ToString();
                    // 승수
                    double stopLossBollPeriod = double.Parse(_strStopLoss[20]);
                    gMainForm.ConditionDig.stopLossType4Period2.Text = stopLossBollPeriod.ToString();
                    // 월,주,일,분봉(0,1,2,3)
                    int stopLossBongType = Int32.Parse(_strStopLoss[14]);
                    gMainForm.ConditionDig.stopLossType4DayComboBox.SelectedIndex = stopLossBongType;
                    // 1,3,5,10,20 등등 분봉
                    int stopLossMinuteType = Int32.Parse(_strStopLoss[15]);
                    //gMainForm.ConditionDig.stopLossType4MoveBongKindTextBox.Text = stopLossMinuteType.ToString();
                    gMainForm.ConditionDig.stopLossType4MoveBongKindComboBox.SelectedIndex = stopLossMinuteType;
                    // 상한선, 중심선, 하한선(0, 1, 2)
                    int stopLossLine3Type = Int32.Parse(_strStopLoss[21]);
                    gMainForm.ConditionDig.stopLossType4LineComboBox.SelectedIndex = stopLossLine3Type;
                    // n%
                    double stopLossLineAccessPer = double.Parse(_strStopLoss[17]);
                    gMainForm.ConditionDig.stopLossType4MoveBongPerTextBox.Text = stopLossLineAccessPer.ToString();
                    // 0:근접, 1:돌파, 2:이탈
                    int stopLossDistance = Int32.Parse(_strStopLoss[18]);
                    gMainForm.ConditionDig.stopLossType4DistanceComboBox.SelectedIndex = stopLossDistance;
                }

                //////////////////////////////////////////////////////////////////////////////// ts 설정 //////////////////////////////////////////////////////////////////////////////
                _tempStr = tempConditionListData[(curSelectRowNumber * (int)Save.Condition) + 7];
                _strTsmedo = _tempStr.Split(';');

                int _tsMedousing = Int32.Parse(_strTsmedo[0]); // ts매도 사용 여부
                int _tsmedoCount = Int32.Parse(_strTsmedo[1]); // ts매도 횟수
                int[] _tsmedoUsingtype = new int[3];
                double[] _tsmedoArchievedPer = new double[3];
                double[] _tsmedoPercent = new double[3];
                double[] _tsmedoProportion = new double[3];
              

                if (_tsMedousing == 1)
                {
                    gMainForm.ConditionDig.tsMedoCheckBox.Checked = true;

                    gMainForm.ConditionDig.tsmedocomboBox.SelectedIndex = _tsmedoCount;
                    _tsmedoUsingtype[0] = int.Parse(_strTsmedo[2]);
                    gMainForm.ConditionDig.tsComboBox1.SelectedIndex = _tsmedoUsingtype[0];
                    _tsmedoArchievedPer[0] = double.Parse(_strTsmedo[3]);
                    gMainForm.ConditionDig.tsnumericUpDown1.Value = Convert.ToDecimal(_tsmedoArchievedPer[0]);
                    _tsmedoPercent[0] = double.Parse(_strTsmedo[4]);
                    gMainForm.ConditionDig.tsMedonumericUpDown1.Value = Convert.ToDecimal(_tsmedoPercent[0]);
                    _tsmedoProportion[0] = double.Parse(_strTsmedo[5]);
                    gMainForm.ConditionDig.tspernumericUpDown1.Value = Convert.ToDecimal(_tsmedoProportion[0]);
                    int tsProfitPreservation1 = int.Parse(_strTsmedo[14]);
                    if(tsProfitPreservation1 == 1)
                    gMainForm.ConditionDig.tsProfitPreservationCheckBox1.Checked = true;


                    _tsmedoUsingtype[1] = int.Parse(_strTsmedo[6]);
                    gMainForm.ConditionDig.tsComboBox2.SelectedIndex = _tsmedoUsingtype[1];
                    _tsmedoArchievedPer[1] = double.Parse(_strTsmedo[7]);
                    gMainForm.ConditionDig.tsnumericUpDown2.Value = Convert.ToDecimal(_tsmedoArchievedPer[1]);
                    _tsmedoPercent[1] = double.Parse(_strTsmedo[8]);
                    gMainForm.ConditionDig.tsMedonumericUpDown2.Value = Convert.ToDecimal(_tsmedoPercent[1]);
                    _tsmedoProportion[1] = double.Parse(_strTsmedo[9]);
                    gMainForm.ConditionDig.tspernumericUpDown2.Value = Convert.ToDecimal(_tsmedoProportion[1]);
                    int tsProfitPreservation2 = int.Parse(_strTsmedo[15]);
                    if (tsProfitPreservation2 == 1)
                    gMainForm.ConditionDig.tsProfitPreservationCheckBox2.Checked = true;

                    _tsmedoUsingtype[2] = int.Parse(_strTsmedo[10]);
                    gMainForm.ConditionDig.tsComboBox3.SelectedIndex = _tsmedoUsingtype[2];
                    _tsmedoArchievedPer[2] = double.Parse(_strTsmedo[11]);
                    gMainForm.ConditionDig.tsnumericUpDown3.Value = Convert.ToDecimal(_tsmedoArchievedPer[2]);
                    _tsmedoPercent[2] = double.Parse(_strTsmedo[12]);
                    gMainForm.ConditionDig.tsMedonumericUpDown3.Value = Convert.ToDecimal(_tsmedoPercent[2]);
                    _tsmedoProportion[2] = double.Parse(_strTsmedo[13]);
                    gMainForm.ConditionDig.tspernumericUpDown3.Value = Convert.ToDecimal(_tsmedoProportion[2]);
                    int tsProfitPreservation3 = int.Parse(_strTsmedo[16]);
                    if (tsProfitPreservation3 == 1)
                    gMainForm.ConditionDig.tsProfitPreservationCheckBox3.Checked = true;
                }
                else
                {
                    gMainForm.ConditionDig.tsMedoCheckBox.Checked = false;
                }
                gMainForm.ConditionDig.stopLossTypeComboBox.SelectedIndex = stopLossType; // 0:기본익절,1:이동평균 근접
                gMainForm.ConditionLoadDig.Close();
                /*
                string[] tempConditionListData = gMainForm.gFileIOInstance.getConditionListData(); //파일을 읽어와서 리스트데이터저장

                if(tempConditionListData.Length ==0)
                {
                    MessageBox.Show("저장된 조건식 매매 방식이 없습니다.");
                    return;
                }
                //각각의 데이터 변환하기
                string conditionName = tempConditionListData[(curSelectRowNumber * (int)Save.Condition)];
                int conditionIndex = int.Parse(tempConditionListData[(curSelectRowNumber) * (int)Save.Condition + 1]);
                string totalInvestment = tempConditionListData[(curSelectRowNumber) * (int)Save.Condition + 2];
                string buyingItemCount = tempConditionListData[(curSelectRowNumber) * (int)Save.Condition + 3];
                bool bPurchaseType = bool.Parse(tempConditionListData[(curSelectRowNumber) * (int)Save.Condition + 4]);
                double purchaseTypeRate = double.Parse(tempConditionListData[curSelectRowNumber * (int)Save.Condition + 5]);
                double takeProfitRate = double.Parse(tempConditionListData[(curSelectRowNumber * (int)Save.Condition) + 6]);
                double stopLossRate = double.Parse(tempConditionListData[(curSelectRowNumber * (int)Save.Condition) + 7]);
                bool rePurchase = bool.Parse(tempConditionListData[(curSelectRowNumber * (int)Save.Condition) + 8]);
                string rePurchaseMoney = tempConditionListData[(curSelectRowNumber * (int)Save.Condition) + 9];
                string rePurchaseRate = tempConditionListData[(curSelectRowNumber * (int)Save.Condition) + 10];

                //각각의 컨트롤에 적용하기 -> 메인폼에 접근 -> conditionsettingdialog에 접근
                gMainForm.ConditionDig.conditionComboBox.Text = conditionName;
                gMainForm.ConditionDig.investmentPerItemTextBox.Text = totalInvestment;
                gMainForm.ConditionDig.buyingItemCountTextBox.Text = buyingItemCount;

                //매수타입
                if(bPurchaseType) // bPurchaseType 이 true라면 -> 버튼1에 체크(즉시매수)
                {
                    gMainForm.ConditionDig.buyingType1ImmediatelyBuyingRadioButton.Checked = true;
                    gMainForm.ConditionDig.buyingType1TransferPricePerRadioButton.Checked = false;
                }
                else // bPurchaseType 이 false라면 -> 버튼2에 체크(편입단가대비 매수)
                {
                    gMainForm.ConditionDig.buyingType1ImmediatelyBuyingRadioButton.Checked = false;
                    gMainForm.ConditionDig.buyingType1TransferPricePerRadioButton.Checked = true;
                }
                //매수비율
                gMainForm.ConditionDig.buyingType1TransferPriceNumericUpDown.Value = Convert.ToDecimal(purchaseTypeRate);
                //익절비율
                gMainForm.ConditionDig.takeProfitBuyingPricePerNumericUpDown.Value = Convert.ToDecimal(takeProfitRate);
                //손절비율
                gMainForm.ConditionDig.stopLossBuyingPricePerNumericUpDown.Value = Convert.ToDecimal(stopLossRate);

                // 추매사용
                if (rePurchase)
                {
                    gMainForm.ConditionDig.rePurchaseCheckBox.Checked = true;
                }
                else
                {
                    gMainForm.ConditionDig.rePurchaseCheckBox.Checked = false;
                }
                // 추매매수금
                string[] _money = rePurchaseMoney.Split(';');
                gMainForm.ConditionDig.rePurchaseMoneyTextBox1.Text = _money[0];
                gMainForm.ConditionDig.rePurchaseMoneyTextBox2.Text = _money[1];
                gMainForm.ConditionDig.rePurchaseMoneyTextBox3.Text = _money[2];
                gMainForm.ConditionDig.rePurchaseMoneyTextBox4.Text = _money[3];
                gMainForm.ConditionDig.rePurchaseMoneyTextBox5.Text = _money[4];
                gMainForm.ConditionDig.rePurchaseMoneyTextBox6.Text = _money[5];
                // 추매비율
                string[] _rate = rePurchaseRate.Split(';');
                gMainForm.ConditionDig.rePurchaseRateNumericUpDown1.Value = Convert.ToDecimal(_rate[0]);
                gMainForm.ConditionDig.rePurchaseRateNumericUpDown2.Value = Convert.ToDecimal(_rate[1]);
                gMainForm.ConditionDig.rePurchaseRateNumericUpDown3.Value = Convert.ToDecimal(_rate[2]);
                gMainForm.ConditionDig.rePurchaseRateNumericUpDown4.Value = Convert.ToDecimal(_rate[3]);
                gMainForm.ConditionDig.rePurchaseRateNumericUpDown5.Value = Convert.ToDecimal(_rate[4]);
                gMainForm.ConditionDig.rePurchaseRateNumericUpDown6.Value = Convert.ToDecimal(_rate[5]);

                //불러오기창 닫기
                gMainForm.ConditionLoadDig.Close();
                */
            }
            else if(sender.Equals(jogunDeleteButton)) //조건식 삭제하기
            {
                string[] tempConditionListData = gMainForm.gFileIOInstance.getConditionListData();

                if(tempConditionListData == null)
                {
                    MessageBox.Show("저장된 조건식 매매 방식이 없습니다.");
                    return;
                }

                string message = "선택된 조건식을 삭제할까요?";
                DialogResult result = MessageBox.Show(message, "조건식 삭제", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                {
                    //현재 선택된 row인덱스를 인자로 전달하여 삭제 처리
                    gMainForm.gFileIOInstance.deleteConditionList(curSelectRowNumber);
                    //전체 텍스트 가져오기
                    tempConditionListData = gMainForm.gFileIOInstance.getConditionListData();

                    //그리드뷰 초기화 -> 전체데이터그리드뷰를 일단삭제
                    int rowCount = gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView.Rows.Count;
                    for(int i = rowCount -1 ;i >= 0; i--)
                    {
                        gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView.Rows.RemoveAt(i);
                    }
                    //다시 그리드뷰에 추가해 주기 -> 위에서 ConditionList파일에서 이미 조건식번호를 찾아서 삭제처리를 했기때문에 나머지를 다시 작성
                    for(int i=0; i< tempConditionListData.Length/(int)Save.Condition; i++)
                    {
                        if(tempConditionListData[i * (int)Save.Condition] != null)
                        {
                            int rowIndex = gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView.Rows.Add();
                            int num = Int32.Parse(tempConditionListData[(i * (int)Save.Condition)]);
                            string conditionName = getConditionName(num);
                            gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView["조건식로딩_조건식명", rowIndex].Value = conditionName;
                            gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView["조건식로딩_조건식번호", rowIndex].Value = num;
                            ;                        }
                    }
                    // 무조건 선택으로 0번으로 셋팅한다.
                    curSelectRowNumber = 0;
                    MessageBox.Show("조건식이 삭제 되었습니다.");
                }
            }
        }
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
        public void setBuyingUI(int buyingType)
        {
            if (buyingType == 0) // 매수타입1 기본매수
            {
                gMainForm.ConditionDig.buyingType1GroupBox.Visible = true;
                gMainForm.ConditionDig.buyingType2GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType3GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType4GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType5GroupBox.Visible = false;
            }
            else if (buyingType == 1) // 매수타입2 이동평균
            {
                gMainForm.ConditionDig.buyingType1GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType2GroupBox.Visible = true;
                gMainForm.ConditionDig.buyingType3GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType4GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType5GroupBox.Visible = false;
            }
            else if (buyingType == 2) // 매수타입3 스토캐스틱
            {
                gMainForm.ConditionDig.buyingType1GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType2GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType3GroupBox.Visible = true;
                gMainForm.ConditionDig.buyingType4GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType5GroupBox.Visible = false;
            }
            else if (buyingType == 3) // 매수타입4 볼린저밴드
            {
                gMainForm.ConditionDig.buyingType1GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType2GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType3GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType4GroupBox.Visible = true;
                gMainForm.ConditionDig.buyingType5GroupBox.Visible = false;
            }
            else if (buyingType == 4) // 매수타입5 엔벨로프
            {
                gMainForm.ConditionDig.buyingType1GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType2GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType3GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType4GroupBox.Visible = false;
                gMainForm.ConditionDig.buyingType5GroupBox.Visible = true;
            }

            gMainForm.ConditionDig.buyingType2GroupBox.Location = new Point(20, 314);
            gMainForm.ConditionDig.buyingType3GroupBox.Location = new Point(20, 314);
            gMainForm.ConditionDig.buyingType4GroupBox.Location = new Point(20, 314);
            gMainForm.ConditionDig.buyingType5GroupBox.Location = new Point(20, 314);
        }
        public void setInvestMoneyTextBox(int _buyingCount)
        {
            if (_buyingCount == 1)
            {
                gMainForm.ConditionDig.investMoneyTextBox_1.ReadOnly = false;
                gMainForm.ConditionDig.investMoneyTextBox_1.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_2.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_2.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_2.Text = "1";
                gMainForm.ConditionDig.investMoneyTextBox_3.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_3.Text = "1";
                gMainForm.ConditionDig.investMoneyTextBox_4.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_4.Text = "1";
                gMainForm.ConditionDig.investMoneyTextBox_5.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_5.Text = "1";
                gMainForm.ConditionDig.investMoneyTextBox_6.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_6.Text = "1";
            }
            else if (_buyingCount == 2)
            {
                gMainForm.ConditionDig.investMoneyTextBox_1.ReadOnly = false;
                gMainForm.ConditionDig.investMoneyTextBox_1.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_2.ReadOnly = false;
                gMainForm.ConditionDig.investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_3.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_3.Text = "1";
                gMainForm.ConditionDig.investMoneyTextBox_4.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_4.Text = "1";
                gMainForm.ConditionDig.investMoneyTextBox_5.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_5.Text = "1";
                gMainForm.ConditionDig.investMoneyTextBox_6.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_6.Text = "1";
            }
            else if (_buyingCount == 3)
            {
                gMainForm.ConditionDig.investMoneyTextBox_1.ReadOnly = false;
                gMainForm.ConditionDig.investMoneyTextBox_1.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_2.ReadOnly = false;
                gMainForm.ConditionDig.investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_3.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_4.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_4.Text = "1";
                gMainForm.ConditionDig.investMoneyTextBox_5.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_5.Text = "1";
                gMainForm.ConditionDig.investMoneyTextBox_6.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_6.Text = "1";
            }
            else if (_buyingCount == 4)
            {
                gMainForm.ConditionDig.investMoneyTextBox_1.ReadOnly = false;
                gMainForm.ConditionDig.investMoneyTextBox_1.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_2.ReadOnly = false;
                gMainForm.ConditionDig.investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_3.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_4.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_5.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_5.Text = "1";
                gMainForm.ConditionDig.investMoneyTextBox_6.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_6.Text = "1";
            }
            else if (_buyingCount == 5)
            {
                gMainForm.ConditionDig.investMoneyTextBox_1.ReadOnly = false;
                gMainForm.ConditionDig.investMoneyTextBox_1.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_2.ReadOnly = false;
                gMainForm.ConditionDig.investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_3.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_4.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_5.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_6.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                gMainForm.ConditionDig.investMoneyTextBox_6.Text = "1";
            }
            else if (_buyingCount == 6)
            {
                gMainForm.ConditionDig.investMoneyTextBox_1.ReadOnly = false;
                gMainForm.ConditionDig.investMoneyTextBox_1.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_2.ReadOnly = false;
                gMainForm.ConditionDig.investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_3.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_4.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_5.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                gMainForm.ConditionDig.investMoneyTextBox_6.ReadOnly = true;
                gMainForm.ConditionDig.investMoneyTextBox_6.BackColor = Color.FromArgb(255, 255, 255);
            }
        }
        public void setReBuyingUI(int reBuyingType)
        {
            if (reBuyingType == 0) // 추매타입1 기본추매
            {
                gMainForm.ConditionDig.reBuyingType1GroupBox.Visible = true;
                gMainForm.ConditionDig.reBuyingType2GroupBox.Visible = false;
            }
            else if (reBuyingType == 1) // 추매타입2 이동평균 근접추매
            {
                gMainForm.ConditionDig.reBuyingType1GroupBox.Visible = false;
                gMainForm.ConditionDig.reBuyingType2GroupBox.Visible = true;
            }
            gMainForm.ConditionDig.reBuyingType2GroupBox.Location = new Point(20, 569);
        }
        public void setTakeProfitUI(int takeProfitType)
        {
            if (takeProfitType == 0) // 익절타입1 기본익절
            {
                gMainForm.ConditionDig.takeProfitType1GroupBox.Visible = true;
                gMainForm.ConditionDig.takeProfitType2GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType3GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType4GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType5GroupBox.Visible = false;
            }
            else if (takeProfitType == 1) // 익절타입2 이동평균
            {
                gMainForm.ConditionDig.takeProfitType1GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType2GroupBox.Visible = true;
                gMainForm.ConditionDig.takeProfitType3GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType4GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType5GroupBox.Visible = false;
            }
            else if (takeProfitType == 2) // 익절타입3 스토캐스틱
            {
                gMainForm.ConditionDig.takeProfitType1GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType2GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType3GroupBox.Visible = true;
                gMainForm.ConditionDig.takeProfitType4GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType5GroupBox.Visible = false;
            }
            else if (takeProfitType == 3) // 익절타입4 볼린저밴드
            {
                gMainForm.ConditionDig.takeProfitType1GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType2GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType3GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType4GroupBox.Visible = true;
                gMainForm.ConditionDig.takeProfitType5GroupBox.Visible = false;
            }
            else if (takeProfitType == 4) // 익절타입5 엔벨로프
            {
                gMainForm.ConditionDig.takeProfitType1GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType2GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType3GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType4GroupBox.Visible = false;
                gMainForm.ConditionDig.takeProfitType5GroupBox.Visible = true;
            }

            gMainForm.ConditionDig.takeProfitType2GroupBox.Location = new Point(356, 314);
            gMainForm.ConditionDig.takeProfitType3GroupBox.Location = new Point(356, 314);
            gMainForm.ConditionDig.takeProfitType4GroupBox.Location = new Point(356, 314);
            gMainForm.ConditionDig.takeProfitType5GroupBox.Location = new Point(356, 314);
        }
        public void setStopLossUI(int stopLossType)
        {
            if (stopLossType == 0) // 손절타입1 기본손절
            {
                gMainForm.ConditionDig.stopLossType1GroupBox.Visible = true;
                gMainForm.ConditionDig.stopLossType2GroupBox.Visible = false;
                gMainForm.ConditionDig.stopLossType3GroupBox.Visible = false;
                gMainForm.ConditionDig.stopLossType4GroupBox.Visible = false;
            }
            else if (stopLossType == 1) // 손절타입2 이동평균
            {
                gMainForm.ConditionDig.stopLossType1GroupBox.Visible = false;
                gMainForm.ConditionDig.stopLossType2GroupBox.Visible = true;
                gMainForm.ConditionDig.stopLossType3GroupBox.Visible = false;
                gMainForm.ConditionDig.stopLossType4GroupBox.Visible = false;
            }
            else if (stopLossType == 2) // 손절타입3 볼린저밴드
            {
                gMainForm.ConditionDig.stopLossType1GroupBox.Visible = false;
                gMainForm.ConditionDig.stopLossType2GroupBox.Visible = false;
                gMainForm.ConditionDig.stopLossType3GroupBox.Visible = true;
                gMainForm.ConditionDig.stopLossType4GroupBox.Visible = false;
            }
            else if (stopLossType == 3) // 손절타입4 엔벨로프
            {
                gMainForm.ConditionDig.stopLossType1GroupBox.Visible = false;
                gMainForm.ConditionDig.stopLossType2GroupBox.Visible = false;
                gMainForm.ConditionDig.stopLossType3GroupBox.Visible = false;
                gMainForm.ConditionDig.stopLossType4GroupBox.Visible = true;
            }
            gMainForm.ConditionDig.stopLossType2GroupBox.Location = new Point(356, 569);
            gMainForm.ConditionDig.stopLossType3GroupBox.Location = new Point(356, 569);
            gMainForm.ConditionDig.stopLossType4GroupBox.Location = new Point(356, 569);
        }
        //조건 데이터를 로딩해서 그리드뷰에 추가한다.
        public bool setJogunDataLoading()
        {
            tempConditionListData = gMainForm.gFileIOInstance.getConditionListData();
            /*
            if(tempConditionListData.Length ==0)
            {
                MessageBox.Show("저장된 조건식 매매 방식이 없습니다.");
                return false;
            }
            */
            //그리드뷰초기화
            int rowCount = gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView.Rows.Count;
            for(int i = rowCount-1; i>=0; i--)
            {
                gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView.Rows.RemoveAt(i);
            }
            //읽어온 조건식 매매 방식 데이터의 조건식명과 조건식번호를 그리드뷰에 추가한다.
            for(int i =0; i< tempConditionListData.Length / (int)Save.Condition; i++)
            {
                int rowIndex = gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView.Rows.Add();
                int num = Int32.Parse(tempConditionListData[(i * (int)Save.Condition)]);

                string conditionName = getConditionName(num);
                gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView["조건식로딩_조건식명", rowIndex].Value = conditionName;
                gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView["조건식로딩_조건식번호", rowIndex].Value = num;
            }
            return true;
        }
        //조건식 불러오기 or 삭제시 그리드뷰 클릭이벤트(로우인덱스 저장을 위함)
        private void jogunSaveLoadDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("전체로우개수: "+ gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView.Rows.Count);
            Console.WriteLine("columnInex : " + e.ColumnIndex + " Datagridview Columns count : " +  gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView.Columns.Count);

            if(sender.Equals(gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView))
            {
                if(0 <= e.ColumnIndex && e.ColumnIndex <= gMainForm.ConditionLoadDig.jogunSaveLoadDataGridView.Columns.Count)
                {
                    // row 가로 ,column 세로
                    if(e.RowIndex>-1)
                    {
                        //클릭된 row 인덱스 번호 저장
                        curSelectRowNumber = e.RowIndex;
                        Console.WriteLine("rowindex: " + e.RowIndex);
                    }
                }
            }
        }
    }
}
