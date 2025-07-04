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
    public partial class HoldingItemSettingDialog : Form
    {
        public MainForm gMainForm = MainForm.GetInstance();

        public string[] tempHoldingitemListData = null;
        public HoldingItemSettingDialog()
        {
            InitializeComponent();

            this.Size = new Size(717, 765);
            this.MinimumSize = new Size(717, 765);
            this.MaximumSize = new Size(717, 765);

            // 실행시 마지막으로 지정한 값을 불러오기
            this.Load += new EventHandler(HoldingItemSettingDialog_load);

            this.takeProfitCheckBox.CheckedChanged += new System.EventHandler(this.takeProfitCheckBox_CheckedChanged);
            this.stopLossCheckBox.CheckedChanged += new System.EventHandler(this.stopLossCheckBox_CheckedChanged);
            this.tsMedoCheckBox.CheckedChanged += new System.EventHandler(this.tsmedoCheckBox_CheckedChanged);
            this.reBuyingCheckBox.CheckedChanged += new System.EventHandler(this.reBuyingCheckBox_CheckedChanged);

            this.investBuyingCountComboBox.SelectedIndexChanged += new System.EventHandler(this.investBuyingCountComboBox_CheckedChanged);
            this.tsmedocomboBox.SelectedIndexChanged += new System.EventHandler(this.tsmedocomboBox_SelectedIndexChanged);
            this.takeProfitComboBox.SelectedIndexChanged += new System.EventHandler(this.takeProfitcomboBox_SelectedIndexChanged);
            this.stopLossComboBox.SelectedIndexChanged += new System.EventHandler(this.stopLosscomboBox_SelectedIndexChanged);
            // ts매도 콤보박스에 따라 visble처리
            this.tsComboBox1.SelectedIndexChanged += new System.EventHandler(this.tsComboBox1_SelectedIndexChanged);
            this.tsComboBox2.SelectedIndexChanged += new System.EventHandler(this.tsComboBox2_SelectedIndexChanged);
            this.tsComboBox3.SelectedIndexChanged += new System.EventHandler(this.tsComboBox3_SelectedIndexChanged);


            // 저장하기 버튼 이벤트 등록
            conditionSaveButton.Click += ButtonClickEvent;
            // TextBox changed 이벤트 등록

            /* ts매도 콤보박스에 따라 visble처리
            tsComboBox1.SelectedIndexChanged += tsComboBox1_SelectedIndexChanged;
            tsComboBox2.SelectedIndexChanged += tsComboBox2_SelectedIndexChanged;
            tsComboBox3.SelectedIndexChanged += tsComboBox3_SelectedIndexChanged;
            tsmedocomboBox.SelectedIndexChanged += tsmedocomboBox_SelectedIndexChanged;
            // 익절,손절 인덱스에따라 visble처리
            stopLossComboBox.SelectedIndexChanged += stopLosscomboBox_SelectedIndexChanged;
            takeProfitComboBox.SelectedIndexChanged += takeProfitcomboBox_SelectedIndexChanged;
            mesuoption1comboBox.SelectedIndexChanged += mesuoption1comboBox_SelectedIndexChanged;*/

            int _buyingCount = investBuyingCountComboBox.SelectedIndex; // 추매포함 매수횟수
            int _tsmedoCount = tsmedocomboBox.SelectedIndex;
            int _takeProfitCount = takeProfitComboBox.SelectedIndex;
            int _stopLossCount = stopLossComboBox.SelectedIndex;

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

            if (_buyingCount == 0)
            {
                //investMoneyTextBox는 매수횟수의 TextBox
                investMoneyTextBox_2.ReadOnly = false;
                investMoneyTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
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
                reBuyingType1InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
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
            }


            investMoneyTextBox_2.KeyPress += Textbox_Key_Press;
            investMoneyTextBox_3.KeyPress += Textbox_Key_Press;
            investMoneyTextBox_4.KeyPress += Textbox_Key_Press;
            investMoneyTextBox_5.KeyPress += Textbox_Key_Press;
            investMoneyTextBox_6.KeyPress += Textbox_Key_Press;

            reBuyingType1InvestPricePerTextBox_2.KeyPress += Textbox_Key_Press4;
            reBuyingType1InvestPricePerTextBox_3.KeyPress += Textbox_Key_Press4;
            reBuyingType1InvestPricePerTextBox_4.KeyPress += Textbox_Key_Press4;
            reBuyingType1InvestPricePerTextBox_5.KeyPress += Textbox_Key_Press4;
            reBuyingType1InvestPricePerTextBox_6.KeyPress += Textbox_Key_Press4;

            reBuyingType1InvestPricePerTextBox_2.LostFocus += LostFocusEvent;
            reBuyingType1InvestPricePerTextBox_3.LostFocus += LostFocusEvent;
            reBuyingType1InvestPricePerTextBox_4.LostFocus += LostFocusEvent;
            reBuyingType1InvestPricePerTextBox_5.LostFocus += LostFocusEvent;
            reBuyingType1InvestPricePerTextBox_6.LostFocus += LostFocusEvent;

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
            if (sender.Equals(conditionSaveButton))
            {
                string _strInvestment = string.Empty, _strInvestmentPrice = string.Empty,  _strReBuying = string.Empty, _strTakeProfit = string.Empty, _strStopLoss = string.Empty, _strTsmedo = string.Empty;

                // 계좌번호
                string account = gMainForm.myAccountComboBox.Text;

                _strInvestment += account;

                //////////////////////////////////////////// 매수금액설정 /////////////////////////////////////////////////////
                // 저장값
                // 매수횟수, 회차별 투자금 buyingInvestment[0],buyingInvestment[1],buyingInvestment[2],buyingInvestment[3],buyingInvestment[4],buyingInvestment[5],
                int rebuyingUsing = 1; // 추가매수 여부
                if (reBuyingCheckBox.Checked) rebuyingUsing = 1;
                else rebuyingUsing = 0;
                int buyingCount = (int)investBuyingCountComboBox.SelectedIndex; // 매수회수(추매 포함) 0: 1차, 1: 2차, 2: 3차, 3: 4차, 4: 5차
                double[] reBuyingPer = new double[5]; // 추매 %

                if (buyingCount > 1)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_2.Text, 0)) return;
                }
                if (buyingCount > 2)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_3.Text, 0)) return;
                }
                if (buyingCount > 3)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_4.Text, 0)) return;
                }
                if (buyingCount > 4)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_5.Text, 0)) return;
                }
                if (buyingCount > 5)
                {
                    if (CheckEmpty(reBuyingType1InvestPricePerTextBox_6.Text, 0)) return;
                }
                reBuyingPer[0] = double.Parse(reBuyingType1InvestPricePerTextBox_2.Text);
                reBuyingPer[1] = double.Parse(reBuyingType1InvestPricePerTextBox_3.Text);
                reBuyingPer[2] = double.Parse(reBuyingType1InvestPricePerTextBox_4.Text);
                reBuyingPer[3] = double.Parse(reBuyingType1InvestPricePerTextBox_5.Text);
                reBuyingPer[4] = double.Parse(reBuyingType1InvestPricePerTextBox_6.Text);

                _strReBuying += rebuyingUsing + ";" + buyingCount + ";" + reBuyingPer[0] + ";" + reBuyingPer[1] + ";" + reBuyingPer[2] + ";" + reBuyingPer[3] + ";" + reBuyingPer[4];


                /////////////////////////////////////////////// 추매 설정 ///////////////////////////////////////////////

                string investMoney_2 = investMoneyTextBox_2.Text; // 추가 매수 비중
                string investMoney_3 = investMoneyTextBox_3.Text;
                string investMoney_4 = investMoneyTextBox_4.Text;
                string investMoney_5 = investMoneyTextBox_5.Text;
                string investMoney_6 = investMoneyTextBox_6.Text;

                if (buyingCount > 1)
                {
                    if (double.Parse(investMoney_2.Replace(",", "")) <= 0)
                    {
                        MessageBox.Show("1차 추매 매수 금액을 설정해 주세요.");
                        return;
                    }
                }
                if (buyingCount > 2)
                {
                    if (double.Parse(investMoney_3.Replace(",", "")) <= 0)
                    {
                        MessageBox.Show("2차 추매 매수 금액을 설정해 주세요.");
                        return;
                    }
                }
                if (buyingCount > 3)
                {
                    if (double.Parse(investMoney_4.Replace(",", "")) <= 0)
                    {
                        MessageBox.Show("3차 추매 매수 금액을 설정해 주세요.");
                        return;
                    }
                }
                if (buyingCount > 4)
                {
                    if (double.Parse(investMoney_5.Replace(",", "")) <= 0)
                    {
                        MessageBox.Show("4차 추매 매수 금액을 설정해 주세요.");
                        return;
                    }
                }
                if (buyingCount > 5)
                {
                    if (double.Parse(investMoney_6.Replace(",", "")) <= 0)
                    {
                        MessageBox.Show("5차 추매 매수 금액을 설정해 주세요.");
                        return;
                    }
                }
                _strInvestmentPrice +=  investMoney_2 + ";" + investMoney_3 + ";" + investMoney_4 + ";" + investMoney_5 + ";" + investMoney_6;


                /////////////////////////////////////////////// 익절 설정 ////////////////////////////////////////////////
                int takeProfitUsing = 1; // 익절 사용 여부
                if (takeProfitCheckBox.Checked) takeProfitUsing = 1;
                else takeProfitUsing = 0;
                int takeProfitCount = (int)takeProfitComboBox.SelectedIndex;// 0: 1차, 1: 2차, 2: 3차, 3: 4차, 4: 5차
                double[] takeProfitBuyingPricePer = new double[5]; // 익절 %
                double[] takeProfitProportion = new double[5]; // 익절비중
                                                               // 이동평균 근접, 돌파 공통 사용        

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

                _strTakeProfit += takeProfitUsing.ToString() + ";"+ takeProfitCount.ToString() + ";" +
                                takeProfitBuyingPricePer[0].ToString() + ";" + takeProfitProportion[0].ToString() + ";" + takeProfitBuyingPricePer[1].ToString() + ";" + takeProfitProportion[1].ToString() + ";" + takeProfitBuyingPricePer[2].ToString() + ";" + takeProfitProportion[2].ToString() + ";" + takeProfitBuyingPricePer[3].ToString() + ";" + takeProfitProportion[3].ToString() + ";" + takeProfitBuyingPricePer[4].ToString() + ";" + takeProfitProportion[4].ToString();

                /////////////////////////////////////////////// 손절 설정 ////////////////////////////////////////////////
                int stopLossUsing = 1; // 손절 사용 여부
                if (stopLossCheckBox.Checked) stopLossUsing = 1;
                else stopLossUsing = 0;
                int stopLossCount = (int)stopLossComboBox.SelectedIndex;// 0: 1차, 1: 2차, 2: 3차, 3: 4차, 4: 5차
                
                double[] stopLossBuyingPricePer = new double[5]; // 손절 %
                double[] stopLossProportion = new double[5]; // 손절비중
                                                             // 이동평균 근접, 이탈 공통 사용        



                stopLossBuyingPricePer[0] = (double)stopLossBuyingPricePerNumericUpDown1.Value;
                stopLossProportion[0] = (double)stopLossPerNumericUpDown1.Value;

                stopLossBuyingPricePer[1] = (double)stopLossBuyingPricePerNumericUpDown2.Value;
                stopLossProportion[1] = (double)stopLossPerNumericUpDown2.Value;

                stopLossBuyingPricePer[2] = (double)stopLossBuyingPricePerNumericUpDown3.Value;
                stopLossProportion[2] = (double)stopLossPerNumericUpDown3.Value;

                stopLossBuyingPricePer[3] = (double)stopLossBuyingPricePerNumericUpDown4.Value;
                stopLossProportion[3] = (double)stopLossPerNumericUpDown4.Value;

                stopLossBuyingPricePer[4] = (double)stopLossBuyingPricePerNumericUpDown5.Value;
                stopLossProportion[4] = (double)stopLossPerNumericUpDown5.Value;

                _strStopLoss += stopLossUsing.ToString() + ";"+ stopLossCount.ToString() + ";" +
                                stopLossBuyingPricePer[0].ToString() + ";" + stopLossProportion[0].ToString() + ";" + stopLossBuyingPricePer[1].ToString() + ";" + stopLossProportion[1].ToString() + ";" + stopLossBuyingPricePer[2].ToString() + ";" + stopLossProportion[2].ToString() + ";" + stopLossBuyingPricePer[3].ToString() + ";" + stopLossProportion[3].ToString() + ";" + stopLossBuyingPricePer[4].ToString() + ";" + stopLossProportion[4].ToString();

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
                    + tsmedoUsingType[1].ToString() + ";" + tsmedoAchievedPer[1] + ";" + tsmedoPercent[1] + ";" + tsmedoProportion[1] + ";"
                    + tsmedoUsingType[2].ToString() + ";" + tsmedoAchievedPer[2] + ";" + tsmedoPercent[2] + ";" + tsmedoProportion[2] + ";" + tsProfitPreservation1.ToString() + ";" + tsProfitPreservation2.ToString() + ";" + tsProfitPreservation3.ToString();

                string[] _holdingItemOptionData = gMainForm.gFileIOInstance.getHoldingItemListData();
                int _holdingItemOptionCount = _holdingItemOptionData.Length / (int)Save.holdingItem;
                /*
                for (int k = 0; k < _holdingItemOptionCount; k++)
                {
                    string strInvestment = _holdingItemOptionData[k * (int)Save.holdingItem];
                    string strReBuying = _holdingItemOptionData[k * (int)Save.holdingItem + 1];
                    string strInvestmentPrice = _holdingItemOptionData[k * (int)Save.holdingItem + 2];
                    string strTakeProfit = _holdingItemOptionData[k * (int)Save.holdingItem + 3];
                    string strStopLoss = _holdingItemOptionData[k * (int)Save.holdingItem + 4];
                    string strTsmedo = _holdingItemOptionData[k * (int)Save.holdingItem + 5];
                }
                */
                MyHoldingItemOption mho= gMainForm.gMyHoldingItemOptionList.Find(o => o.account == account /* 또는 division */);
                if (mho != null)
                {
                    // 기존 객체가 있으면 reSet 메서드로 설정 갱신
                    mho.reSetMyHoldingItemOption(
                        _strInvestment,
                        _strInvestmentPrice,
                        _strReBuying,
                        _strTakeProfit,
                        _strStopLoss,
                        _strTsmedo
                    );
                    foreach(MyHoldingItemOption _mho in gMainForm.gMyHoldingItemOptionList)
                    {
                        foreach(MyHoldingItem _mhi in _mho.MyHoldingItemList)
                        {
                            if(_mhi != null)
                            {
                                _mhi.reSetOptionData(_mho);
                               
                            }
                        }
                    }
                    gMainForm.gFileIOInstance.saveHoldingitem(_strInvestment, _strInvestmentPrice, _strReBuying, _strTakeProfit, _strStopLoss, _strTsmedo);
                }
                else
                {
                    gMainForm.gFileIOInstance.saveHoldingitem(_strInvestment, _strInvestmentPrice, _strReBuying, _strTakeProfit, _strStopLoss, _strTsmedo);
                    string screenNumber = gMainForm.GetScreenNumber();
                    MyHoldingItemOption _mho = new MyHoldingItemOption(_strInvestment, _strInvestmentPrice, _strReBuying, _strTakeProfit, _strStopLoss, _strTsmedo, screenNumber);
                    gMainForm.gMyHoldingItemOptionList.Add(_mho);
                }
            }
        }
        private void HoldingItemSettingDialog_load(object sender, EventArgs e)
        {
            tempHoldingitemListData = gMainForm.gFileIOInstance.getHoldingItemListData();

            if(tempHoldingitemListData != null)
            {
                string _tempStr = string.Empty;
                string[] _strInvestmentPrice, _strReBuying, _strTakeProfit, _strStopLoss, _strTsmedo;

                /////////////////////////////////////////////// 추매 설정 ///////////////////////////////////////////////
                _tempStr = tempHoldingitemListData[0 * (int)Save.holdingItem + 1];
                _strInvestmentPrice = _tempStr.Split(';');
                double _value1 = 0, _value2 = 0, _value3 = 0, _value4 = 0, _value5 = 0;
                _value1 = double.Parse(_strInvestmentPrice[0]);
                _value2 = double.Parse(_strInvestmentPrice[1]);
                _value3 = double.Parse(_strInvestmentPrice[2]);
                _value4 = double.Parse(_strInvestmentPrice[3]);
                _value5 = double.Parse(_strInvestmentPrice[4]);

                gMainForm.HolditemDig.investMoneyTextBox_2.Text = _value1.ToString();
                gMainForm.HolditemDig.investMoneyTextBox_3.Text = _value2.ToString();
                gMainForm.HolditemDig.investMoneyTextBox_4.Text = _value3.ToString();
                gMainForm.HolditemDig.investMoneyTextBox_5.Text = _value4.ToString();
                gMainForm.HolditemDig.investMoneyTextBox_6.Text = _value5.ToString();
                //////////////////////////////////////////// 매수금액설정 /////////////////////////////////////////////////////
                _tempStr = tempHoldingitemListData[0 * (int)Save.holdingItem + 2];
                _strReBuying = _tempStr.Split(';');
                int _rebuyingUsing = int.Parse(_strReBuying[0]);
                int _investBuyingCount = int.Parse(_strReBuying[1]);
                gMainForm.HolditemDig.investBuyingCountComboBox.SelectedIndex = _investBuyingCount;
                _value1 = double.Parse(_strReBuying[2]);
                _value2 = double.Parse(_strReBuying[3]);
                _value3 = double.Parse(_strReBuying[4]);
                _value4 = double.Parse(_strReBuying[5]);
                _value5 = double.Parse(_strReBuying[6]);
                gMainForm.HolditemDig.reBuyingType1InvestPricePerTextBox_2.Text = _value1.ToString();
                gMainForm.HolditemDig.reBuyingType1InvestPricePerTextBox_3.Text = _value2.ToString();
                gMainForm.HolditemDig.reBuyingType1InvestPricePerTextBox_4.Text = _value3.ToString();
                gMainForm.HolditemDig.reBuyingType1InvestPricePerTextBox_5.Text = _value4.ToString();
                gMainForm.HolditemDig.reBuyingType1InvestPricePerTextBox_6.Text = _value5.ToString();
                if (_rebuyingUsing == 1)
                {
                    gMainForm.HolditemDig.reBuyingCheckBox.Checked = true;
                    //화면ui세팅
                    gMainForm.HolditemDig.groupBox1.Visible = true;
                    gMainForm.HolditemDig.groupBox1.BringToFront();
                    gMainForm.HolditemDig.reBuyingType1GroupBox.Visible = true;
                    //gMainForm.HolditemDig.reBuyingType1GroupBox.BringToFront();
                    gMainForm.HolditemDig.rebuyingNotUsedGroupBox1.Visible = false;
                }
                else
                {
                    gMainForm.HolditemDig.reBuyingCheckBox.Checked = false;
                    //화면ui세팅
                    gMainForm.HolditemDig.groupBox1.Visible = false;
                    gMainForm.HolditemDig.reBuyingType1GroupBox.Visible = false;
                    gMainForm.HolditemDig.rebuyingNotUsedGroupBox1.Visible = true;
                    gMainForm.HolditemDig.rebuyingNotUsedGroupBox1.BringToFront();
                    gMainForm.HolditemDig.rebuyingNotUsedGroupBox1.Location = new Point(19, 66);
                    gMainForm.HolditemDig.rebuyingNotUsedGroupBox2.BringToFront();
                    gMainForm.HolditemDig.rebuyingNotUsedGroupBox2.Location = new Point(19, 317);
                }
                //////////////////////////////////////////////////////////////////////////////// 익절 설정 //////////////////////////////////////////////////////////////////////////////
                _tempStr = tempHoldingitemListData[0 * (int)Save.holdingItem + 3];
                _strTakeProfit = _tempStr.Split(';');
                int _takeProfitUsing = int.Parse(_strTakeProfit[0]);
                int _takeProfitCount = int.Parse(_strTakeProfit[1]);
                gMainForm.HolditemDig.takeProfitComboBox.SelectedIndex = _takeProfitCount;
                double[] _takeProfitBuyingPricePer = new double[5];
                double[] _takeProfitProportion = new double[5];
                _takeProfitBuyingPricePer[0] = double.Parse(_strTakeProfit[2]);
                _takeProfitProportion[0] = double.Parse(_strTakeProfit[3]);
                gMainForm.HolditemDig.takeProfitBuyingPricePerNumericUpDown1.Value = Convert.ToDecimal(_takeProfitBuyingPricePer[0]);
                gMainForm.HolditemDig.takeProfitPerNumericUpDown1.Value = Convert.ToDecimal(_takeProfitProportion[0]);
                _takeProfitBuyingPricePer[1] = double.Parse(_strTakeProfit[4]);
                _takeProfitProportion[1] = double.Parse(_strTakeProfit[5]);
                gMainForm.HolditemDig.takeProfitBuyingPricePerNumericUpDown2.Value = Convert.ToDecimal(_takeProfitBuyingPricePer[1]);
                gMainForm.HolditemDig.takeProfitPerNumericUpDown2.Value = Convert.ToDecimal(_takeProfitProportion[1]);
                _takeProfitBuyingPricePer[2] = double.Parse(_strTakeProfit[6]);
                _takeProfitProportion[2] = double.Parse(_strTakeProfit[7]);
                gMainForm.HolditemDig.takeProfitBuyingPricePerNumericUpDown3.Value = Convert.ToDecimal(_takeProfitBuyingPricePer[2]);
                gMainForm.HolditemDig.takeProfitPerNumericUpDown3.Value = Convert.ToDecimal(_takeProfitProportion[2]);
                _takeProfitBuyingPricePer[3] = double.Parse(_strTakeProfit[8]);
                _takeProfitProportion[3] = double.Parse(_strTakeProfit[9]);
                gMainForm.HolditemDig.takeProfitBuyingPricePerNumericUpDown4.Value = Convert.ToDecimal(_takeProfitBuyingPricePer[3]);
                gMainForm.HolditemDig.takeProfitPerNumericUpDown4.Value = Convert.ToDecimal(_takeProfitProportion[3]);
                _takeProfitBuyingPricePer[4] = double.Parse(_strTakeProfit[10]);
                _takeProfitProportion[4] = double.Parse(_strTakeProfit[11]);
                gMainForm.HolditemDig.takeProfitBuyingPricePerNumericUpDown5.Value = Convert.ToDecimal(_takeProfitBuyingPricePer[4]);
                gMainForm.HolditemDig.takeProfitPerNumericUpDown5.Value = Convert.ToDecimal(_takeProfitProportion[4]);
                if (_takeProfitUsing == 1)
                {
                    gMainForm.HolditemDig.takeProfitCheckBox.Checked = true;
                    gMainForm.HolditemDig.takeProfitType1GroupBox.Visible = true;
                    gMainForm.HolditemDig.takeProfitNotUsedGroupBox.Visible = false;
                }
                else
                {
                    gMainForm.HolditemDig.takeProfitCheckBox.Checked = false;
                    gMainForm.HolditemDig.takeProfitType1GroupBox.Visible = false;
                    gMainForm.HolditemDig.takeProfitNotUsedGroupBox.Visible = true;
                    gMainForm.HolditemDig.takeProfitNotUsedGroupBox.BringToFront();
                    gMainForm.HolditemDig.takeProfitNotUsedGroupBox.Location = new Point(356, 72);

                }
                //////////////////////////////////////////////////////////////////////////////// 손절 설정 //////////////////////////////////////////////////////////////////////////////
                _tempStr = tempHoldingitemListData[0 * (int)Save.holdingItem + 4];
                _strStopLoss = _tempStr.Split(';');
                int _stopLossUsing = int.Parse(_strStopLoss[0]);
                int _stopLossCount = int.Parse(_strStopLoss[1]);
                gMainForm.HolditemDig.stopLossComboBox.SelectedIndex = _stopLossCount;
                double[] _stopLossBuyingPricePer = new double[5];
                double[] _stopLossProportion = new double[5];
                _stopLossBuyingPricePer[0] = double.Parse(_strStopLoss[2]);
                _stopLossProportion[0] = double.Parse(_strStopLoss[3]);
                gMainForm.HolditemDig.stopLossBuyingPricePerNumericUpDown1.Value = Convert.ToDecimal(_stopLossBuyingPricePer[0]);
                gMainForm.HolditemDig.stopLossPerNumericUpDown1.Value = Convert.ToDecimal(_stopLossProportion[0]);
                _stopLossBuyingPricePer[1] = double.Parse(_strStopLoss[4]);
                _stopLossProportion[1] = double.Parse(_strStopLoss[5]);
                gMainForm.HolditemDig.stopLossBuyingPricePerNumericUpDown2.Value = Convert.ToDecimal(_stopLossBuyingPricePer[1]);
                gMainForm.HolditemDig.stopLossPerNumericUpDown2.Value = Convert.ToDecimal(_stopLossProportion[1]);
                _stopLossBuyingPricePer[2] = double.Parse(_strStopLoss[6]);
                _stopLossProportion[2] = double.Parse(_strStopLoss[7]);
                gMainForm.HolditemDig.stopLossBuyingPricePerNumericUpDown3.Value = Convert.ToDecimal(_stopLossBuyingPricePer[2]);
                gMainForm.HolditemDig.stopLossPerNumericUpDown3.Value = Convert.ToDecimal(_stopLossProportion[2]);
                _stopLossBuyingPricePer[3] = double.Parse(_strStopLoss[8]);
                _stopLossProportion[3] = double.Parse(_strStopLoss[9]);
                gMainForm.HolditemDig.stopLossBuyingPricePerNumericUpDown4.Value = Convert.ToDecimal(_stopLossBuyingPricePer[3]);
                gMainForm.HolditemDig.stopLossPerNumericUpDown4.Value = Convert.ToDecimal(_stopLossProportion[3]);
                _stopLossBuyingPricePer[4] = double.Parse(_strStopLoss[10]);
                _stopLossProportion[4] = double.Parse(_strStopLoss[11]);
                gMainForm.HolditemDig.stopLossBuyingPricePerNumericUpDown5.Value = Convert.ToDecimal(_stopLossBuyingPricePer[4]);
                gMainForm.HolditemDig.stopLossPerNumericUpDown5.Value = Convert.ToDecimal(_stopLossProportion[4]);
                if (_stopLossUsing == 1)
                {
                    gMainForm.HolditemDig.stopLossCheckBox.Checked = true;
                    gMainForm.HolditemDig.stopLossType1GroupBox.Visible = true;
                    gMainForm.HolditemDig.stopLossNotUsedGroupBox.Visible = false;
                }
                else
                {
                    gMainForm.HolditemDig.stopLossCheckBox.Checked = false;
                    gMainForm.HolditemDig.stopLossType1GroupBox.Visible = false;
                    gMainForm.HolditemDig.stopLossNotUsedGroupBox.Visible = true;
                    gMainForm.HolditemDig.stopLossNotUsedGroupBox.BringToFront();
                    gMainForm.HolditemDig.stopLossNotUsedGroupBox.Location = new Point(356, 318);

                }
                /////////////////////////////////////////////// ts매도 설정 ////////////////////////////////////////////////
                _tempStr = tempHoldingitemListData[0 * (int)Save.holdingItem + 5];
                _strTsmedo = _tempStr.Split(';');
                int _tsmedoUsing = int.Parse(_strTsmedo[0]);
                int _tsmedoCount = int.Parse(_strTsmedo[1]);
                gMainForm.HolditemDig.tsmedocomboBox.SelectedIndex = _tsmedoCount;
                int[] _tsmedoUsingType = new int[3];
                double[] _tsmedoAchievedPer = new double[3];
                double[] _tsmedoPercent = new double[3];
                double[] _tsmedoProportion = new double[3];
                _tsmedoUsingType[0] = int.Parse(_strTsmedo[2]);
                _tsmedoAchievedPer[0] = double.Parse(_strTsmedo[3]);
                _tsmedoPercent[0] = double.Parse(_strTsmedo[4]);
                _tsmedoProportion[0] = double.Parse(_strTsmedo[5]);
                gMainForm.HolditemDig.tsComboBox1.SelectedIndex = _tsmedoUsingType[0];
                gMainForm.HolditemDig.tsnumericUpDown1.Value = Convert.ToDecimal(_tsmedoAchievedPer[0]);
                gMainForm.HolditemDig.tsMedonumericUpDown1.Value = Convert.ToDecimal(_tsmedoPercent[0]);
                gMainForm.HolditemDig.tspernumericUpDown1.Value = Convert.ToDecimal(_tsmedoProportion[0]);

                _tsmedoUsingType[1] = int.Parse(_strTsmedo[6]);
                _tsmedoAchievedPer[1] = double.Parse(_strTsmedo[7]);
                _tsmedoPercent[1] = double.Parse(_strTsmedo[8]);
                _tsmedoProportion[1] = double.Parse(_strTsmedo[9]);
                gMainForm.HolditemDig.tsComboBox2.SelectedIndex = _tsmedoUsingType[1];
                gMainForm.HolditemDig.tsnumericUpDown2.Value = Convert.ToDecimal(_tsmedoAchievedPer[1]);
                gMainForm.HolditemDig.tsMedonumericUpDown2.Value = Convert.ToDecimal(_tsmedoPercent[1]);
                gMainForm.HolditemDig.tspernumericUpDown2.Value = Convert.ToDecimal(_tsmedoProportion[1]);

                _tsmedoUsingType[2] = int.Parse(_strTsmedo[10]);
                _tsmedoAchievedPer[2] = double.Parse(_strTsmedo[11]);
                _tsmedoPercent[2] = double.Parse(_strTsmedo[12]);
                _tsmedoProportion[2] = double.Parse(_strTsmedo[13]);
                gMainForm.HolditemDig.tsComboBox3.SelectedIndex = _tsmedoUsingType[2];
                gMainForm.HolditemDig.tsnumericUpDown3.Value = Convert.ToDecimal(_tsmedoAchievedPer[2]);
                gMainForm.HolditemDig.tsMedonumericUpDown3.Value = Convert.ToDecimal(_tsmedoPercent[2]);
                gMainForm.HolditemDig.tspernumericUpDown3.Value = Convert.ToDecimal(_tsmedoProportion[2]);

                int _tsProfitPreservation1 = int.Parse(_strTsmedo[14]);
                if(_tsProfitPreservation1 == 1)
                {
                    gMainForm.HolditemDig.tsProfitPreservationCheckBox1.Checked = true;
                }
                int _tsProfitPreservation2 = int.Parse(_strTsmedo[15]);
                if (_tsProfitPreservation2 == 1)
                {
                    gMainForm.HolditemDig.tsProfitPreservationCheckBox2.Checked = true;
                }
                int _tsProfitPreservation3 = int.Parse(_strTsmedo[16]);
                if (_tsProfitPreservation3 == 1)
                {
                    gMainForm.HolditemDig.tsProfitPreservationCheckBox3.Checked = true;
                }

                if (_tsmedoUsing == 1)
                {
                    gMainForm.HolditemDig.tsMedoCheckBox.Checked = true;
                    gMainForm.HolditemDig.groupBox2.Visible = true;
                    gMainForm.HolditemDig.tsmedoNotUsedGroupBox.Visible = false;
                }
                else
                {
                    gMainForm.HolditemDig.tsMedoCheckBox.Checked = false;
                    gMainForm.HolditemDig.groupBox2.Visible = false;
                    gMainForm.HolditemDig.tsmedoNotUsedGroupBox.Visible = true;
                    gMainForm.HolditemDig.tsmedoNotUsedGroupBox.BringToFront();
                    gMainForm.HolditemDig.tsmedoNotUsedGroupBox.Location = new Point(42, 552);

                }

            }
        }

        private void investBuyingCountComboBox_CheckedChanged(object sender, EventArgs e)
        {
            int _buyingCount = investBuyingCountComboBox.SelectedIndex; // 추매포함 매수횟수
            if (_buyingCount == 0)
            {
                //investMoneyTextBox는 매수횟수의 TextBox
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

                //reBuyingType1InvestPricePerTextBox 는 추매 방식설정의 TextBox
                reBuyingType1InvestPricePerTextBox_2.ReadOnly = false;
                reBuyingType1InvestPricePerTextBox_2.BackColor = Color.FromArgb(255, 255, 255);
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
            }
            else if (_buyingCount == 1)
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
            }
            else if (_buyingCount == 2)
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
                reBuyingType1InvestPricePerTextBox_4.Text = "-9";
                reBuyingType1InvestPricePerTextBox_5.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_5.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_5.Text = "-12";
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_6.Text = "-15";
            }
            else if (_buyingCount == 3)
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
                reBuyingType1InvestPricePerTextBox_5.Text = "-12";
                reBuyingType1InvestPricePerTextBox_6.ReadOnly = true;
                reBuyingType1InvestPricePerTextBox_6.BackColor = Color.FromArgb(240, 240, 240);
                reBuyingType1InvestPricePerTextBox_6.Text = "-15";
            }
            else if (_buyingCount == 4)
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
                reBuyingType1InvestPricePerTextBox_6.Text = "-15";
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
        private void takeProfitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (takeProfitCheckBox.Checked)
            {
                takeProfitNotUsedGroupBox.Visible = false;
                takeProfitType1GroupBox.Visible = true;
            }
            else
            {
                takeProfitType1GroupBox.Visible = false;
                takeProfitNotUsedGroupBox.Visible = true;
                takeProfitNotUsedGroupBox.BringToFront();
                takeProfitNotUsedGroupBox.Location = new Point(356, 72);
            }
        }
        private void stopLossCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (stopLossCheckBox.Checked)
            {
                stopLossNotUsedGroupBox.Visible = false;
                stopLossType1GroupBox.Visible = true;
            }
            else
            {
                stopLossType1GroupBox.Visible = false;
                stopLossNotUsedGroupBox.Visible = true;
                stopLossNotUsedGroupBox.BringToFront();
                stopLossNotUsedGroupBox.Location = new Point(356, 318);
            }
        }
        private void tsmedoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (tsMedoCheckBox.Checked)
            {
                tsmedoNotUsedGroupBox.Visible = false;
                tsmedocomboBox.Visible = true;
                groupBox2.Visible = true;
            }
            else
            {
                groupBox2.Visible = false;
                tsmedoNotUsedGroupBox.Visible = true;
                tsmedoNotUsedGroupBox.BringToFront();
                tsmedoNotUsedGroupBox.Location = new Point(42, 552);
                tsmedocomboBox.Visible = false;
            }
        }
        private void reBuyingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (reBuyingCheckBox.Checked)
            {
                rebuyingNotUsedGroupBox1.Visible = false;
                rebuyingNotUsedGroupBox2.Visible = false;
                investBuyingCountComboBox.Visible = true;
                reBuyingType1GroupBox.Visible = true;
                groupBox1.Visible = true;
            }
            else
            {
                rebuyingNotUsedGroupBox1.Visible = true;
                rebuyingNotUsedGroupBox2.Visible = true;
                rebuyingNotUsedGroupBox1.BringToFront();
                rebuyingNotUsedGroupBox2.BringToFront();
                rebuyingNotUsedGroupBox1.Location = new Point(19, 66);
                rebuyingNotUsedGroupBox2.Location = new Point(19, 317);
                investBuyingCountComboBox.Visible = false;
            }
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
        private void Textbox_Key_Press(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8)
            {
                e.Handled = true;
                return;

            }
            if (e.KeyChar == 8) // (char)Keys.Back
                return;

            // (3) 지금 입력하려는 e.KeyChar를 반영했을 때
            //     최종적으로 어떤 문자열이 될지 시뮬레이션
            TextBox textBox = sender as TextBox;

            // 사용자가 현재 드래그해서 범위선택 중이라면 그 부분을 제거 후 새 문자 삽입된다고 가정
            string oldText = textBox.Text;
            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;

            // 3-1) 기존 문자열에서 선택영역(selectionLength)만큼 지우기
            string newText = oldText.Remove(selectionStart, selectionLength);
            // 3-2) 해당 위치에 이번에 입력한 e.KeyChar(1글자)를 삽입
            newText = newText.Insert(selectionStart, e.KeyChar.ToString());

            // (4) int 변환해서 0~100 범위를 넘어서는지 검사
            if (int.TryParse(newText, out int newValue))
            {
                if (newValue < 0 || newValue > 1001)
                {
                    // 범위를 벗어나면 입력 자체를 무효화
                    e.Handled = true;
                }
            }
            else
            {
                // 숫자로 변환이 안 되면(빈문자열 등) 막기
                e.Handled = true;
            }
        }
        private void Textbox_Key_Press4(object sender, KeyPressEventArgs e)
        {

            TextBox textBox = sender as TextBox;

            // -----------------------------
            // 1) 숫자 / '.' / '-' / 백스페이스(8) 이외 입력 차단
            // -----------------------------
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != '.' && e.KeyChar != 8 && e.KeyChar != '-')
            {
                e.Handled = true;
                return;
            }

            // -----------------------------
            // 2) 백스페이스(삭제)는 바로 통과
            // -----------------------------
            if (e.KeyChar == 8)
                return;

            // -----------------------------
            // 선택 상태 및 기존 문자열 분석
            // -----------------------------
            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;
            string oldText = textBox.Text;

            // -----------------------------
            // 3) '-'(마이너스) 입력 처리
            // -----------------------------
            if (e.KeyChar == '-')
            {
                // 3-1) 이미 문자열에 '-'가 있는지 확인
                bool alreadyHasMinus = oldText.Contains('-');

                // (A) 전체가 선택되어 있는 경우(예: 기존 "-12" 전부 선택 후 '-' 다시 입력)
                //     -> 기존 내용을 다 지우고 '-'만 남기는 것을 허용
                if (selectionLength == oldText.Length)
                {
                    // 이 경우는 이미 '-'가 있든 없든 전부 선택했으므로
                    // "새로 '-' 하나로 교체"하는 것을 허용
                    // 별도 로직 없이 그냥 진행하면 아래에서 TryParse() 검사로 넘어갑니다.
                    return;
                }
                else
                {
                    // (B) 전체선택이 아닌데 이미 '-'가 존재 -> 중복 입력 막기
                    if (alreadyHasMinus)
                    {
                        e.Handled = true;
                        return;
                    }

                    // (C) 기존에 '-'가 없는데 지금 '-'를 입력하려는 경우
                    //     -> 일반적으로 맨 앞(SelectionStart=0)에서만 허용
                    //        (단, 부분선택/부분수정 등은 허용 범위를 어떻게 할지 프로젝트 상황에 따라 달라질 수 있음)

                    // 여기서는 "음수부호는 무조건 첫 문자"만 허용한다 가정
                    if (selectionStart != 0)
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }

            // -----------------------------
            // 4) '.'(소수점) 입력 처리
            // -----------------------------
            if (e.KeyChar == '.')
            {
                // 이미 소수점이 있는지 확인
                if (oldText.Contains('.') && selectionLength != oldText.Length)
                {
                    // 전체선택이라면 문자열이 전부 교체되므로 중복 허용 가능하지만,
                    // 부분선택의 경우엔 이미 '.'이 있으므로 또 입력은 막는다.
                    e.Handled = true;
                    return;
                }
            }

            // -----------------------------
            // 5) 입력 결과 시뮬레이션(Selection 반영)
            // -----------------------------
            //   - selectionLength 만큼 먼저 oldText에서 제거
            //   - 그 위치에 e.KeyChar(1글자) 삽입
            string newText = oldText.Remove(selectionStart, selectionLength);
            newText = newText.Insert(selectionStart, e.KeyChar.ToString());

            // -----------------------------
            // (5-1) 소수점 이하 한 자리만 허용
            // -----------------------------
            int dotIndex = newText.IndexOf('.');
            if (dotIndex >= 0)
            {
                // 소수점 뒤 길이를 계산
                int decimalsCount = newText.Length - (dotIndex + 1);

                // 소수점 뒤 자릿수가 1 초과라면(즉 2자리 이상) 막음
                if (decimalsCount > 1)
                {
                    e.Handled = true;
                    return;
                }
            }

            // -----------------------------
            // 6) double.TryParse 후, -100.0 <= 값 <= 100.0 검사
            // -----------------------------
            if (double.TryParse(newText, out double newValue))
            {
                if (newValue < -100.0 || newValue > 100.0)
                {
                    // 범위를 벗어나면 입력 무효화
                    e.Handled = true;
                }
            }
            else
            {
                // 숫자(또는 -숫자, 소수 포함)로 변환 불가 시 입력 차단
                e.Handled = true;
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

    }
}
