use std::error::Error;
use crate::util;

pub fn print_results() -> Result<(), Box<dyn Error>> {
    let input = util::parse_input("day_2.txt");
    let lines = input.lines();
    let mut total_score = 0;
    for line in lines {
        let actions = line.split(" ").collect::<Vec<&str>>();
        total_score += score(actions[0], convert(actions[1]));
    }
    println!("NEW: {:#?}", total_score);
    Ok(())
}
//ROCK      A | X | +1 | Paper
//PAPER     B | Y | +2 | Scissors
//SCISSORS  C | Z | +3 | Rock
// Part II
// X = Lose
// Y = Draw
// Z = Win
fn convert(action: &str) -> &str {
    match action {
        "X" => "A",
        "Y" => "B",
        "Z" => "C",
        _ => {
            println!("Something has gone wrong!");
            "!"
        }
    }
}

fn outcome(them: &str, you: &str) -> i32 {
    if them.eq(you) {
        return 3;
    }
    if won(them, you) {
        return 6;
    }
    0
}

fn won(them: &str, you: &str) -> bool {
    (them == "C" && you == "A") || (them == "B" && you == "C") || (them == "A" && you == "B")
}

fn option_value(option: &str) -> i32 {
    match option {
        "A" => 1,
        "B" => 2,
        "C" => 3,
        _ => {
            println!("Something has gone wrong");
            -99
        }
    }
}

fn score(them: &str, you: &str) -> i32 {
    outcome(them, you) + option_value(you)
}