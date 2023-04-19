use std::error::Error;
use crate::util;

pub fn print_results() -> Result<(), Box<dyn Error>> {
    let input = util::parse_input("day_2.txt");
    let lines = input.lines();
    let mut part_1_score = 0;
    let mut part_2_score = 0;
    for line in lines {
        let actions = line.split(" ").collect::<Vec<&str>>();
        part_1_score += score(actions[0], convert(actions[1]));
        part_2_score += score(actions[0], convert_part_ii(actions[1], actions[0]));
    }
    println!("Day 2 - Part 1 - Result: {:#?}", part_1_score);
    println!("Day 2 - Part 2 - Result: {:#?}", part_2_score);
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

fn convert_part_ii<'a>(action: &'a str, them: &'a str) -> &'a str {
    match action {
        "X" => lose_action(them),
        "Y" => them,
        "Z" => win_action(them),
        _ => {
            println!("Something has gone wrong!");
            "!"
        }
    }
}

fn win_action(action: &str) -> &str {
    match action {
        "A" => "B",
        "B" => "C",
        "C" => "A",
        _ => {
            println!("Something has gone wrong!");
            "!"
        }
    }
}

fn lose_action(action: &str) -> &str {
    match action {
        "A" => "C",
        "B" => "A",
        "C" => "B",
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
    you.eq(win_action(them))
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