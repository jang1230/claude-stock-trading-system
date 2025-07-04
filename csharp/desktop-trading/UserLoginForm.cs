using StockAutoTrade2;
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
    public partial class UserLoginForm : Form
    {
        private UserAuthManager authManager;
        private Button loginButton;
        private TextBox emailTextBox;
        private TextBox passwordTextBox;
        private Label titleLabel;
        private Label emailLabel;
        private Label passwordLabel;
        private Label statusLabel;
        private PictureBox logoPictureBox;

        public UserLoginForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
            authManager = UserAuthManager.GetInstance();
        }

        private void InitializeCustomComponents()
        {
            // 폼 설정
            this.Text = "사용자 인증";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 244, 248);

            // 제목 레이블
            titleLabel = new Label();
            titleLabel.Text = "🔐 사용자 로그인";
            titleLabel.Font = new Font("맑은 고딕", 16, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(51, 51, 51);
            titleLabel.Size = new Size(300, 30);
            titleLabel.Location = new Point(50, 30);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(titleLabel);

            // 이메일 레이블
            emailLabel = new Label();
            emailLabel.Text = "이메일:";
            emailLabel.Font = new Font("맑은 고딕", 9);
            emailLabel.Size = new Size(60, 20);
            emailLabel.Location = new Point(50, 80);
            this.Controls.Add(emailLabel);

            // 이메일 텍스트박스
            emailTextBox = new TextBox();
            emailTextBox.Font = new Font("맑은 고딕", 10);
            emailTextBox.Size = new Size(250, 25);
            emailTextBox.Location = new Point(50, 100);
            emailTextBox.Text = "test@example.com"; // 테스트용 기본값
            this.Controls.Add(emailTextBox);

            // 비밀번호 레이블
            passwordLabel = new Label();
            passwordLabel.Text = "비밀번호:";
            passwordLabel.Font = new Font("맑은 고딕", 9);
            passwordLabel.Size = new Size(80, 20);
            passwordLabel.Location = new Point(50, 140);
            this.Controls.Add(passwordLabel);

            // 비밀번호 텍스트박스
            passwordTextBox = new TextBox();
            passwordTextBox.Font = new Font("맑은 고딕", 10);
            passwordTextBox.Size = new Size(250, 25);
            passwordTextBox.Location = new Point(50, 160);
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Text = "password123"; // 테스트용 기본값
            this.Controls.Add(passwordTextBox);

            // 로그인 버튼
            loginButton = new Button();
            loginButton.Text = "로그인";
            loginButton.Font = new Font("맑은 고딕", 10, FontStyle.Bold);
            loginButton.Size = new Size(250, 35);
            loginButton.Location = new Point(50, 200);
            loginButton.BackColor = Color.FromArgb(70, 139, 181);
            loginButton.ForeColor = Color.White;
            loginButton.FlatStyle = FlatStyle.Flat;
            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.Cursor = Cursors.Hand;
            loginButton.Click += LoginButton_Click;
            this.Controls.Add(loginButton);

            // 상태 레이블
            statusLabel = new Label();
            statusLabel.Text = "";
            statusLabel.Font = new Font("맑은 고딕", 8);
            statusLabel.Size = new Size(250, 20);
            statusLabel.Location = new Point(50, 245);
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(statusLabel);

            // Enter 키 이벤트
            this.KeyPreview = true;
            this.KeyDown += UserLoginForm_KeyDown;
        }

        private void UserLoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Text.Trim();
            string password = passwordTextBox.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowStatus("이메일과 비밀번호를 입력해주세요.", Color.Red);
                return;
            }

            // 로그인 중 상태 표시
            SetLoginState(true);
            ShowStatus("로그인 중...", Color.Blue);

            try
            {
                var result = await authManager.LoginAsync(email, password);

                if (result.Success)
                {
                    ShowStatus("로그인 성공! 메인 화면으로 이동합니다.", Color.Green);

                    // 잠시 기다린 후 메인폼으로 전환
                    await Task.Delay(1000);

                    // 메인폼 열기
                    MainForm mainForm = new MainForm();
                    mainForm.Show();

                    // 현재 로그인폼 숨기기
                    this.Hide();
                }
                else
                {
                    ShowStatus(result.Message, Color.Red);
                    SetLoginState(false);
                }
            }
            catch (Exception ex)
            {
                ShowStatus($"오류: {ex.Message}", Color.Red);
                SetLoginState(false);
            }
        }

        private void SetLoginState(bool isLogging)
        {
            loginButton.Enabled = !isLogging;
            emailTextBox.Enabled = !isLogging;
            passwordTextBox.Enabled = !isLogging;

            if (isLogging)
            {
                loginButton.Text = "로그인 중...";
            }
            else
            {
                loginButton.Text = "로그인";
            }
        }

        private void ShowStatus(string message, Color color)
        {
            statusLabel.Text = message;
            statusLabel.ForeColor = color;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            authManager?.Dispose();
            Application.Exit(); // 로그인폼이 닫히면 앱 종료
            base.OnFormClosing(e);
        }
    }
}
