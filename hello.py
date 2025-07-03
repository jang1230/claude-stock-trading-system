#!/usr/bin/env python3
"""
Simple Hello World Python script
"""

def main():
    print("Hello, World!")
    print("Welcome to Python programming!")
    
    # 샘플 데이터로 기능 시연
    name = "GitHub User"
    print(f"Nice to meet you, {name}!")
    
    # 간단한 계산 예제
    num1 = 10
    num2 = 5
    result = num1 + num2
    print(f"{num1} + {num2} = {result}")
    
    # 현재 시간 출력
    import datetime
    current_time = datetime.datetime.now()
    print(f"Current time: {current_time.strftime('%Y-%m-%d %H:%M:%S')}")

if __name__ == "__main__":
    main()