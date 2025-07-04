# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 프로젝트 개요

이 프로젝트는 **C# 주식 자동매매 시스템**을 **Python으로 변환**하는 작업입니다.

### 현재 시스템 구성
- **웹 관리 시스템** (`claude/UserManagementSystem/`): ASP.NET Core Web API
- **데스크톱 자동매매 앱** (`claude2/`): C# Windows Forms + 키움 OpenAPI

### 변환 목표
- C# → Python 완전 변환
- 기능과 UI 레이아웃 최대한 유지
- 안정적이고 점진적인 변환 진행

## 변환 전략 및 순서

### 1단계: C#에서 UI 개선 (현재 단계)
- 기존 Windows Forms UI 완성
- 사용자 경험 개선
- 레이아웃 및 기능 최적화

### 2단계: 데이터베이스 모델 변환
- C# Entity Framework → Python SQLAlchemy
- User, Admin 모델 변환
- 데이터베이스 스키마 유지

### 3단계: 웹 API 서버 변환
- ASP.NET Core → FastAPI
- AuthController → FastAPI 라우터
- BCrypt 인증 시스템 유지

### 4단계: 데이터 모델 클래스 변환
- MyTradingCondition, MyTradingItem 등
- C# 클래스 → Python 클래스 (dataclass 사용)

### 5단계: 비즈니스 로직 변환
- LoginManager, TradingManager 등
- 키움 OpenAPI → pykiwoom 라이브러리

### 6단계: UI 레이아웃 변환
- Windows Forms → PyQt5/6
- 90% 유사한 외관 목표
- 기존 사용자 경험 유지

## 기술 스택

### 현재 (C#)
- **Backend**: ASP.NET Core 5.0, Entity Framework Core, SQL Server
- **Frontend**: Windows Forms, 키움 OpenAPI
- **인증**: BCrypt.NET
- **IDE**: Visual Studio 2019

### 변환 후 (Python)
- **Backend**: FastAPI, SQLAlchemy, PostgreSQL/MySQL
- **Frontend**: PyQt5/6, pykiwoom
- **인증**: bcrypt (Python)
- **IDE**: VS Code, PyCharm

## 필요한 Python 라이브러리

### 웹 API 서버용
```
fastapi==0.104.1
sqlalchemy==2.0.21
bcrypt==4.0.1
uvicorn==0.24.0
psycopg2-binary==2.9.7
```

### 데스크톱 앱용
```
PyQt5==5.15.9
pykiwoom==0.5.4
requests==2.31.0
pandas==2.1.3
numpy==1.25.2
```

## 핵심 파일 구조

### 웹 관리 시스템 (claude/UserManagementSystem/)
```
Controllers/AuthController.cs     → FastAPI 라우터로 변환
Models/User.cs, Admin.cs         → SQLAlchemy 모델로 변환
admin.html                       → Vue.js/React 또는 Jinja2 템플릿
```

### 자동매매 앱 (claude2/)
```
MainForm.cs                      → PyQt MainWindow
TradingManager.cs                → Python 클래스 (pykiwoom 사용)
LoginManager.cs                  → Python 클래스
MyTradingCondition.cs            → Python dataclass
UserLoginForm.cs                 → PyQt Dialog
```

## 주요 기능

### 인증 시스템
- 관리자 웹 로그인
- 사용자 Windows Forms 로그인
- 계좌 권한 관리 (실계좌/모의투자)

### 자동매매 시스템
- 키움 OpenAPI 연동
- 조건식 기반 매매
- 실시간 시세 모니터링
- 손익 계산 및 리스크 관리

### 웹 관리 기능
- 사용자 등록/삭제
- 계좌 승인/거부
- 매매 현황 모니터링

## 개발 환경 설정

### C# 개발 환경
```bash
# Visual Studio 2019 이상
# SQL Server Express LocalDB
# .NET Framework 4.7.2 이상
# 키움 OpenAPI+ 설치
```

### Python 개발 환경 (변환 후)
```bash
# Python 3.9 이상
# pip install -r requirements.txt
# PyQt5 Designer (UI 편집용)
# 키움 OpenAPI+ 설치 (Windows 필수)
```

## 중요 고려사항

### 키움 OpenAPI 제약
- Windows 전용 (Wine 등으로 Linux 사용 가능하나 불안정)
- COM 인터페이스 → pykiwoom 래퍼 사용
- 실시간 데이터 처리 방식 차이

### UI 프레임워크 선택
- **PyQt5/6 권장**: 90% 유사한 외관 가능
- Tkinter 대안: 70% 유사, 더 간단

### 데이터베이스 마이그레이션
- SQL Server → PostgreSQL/MySQL 권장
- 스키마 구조 동일하게 유지
- 데이터 마이그레이션 스크립트 필요

## 테스트 계정

### 관리자 계정
- Email: admin@company.com
- Password: admin123

### 테스트 사용자
- Email: test@example.com  
- Password: password123

## 실행 방법

### 현재 C# 시스템
1. UserManagementSystem API 서버 실행 (F5)
2. admin.html 브라우저에서 열기
3. StockAutoTrade2 Windows Forms 실행 (F5)

### 변환 후 Python 시스템 (예정)
1. `uvicorn main:app --reload` (FastAPI 서버)
2. 웹 브라우저에서 관리 페이지 접속
3. `python main.py` (PyQt 자동매매 앱)

## 향후 개선 계획

### 단기 목표
- [ ] C# UI 개선 완료
- [ ] 데이터베이스 모델 Python 변환
- [ ] 웹 API FastAPI 변환

### 중기 목표  
- [ ] 비즈니스 로직 Python 변환
- [ ] PyQt UI 구현
- [ ] 통합 테스트 완료

### 장기 목표
- [ ] 성능 최적화
- [ ] 추가 기능 개발
- [ ] 클라우드 배포 지원

## 개발 팁

### C# → Python 변환 시 주의사항
- 이벤트 기반 프로그래밍 → 콜백/async 패턴
- LINQ → list comprehension/pandas
- Timer → threading.Timer/asyncio
- DataGridView → QTableWidget

### 코딩 컨벤션
- Python PEP 8 스타일 가이드 준수
- 클래스명: PascalCase → snake_case
- 메서드명: camelCase → snake_case

### 디버깅 도구
- C#: Visual Studio Debugger
- Python: PyCharm Debugger, pdb

## 멀티 PC 작업 환경 설정

### 현재 작업 환경
- **사무실 PC**: 주 개발 환경 (WSL2 Ubuntu + Claude Code)
- **집 PC**: 보조 개발 환경 (WSL2 Ubuntu + Claude Code)
- **GitHub 저장소**: claude-stock-trading-system

### 집 PC 초기 설정 가이드

#### 1단계: WSL2 Ubuntu 설치
```bash
# Windows에서 WSL2 + Ubuntu 설치 (관리자 권한으로 PowerShell 실행)
wsl --install -d Ubuntu

# WSL2가 이미 설치된 경우 Ubuntu 업데이트
sudo apt update && sudo apt upgrade -y

# 필수 패키지 설치
sudo apt install curl wget git build-essential -y
```

#### 2단계: Git 설정
```bash
# Git 사용자 정보 설정
git config --global user.name "jang1230"
git config --global user.email "your_email@example.com"

# 저장소 클론
git clone https://github.com/jang1230/claude-stock-trading-system.git
cd claude-stock-trading-system
```

#### 3단계: SSH 키 생성 및 GitHub 연동
```bash
# SSH 키 생성 (집 PC용)
ssh-keygen -t rsa -b 4096 -C "jang1230-home@github.com"

# 공개키 확인 및 복사
cat ~/.ssh/id_rsa.pub

# GitHub Settings > SSH and GPG keys에서 "New SSH key" 추가
# Title: "Home PC - WSL2" 
# Key: 위에서 복사한 내용 붙여넣기
```

#### 4단계: SSH 연결 테스트
```bash
# GitHub 호스트 키 추가
ssh-keyscan -H github.com >> ~/.ssh/known_hosts

# SSH 연결 테스트
ssh -T git@github.com
# 성공 시: "Hi jang1230! You've successfully authenticated..."

# Remote URL을 SSH로 변경
git remote set-url origin git@github.com:jang1230/claude-stock-trading-system.git
```

#### 5단계: Claude Code 설치
```bash
# Claude Code 설치 (공식 설치 방법)
# 1. NPM 설치 (Node.js 포함)
curl -fsSL https://deb.nodesource.com/setup_lts.x | sudo -E bash -
sudo apt-get install -y nodejs

# 2. Claude Code 설치
npm install -g @anthropic/claude-code

# 3. Claude Code 인증 (API 키 필요)
claude auth login

# 4. 설치 확인
claude --version
```

#### 6단계: 추가 개발 도구 설치 (선택사항)
```bash
# C# 개발 환경
sudo apt install dotnet-sdk-6.0 -y

# Python 개발 환경  
sudo apt install python3 python3-pip python3-venv -y

# Visual Studio Code (Claude Code와 함께 사용)
wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > packages.microsoft.gpg
sudo install -o root -g root -m 644 packages.microsoft.gpg /etc/apt/trusted.gpg.d/
sudo sh -c 'echo "deb [arch=amd64,arm64,armhf signed-by=/etc/apt/trusted.gpg.d/packages.microsoft.gpg] https://packages.microsoft.com/repos/code stable main" > /etc/apt/sources.list.d/vscode.list'
sudo apt update
sudo apt install code -y
```

### 일상적인 멀티 PC 작업 플로우

#### 🏢 사무실에서 작업 시작 (WSL2 Ubuntu + Claude Code)
```bash
# WSL2 Ubuntu 터미널에서
cd /mnt/d/claude-project      # 또는 작업 디렉토리
git pull                      # 집에서 작업한 내용 받아오기

# Claude Code로 작업 진행
claude                        # Claude Code 대화형 모드 시작
# 또는
claude "C# 코드 분석 및 개선 작업 진행"

# 작업 완료 후
git add .
git commit -m "feat: 새 기능 구현"
git push                      # GitHub에 업로드
```

#### 🏠 집에서 작업 계속 (WSL2 Ubuntu + Claude Code)
```bash
# WSL2 Ubuntu 터미널에서
cd claude-stock-trading-system
git pull                      # 사무실에서 작업한 내용 받아오기

# Claude Code로 작업 진행  
claude                        # Claude Code 대화형 모드 시작
# 또는
claude "Python 변환 작업 계속 진행"

# 작업 완료 후
git add .
git commit -m "feat: Python 변환 작업 진행"
git push                      # GitHub에 업로드
```

### 충돌 방지 및 해결 가이드

#### 작업 전 필수 체크리스트
- [ ] `git pull` 실행하여 최신 코드 받아오기
- [ ] 작업할 파일이 다른 PC에서 수정되지 않았는지 확인
- [ ] 작업 완료 후 즉시 `git push` 실행

#### 충돌 발생 시 해결 방법
```bash
# 충돌 발생 시
git pull                    # 충돌 파일 확인
# 충돌된 파일 수동 편집 (<<<< ==== >>>> 마크 제거)
git add .
git commit -m "resolve: 충돌 해결"
git push
```

### PC별 역할 분담 (권장)

#### 🏢 사무실 PC (WSL2 Ubuntu + Claude Code)
- C# 코드 분석 및 개선
- 데이터베이스 모델 설계  
- 웹 API 개발
- GitHub 이슈 관리
- Claude Code를 통한 코드 리팩토링

#### 🏠 집 PC (WSL2 Ubuntu + Claude Code)
- Python 변환 작업
- PyQt UI 개발
- 테스트 및 디버깅
- 문서 작성 (CLAUDE.md, README.md 등)
- Claude Code를 통한 Python 코드 생성

### 백업 및 동기화 전략

#### 자동 백업 설정 (선택사항)
```bash
# 작업 전 자동 pull을 위한 alias 설정
echo 'alias work="git pull && echo 작업 준비 완료!"' >> ~/.bashrc
echo 'alias save="git add . && git commit -m 'auto-save: 작업 중간 저장' && git push"' >> ~/.bashrc
source ~/.bashrc
```

#### 중요 파일 별도 백업
- `CLAUDE.md`: 프로젝트 가이드 (항상 최신 유지)
- `.gitignore`: ignore 규칙  
- `README.md`: 프로젝트 소개

### Claude Code 활용 팁

#### 집 PC에서 Claude Code 최적 활용법
```bash
# 프로젝트 디렉토리에서 Claude Code 시작
cd claude-stock-trading-system
claude

# 주요 활용 명령어
claude "CLAUDE.md 파일을 참조해서 현재 프로젝트 상황 파악해줘"
claude "C# MainForm.cs를 Python PyQt로 변환해줘"  
claude "TradingManager 클래스를 분석하고 Python 버전 작성해줘"
claude "현재 작업한 내용을 GitHub에 커밋해줘"
```

#### 동일한 작업 환경 보장
- **사무실 & 집 PC 모두**: WSL2 Ubuntu + Claude Code
- **동일한 프롬프트 방식**: CLAUDE.md 기반 컨텍스트 공유
- **일관된 개발 플로우**: Git + Claude Code 조합

## GitHub 버전 관리 전략

### 저장소 구조
- **메인 저장소**: claude-stock-trading-system
- **브랜치 전략**: GitFlow 기반
- **이슈 관리**: GitHub Issues + Projects 활용

### 브랜치 전략
```
main                    # 안정된 프로덕션 코드
├── develop            # 개발 통합 브랜치
├── feature/python-conversion    # Python 변환 작업
├── feature/ui-improvement      # UI 개선 작업
├── bugfix/login-issue         # 버그 수정
└── release/v1.0              # 릴리스 준비
```

### 커밋 메시지 컨벤션
```
feat: 새 기능 추가
fix: 버그 수정  
refactor: 코드 리팩토링
docs: 문서 업데이트
test: 테스트 추가/수정
style: 코드 스타일 수정
chore: 빌드 설정 등 기타 작업
```

### 프로젝트 설정 체크리스트
- [ ] GitHub 저장소 생성
- [ ] .gitignore 파일 설정 (C#/Python 통합)
- [ ] README.md 작성
- [ ] CONTRIBUTING.md 가이드라인
- [ ] GitHub Actions CI/CD 설정
- [ ] 브랜치 보호 규칙 설정

### GitHub 워크플로우
1. **이슈 생성**: 작업 전 GitHub Issue 생성
2. **브랜치 생성**: feature/이슈번호-설명 형태
3. **개발 진행**: 커밋 메시지 컨벤션 준수
4. **Pull Request**: 코드 리뷰 및 테스트
5. **머지**: develop → main 순차 진행

### 필수 파일들
```
.gitignore              # C#/Python 통합 ignore 규칙
README.md              # 프로젝트 소개 및 설치 가이드
CONTRIBUTING.md        # 기여 가이드라인
docs/                  # 추가 문서
├── api-docs.md        # API 문서
├── deployment.md      # 배포 가이드
└── troubleshooting.md # 문제 해결 가이드
```

## 연락처 및 문서

### 참고 문서
- C# 기존 프로젝트 설명: `c# 홈페이지및 데이터베이스.txt`
- 자동매매 시스템 가이드: `c# 자동매매프로그램.txt`
- 키움 OpenAPI 개발가이드: https://www.kiwoom.com/
- pykiwoom 문서: https://github.com/pykiwoom/pykiwoom

### 변환 진행 상황
- **현재 단계**: C# 코드 분석 완료, UI 개선 준비 중
- **다음 단계**: UI 개선 후 Python 데이터베이스 모델 변환
- **예상 완료**: 단계별 진행 (약 2-3개월 소요 예상)

---

*이 문서는 C# → Python 변환 프로젝트의 전체 가이드입니다. 새로운 Claude 세션에서 이 문서를 참조하여 프로젝트 상황을 빠르게 파악할 수 있습니다.*

## 새로운 작업 메모리

- `to memorize` 명령을 사용해 새로운 작업 메모를 추가