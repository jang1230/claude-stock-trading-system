#!/usr/bin/env python3
"""
Simple Hello World Python script
"""

def main():
    print("Hello, World!")
    print("Welcome to Python programming!")
    
    # 추가 기능들
    name = input("What's your name? ")
    print(f"Nice to meet you, {name}!")
    
    # 간단한 계산
    num1 = int(input("Enter first number: "))
    num2 = int(input("Enter second number: "))
    result = num1 + num2
    print(f"{num1} + {num2} = {result}")

if __name__ == "__main__":
    main()