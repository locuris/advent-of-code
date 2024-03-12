import os
from pathlib import Path


language = "rust"
language_extension = "rs"
prefix = "aoc"
year = "2015"
source = "src"

directory = f"./{language}/{prefix}-{year}/{source}/"

for n in range(1, 25):
    day_dir = f"{directory}day_{n}"
    if not os.path.exists(day_dir):
        os.makedirs(day_dir)
        Path(f"{day_dir}/test.txt").touch()
        Path(f"{day_dir}/input.txt").touch()
        Path(f"{day_dir}/solution.{language_extension}").touch()
