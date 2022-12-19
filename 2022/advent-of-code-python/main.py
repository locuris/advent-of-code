from common.util import get_lines
from day12.paths import HeightMap, shortest_path
from day8 import day_8
from day9 import day9_part2, day_9
from day10 import main as main10
from day11 import main as main11
import asyncio


async def day12():
    string_map = get_lines('day_12.txt')
    print('Starting')
    height_map = HeightMap(string_map)
    task = asyncio.create_task(height_map.path_generation_part_ii())
    await asyncio.sleep(0)
    result = await task
    print('Done!')



def day8():
    day_8.part_1()
    day_8.part_2()


def day9():
    day_9.part_1()
    day9_part2.part_ii()


def day_10():
    main10.print_result(True)


def day_11():
    main11.print_results(10000)


def test(number, mod):
    cent = int(number / 100)
    dec = number - (cent * 100)
    cent = cent // 3
    dec = dec // 3
    print(f'before div cent {cent} dec {dec}')
    cent_mod = mod // 100
    cent = 0 if cent_mod == 0 else int(cent % cent_mod)
    dec_mod = mod - cent_mod
    print(f'mods: cent {cent_mod} dec {dec_mod}')
    dec = 0 if dec_mod == 0 else int(dec % dec_mod) if dec != 0 else -1
    print(f'cent {cent} dec {dec}')


def test_ii(cent, dec, value):
    cent_value = int(value / 100)
    dec_value = value - (value * 100)


if __name__ == '__main__':
    day12()
    # left = Double(1501)
    # right = Double(101)
    # answer = left / 3
    # print(answer)
    # day_11()
    # day_11(True)
