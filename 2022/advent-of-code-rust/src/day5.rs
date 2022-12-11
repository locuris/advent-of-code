use regex::Regex;

use crate::util;

pub fn print_results() {
    let input = util::parse_input("day_5.txt");
    let groups = Regex::new(r"\n\n").unwrap().split(&input).collect::<Vec<&str>>();
    let mut stacks = parse_stack(groups[0]);
    let actions = parse_actions(groups[1]);
    let moved_stacks_1 = move_crates_part1(stacks.clone(), actions.clone());
    let moved_stacks_2 = move_crates_part2(stacks.clone(), actions.clone());
    let answer_1 = format_answer(moved_stacks_1);
    let answer_2 = format_answer(moved_stacks_2);
    println!("Day 5 - Part 1 - Result: {}", answer_1);
    println!("Day 5 - Part 2 - Result: {}", answer_2);
}

fn parse_stack(stack: &str) -> Vec<Vec<String>> {
    let mut stacks = vec![vec![]];
    let lines = stack.lines();
    let stack_info = lines.clone().last().unwrap();
    for line in lines.rev() {
        if line.eq(stack_info) {
            continue;
        }
        let mut i = 0;
        for c in line.chars() {
            i += 1;
            if (i + 2) % 4 == 0 {
                let stack_number = (i - 2) / 4;
                if !c.to_string().eq(" ") {
                    match stacks.get_mut(stack_number) {
                        None => stacks.push(vec![c.to_string()]),
                        _ => stacks.get_mut(stack_number).unwrap().push(c.to_string())
                    }
                }
            }
        }
    }
    stacks
}

fn parse_actions(input: &str) -> Vec<(usize,usize,usize)> {
    let mut actions = vec![];
    let lines = input.lines();
    for line in lines {
        let mut groups = Regex::new(r"\D").unwrap().split(line).collect::<Vec<&str>>();
        groups.retain(|&x| x != "");
        actions.push((groups[0].parse().unwrap(), groups[1].parse().unwrap(), groups[2].parse().unwrap()));
    }
    actions
}

fn move_crates_part1(mut stacks: Vec<Vec<String>>, actions: Vec<(usize, usize, usize)>) -> Vec<Vec<String>> {
    for action in actions {
        for _ in 0..action.0 {
            let stack = stacks.get_mut(action.1 - 1).unwrap();
            let c = stack.pop().unwrap();
            stacks.get_mut(action.2 - 1).unwrap().push(c.to_string());
        }
    }
    stacks
}

fn move_crates_part2(mut stacks: Vec<Vec<String>>, actions: Vec<(usize, usize, usize)>) -> Vec<Vec<String>> {
    for action in actions {
        let mut moving_crates = vec![];
        for _ in 0..action.0 {
            let stack = stacks.get_mut(action.1 - 1).unwrap();
            let c = stack.pop().unwrap();
            moving_crates.push(c);
        }
        for c in moving_crates.iter().rev() {
            stacks.get_mut(action.2 - 1).unwrap().push(c.to_string());
        }
    }
    stacks
}

fn format_answer(stacks: Vec<Vec<String>>) -> String {
    let mut answer = String::from("");
    for stack in stacks {
        answer = answer + stack.last().unwrap();
    }
    answer
}