use std::borrow::Borrow;
use std::ops::Range;
use crate::util;

pub fn print_results() {
    let input = util::parse_input("day_4.txt");
    let lines = input.lines();
    let mut matches = 0;
    for line in lines {
        if check_ranges(line) {
            matches += 1;
        }
    }
    println!("Day 4 - Part 1 - Result: {}", matches);
}

fn check_ranges(input: &str) -> bool {
    let pairs = input.split(",").collect::<Vec<&str>>();
    let pair_1 = range(pairs[0]);
    let pair_2 = range(pairs[1]);
    range_overlaps(&pair_1, &pair_2) || range_overlaps(&pair_2, &pair_1)
}

fn range_overlaps(range1: &Range<i32>, range2: &Range<i32>) -> bool {
    range1.start <= range2.start && range1.end >= range2.end
}

fn range(input: &str) -> Range<i32> {
    let pair = input.split("-").collect::<Vec<&str>>();
    let start: i32 = pair[0].parse::<i32>().unwrap();
    let end: i32 = pair[1].parse::<i32>().unwrap();
    Range { start, end }
}