using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Models;
using BCrypt.Net;

namespace UserManagementSystem.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 관리자 로그인
        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginRequest request)
        {
            try
            {
                var admin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.Email == request.Email);

                if (admin == null)
                {
                    return BadRequest(new { message = "관리자를 찾을 수 없습니다." });
                }

                // 비밀번호 검증
                if (!BCrypt.Net.BCrypt.Verify(request.Password, admin.PasswordHash))
                {
                    return BadRequest(new { message = "비밀번호가 올바르지 않습니다." });
                }

                return Ok(new
                {
                    message = "로그인 성공",
                    adminId = admin.Id,
                    email = admin.Email
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "서버 오류가 발생했습니다.", error = ex.Message });
            }
        }

        // 새 사용자 등록 (관리자만 가능)
        [HttpPost("user/register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            try
            {
                // 이메일 중복 체크
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (existingUser != null)
                {
                    return BadRequest(new { message = "이미 등록된 이메일입니다." });
                }

                // 비밀번호 해싱
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // 새 사용자 생성
                var newUser = new User
                {
                    Email = request.Email,
                    PasswordHash = hashedPassword,
                    CreatedBy = request.AdminId,
                    IsActive = true,
                    CreatedAt = System.DateTime.Now
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "사용자가 성공적으로 등록되었습니다.",
                    userId = newUser.Id,
                    email = newUser.Email
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "서버 오류가 발생했습니다.", error = ex.Message });
            }
        }

        // 사용자 로그인 (Windows Forms 앱에서 사용)
        [HttpPost("user/login")]
        public async Task<IActionResult> UserLogin([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive == true);

                if (user == null)
                {
                    return BadRequest(new { message = "사용자를 찾을 수 없거나 비활성화되었습니다." });
                }

                // 비밀번호 검증
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return BadRequest(new { message = "비밀번호가 올바르지 않습니다." });
                }

                return Ok(new
                {
                    message = "로그인 성공",
                    userId = user.Id,
                    email = user.Email,
                    isActive = user.IsActive
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "서버 오류가 발생했습니다.", error = ex.Message });
            }
        }

        // 등록된 사용자 목록 조회
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Where(u => u.IsActive == true)
                    .Select(u => new
                    {
                        id = u.Id,
                        email = u.Email,
                        createdAt = u.CreatedAt.ToString("yyyy-MM-dd")
                    })
                    .ToListAsync();

                return Ok(users);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "서버 오류가 발생했습니다.", error = ex.Message });
            }
        }

        // 사용자 삭제
        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound(new { message = "사용자를 찾을 수 없습니다." });
                }

                user.IsActive = false; // 실제 삭제 대신 비활성화
                await _context.SaveChangesAsync();

                return Ok(new { message = "사용자가 삭제되었습니다." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "서버 오류가 발생했습니다.", error = ex.Message });
            }
        }

        // 임시: 관리자 계정 생성 (테스트용)
        [HttpPost("admin/create")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminRequest request)
        {
            try
            {
                // 기존 관리자 삭제
                var existingAdmin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.Email == request.Email);

                if (existingAdmin != null)
                {
                    _context.Admins.Remove(existingAdmin);
                }

                // 새 관리자 생성 (BCrypt로 정확히 해싱)
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var newAdmin = new Admin
                {
                    Email = request.Email,
                    PasswordHash = hashedPassword,
                    CreatedAt = System.DateTime.Now
                };

                _context.Admins.Add(newAdmin);
                await _context.SaveChangesAsync();

                return Ok(new { message = "관리자가 생성되었습니다.", email = newAdmin.Email });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "오류 발생", error = ex.Message });
            }
        }

        // 요청 모델 추가
        public class CreateAdminRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
    // 요청 모델 클래스들
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int AdminId { get; set; }
    }


}
