use std::fs;

const FILEPATH: &str = "data/";

pub fn parse_input(filename: &str) -> String {
    let result = fs::read_to_string(String::from(FILEPATH) + filename).expect("Could not read file.");
    result
}