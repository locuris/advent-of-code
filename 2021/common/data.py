def lines(test: bool = False) -> [str]:
    filename = 'test.txt' if test else 'input.txt'
    return open(filename).readlines()
