using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

namespace StockAutoTrade2
{
    public class UserAuthManager
    {
        private static UserAuthManager instance;
        private readonly HttpClient httpClient;
        private const string API_BASE_URL = "https://localhost:44309";

        // UserAuthManager 클래스에 추가
        private DateTime loginTime;
        private const int SESSION_TIMEOUT_MINUTES = 480; // 8시간

        public string CurrentUserEmail { get; private set; }
        public int CurrentUserId { get; private set; }

        private UserAuthManager()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public static UserAuthManager GetInstance()
        {
            if (instance == null)
                instance = new UserAuthManager();
            return instance;
        }

        // 인터넷 연결 확인
        public bool IsInternetConnected()
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send("8.8.8.8", 3000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        // 사용자 로그인
        public async Task<UserLoginResult> LoginAsync(string email, string password)
        {
            try
            {
                // 인터넷 연결 확인
                if (!IsInternetConnected())
                {
                    return new UserLoginResult
                    {
                        Success = false,
                        Message = "인터넷 연결을 확인해주세요."
                    };
                }

                var loginRequest = new
                {
                    email = email,
                    password = password
                };

                string jsonContent = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{API_BASE_URL}/api/Auth/user/login", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    CurrentUserEmail = result.email;
                    CurrentUserId = result.userId;
                    loginTime = DateTime.Now; // 추가

                    return new UserLoginResult
                    {
                        Success = true,
                        Message = "로그인 성공",
                        UserEmail = CurrentUserEmail,
                        UserId = CurrentUserId
                    };
                }
                else
                {
                    var errorResult = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    return new UserLoginResult
                    {
                        Success = false,
                        Message = errorResult.message ?? "로그인에 실패했습니다."
                    };
                }
            }
            catch (HttpRequestException)
            {
                return new UserLoginResult
                {
                    Success = false,
                    Message = "서버에 연결할 수 없습니다. 인터넷 연결을 확인해주세요."
                };
            }
            catch (TaskCanceledException)
            {
                return new UserLoginResult
                {
                    Success = false,
                    Message = "요청 시간이 초과되었습니다."
                };
            }
            catch (Exception ex)
            {
                return new UserLoginResult
                {
                    Success = false,
                    Message = $"오류가 발생했습니다: {ex.Message}"
                };
            }
        }

        // 리소스 정리
        public void Dispose()
        {
            httpClient?.Dispose();
        }
        public bool IsSessionValid()
        {
            if (string.IsNullOrEmpty(CurrentUserEmail))
                return false;

            return DateTime.Now.Subtract(loginTime).TotalMinutes < SESSION_TIMEOUT_MINUTES;
        }
    }

    // 로그인 결과 클래스
    public class UserLoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string UserEmail { get; set; }
        public int UserId { get; set; }
    }
}