use crate::util;

pub fn print_results() {
    let input = util::parse_input("day_6.txt");
    let lines = input.lines();
    for line in lines {
        println!("Day 6 - Part 1 - Result: {}", find_marker(line, 4));
        println!("Day 6 - Part 2 - Result: {}", find_marker(line, 14));
    }
}

fn find_marker(input: &str, marker_size: usize) -> usize {
    let input_length = input.chars().count();
    let mut answer = 0;
    'lp: for i in 0..(input_length - marker_size) {
        let marker = &input[i..i+marker_size];
        let mut n = 0;
        for c in marker.chars() {
            if n == 0 {
                n += 1;
                continue;
            }
            let s = c.to_string();
            if marker[..n].contains(&s) || marker[n + 1..].contains(&s) {
                continue 'lp;
            }
            n += 1;
        }
        answer = i + marker_size;
        break;
    }
    answer
}