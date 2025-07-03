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
    public partial class ProfitLossDialog : Form
    {
        public MainForm gMainForm = MainForm.GetInstance();
        public ProfitLossDialog()
        {
            InitializeComponent();

            // profitLossDataGirdView 초기 설정하기
            //머리글 중앙 정렬
            profitLossDataGirdView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //더블버퍼 설정
            profitLossDataGirdView.DoubleBuffered(true);
            //각각의 열 정렬
            for(int i=0; i <profitLossDataGirdView.Columns.Count; i++)
            {
                if (i < 2)
                    profitLossDataGirdView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //중앙 정렬
                else
                    profitLossDataGirdView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; //우측 정렬
            }

            profitLossDataGirdView.CellFormatting += profitLossDataGridView_CellFormatting;
        }
        // profitLossDataGirdView 서식 지정 , 실현손익이나 수익률이 0 일때는 검은색, -면 파란색, +면 붉은색으로 표시
        private void profitLossDataGridView_CellFormatting(object sender , DataGridViewCellFormattingEventArgs e)
        {
            if(profitLossDataGirdView.Columns[e.ColumnIndex].Name == "실현손익_실현손익" || profitLossDataGirdView.Columns[e.ColumnIndex].Name == "실현손익_수익률")
            {
                if(e.Value.ToString() == "0.00%")
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
        // profitLossDataGirdView 데이터 바인딩 메소드
        public void AddProfitLossToDataGridView(string date, string itemCode, double totalPurchaseAmount, double totalSellAmount, int qnt, double totalEvaluationProfitLoss, double rateOfReturn)
        {
            // rowIndex = 몇번째 줄인지 리턴
            int rowIndex = profitLossDataGirdView.Rows.Add(); //그리드뷰에 한줄이 추가됨

            string itemName = gMainForm.KiwoomAPI.GetMasterCodeName(itemCode); // 종목명
            profitLossDataGirdView["실현손익_매매일", rowIndex].Value = date; //매매일
            profitLossDataGirdView["실현손익_종목명", rowIndex].Value = itemName; // 종목명
            profitLossDataGirdView["실현손익_종목코드", rowIndex].Value = itemCode; //종목코드
            profitLossDataGirdView["실현손익_매수금", rowIndex].Value = totalPurchaseAmount.ToString("N0"); //매수금
            profitLossDataGirdView["실현손익_매도금", rowIndex].Value = totalSellAmount.ToString("N0"); //매도금
            profitLossDataGirdView["실현손익_매도량", rowIndex].Value = qnt.ToString("N0"); //매도량
            profitLossDataGirdView["실현손익_실현손익", rowIndex].Value = totalEvaluationProfitLoss.ToString("N0"); //실현손익
            profitLossDataGirdView["실현손익_수익률", rowIndex].Value = rateOfReturn.ToString("N2") +"%";//수익률

            //열의 높이지정
            profitLossDataGirdView.Rows[rowIndex].Height = 30;
        }

        public void getProfitLossFileDataToDataGridView()
        {
            // profitLossDataGridView 전체 삭제 -> 그리드뷰 초기화
            int _listCount = profitLossDataGirdView.Rows.Count;
            for (int i = _listCount - 1; i >= 0; i--)
                profitLossDataGirdView.Rows.RemoveAt(i);

            string[] _tradingList = gMainForm.gFileIOInstance.getTradingListData();
            int _tradingListLength = _tradingList.Length / (int)Save.trading;

            //그리드뷰에 추가(아래의 for문은 역순으로추가 -> 최신데이터를 상단에추가하는방식)
            for(int i = _tradingListLength-1; i>=0; i--)
            {
                string _date = _tradingList[i * (int)Save.trading + 0]; // 날짜
                string _itemCode = _tradingList[i * (int)Save.trading + 1]; //종목코드
                double _totalPurchaseAmount = double.Parse(_tradingList[i * (int)Save.trading + 2]); //매수량
                int _totalQnt = int.Parse(_tradingList[i * (int)Save.trading + 3]); //매도량
                int _sellPrice = int.Parse(_tradingList[i * (int)Save.trading + 4]); //매도가
                double _totalEvaluationProfitLoss = double.Parse(_tradingList[i * (int)Save.trading + 5]); //평가손익
                double _rateOfReturn = double.Parse(_tradingList[i * (int)Save.trading + 6]); //수익률

                double _totalSellAmount = _totalEvaluationProfitLoss + _totalPurchaseAmount;
                AddProfitLossToDataGridView(_date, _itemCode, _totalPurchaseAmount, _totalSellAmount, _totalQnt, _totalEvaluationProfitLoss, _rateOfReturn);
            }
            TotalRateOfReturnAndEvaluationProfitLoss();
        }

        //총수익률 계산 및 총손익, 총수익률 label 색지정
        private void TotalRateOfReturnAndEvaluationProfitLoss()
        {
            int allTotalPurchaseAmount = 0; // 총매입금액
            // int allTotalEvaluationPrice = 0; // 총평가금액
            int allTotalEvaluationProfitLoss = 0; // 총평가손익
            double allRateOfReturn = 0; // 총수익률

            foreach(DataGridViewRow row in profitLossDataGirdView.Rows)
            {
                double totalPurchaseAmount = double.Parse(row.Cells["실현손익_매수금"].Value.ToString().Replace(",", "")); // 총매입금
                int totalEvaluationProfitLoss = int.Parse(row.Cells["실현손익_실현손익"].Value.ToString().Replace(",", "")); //실현손익

                // 총매입금액(데이터그리드뷰를돌면서 "실현손익_매수금"row를 전부더한다)
                allTotalPurchaseAmount += (int)totalPurchaseAmount;
                // 총평가금액(데이터그리드뷰를돌면서 "실현손익_실현손익"row를 전부더한다)
                allTotalEvaluationProfitLoss += totalEvaluationProfitLoss;
            }

            // 총 실현손익
            totalProfitLossDlgLabel.Text = allTotalEvaluationProfitLoss.ToString("N0");
            if (allTotalEvaluationProfitLoss < 0)
                totalProfitLossDlgLabel.ForeColor = Color.Blue;
            else if (allTotalEvaluationProfitLoss > 0)
                totalProfitLossDlgLabel.ForeColor = Color.Red;
            else
                totalProfitLossDlgLabel.ForeColor = Color.Black;

            // 총 수익률
            if (allTotalPurchaseAmount > 0)
                allRateOfReturn = (double)allTotalEvaluationProfitLoss / allTotalPurchaseAmount * 100;
            else
                allRateOfReturn = 0;

            totalRateOfReturnDlgLabel.Text = allRateOfReturn.ToString("N2") + "%";
            if (allRateOfReturn < 0)
                totalRateOfReturnDlgLabel.ForeColor = Color.Blue;
            else if (allRateOfReturn > 0)
                totalRateOfReturnDlgLabel.ForeColor = Color.Red;
            else
                totalRateOfReturnDlgLabel.ForeColor = Color.Black;
        }
    }
}
