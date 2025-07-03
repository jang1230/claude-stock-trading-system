# C# to Python Stock Trading System

## 프로젝트 개요

이 프로젝트는 **C# 기반 주식 자동매매 시스템**을 **Python으로 완전 변환**하는 작업입니다.

### 시스템 구성

- **웹 관리 시스템**: ASP.NET Core → FastAPI 변환
- **데스크톱 자동매매 앱**: Windows Forms → PyQt5/6 변환
- **데이터베이스**: SQL Server → PostgreSQL/MySQL 마이그레이션

## 기술 스택

### 현재 (C#)
- ASP.NET Core 5.0
- Entity Framework Core
- Windows Forms
- 키움 OpenAPI
- SQL Server

### 변환 후 (Python)
- FastAPI
- SQLAlchemy
- PyQt5/6
- pykiwoom
- PostgreSQL/MySQL

## 설치 및 실행

### C# 시스템 (현재)

1. 필수 요구사항
   - Visual Studio 2019+
   - .NET Framework 4.7.2+
   - SQL Server Express LocalDB
   - 키움 OpenAPI+

2. 실행 방법
   ```bash
   # 웹 API 서버 실행
   cd claude/UserManagementSystem
   dotnet run

   # 자동매매 앱 실행
   cd claude2
   # Visual Studio에서 F5 실행
   ```

### Python 시스템 (변환 후)

1. Python 환경 설정
   ```bash
   python -m venv venv
   source venv/bin/activate  # Windows: venv\Scripts\activate
   pip install -r requirements.txt
   ```

2. 실행 방법
   ```bash
   # FastAPI 서버 실행
   uvicorn main:app --reload

   # PyQt 자동매매 앱 실행
   python main.py
   ```

## 변환 진행 상황

- [x] C# 코드 분석 완료
- [ ] 데이터베이스 모델 Python 변환
- [ ] 웹 API FastAPI 변환
- [ ] 비즈니스 로직 Python 변환
- [ ] PyQt UI 구현
- [ ] 통합 테스트

## 테스트 계정

- **관리자**: admin@company.com / admin123
- **사용자**: test@example.com / password123

## 기여 가이드

자세한 내용은 [CONTRIBUTING.md](CONTRIBUTING.md)를 참조하세요.

## 라이선스

이 프로젝트는 MIT 라이선스를 따릅니다.