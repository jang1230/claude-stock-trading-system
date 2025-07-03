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
    public partial class ConditionSettingDialog : Form
    {
        public MainForm gMainForm = MainForm.GetInstance();

        public string[] tempConditionListData = null;
        public ConditionSettingDialog()
        {
            InitializeComponent();

            this.Size = new Size(706, 1000);
            this.MinimumSize = new Size(706, 1000);
            this.MaximumSize = new Size(706, 1000);

            // 실행시 마지막으로 지정한 값을 불러오기
            //this.Load += new EventHandler(ConditionSettingDialog_load);
            // 종료시 마지막으로 지정한 값을 저장하기
            //this.FormClosing += new FormClosingEventHandler(ConditionSettingDialog_closing);

            // 등록하기 버튼 이벤트 등록
            conditionRegisterbutton.Click += ButtonClickEvent;
            conditionSaveButton.Click += ButtonClickEvent;
            conditionEditButton.Click += ButtonClickEvent;
            conditionLoadButton.Click += ButtonClickEvent;
            // TextBox changed 이벤트 등록
            investmentPerItemTextBox.TextChanged += TextBoxTextChangedEvent;
            buyingItemCountTextBox.TextChanged += TextBoxTextChangedEvent;
            // ts매도 콤보박스에 따라 visble처리
            tsComboBox1.SelectedIndexChanged += tsComboBox1_SelectedIndexChanged;
            tsComboBox2.SelectedIndexChanged += tsComboBox2_SelectedIndexChanged;
            tsComboBox3.SelectedIndexChanged += tsComboBox3_SelectedIndexChanged;
            tsmedocomboBox.SelectedIndexChanged += tsmedocomboBox_SelectedIndexChanged;
            // 익절,손절 인덱스에따라 visble처리
            stopLossComboBox.SelectedIndexChanged += stopLosscomboBox_SelectedIndexChanged;
            takeProfitComboBox.SelectedIndexChanged += takeProfitcomboBox_SelectedIndexChanged;
            mesuoption1comboBox.SelectedIndexChanged += mesuoption1comboBox_SelectedIndexChanged;

            int _buyingCount = (int)investBuyingCountNumeriUpDown.Value; // 추매포함 매수횟수
            int _tsmedoCount = tsmedocomboBox.SelectedIndex;
            int _takeProfitCount = takeProfitComboBox.SelectedIndex;
            int _stopLossCount = stopLossComboBox.SelectedIndex;
            int _mesuoption1 = mesuoption1comboBox.SelectedIndex;

            if(_mesuoption1 == 0)
            {
                nontradingtimeNumericUpDown.Enabled = false;
                nontradingtimeNumericUpDown.BackColor = Color.FromArgb(240, 240, 240);

                mesuoption2comboBox.Enabled = false;
                mesuoption2comboBox.BackColor = Color.FromArgb(240, 240, 240);
            }

            if (_tsmedoCount == 0)
            {
                tsComboBox1.Enabled = true;
                tsComboBox1.BackColor = Color.FromArgb(255, 255, 255);

                tsnumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tsMedonumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tspernumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tsProfitPreservationCheckBox1.Enabled = true;
                tsProfitPreservationCheckBox1.BackColor = Color.FromArgb(255, 255, 255);

                tsComboBox2.Enabled = false;
                tsComboBox2.BackColor = Color.FromArgb(240, 240, 240);

                tsnumericUpDown2.Enabled = false;
                tsnumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);

                tsMedonumericUpDown2.Enabled = false;
                tsMedonumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);

                tspernumericUpDown2.Enabled = false;
                tspernumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);
                tspernumericUpDown2.Value = 0;

                tsProfitPreservationCheckBox2.Enabled = false;
                tsProfitPreservationCheckBox2.BackColor = Color.FromArgb(240, 240, 240);

                tsComboBox3.Enabled = false;
                tsComboBox3.BackColor = Color.FromArgb(240, 240, 240);

                tsnumericUpDown3.Enabled = false;
                tsnumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                tsMedonumericUpDown3.Enabled = false;
                tsMedonumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                tspernumericUpDown3.Enabled = false;
                tspernumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);
                tspernumericUpDown3.Value = 0;

                tsProfitPreservationCheckBox3.Enabled = false;
                tsProfitPreservationCheckBox3.BackColor = Color.FromArgb(240, 240, 240);
            }

            if (_tsmedoCount == 1)
            {
                tsComboBox1.Enabled = true;
                tsComboBox1.BackColor = Color.FromArgb(255, 255, 255);

                tsnumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tsMedonumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tspernumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tsProfitPreservationCheckBox1.Enabled = true;
                tsProfitPreservationCheckBox1.BackColor = Color.FromArgb(255, 255, 255);

                tsComboBox2.Enabled = true;
                tsComboBox2.BackColor = Color.FromArgb(255, 255, 255);

                tsnumericUpDown2.Enabled = true;
                tsnumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                tsMedonumericUpDown2.Enabled = true;
                tsMedonumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                tspernumericUpDown2.Enabled = true;
                tspernumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                tsProfitPreservationCheckBox2.Enabled = true;
                tsProfitPreservationCheckBox2.BackColor = Color.FromArgb(255, 255, 255);

                tsComboBox3.Enabled = false;
                tsComboBox3.BackColor = Color.FromArgb(240, 240, 240);

                tsnumericUpDown3.Enabled = false;
                tsnumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                tsMedonumericUpDown3.Enabled = false;
                tsMedonumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                tspernumericUpDown3.Enabled = false;
                tspernumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);
                tspernumericUpDown3.Value = 0;

                tsProfitPreservationCheckBox3.Enabled = false;
                tsProfitPreservationCheckBox3.BackColor = Color.FromArgb(240, 240, 240);
            }

            if (_tsmedoCount == 2)
            {
                tsComboBox1.Enabled = true;
                tsComboBox1.BackColor = Color.FromArgb(255, 255, 255);

                tsnumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tsMedonumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tspernumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tsProfitPreservationCheckBox1.Enabled = true;
                tsProfitPreservationCheckBox1.BackColor = Color.FromArgb(255, 255, 255);

                tsComboBox2.Enabled = true;
                tsComboBox2.BackColor = Color.FromArgb(255, 255, 255);

                tsnumericUpDown2.Enabled = true;
                tsnumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                tsMedonumericUpDown2.Enabled = true;
                tsMedonumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                tspernumericUpDown2.Enabled = true;
                tspernumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                tsProfitPreservationCheckBox2.Enabled = true;
                tsProfitPreservationCheckBox2.BackColor = Color.FromArgb(255, 255, 255);

                tsComboBox3.Enabled = true;
                tsComboBox3.BackColor = Color.FromArgb(255, 255, 255);

                tsnumericUpDown3.Enabled = true;
                tsnumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                tsMedonumericUpDown3.Enabled = true;
                tsMedonumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                tspernumericUpDown3.Enabled = true;
                tspernumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                tsProfitPreservationCheckBox3.Enabled = true;
                tsProfitPreservationCheckBox3.BackColor = Color.FromArgb(255, 255, 255);
            }
            if (_stopLossCount == 0)
            {
                stopLossBuyingPricePerNumericUpDown1.Enabled = true;
                stopLossBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown1.Enabled = true;
                stopLossPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown2.Enabled = false;
                stopLossBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown2.Enabled = false;
                stopLossPerNumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown2.Value = 0;

                stopLossBuyingPricePerNumericUpDown3.Enabled = false;
                stopLossBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown3.Enabled = false;
                stopLossPerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown3.Value = 0;

                stopLossBuyingPricePerNumericUpDown4.Enabled = false;
                stopLossBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown4.Enabled = false;
                stopLossPerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown4.Value = 0;

                stopLossBuyingPricePerNumericUpDown5.Enabled = false;
                stopLossBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown5.Enabled = false;
                stopLossPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown5.Value = 0;
            }
            if (_stopLossCount == 1)
            {
                stopLossBuyingPricePerNumericUpDown1.Enabled = true;
                stopLossBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown1.Enabled = true;
                stopLossPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown2.Enabled = true;
                stopLossBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown2.Enabled = true;
                stopLossPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown3.Enabled = false;
                stopLossBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown3.Enabled = false;
                stopLossPerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown3.Value = 0;

                stopLossBuyingPricePerNumericUpDown4.Enabled = false;
                stopLossBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown4.Enabled = false;
                stopLossPerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown4.Value = 0;

                stopLossBuyingPricePerNumericUpDown5.Enabled = false;
                stopLossBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown5.Enabled = false;
                stopLossPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown5.Value = 0;
            }
            if (_stopLossCount == 2)
            {
                stopLossBuyingPricePerNumericUpDown1.Enabled = true;
                stopLossBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown1.Enabled = true;
                stopLossPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown2.Enabled = true;
                stopLossBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown2.Enabled = true;
                stopLossPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown3.Enabled = true;
                stopLossBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown3.Enabled = true;
                stopLossPerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown4.Enabled = false;
                stopLossBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown4.Enabled = false;
                stopLossPerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown4.Value = 0;

                stopLossBuyingPricePerNumericUpDown5.Enabled = false;
                stopLossBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown5.Enabled = false;
                stopLossPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown5.Value = 0;
            }
            if (_stopLossCount == 3)
            {
                stopLossBuyingPricePerNumericUpDown1.Enabled = true;
                stopLossBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown1.Enabled = true;
                stopLossPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown2.Enabled = true;
                stopLossBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown2.Enabled = true;
                stopLossPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown3.Enabled = true;
                stopLossBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown3.Enabled = true;
                stopLossPerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown4.Enabled = true;
                stopLossBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown4.Enabled = true;
                stopLossPerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown5.Enabled = false;
                stopLossBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown5.Enabled = false;
                stopLossPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown5.Value = 0;
            }
            if (_stopLossCount == 4)
            {
                stopLossBuyingPricePerNumericUpDown1.Enabled = true;
                stopLossBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown1.Enabled = true;
                stopLossPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown2.Enabled = true;
                stopLossBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown2.Enabled = true;
                stopLossPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown3.Enabled = true;
                stopLossBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown3.Enabled = true;
                stopLossPerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown4.Enabled = true;
                stopLossBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown4.Enabled = true;
                stopLossPerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown5.Enabled = true;
                stopLossBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown5.Enabled = true;
                stopLossPerNumericUpDown5.BackColor = Color.FromArgb(255, 255, 255);
            }
            if (_takeProfitCount == 0)
            {
                takeProfitBuyingPricePerNumericUpDown1.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown1.Enabled = true;
                takeProfitPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown2.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown2.Enabled = false;
                takeProfitPerNumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown2.Value = 0;

                takeProfitBuyingPricePerNumericUpDown3.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown3.Enabled = false;
                takeProfitPerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown3.Value = 0;

                takeProfitBuyingPricePerNumericUpDown4.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown4.Enabled = false;
                takeProfitPerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown4.Value = 0;

                takeProfitBuyingPricePerNumericUpDown5.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown5.Enabled = false;
                takeProfitPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown5.Value = 0;
            }

            if (_takeProfitCount == 1)
            {
                takeProfitBuyingPricePerNumericUpDown1.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown1.Enabled = true;
                takeProfitPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown2.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown2.Enabled = true;
                takeProfitPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown3.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown3.Enabled = false;
                takeProfitPerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown3.Value = 0;

                takeProfitBuyingPricePerNumericUpDown4.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown4.Enabled = false;
                takeProfitPerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown4.Value = 0;

                takeProfitBuyingPricePerNumericUpDown5.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown5.Enabled = false;
                takeProfitPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown5.Value = 0;
            }

            if (_takeProfitCount == 2)
            {
                takeProfitBuyingPricePerNumericUpDown1.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown1.Enabled = true;
                takeProfitPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown2.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown2.Enabled = true;
                takeProfitPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown3.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown3.Enabled = true;
                takeProfitPerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown4.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown4.Enabled = false;
                takeProfitPerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown4.Value = 0;

                takeProfitBuyingPricePerNumericUpDown5.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown5.Enabled = false;
                takeProfitPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown5.Value = 0;
            }

            if (_takeProfitCount == 3)
            {
                takeProfitBuyingPricePerNumericUpDown1.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown1.Enabled = true;
                takeProfitPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown2.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown2.Enabled = true;
                takeProfitPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown3.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown3.Enabled = true;
                takeProfitPerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown4.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown4.Enabled = true;
                takeProfitPerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown5.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown5.Enabled = false;
                takeProfitPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown5.Value = 0;
            }

            if (_takeProfitCount == 4)
            {
                takeProfitBuyingPricePerNumericUpDown1.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown1.Enabled = true;
                takeProfitPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown2.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown2.Enabled = true;
                takeProfitPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown3.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown3.Enabled = true;
                takeProfitPerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown4.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown4.Enabled = true;
                takeProfitPerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown5.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown5.Enabled = true;
                takeProfitPerNumericUpDown5.BackColor = Color.FromArgb(255, 255, 255);
            }

            if (_buyingCount == 1)
            {
                //investMoneyTextBox는 매수횟수의 TextBox
                investMoneyTextBox_2.ReadOnly = true;
                investMoneyTextBox_2.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_2.Text = "0";
                investMoneyTextBox_3.ReadOnly = true;
                investMoneyTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_3.Text = "0";
                investMoneyTextBox_4.ReadOnly = true;
                investMoneyTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_4.Text = "0";
                investMoneyTextBox_5.ReadOnly = true;
                investMoneyTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_5.Text = "0";
                investMoneyTextBox_6.ReadOnly = true;
                investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_6.Text = "0";

                //reBuyingType1InvestPricePerTextBox 는 추매 방식설정의 TextBox
                reBuyingType1InvestPricePerTextBox_2.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_2.Text = "-3";
                reBuyingType1InvestPricePerTextBox_3.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_3.Text = "-6";
                reBuyingType1InvestPricePerTextBox_4.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_4.Text = "-9";
                reBuyingType1InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_5.Text = "-12";
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_6.Text = "-15";

                //reBuyingType2MoveLineTextBox는 추매부분 TextBox(안보이게 처리되어있음)
                reBuyingType2MoveLineTextBox_2.ReadOnly = true;
                reBuyingType2MoveLineTextBox_2.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_2.Text = "1";
                reBuyingType2MoveLineTextBox_3.ReadOnly = true;
                reBuyingType2MoveLineTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_3.Text = "1";
                reBuyingType2MoveLineTextBox_4.ReadOnly = true;
                reBuyingType2MoveLineTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_4.Text = "1";
                reBuyingType2MoveLineTextBox_5.ReadOnly = true;
                reBuyingType2MoveLineTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_5.Text = "1";
                reBuyingType2MoveLineTextBox_6.ReadOnly = true;
                reBuyingType2MoveLineTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_6.Text = "1";

                //reBuyingType2InvestPricePerTextBox는 추매부분 TextBox(안보이게 처리되어있음)
                reBuyingType2InvestPricePerTextBox_2.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_2.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_2.Text = "1";
                reBuyingType2InvestPricePerTextBox_3.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_3.Text = "1";
                reBuyingType2InvestPricePerTextBox_4.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_4.Text = "1";
                reBuyingType2InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_5.Text = "1";
                reBuyingType2InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_6.Text = "1";
            }
            else if (_buyingCount == 2)
            {
                investMoneyTextBox_2.ReadOnly = false;
                investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_3.ReadOnly = true;
                investMoneyTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_3.Text = "0";
                investMoneyTextBox_4.ReadOnly = true;
                investMoneyTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_4.Text = "0";
                investMoneyTextBox_5.ReadOnly = true;
                investMoneyTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_5.Text = "0";
                investMoneyTextBox_6.ReadOnly = true;
                investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_6.Text = "0";

                reBuyingType1InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_3.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_3.Text = "-6";
                reBuyingType1InvestPricePerTextBox_4.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_4.Text = "-9";
                reBuyingType1InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_5.Text = "-12";
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_6.Text = "-15";

                reBuyingType2MoveLineTextBox_2.ReadOnly = false;
                reBuyingType2MoveLineTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_3.ReadOnly = true;
                reBuyingType2MoveLineTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_3.Text = "1";
                reBuyingType2MoveLineTextBox_4.ReadOnly = true;
                reBuyingType2MoveLineTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_4.Text = "1";
                reBuyingType2MoveLineTextBox_5.ReadOnly = true;
                reBuyingType2MoveLineTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_5.Text = "1";
                reBuyingType2MoveLineTextBox_6.ReadOnly = true;
                reBuyingType2MoveLineTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_6.Text = "1";

                reBuyingType2InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_3.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_3.Text = "1";
                reBuyingType2InvestPricePerTextBox_4.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_4.Text = "1";
                reBuyingType2InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_5.Text = "1";
                reBuyingType2InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_6.Text = "1";
            }
            else if (_buyingCount == 3)
            {
                investMoneyTextBox_2.ReadOnly = false;
                investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_3.ReadOnly = false;
                investMoneyTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_4.ReadOnly = true;
                investMoneyTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_4.Text = "0";
                investMoneyTextBox_5.ReadOnly = true;
                investMoneyTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_5.Text = "0";
                investMoneyTextBox_6.ReadOnly = true;
                investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_6.Text = "0";

                reBuyingType1InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_4.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_4.Text = "-9";
                reBuyingType1InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_5.Text = "-12";
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_6.Text = "-15";

                reBuyingType2MoveLineTextBox_2.ReadOnly = false;
                reBuyingType2MoveLineTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_3.ReadOnly = false;
                reBuyingType2MoveLineTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_4.ReadOnly = true;
                reBuyingType2MoveLineTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_4.Text = "1";
                reBuyingType2MoveLineTextBox_5.ReadOnly = true;
                reBuyingType2MoveLineTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_5.Text = "1";
                reBuyingType2MoveLineTextBox_6.ReadOnly = true;
                reBuyingType2MoveLineTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_6.Text = "1";

                reBuyingType2InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_4.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_4.Text = "1";
                reBuyingType2InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_5.Text = "1";
                reBuyingType2InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_6.Text = "1";
            }
            else if (_buyingCount == 4)
            {
                investMoneyTextBox_2.ReadOnly = false;
                investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_3.ReadOnly = false;
                investMoneyTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_4.ReadOnly = false;
                investMoneyTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_5.ReadOnly = true;
                investMoneyTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_5.Text = "0";
                investMoneyTextBox_6.ReadOnly = true;
                investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_6.Text = "0";

                reBuyingType1InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_4.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_5.Text = "-12";
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_6.Text = "-15";

                reBuyingType2MoveLineTextBox_2.ReadOnly = false;
                reBuyingType2MoveLineTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_3.ReadOnly = false;
                reBuyingType2MoveLineTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_4.ReadOnly = false;
                reBuyingType2MoveLineTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_5.ReadOnly = true;
                reBuyingType2MoveLineTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_5.Text = "1";
                reBuyingType2MoveLineTextBox_6.ReadOnly = true;
                reBuyingType2MoveLineTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_6.Text = "1";

                reBuyingType2InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_4.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_5.Text = "1";
                reBuyingType2InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_6.Text = "1";
            }
            else if (_buyingCount == 5)
            {
                investMoneyTextBox_2.ReadOnly = false;
                investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_3.ReadOnly = false;
                investMoneyTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_4.ReadOnly = false;
                investMoneyTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_5.ReadOnly = false;
                investMoneyTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_6.ReadOnly = true;
                investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_6.Text = "0";

                reBuyingType1InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_4.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_5.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_6.Text = "-15";

                reBuyingType2MoveLineTextBox_2.ReadOnly = false;
                reBuyingType2MoveLineTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_3.ReadOnly = false;
                reBuyingType2MoveLineTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_4.ReadOnly = false;
                reBuyingType2MoveLineTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_5.ReadOnly = false;
                reBuyingType2MoveLineTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_6.ReadOnly = true;
                reBuyingType2MoveLineTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_6.Text = "1";

                reBuyingType2InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_4.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_5.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_6.Text = "1";
            }
            else if (_buyingCount == 6)
            {
                investMoneyTextBox_2.ReadOnly = false;
                investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_3.ReadOnly = false;
                investMoneyTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_4.ReadOnly = false;
                investMoneyTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_5.ReadOnly = false;
                investMoneyTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_6.ReadOnly = false;
                investMoneyTextBox_6.BackColor = Color.FromArgb(255, 255, 255);

                reBuyingType1InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_4.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_5.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(255, 255, 255);

                reBuyingType2MoveLineTextBox_2.ReadOnly = false;
                reBuyingType2MoveLineTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_3.ReadOnly = false;
                reBuyingType2MoveLineTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_4.ReadOnly = false;
                reBuyingType2MoveLineTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_5.ReadOnly = false;
                reBuyingType2MoveLineTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_6.ReadOnly = false;
                reBuyingType2MoveLineTextBox_6.BackColor = Color.FromArgb(255, 255, 255);

                reBuyingType2InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_4.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_5.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_6.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_6.BackColor = Color.FromArgb(255, 255, 255);
            }

            investMoneyTextBox_1.KeyPress += Key_Press;
            investMoneyTextBox_2.KeyPress += Key_Press;
            investMoneyTextBox_3.KeyPress += Key_Press;
            investMoneyTextBox_4.KeyPress += Key_Press;
            investMoneyTextBox_5.KeyPress += Key_Press;
            investMoneyTextBox_6.KeyPress += Key_Press;

            investMoneyTextBox_1.TextChanged += Text_Changed;
            investMoneyTextBox_2.TextChanged += Text_Changed;
            investMoneyTextBox_3.TextChanged += Text_Changed;
            investMoneyTextBox_4.TextChanged += Text_Changed;
            investMoneyTextBox_5.TextChanged += Text_Changed;
            investMoneyTextBox_6.TextChanged += Text_Changed;

            //buyingType3Period1.TextChanged += Text_PeriodChanged;

            buyingType2MoveBongPerTextBox.KeyPress += Key_Press6;
            buyingType3StocValueTextBox.KeyPress += Key_Press6;
            buyingType4MoveBongPerTextBox.KeyPress += Key_Press6;
            buyingType5MoveBongPerTextBox.KeyPress += Key_Press6;

            reBuyingType2MoveLineTextBox_2.KeyPress += Key_Press3;
            reBuyingType2MoveLineTextBox_3.KeyPress += Key_Press3;
            reBuyingType2MoveLineTextBox_4.KeyPress += Key_Press3;
            reBuyingType2MoveLineTextBox_5.KeyPress += Key_Press3;
            reBuyingType2MoveLineTextBox_6.KeyPress += Key_Press3;
            reBuyingType2MoveLineTextBox_2.LostFocus += LostFocusEvent;
            reBuyingType2MoveLineTextBox_3.LostFocus += LostFocusEvent;
            reBuyingType2MoveLineTextBox_4.LostFocus += LostFocusEvent;
            reBuyingType2MoveLineTextBox_5.LostFocus += LostFocusEvent;
            reBuyingType2MoveLineTextBox_6.LostFocus += LostFocusEvent;

            reBuyingType2InvestPricePerTextBox_2.KeyPress += Key_Press4;
            reBuyingType2InvestPricePerTextBox_3.KeyPress += Key_Press4;
            reBuyingType2InvestPricePerTextBox_4.KeyPress += Key_Press4;
            reBuyingType2InvestPricePerTextBox_4.KeyPress += Key_Press4;
            reBuyingType2InvestPricePerTextBox_5.KeyPress += Key_Press4;
            reBuyingType2InvestPricePerTextBox_6.KeyPress += Key_Press4;
            reBuyingType2InvestPricePerTextBox_2.LostFocus += LostFocusEvent;
            reBuyingType2InvestPricePerTextBox_3.LostFocus += LostFocusEvent;
            reBuyingType2InvestPricePerTextBox_4.LostFocus += LostFocusEvent;
            reBuyingType2InvestPricePerTextBox_5.LostFocus += LostFocusEvent;
            reBuyingType2InvestPricePerTextBox_6.LostFocus += LostFocusEvent;

            takeProfitType2MoveBongPerTextBox.KeyPress += Key_Press6;
            takeProfitType3StocValueTextBox.KeyPress += Key_Press6;
            takeProfitType4MoveBongPerTextBox.KeyPress += Key_Press6;
            takeProfitType5MoveBongPerTextBox.KeyPress += Key_Press6;

            stopLossType2MoveBongPerTextBox.KeyPress += Key_Press6;
            stopLossType3MoveBongPerTextBox.KeyPress += Key_Press6;
            stopLossType4MoveBongPerTextBox.KeyPress += Key_Press6;

            buyingType3Period1.KeyPress += Key_Press6;
            buyingType3Period2.KeyPress += Key_Press6;
            buyingType3Period3.KeyPress += Key_Press6;
            buyingType4Period1.KeyPress += Key_Press6;
            buyingType4Period2.KeyPress += Key_Press6;
            buyingType5Period1.KeyPress += Key_Press6;
            buyingType5Period2.KeyPress += Key_Press6;

            takeProfitType3Period1.KeyPress += Key_Press6;
            takeProfitType3Period2.KeyPress += Key_Press6;
            takeProfitType3Period3.KeyPress += Key_Press6;
            takeProfitType4Period1.KeyPress += Key_Press6;
            takeProfitType4Period2.KeyPress += Key_Press6;
            takeProfitType5Period1.KeyPress += Key_Press6;
            takeProfitType5Period2.KeyPress += Key_Press6;

            stopLossType3Period1.KeyPress += Key_Press6;
            stopLossType3Period2.KeyPress += Key_Press6;
            stopLossType4Period1.KeyPress += Key_Press6;
            stopLossType4Period2.KeyPress += Key_Press6;

            buyingType2MoveKindComboBox.KeyPress += Key_Press5;
            buyingType2DayComboBox.KeyPress += Key_Press5;
            buyingType3DayComboBox.KeyPress += Key_Press5;
            buyingType4DayComboBox.KeyPress += Key_Press5;
            buyingType5DayComboBox.KeyPress += Key_Press5;
            buyingType2MoveBongKindComboBox.KeyPress += Key_Press5;
            buyingType3MoveBongKindComboBox.KeyPress += Key_Press5;
            buyingType4MoveBongKindComboBox.KeyPress += Key_Press5;
            buyingType5MoveBongKindComboBox.KeyPress += Key_Press5;
            buyingType2MoveBongKindComboBox.TextChanged += BongTextChanged;
            buyingType3MoveBongKindComboBox.TextChanged += BongTextChanged;
            buyingType4MoveBongKindComboBox.TextChanged += BongTextChanged;
            buyingType5MoveBongKindComboBox.TextChanged += BongTextChanged;
            buyingType2DistanceComboBox.KeyPress += Key_Press5;
            buyingType3DistanceComboBox.KeyPress += Key_Press5;
            buyingType4DistanceComboBox.KeyPress += Key_Press5;
            buyingType5DistanceComboBox.KeyPress += Key_Press5;

            reBuyingType2MoveKindComboBox.KeyPress += Key_Press5;
            reBuyingType2DayComboBox.KeyPress += Key_Press5;
            reBuyingType2MoveBongKindComboBox.KeyPress += Key_Press5;
            reBuyingType2MoveBongKindComboBox.TextChanged += BongTextChanged;

            takeProfitType2MoveKindComboBox.KeyPress += Key_Press5;
            takeProfitType2DayComboBox.KeyPress += Key_Press5;
            takeProfitType3DayComboBox.KeyPress += Key_Press5;
            takeProfitType4DayComboBox.KeyPress += Key_Press5;
            takeProfitType5DayComboBox.KeyPress += Key_Press5;
            takeProfitType2MoveBongKindComboBox.KeyPress += Key_Press5;
            takeProfitType3MoveBongKindComboBox.KeyPress += Key_Press5;
            takeProfitType4MoveBongKindComboBox.KeyPress += Key_Press5;
            takeProfitType5MoveBongKindComboBox.KeyPress += Key_Press5;
            takeProfitType2MoveBongKindComboBox.TextChanged += BongTextChanged;
            takeProfitType3MoveBongKindComboBox.TextChanged += BongTextChanged;
            takeProfitType4MoveBongKindComboBox.TextChanged += BongTextChanged;
            takeProfitType5MoveBongKindComboBox.TextChanged += BongTextChanged;
            takeProfitType2DistanceComboBox.KeyPress += Key_Press5;
            takeProfitType3DistanceComboBox.KeyPress += Key_Press5;
            takeProfitType4DistanceComboBox.KeyPress += Key_Press5;
            takeProfitType5DistanceComboBox.KeyPress += Key_Press5;

            stopLossType2MoveKindComboBox.KeyPress += Key_Press5;
            stopLossType2DayComboBox.KeyPress += Key_Press5;
            stopLossType3DayComboBox.KeyPress += Key_Press5;
            stopLossType4DayComboBox.KeyPress += Key_Press5;
            stopLossType2MoveBongKindComboBox.KeyPress += Key_Press5;
            stopLossType3MoveBongKindComboBox.KeyPress += Key_Press5;
            stopLossType4MoveBongKindComboBox.KeyPress += Key_Press5;
            stopLossType2MoveBongKindComboBox.TextChanged += BongTextChanged;
            stopLossType3MoveBongKindComboBox.TextChanged += BongTextChanged;
            stopLossType4MoveBongKindComboBox.TextChanged += BongTextChanged;
            stopLossType2DistanceComboBox.KeyPress += Key_Press5;
            stopLossType3DistanceComboBox.KeyPress += Key_Press5;
            stopLossType4DistanceComboBox.KeyPress += Key_Press5;

            reBuyingType1InvestPricePerTextBox_2.KeyPress += Key_Press4;
            reBuyingType1InvestPricePerTextBox_3.KeyPress += Key_Press4;
            reBuyingType1InvestPricePerTextBox_4.KeyPress += Key_Press4;
            reBuyingType1InvestPricePerTextBox_5.KeyPress += Key_Press4;
            reBuyingType1InvestPricePerTextBox_6.KeyPress += Key_Press4;
            reBuyingType1InvestPricePerTextBox_2.LostFocus += LostFocusEvent;
            reBuyingType1InvestPricePerTextBox_3.LostFocus += LostFocusEvent;
            reBuyingType1InvestPricePerTextBox_4.LostFocus += LostFocusEvent;
            reBuyingType1InvestPricePerTextBox_5.LostFocus += LostFocusEvent;
            reBuyingType1InvestPricePerTextBox_6.LostFocus += LostFocusEvent;

            buyingType1TransferPriceNumericUpDown.LostFocus += LostFocusEvent2;
            takeProfitBuyingPricePerNumericUpDown1.LostFocus += LostFocusEvent2;
            stopLossBuyingPricePerNumericUpDown1.LostFocus += LostFocusEvent2;

            //익절
            takeProfitBuyingPricePerNumericUpDown1.KeyPress += Key_Press;
            takeProfitBuyingPricePerNumericUpDown2.KeyPress += Key_Press;
            takeProfitBuyingPricePerNumericUpDown3.KeyPress += Key_Press;
            takeProfitBuyingPricePerNumericUpDown4.KeyPress += Key_Press;
            takeProfitBuyingPricePerNumericUpDown5.KeyPress += Key_Press;

            //익절비중
            takeProfitPerNumericUpDown1.KeyPress += Key_Press;
            takeProfitPerNumericUpDown2.KeyPress += Key_Press;
            takeProfitPerNumericUpDown3.KeyPress += Key_Press;
            takeProfitPerNumericUpDown4.KeyPress += Key_Press;
            takeProfitPerNumericUpDown5.KeyPress += Key_Press;

            //손절비중
            stopLossPerNumericUpDown1.KeyPress += Key_Press;
            stopLossPerNumericUpDown2.KeyPress += Key_Press;
            stopLossPerNumericUpDown3.KeyPress += Key_Press;
            stopLossPerNumericUpDown4.KeyPress += Key_Press;
            stopLossPerNumericUpDown5.KeyPress += Key_Press;

            //손절
            stopLossBuyingPricePerNumericUpDown1.KeyPress += Key_Press;
            stopLossBuyingPricePerNumericUpDown2.KeyPress += Key_Press;
            stopLossBuyingPricePerNumericUpDown3.KeyPress += Key_Press;
            stopLossBuyingPricePerNumericUpDown4.KeyPress += Key_Press;
            stopLossBuyingPricePerNumericUpDown5.KeyPress += Key_Press;

            // 이평범위
            buyingType2MoveLineTextBox.KeyPress += Key_Press2;
            takeProfitType2MoveLineTextBox.KeyPress += Key_Press2;
            stopLossType2MoveLineTextBox.KeyPress += Key_Press2;

            buyingType2MoveLineTextBox.TextChanged += MovingPriceTextChanged;
            takeProfitType2MoveLineTextBox.TextChanged += MovingPriceTextChanged;
            stopLossType2MoveLineTextBox.TextChanged += MovingPriceTextChanged;

            // 포커스가 상실되었을때 공백인 경우 1로 채워준다.
            // 매수 이평
            buyingType2MoveLineTextBox.LostFocus += LostFocusEvent;
            buyingType2MoveBongPerTextBox.LostFocus += LostFocusEvent;
            // 매수 스토캐스틱
            buyingType3Period1.LostFocus += LostFocusEvent;
            buyingType3Period2.LostFocus += LostFocusEvent;
            buyingType3Period3.LostFocus += LostFocusEvent;
            buyingType3StocValueTextBox.LostFocus += LostFocusEvent;
            // 매수 볼린저
            buyingType4Period1.LostFocus += LostFocusEvent;
            buyingType4Period2.LostFocus += LostFocusEvent;
            buyingType4MoveBongPerTextBox.LostFocus += LostFocusEvent;
            // 매수 엔벨로프
            buyingType5Period1.LostFocus += LostFocusEvent;
            buyingType5Period2.LostFocus += LostFocusEvent;
            buyingType5MoveBongPerTextBox.LostFocus += LostFocusEvent;
            // 익절 이평
            takeProfitType2MoveLineTextBox.LostFocus += LostFocusEvent;
            takeProfitType2MoveBongPerTextBox.LostFocus += LostFocusEvent;
            // 익절 스토캐스틱
            takeProfitType3Period1.LostFocus += LostFocusEvent;
            takeProfitType3Period2.LostFocus += LostFocusEvent;
            takeProfitType3Period3.LostFocus += LostFocusEvent;
            takeProfitType3StocValueTextBox.LostFocus += LostFocusEvent;
            // 익절 볼린저
            takeProfitType4Period1.LostFocus += LostFocusEvent;
            takeProfitType4Period2.LostFocus += LostFocusEvent;
            takeProfitType4MoveBongPerTextBox.LostFocus += LostFocusEvent;
            // 익절 엔벨로프
            takeProfitType5Period1.LostFocus += LostFocusEvent;
            takeProfitType5Period2.LostFocus += LostFocusEvent;
            takeProfitType5MoveBongPerTextBox.LostFocus += LostFocusEvent;
            // 손절 이평
            stopLossType2MoveLineTextBox.LostFocus += LostFocusEvent;
            stopLossType2MoveBongPerTextBox.LostFocus += LostFocusEvent;
            // 손절 볼린저
            stopLossType3Period1.LostFocus += LostFocusEvent;
            stopLossType3Period2.LostFocus += LostFocusEvent;
            stopLossType3MoveBongPerTextBox.LostFocus += LostFocusEvent;
            // 손절 엔벨로프
            stopLossType4Period1.LostFocus += LostFocusEvent;
            stopLossType4Period2.LostFocus += LostFocusEvent;
            stopLossType4MoveBongPerTextBox.LostFocus += LostFocusEvent;


            // 스토캐스틱 slow
            buyingType3Period1.TextChanged += StocTextChanged;
            buyingType3Period2.TextChanged += StocTextChanged;
            buyingType3Period3.TextChanged += StocTextChanged;

            takeProfitType3Period1.TextChanged += StocTextChanged;
            takeProfitType3Period2.TextChanged += StocTextChanged;
            takeProfitType3Period3.TextChanged += StocTextChanged;

            // 볼린저밴드, 엔벨로프
            buyingType4Period1.TextChanged += BollEnveTextChanged;
            buyingType4Period2.TextChanged += BollEnveTextChanged;
            buyingType5Period1.TextChanged += BollEnveTextChanged;
            buyingType5Period2.TextChanged += BollEnveTextChanged;

            takeProfitType4Period1.TextChanged += BollEnveTextChanged;
            takeProfitType4Period2.TextChanged += BollEnveTextChanged;
            takeProfitType5Period1.TextChanged += BollEnveTextChanged;
            takeProfitType5Period2.TextChanged += BollEnveTextChanged;

            stopLossType3Period1.TextChanged += BollEnveTextChanged;
            stopLossType3Period2.TextChanged += BollEnveTextChanged;
            stopLossType4Period1.TextChanged += BollEnveTextChanged;
            stopLossType4Period2.TextChanged += BollEnveTextChanged;

            buyingType1TransferPriceUpDownNumericUpDown.KeyPress += Key_Press5;

            conditionComboBox.KeyPress += Key_Press5;
            buyingTypeComboBox.KeyPress += Key_Press5;
            addBuyingTypeComboBox.KeyPress += Key_Press5;
            takeProfitTypeComboBox.KeyPress += Key_Press5;
            stopLossTypeComboBox.KeyPress += Key_Press5;

            investmentPerItemTextBox.KeyPress += Key_Press3;
            buyingItemCountTextBox.KeyPress += Key_Press3;
            transferItemCountTextBox.KeyPress += Key_Press3;
            investBuyingCountNumeriUpDown.KeyPress += Key_Press3;
            investBuyingCountNumeriUpDown.LostFocus += LostFocusEvent3;

            tsnumericUpDown1.KeyPress += Key_Press;
            tsMedonumericUpDown1.KeyPress += Key_Press;
            tspernumericUpDown1.KeyPress += Key_Press3;

            tsnumericUpDown1.LostFocus += LostFocusEvent4;
            tsMedonumericUpDown1.LostFocus += LostFocusEvent4;
            tspernumericUpDown1.LostFocus += LostFocusEvent4;

            tsnumericUpDown2.KeyPress += Key_Press;
            tsMedonumericUpDown2.KeyPress += Key_Press;
            tspernumericUpDown2.KeyPress += Key_Press3;

            tsnumericUpDown2.LostFocus += LostFocusEvent4;
            tsMedonumericUpDown2.LostFocus += LostFocusEvent4;
            tspernumericUpDown2.LostFocus += LostFocusEvent4;

            tsnumericUpDown3.KeyPress += Key_Press;
            tsMedonumericUpDown3.KeyPress += Key_Press;
            tspernumericUpDown3.KeyPress += Key_Press3;

            tsnumericUpDown3.LostFocus += LostFocusEvent4;
            tsMedonumericUpDown3.LostFocus += LostFocusEvent4;
            tspernumericUpDown3.LostFocus += LostFocusEvent4;

        }
        private void ButtonClickEvent(object sender, EventArgs e)
        {
            //등록하기 버튼
            if(sender.Equals(conditionRegisterbutton))
            {
                ConditionRegister();
            }
            //저장하기 버튼
            else if (sender.Equals(conditionSaveButton))
            {
                saveCondition(0, -1);
                /*
                // 투자금 설정확인
                if(investmentPerItemTextBox.Text =="")
                {
                    MessageBox.Show("종목당 투자금을 설정해주세요.");
                    return;
                        
                }
                //투자금이 0이거나 작으면
                double _itemInvestment = double.Parse(investmentPerItemTextBox.Text);
                if(_itemInvestment <=0)
                {
                    MessageBox.Show("종목당 투자금을 설정해주세요.");
                    return;
                }
                //매수 종목수 설정확인
                if(buyingItemCountTextBox.Text == "")
                {
                    MessageBox.Show("매수 종목수를 설정해 주세요.");
                    return;
                }
                //매수 종목수가 0이거나 작으면
                double _buyingItemCount = double.Parse(buyingItemCountTextBox.Text);
                if (_buyingItemCount <= 0)
                {
                    MessageBox.Show("매수 종목수를 설정해 주세요.");
                    return;
                }
                //조건식 관련 데이터 가져오기
                string _conditionName = conditionComboBox.Text;
                ConditionData _condition = gMainForm.conditionDataList.Find(o => o.conditionName.Equals(_conditionName));
                // 매수방식 _bPurchaseType == true : 즉시매수, _bPurchaseType == false 편입가격대비 매수
                bool _bPurchaseType = true;
                if (buyingType1TransferPricePerRadioButton.Checked) // 편입가격대비매수로 선택되어 있다면
                    _bPurchaseType = false;
                // 추매사용
                bool _rePurchase = false;
                if (rePurchaseCheckBox.Checked)
                    _rePurchase = true;
                // 추매금액
                string _rePurchaseMoney = "";
                _rePurchaseMoney += rePurchaseMoneyTextBox1.Text +";";
                _rePurchaseMoney += rePurchaseMoneyTextBox2.Text + ";";
                _rePurchaseMoney += rePurchaseMoneyTextBox3.Text + ";";
                _rePurchaseMoney += rePurchaseMoneyTextBox4.Text + ";";
                _rePurchaseMoney += rePurchaseMoneyTextBox5.Text + ";";
                _rePurchaseMoney += rePurchaseMoneyTextBox6.Text + ";";
                // 추매매수가격대비비율
                string _rePurchaseRate = "";
                _rePurchaseRate += rePurchaseRateNumericUpDown1.Text + ";";
                _rePurchaseRate += rePurchaseRateNumericUpDown2.Text + ";";
                _rePurchaseRate += rePurchaseRateNumericUpDown3.Text + ";";
                _rePurchaseRate += rePurchaseRateNumericUpDown4.Text + ";";
                _rePurchaseRate += rePurchaseRateNumericUpDown5.Text + ";";
                _rePurchaseRate += rePurchaseRateNumericUpDown6.Text + ";";

                
                // 저장하기
                gMainForm.gFileIOInstance.saveConditionList(
                                        _conditionName, _condition.conditionIndex, //조건식 이름, 조건식인덱스
                                        investmentPerItemTextBox.Text, buyingItemCountTextBox.Text, // 종목당투자금, 매수종목수
                                        _bPurchaseType, Convert.ToDouble(buyingType1TransferPriceNumericUpDown.Value), //매수타입, 편입가격대비 %
                                        Convert.ToDouble(takeProfitBuyingPricePerNumericUpDown.Value), // 익절%
                                        Convert.ToDouble(stopLossBuyingPricePerNumericUpDown.Value), // 손절%
                                        _rePurchase,_rePurchaseMoney,_rePurchaseRate);*/
            }
            //수정하기 버튼
            else if (sender.Equals(conditionEditButton))
            {
                string[] cListData = gMainForm.gFileIOInstance.getConditionListData();
                int cListDataLength = cListData.Length / (int)Save.Condition;

                if (cListDataLength == 0)
                {
                    MessageBox.Show("저장된 조건식 설정이 없습니다.");
                    return;
                }

                string conditionIndex = gMainForm.conditionDataList[conditionComboBox.SelectedIndex].conditionIndex.ToString();
                int sameNumber = gMainForm.gFileIOInstance.getSameConditionIndex(conditionIndex);
                if (sameNumber == -1)
                {
                    MessageBox.Show("저장된 조건식 설정이 없습니다.");
                    return;
                }

                string message = "조건식 설정을 수정하시겠습니까?";
                DialogResult result = MessageBox.Show(message, "수정하기", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    saveCondition(1, sameNumber);
                }
                /*
                string _conditionName = conditionComboBox.Text;
                // 저장되어 있는지, 있으면 저장 위치를 리턴받는다.
                int _sameIndex = gMainForm.gFileIOInstance.getSameConditionIndex(_conditionName);
                if(_sameIndex == -1)
                {
                    MessageBox.Show("저장된 조건식이 없습니다.");
                    return;
                }

                string message = "조건식 매매 방식을 수정할까요?";
                DialogResult result = MessageBox.Show(message, "조건식 매매 방식 수정", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                {
                    ConditionData _condition = gMainForm.conditionDataList.Find(o => o.conditionName.Equals(_conditionName));
                    bool _bPurchaseType = true;
                    if(buyingType1TransferPricePerRadioButton.Checked) //편입가격대비 라디오버튼이 선택되어 있으면
                    {
                        _bPurchaseType = false;
                    }
                    // 추매사용
                    bool _rePurchase = false;
                    if (rePurchaseCheckBox.Checked)
                        _rePurchase = true;
                    // 추매금액
                    string _rePurchaseMoney = "";
                    _rePurchaseMoney += rePurchaseMoneyTextBox1.Text + ";";
                    _rePurchaseMoney += rePurchaseMoneyTextBox2.Text + ";";
                    _rePurchaseMoney += rePurchaseMoneyTextBox3.Text + ";";
                    _rePurchaseMoney += rePurchaseMoneyTextBox4.Text + ";";
                    _rePurchaseMoney += rePurchaseMoneyTextBox5.Text + ";";
                    _rePurchaseMoney += rePurchaseMoneyTextBox6.Text + ";";
                    // 추매매수가격대비비율
                    string _rePurchaseRate = "";
                    _rePurchaseRate += rePurchaseRateNumericUpDown1.Text + ";";
                    _rePurchaseRate += rePurchaseRateNumericUpDown2.Text + ";";
                    _rePurchaseRate += rePurchaseRateNumericUpDown3.Text + ";";
                    _rePurchaseRate += rePurchaseRateNumericUpDown4.Text + ";";
                    _rePurchaseRate += rePurchaseRateNumericUpDown5.Text + ";";
                    _rePurchaseRate += rePurchaseRateNumericUpDown6.Text + ";";
                    /*
                    //수정하기
                    gMainForm.gFileIOInstance.editConditionList(
                            _sameIndex, _conditionName, _condition.conditionIndex,
                            investmentPerItemTextBox.Text, buyingItemCountTextBox.Text,
                            _bPurchaseType, Convert.ToDouble(buyingType1TransferPriceNumericUpDown.Value),
                            Convert.ToDouble(takeProfitBuyingPricePerNumericUpDown.Value),
                            Convert.ToDouble(stopLossBuyingPricePerNumericUpDown.Value),
                            _rePurchase, _rePurchaseMoney, _rePurchaseRate);

                    // 그리드뷰에 등록되어있는지 확인해서 등록이 되어있으면 수정
                    double _itemInvetment = double.Parse(investmentPerItemTextBox.Text);
                    int _buyingItemCount = int.Parse(buyingItemCountTextBox.Text);

                    bool _buyingType2 = true;
                    double _transferPricePercent = 0.0;
                    if(buyingType1TransferPricePerRadioButton.Checked) // 편입가격대비매수 버튼이 체크되어있으면
                    {
                        _buyingType2 = false;
                        _transferPricePercent = Convert.ToDouble(buyingType1TransferPriceNumericUpDown.Value);
                    }
                    // 익절
                    double _takeProfitBuyinhPricePercent = Convert.ToDouble(takeProfitBuyingPricePerNumericUpDown.Value);
                    // 손절
                    double _stopLossBuyingPricePercent = Convert.ToDouble(stopLossBuyingPricePerNumericUpDown.Value);

                    // 추매타입
                    bool _rePurchase2 = false;
                    if (rePurchaseCheckBox.Checked) //추매사용버튼이 체크되어있다면
                        _rePurchase2 = true;

                    //ts매도사용
                    bool _tsMedo = false;
                    if (tsMedoCheckBox.Checked)
                        _tsMedo = true;
                    //ts매도 %
                    double _tsMedoPercent = Convert.ToDouble(tsMedonumericUpDown.Value);

                    // MyTradingCondition 생성
                    MyTradingCondition mtc = new MyTradingCondition(null, _itemInvetment, _buyingItemCount,
                                                                    _buyingType2, _transferPricePercent, _takeProfitBuyinhPricePercent,
                                                                    _stopLossBuyingPricePercent, _rePurchase2, _rePurchaseMoney, _rePurchaseRate,
                                                                    null, _tsMedo, _tsMedoPercent); // 조건식이름과 스크린번호가 null값인 이유는 서버로부터 데이터수신이아니고, 텍스트파일에서 조건식이름을 받아오고, 스크린번호는 필요없기때문
                    foreach(DataGridViewRow row in gMainForm.tradingConditionDataGridView.Rows)
                    {
                        if (row == null)
                            continue;

                        if(row.Cells["매매조건식_조건식"].Value.ToString().Equals(_conditionName))
                        {
                            row.Cells["매매조건식_종목당투자금"].Value = mtc.itemInvestment.ToString("N0"); // 종목당투자금
                            row.Cells["매매조건식_매수종목수"].Value = mtc.buyingItemCount.ToString("N0"); // 매수종목수
                            if (mtc.buyingType)
                                row.Cells["매매조건식_매수타입"].Value = "즉시매수";
                            else
                                row.Cells["매매조건식_매수타입"].Value = mtc.transferPricePercent.ToString("N2") + "%";
                            row.Cells["매매조건식_익절"].Value = mtc.takeProfitBuyingPricePercent.ToString("N2") + "%"; // 익절%
                            row.Cells["매매조건식_손절"].Value = mtc.stopLossBuyingPricePercent.ToString("N2") + "%"; // 손절%

                            if (mtc.rePurchase)
                                row.Cells["매매조건식_추매"].Value = "사용함";
                            else
                                row.Cells["매매조건식_추매"].Value = "사용안함";
                        }
                    }
                    // 조건식데이터리스트에 있는지 확인 후 수정
                    foreach(MyTradingCondition _mtc in gMainForm.gMyTradingConditionList)
                    {
                        if (_mtc == null)
                            continue;

                        int _itemCount = _mtc.MyTradingItemList.Count; //현재 진행 종목수

                        string _conditionName2 = _mtc.conditionData.conditionName;
                        if(_conditionName2.Equals(_conditionName))
                        {
                            _mtc.itemInvestment = _itemInvetment;
                            _mtc.buyingItemCount = _buyingItemCount;
                            _mtc.remainingBuyingItemCount = _buyingItemCount - _itemCount; //남은 매수 종목수
                            if (_mtc.remainingBuyingItemCount < 0)
                                _mtc.remainingBuyingItemCount = 0;

                            _mtc.buyingType = _buyingType2; //매수타입
                            _mtc.transferPricePercent = _transferPricePercent; //매수: 편입단가대비%
                            _mtc.takeProfitBuyingPricePercent = _takeProfitBuyinhPricePercent; // 익절: 매수단가대비 %
                            _mtc.stopLossBuyingPricePercent = _stopLossBuyingPricePercent; // 손절: 매수단가대비 %
                            _mtc.rePurchase = _rePurchase2;

                            string[] _money = _rePurchaseMoney.Split(';');
                            string[] _rate = _rePurchaseRate.Split(';');
                            for(int i=0;i<6;i++)
                            {
                                _mtc.rePurchaseMoney[i] = int.Parse(_money[i]);
                                _mtc.rePurchaseRate[i] = double.Parse(_rate[i]);
                            }
                            break;
                        }
                    }
                    MessageBox.Show("조건식이 수정 되었습니다.");
                }*/
            }
            //불러오기 버튼
            else if (sender.Equals(conditionLoadButton))
            {
                if(gMainForm.ConditionLoadDig.setJogunDataLoading())
                {
                    //row선택 번호 초기화
                    gMainForm.ConditionLoadDig.curSelectRowNumber = 0;
                    //조건식 불러오기창 오픈
                    gMainForm.ConditionLoadDig.ShowDialog();
                }
            }
        }

        private void saveCondition(int type, int editnumber)
        {
            string _strInvestment = string.Empty, _strInvestmentPrice = string.Empty, _strBuying = string.Empty, _strReBuying = string.Empty, _strTakeProfit = string.Empty, _strStopLoss = string.Empty, _strTsmedo = string.Empty;

            // 투자금 설정확인
            if (investmentPerItemTextBox.Text == "")
            {
                MessageBox.Show("종목당 투자금을 설정해주세요.");
                return;

            }
            //투자금이 0이거나 작으면
            double _itemInvestment = double.Parse(investmentPerItemTextBox.Text);
            if (_itemInvestment <= 0)
            {
                MessageBox.Show("종목당 투자금을 설정해주세요.");
                return;
            }
            //매수 종목수 설정확인
            if (buyingItemCountTextBox.Text == "")
            {
                MessageBox.Show("매수 종목수를 설정해 주세요.");
                return;
            }
            if (transferItemCountTextBox.Text == "")
            {
                MessageBox.Show("편입 종목수를 설정해 주세요.");
                return;
            }
            //매수 종목수가 0이거나 작으면
            int _buyingItemCount = int.Parse(buyingItemCountTextBox.Text);
            if (_buyingItemCount <= 0)
            {
                MessageBox.Show("매수 종목수를 설정해 주세요.");
                return;
            }
            int _transferItemCount = int.Parse(transferItemCountTextBox.Text);
            if(_transferItemCount <= 0)
            {
                MessageBox.Show("편입 종목수를 설정해 주세요.");
                return;
            }

            // 계좌번호
            string account = gMainForm.myAccountComboBox.Text;
            // 조건식 이름
            string conditionName = gMainForm.conditionDataList[conditionComboBox.SelectedIndex].conditionName;
            // 조건식 번호
            string conditionIndex = gMainForm.conditionDataList[conditionComboBox.SelectedIndex].conditionIndex.ToString();
            // 매수 종목 수
            string buyingItemCount = buyingItemCountTextBox.Text;
            // 편입 종목 수
            string transferItemCount = transferItemCountTextBox.Text;
            // 종목별 투자금액
            string _sitemInvestment = investmentPerItemTextBox.Text;
            string _stotalItemInvestment = investMoneyTotalTextBox.Text;
            double _totalItemInvestment = double.Parse(_stotalItemInvestment.Replace(",", ""));

            if(_itemInvestment < _totalItemInvestment)
            {
                MessageBox.Show("매수 금액 합계가 종목당 투자금을 초과하였습니다.");
                return;
            }
            _strInvestment += account + ";" + conditionName + ";" + conditionIndex + ";" + buyingItemCount + ";" + _sitemInvestment + ";" + transferItemCount;

            //////////////////////////////////////////// 매수금액설정 /////////////////////////////////////////////////////
            // 저장값
            // 매수횟수, 회차별 투자금 buyingInvestment[0],buyingInvestment[1],buyingInvestment[2],buyingInvestment[3],buyingInvestment[4],buyingInvestment[5],
            string buyingCount = investBuyingCountNumeriUpDown.Value.ToString(); // 매수회수(추매 포함)
            string investMoney_1 = investMoneyTextBox_1.Text;
            string investMoney_2 = investMoneyTextBox_2.Text;
            string investMoney_3 = investMoneyTextBox_3.Text;
            string investMoney_4 = investMoneyTextBox_4.Text;
            string investMoney_5 = investMoneyTextBox_5.Text;
            string investMoney_6 = investMoneyTextBox_6.Text;

            int buyingCountValue = int.Parse(buyingCount);

            if (buyingCountValue > 0)
            {
                if (double.Parse(investMoney_1.Replace(",", "")) <= 0)
                {
                    MessageBox.Show("최소 매수 금액을 설정해 주세요.");
                    return;
                }
            }
            if (buyingCountValue > 1)
            {
                if (double.Parse(investMoney_2.Replace(",", "")) <= 0)
                {
                    MessageBox.Show("1차 추매 매수 금액을 설정해 주세요.");
                    return;
                }
            }
            if (buyingCountValue > 2)
            {
                if (double.Parse(investMoney_3.Replace(",", "")) <= 0)
                {
                    MessageBox.Show("2차 추매 매수 금액을 설정해 주세요.");
                    return;
                }
            }
            if (buyingCountValue > 3)
            {
                if (double.Parse(investMoney_4.Replace(",", "")) <= 0)
                {
                    MessageBox.Show("3차 추매 매수 금액을 설정해 주세요.");
                    return;
                }
            }
            if (buyingCountValue > 4)
            {
                if (double.Parse(investMoney_5.Replace(",", "")) <= 0)
                {
                    MessageBox.Show("4차 추매 매수 금액을 설정해 주세요.");
                    return;
                }
            }
            if (buyingCountValue > 5)
            {
                if (double.Parse(investMoney_6.Replace(",", "")) <= 0)
                {
                    MessageBox.Show("5차 추매 매수 금액을 설정해 주세요.");
                    return;
                }
            }
            _strInvestmentPrice += buyingCount + ";" + investMoney_1 + ";" + investMoney_2 + ";" + investMoney_3 + ";" + investMoney_4 + ";" + investMoney_5 + ";" + investMoney_6;

            /////////////////////////////////////////////// 매수 설정 ////////////////////////////////////////////////
            // 매수 사용 여부
            int buyingUsing = 1;
            if (buyingCheckBox.Checked) buyingUsing = 1; // 매수 설정 -> 매수방식선택 체크박스
            else buyingUsing = 0;
            int buyingType = buyingTypeComboBox.SelectedIndex; // 0:기본매수, 1:이동평균,2:스토케스틱,3:볼린저밴드,4:엔벨로프
            int buyingTransferType = 0; // 0: 편입즉시매수 1: 편입가격대비매수 2:저점대비상승매수
            double buyingTransferPer = 0; // 편입가격대비매수시 %
            int buyingTransferUpdown = 0; //편입가격대비 매수시 %에 대한 0: 이상, 1: 이하
            double buyingTransferPer2 = 0; // 저점대비 도달 %
            double riseTransferPer2 = 0; // 저점대비 반등 %
            int mesuoption1 = mesuoption1comboBox.SelectedIndex; // 0: 시장가 , 1: 현재가(지정가)
            int nontradingtime = 0; // 정정대기시간
            int mesuoption2 = mesuoption2comboBox.SelectedIndex; // 1: 시장가로정정 , 2: 일괄정정 , 3: 일괄취소
            // 공통 사용처리 변수
            int buyingMovePriceKindType = 0; //0:단순 , 1:지수
            int buyingBongType = 0; // 0:월, 1:주 ,2:일, 3:분
            int buyingMinuteType = 0; // 1,3,5분봉 등등 입력
            int buyingMinuteLineType = 0;// 1,3,5,20이평 등등            
            double buyingMinuteLineAccessPer = 0;// 근접 %
            int buyingDistance = 0; // 0:근접, 1:돌파, 2:이탈, 0:이상, 1:이하
            double buyingStocPeriod1 = 0; //기간
            double buyingStocPeriod2 = 0; //%K
            double buyingStocPeriod3 = 0; //%D
            double buyingBollPeriod = 0; //승수, 엔벨%
            int buyingLine3Type = 0; // 0:상한선,1:중심선,2:하한선

            if (buyingType < 0)
            {
                MessageBox.Show("매수 방식 선택 리스트를 확인해 주세요.");
                return;
            }
            if(buyingType ==0)//기본매수
            {
                if(mesuoption1 == 0)
                {
                    if (buyingType1ImmediatelyBuyingRadioButton.Checked) buyingTransferType = 0; // 편입 즉시매수가 체크되어있다면
                    else if (buyingType1TransferPricePerRadioButton.Checked) // 편입가격대비매수가 체크되어있다면
                    {
                        buyingTransferType = 1;
                        // 대비 n%
                        buyingTransferPer = (double)buyingType1TransferPriceNumericUpDown.Value;
                        // 이상, 이하
                        buyingTransferUpdown = buyingType1TransferPriceUpDownNumericUpDown.SelectedIndex;
                    }
                    else if (buyingType2TransferPricePerRadioButton.Checked) // 저점대비매수가 체크되어있다면
                    {
                        buyingTransferType = 2;
                        buyingTransferPer2 = (double)buyingType2TransferPriceNumericUpDown.Value;
                        riseTransferPer2 = (double)buyingType2RisePriceNumericUpDown.Value;
                    }
                }
                else if(mesuoption1 == 1)
                {
                    nontradingtime = (int)nontradingtimeNumericUpDown.Value;
                    mesuoption2 = mesuoption2comboBox.SelectedIndex;
                    if (buyingType1ImmediatelyBuyingRadioButton.Checked) buyingTransferType = 0; // 편입 즉시매수가 체크되어있다면
                    else if (buyingType1TransferPricePerRadioButton.Checked) // 편입가격대비매수가 체크되어있다면
                    {
                        buyingTransferType = 1;
                        // 대비 n%
                        buyingTransferPer = (double)buyingType1TransferPriceNumericUpDown.Value;
                        // 이상, 이하
                        buyingTransferUpdown = buyingType1TransferPriceUpDownNumericUpDown.SelectedIndex;
                    }
                    else if (buyingType2TransferPricePerRadioButton.Checked) // 저점대비매수가 체크되어있다면
                    {
                        buyingTransferType = 2;
                        buyingTransferPer2 = (double)buyingType2TransferPriceNumericUpDown.Value;
                        riseTransferPer2 = (double)buyingType2RisePriceNumericUpDown.Value;
                    }
                }

            }
            else if(buyingType ==1) // 이동평균선
            {
                // 이평종류 0:단순 ,1:지수
                buyingMovePriceKindType = buyingType2MoveBongKindComboBox.SelectedIndex;
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = buyingType2DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = buyingType2MoveBongKindComboBox.SelectedIndex;
                // 1,3,5,20 이평종류
                buyingMinuteLineType = int.Parse(buyingType2MoveLineTextBox.Text);
                // n%
                buyingMinuteLineAccessPer = double.Parse(buyingType2MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파
                buyingDistance = buyingType2DistanceComboBox.SelectedIndex;
            }
            else if (buyingType == 2) // 스토캐스틱
            {
                // 기간
                buyingStocPeriod1 = double.Parse(buyingType3Period1.Text);
                // %K
                buyingStocPeriod2 = double.Parse(buyingType3Period2.Text);
                // %D
                buyingStocPeriod3 = double.Parse(buyingType3Period3.Text);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = buyingType3DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = buyingType3MoveBongKindComboBox.SelectedIndex;
                // k값
                buyingMinuteLineAccessPer = double.Parse(buyingType3StocValueTextBox.Text);
                // 0:이상, 1:이하
                buyingDistance = buyingType3DistanceComboBox.SelectedIndex;
            }
            else if (buyingType == 3) // 볼린져밴드
            {
                // 기간
                buyingStocPeriod1 = double.Parse(buyingType4Period1.Text);
                // 승수
                buyingBollPeriod = double.Parse(buyingType4Period2.Text);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = buyingType4DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = buyingType4MoveBongKindComboBox.SelectedIndex;
                // 상한선, 중심선, 하한선(0, 1, 2)
                buyingLine3Type = buyingType4LineComboBox.SelectedIndex;
                // n%
                buyingMinuteLineAccessPer = double.Parse(buyingType4MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파, 2:이탈
                buyingDistance = buyingType4DistanceComboBox.SelectedIndex;
            }
            else if (buyingType == 4) // 엔벨로프
            {
                // 기간
                buyingStocPeriod1 = double.Parse(buyingType5Period1.Text);
                // 승수
                buyingBollPeriod = double.Parse(buyingType5Period2.Text);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = buyingType5DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = buyingType5MoveBongKindComboBox.SelectedIndex;
                // 상한선, 중심선, 하한선(0, 1, 2)
                buyingLine3Type = buyingType5LineComboBox.SelectedIndex;
                // n%
                buyingMinuteLineAccessPer = double.Parse(buyingType5MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파, 2:이탈
                buyingDistance = buyingType5DistanceComboBox.SelectedIndex;
            }
            _strBuying += buyingUsing.ToString() + ";" + buyingType.ToString() + ";" + buyingTransferType.ToString() + ";" + buyingTransferPer.ToString() + ";" + buyingTransferUpdown.ToString() + ";" +
                                  buyingMovePriceKindType.ToString() + ";" + buyingBongType.ToString() + ";" + buyingMinuteType.ToString() + ";" + buyingMinuteLineType.ToString() + ";" + buyingMinuteLineAccessPer.ToString() + ";" + buyingDistance.ToString() + ";" +
                                  buyingStocPeriod1.ToString() + ";" + buyingStocPeriod2.ToString() + ";" + buyingStocPeriod3.ToString() + ";" + buyingBollPeriod.ToString() + ";" + buyingLine3Type.ToString() + ";" + buyingTransferPer2.ToString() + ";" + riseTransferPer2.ToString() + ";" + mesuoption1.ToString() + ";" + nontradingtime.ToString() + ";" + mesuoption2.ToString();

            /////////////////////////////////////////////// 추매 설정 ////////////////////////////////////////////////
            // 추매타입 0:기본추매, 1:이동평균선
            int reBuyingType = addBuyingTypeComboBox.SelectedIndex;
            double[] reBuyingPer = new double[5]; // 추매 %
            int reBuyingMovePriceKindType = 0; // 0:단순 1:지수(이평기준일떄만쓸듯)
            int reBuyingBongType = 0; // 0:월,1:주,2:일,3:분
            int reBuyingMinuteType = 0; // 1,3,5분봉 등등 입력
            int[] reBuyingMinuteLineType = new int[5];// 1,3,5,20이평 등등

            if (reBuyingType < 0)
            {
                MessageBox.Show("추매 방식 선택 리스트를 확인해 주세요.");
                return;
            }
            if (reBuyingType == 0) //기본추매
            {
                if(buyingCountValue > 1)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_2.Text, 0)) return;
                }
                if(buyingCountValue > 2)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_3.Text, 0)) return;
                }
                if (buyingCountValue > 3)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_4.Text, 0)) return;
                }
                if (buyingCountValue > 4)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_5.Text, 0)) return;
                }
                if (buyingCountValue > 5)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_6.Text, 0)) return;
                }
                reBuyingPer[0] = double.Parse(reBuyingType1InvestPricePerTextBox_2.Text);
                reBuyingPer[1] = double.Parse(reBuyingType1InvestPricePerTextBox_3.Text);
                reBuyingPer[2] = double.Parse(reBuyingType1InvestPricePerTextBox_4.Text);
                reBuyingPer[3] = double.Parse(reBuyingType1InvestPricePerTextBox_5.Text);
                reBuyingPer[4] = double.Parse(reBuyingType1InvestPricePerTextBox_6.Text);

            }
            else if(reBuyingType == 1)//이동평균선 추매
            {
                //0: 단순 1:지수
                reBuyingMovePriceKindType = reBuyingType2MoveKindComboBox.SelectedIndex;
                // 월,주,일,분봉(0,1,2,3)
                reBuyingBongType = reBuyingType2DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                reBuyingMinuteType = reBuyingType2MoveBongKindComboBox.SelectedIndex;
                // 1,3,5,20이평 등등
                if (CheckEmpty(reBuyingType2MoveLineTextBox_2.Text, 0)) return;
                if (CheckEmpty(reBuyingType2MoveLineTextBox_3.Text, 0)) return;
                if (CheckEmpty(reBuyingType2MoveLineTextBox_4.Text, 0)) return;
                if (CheckEmpty(reBuyingType2MoveLineTextBox_5.Text, 0)) return;
                if (CheckEmpty(reBuyingType2MoveLineTextBox_6.Text, 0)) return;
                reBuyingMinuteLineType[0] = int.Parse(reBuyingType2MoveLineTextBox_2.Text);
                reBuyingMinuteLineType[1] = int.Parse(reBuyingType2MoveLineTextBox_3.Text);
                reBuyingMinuteLineType[2] = int.Parse(reBuyingType2MoveLineTextBox_4.Text);
                reBuyingMinuteLineType[3] = int.Parse(reBuyingType2MoveLineTextBox_5.Text);
                reBuyingMinuteLineType[4] = int.Parse(reBuyingType2MoveLineTextBox_6.Text);
                // 추매 %
                if (CheckEmpty(reBuyingType2InvestPricePerTextBox_2.Text, 0)) return;
                if (CheckEmpty(reBuyingType2InvestPricePerTextBox_3.Text, 0)) return;
                if (CheckEmpty(reBuyingType2InvestPricePerTextBox_4.Text, 0)) return;
                if (CheckEmpty(reBuyingType2InvestPricePerTextBox_5.Text, 0)) return;
                if (CheckEmpty(reBuyingType2InvestPricePerTextBox_6.Text, 0)) return;
                reBuyingPer[0] = double.Parse(reBuyingType2InvestPricePerTextBox_2.Text);
                reBuyingPer[1] = double.Parse(reBuyingType2InvestPricePerTextBox_3.Text);
                reBuyingPer[2] = double.Parse(reBuyingType2InvestPricePerTextBox_4.Text);
                reBuyingPer[3] = double.Parse(reBuyingType2InvestPricePerTextBox_5.Text);
                reBuyingPer[4] = double.Parse(reBuyingType2InvestPricePerTextBox_6.Text);
            }
            _strReBuying += reBuyingType + ";" + reBuyingPer[0] + ";" + reBuyingPer[1] + ";" + reBuyingPer[2] + ";" + reBuyingPer[3] + ";" + reBuyingPer[4] + ";" +
                                      reBuyingMovePriceKindType + ";" + reBuyingBongType + ";" + reBuyingMinuteType + ";" +
                                      reBuyingMinuteLineType[0] + ";" + reBuyingMinuteLineType[1] + ";" + reBuyingMinuteLineType[2] + ";" + reBuyingMinuteLineType[3] + ";" + reBuyingMinuteLineType[4];

            /////////////////////////////////////////////// 익절 설정 ////////////////////////////////////////////////
            int takeProfitUsing = 1; // 익절 사용 여부
            if (takeProfitCheckBox.Checked) takeProfitUsing = 1;
            else takeProfitUsing = 0;
            int takeProfitCount = (int)takeProfitComboBox.SelectedIndex;// 0: 1차, 1: 2차, 2: 3차, 3: 4차, 4: 5차
            int takeProfitType = takeProfitTypeComboBox.SelectedIndex; // 0:기본익절, 1:이동평균근접, 2: 스토캐스틱SLOW, 3:볼린저밴드, 4:엔벨로프
            double[] takeProfitBuyingPricePer = new double[5]; // 익절 %
            double[] takeProfitProportion = new double[5]; // 익절비중
            // 이동평균 근접, 돌파 공통 사용        
            int takeProfitMovePriceKindType = 0; //0:단순, 1:지수        
            int takeProfitBongType = 0; // 0:월,1:주,2:일,3:분
            int takeProfitMinuteType = 0; // 1,3,5분봉 등등 입력
            int takeProfitMinuteLineType = 0;// 1,3,5,20이평 등등            
            double takeProfitLineAccessPer = 0;// 근접 %
            int takeProfitDistance = 0; // 0:근접, 1:돌파,2:이탈, 0:이상, 1:이하
            Double takeProfitStocPeriod1 = 0; //기간
            Double takeProfitStocPeriod2 = 0; //%K
            Double takeProfitStocPeriod3 = 0; //%D
            double takeProfitBollPeriod = 0; //승수, 엔벨%
            int takeProfitLine3Type = 0; // 0:상한선,1:중심선,2:하한선

            if(takeProfitType < 0)
            {
                { MessageBox.Show("익절 방식 선택 리스트를 확인해 주세요."); return;}
            }
            if(takeProfitType == 0) //기본익절
            {
                takeProfitBuyingPricePer[0] = (double)takeProfitBuyingPricePerNumericUpDown1.Value;
                takeProfitProportion[0] = (double)takeProfitPerNumericUpDown1.Value;

                takeProfitBuyingPricePer[1] = (double)takeProfitBuyingPricePerNumericUpDown2.Value;
                takeProfitProportion[1] = (double)takeProfitPerNumericUpDown2.Value;

                takeProfitBuyingPricePer[2] = (double)takeProfitBuyingPricePerNumericUpDown3.Value;
                takeProfitProportion[2] = (double)takeProfitPerNumericUpDown3.Value;

                takeProfitBuyingPricePer[3] = (double)takeProfitBuyingPricePerNumericUpDown4.Value;
                takeProfitProportion[3] = (double)takeProfitPerNumericUpDown4.Value;

                takeProfitBuyingPricePer[4] = (double)takeProfitBuyingPricePerNumericUpDown5.Value;
                takeProfitProportion[4] = (double)takeProfitPerNumericUpDown5.Value;
            }
            else if(takeProfitType == 1) // 이동평균선 익절
            {
                // 이평종류 0: 단순 , 1: 지수
                takeProfitMovePriceKindType = takeProfitType2MoveKindComboBox.SelectedIndex;
                // 일봉, 분봉(0,1)
                takeProfitBongType = takeProfitType2DayComboBox.SelectedIndex;
                // 1,3,5,10,15,30,45,60 분봉
                takeProfitMinuteType = takeProfitType2MoveBongKindComboBox.SelectedIndex;

                // 1,3,5,20 이평종류
                if (CheckEmpty(takeProfitType2MoveLineTextBox.Text, 1)) return;
                takeProfitMinuteLineType = int.Parse(takeProfitType2MoveLineTextBox.Text);
                // n%                
                if (CheckEmpty(takeProfitType2MoveBongPerTextBox.Text, 1)) return;
                takeProfitLineAccessPer = double.Parse(takeProfitType2MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파
                takeProfitDistance = takeProfitType2DistanceComboBox.SelectedIndex;
            }
            else if (takeProfitType == 2) // 스토캐스틱SLOW
            {
                if (CheckEmpty(takeProfitType3Period1.Text, 1)) return;
                if (CheckEmpty(takeProfitType3Period2.Text, 1)) return;
                if (CheckEmpty(takeProfitType3Period3.Text, 1)) return;
                // 기간
                takeProfitStocPeriod1 = double.Parse(takeProfitType3Period1.Text);
                // %K
                takeProfitStocPeriod2 = double.Parse(takeProfitType3Period2.Text);
                // %D
                takeProfitStocPeriod3 = double.Parse(takeProfitType3Period3.Text);
                // 일봉, 분봉(0,1)
                takeProfitBongType = takeProfitType3DayComboBox.SelectedIndex;
                // 1,3,5,10,15,30,45,60 분봉
                takeProfitMinuteType = takeProfitType3MoveBongKindComboBox.SelectedIndex;
                // k값
                if (CheckEmpty(takeProfitType3StocValueTextBox.Text, 1)) return;
                takeProfitLineAccessPer = double.Parse(takeProfitType3StocValueTextBox.Text);
                // 0:이상, 1:이하
                takeProfitDistance = takeProfitType3DistanceComboBox.SelectedIndex;
            }
            else if (takeProfitType == 3) // 볼린저밴드
            {
                if (CheckEmpty(takeProfitType4Period1.Text, 1)) return;
                if (CheckEmpty(takeProfitType4Period2.Text, 1)) return;
                // 기간
                takeProfitStocPeriod1 = double.Parse(takeProfitType4Period1.Text);
                // 승수
                takeProfitBollPeriod = double.Parse(takeProfitType4Period2.Text);
                // 일봉, 분봉(0,1)
                takeProfitBongType = takeProfitType4DayComboBox.SelectedIndex;
                // 1,3,5,10,15,30,45,60 분봉
                takeProfitMinuteType = takeProfitType4MoveBongKindComboBox.SelectedIndex;
                // 상한선, 중심선, 하한선(0, 1, 2)
                takeProfitLine3Type = takeProfitType4LineComboBox.SelectedIndex;
                // n%
                if (CheckEmpty(takeProfitType4MoveBongPerTextBox.Text, 1)) return;
                takeProfitLineAccessPer = double.Parse(takeProfitType4MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파, 2:이탈
                takeProfitDistance = takeProfitType4DistanceComboBox.SelectedIndex;
            }
            else if (takeProfitType == 4) // 엔벨로프
            {
                if (CheckEmpty(takeProfitType5Period1.Text, 1)) return;
                if (CheckEmpty(takeProfitType5Period2.Text, 1)) return;
                // 기간
                takeProfitStocPeriod1 = double.Parse(takeProfitType5Period1.Text);
                // 승수
                takeProfitBollPeriod = double.Parse(takeProfitType5Period2.Text);
                // 일봉, 분봉(0,1)
                takeProfitBongType = takeProfitType5DayComboBox.SelectedIndex;
                // 1,3,5,10,15,30,45,60 분봉
                takeProfitMinuteType = takeProfitType5MoveBongKindComboBox.SelectedIndex;
                // 저항선, 중심선, 지지선(0, 1, 2)
                takeProfitLine3Type = takeProfitType5LineComboBox.SelectedIndex;
                // n%
                if (CheckEmpty(takeProfitType5MoveBongPerTextBox.Text, 1)) return;
                takeProfitLineAccessPer = double.Parse(takeProfitType5MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파, 2:이탈
                takeProfitDistance = takeProfitType5DistanceComboBox.SelectedIndex;
            }
            _strTakeProfit += takeProfitUsing.ToString() + ";" + takeProfitType.ToString() + ";" + takeProfitCount.ToString() + ";" +
                            takeProfitBuyingPricePer[0].ToString() + ";" + takeProfitProportion[0].ToString() + ";" + takeProfitBuyingPricePer[1].ToString() + ";" + takeProfitProportion[1].ToString() + ";" + takeProfitBuyingPricePer[2].ToString() + ";" + takeProfitProportion[2].ToString() + ";" + takeProfitBuyingPricePer[3].ToString() + ";" + takeProfitProportion[3].ToString() + ";" + takeProfitBuyingPricePer[4].ToString() + ";" + takeProfitProportion[4].ToString() + ";" +
                            takeProfitMovePriceKindType.ToString() + ";" + takeProfitBongType.ToString() + ";" + takeProfitMinuteType.ToString() + ";" + takeProfitMinuteLineType.ToString() + ";" + takeProfitLineAccessPer.ToString() + ";" + takeProfitDistance.ToString() + ";" +
                            takeProfitStocPeriod1.ToString() + ";" + takeProfitStocPeriod2.ToString() + ";" + takeProfitStocPeriod3.ToString() + ";" + takeProfitBollPeriod.ToString() + ";" + takeProfitLine3Type.ToString(); ;
            
            /////////////////////////////////////////////// 손절 설정 ////////////////////////////////////////////////
            int stopLossUsing = 1; // 손절 사용 여부
            if (stopLossCheckBox.Checked) stopLossUsing = 1;
            else stopLossUsing = 0;
            int stopLossCount = (int)stopLossComboBox.SelectedIndex;// 0: 1차, 1: 2차, 2: 3차, 3: 4차, 4: 5차
            int stopLossType = stopLossTypeComboBox.SelectedIndex; // 0:기본손절,1:이동평균 근접 2:볼린저밴드. 3:엔벨로프
            double[] stopLossBuyingPricePer = new double[5]; // 손절 %
            double[] stopLossProportion = new double[5]; // 손절비중
            // 이동평균 근접, 이탈 공통 사용        
            int stopLossMovePriceKindType = 0; //0:단수, 1:지수        
            int stopLossBongType = 0; // 0:월,1:주,2:일,3:분
            int stopLossMinuteType = 0; // 1,3,5분봉 등등 입력
            int stopLossMinuteLineType = 0;// 1,3,5,20이평 등등            
            double stopLossLineAccessPer = 0;// 근접 %
            int stopLossDistance = 0; // 0:근접, 1:돌파,2:이탈, 0:이상, 1:이하
            double stopLossStocPeriod1 = 0; //기간
            double stopLossStocPeriod2 = 0; //%K
            double stopLossStocPeriod3 = 0; //%D
            double stopLossBollPeriod = 0; //승수, 엔벨%
            int stopLossLine3Type = 0; // 0:상한선,1:중심선,2:하한선
            
            if (stopLossType < 0)
            {
                MessageBox.Show("손절 방식 선택 리스트를 확인해 주세요.");
                return;
            }

            if (stopLossType == 0) // 기본 손절
            {
                stopLossBuyingPricePer[0] = (double)stopLossBuyingPricePerNumericUpDown1.Value;
                stopLossProportion[0]= (double)stopLossPerNumericUpDown1.Value;

                stopLossBuyingPricePer[1] = (double)stopLossBuyingPricePerNumericUpDown2.Value;
                stopLossProportion[1] = (double)stopLossPerNumericUpDown2.Value;

                stopLossBuyingPricePer[2] = (double)stopLossBuyingPricePerNumericUpDown3.Value;
                stopLossProportion[2] = (double)stopLossPerNumericUpDown3.Value;

                stopLossBuyingPricePer[3] = (double)stopLossBuyingPricePerNumericUpDown4.Value;
                stopLossProportion[3] = (double)stopLossPerNumericUpDown4.Value;

                stopLossBuyingPricePer[4] = (double)stopLossBuyingPricePerNumericUpDown5.Value;
                stopLossProportion[4] = (double)stopLossPerNumericUpDown5.Value;
            }
            else if (stopLossType == 1) // 이동평균선 손절
            {
                // 이평종류 0:단순, 1:지수                
                stopLossMovePriceKindType = stopLossType2MoveKindComboBox.SelectedIndex;
                // 월,주,일,분봉(0,1,2,3)
                stopLossBongType = stopLossType2DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                stopLossMinuteType = stopLossType2MoveBongKindComboBox.SelectedIndex;
                // 1,3,5,20 이평종류
                if (CheckEmpty(stopLossType2MoveLineTextBox.Text, 2)) return;
                stopLossMinuteLineType = int.Parse(stopLossType2MoveLineTextBox.Text);
                // n%
                if (CheckEmpty(stopLossType2MoveBongPerTextBox.Text, 2)) return;
                stopLossLineAccessPer = double.Parse(stopLossType2MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파
                stopLossDistance = stopLossType2DistanceComboBox.SelectedIndex;
            }
            else if (stopLossType == 2) // 볼린저벤드
            {
                if (CheckEmpty(stopLossType3Period1.Text, 2)) return;
                if (CheckEmpty(stopLossType3Period2.Text, 2)) return;
                // 기간
                stopLossStocPeriod1 = double.Parse(stopLossType3Period1.Text);
                // 승수
                stopLossBollPeriod = double.Parse(stopLossType3Period2.Text);
                // 월,주,일,분봉(0,1,2,3)
                stopLossBongType = stopLossType3DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                stopLossMinuteType = stopLossType3MoveBongKindComboBox.SelectedIndex;
                // 상한선, 중심선, 하한선(0, 1, 2)
                stopLossLine3Type = stopLossType3LineComboBox.SelectedIndex;
                // n%
                if (CheckEmpty(stopLossType3MoveBongPerTextBox.Text, 2)) return;
                stopLossLineAccessPer = double.Parse(stopLossType3MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파, 2:이탈
                stopLossDistance = stopLossType3DistanceComboBox.SelectedIndex;
            }
            else if (stopLossType == 3) // 엔벨로프
            {
                if (CheckEmpty(stopLossType4Period1.Text, 2)) return;
                if (CheckEmpty(stopLossType4Period2.Text, 2)) return;
                // 기간
                stopLossStocPeriod1 = double.Parse(stopLossType4Period1.Text);
                // 승수
                stopLossBollPeriod = double.Parse(stopLossType4Period2.Text);
                // 월,주,일,분봉(0,1,2,3)
                stopLossBongType = stopLossType4DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                stopLossMinuteType = stopLossType4MoveBongKindComboBox.SelectedIndex;
                // 상한선, 중심선, 하한선(0, 1, 2)
                stopLossLine3Type = stopLossType4LineComboBox.SelectedIndex;
                // n%
                if (CheckEmpty(stopLossType4MoveBongPerTextBox.Text, 2)) return;
                stopLossLineAccessPer = double.Parse(stopLossType4MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파, 2:이탈
                stopLossDistance = stopLossType4DistanceComboBox.SelectedIndex;
            }

            _strStopLoss += stopLossUsing.ToString() + ";" + stopLossType.ToString() + ";" + stopLossCount.ToString() + ";" +
                            stopLossBuyingPricePer[0].ToString() + ";" + stopLossProportion[0].ToString() + ";" + stopLossBuyingPricePer[1].ToString() + ";" + stopLossProportion[1].ToString() + ";" + stopLossBuyingPricePer[2].ToString() + ";" + stopLossProportion[2].ToString() + ";" + stopLossBuyingPricePer[3].ToString() + ";" + stopLossProportion[3].ToString() + ";" + stopLossBuyingPricePer[4].ToString() + ";" + stopLossProportion[4].ToString() + ";" +
                            stopLossMovePriceKindType.ToString() + ";" + stopLossBongType.ToString() + ";" + stopLossMinuteType.ToString() + ";" + stopLossMinuteLineType.ToString() + ";" + stopLossLineAccessPer.ToString() + ";" + stopLossDistance.ToString() + ";" +
                            stopLossStocPeriod1.ToString() + ";" + stopLossStocPeriod2.ToString() + ";" + stopLossStocPeriod3.ToString() + ";" + stopLossBollPeriod.ToString() + ";" + stopLossLine3Type.ToString();

            /////////////////////////////////////////////// ts매도 설정 ////////////////////////////////////////////////
            int tsmedoUsing = 1; // ts매도 사용 여부
            if (tsMedoCheckBox.Checked) tsmedoUsing = 1;
            else tsmedoUsing = 0;
            int tsmedoCount = (int)tsmedocomboBox.SelectedIndex; // 0: 1차 , 1: 2차, 2: 3차
            int[] tsmedoUsingType = new int[3];
            double[] tsmedoAchievedPer = new double[3];
            double[] tsmedoPercent = new double[3];
            double[] tsmedoProportion = new double[3];

            int tsProfitPreservation1 = 1; // 1차ts 이익구간사용여부
            if (tsProfitPreservationCheckBox1.Checked) tsProfitPreservation1 = 1;
            else tsProfitPreservation1 = 0;

            int tsProfitPreservation2 = 1; // 2차ts 이익구긴사용여부
            if (tsProfitPreservationCheckBox2.Checked) tsProfitPreservation2 = 1;
            else tsProfitPreservation2 = 0;

            int tsProfitPreservation3 = 1; // 3차ts 이익구간사용여부
            if (tsProfitPreservationCheckBox3.Checked) tsProfitPreservation3 = 1;
            else tsProfitPreservation3 = 0;

            tsmedoUsingType[0] = tsComboBox1.SelectedIndex;
            tsmedoAchievedPer[0] = (double)tsnumericUpDown1.Value;
            tsmedoPercent[0] = (double)tsMedonumericUpDown1.Value;
            tsmedoProportion[0] = (double)tspernumericUpDown1.Value;

            tsmedoUsingType[1] = tsComboBox2.SelectedIndex;
            tsmedoAchievedPer[1] = (double)tsnumericUpDown2.Value;
            tsmedoPercent[1] = (double)tsMedonumericUpDown2.Value;
            tsmedoProportion[1] = (double)tspernumericUpDown2.Value;

            tsmedoUsingType[2] = tsComboBox3.SelectedIndex;
            tsmedoAchievedPer[2] = (double)tsnumericUpDown3.Value;
            tsmedoPercent[2] = (double)tsMedonumericUpDown3.Value;
            tsmedoProportion[2] = (double)tspernumericUpDown3.Value;



            _strTsmedo += tsmedoUsing.ToString() + ";" + tsmedoCount.ToString() + ";"
                + tsmedoUsingType[0].ToString() + ";" + tsmedoAchievedPer[0] + ";" + tsmedoPercent[0] + ";" + tsmedoProportion[0] + ";"
                +tsmedoUsingType[1].ToString() + ";" + tsmedoAchievedPer[1] + ";" + tsmedoPercent[1] + ";" + tsmedoProportion[1] + ";"
                +tsmedoUsingType[2].ToString() + ";" + tsmedoAchievedPer[2] + ";" + tsmedoPercent[2] + ";" + tsmedoProportion[2] + ";" + tsProfitPreservation1.ToString() + ";" + tsProfitPreservation2.ToString() + ";" + tsProfitPreservation3.ToString();

            if (type == 0) // 0 저장하기
            {
                gMainForm.gFileIOInstance.saveConditionList(conditionIndex, _strInvestment, _strInvestmentPrice, _strBuying, _strReBuying, _strTakeProfit, _strStopLoss, _strTsmedo);
            }
            else if (type == 1)// 1 수정하기
            {
                gMainForm.gFileIOInstance.editConditionList(editnumber, conditionIndex, _strInvestment, _strInvestmentPrice, _strBuying, _strReBuying, _strTakeProfit, _strStopLoss, _strTsmedo);
                // 그리듀에 조건식이 있으면 같이 수정을 한다.
                MyTradingCondition _mtc = new MyTradingCondition(conditionIndex, _strInvestment, _strInvestmentPrice, _strBuying, _strReBuying, _strTakeProfit, _strStopLoss, "", _strTsmedo);
                // 생성된 MyTradingCondition을 gMyTradingConditionList에 추가한다.
                ///////////////////////////////////////// 조건식 그리드뷰에 등록이 되어 있으면 같이 수정을 한다.
                int indexCount = 0;
                foreach (DataGridViewRow row in gMainForm.tradingConditionDataGridView.Rows)
                {
                    if (row == null)
                        continue;

                    if (row.Cells["매매조건식_조건식"].Value.ToString().Equals(conditionName))
                    {
                        gMainForm.reSetAddMyTradingConditionToDataGridView(_mtc, indexCount);
                        break;
                    }
                    indexCount++;
                }
                ///////////////////////////////////////// 조건식데이타리스트도 수정을 한다.
                foreach (MyTradingCondition mtc in gMainForm.gMyTradingConditionList)
                {
                    if (mtc == null)
                        continue;

                    string _conditionName2 = mtc.conditionData.conditionName;

                    if (_conditionName2.Equals(conditionName))
                    {
                        mtc.reSetMyTradingCondition(conditionIndex, _strInvestment, _strInvestmentPrice, _strBuying, _strReBuying, _strTakeProfit, _strStopLoss, _strTsmedo); ;
                        int _itemCount = mtc.MyTradingItemList.Count; // 현재 편입 종목수
                        mtc.buyingItemCount = _buyingItemCount; // 총 매수 개수
                        mtc.remainingTransferItemCount = mtc.remainingTransferItemCount - _itemCount; // 남은 편입 종목수

                        foreach (DataGridViewRow row in gMainForm.tradingConditionDataGridView.Rows) // 남은 매수 개수 카운트
                        {
                            string _conditionName = row.Cells["매매조건식_조건식"].Value.ToString();
                            int _count = 0;
                            foreach (DataGridViewRow item in gMainForm.tradingItemDataGridView.Rows)
                            {
                                string _itemConditionName = item.Cells["매매진행_조건식"].Value.ToString();

                                if (_conditionName.Equals(_itemConditionName) && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("매도완료") && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("매도중") && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("대기중"))
                                {
                                    _count++;
                                }
                            }
                            mtc.remainingBuyingItemCount = mtc.remainingBuyingItemCount - _count;
                            //Console.WriteLine("수정하기 이후 남은 매수개수: " + mtc.remainingBuyingItemCount);
                            if (mtc.remainingBuyingItemCount < 0)
                                mtc.remainingBuyingItemCount = 0;
                        }
                        if (mtc.remainingTransferItemCount < 0)
                            mtc.remainingTransferItemCount = 0;
                        break;
                    }
                }
                ///////////////////////////////////////// 수정된 조건식을 사용하는 종목데이타리스트도 수정을 한다.
                foreach (MyTradingCondition mtc in gMainForm.gMyTradingConditionList) // 조건식데이타 검색
                {
                    foreach (MyTradingItem mti in mtc.MyTradingItemList)
                    {
                        if (mti != null && mti.m_conditionName.Equals(conditionName)) // 매매중인 종목을 찾은 경우...
                        {
                            mti.reSetConditionData(_mtc);
                            mti.CalculateIndicator();
                            gMainForm.gTradingManager.DrawIndicateData(gMainForm.g_selectItemCode);
                        }
                    }
                }

                // 조건식그리드뷰에 현재 매매되고 있는 종목갯수저장
                foreach (DataGridViewRow row in gMainForm.tradingConditionDataGridView.Rows)
                {
                    string _conditionName = row.Cells["매매조건식_조건식"].Value.ToString();
                    int _count = 0;
                    foreach (DataGridViewRow item in gMainForm.tradingItemDataGridView.Rows)
                    {
                        string _itemConditionName = item.Cells["매매진행_조건식"].Value.ToString();

                        if (_conditionName.Equals(_itemConditionName) && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("매도완료") && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("매도중") && !item.Cells["매매진행_진행상황"].Value.ToString().Equals("대기중"))
                        {
                            _count++;
                        }
                    }
                    row.Cells["매매조건식_실매수종목수"].Value = _count;
                }

                MessageBox.Show("수정되었습니다.");
                gMainForm.setLogText(conditionName  + " 설정 수정완료.");
            }
            else if (type == 2) // 2 등록하기
            {
                string screenNumber = gMainForm.GetConditionScreenNumber();
                gMainForm.setConditionListCreateCheck(int.Parse(screenNumber) - (int)ConditionNumber.ConditionStartNum, true);
                MyTradingCondition _mtc = new MyTradingCondition(conditionIndex, _strInvestment, _strInvestmentPrice, _strBuying, _strReBuying, _strTakeProfit, _strStopLoss, screenNumber,_strTsmedo);
                // 생성된 MyTradingCondition을 gMyTradingConditionList에 추가한다.
                gMainForm.gMyTradingConditionList.Add(_mtc);
                // 데이타그리드뷰에 출력하기
                gMainForm.AddmyTradingConditionToDataGridView(_mtc);
            }
        }

        public bool CheckEmpty(string txt, int type)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                if (type == 0) MessageBox.Show("추매 방식 - 빈 입력창이 있습니다.", "추매 방식", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (type == 1) MessageBox.Show("익절 방식 - 빈 입력창이 있습니다.", "익절 방식", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (type == 2) MessageBox.Show("손절 방식 - 빈 입력창이 있습니다.", "익절 방식", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            return false;
        }
        private void TextBoxTextChangedEvent(object sender, EventArgs e)
        {
            //TextBox가 공백이면 계산하지 않고 return
            if (investmentPerItemTextBox.Text == "" || buyingItemCountTextBox.Text == "")
                return;
            //종목당 투자금액
            int _itemInvestment = int.Parse(investmentPerItemTextBox.Text.Replace(",", ""));
            //매수 종목수
            int _buyingItemCount = int.Parse(buyingItemCountTextBox.Text);
            //총 투자금
            int _totalInvestment = _itemInvestment * _buyingItemCount;

            totalInvestmentTextbox.Text = _totalInvestment.ToString("N0"); // N0  소숫점이 없고, 3자리마다 ,(콤마) 표시한다.

            if(sender.Equals(investmentPerItemTextBox))
            {
                investmentPerItemTextBox.Text = _itemInvestment.ToString("N0");
                investmentPerItemTextBox.Select(investmentPerItemTextBox.Text.Length, 0);
            }
        }
        //조건식 등록하기 메서드
        private void ConditionRegister()
        {
            // 계좌번호
            string _account = gMainForm.myAccountComboBox.Text;
            // 조건식이름, 조건식번호
            ConditionData _conditionData = null; //매수조건식
            string _conditionName = conditionComboBox.Text; //조건식 콤보박스
            //조건식 저장리스트에서 같은 이름의 조건식을 찾아낸다
            _conditionData = gMainForm.conditionDataList.Find(o => o.conditionName.Equals(_conditionName));
            //종목당 투자금액
            if(investmentPerItemTextBox.Text == "")
            {
                MessageBox.Show("총 투자금을 설정해 주세요.");
                return;
            }
            double _itemInvestment = double.Parse(investmentPerItemTextBox.Text);
            if(_itemInvestment <=0)
            {
                MessageBox.Show("총 투자금을 설정해 주세요.");
                return;
            }
            // 매수 종목수
            if (buyingItemCountTextBox.Text =="")
            {
                MessageBox.Show("매수 종목수를 설정해 주세요.");
                return;
            }
            if (transferItemCountTextBox.Text == "")
            {
                MessageBox.Show("편입 종목수를 설정해 주세요.");
                return;
            }
            //매수 종목수가 0이거나 작으면
            int _buyingItemCount = int.Parse(buyingItemCountTextBox.Text);
            if (_buyingItemCount <= 0)
            {
                MessageBox.Show("매수 종목수를 설정해 주세요.");
                return;
            }
            int _transferItemCount = int.Parse(transferItemCountTextBox.Text);
            if (_transferItemCount <= 0)
            {
                MessageBox.Show("편입 종목수를 설정해 주세요.");
                return;
            }

            // 데이터그리드뷰의 조건식이름과 현재 선택된 조건식이름을 비교하여 같으면 메시지를 출력해준다.
            foreach (DataGridViewRow row in gMainForm.tradingConditionDataGridView.Rows)
            {
                if(row.Cells["매매조건식_조건식"].Value !=null)
                {
                    if(row.Cells["매매조건식_조건식"].Value.ToString().Equals(_conditionName))
                    {
                        MessageBox.Show("동일한 조건식이 등록되어 있습니다.");
                        return;
                    }
                }
            }
            saveCondition(2, -1);

            /*
            // 매수타입(true:편입즉시매수, false: 편입가격대비 -%매수)
            bool _buyingType = true;
            double _transferPricePercent = 0.0;
            if(buyingType1TransferPricePerRadioButton.Checked) // 편입가격대비로 라디오버튼이 선택되어 있으면
            {
                _buyingType = false;
                _transferPricePercent = Convert.ToDouble(buyingType1TransferPriceNumericUpDown.Value); // 편입가격대비 -%
            }
            //익절
            double _takeProfitBuyingPricePercent = Convert.ToDouble(takeProfitBuyingPricePerNumericUpDown.Value);
            //손절
            double _stopLossBuyingPricePercent = Convert.ToDouble(stopLossBuyingPricePerNumericUpDown.Value);
            // 추매사용
            bool _rePurchase = false;
            if (rePurchaseCheckBox.Checked)
                _rePurchase = true;
            // 추매금액
            string _rePurchaseMoney = "";
            _rePurchaseMoney += rePurchaseMoneyTextBox1.Text + ";";
            _rePurchaseMoney += rePurchaseMoneyTextBox2.Text + ";";
            _rePurchaseMoney += rePurchaseMoneyTextBox3.Text + ";";
            _rePurchaseMoney += rePurchaseMoneyTextBox4.Text + ";";
            _rePurchaseMoney += rePurchaseMoneyTextBox5.Text + ";";
            _rePurchaseMoney += rePurchaseMoneyTextBox6.Text + ";";
            // 추매매수가격대비비율
            string _rePurchaseRate = "";
            _rePurchaseRate += rePurchaseRateNumericUpDown1.Text + ";";
            _rePurchaseRate += rePurchaseRateNumericUpDown2.Text + ";";
            _rePurchaseRate += rePurchaseRateNumericUpDown3.Text + ";";
            _rePurchaseRate += rePurchaseRateNumericUpDown4.Text + ";";
            _rePurchaseRate += rePurchaseRateNumericUpDown5.Text + ";";
            _rePurchaseRate += rePurchaseRateNumericUpDown6.Text + ";";

            //ts매도사용
            bool _tsMedo = false;
            if (tsMedoCheckBox.Checked)
                _tsMedo = true;
            //ts매도 %
            double _tsMedoPercent = Convert.ToDouble(tsMedonumericUpDown.Value);

            //화면번호
            //현재 조건식이 5개 이상이면 더이상 등록할 수 없다.
            if (!gMainForm.GetConditionListCreateCheck()) // GetConditionListCreateCheck == false라면
            {
                MessageBox.Show("5개 이상 조건식을 등록할 수 없습니다.");
                return;
            }
            string _screenNumber = gMainForm.GetConditionScreenNumber(); // 스크린번호를 받아온다.
            // bConditionSNUserorNot 변수를 셋팅한다.
            gMainForm.setConditionListCreateCheck(int.Parse(_screenNumber) - (int)ConditionNumber.ConditionStartNum, true);
            // MyTradingCondition 인스턴스 생성
            MyTradingCondition _mtc = new MyTradingCondition(_conditionData, _itemInvestment, _buyingItemCount,
                                                            _buyingType, _transferPricePercent, _takeProfitBuyingPricePercent, _stopLossBuyingPricePercent, 
                                                            _rePurchase,_rePurchaseMoney,_rePurchaseRate,_screenNumber,_tsMedo,_tsMedoPercent);

            //생성된 MyTradingCondition을 gMyTradingConditionList에 추가한다.
            gMainForm.gMyTradingConditionList.Add(_mtc);
            //데이터그리드뷰에 출력하기
            gMainForm.AddmyTradingConditionToDataGridView(_mtc);
            */
        }
        private void tsComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ComboBox에서 선택된 값이 "고점"이라면
            if (tsComboBox1.Text.ToString() == "고점")
            {
                // NumericUpDown1을 보이지 않게 설정
                tsnumericUpDown1.Visible = false;
            }
            else
            {
                // 다른 경우에는 NumericUpDown1을 보이게 설정
                tsnumericUpDown1.Visible = true;
            }
        }
        private void tsComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ComboBox에서 선택된 값이 "고점"이라면
            if (tsComboBox2.Text.ToString() == "고점")
            {
                // NumericUpDown1을 보이지 않게 설정
                tsnumericUpDown2.Visible = false;
            }
            else
            {
                // 다른 경우에는 NumericUpDown1을 보이게 설정
                tsnumericUpDown2.Visible = true;
            }
        }
        private void tsComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ComboBox에서 선택된 값이 "고점"이라면
            if (tsComboBox3.Text.ToString() == "고점")
            {
                // NumericUpDown1을 보이지 않게 설정
                tsnumericUpDown3.Visible = false;
                Console.WriteLine(tsComboBox3.SelectedIndex);
            }
            else
            {
                // 다른 경우에는 NumericUpDown1을 보이게 설정
                tsnumericUpDown3.Visible = true;
                Console.WriteLine(tsComboBox3.SelectedIndex);
            }
        }
        private void tsmedocomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _tsmedoCount = tsmedocomboBox.SelectedIndex;

            if (_tsmedoCount == 0)
            {
                tsComboBox1.Enabled = true;
                tsComboBox1.BackColor = Color.FromArgb(255, 255, 255);

                tsnumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tsMedonumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tspernumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tsProfitPreservationCheckBox1.Enabled = true;
                tsProfitPreservationCheckBox1.BackColor = Color.FromArgb(255, 255, 255);

                tsComboBox2.Enabled = false;
                tsComboBox2.BackColor = Color.FromArgb(240, 240, 240);

                tsnumericUpDown2.Enabled = false;
                tsnumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);

                tsMedonumericUpDown2.Enabled = false;
                tsMedonumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);

                tspernumericUpDown2.Enabled = false;
                tspernumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);
                tspernumericUpDown2.Value = 0;

                tsProfitPreservationCheckBox2.Enabled = false;
                tsProfitPreservationCheckBox2.BackColor = Color.FromArgb(240, 240, 240);

                tsComboBox3.Enabled = false;
                tsComboBox3.BackColor = Color.FromArgb(240, 240, 240);

                tsnumericUpDown3.Enabled = false;
                tsnumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                tsMedonumericUpDown3.Enabled = false;
                tsMedonumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                tspernumericUpDown3.Enabled = false;
                tspernumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);
                tspernumericUpDown3.Value = 0;

                tsProfitPreservationCheckBox3.Enabled = false;
                tsProfitPreservationCheckBox3.BackColor = Color.FromArgb(240, 240, 240);
            }

            if (_tsmedoCount == 1)
            {
                tsComboBox1.Enabled = true;
                tsComboBox1.BackColor = Color.FromArgb(255, 255, 255);

                tsnumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tsMedonumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tspernumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tsProfitPreservationCheckBox1.Enabled = true;
                tsProfitPreservationCheckBox1.BackColor = Color.FromArgb(255, 255, 255);

                tsComboBox2.Enabled = true;
                tsComboBox2.BackColor = Color.FromArgb(255, 255, 255);

                tsnumericUpDown2.Enabled = true;
                tsnumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                tsMedonumericUpDown2.Enabled = true;
                tsMedonumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                tspernumericUpDown2.Enabled = true;
                tspernumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                tsProfitPreservationCheckBox2.Enabled = true;
                tsProfitPreservationCheckBox2.BackColor = Color.FromArgb(255, 255, 255);

                tsComboBox3.Enabled = false;
                tsComboBox3.BackColor = Color.FromArgb(240, 240, 240);

                tsnumericUpDown3.Enabled = false;
                tsnumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                tsMedonumericUpDown3.Enabled = false;
                tsMedonumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                tspernumericUpDown3.Enabled = false;
                tspernumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);
                tspernumericUpDown3.Value = 0;

                tsProfitPreservationCheckBox3.Enabled = false;
                tsProfitPreservationCheckBox3.BackColor = Color.FromArgb(240, 240, 240);
            }

            if (_tsmedoCount == 2)
            {
                tsComboBox1.Enabled = true;
                tsComboBox1.BackColor = Color.FromArgb(255, 255, 255);

                tsnumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tsMedonumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tspernumericUpDown1.Enabled = true;
                tsnumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                tsProfitPreservationCheckBox1.Enabled = true;
                tsProfitPreservationCheckBox1.BackColor = Color.FromArgb(255, 255, 255);

                tsComboBox2.Enabled = true;
                tsComboBox2.BackColor = Color.FromArgb(255, 255, 255);

                tsnumericUpDown2.Enabled = true;
                tsnumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                tsMedonumericUpDown2.Enabled = true;
                tsMedonumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                tspernumericUpDown2.Enabled = true;
                tspernumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                tsProfitPreservationCheckBox2.Enabled = true;
                tsProfitPreservationCheckBox2.BackColor = Color.FromArgb(255, 255, 255);

                tsComboBox3.Enabled = true;
                tsComboBox3.BackColor = Color.FromArgb(255, 255, 255);

                tsnumericUpDown3.Enabled = true;
                tsnumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                tsMedonumericUpDown3.Enabled = true;
                tsMedonumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                tspernumericUpDown3.Enabled = true;
                tspernumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                tsProfitPreservationCheckBox3.Enabled = true;
                tsProfitPreservationCheckBox3.BackColor = Color.FromArgb(255, 255, 255);
            }

        }
        private void mesuoption1comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _mesuoption1 = mesuoption1comboBox.SelectedIndex;

            if (_mesuoption1 == 0)
            {
                nontradingtimeNumericUpDown.Enabled = false;
                nontradingtimeNumericUpDown.BackColor = Color.FromArgb(240, 240, 240);
                nontradingtimeNumericUpDown.Value = 5;

                mesuoption2comboBox.Enabled = false;
                mesuoption2comboBox.BackColor = Color.FromArgb(240, 240, 240);
                mesuoption2comboBox.SelectedIndex = 0;
            }
            if (_mesuoption1 == 1)
            {
                nontradingtimeNumericUpDown.Enabled = true;
                nontradingtimeNumericUpDown.BackColor = Color.FromArgb(255, 255, 255);

                mesuoption2comboBox.Enabled = true;
                mesuoption2comboBox.BackColor = Color.FromArgb(255, 255, 255);
            }
        }
        private void takeProfitcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _takeProfitCount = takeProfitComboBox.SelectedIndex;

            if (_takeProfitCount == 0)
            {
                takeProfitBuyingPricePerNumericUpDown1.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown1.Enabled = true;
                takeProfitPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown2.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown2.Enabled = false;
                takeProfitPerNumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown2.Value = 0;

                takeProfitBuyingPricePerNumericUpDown3.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown3.Enabled = false;
                takeProfitPerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown3.Value = 0;

                takeProfitBuyingPricePerNumericUpDown4.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown4.Enabled = false;
                takeProfitPerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown4.Value = 0;

                takeProfitBuyingPricePerNumericUpDown5.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown5.Enabled = false;
                takeProfitPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown5.Value = 0;
            }

            if (_takeProfitCount == 1)
            {
                takeProfitBuyingPricePerNumericUpDown1.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown1.Enabled = true;
                takeProfitPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown2.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown2.Enabled = true;
                takeProfitPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown3.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown3.Enabled = false;
                takeProfitPerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown3.Value = 0;

                takeProfitBuyingPricePerNumericUpDown4.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown4.Enabled = false;
                takeProfitPerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown4.Value = 0;

                takeProfitBuyingPricePerNumericUpDown5.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown5.Enabled = false;
                takeProfitPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown5.Value = 0;
            }

            if (_takeProfitCount == 2)
            {
                takeProfitBuyingPricePerNumericUpDown1.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown1.Enabled = true;
                takeProfitPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown2.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown2.Enabled = true;
                takeProfitPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown3.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown3.Enabled = true;
                takeProfitPerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown4.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown4.Enabled = false;
                takeProfitPerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown4.Value = 0;

                takeProfitBuyingPricePerNumericUpDown5.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown5.Enabled = false;
                takeProfitPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown5.Value = 0;
            }

            if (_takeProfitCount == 3)
            {
                takeProfitBuyingPricePerNumericUpDown1.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown1.Enabled = true;
                takeProfitPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown2.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown2.Enabled = true;
                takeProfitPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown3.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown3.Enabled = true;
                takeProfitPerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown4.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown4.Enabled = true;
                takeProfitPerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown5.Enabled = false;
                takeProfitBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                takeProfitPerNumericUpDown5.Enabled = false;
                takeProfitPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                takeProfitPerNumericUpDown5.Value = 0;
            }

            if (_takeProfitCount == 4)
            {
                takeProfitBuyingPricePerNumericUpDown1.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown1.Enabled = true;
                takeProfitPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown2.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown2.Enabled = true;
                takeProfitPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown3.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown3.Enabled = true;
                takeProfitPerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown4.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown4.Enabled = true;
                takeProfitPerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitBuyingPricePerNumericUpDown5.Enabled = true;
                takeProfitBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(255, 255, 255);

                takeProfitPerNumericUpDown5.Enabled = true;
                takeProfitPerNumericUpDown5.BackColor = Color.FromArgb(255, 255, 255);
            }
        }
        private void stopLosscomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _stopLossCount = stopLossComboBox.SelectedIndex;

            if (_stopLossCount == 0)
            {
                stopLossBuyingPricePerNumericUpDown1.Enabled = true;
                stopLossBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown1.Enabled = true;
                stopLossPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown2.Enabled = false;
                stopLossBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown2.Enabled = false;
                stopLossPerNumericUpDown2.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown2.Value = 0;

                stopLossBuyingPricePerNumericUpDown3.Enabled = false;
                stopLossBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown3.Enabled = false;
                stopLossPerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown3.Value = 0;

                stopLossBuyingPricePerNumericUpDown4.Enabled = false;
                stopLossBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown4.Enabled = false;
                stopLossPerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown4.Value = 0;

                stopLossBuyingPricePerNumericUpDown5.Enabled = false;
                stopLossBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown5.Enabled = false;
                stopLossPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown5.Value = 0;
            }
            if (_stopLossCount == 1)
            {
                stopLossBuyingPricePerNumericUpDown1.Enabled = true;
                stopLossBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown1.Enabled = true;
                stopLossPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown2.Enabled = true;
                stopLossBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown2.Enabled = true;
                stopLossPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown3.Enabled = false;
                stopLossBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown3.Enabled = false;
                stopLossPerNumericUpDown3.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown3.Value = 0;

                stopLossBuyingPricePerNumericUpDown4.Enabled = false;
                stopLossBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown4.Enabled = false;
                stopLossPerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown4.Value = 0;

                stopLossBuyingPricePerNumericUpDown5.Enabled = false;
                stopLossBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown5.Enabled = false;
                stopLossPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown5.Value = 0;
            }
            if (_stopLossCount == 2)
            {
                stopLossBuyingPricePerNumericUpDown1.Enabled = true;
                stopLossBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown1.Enabled = true;
                stopLossPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown2.Enabled = true;
                stopLossBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown2.Enabled = true;
                stopLossPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown3.Enabled = true;
                stopLossBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown3.Enabled = true;
                stopLossPerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown4.Enabled = false;
                stopLossBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown4.Enabled = false;
                stopLossPerNumericUpDown4.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown4.Value = 0;

                stopLossBuyingPricePerNumericUpDown5.Enabled = false;
                stopLossBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown5.Enabled = false;
                stopLossPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown5.Value = 0;
            }
            if (_stopLossCount == 3)
            {
                stopLossBuyingPricePerNumericUpDown1.Enabled = true;
                stopLossBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown1.Enabled = true;
                stopLossPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown2.Enabled = true;
                stopLossBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown2.Enabled = true;
                stopLossPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown3.Enabled = true;
                stopLossBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown3.Enabled = true;
                stopLossPerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown4.Enabled = true;
                stopLossBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown4.Enabled = true;
                stopLossPerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown5.Enabled = false;
                stopLossBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);

                stopLossPerNumericUpDown5.Enabled = false;
                stopLossPerNumericUpDown5.BackColor = Color.FromArgb(240, 240, 240);
                stopLossPerNumericUpDown5.Value = 0;
            }
            if (_stopLossCount == 4)
            {
                stopLossBuyingPricePerNumericUpDown1.Enabled = true;
                stopLossBuyingPricePerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown1.Enabled = true;
                stopLossPerNumericUpDown1.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown2.Enabled = true;
                stopLossBuyingPricePerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown2.Enabled = true;
                stopLossPerNumericUpDown2.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown3.Enabled = true;
                stopLossBuyingPricePerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown3.Enabled = true;
                stopLossPerNumericUpDown3.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown4.Enabled = true;
                stopLossBuyingPricePerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown4.Enabled = true;
                stopLossPerNumericUpDown4.BackColor = Color.FromArgb(255, 255, 255);

                stopLossBuyingPricePerNumericUpDown5.Enabled = true;
                stopLossBuyingPricePerNumericUpDown5.BackColor = Color.FromArgb(255, 255, 255);

                stopLossPerNumericUpDown5.Enabled = true;
                stopLossPerNumericUpDown5.BackColor = Color.FromArgb(255, 255, 255);
            }

        }

        private void investMoneyInitButton_Click(object sender, EventArgs e)
        {
            string _sitemInvestment = investmentPerItemTextBox.Text;
            double _itemInvestment = double.Parse(_sitemInvestment.Replace(",", ""));
            int _buyingCount = (int)investBuyingCountNumeriUpDown.Value;

            // 균등배분
            double _calInvestment = 0;
            if (_itemInvestment != 0)
            {
                _calInvestment = _itemInvestment / (double)_buyingCount;
            }

            if (_buyingCount > 0)
                investMoneyTextBox_1.Text = _calInvestment.ToString("N0");
            if (_buyingCount > 1)
                investMoneyTextBox_2.Text = _calInvestment.ToString("N0");
            if (_buyingCount > 2)
                investMoneyTextBox_3.Text = _calInvestment.ToString("N0");
            if (_buyingCount > 3)
                investMoneyTextBox_4.Text = _calInvestment.ToString("N0");
            if (_buyingCount > 4)
                investMoneyTextBox_5.Text = _calInvestment.ToString("N0");
            if (_buyingCount > 5)
                investMoneyTextBox_6.Text = _calInvestment.ToString("N0");
        }
        // 매수 회수 변경
        private void investBuyingCountNumeriUpDown_ValueChanged(object sender, EventArgs e)
        {
            int _buyingCount = (int)investBuyingCountNumeriUpDown.Value;

            if (_buyingCount == 1)
            {
                investMoneyTextBox_2.ReadOnly = true;
                investMoneyTextBox_2.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_2.Text = "0";
                investMoneyTextBox_3.ReadOnly = true;
                investMoneyTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_3.Text = "0";
                investMoneyTextBox_4.ReadOnly = true;
                investMoneyTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_4.Text = "0";
                investMoneyTextBox_5.ReadOnly = true;
                investMoneyTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_5.Text = "0";
                investMoneyTextBox_6.ReadOnly = true;
                investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_6.Text = "0";

                reBuyingType1InvestPricePerTextBox_2.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_2.Text = "-3";
                reBuyingType1InvestPricePerTextBox_3.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_3.Text = "-6";
                reBuyingType1InvestPricePerTextBox_4.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_4.Text = "-9";
                reBuyingType1InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_5.Text = "-12";
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_6.Text = "-15";

                reBuyingType2MoveLineTextBox_2.ReadOnly = true;
                reBuyingType2MoveLineTextBox_2.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_2.Text = "1";
                reBuyingType2MoveLineTextBox_3.ReadOnly = true;
                reBuyingType2MoveLineTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_3.Text = "1";
                reBuyingType2MoveLineTextBox_4.ReadOnly = true;
                reBuyingType2MoveLineTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_4.Text = "1";
                reBuyingType2MoveLineTextBox_5.ReadOnly = true;
                reBuyingType2MoveLineTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_5.Text = "1";
                reBuyingType2MoveLineTextBox_6.ReadOnly = true;
                reBuyingType2MoveLineTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_6.Text = "1";

                reBuyingType2InvestPricePerTextBox_2.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_2.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_2.Text = "1";
                reBuyingType2InvestPricePerTextBox_3.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_3.Text = "1";
                reBuyingType2InvestPricePerTextBox_4.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_4.Text = "1";
                reBuyingType2InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_5.Text = "1";
                reBuyingType2InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_6.Text = "1";

            }
            else if (_buyingCount == 2)
            {
                investMoneyTextBox_2.ReadOnly = false;
                investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_3.ReadOnly = true;
                investMoneyTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_3.Text = "0";
                investMoneyTextBox_4.ReadOnly = true;
                investMoneyTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_4.Text = "0";
                investMoneyTextBox_5.ReadOnly = true;
                investMoneyTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_5.Text = "0";
                investMoneyTextBox_6.ReadOnly = true;
                investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_6.Text = "0";

                reBuyingType1InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_3.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_3.Text = "-6";
                reBuyingType1InvestPricePerTextBox_4.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_4.Text = "-9";
                reBuyingType1InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_5.Text = "-12";
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_6.Text = "-15";

                reBuyingType2MoveLineTextBox_2.ReadOnly = false;
                reBuyingType2MoveLineTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_3.ReadOnly = true;
                reBuyingType2MoveLineTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_3.Text = "1";
                reBuyingType2MoveLineTextBox_4.ReadOnly = true;
                reBuyingType2MoveLineTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_4.Text = "1";
                reBuyingType2MoveLineTextBox_5.ReadOnly = true;
                reBuyingType2MoveLineTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_5.Text = "1";
                reBuyingType2MoveLineTextBox_6.ReadOnly = true;
                reBuyingType2MoveLineTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_6.Text = "1";

                reBuyingType2InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_3.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_3.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_3.Text = "1";
                reBuyingType2InvestPricePerTextBox_4.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_4.Text = "1";
                reBuyingType2InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_5.Text = "1";
                reBuyingType2InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_6.Text = "1";
            }
            else if (_buyingCount == 3)
            {
                investMoneyTextBox_2.ReadOnly = false;
                investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_3.ReadOnly = false;
                investMoneyTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_4.ReadOnly = true;
                investMoneyTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_4.Text = "0";
                investMoneyTextBox_5.ReadOnly = true;
                investMoneyTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_5.Text = "0";
                investMoneyTextBox_6.ReadOnly = true;
                investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_6.Text = "0";

                reBuyingType1InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_4.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_4.Text = "-9";
                reBuyingType1InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_5.Text = "-12";
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_6.Text = "-15";

                reBuyingType2MoveLineTextBox_2.ReadOnly = false;
                reBuyingType2MoveLineTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_3.ReadOnly = false;
                reBuyingType2MoveLineTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_4.ReadOnly = true;
                reBuyingType2MoveLineTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_4.Text = "1";
                reBuyingType2MoveLineTextBox_5.ReadOnly = true;
                reBuyingType2MoveLineTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_5.Text = "1";
                reBuyingType2MoveLineTextBox_6.ReadOnly = true;
                reBuyingType2MoveLineTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_6.Text = "1";

                reBuyingType2InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_4.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_4.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_4.Text = "1";
                reBuyingType2InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_5.Text = "1";
                reBuyingType2InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_6.Text = "1";
            }
            else if (_buyingCount == 4)
            {
                investMoneyTextBox_2.ReadOnly = false;
                investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_3.ReadOnly = false;
                investMoneyTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_4.ReadOnly = false;
                investMoneyTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_5.ReadOnly = true;
                investMoneyTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_5.Text = "0";
                investMoneyTextBox_6.ReadOnly = true;
                investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_6.Text = "0";

                reBuyingType1InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_4.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_5.Text = "-12";
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_6.Text = "-15";

                reBuyingType2MoveLineTextBox_2.ReadOnly = false;
                reBuyingType2MoveLineTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_3.ReadOnly = false;
                reBuyingType2MoveLineTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_4.ReadOnly = false;
                reBuyingType2MoveLineTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_5.ReadOnly = true;
                reBuyingType2MoveLineTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_5.Text = "1";
                reBuyingType2MoveLineTextBox_6.ReadOnly = true;
                reBuyingType2MoveLineTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_6.Text = "1";

                reBuyingType2InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_4.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_5.Text = "1";
                reBuyingType2InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_6.Text = "1";
            }
            else if (_buyingCount == 5)
            {
                investMoneyTextBox_2.ReadOnly = false;
                investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_3.ReadOnly = false;
                investMoneyTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_4.ReadOnly = false;
                investMoneyTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_5.ReadOnly = false;
                investMoneyTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_6.ReadOnly = true;
                investMoneyTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                investMoneyTextBox_6.Text = "0";

                reBuyingType1InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_4.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_5.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_6.Text = "-15";

                reBuyingType2MoveLineTextBox_2.ReadOnly = false;
                reBuyingType2MoveLineTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_3.ReadOnly = false;
                reBuyingType2MoveLineTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_4.ReadOnly = false;
                reBuyingType2MoveLineTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_5.ReadOnly = false;
                reBuyingType2MoveLineTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_6.ReadOnly = true;
                reBuyingType2MoveLineTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2MoveLineTextBox_6.Text = "1";

                reBuyingType2InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_4.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_5.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType2InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType2InvestPricePerTextBox_6.Text = "1";
            }
            else if (_buyingCount == 6)
            {
                investMoneyTextBox_2.ReadOnly = false;
                investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_3.ReadOnly = false;
                investMoneyTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_4.ReadOnly = false;
                investMoneyTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_5.ReadOnly = false;
                investMoneyTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                investMoneyTextBox_6.ReadOnly = false;
                investMoneyTextBox_6.BackColor = Color.FromArgb(255, 255, 255);

                reBuyingType1InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_4.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_5.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(255, 255, 255);

                reBuyingType2MoveLineTextBox_2.ReadOnly = false;
                reBuyingType2MoveLineTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_3.ReadOnly = false;
                reBuyingType2MoveLineTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_4.ReadOnly = false;
                reBuyingType2MoveLineTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_5.ReadOnly = false;
                reBuyingType2MoveLineTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2MoveLineTextBox_6.ReadOnly = false;
                reBuyingType2MoveLineTextBox_6.BackColor = Color.FromArgb(255, 255, 255);

                reBuyingType2InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_3.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_3.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_4.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_4.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_5.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_5.BackColor = Color.FromArgb(255, 255, 255);
                reBuyingType2InvestPricePerTextBox_6.ReadOnly = false;
                reBuyingType2InvestPricePerTextBox_6.BackColor = Color.FromArgb(255, 255, 255);
            }
        }
        // 매수 콤보박스 내용변경
        private void buyingTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (buyingTypeComboBox.SelectedIndex == 0)
                buyingType1GroupBox.Visible = true;
            else
                buyingType1GroupBox.Visible = false;

            if (buyingTypeComboBox.SelectedIndex == 1)
                buyingType2GroupBox.Visible = true;
            else
                buyingType2GroupBox.Visible = false;

            if (buyingTypeComboBox.SelectedIndex == 2)
                buyingType3GroupBox.Visible = true;
            else
                buyingType3GroupBox.Visible = false;

            if (buyingTypeComboBox.SelectedIndex == 3)
                buyingType4GroupBox.Visible = true;
            else
                buyingType4GroupBox.Visible = false;

            if (buyingTypeComboBox.SelectedIndex == 4)
                buyingType5GroupBox.Visible = true;
            else
                buyingType5GroupBox.Visible = false;

            buyingType2GroupBox.Location = new Point(20, 314);
            buyingType3GroupBox.Location = new Point(20, 314);
            buyingType4GroupBox.Location = new Point(20, 314);
            buyingType5GroupBox.Location = new Point(20, 314);
        }
        // 추매 콤보박스 내용변경
        private void addBuyingTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (addBuyingTypeComboBox.SelectedIndex == 0) // 추매타입1 기본추매
                reBuyingType1GroupBox.Visible = true;
            else
                reBuyingType1GroupBox.Visible = false;

            if (addBuyingTypeComboBox.SelectedIndex == 1) // 추매타입2 이동평균 근접추매                        
                reBuyingType2GroupBox.Visible = true;
            else
                reBuyingType2GroupBox.Visible = false;

            reBuyingType2GroupBox.Location = new Point(20, 569);
        }
        // 익절 콤보박스 내용변경
        private void takeProfitTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (takeProfitTypeComboBox.SelectedIndex == 0) // 익절타입1 기본익절
                takeProfitType1GroupBox.Visible = true;
            else
                takeProfitType1GroupBox.Visible = false;

            if (takeProfitTypeComboBox.SelectedIndex == 1) // 익절타입2
                takeProfitType2GroupBox.Visible = true;
            else
                takeProfitType2GroupBox.Visible = false;

            if (takeProfitTypeComboBox.SelectedIndex == 2) // 익절타입3
                takeProfitType3GroupBox.Visible = true;
            else
                takeProfitType3GroupBox.Visible = false;

            if (takeProfitTypeComboBox.SelectedIndex == 3) // 익절타입4
                takeProfitType4GroupBox.Visible = true;
            else
                takeProfitType4GroupBox.Visible = false;

            if (takeProfitTypeComboBox.SelectedIndex == 4) // 익절타입5
                takeProfitType5GroupBox.Visible = true;
            else
                takeProfitType5GroupBox.Visible = false;

            takeProfitType2GroupBox.Location = new Point(356, 314);
            takeProfitType3GroupBox.Location = new Point(356, 314);
            takeProfitType4GroupBox.Location = new Point(356, 314);
            takeProfitType5GroupBox.Location = new Point(356, 314);
        }
        // 손절 콤보박스 내용변경
        private void stopLossTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stopLossTypeComboBox.SelectedIndex == 0) // 손절타입1
                stopLossType1GroupBox.Visible = true;
            else
                stopLossType1GroupBox.Visible = false;

            if (stopLossTypeComboBox.SelectedIndex == 1) // 손절타입2
                stopLossType2GroupBox.Visible = true;
            else
                stopLossType2GroupBox.Visible = false;

            if (stopLossTypeComboBox.SelectedIndex == 2) // 손절타입3
                stopLossType3GroupBox.Visible = true;
            else
                stopLossType3GroupBox.Visible = false;

            if (stopLossTypeComboBox.SelectedIndex == 3) // 손절타입4
                stopLossType4GroupBox.Visible = true;
            else
                stopLossType4GroupBox.Visible = false;

            stopLossType2GroupBox.Location = new Point(356, 569);
            stopLossType3GroupBox.Location = new Point(356, 569);
            stopLossType4GroupBox.Location = new Point(356, 569);
        }
        // 매수타입2 n봉 선택 콤보박스(이동평균선 일봉분봉)
        private void buyingType2DayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (buyingType2DayComboBox.SelectedIndex == 0) // 일봉
            {
                buyingType2DayComboBox.Location = new Point(17, 87);
                buyingType2MoveBongKindComboBox.Visible = false;
            }
            else // 분봉
            {
                buyingType2DayComboBox.Location = new Point(17 + 43, 87);
                buyingType2MoveBongKindComboBox.Location = new Point(17, 87);
                buyingType2MoveBongKindComboBox.Visible = true;
            }
        }
        // 매수타입2 근접 돌파(이동평균선)
        private void buyingType2DistanceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (buyingType2DistanceComboBox.SelectedIndex == 0) // 근접
            {
                buyingType2MoveBongPerTextBox.Visible = true;
                buyingType2TextPercent.Visible = true;
                buyingType2DistanceComboBox.Location = new Point(201, 118);
                buyingType2TextBuying.Location = new Point(249, 122);
            }
            else // 돌파
            {
                buyingType2MoveBongPerTextBox.Visible = false;
                buyingType2TextPercent.Visible = false;
                buyingType2DistanceComboBox.Location = new Point(201 - 55, 118);
                buyingType2TextBuying.Location = new Point(249 - 55, 122);
            }
        }
        // 매수 스토캐스틱 n봉 선택 콤보박스(일봉 분봉)
        private void buyingType3DayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (buyingType3DayComboBox.SelectedIndex == 0) // 일봉
            {
                buyingType3DayComboBox.Location = new Point(17, 87);
                buyingType3MoveBongKindComboBox.Visible = false;
            }
            else // 분봉
            {
                buyingType3DayComboBox.Location = new Point(17 + 43, 87);
                buyingType3MoveBongKindComboBox.Location = new Point(17, 87);
                buyingType3MoveBongKindComboBox.Visible = true;
            }
            StocTextChanged2(0);
            // 위에가 없으면 분봉에서 일봉으로 넘어갈때 숫자가 변경되지않는 상황 발생
            /*StocTextChanged(buyingType3Period1, null);
            StocTextChanged(buyingType3Period2, null);
            StocTextChanged(buyingType3Period3, null);*/
            // 위에처럼 작성해도되나 코딩하기 불편함
        }
        // 매수 볼린저밴드 n봉 선택 콤보박스(일봉 분봉)
        private void buyingType4DayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (buyingType4DayComboBox.SelectedIndex == 0) // 일봉
            {
                buyingType4DayComboBox.Location = new Point(17, 87);
                buyingType4MoveBongKindComboBox.Visible = false;
            }
            else // 분봉
            {
                buyingType4DayComboBox.Location = new Point(17 + 43, 87);
                buyingType4MoveBongKindComboBox.Location = new Point(17, 87);
                buyingType4MoveBongKindComboBox.Visible = true;
            }
            BollEnveTextChanged2(0);
        }
        // 매수 볼린저 근접,돌파, 이탈 선택
        private void buyingType4DistanceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (buyingType4DistanceComboBox.SelectedIndex == 0) // 근접
            {
                buyingType4MoveBongPerTextBox.Visible = true;
                buyingType4TextPercent.Visible = true;
                buyingType4DistanceComboBox.Location = new Point(168, 118);
                buyingType4TextBuying.Location = new Point(222, 122);
            }
            else // 돌파, 이탈
            {
                buyingType4MoveBongPerTextBox.Visible = false;
                buyingType4TextPercent.Visible = false;
                buyingType4DistanceComboBox.Location = new Point(168 - 55, 118);
                buyingType4TextBuying.Location = new Point(222 - 55, 122);
            }
        }
        // 매수 엔벨로프 n봉 선택 콤보박스(일봉 분봉)
        private void buyingType5DayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (buyingType5DayComboBox.SelectedIndex == 0) // 일봉
            {
                buyingType5DayComboBox.Location = new Point(17, 87);
                buyingType5MoveBongKindComboBox.Visible = false;
            }
            else // 분봉
            {
                buyingType5DayComboBox.Location = new Point(17 + 43, 87);
                buyingType5MoveBongKindComboBox.Location = new Point(17, 87);
                buyingType5MoveBongKindComboBox.Visible = true;
            }
            BollEnveTextChanged2(1);
        }
        // 매수 엔벨로프 근접,돌파,이탈 선택
        private void buyingType5DistanceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (buyingType5DistanceComboBox.SelectedIndex == 0) // 근접
            {
                buyingType5MoveBongPerTextBox.Visible = true;
                buyingType5TextPercent.Visible = true;
                buyingType5DistanceComboBox.Location = new Point(168, 118);
                buyingType5TextBuying.Location = new Point(222, 122);
            }
            else // 돌파, 이탈
            {
                buyingType5MoveBongPerTextBox.Visible = false;
                buyingType5TextPercent.Visible = false;
                buyingType5DistanceComboBox.Location = new Point(168 - 55, 118);
                buyingType5TextBuying.Location = new Point(222 - 55, 122);
            }
        }
        // 매수 사용 체크박스(매수 방식 선택 체크박스) 
        private void buyingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (buyingCheckBox.Checked)
            {
                buyingNotUsedGroupBox.Visible = false;
                buyingTypeComboBox.Visible = true;
            }
            else
            {
                buyingNotUsedGroupBox.Visible = true;
                buyingNotUsedGroupBox.BringToFront();
                buyingNotUsedGroupBox.Location = new Point(20, 314);
                buyingTypeComboBox.Visible = false;
            }
        }
        // 익절 체크박스(익절 방식 선택 체크박스)
        private void takeProfitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (takeProfitCheckBox.Checked)
            {
                takeProfitNotUsedGroupBox.Visible = false;
                takeProfitTypeComboBox.Visible = true;
            }
            else
            {
                takeProfitNotUsedGroupBox.Visible = true;
                takeProfitNotUsedGroupBox.BringToFront();
                takeProfitNotUsedGroupBox.Location = new Point(356, 314);
                takeProfitTypeComboBox.Visible = false;
            }
        }
        // 손절 체크박스(손절 방식 선택 체크박스)
        private void stopLossCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (stopLossCheckBox.Checked)
            {
                stopLossNotUsedGroupBox.Visible = false;
                stopLossTypeComboBox.Visible = true;
            }
            else
            {
                stopLossNotUsedGroupBox.Visible = true;
                stopLossNotUsedGroupBox.BringToFront();
                stopLossNotUsedGroupBox.Location = new Point(356, 569);
                stopLossTypeComboBox.Visible = false;
            }
        }
        // 추매타입2 n봉 콤보박스(이동평균선 추가매수 일봉 분봉)
        private void reBuyingType2DayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reBuyingType2DayComboBox.SelectedIndex == 0) //일봉
            {
                reBuyingType2DayComboBox.Location = new Point(183, 16);
                reBuyingType2MoveBongKindComboBox.Visible = false;
            }
            else // 분봉
            {
                reBuyingType2DayComboBox.Location = new Point(183 + 43, 16);
                reBuyingType2MoveBongKindComboBox.Location = new Point(183, 16);
                reBuyingType2MoveBongKindComboBox.Visible = true;
            }
        }
        // 익절타입2 n봉 콤보박스
        private void takeProfitType2DayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (takeProfitType2DayComboBox.SelectedIndex == 0) // 일봉
            {
                takeProfitType2DayComboBox.Location = new Point(17, 87);
                takeProfitType2MoveBongKindComboBox.Visible = false;
            }
            else // 분봉
            {
                takeProfitType2DayComboBox.Location = new Point(17 + 43, 87);
                takeProfitType2MoveBongKindComboBox.Location = new Point(17, 87);
                takeProfitType2MoveBongKindComboBox.Visible = true;
            }
        }
        // 익절타입2 근접, 돌파 선택
        private void takeProfitType2DistanceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (takeProfitType2DistanceComboBox.SelectedIndex == 0) // 근접
            {
                takeProfitType2MoveBongPerTextBox.Visible = true;
                takeProfitType2TextPercent.Visible = true;
                takeProfitType2DistanceComboBox.Location = new Point(201, 118);
                takeProfitType2TextTakeProfit.Location = new Point(249, 122);
            }
            else // 돌파
            {
                takeProfitType2MoveBongPerTextBox.Visible = false;
                takeProfitType2TextPercent.Visible = false;
                takeProfitType2DistanceComboBox.Location = new Point(201 - 55, 118);
                takeProfitType2TextTakeProfit.Location = new Point(249 - 55, 122);
            }
        }
        // 익절타입3 스토캐스틱 n봉 콤보박스
        private void takeProfitType3DayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (takeProfitType3DayComboBox.SelectedIndex == 0) // 일봉
            {
                takeProfitType3DayComboBox.Location = new Point(17, 87);
                takeProfitType3MoveBongKindComboBox.Visible = false;
            }
            else // 분봉
            {
                takeProfitType3DayComboBox.Location = new Point(17 + 43, 87);
                takeProfitType3MoveBongKindComboBox.Location = new Point(17, 87);
                takeProfitType3MoveBongKindComboBox.Visible = true;
            }
            StocTextChanged2(1);
        }
        // 익절타입4 볼린저밴드 n봉 콤보박스
        private void takeProfitType4DayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (takeProfitType4DayComboBox.SelectedIndex == 0) // 일봉
            {
                takeProfitType4DayComboBox.Location = new Point(17, 87);
                takeProfitType4MoveBongKindComboBox.Visible = false;
            }
            else // 분봉
            {
                takeProfitType4DayComboBox.Location = new Point(17 + 43, 87);
                takeProfitType4MoveBongKindComboBox.Location = new Point(17, 87);
                takeProfitType4MoveBongKindComboBox.Visible = true;
            }
            BollEnveTextChanged2(2);
        }
        // 익절 볼린져 근접,돌파,이탈 선택
        private void takeProfitType4DistanceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (takeProfitType4DistanceComboBox.SelectedIndex == 0) // 근접
            {
                takeProfitType4MoveBongPerTextBox.Visible = true;
                takeProfitType4TextPercent.Visible = true;
                takeProfitType4DistanceComboBox.Location = new Point(168, 118);
                takeProfitType4TextBuying.Location = new Point(222, 122);
            }
            else // 돌파, 이탈
            {
                takeProfitType4MoveBongPerTextBox.Visible = false;
                takeProfitType4TextPercent.Visible = false;
                takeProfitType4DistanceComboBox.Location = new Point(168 - 55, 118);
                takeProfitType4TextBuying.Location = new Point(222 - 55, 122);
            }
        }
        // 익절타입5 엔벨로프 n봉 콤보박스
        private void takeProfitType5DayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (takeProfitType5DayComboBox.SelectedIndex == 0) // 일봉
            {
                takeProfitType5DayComboBox.Location = new Point(17, 87);
                takeProfitType5MoveBongKindComboBox.Visible = false;
            }
            else // 분봉
            {
                takeProfitType5DayComboBox.Location = new Point(17 + 43, 87);
                takeProfitType5MoveBongKindComboBox.Location = new Point(17, 87);
                takeProfitType5MoveBongKindComboBox.Visible = true;
            }
            BollEnveTextChanged2(3);
        }
        // 손절 이동평균
        private void stopLossType2DayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stopLossType2DayComboBox.SelectedIndex == 0) // 일봉
            {
                stopLossType2DayComboBox.Location = new Point(17, 87);
                stopLossType2MoveBongKindComboBox.Visible = false;
            }
            else // 분봉
            {
                stopLossType2DayComboBox.Location = new Point(17 + 43, 87);
                stopLossType2MoveBongKindComboBox.Location = new Point(17, 87);
                stopLossType2MoveBongKindComboBox.Visible = true;
            }
        }
        // 손절 근접, 이탈 선택
        private void stopLossType2DistanceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stopLossType2DistanceComboBox.SelectedIndex == 0) // 근접
            {
                stopLossType2Text_1.Visible = true; // 이동평균선 
                stopLossType2Text_2.Visible = true; // 시 손절                
                stopLossType2Text_3.Visible = false;
                stopLossType2Text_4.Visible = false;
                stopLossType2Text_5.Visible = false;
                stopLossType2Text_6.Visible = false;

                stopLossType2MoveBongPerTextBox.Location = new Point(146, 118);
                stopLossType2TextPercent.Location = new Point(182, 123);
                stopLossType2DistanceComboBox.Location = new Point(201, 118);
                stopLossType2Text_1.Location = new Point(75, 122);
                stopLossType2Text_2.Location = new Point(249, 122);
            }
            else // 돌파
            {
                stopLossType2Text_1.Visible = false; // 이동평균선 
                stopLossType2Text_2.Visible = false; // 시 손절                
                stopLossType2Text_3.Visible = true;
                stopLossType2Text_4.Visible = true;
                stopLossType2Text_5.Visible = true;
                stopLossType2Text_6.Visible = true;

                stopLossType2Text_3.Location = new Point(75, 122);
                stopLossType2MoveBongPerTextBox.Location = new Point(137 + 38, 118);
                stopLossType2TextPercent.Location = new Point(173 + 38, 123);
                stopLossType2DistanceComboBox.Location = new Point(193 + 38, 118);
                stopLossType2Text_4.Location = new Point(244 + 38, 122);
            }
        }
        // 손절 볼린져 밴드 n봉 선택
        private void stopLossType3DayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stopLossType3DayComboBox.SelectedIndex == 0) // 일봉
            {
                stopLossType3DayComboBox.Location = new Point(17, 87);
                stopLossType3MoveBongKindComboBox.Visible = false;
            }
            else // 분봉
            {
                stopLossType3DayComboBox.Location = new Point(17 + 43, 87);
                stopLossType3MoveBongKindComboBox.Location = new Point(17, 87);
                stopLossType3MoveBongKindComboBox.Visible = true;
            }
            BollEnveTextChanged2(4);
        }
        // 손절 볼린져 근접,돌파,이탈 선택
        private void stopLossType3DistanceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stopLossType3DistanceComboBox.SelectedIndex == 0) // 근접
            {
                stopLossType3MoveBongPerTextBox.Visible = true;
                stopLossType3TextPercent.Visible = true;
                stopLossType3DistanceComboBox.Location = new Point(168, 118);
                stopLossType3TextBuying.Location = new Point(222, 122);
            }
            else // 돌파, 이탈
            {
                stopLossType3MoveBongPerTextBox.Visible = false;
                stopLossType3TextPercent.Visible = false;
                stopLossType3DistanceComboBox.Location = new Point(168 - 55, 118);
                stopLossType3TextBuying.Location = new Point(222 - 55, 122);
            }
        }
        // 손절 엔벨로프 밴드 n봉 선택
        private void stopLossType4DayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stopLossType4DayComboBox.SelectedIndex == 0) // 일봉
            {
                stopLossType4DayComboBox.Location = new Point(17, 87);
                stopLossType4MoveBongKindComboBox.Visible = false;
            }
            else // 분봉
            {
                stopLossType4DayComboBox.Location = new Point(17 + 43, 87);
                stopLossType4MoveBongKindComboBox.Location = new Point(17, 87);
                stopLossType4MoveBongKindComboBox.Visible = true;
            }
            BollEnveTextChanged2(5);
        }
        // 손절 엔벨로프 근접,돌파,이탈 선택
        private void stopLossType4DistanceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stopLossType4DistanceComboBox.SelectedIndex == 0) // 근접
            {
                stopLossType4MoveBongPerTextBox.Visible = true;
                stopLossType4TextPercent.Visible = true;
                stopLossType4DistanceComboBox.Location = new Point(168, 118);
                stopLossType4TextBuying.Location = new Point(222, 122);
            }
            else // 돌파, 이탈
            {
                stopLossType4MoveBongPerTextBox.Visible = false;
                stopLossType4TextPercent.Visible = false;
                stopLossType4DistanceComboBox.Location = new Point(168 - 55, 118);
                stopLossType4TextBuying.Location = new Point(222 - 55, 122);
            }
        }
        // 추매 금액부분 관련 설정(각각의 값을 1000단위마다 , 설정 및 전체 금액을 더해서 합계에 표시)
        private void Text_Changed(object sender, EventArgs e)
        {
            string _vstr = "";
            double _value1 = 0, _value2 = 0, _value3 = 0, _value4 = 0, _value5 = 0, _value6 = 0;

            if (sender.Equals(investMoneyTextBox_1))
            {
                if (String.IsNullOrWhiteSpace(investMoneyTextBox_1.Text))
                {
                    investMoneyTextBox_1.Text = "0";
                    return;
                }
                string _str = investMoneyTextBox_1.Text.Replace(",", "");
                int _value = int.Parse(_str);
                investMoneyTextBox_1.Text = _value.ToString("N0");
                investMoneyTextBox_1.Select(investMoneyTextBox_1.Text.Length, 0);
            }
            else if (sender.Equals(investMoneyTextBox_2))
            {
                if (String.IsNullOrWhiteSpace(investMoneyTextBox_2.Text))
                {
                    investMoneyTextBox_2.Text = "0";
                    return;
                }
                string _str = investMoneyTextBox_2.Text.Replace(",", "");
                int _value = int.Parse(_str);
                investMoneyTextBox_2.Text = _value.ToString("N0");
                investMoneyTextBox_2.Select(investMoneyTextBox_2.Text.Length, 0);
            }
            else if (sender.Equals(investMoneyTextBox_3))
            {
                if (String.IsNullOrWhiteSpace(investMoneyTextBox_3.Text))
                {
                    investMoneyTextBox_3.Text = "0";
                    return;
                }
                string _str = investMoneyTextBox_3.Text.Replace(",", "");
                int _value = int.Parse(_str);
                investMoneyTextBox_3.Text = _value.ToString("N0");
                investMoneyTextBox_3.Select(investMoneyTextBox_3.Text.Length, 0);
            }
            else if (sender.Equals(investMoneyTextBox_4))
            {
                if (String.IsNullOrWhiteSpace(investMoneyTextBox_4.Text))
                {
                    investMoneyTextBox_4.Text = "0";
                    return;
                }
                string _str = investMoneyTextBox_4.Text.Replace(",", "");
                int _value = int.Parse(_str);
                investMoneyTextBox_4.Text = _value.ToString("N0");
                investMoneyTextBox_4.Select(investMoneyTextBox_4.Text.Length, 0);
            }
            else if (sender.Equals(investMoneyTextBox_5))
            {
                if (String.IsNullOrWhiteSpace(investMoneyTextBox_5.Text))
                {
                    investMoneyTextBox_5.Text = "0";
                    return;
                }
                string _str = investMoneyTextBox_5.Text.Replace(",", "");
                int _value = int.Parse(_str);
                investMoneyTextBox_5.Text = _value.ToString("N0");
                investMoneyTextBox_5.Select(investMoneyTextBox_5.Text.Length, 0);
            }
            else if (sender.Equals(investMoneyTextBox_6))
            {
                if (String.IsNullOrWhiteSpace(investMoneyTextBox_6.Text))
                {
                    investMoneyTextBox_6.Text = "0";
                    return;
                }
                string _str = investMoneyTextBox_6.Text.Replace(",", "");
                int _value = int.Parse(_str);
                investMoneyTextBox_6.Text = _value.ToString("N0");
                investMoneyTextBox_6.Select(investMoneyTextBox_5.Text.Length, 0);
            }

            _vstr = investMoneyTextBox_1.Text.Replace(",", "");
            _value1 = double.Parse(_vstr);
            _vstr = investMoneyTextBox_2.Text.Replace(",", "");
            _value2 = double.Parse(_vstr);
            _vstr = investMoneyTextBox_3.Text.Replace(",", "");
            _value3 = double.Parse(_vstr);
            _vstr = investMoneyTextBox_4.Text.Replace(",", "");
            _value4 = double.Parse(_vstr);
            _vstr = investMoneyTextBox_5.Text.Replace(",", "");
            _value5 = double.Parse(_vstr);
            _vstr = investMoneyTextBox_6.Text.Replace(",", "");
            _value6 = double.Parse(_vstr);

            investMoneyTotalTextBox.Text = (_value1 + _value2 + _value3 + _value4 + _value5 + _value6).ToString("N0");
        }
        // 텍스트박스 기준 공백일경우 1로 표기
        private void LostFocusEvent(object sender, EventArgs e)
        {
            TextBox _textbox = (TextBox)sender;

            if (String.IsNullOrWhiteSpace(_textbox.Text))
            {
                _textbox.Text = "1";
            }
        }
        // 뉴메릭업다운 기준 익절이 공백이면 3, 나머지는 -3
        private void LostFocusEvent2(object sender, EventArgs e)
        {
            NumericUpDown _textbox = (NumericUpDown)sender;

            if (String.IsNullOrWhiteSpace(_textbox.Text))
            {
                if (sender.Equals(takeProfitBuyingPricePerNumericUpDown1))
                    _textbox.Text = "3.0";
                else
                    _textbox.Text = "-3.0";
            }
        }
        // 텍스트 박스 기준 공백이면 1 (여기서는 매수 금액설정 그룹박스 -> 매수 회수 부분)
        private void LostFocusEvent3(object sender, EventArgs e)
        {
            NumericUpDown _textbox = (NumericUpDown)sender;

            if (String.IsNullOrWhiteSpace(_textbox.Text))
            {
                _textbox.Text = "1";
            }
        }

        private void LostFocusEvent4(object sender, EventArgs e)
        {
            NumericUpDown _textbox = (NumericUpDown)sender;

            if (String.IsNullOrWhiteSpace(_textbox.Text))
            {
                _textbox.Text = "0";
            }
        }
        // 숫자(0~9), 소수점(.), 백스페이스 외의 입력을 허용하지 않도록 설정
        private void Key_Press(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != '.' && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
        // 숫자와 백스페이스 키 외에는 입력을 허용하지 않도록 설정
        private void Key_Press2(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
        // 숫자(0~9), 백스페이스 외의 입력을 허용하지 않도록 설정
        private void Key_Press3(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
        // 숫자(0~9), 소수점(.), 마이너스(-), 백스페이스 외의 입력을 허용하지 않도록 설정
        private void Key_Press4(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != '.' && e.KeyChar != 8 && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }
        // 모든 입력을 차단
        private void Key_Press5(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        // 숫자(0~9), 소수점(.), 백스페이스 외의 입력을 허용하지 않도록 설정
        private void Key_Press6(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != '.' && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
        private void BongTextChanged(object sender, EventArgs e)
        {
            ComboBox _comboBox = (ComboBox)sender;
            if (String.IsNullOrWhiteSpace(_comboBox.Text))
                return;

            int _num = Int32.Parse(_comboBox.Text);

            if (_num == 1 || _num == 3 || _num == 5 || _num == 10 || _num == 15 || _num == 30 || _num == 45 || _num == 60)
            {

            }
            else
            {
                _comboBox.Text = "1";
            }

            // 최대값 자동으로 넣어주기
            if (sender.Equals(buyingType2MoveBongKindComboBox)) // 매수 이동평균
                MovingPriceTextChanged(buyingType2MoveLineTextBox, null);
            // MovingPriceTextChanged(buyingType2MoveLineTextBox, null); -> 부가설명: 해당 메서드는 이벤트메서드임 , 이벤트메서드는 기본적으로 어떤 특정행동을 하면 자동으로 호출되는 방식인데, 이걸 일반 메서드처럼 활용하기위해 뒤에 null값을 붙여서 사용한거임
            else if (sender.Equals(takeProfitType2MoveBongKindComboBox)) // 익절 이동평균
                MovingPriceTextChanged(takeProfitType2MoveLineTextBox, null);
            else if (sender.Equals(stopLossType2MoveBongKindComboBox)) // 손절 이동평균
                MovingPriceTextChanged(stopLossType2MoveLineTextBox, null);

            if (sender.Equals(buyingType3MoveBongKindComboBox)) // 매수 스토캐스틱
                StocTextChanged2(0);
            else if (sender.Equals(takeProfitType3MoveBongKindComboBox)) // 익절 스토캐스틱
                StocTextChanged2(1);

            if (sender.Equals(buyingType4MoveBongKindComboBox)) // 매수 볼린져밴드
                BollEnveTextChanged2(0);
            else if (sender.Equals(buyingType5MoveBongKindComboBox)) // 매수 엔벨로프
                BollEnveTextChanged2(1);
            else if (sender.Equals(takeProfitType4MoveBongKindComboBox)) // 익절 볼린져밴드
                BollEnveTextChanged2(2);
            else if (sender.Equals(takeProfitType5MoveBongKindComboBox)) // 익절 엔벨로프
                BollEnveTextChanged2(3);
            else if (sender.Equals(stopLossType3MoveBongKindComboBox)) // 손절 볼린져밴드
                BollEnveTextChanged2(4);
            else if (sender.Equals(stopLossType4MoveBongKindComboBox)) // 손절 엔벨로프
                BollEnveTextChanged2(5);
        }
        private void StocTextChanged(object sender, EventArgs e)
        {
            int _dayBunbong = 0;// 일봉, 분봉
            int _bunbong = 0;//
            int _value1 = 0, _value2 = 0, _value3 = 0;
            int[] _dayBongValue = { 120, 80, 80 };
            int[,] _bunBongValue = { {200,150,150},
                                                  {190,140,140},
                                                  {180,130,130},
                                                  {150,110,110},
                                                  {80,80,80},
                                                  {50,40,40},
                                                  {30,20,20},
                                                  {20,10,10}};

            TextBox _textbox = (TextBox)sender;
            if (String.IsNullOrWhiteSpace(_textbox.Text))
                return;
            if (_textbox.Text == ".")
                return;

            if (sender.Equals(buyingType3Period1) || sender.Equals(buyingType3Period2) || sender.Equals(buyingType3Period3)) // 매수 스토캐스틱
            {
                if (sender.Equals(buyingType3Period1)) _value1 = 0;
                else if (sender.Equals(buyingType3Period2)) _value1 = 1;
                else if (sender.Equals(buyingType3Period3)) _value1 = 2;
                _dayBunbong = buyingType3DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = buyingType3MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
            }
            else if (sender.Equals(takeProfitType3Period1) || sender.Equals(takeProfitType3Period2) || sender.Equals(takeProfitType3Period3)) // 익절 스토캐스틱
            {
                if (sender.Equals(takeProfitType3Period1)) _value1 = 0;
                else if (sender.Equals(takeProfitType3Period2)) _value1 = 1;
                else if (sender.Equals(takeProfitType3Period3)) _value1 = 2;
                _dayBunbong = takeProfitType3DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = takeProfitType3MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
            }

            Double _num = Double.Parse(_textbox.Text);

            if (_dayBunbong == 0) // 일봉
            {
                if (_num > _dayBongValue[_value1])
                {
                    _textbox.Text = _dayBongValue[_value1].ToString();

                    if (e != null)
                    {
                        string _str = "값의 범위는 1부터 " + _dayBongValue[_value1].ToString() + "까지입니다.";
                        MessageBox.Show(_str, "범위", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (_dayBunbong == 1) // 분봉
            {
                if (_num > _bunBongValue[_bunbong, _value1])
                {
                    _textbox.Text = _bunBongValue[_bunbong, _value1].ToString();

                    if (e != null)
                    {
                        string _str = "값의 범위는 1부터 " + _bunBongValue[_bunbong, _value1].ToString() + "까지입니다.";
                        MessageBox.Show(_str, "범위", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void StocTextChanged2(int type)
        {
            int _dayBunbong = 0;// 일봉, 분봉
            int _bunbong = 0;//
            int[] _dayBongValue = { 120, 80, 80 };
            int[,] _bunBongValue = { {200,150,150},
                                                  {190,140,140},
                                                  {180,130,130},
                                                  {150,110,110},
                                                  {80,80,80},
                                                  {50,40,40},
                                                  {30,20,20},
                                                  {20,10,10}};

            // 1분봉 200, 150, 150
            // 3분봉 190, 140, 140
            // 5분봉 180, 130, 130
            // 10분봉 150, 110, 110
            // 15분봉 80, 80, 80
            // 30분봉 50, 40, 40
            // 45분봉 30, 20, 20
            // 60분봉 20, 10, 10

            if (type == 0) // 매수 스토캐스틱
            {
                _dayBunbong = buyingType3DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = buyingType3MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
                if (_bunbong == -1)
                {
                    buyingType3MoveBongKindComboBox.SelectedIndex = 0;
                    _bunbong = 0;
                }
                if (_dayBunbong == 0) // 일봉
                {
                    if (int.Parse(buyingType3Period1.Text) > _dayBongValue[0])
                        buyingType3Period1.Text = _dayBongValue[0].ToString();
                    if (int.Parse(buyingType3Period2.Text) > _dayBongValue[1])
                        buyingType3Period2.Text = _dayBongValue[1].ToString();
                    if (int.Parse(buyingType3Period3.Text) > _dayBongValue[2])
                        buyingType3Period3.Text = _dayBongValue[2].ToString();
                }
                else if (_dayBunbong == 1) // 분봉
                {
                    if (int.Parse(buyingType3Period1.Text) > _bunBongValue[_bunbong, 0])
                        buyingType3Period1.Text = _bunBongValue[_bunbong, 0].ToString();
                    if (int.Parse(buyingType3Period2.Text) > _bunBongValue[_bunbong, 1])
                        buyingType3Period2.Text = _bunBongValue[_bunbong, 1].ToString();
                    if (int.Parse(buyingType3Period3.Text) > _bunBongValue[_bunbong, 2])
                        buyingType3Period3.Text = _bunBongValue[_bunbong, 2].ToString();
                }
            }
            else if (type == 1) // 익절 스토캐스틱 
            {
                _dayBunbong = takeProfitType3DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = takeProfitType3MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
                if (_bunbong == -1)
                {
                    takeProfitType3MoveBongKindComboBox.SelectedIndex = 0;
                    _bunbong = 0;
                }
                if (_dayBunbong == 0) // 일봉
                {
                    if (int.Parse(takeProfitType3Period1.Text) > _dayBongValue[0])
                        takeProfitType3Period1.Text = _dayBongValue[0].ToString();
                    if (int.Parse(takeProfitType3Period2.Text) > _dayBongValue[1])
                        takeProfitType3Period2.Text = _dayBongValue[1].ToString();
                    if (int.Parse(takeProfitType3Period3.Text) > _dayBongValue[2])
                        takeProfitType3Period3.Text = _dayBongValue[2].ToString();
                }
                else if (_dayBunbong == 1) // 분봉
                {
                    if (int.Parse(takeProfitType3Period1.Text) > _bunBongValue[_bunbong, 0])
                        takeProfitType3Period1.Text = _bunBongValue[_bunbong, 0].ToString();
                    if (int.Parse(takeProfitType3Period2.Text) > _bunBongValue[_bunbong, 1])
                        takeProfitType3Period2.Text = _bunBongValue[_bunbong, 1].ToString();
                    if (int.Parse(takeProfitType3Period3.Text) > _bunBongValue[_bunbong, 2])
                        takeProfitType3Period3.Text = _bunBongValue[_bunbong, 2].ToString();
                }
            }
        }
        private void BollEnveTextChanged(object sender, EventArgs e)
        {
            int _dayBunbong = 0;// 일봉, 분봉
            int _bunbong = 0;//
            int _value1 = 0, _value2 = 0, _value3 = 0;
            int[] _dayBongValue = { 300, 100 };
            int[,] _bunBongValue = { {300,100},
                                                  {300,100},
                                                  {300,100},
                                                  {300,100},
                                                  {300,100},
                                                  {200,100},
                                                  {100,100},
                                                  {50,100}};
            // _bunBongValue는 2차원 배열 _bunBongValue[0,0] = 300 , _bunBongValue[0,1] = 100 , _bunBongValue[1,0] = 300, _bunBongValue[1,1] = 100,
            // ..., _bunBongValue[7,0] = 50, _bunBongValue[7,1] = 100

            TextBox _textbox = (TextBox)sender;
            if (String.IsNullOrWhiteSpace(_textbox.Text))
                return;
            if (_textbox.Text == ".")
                return;

            if (sender.Equals(buyingType4Period1) || sender.Equals(buyingType4Period2)) // 매수 볼린저밴드
            {
                if (sender.Equals(buyingType4Period1)) _value1 = 0;
                else if (sender.Equals(buyingType4Period2)) _value1 = 1;
                _dayBunbong = buyingType4DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = buyingType4MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
            }
            else if (sender.Equals(buyingType5Period1) || sender.Equals(buyingType5Period2)) // 매수 엔벨로프
            {
                if (sender.Equals(buyingType5Period1)) _value1 = 0;
                else if (sender.Equals(buyingType5Period2)) _value1 = 1;
                _dayBunbong = buyingType5DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = buyingType5MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
            }
            else if (sender.Equals(takeProfitType4Period1) || sender.Equals(takeProfitType4Period2)) // 익절 볼린저밴드
            {
                if (sender.Equals(takeProfitType4Period1)) _value1 = 0;
                else if (sender.Equals(takeProfitType4Period2)) _value1 = 1;
                _dayBunbong = takeProfitType4DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = takeProfitType4MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
            }
            else if (sender.Equals(takeProfitType5Period1) || sender.Equals(takeProfitType5Period2)) // 익절 엔벨로프
            {
                if (sender.Equals(takeProfitType5Period1)) _value1 = 0;
                else if (sender.Equals(takeProfitType5Period2)) _value1 = 1;
                _dayBunbong = takeProfitType5DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = takeProfitType5MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
            }
            else if (sender.Equals(stopLossType3Period1) || sender.Equals(stopLossType3Period2)) // 손절 볼린저밴드
            {
                if (sender.Equals(stopLossType3Period1)) _value1 = 0;
                else if (sender.Equals(stopLossType3Period2)) _value1 = 1;
                _dayBunbong = stopLossType3DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = stopLossType3MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
            }
            else if (sender.Equals(stopLossType4Period1) || sender.Equals(stopLossType4Period2)) // 손절 엔벨로프
            {
                if (sender.Equals(stopLossType4Period1)) _value1 = 0;
                else if (sender.Equals(stopLossType4Period2)) _value1 = 1;
                _dayBunbong = stopLossType4DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = stopLossType4MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
            }

            Double _num = Double.Parse(_textbox.Text);

            if (_dayBunbong == 0) // 일봉
            {
                if (_num > _dayBongValue[_value1])
                {
                    _textbox.Text = _dayBongValue[_value1].ToString();

                    if (e != null)
                    {
                        string _str = "값의 범위는 1부터 " + _dayBongValue[_value1].ToString() + "까지입니다.";
                        MessageBox.Show(_str, "범위", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (_dayBunbong == 1) // 분봉
            {
                if (_num > _bunBongValue[_bunbong, _value1])
                {
                    _textbox.Text = _bunBongValue[_bunbong, _value1].ToString();

                    if (e != null)
                    {
                        string _str = "값의 범위는 1부터 " + _bunBongValue[_bunbong, _value1].ToString() + "까지입니다.";
                        MessageBox.Show(_str, "범위", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void BollEnveTextChanged2(int type)
        {
            int _dayBunbong = 0;// 일봉, 분봉
            int _bunbong = 0;
            int[] _dayBongValue = { 300, 100 };
            int[,] _bunBongValue = { {300,100},
                                                  {300,100},
                                                  {300,100},
                                                  {300,100},
                                                  {300,100},
                                                  {200,100},
                                                  {100,100},
                                                  {50,100}};

            if (type == 0) // 매수 볼린저밴드
            {
                _dayBunbong = buyingType4DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = buyingType4MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
                if (_bunbong == -1)
                {
                    buyingType4MoveBongKindComboBox.SelectedIndex = 0;
                    _bunbong = 0;
                }
                if (_dayBunbong == 0) // 일봉
                {
                    if (double.Parse(buyingType4Period1.Text) > _dayBongValue[0])
                        buyingType4Period1.Text = _dayBongValue[0].ToString();
                    if (double.Parse(buyingType4Period2.Text) > _dayBongValue[1])
                        buyingType4Period2.Text = _dayBongValue[1].ToString();
                }
                else if (_dayBunbong == 1) // 분봉
                {
                    if (double.Parse(buyingType4Period1.Text) > _bunBongValue[_bunbong, 0])
                        buyingType4Period1.Text = _bunBongValue[_bunbong, 0].ToString();
                    if (double.Parse(buyingType4Period2.Text) > _bunBongValue[_bunbong, 1])
                        buyingType4Period2.Text = _bunBongValue[_bunbong, 1].ToString();
                }
            }
            else if (type == 1) // 매수 엔벨로프
            {
                _dayBunbong = buyingType5DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = buyingType5MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
                if (_bunbong == -1)
                {
                    buyingType5MoveBongKindComboBox.SelectedIndex = 0;
                    _bunbong = 0;
                }
                if (_dayBunbong == 0) // 일봉
                {
                    if (double.Parse(buyingType5Period1.Text) > _dayBongValue[0])
                        buyingType5Period1.Text = _dayBongValue[0].ToString();
                    if (double.Parse(buyingType5Period2.Text) > _dayBongValue[1])
                        buyingType5Period2.Text = _dayBongValue[1].ToString();
                }
                else if (_dayBunbong == 1) // 분봉
                {
                    if (double.Parse(buyingType5Period1.Text) > _bunBongValue[_bunbong, 0])
                        buyingType5Period1.Text = _bunBongValue[_bunbong, 0].ToString();
                    if (double.Parse(buyingType5Period2.Text) > _bunBongValue[_bunbong, 1])
                        buyingType5Period2.Text = _bunBongValue[_bunbong, 1].ToString();
                }
            }
            else if (type == 2) // 익절 볼린져밴드
            {
                _dayBunbong = takeProfitType4DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = takeProfitType4MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
                if (_bunbong == -1)
                {
                    takeProfitType4MoveBongKindComboBox.SelectedIndex = 0;
                    _bunbong = 0;
                }
                if (_dayBunbong == 0) // 일봉
                {
                    if (double.Parse(takeProfitType4Period1.Text) > _dayBongValue[0])
                        takeProfitType4Period1.Text = _dayBongValue[0].ToString();
                    if (double.Parse(takeProfitType4Period2.Text) > _dayBongValue[1])
                        takeProfitType4Period2.Text = _dayBongValue[1].ToString();
                }
                else if (_dayBunbong == 1) // 분봉
                {
                    if (double.Parse(takeProfitType4Period1.Text) > _bunBongValue[_bunbong, 0])
                        takeProfitType4Period1.Text = _bunBongValue[_bunbong, 0].ToString();
                    if (double.Parse(takeProfitType4Period2.Text) > _bunBongValue[_bunbong, 1])
                        takeProfitType4Period2.Text = _bunBongValue[_bunbong, 1].ToString();
                }
            }
            else if (type == 3) // 익절 엔벨로프
            {
                _dayBunbong = takeProfitType5DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = takeProfitType5MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
                if (_bunbong == -1)
                {
                    takeProfitType5MoveBongKindComboBox.SelectedIndex = 0;
                    _bunbong = 0;
                }
                if (_dayBunbong == 0) // 일봉
                {
                    if (double.Parse(takeProfitType5Period1.Text) > _dayBongValue[0])
                        takeProfitType5Period1.Text = _dayBongValue[0].ToString();
                    if (double.Parse(takeProfitType5Period2.Text) > _dayBongValue[1])
                        takeProfitType5Period2.Text = _dayBongValue[1].ToString();
                }
                else if (_dayBunbong == 1) // 분봉
                {
                    if (double.Parse(takeProfitType5Period1.Text) > _bunBongValue[_bunbong, 0])
                        takeProfitType5Period1.Text = _bunBongValue[_bunbong, 0].ToString();
                    if (double.Parse(takeProfitType5Period2.Text) > _bunBongValue[_bunbong, 1])
                        takeProfitType5Period2.Text = _bunBongValue[_bunbong, 1].ToString();
                }
            }
            else if (type == 4) // 손절 볼린져밴드
            {
                _dayBunbong = stopLossType3DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = stopLossType3MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
                if (_bunbong == -1)
                {
                    stopLossType3MoveBongKindComboBox.SelectedIndex = 0;
                    _bunbong = 0;
                }
                if (_dayBunbong == 0) // 일봉
                {
                    if (double.Parse(stopLossType3Period1.Text) > _dayBongValue[0])
                        stopLossType3Period1.Text = _dayBongValue[0].ToString();
                    if (double.Parse(stopLossType3Period2.Text) > _dayBongValue[1])
                        stopLossType3Period2.Text = _dayBongValue[1].ToString();
                }
                else if (_dayBunbong == 1) // 분봉
                {
                    if (double.Parse(stopLossType3Period1.Text) > _bunBongValue[_bunbong, 0])
                        stopLossType3Period1.Text = _bunBongValue[_bunbong, 0].ToString();
                    if (double.Parse(stopLossType3Period2.Text) > _bunBongValue[_bunbong, 1])
                        stopLossType3Period2.Text = _bunBongValue[_bunbong, 1].ToString();
                }
            }
            else if (type == 5) // 손절 엔벨로프
            {
                _dayBunbong = stopLossType4DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = stopLossType4MoveBongKindComboBox.SelectedIndex; // 1,3,5,...
                if (_bunbong == -1)
                {
                    stopLossType4MoveBongKindComboBox.SelectedIndex = 0;
                    _bunbong = 0;
                }
                if (_dayBunbong == 0) // 일봉
                {
                    if (double.Parse(stopLossType4Period1.Text) > _dayBongValue[0])
                        stopLossType4Period1.Text = _dayBongValue[0].ToString();
                    if (double.Parse(stopLossType4Period2.Text) > _dayBongValue[1])
                        stopLossType4Period2.Text = _dayBongValue[1].ToString();
                }
                else if (_dayBunbong == 1) // 분봉
                {
                    if (double.Parse(stopLossType4Period1.Text) > _bunBongValue[_bunbong, 0])
                        stopLossType4Period1.Text = _bunBongValue[_bunbong, 0].ToString();
                    if (double.Parse(stopLossType4Period2.Text) > _bunBongValue[_bunbong, 1])
                        stopLossType4Period2.Text = _bunBongValue[_bunbong, 1].ToString();
                }
            }
        }
        // 이동평균선일경우(매수, 익절 손절)시 값의 범위를 제한하고 범위를 넘을경우 메시지박스 표시 및 최댓값으로 고정
        private void MovingPriceTextChanged(object sender, EventArgs e)
        {
            //sender를 textbox로 캐스팅?
            TextBox _textbox = (TextBox)sender;

            int _simpleMovePrice = 0;
            int _dayBunbong = 0;
            int _bunbong = 0;

            if (sender.Equals(buyingType2MoveLineTextBox))
            {
                _simpleMovePrice = buyingType2MoveKindComboBox.SelectedIndex; // 단순, 지수
                _dayBunbong = buyingType2DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = buyingType2MoveBongKindComboBox.SelectedIndex;
                if (_bunbong == -1)
                {
                    buyingType2MoveBongKindComboBox.SelectedIndex = 0;
                    _bunbong = 0;
                }
            }
            else if (sender.Equals(takeProfitType2MoveLineTextBox))
            {
                _simpleMovePrice = takeProfitType2MoveKindComboBox.SelectedIndex; // 단순, 지수
                _dayBunbong = takeProfitType2DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = takeProfitType2MoveBongKindComboBox.SelectedIndex;
                if (_bunbong == -1)
                {
                    takeProfitType2MoveBongKindComboBox.SelectedIndex = 0;
                    _bunbong = 0;
                }
            }
            else if (sender.Equals(stopLossType2MoveLineTextBox))
            {
                _simpleMovePrice = stopLossType2MoveKindComboBox.SelectedIndex; // 단순, 지수
                _dayBunbong = stopLossType2DayComboBox.SelectedIndex; // 일봉, 분봉
                _bunbong = stopLossType2MoveBongKindComboBox.SelectedIndex;
                if (_bunbong == -1)
                {
                    stopLossType2MoveBongKindComboBox.SelectedIndex = 0;
                    _bunbong = 0;
                }
            }
            // 텍스트박스 입력된 값이 비어있거나 공백일경우 return(처리하지않음)
            if (String.IsNullOrWhiteSpace(_textbox.Text))
            {
                return;
            }

            if (_dayBunbong == 0) // 일봉
            {
                int _num = Int32.Parse(_textbox.Text);

                if (_num > 550)
                {
                    _textbox.Text = "550"; // 텍스트박스 값을 범위의 최대값으로 고정
                    if (e != null)
                        MessageBox.Show("값의 범위는 1부터 600까지입니다.", "범위", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (_dayBunbong == 1) // 분봉
            {
                int[] _rangeNormal = { 600, 600, 600, 450, 300, 150, 100, 70 }; // 단순
                int[] _rangeExopnential = { 600, 600, 600, 450, 300, 150, 100, 70 }; // 지수
                int[] _bong = { 1, 3, 5, 10, 15, 30, 45, 60 };
                int _num = Int32.Parse(_textbox.Text);

                if (_simpleMovePrice == 0) // 단순
                {
                    if (_num > _rangeNormal[_bunbong])
                    {
                        _textbox.Text = _rangeNormal[_bunbong].ToString(); // 텍스트박스 값을 범위의 최대값으로 고정
                        if (e != null)
                        {
                            string _msg = _bong[_bunbong].ToString() + "분봉일때 값의 범위는 1부터 " + _textbox.Text + "까지입니다.";
                            MessageBox.Show(_msg, "범위", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else // 지수
                {
                    if (_num > _rangeExopnential[_bunbong])
                    {
                        _textbox.Text = _rangeExopnential[_bunbong].ToString(); // 텍스트박스 값을 범위의 최대값으로 고정
                        if (e != null)
                        {
                            string _msg = _bong[_bunbong].ToString() + "분봉일때 값의 범위는 1부터 " + _textbox.Text + "까지입니다.";
                            MessageBox.Show(_msg, "범위", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        // 폼 실행시 마지막 지정한 설정값 불러오기
        private void ConditionSettingDialog_load(object sender, EventArgs e)
        {
            // 폼 로드 시 필요한 작업들 수행
            tempConditionListData = gMainForm.gFileIOInstance.getlastConditionListData();

            if (tempConditionListData != null)
            {
                string _tempStr = string.Empty;
                string _conditionIndex = string.Empty;
                string _conditionName = string.Empty;
                string[] _strInvestment, _strInvestmentPrice, _strBuying, _strReBuying, _strTakeProfit, _strStopLoss, _strTsmedo;

                // 조건식인덱스
                _conditionIndex = tempConditionListData[(0 * (int)Save.Condition)];
                //_conditionName = gMainForm.gLoginInstance.conditionList[Int32.Parse(_conditionIndex)].Name;
                _conditionName = gMainForm.ConditionLoadDig.getConditionName(Int32.Parse(_conditionIndex));
                ////////////////////////////////////////////////////////////////////////////// 투자금 설정 //////////////////////////////////////////////////////////////////////////////
                _tempStr = tempConditionListData[(0 * (int)Save.Condition) + 1];
                _strInvestment = _tempStr.Split(';');
                //_strInvestment[0] account
                // _strInvestment[1] conditionName
                // _strInvestment[2] conditionIndex 
                // _strInvestment[3] buyingItmeCount 
                // _strInvestment[4] itemInvestment
                // UI에 셋팅을 한다.
                string conditionName = gMainForm.ConditionLoadDig.getConditionName(Int32.Parse(_strInvestment[2])); //gMainForm.gLoginInstance.conditionList[Int32.Parse(_strInvestment[2])].Name; // 조건식 이름
                gMainForm.ConditionDig.conditionComboBox.Text = conditionName;
                gMainForm.ConditionDig.buyingItemCountTextBox.Text = _strInvestment[3]; // 매수 종목수
                gMainForm.ConditionDig.investmentPerItemTextBox.Text = _strInvestment[4]; // 종목당 투자금
                gMainForm.ConditionDig.transferItemCountTextBox.Text = _strInvestment[5]; // 편입 종목수

                ////////////////////////////////////////////////////////////////////////// 매수금액설정 ///////////////////////////////////////////////////////////////////////////////////
                _tempStr = tempConditionListData[(0 * (int)Save.Condition) + 2];
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
                // ui설정
                gMainForm.ConditionLoadDig.setInvestMoneyTextBox(_buyingCount);
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
                _tempStr = tempConditionListData[(0 * (int)Save.Condition) + 3];
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

                gMainForm.ConditionLoadDig.setBuyingUI(buyingType);
                if (buyingType == 0) // 기본매수
                {
                    int buyingTransferType = Int32.Parse(_strBuying[2]);
                    if (buyingTransferType == 0) // 편입즉시매수
                    {
                        gMainForm.ConditionDig.buyingType1ImmediatelyBuyingRadioButton.Checked = true;
                        gMainForm.ConditionDig.buyingType1TransferPricePerRadioButton.Checked = false;
                    }
                    else // 편입후 n% 매수
                    {
                        gMainForm.ConditionDig.buyingType1ImmediatelyBuyingRadioButton.Checked = false;
                        gMainForm.ConditionDig.buyingType1TransferPricePerRadioButton.Checked = true;
                        // 대비 n%
                        double buyingTransferPer = double.Parse(_strBuying[3]);
                        gMainForm.ConditionDig.buyingType1TransferPriceNumericUpDown.Value = Convert.ToDecimal(buyingTransferPer);
                        // 이상, 이하
                        int buyingTransferUpdown = Int32.Parse(_strBuying[4]);
                        gMainForm.ConditionDig.buyingType1TransferPriceUpDownNumericUpDown.SelectedIndex = buyingTransferUpdown;
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
                _tempStr = tempConditionListData[(0 * (int)Save.Condition) + 4];
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
                gMainForm.ConditionLoadDig.setReBuyingUI(reBuyingType);
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
                _tempStr = tempConditionListData[(0 * (int)Save.Condition) + 5];
                _strTakeProfit = _tempStr.Split(';');
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

                gMainForm.ConditionLoadDig.setTakeProfitUI(takeProfitType);
                if (takeProfitType == 0) // 기본 익절
                {
                    // 매수단가대비 n%
                    double takeProfitBuyingPricePer = double.Parse(_strTakeProfit[2]);
                    gMainForm.ConditionDig.takeProfitBuyingPricePerNumericUpDown1.Value = Convert.ToDecimal(takeProfitBuyingPricePer);
                }
                else if (takeProfitType == 1) // 이동평균선
                {
                    // 이평종류 0:단순, 1:지수                
                    int takeProfitMovePriceKindType = Int32.Parse(_strTakeProfit[3]);
                    gMainForm.ConditionDig.takeProfitType2MoveKindComboBox.SelectedIndex = takeProfitMovePriceKindType;
                    // 월,주,일,분봉(0,1,2,3)
                    int takeProfitBongType = Int32.Parse(_strTakeProfit[4]);
                    gMainForm.ConditionDig.takeProfitType2DayComboBox.SelectedIndex = takeProfitBongType;
                    // 1,3,5,10,20 등등 분봉
                    int takeProfitMinuteType = Int32.Parse(_strTakeProfit[5]);
                    //gMainForm.ConditionDig.takeProfitType2MoveBongKindTextBox.Text = takeProfitMinuteType.ToString();
                    gMainForm.ConditionDig.takeProfitType2MoveBongKindComboBox.SelectedIndex = takeProfitMinuteType;
                    // 1,3,5,20 이평종류
                    int takeProfitMinuteLineType = Int32.Parse(_strTakeProfit[6]);
                    gMainForm.ConditionDig.takeProfitType2MoveLineTextBox.Text = takeProfitMinuteLineType.ToString();
                    // n%
                    double takeProfitLineAccessPer = double.Parse(_strTakeProfit[7]);
                    gMainForm.ConditionDig.takeProfitType2MoveBongPerTextBox.Text = takeProfitLineAccessPer.ToString();
                    // 0:근접, 1:돌파
                    int takeProfitDistance = Int32.Parse(_strTakeProfit[8]);
                    gMainForm.ConditionDig.takeProfitType2DistanceComboBox.SelectedIndex = takeProfitDistance;
                }
                else if (takeProfitType == 2) // 스토캐스틱SLOW
                {
                    // 기간
                    int takeProfitStocPeriod1 = Int32.Parse(_strTakeProfit[9]);
                    gMainForm.ConditionDig.takeProfitType3Period1.Text = takeProfitStocPeriod1.ToString();
                    // %K
                    int takeProfitStocPeriod2 = Int32.Parse(_strTakeProfit[10]);
                    gMainForm.ConditionDig.takeProfitType3Period2.Text = takeProfitStocPeriod2.ToString();
                    // %D
                    int takeProfitStocPeriod3 = Int32.Parse(_strTakeProfit[11]);
                    gMainForm.ConditionDig.takeProfitType3Period3.Text = takeProfitStocPeriod3.ToString();
                    // 월,주,일,분봉(0,1,2,3)
                    int takeProfitBongType = Int32.Parse(_strTakeProfit[4]);
                    gMainForm.ConditionDig.takeProfitType3DayComboBox.SelectedIndex = takeProfitBongType;
                    // 1,3,5,10,20 등등 분봉
                    int takeProfitMinuteType = Int32.Parse(_strTakeProfit[5]);
                    //gMainForm.ConditionDig.takeProfitType3MoveBongKindTextBox.Text = takeProfitMinuteType.ToString();
                    gMainForm.ConditionDig.takeProfitType3MoveBongKindComboBox.SelectedIndex = takeProfitMinuteType;
                    // k값
                    double takeProfitLineAccessPer = double.Parse(_strTakeProfit[7]);
                    gMainForm.ConditionDig.takeProfitType3StocValueTextBox.Text = takeProfitLineAccessPer.ToString();
                    // 0:이상, 1:이하
                    int takeProfitDistance = Int32.Parse(_strTakeProfit[8]);
                    gMainForm.ConditionDig.takeProfitType3DistanceComboBox.SelectedIndex = takeProfitDistance;
                }
                else if (takeProfitType == 3) // 볼린저밴드
                {
                    // 기간
                    int takeProfitStocPeriod1 = Int32.Parse(_strTakeProfit[9]);
                    gMainForm.ConditionDig.takeProfitType4Period1.Text = takeProfitStocPeriod1.ToString();
                    // 승수
                    double takeProfitBollPeriod = double.Parse(_strTakeProfit[12]);
                    gMainForm.ConditionDig.takeProfitType4Period2.Text = takeProfitBollPeriod.ToString();
                    // 월,주,일,분봉(0,1,2,3)
                    int takeProfitBongType = Int32.Parse(_strTakeProfit[4]);
                    gMainForm.ConditionDig.takeProfitType4DayComboBox.SelectedIndex = takeProfitBongType;
                    // 1,3,5,10,20 등등 분봉
                    int takeProfitMinuteType = Int32.Parse(_strTakeProfit[5]);
                    //gMainForm.ConditionDig.takeProfitType4MoveBongKindTextBox.Text = takeProfitMinuteType.ToString();
                    gMainForm.ConditionDig.takeProfitType4MoveBongKindComboBox.SelectedIndex = takeProfitMinuteType;
                    // 상한선, 중심선, 하한선(0, 1, 2)
                    int takeProfitLine3Type = Int32.Parse(_strTakeProfit[13]);
                    gMainForm.ConditionDig.takeProfitType4LineComboBox.SelectedIndex = takeProfitLine3Type;
                    // n%
                    double takeProfitLineAccessPer = double.Parse(_strTakeProfit[7]);
                    gMainForm.ConditionDig.takeProfitType4MoveBongPerTextBox.Text = takeProfitLineAccessPer.ToString();
                    // 0:근접, 1:돌파, 2:이탈
                    int takeProfitDistance = Int32.Parse(_strTakeProfit[8]);
                    gMainForm.ConditionDig.takeProfitType4DistanceComboBox.SelectedIndex = takeProfitDistance;
                }
                else if (takeProfitType == 4) // 엔벨로프
                {
                    // 기간
                    int takeProfitStocPeriod1 = Int32.Parse(_strTakeProfit[9]);
                    gMainForm.ConditionDig.takeProfitType5Period1.Text = takeProfitStocPeriod1.ToString();
                    // 승수
                    double takeProfitBollPeriod = double.Parse(_strTakeProfit[12]);
                    gMainForm.ConditionDig.takeProfitType5Period2.Text = takeProfitBollPeriod.ToString();
                    // 월,주,일,분봉(0,1,2,3)
                    int takeProfitBongType = Int32.Parse(_strTakeProfit[4]);
                    gMainForm.ConditionDig.takeProfitType5DayComboBox.SelectedIndex = takeProfitBongType;
                    // 1,3,5,10,20 등등 분봉
                    int takeProfitMinuteType = Int32.Parse(_strTakeProfit[5]);
                    //gMainForm.ConditionDig.takeProfitType5MoveBongKindTextBox.Text = takeProfitMinuteType.ToString();
                    gMainForm.ConditionDig.takeProfitType5MoveBongKindComboBox.SelectedIndex = takeProfitMinuteType;
                    // 상한선, 중심선, 하한선(0, 1, 2)
                    int takeProfitLine3Type = Int32.Parse(_strTakeProfit[13]);
                    gMainForm.ConditionDig.takeProfitType5LineComboBox.SelectedIndex = takeProfitLine3Type;
                    // n%
                    double takeProfitLineAccessPer = double.Parse(_strTakeProfit[7]);
                    gMainForm.ConditionDig.takeProfitType5MoveBongPerTextBox.Text = takeProfitLineAccessPer.ToString();
                    // 0:근접, 1:돌파, 2:이탈
                    int takeProfitDistance = Int32.Parse(_strTakeProfit[8]);
                    gMainForm.ConditionDig.takeProfitType5DistanceComboBox.SelectedIndex = takeProfitDistance;
                }

                //////////////////////////////////////////////////////////////////////////////// 손절 설정 //////////////////////////////////////////////////////////////////////////////
                _tempStr = tempConditionListData[(0 * (int)Save.Condition) + 6];
                _strStopLoss = _tempStr.Split(';');
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

                gMainForm.ConditionLoadDig.setStopLossUI(stopLossType);

                if (stopLossType == 0) // 기본익절
                {
                    // 매수단가대비 n%
                    double stopLossBuyingPricePer = double.Parse(_strStopLoss[2]);
                    gMainForm.ConditionDig.stopLossBuyingPricePerNumericUpDown1.Value = Convert.ToDecimal(stopLossBuyingPricePer);
                }
                else if (stopLossType == 1) // 이동평균선 손절
                {
                    // 이평종류 0:단순, 1:지수                
                    int stopLossMovePriceKindType = Int32.Parse(_strStopLoss[3]);
                    gMainForm.ConditionDig.stopLossType2MoveKindComboBox.SelectedIndex = stopLossMovePriceKindType;
                    // 월,주,일,분봉(0,1,2,3)
                    int stopLossBongType = Int32.Parse(_strStopLoss[4]);
                    gMainForm.ConditionDig.stopLossType2DayComboBox.SelectedIndex = stopLossBongType;
                    // 1,3,5,10,20 등등 분봉
                    int stopLossMinuteType = Int32.Parse(_strStopLoss[5]);
                    //gMainForm.ConditionDig.stopLossType2MoveBongKindTextBox.Text = stopLossMinuteType.ToString();
                    gMainForm.ConditionDig.stopLossType2MoveBongKindComboBox.SelectedIndex = stopLossMinuteType;
                    // 1,3,5,20 이평종류
                    int stopLossMinuteLineType = Int32.Parse(_strStopLoss[6]);
                    gMainForm.ConditionDig.stopLossType2MoveLineTextBox.Text = stopLossMinuteLineType.ToString();
                    // n%
                    double stopLossLineAccessPer = double.Parse(_strStopLoss[7]);
                    gMainForm.ConditionDig.stopLossType2MoveBongPerTextBox.Text = stopLossLineAccessPer.ToString();
                    // 0:근접, 1:돌파
                    int stopLossDistance = Int32.Parse(_strStopLoss[8]);
                    gMainForm.ConditionDig.stopLossType2DistanceComboBox.SelectedIndex = stopLossDistance;
                }
                else if (stopLossType == 2) // 볼린저벤드
                {
                    // 기간
                    int stopLossStocPeriod1 = Int32.Parse(_strStopLoss[9]);
                    gMainForm.ConditionDig.stopLossType3Period1.Text = stopLossStocPeriod1.ToString();
                    // 승수
                    double stopLossBollPeriod = double.Parse(_strStopLoss[12]);
                    gMainForm.ConditionDig.stopLossType3Period2.Text = stopLossBollPeriod.ToString();
                    // 월,주,일,분봉(0,1,2,3)
                    int stopLossBongType = Int32.Parse(_strStopLoss[4]);
                    gMainForm.ConditionDig.stopLossType3DayComboBox.SelectedIndex = stopLossBongType;
                    // 1,3,5,10,20 등등 분봉
                    int stopLossMinuteType = Int32.Parse(_strStopLoss[5]);
                    //gMainForm.ConditionDig.stopLossType3MoveBongKindTextBox.Text = stopLossMinuteType.ToString();
                    gMainForm.ConditionDig.stopLossType3MoveBongKindComboBox.SelectedIndex = stopLossMinuteType;
                    // 상한선, 중심선, 하한선(0, 1, 2)
                    int stopLossLine3Type = Int32.Parse(_strStopLoss[13]);
                    gMainForm.ConditionDig.stopLossType3LineComboBox.SelectedIndex = stopLossLine3Type;
                    // n%
                    double stopLossLineAccessPer = double.Parse(_strStopLoss[7]);
                    gMainForm.ConditionDig.stopLossType3MoveBongPerTextBox.Text = stopLossLineAccessPer.ToString();
                    // 0:근접, 1:돌파, 2:이탈
                    int stopLossDistance = Int32.Parse(_strStopLoss[8]);
                    gMainForm.ConditionDig.stopLossType3DistanceComboBox.SelectedIndex = stopLossDistance;
                }
                else if (stopLossType == 3) // 엔벨로프
                {
                    // 기간
                    int stopLossStocPeriod1 = Int32.Parse(_strStopLoss[9]);
                    gMainForm.ConditionDig.stopLossType4Period1.Text = stopLossStocPeriod1.ToString();
                    // 승수
                    double stopLossBollPeriod = double.Parse(_strStopLoss[12]);
                    gMainForm.ConditionDig.stopLossType4Period2.Text = stopLossBollPeriod.ToString();
                    // 월,주,일,분봉(0,1,2,3)
                    int stopLossBongType = Int32.Parse(_strStopLoss[4]);
                    gMainForm.ConditionDig.stopLossType4DayComboBox.SelectedIndex = stopLossBongType;
                    // 1,3,5,10,20 등등 분봉
                    int stopLossMinuteType = Int32.Parse(_strStopLoss[5]);
                    //gMainForm.ConditionDig.stopLossType4MoveBongKindTextBox.Text = stopLossMinuteType.ToString();
                    gMainForm.ConditionDig.stopLossType4MoveBongKindComboBox.SelectedIndex = stopLossMinuteType;
                    // 상한선, 중심선, 하한선(0, 1, 2)
                    int stopLossLine3Type = Int32.Parse(_strStopLoss[13]);
                    gMainForm.ConditionDig.stopLossType4LineComboBox.SelectedIndex = stopLossLine3Type;
                    // n%
                    double stopLossLineAccessPer = double.Parse(_strStopLoss[7]);
                    gMainForm.ConditionDig.stopLossType4MoveBongPerTextBox.Text = stopLossLineAccessPer.ToString();
                    // 0:근접, 1:돌파, 2:이탈
                    int stopLossDistance = Int32.Parse(_strStopLoss[8]);
                    gMainForm.ConditionDig.stopLossType4DistanceComboBox.SelectedIndex = stopLossDistance;
                }

                //////////////////////////////////////////////////////////////////////////////// ts 설정 //////////////////////////////////////////////////////////////////////////////
                _tempStr = tempConditionListData[(0 * (int)Save.Condition) + 7];
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

                    _tsmedoUsingtype[1] = int.Parse(_strTsmedo[6]);
                    gMainForm.ConditionDig.tsComboBox2.SelectedIndex = _tsmedoUsingtype[1];
                    _tsmedoArchievedPer[1] = double.Parse(_strTsmedo[7]);
                    gMainForm.ConditionDig.tsnumericUpDown2.Value = Convert.ToDecimal(_tsmedoArchievedPer[1]);
                    _tsmedoPercent[1] = double.Parse(_strTsmedo[8]);
                    gMainForm.ConditionDig.tsMedonumericUpDown2.Value = Convert.ToDecimal(_tsmedoPercent[1]);
                    _tsmedoProportion[1] = double.Parse(_strTsmedo[9]);
                    gMainForm.ConditionDig.tspernumericUpDown2.Value = Convert.ToDecimal(_tsmedoProportion[1]);

                    _tsmedoUsingtype[2] = int.Parse(_strTsmedo[10]);
                    gMainForm.ConditionDig.tsComboBox3.SelectedIndex = _tsmedoUsingtype[2];
                    _tsmedoArchievedPer[2] = double.Parse(_strTsmedo[11]);
                    gMainForm.ConditionDig.tsnumericUpDown3.Value = Convert.ToDecimal(_tsmedoArchievedPer[2]);
                    _tsmedoPercent[2] = double.Parse(_strTsmedo[12]);
                    gMainForm.ConditionDig.tsMedonumericUpDown3.Value = Convert.ToDecimal(_tsmedoPercent[2]);
                    _tsmedoProportion[2] = double.Parse(_strTsmedo[13]);
                    gMainForm.ConditionDig.tspernumericUpDown3.Value = Convert.ToDecimal(_tsmedoProportion[2]);
                }
                else
                {
                    gMainForm.ConditionDig.tsMedoCheckBox.Checked = false;
                }
                gMainForm.ConditionDig.stopLossTypeComboBox.SelectedIndex = stopLossType; // 0:기본익절,1:이동평균 근접
            }
            else
            {
                
            }
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
        private void ConditionSettingDialog_closing(object sender, FormClosingEventArgs e)
        {
            // 폼 종료 시 필요한 작업들 수행
            string _strInvestment = string.Empty, _strInvestmentPrice = string.Empty, _strBuying = string.Empty, _strReBuying = string.Empty, _strTakeProfit = string.Empty, _strStopLoss = string.Empty, _strTsmedo = string.Empty;

            // 투자금 설정확인
            if (investmentPerItemTextBox.Text == "")
            {
                MessageBox.Show("종목당 투자금을 설정해주세요.");
                return;

            }
            //투자금이 0이거나 작으면
            double _itemInvestment = double.Parse(investmentPerItemTextBox.Text);
            if (_itemInvestment <= 0)
            {
                MessageBox.Show("종목당 투자금을 설정해주세요.");
                return;
            }
            //매수 종목수 설정확인
            if (buyingItemCountTextBox.Text == "")
            {
                MessageBox.Show("매수 종목수를 설정해 주세요.");
                return;
            }
            //매수 종목수가 0이거나 작으면
            int _buyingItemCount = int.Parse(buyingItemCountTextBox.Text);
            if (_buyingItemCount <= 0)
            {
                MessageBox.Show("매수 종목수를 설정해 주세요.");
                return;
            }

            // 계좌번호
            string account = gMainForm.myAccountComboBox.Text;
            // 조건식 이름
            string conditionName = gMainForm.conditionDataList[conditionComboBox.SelectedIndex].conditionName;
            // 조건식 번호
            string conditionIndex = gMainForm.conditionDataList[conditionComboBox.SelectedIndex].conditionIndex.ToString();
            // 매수 종목 수
            string buyingItemCount = buyingItemCountTextBox.Text;
            // 종목별 투자금액
            string _sitemInvestment = investmentPerItemTextBox.Text;
            string _stotalItemInvestment = investMoneyTotalTextBox.Text;
            double _totalItemInvestment = double.Parse(_stotalItemInvestment.Replace(",", ""));

            if (_itemInvestment < _totalItemInvestment)
            {
                MessageBox.Show("매수 금액 합계가 종목당 투자금을 초과하였습니다.");
                return;
            }
            _strInvestment += account + ";" + conditionName + ";" + conditionIndex + ";" + buyingItemCount + ";" + _sitemInvestment;

            //////////////////////////////////////////// 매수금액설정 /////////////////////////////////////////////////////
            // 저장값
            // 매수횟수, 회차별 투자금 buyingInvestment[0],buyingInvestment[1],buyingInvestment[2],buyingInvestment[3],buyingInvestment[4],buyingInvestment[5],
            string buyingCount = investBuyingCountNumeriUpDown.Value.ToString(); // 매수회수(추매 포함)
            string investMoney_1 = investMoneyTextBox_1.Text;
            string investMoney_2 = investMoneyTextBox_2.Text;
            string investMoney_3 = investMoneyTextBox_3.Text;
            string investMoney_4 = investMoneyTextBox_4.Text;
            string investMoney_5 = investMoneyTextBox_5.Text;
            string investMoney_6 = investMoneyTextBox_6.Text;

            int buyingCountValue = int.Parse(buyingCount);

            if (buyingCountValue > 0)
            {
                if (double.Parse(investMoney_1.Replace(",", "")) <= 0)
                {
                    MessageBox.Show("최소 매수 금액을 설정해 주세요.");
                    return;
                }
            }
            if (buyingCountValue > 1)
            {
                if (double.Parse(investMoney_2.Replace(",", "")) <= 0)
                {
                    MessageBox.Show("1차 추매 매수 금액을 설정해 주세요.");
                    return;
                }
            }
            if (buyingCountValue > 2)
            {
                if (double.Parse(investMoney_3.Replace(",", "")) <= 0)
                {
                    MessageBox.Show("2차 추매 매수 금액을 설정해 주세요.");
                    return;
                }
            }
            if (buyingCountValue > 3)
            {
                if (double.Parse(investMoney_4.Replace(",", "")) <= 0)
                {
                    MessageBox.Show("3차 추매 매수 금액을 설정해 주세요.");
                    return;
                }
            }
            if (buyingCountValue > 4)
            {
                if (double.Parse(investMoney_5.Replace(",", "")) <= 0)
                {
                    MessageBox.Show("4차 추매 매수 금액을 설정해 주세요.");
                    return;
                }
            }
            if (buyingCountValue > 5)
            {
                if (double.Parse(investMoney_6.Replace(",", "")) <= 0)
                {
                    MessageBox.Show("5차 추매 매수 금액을 설정해 주세요.");
                    return;
                }
            }
            _strInvestmentPrice += buyingCount + ";" + investMoney_1 + ";" + investMoney_2 + ";" + investMoney_3 + ";" + investMoney_4 + ";" + investMoney_5 + ";" + investMoney_6;

            /////////////////////////////////////////////// 매수 설정 ////////////////////////////////////////////////
            // 매수 사용 여부
            int buyingUsing = 1;
            if (buyingCheckBox.Checked) buyingUsing = 1; // 매수 설정 -> 매수방식선택 체크박스
            else buyingUsing = 0;
            int buyingType = buyingTypeComboBox.SelectedIndex; // 0:기본매수, 1:이동평균,2:스토케스틱,3:볼린저밴드,4:엔벨로프
            int buyingTransferType = 0; // 0: 편입즉시매수 1: 편입가격대비매수
            double buyingTransferPer = 0; // 편입가격대비매수시 %
            int buyingTransferUpdown = 0; //편입가격대비 매수시 %에 대한 0: 이상, 1: 이하
                                          // 공통 사용처리 변수
            int buyingMovePriceKindType = 0; //0:단순 , 1:지수
            int buyingBongType = 0; // 0:월, 1:주 ,2:일, 3:분
            int buyingMinuteType = 0; // 1,3,5분봉 등등 입력
            int buyingMinuteLineType = 0;// 1,3,5,20이평 등등            
            double buyingMinuteLineAccessPer = 0;// 근접 %
            int buyingDistance = 0; // 0:근접, 1:돌파, 2:이탈, 0:이상, 1:이하
            double buyingStocPeriod1 = 0; //기간
            double buyingStocPeriod2 = 0; //%K
            double buyingStocPeriod3 = 0; //%D
            double buyingBollPeriod = 0; //승수, 엔벨%
            int buyingLine3Type = 0; // 0:상한선,1:중심선,2:하한선

            if (buyingType < 0)
            {
                MessageBox.Show("매수 방식 선택 리스트를 확인해 주세요.");
                return;
            }
            if (buyingType == 0)//기본매수
            {
                if (buyingType1ImmediatelyBuyingRadioButton.Checked) buyingTransferType = 0; // 편입 즉시매수가 체크되어있다면
                else if (buyingType1TransferPricePerRadioButton.Checked) // 편입가격대비매수가 체크되어있다면
                {
                    buyingTransferType = 1;
                    // 대비 n%
                    buyingTransferPer = (double)buyingType1TransferPriceNumericUpDown.Value;
                    // 이상, 이하
                    buyingTransferUpdown = buyingType1TransferPriceUpDownNumericUpDown.SelectedIndex;
                }
            }
            else if (buyingType == 1) // 이동평균선
            {
                // 이평종류 0:단순 ,1:지수
                buyingMovePriceKindType = buyingType2MoveBongKindComboBox.SelectedIndex;
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = buyingType2DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = buyingType2MoveBongKindComboBox.SelectedIndex;
                // 1,3,5,20 이평종류
                buyingMinuteLineType = int.Parse(buyingType2MoveLineTextBox.Text);
                // n%
                buyingMinuteLineAccessPer = double.Parse(buyingType2MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파
                buyingDistance = buyingType2DistanceComboBox.SelectedIndex;
            }
            else if (buyingType == 2) // 스토캐스틱
            {
                // 기간
                buyingStocPeriod1 = double.Parse(buyingType3Period1.Text);
                // %K
                buyingStocPeriod2 = double.Parse(buyingType3Period2.Text);
                // %D
                buyingStocPeriod3 = double.Parse(buyingType3Period3.Text);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = buyingType3DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = buyingType3MoveBongKindComboBox.SelectedIndex;
                // k값
                buyingMinuteLineAccessPer = double.Parse(buyingType3StocValueTextBox.Text);
                // 0:이상, 1:이하
                buyingDistance = buyingType3DistanceComboBox.SelectedIndex;
            }
            else if (buyingType == 3) // 볼린져밴드
            {
                // 기간
                buyingStocPeriod1 = double.Parse(buyingType4Period1.Text);
                // 승수
                buyingBollPeriod = double.Parse(buyingType4Period2.Text);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = buyingType4DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = buyingType4MoveBongKindComboBox.SelectedIndex;
                // 상한선, 중심선, 하한선(0, 1, 2)
                buyingLine3Type = buyingType4LineComboBox.SelectedIndex;
                // n%
                buyingMinuteLineAccessPer = double.Parse(buyingType4MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파, 2:이탈
                buyingDistance = buyingType4DistanceComboBox.SelectedIndex;
            }
            else if (buyingType == 4) // 엔벨로프
            {
                // 기간
                buyingStocPeriod1 = double.Parse(buyingType5Period1.Text);
                // 승수
                buyingBollPeriod = double.Parse(buyingType5Period2.Text);
                // 월,주,일,분봉(0,1,2,3)
                buyingBongType = buyingType5DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                buyingMinuteType = buyingType5MoveBongKindComboBox.SelectedIndex;
                // 상한선, 중심선, 하한선(0, 1, 2)
                buyingLine3Type = buyingType5LineComboBox.SelectedIndex;
                // n%
                buyingMinuteLineAccessPer = double.Parse(buyingType5MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파, 2:이탈
                buyingDistance = buyingType5DistanceComboBox.SelectedIndex;
            }
            _strBuying += buyingUsing.ToString() + ";" + buyingType.ToString() + ";" + buyingTransferType.ToString() + ";" + buyingTransferPer.ToString() + ";" + buyingTransferUpdown.ToString() + ";" +
                                  buyingMovePriceKindType.ToString() + ";" + buyingBongType.ToString() + ";" + buyingMinuteType.ToString() + ";" + buyingMinuteLineType.ToString() + ";" + buyingMinuteLineAccessPer.ToString() + ";" + buyingDistance.ToString() + ";" +
                                  buyingStocPeriod1.ToString() + ";" + buyingStocPeriod2.ToString() + ";" + buyingStocPeriod3.ToString() + ";" + buyingBollPeriod.ToString() + ";" + buyingLine3Type.ToString();

            /////////////////////////////////////////////// 추매 설정 ////////////////////////////////////////////////
            // 추매타입 0:기본추매, 1:이동평균선
            int reBuyingType = addBuyingTypeComboBox.SelectedIndex;
            double[] reBuyingPer = new double[5]; // 추매 %
            int reBuyingMovePriceKindType = 0; // 0:단순 1:지수(이평기준일떄만쓸듯)
            int reBuyingBongType = 0; // 0:월,1:주,2:일,3:분
            int reBuyingMinuteType = 0; // 1,3,5분봉 등등 입력
            int[] reBuyingMinuteLineType = new int[5];// 1,3,5,20이평 등등

            if (reBuyingType < 0)
            {
                MessageBox.Show("추매 방식 선택 리스트를 확인해 주세요.");
                return;
            }
            if (reBuyingType == 0) //기본추매
            {
                if (buyingCountValue > 1)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_2.Text, 0)) return;
                }
                if (buyingCountValue > 2)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_3.Text, 0)) return;
                }
                if (buyingCountValue > 3)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_4.Text, 0)) return;
                }
                if (buyingCountValue > 4)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_5.Text, 0)) return;
                }
                if (buyingCountValue > 5)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_6.Text, 0)) return;
                }
                reBuyingPer[0] = double.Parse(reBuyingType1InvestPricePerTextBox_2.Text);
                reBuyingPer[1] = double.Parse(reBuyingType1InvestPricePerTextBox_3.Text);
                reBuyingPer[2] = double.Parse(reBuyingType1InvestPricePerTextBox_4.Text);
                reBuyingPer[3] = double.Parse(reBuyingType1InvestPricePerTextBox_5.Text);
                reBuyingPer[4] = double.Parse(reBuyingType1InvestPricePerTextBox_6.Text);

            }
            else if (reBuyingType == 1)//이동평균선 추매
            {
                //0: 단순 1:지수
                reBuyingMovePriceKindType = reBuyingType2MoveKindComboBox.SelectedIndex;
                // 월,주,일,분봉(0,1,2,3)
                reBuyingBongType = reBuyingType2DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                reBuyingMinuteType = reBuyingType2MoveBongKindComboBox.SelectedIndex;
                // 1,3,5,20이평 등등
                if (CheckEmpty(reBuyingType2MoveLineTextBox_2.Text, 0)) return;
                if (CheckEmpty(reBuyingType2MoveLineTextBox_3.Text, 0)) return;
                if (CheckEmpty(reBuyingType2MoveLineTextBox_4.Text, 0)) return;
                if (CheckEmpty(reBuyingType2MoveLineTextBox_5.Text, 0)) return;
                if (CheckEmpty(reBuyingType2MoveLineTextBox_6.Text, 0)) return;
                reBuyingMinuteLineType[0] = int.Parse(reBuyingType2MoveLineTextBox_2.Text);
                reBuyingMinuteLineType[1] = int.Parse(reBuyingType2MoveLineTextBox_3.Text);
                reBuyingMinuteLineType[2] = int.Parse(reBuyingType2MoveLineTextBox_4.Text);
                reBuyingMinuteLineType[3] = int.Parse(reBuyingType2MoveLineTextBox_5.Text);
                reBuyingMinuteLineType[4] = int.Parse(reBuyingType2MoveLineTextBox_6.Text);
                // 추매 %
                if (CheckEmpty(reBuyingType2InvestPricePerTextBox_2.Text, 0)) return;
                if (CheckEmpty(reBuyingType2InvestPricePerTextBox_3.Text, 0)) return;
                if (CheckEmpty(reBuyingType2InvestPricePerTextBox_4.Text, 0)) return;
                if (CheckEmpty(reBuyingType2InvestPricePerTextBox_5.Text, 0)) return;
                if (CheckEmpty(reBuyingType2InvestPricePerTextBox_6.Text, 0)) return;
                reBuyingPer[0] = double.Parse(reBuyingType2InvestPricePerTextBox_2.Text);
                reBuyingPer[1] = double.Parse(reBuyingType2InvestPricePerTextBox_3.Text);
                reBuyingPer[2] = double.Parse(reBuyingType2InvestPricePerTextBox_4.Text);
                reBuyingPer[3] = double.Parse(reBuyingType2InvestPricePerTextBox_5.Text);
                reBuyingPer[4] = double.Parse(reBuyingType2InvestPricePerTextBox_6.Text);
            }
            _strReBuying += reBuyingType + ";" + reBuyingPer[0] + ";" + reBuyingPer[1] + ";" + reBuyingPer[2] + ";" + reBuyingPer[3] + ";" + reBuyingPer[4] + ";" +
                                      reBuyingMovePriceKindType + ";" + reBuyingBongType + ";" + reBuyingMinuteType + ";" +
                                      reBuyingMinuteLineType[0] + ";" + reBuyingMinuteLineType[1] + ";" + reBuyingMinuteLineType[2] + ";" + reBuyingMinuteLineType[3] + ";" + reBuyingMinuteLineType[4];

            /////////////////////////////////////////////// 익절 설정 ////////////////////////////////////////////////
            int takeProfitUsing = 1; // 익절 사용 여부
            if (takeProfitCheckBox.Checked) takeProfitUsing = 1;
            else takeProfitUsing = 0;
            int takeProfitType = takeProfitTypeComboBox.SelectedIndex; // 0:기본익절, 1:이동평균근접, 2: 스토캐스틱SLOW, 3:볼린저밴드, 4:엔벨로프
            double takeProfitBuyingPricePer = 0; // 매수단가대비 %
            // 이동평균 근접, 돌파 공통 사용        
            int takeProfitMovePriceKindType = 0; //0:단순, 1:지수        
            int takeProfitBongType = 0; // 0:월,1:주,2:일,3:분
            int takeProfitMinuteType = 0; // 1,3,5분봉 등등 입력
            int takeProfitMinuteLineType = 0;// 1,3,5,20이평 등등            
            double takeProfitLineAccessPer = 0;// 근접 %
            int takeProfitDistance = 0; // 0:근접, 1:돌파,2:이탈, 0:이상, 1:이하
            Double takeProfitStocPeriod1 = 0; //기간
            Double takeProfitStocPeriod2 = 0; //%K
            Double takeProfitStocPeriod3 = 0; //%D
            double takeProfitBollPeriod = 0; //승수, 엔벨%
            int takeProfitLine3Type = 0; // 0:상한선,1:중심선,2:하한선

            if (takeProfitType < 0)
            {
                { MessageBox.Show("익절 방식 선택 리스트를 확인해 주세요."); return; }
            }
            if (takeProfitType == 0) //기본익절
            {
                //매수단가대비 n% 상승시 매도
                takeProfitBuyingPricePer = (double)takeProfitBuyingPricePerNumericUpDown1.Value;
            }
            else if (takeProfitType == 1) // 이동평균선 익절
            {
                // 이평종류 0: 단순 , 1: 지수
                takeProfitMovePriceKindType = takeProfitType2MoveKindComboBox.SelectedIndex;
                // 일봉, 분봉(0,1)
                takeProfitBongType = takeProfitType2DayComboBox.SelectedIndex;
                // 1,3,5,10,15,30,45,60 분봉
                takeProfitMinuteType = takeProfitType2MoveBongKindComboBox.SelectedIndex;

                // 1,3,5,20 이평종류
                if (CheckEmpty(takeProfitType2MoveLineTextBox.Text, 1)) return;
                takeProfitMinuteLineType = int.Parse(takeProfitType2MoveLineTextBox.Text);
                // n%                
                if (CheckEmpty(takeProfitType2MoveBongPerTextBox.Text, 1)) return;
                takeProfitLineAccessPer = double.Parse(takeProfitType2MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파
                takeProfitDistance = takeProfitType2DistanceComboBox.SelectedIndex;
            }
            else if (takeProfitType == 2) // 스토캐스틱SLOW
            {
                if (CheckEmpty(takeProfitType3Period1.Text, 1)) return;
                if (CheckEmpty(takeProfitType3Period2.Text, 1)) return;
                if (CheckEmpty(takeProfitType3Period3.Text, 1)) return;
                // 기간
                takeProfitStocPeriod1 = double.Parse(takeProfitType3Period1.Text);
                // %K
                takeProfitStocPeriod2 = double.Parse(takeProfitType3Period2.Text);
                // %D
                takeProfitStocPeriod3 = double.Parse(takeProfitType3Period3.Text);
                // 일봉, 분봉(0,1)
                takeProfitBongType = takeProfitType3DayComboBox.SelectedIndex;
                // 1,3,5,10,15,30,45,60 분봉
                takeProfitMinuteType = takeProfitType3MoveBongKindComboBox.SelectedIndex;
                // k값
                if (CheckEmpty(takeProfitType3StocValueTextBox.Text, 1)) return;
                takeProfitLineAccessPer = double.Parse(takeProfitType3StocValueTextBox.Text);
                // 0:이상, 1:이하
                takeProfitDistance = takeProfitType3DistanceComboBox.SelectedIndex;
            }
            else if (takeProfitType == 3) // 볼린저밴드
            {
                if (CheckEmpty(takeProfitType4Period1.Text, 1)) return;
                if (CheckEmpty(takeProfitType4Period2.Text, 1)) return;
                // 기간
                takeProfitStocPeriod1 = double.Parse(takeProfitType4Period1.Text);
                // 승수
                takeProfitBollPeriod = double.Parse(takeProfitType4Period2.Text);
                // 일봉, 분봉(0,1)
                takeProfitBongType = takeProfitType4DayComboBox.SelectedIndex;
                // 1,3,5,10,15,30,45,60 분봉
                takeProfitMinuteType = takeProfitType4MoveBongKindComboBox.SelectedIndex;
                // 상한선, 중심선, 하한선(0, 1, 2)
                takeProfitLine3Type = takeProfitType4LineComboBox.SelectedIndex;
                // n%
                if (CheckEmpty(takeProfitType4MoveBongPerTextBox.Text, 1)) return;
                takeProfitLineAccessPer = double.Parse(takeProfitType4MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파, 2:이탈
                takeProfitDistance = takeProfitType4DistanceComboBox.SelectedIndex;
            }
            else if (takeProfitType == 4) // 엔벨로프
            {
                if (CheckEmpty(takeProfitType5Period1.Text, 1)) return;
                if (CheckEmpty(takeProfitType5Period2.Text, 1)) return;
                // 기간
                takeProfitStocPeriod1 = double.Parse(takeProfitType5Period1.Text);
                // 승수
                takeProfitBollPeriod = double.Parse(takeProfitType5Period2.Text);
                // 일봉, 분봉(0,1)
                takeProfitBongType = takeProfitType5DayComboBox.SelectedIndex;
                // 1,3,5,10,15,30,45,60 분봉
                takeProfitMinuteType = takeProfitType5MoveBongKindComboBox.SelectedIndex;
                // 저항선, 중심선, 지지선(0, 1, 2)
                takeProfitLine3Type = takeProfitType5LineComboBox.SelectedIndex;
                // n%
                if (CheckEmpty(takeProfitType5MoveBongPerTextBox.Text, 1)) return;
                takeProfitLineAccessPer = double.Parse(takeProfitType5MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파, 2:이탈
                takeProfitDistance = takeProfitType5DistanceComboBox.SelectedIndex;
            }
            _strTakeProfit += takeProfitUsing.ToString() + ";" + takeProfitType.ToString() + ";" + takeProfitBuyingPricePer.ToString() + ";" +
                                  takeProfitMovePriceKindType.ToString() + ";" + takeProfitBongType.ToString() + ";" + takeProfitMinuteType.ToString() + ";" + takeProfitMinuteLineType.ToString() + ";" + takeProfitLineAccessPer.ToString() + ";" + takeProfitDistance.ToString() + ";" +
                                  takeProfitStocPeriod1.ToString() + ";" + takeProfitStocPeriod2.ToString() + ";" + takeProfitStocPeriod3.ToString() + ";" + takeProfitBollPeriod.ToString() + ";" + takeProfitLine3Type.ToString(); ;

            /////////////////////////////////////////////// 손절 설정 ////////////////////////////////////////////////
            int stopLossUsing = 1; // 손절 사용 여부
            if (stopLossCheckBox.Checked) stopLossUsing = 1;
            else stopLossUsing = 0;
            int stopLossType = stopLossTypeComboBox.SelectedIndex; // 0:기본손절,1:이동평균 근접 2:볼린저밴드. 3:엔벨로프
            double stopLossBuyingPricePer = 0; // 매수단가대비 %
            // 이동평균 근접, 이탈 공통 사용        
            int stopLossMovePriceKindType = 0; //0:단수, 1:지수        
            int stopLossBongType = 0; // 0:월,1:주,2:일,3:분
            int stopLossMinuteType = 0; // 1,3,5분봉 등등 입력
            int stopLossMinuteLineType = 0;// 1,3,5,20이평 등등            
            double stopLossLineAccessPer = 0;// 근접 %
            int stopLossDistance = 0; // 0:근접, 1:돌파,2:이탈, 0:이상, 1:이하
            double stopLossStocPeriod1 = 0; //기간
            double stopLossStocPeriod2 = 0; //%K
            double stopLossStocPeriod3 = 0; //%D
            double stopLossBollPeriod = 0; //승수, 엔벨%
            int stopLossLine3Type = 0; // 0:상한선,1:중심선,2:하한선

            if (stopLossType < 0)
            {
                MessageBox.Show("손절 방식 선택 리스트를 확인해 주세요.");
                return;
            }

            if (stopLossType == 0) // 기본 손절
            {
                // 매수단가대비 n%
                stopLossBuyingPricePer = (double)stopLossBuyingPricePerNumericUpDown1.Value;
            }
            else if (stopLossType == 1) // 이동평균선 손절
            {
                // 이평종류 0:단순, 1:지수                
                stopLossMovePriceKindType = stopLossType2MoveKindComboBox.SelectedIndex;
                // 월,주,일,분봉(0,1,2,3)
                stopLossBongType = stopLossType2DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                stopLossMinuteType = stopLossType2MoveBongKindComboBox.SelectedIndex;
                // 1,3,5,20 이평종류
                if (CheckEmpty(stopLossType2MoveLineTextBox.Text, 2)) return;
                stopLossMinuteLineType = int.Parse(stopLossType2MoveLineTextBox.Text);
                // n%
                if (CheckEmpty(stopLossType2MoveBongPerTextBox.Text, 2)) return;
                stopLossLineAccessPer = double.Parse(stopLossType2MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파
                stopLossDistance = stopLossType2DistanceComboBox.SelectedIndex;
            }
            else if (stopLossType == 2) // 볼린저벤드
            {
                if (CheckEmpty(stopLossType3Period1.Text, 2)) return;
                if (CheckEmpty(stopLossType3Period2.Text, 2)) return;
                // 기간
                stopLossStocPeriod1 = double.Parse(stopLossType3Period1.Text);
                // 승수
                stopLossBollPeriod = double.Parse(stopLossType3Period2.Text);
                // 월,주,일,분봉(0,1,2,3)
                stopLossBongType = stopLossType3DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                stopLossMinuteType = stopLossType3MoveBongKindComboBox.SelectedIndex;
                // 상한선, 중심선, 하한선(0, 1, 2)
                stopLossLine3Type = stopLossType3LineComboBox.SelectedIndex;
                // n%
                if (CheckEmpty(stopLossType3MoveBongPerTextBox.Text, 2)) return;
                stopLossLineAccessPer = double.Parse(stopLossType3MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파, 2:이탈
                stopLossDistance = stopLossType3DistanceComboBox.SelectedIndex;
            }
            else if (stopLossType == 3) // 엔벨로프
            {
                if (CheckEmpty(stopLossType4Period1.Text, 2)) return;
                if (CheckEmpty(stopLossType4Period2.Text, 2)) return;
                // 기간
                stopLossStocPeriod1 = double.Parse(stopLossType4Period1.Text);
                // 승수
                stopLossBollPeriod = double.Parse(stopLossType4Period2.Text);
                // 월,주,일,분봉(0,1,2,3)
                stopLossBongType = stopLossType4DayComboBox.SelectedIndex;
                // 1,3,5,10,20 등등 분봉
                stopLossMinuteType = stopLossType4MoveBongKindComboBox.SelectedIndex;
                // 상한선, 중심선, 하한선(0, 1, 2)
                stopLossLine3Type = stopLossType4LineComboBox.SelectedIndex;
                // n%
                if (CheckEmpty(stopLossType4MoveBongPerTextBox.Text, 2)) return;
                stopLossLineAccessPer = double.Parse(stopLossType4MoveBongPerTextBox.Text);
                // 0:근접, 1:돌파, 2:이탈
                stopLossDistance = stopLossType4DistanceComboBox.SelectedIndex;
            }

            _strStopLoss += stopLossUsing.ToString() + ";" + stopLossType.ToString() + ";" + stopLossBuyingPricePer.ToString() + ";" +
                                  stopLossMovePriceKindType.ToString() + ";" + stopLossBongType.ToString() + ";" + stopLossMinuteType.ToString() + ";" + stopLossMinuteLineType.ToString() + ";" + stopLossLineAccessPer.ToString() + ";" + stopLossDistance.ToString() + ";" +
                                  stopLossStocPeriod1.ToString() + ";" + stopLossStocPeriod2.ToString() + ";" + stopLossStocPeriod3.ToString() + ";" + stopLossBollPeriod.ToString() + ";" + stopLossLine3Type.ToString();

            /////////////////////////////////////////////// ts매도 설정 ////////////////////////////////////////////////
            int tsmedoUsing = 1; // ts매도 사용 여부
            if (tsMedoCheckBox.Checked) tsmedoUsing = 1;
            else tsmedoUsing = 0;
            int tsmedoCount = (int)tsmedocomboBox.SelectedIndex; // 0: 1차 , 1: 2차, 2: 3차
            int[] tsmedoUsingType = new int[3];
            double[] tsmedoAchievedPer = new double[3];
            double[] tsmedoPercent = new double[3];
            double[] tsmedoProportion = new double[3];

            tsmedoUsingType[0] = tsComboBox1.SelectedIndex;
            tsmedoAchievedPer[0] = (double)tsnumericUpDown1.Value;
            tsmedoPercent[0] = (double)tsMedonumericUpDown1.Value;
            tsmedoProportion[0] = (double)tspernumericUpDown1.Value;

            tsmedoUsingType[1] = tsComboBox2.SelectedIndex;
            tsmedoAchievedPer[1] = (double)tsnumericUpDown2.Value;
            tsmedoPercent[1] = (double)tsMedonumericUpDown2.Value;
            tsmedoProportion[1] = (double)tspernumericUpDown2.Value;

            tsmedoUsingType[2] = tsComboBox3.SelectedIndex;
            tsmedoAchievedPer[2] = (double)tsnumericUpDown3.Value;
            tsmedoPercent[2] = (double)tsMedonumericUpDown3.Value;
            tsmedoProportion[2] = (double)tspernumericUpDown3.Value;



            _strTsmedo += tsmedoUsing.ToString() + ";" + tsmedoCount.ToString() + ";"
                + tsmedoUsingType[0].ToString() + ";" + tsmedoAchievedPer[0] + ";" + tsmedoPercent[0] + ";" + tsmedoProportion[0] + ";"
                + tsmedoUsingType[1].ToString() + ";" + tsmedoAchievedPer[1] + ";" + tsmedoPercent[1] + ";" + tsmedoProportion[1] + ";"
                + tsmedoUsingType[2].ToString() + ";" + tsmedoAchievedPer[2] + ";" + tsmedoPercent[2] + ";" + tsmedoProportion[2] + ";";

            gMainForm.gFileIOInstance.savelastConditionList(conditionIndex, _strInvestment, _strInvestmentPrice, _strBuying, _strReBuying, _strTakeProfit, _strStopLoss, _strTsmedo);
        }
    }
}
