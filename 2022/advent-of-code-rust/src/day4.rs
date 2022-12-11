use std::borrow::Borrow;
use std::ops::{RangeInclusive};
use crate::util;

pub fn print_results() {
    let input = util::parse_input("day_4.txt");
    let lines = input.lines();
    let mut part_1_score = 0;
    let mut part_2_score = 0;
    for line in lines {
        let range_check = check_ranges(line);
        if range_check.0 {
            part_1_score += 1;
        }
        if range_check.1 {
            part_2_score += 1;
        }
    }
    println!("Day 4 - Part 1 - Result: {}", part_1_score);
    println!("Day 4 - Part 2 - Result: {}", part_2_score);
}

fn check_ranges(input: &str) -> (bool, bool) {
    let pairs = input.split(",").collect::<Vec<&str>>();
    let pair_1 = range(pairs[0]);
    let pair_2 = range(pairs[1]);
    (range_contains(&pair_1, &pair_2) || range_contains(&pair_2, &pair_1),
     range_overlaps(&pair_1, &pair_2) || range_overlaps(&pair_2, &pair_1))
}

fn range_contains(range1: &RangeInclusive<i32>, range2: &RangeInclusive<i32>) -> bool {
    range1.start() <= range2.start() && range1.end() >= range2.end()
}

fn range_overlaps(range1: &RangeInclusive<i32>, range2: &RangeInclusive<i32>) -> bool {
    range1.contains(range2.start().borrow()) || range1.contains(range2.end().borrow())
}

fn range(input: &str) -> RangeInclusive<i32> {
    let pair = input.split("-").collect::<Vec<&str>>();
    let start: i32 = pair[0].parse::<i32>().unwrap();
    let end: i32 = pair[1].parse::<i32>().unwrap();
    RangeInclusive::new(start, end)
}