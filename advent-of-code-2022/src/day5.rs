use std::borrow::Borrow;
use std::collections::HashMap;
use regex::{CaptureMatches, Regex};
use itertools::Itertools;

use crate::util;

pub fn print_results() {
    let input = util::parse_input("day_5.txt");
    let groups = Regex::new(r"\n\n").unwrap().split(&input).collect::<Vec<&str>>();
    let mut stacks = parse_stack(groups[0]);
    /*let mut stacks = &mut HashMap::new();
    let lines = groups[0].lines();
    let stack_info = lines.clone().last().unwrap();
    let stack_count = Regex::new(r"(\d)").unwrap().captures_iter(stack_info).count();
    println!("Stack Info: {}\nStack Count: {}", stack_info, stack_count);
    for line in lines.rev() {
        if line.eq(stack_info) {
            continue;
        }
        let mut found_bracket = false;
        let mut i = 0;
        for c in line.chars() {
            i += 1;
            if c.to_string().eq("[") {
                found_bracket = true;
            }
            else if (i + 2) % 4 == 0 {
                let stack_number = (i + 2) / 4;
                println!("Stack number {}", stack_number);
                if !c.to_string().eq(" ") && found_bracket {
                    let mut stack = stacks.entry(stack_number).or_insert( Vec::new());
                    stack.push(c.to_string());
                }
            }
            else if c.to_string().eq("]") {
                found_bracket = false;
            }
        }
    }*/
    let mut actions = Vec::new();
    for line in groups[1].lines() {
        actions.push(parse_action(line));
    }
    for action in actions {
        println!("Action move {} from {} to {}", action.0, action.1, action.2);
        let mut moving_crates = Vec::new();
        for i in 0..action.0 {
            let mut stack = &mut stacks.get_mut(&action.1).unwrap();
            let c = stack.pop().unwrap();
            //&mut stacks.get_mut(&action.2).unwrap().push(c.to_string());
            moving_crates.push(c);
        }
        for c in moving_crates.iter().rev() {
            &mut stacks.get_mut(&action.2).unwrap().push(c.to_string());
        }
        print_stack(stacks);
    }
    for key in stacks.keys().sorted() {
        println!("Stack {} top crate {}", key, &stacks.get(key).unwrap().last().unwrap());
    }
}

fn parse_stack(stack: &str) -> &mut HashMap<i32, &mut Vec<String>> {
    let mut stacks = &mut HashMap::new();
    let lines = stack.lines();
    let stack_info = lines.clone().last().unwrap();
    for line in lines.rev() {
        if line.eq(stack_info) {
            continue;
        }
        let mut found_bracket = false;
        let mut i = 0;
        for c in line.chars() {
            i += 1;
            if c.to_string().eq("[") {
                found_bracket = true;
            }
            else if (i + 2) % 4 == 0 {
                let stack_number = (i + 2) / 4;
                if !c.to_string().eq(" ") && found_bracket {
                    let stack = stacks.entry(stack_number).or_insert(&mut Vec::new());
                    stack.push(c.to_string());
                }
            }
            else if c.to_string().eq("]") {
                found_bracket = false;
            }
        }
    }
    stacks
}

fn parse_action(input: &str) -> (i32, i32, i32) {
    let mut groups = Regex::new(r"\D").unwrap().split(&input).collect::<Vec<&str>>();
    groups.retain(|&x| x != "");
    (groups[0].parse().unwrap(), groups[1].parse().unwrap(), groups[2].parse().unwrap())
}

fn print_stack(stacks: &mut HashMap<i32, &mut Vec<String>>) {
    for stack in stacks.keys().clone() {
        print!("Stack {}:", stack);
        for crates in stacks.get_mut(stack).unwrap().iter() {
            print!(" [{}] ", &crates);
        }
        print!("\n");
    }
}