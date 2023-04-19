module_prefix = '../data/'
main_prefix = 'data/'
prefix = ''
suffix = '.txt'
alphabet = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"


def __filename(file):
    return prefix + file + suffix


def set_prefix(from_main):
    global prefix
    prefix = main_prefix if from_main else module_prefix


def get_lines(file, ignore_empty_line=False, from_main=False):
    set_prefix(from_main)
    if ignore_empty_line:
        return open(__filename(file)).read().strip('\n\n').splitlines()
    return open(__filename(file)).read().splitlines()


def get_string_by_empty_line(file, from_main=False):
    set_prefix(from_main)
    return open(__filename(file)).read().split('\n\n')


def get_input(file):
    return open(__filename(file)).read()




def alphabet_index(letter: str):
    return alphabet.index(letter)
