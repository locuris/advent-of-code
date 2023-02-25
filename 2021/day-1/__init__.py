from common.data import lines


def determine_depth_increase(group_size: int = 1):
    sonar_readings = lines()
    sonar_readings = [int(sr) for sr in sonar_readings]
    times_depth_increased = 0
    sonar_readings_count = len(sonar_readings)
    for start_idx, _ in enumerate(sonar_readings):
        end_idx = start_idx + group_size
        if end_idx >= sonar_readings_count:
            break
        group_depth_1 = sum(sonar_readings[start_idx:end_idx])
        group_depth_2 = sum(sonar_readings[start_idx + 1:end_idx + 1])
        if group_depth_1 < group_depth_2:
            times_depth_increased += 1

    print(times_depth_increased)


def part_i():
    determine_depth_increase()


def part_ii():
    determine_depth_increase(3)


if __name__ == '__main__':
    part_i()
    part_ii()
