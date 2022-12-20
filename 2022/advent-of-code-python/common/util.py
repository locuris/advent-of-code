prefix = '../data/'

alphabet = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"


def get_lines(file):
    return open(prefix + file).read().splitlines()


def get_string_by_empty_line(file):
    return open(prefix + file).read().split('\n\n')


def get_input(file):
    return open(prefix + file).read()


def alphabet_index(letter: str):
    return alphabet.index(letter)
