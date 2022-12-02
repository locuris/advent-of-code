use std::error::Error;
use crate::util;

pub fn print_result() -> Result<(), Box<dyn Error>> {
    let input = util::parse_input("day_1.txt");
    let lines  = input.lines();
    let mut summed = Vec::new();
    let mut total = 0;
    for line in lines {
        if line == "" {
            summed.push(total);
            total = 0;
        } else {
            let line_value: i32 = line.parse::<i32>().unwrap();
            total += line_value;
        }
    }

    summed.sort();
    let length = summed.len() - 1;

    let part_1_score = summed[length];
    let part_2_score: i32 = summed[length-2..].iter().sum();

    println!("Day 1 - Part 1 - Result: {:#?}", part_1_score);
    println!("Day 1 - Part 2 - Result: {:#?}", part_2_score);
    Ok(())

}