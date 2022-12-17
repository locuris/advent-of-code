prefix = '../data/'


def get_lines(file):
    return open(prefix + file).read().splitlines()


def get_string_by_empty_line(file):
    return open(prefix + file).read().split('\n\n')


def get_input(file):
    return open(prefix + file).read()
