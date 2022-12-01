use std::fs;
use std::error::Error;

fn main() -> Result<(), Box<dyn Error>> {
    let input = fs::read_to_string("Data/day_1.txt")?;
    let lines = input.lines();
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

    println!("{:#?}", summed.last());
    Ok(())
}