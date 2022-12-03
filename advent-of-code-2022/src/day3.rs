use std::str::Lines;
use crate::util;

const ALPHABET: &str = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

pub fn print_results() {
    let input = util::parse_input("day_3.txt");
    let lines = input.lines();
    part_one(lines.clone());
    part_two(lines);
}

fn part_one(lines: Lines) {
    let mut score = 0;
    'lineloop: for line in lines {
        let length = line.chars().count();
        if length % 2 != 0 {
            println!("WORRY!");
        }
        let middle = length  / 2;
        let comp1 = &line[..middle];
        let comp2 = &line[middle..];
        for i in comp1.chars() {
            for n in comp2.chars() {
                if i.eq(&n) {
                    let priority = ALPHABET.find(i).expect("Something has gone wrong.");
                    score += priority;
                    continue 'lineloop;
                }
            }
        }
    }
    println!("Day 3 - Part 1 - Result: {}", score);
}

fn part_two(lines: Lines) {
    let lines_vec = lines.collect::<Vec<&str>>();
    let mut score = 0;
    'groups: for group in lines_vec.chunks(3) {
        let item_1 = group[0];
        let item_2 = group[1];
        let item_3 = group[2];
        let mut first_matches = String::from("");
        for i in item_1.chars() {
            for n in item_2.chars() {
                if i.eq(&n) {
                    first_matches.push(i);
                }
            }
        }
        for i in item_3.chars() {
            for n in first_matches.chars() {
                if i.eq(&n) {
                    score += ALPHABET.find(i).expect("Something has gone wrong.");
                    continue 'groups;
                }
            }
        }
    }
    println!("Day 3 - Part 2 - Result: {}", score);
}