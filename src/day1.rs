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
    summed.reverse();
    let mut i = 0;
    let mut backup = 0;
    for sum in &summed {
        i += 1;
        if i > 0 && i < 4 {
            backup += sum;
        }
        continue;
    }

    match summed.first() {
        Some(x) => println!("Day 1 - Part 1 - Result: {x}"),
        None => println!("You buggered up!")
    }
    println!("Day 1 - Part 2 - Result: {:#?}", backup);
    Ok(())

}