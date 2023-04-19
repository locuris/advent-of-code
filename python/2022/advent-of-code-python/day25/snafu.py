from enum import Enum
import math
import re


class SnafuDigit(Enum):
    Zero = '0'
    One = '1'
    Two = '2'
    Minus = '-'
    DoubleMinus = '='


def convert_snafu_digit(digit: SnafuDigit) -> str:
    match digit:
        case SnafuDigit.Minus:
            return '-1'
        case SnafuDigit.DoubleMinus:
            return '-2'
        case _:
            return digit.value


class Snafu:
    def __init__(self, dec_value: int = 1, snafu_value: str = ''):
        self.decimal_int = dec_value
        self.snafu = str(dec_value)
        if not snafu_value == '':
            self.snafu = snafu_value
            self.__convert_snafu()
        else:
            self.snafu = ''
            self.__convert_decimal()

    # 2=01
    def __convert_snafu(self):
        unit = 1
        values = []
        for v in self.snafu[::-1]:
            snafu_digit = SnafuDigit(v)
            snafu_digit = convert_snafu_digit(snafu_digit)
            value = int(snafu_digit) * unit
            values.append(value)
            unit *= 5

        operation = ''
        for value in values:
            operation += f'{value} + '
        operation += f' 0'

        self.decimal_int = eval(operation)

    def __convert_decimal(self):
        dec = self.decimal_int
        units = math.ceil(dec**(1/5))
        snafu = ''
        for unit in reversed(range(0, units + 1)):
            unit_val = 5 ** unit
            value = dec // unit_val
            snafu += str(value)
            dec = dec % unit_val

        units = len(snafu)
        snafu_list = list(snafu)
        while re.search('[^012\-=]', ''.join(snafu_list)) is not None:
            for idx, d in enumerate(snafu_list):
                if d == '-' or d == '=':
                    continue
                digit = int(d)
                if digit > 2:
                    single_minus = True if 5 - digit == 1 else False
                    prev_value = snafu_list[idx - 1]
                    new_value = ''
                    match prev_value:
                        case '-':
                            new_value = '0'
                        case '=':
                            new_value = '-'
                        case _:
                            new_value = int(prev_value) + 1

                    snafu_list[idx - 1] = str(new_value)
                    snafu_list[idx] = '-' if single_minus else '='

        if snafu_list[0] == '0':
            snafu_list = snafu_list[1:]
        self.snafu = ''.join(snafu_list)
