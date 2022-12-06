use std::borrow::Borrow;
use std::fs;

const FILEPATH: &str = "data/";

pub fn parse_input(filename: &str) -> String {
    let path = String::from(FILEPATH) + filename;
    let result = fs::read_to_string(&path).expect(&*format!("Could not read file from path \"{}\"", path));
    result
}