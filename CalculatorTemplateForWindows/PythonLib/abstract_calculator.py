import sys

def add(a, b) -> float:
    return a + b

def substract(a, b) -> float:
    return a - b

def multiply(a, b) -> float:
    return a * b

def divide(a, b) -> float:
    return a / b

def operation_X(a, b) -> float:
    # to customize your project for difference from other's 
    return 0

if __name__ == "__main__":
    path_to_output_file = "\\CalculatorTemplateForWindows\\CalculatorTemplateForWindows\\Output\\output.txt"
    val = -1
    try:
        operation_option = sys.argv[1]
        operand1 = float(sys.argv[2])
        operand2 = float(sys.argv[3])

        if operation_option == "+":
            val = add(operand1, operand2)
        elif operation_option == "-":
            val = substract(operand1, operand2)
        elif operation_option == "*":
            val = multiply(operand1, operand2)
        elif operation_option == "/":
            val = divide(operand1, operand2)
        else: # operation X 
            val = operation_X(operand1, operand2)
    except ex as Exception:
        val = -2
        print(str(ex)) # DEBUG message 

    try:
        with open(path_to_output_file,'w',encoding = 'utf-8') as f:
            f.write(str(val)) # write to a specific file for C# 
        print('Python完整執行完畢')
    except ex as Exeption:
        print(str(ex)) # DEBUG message 
