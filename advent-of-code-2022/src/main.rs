mod day1;
mod util;
mod day2;
mod day3;
mod day4;

fn main() {
    day1::print_result().expect("Something went wrong");
    day2::print_results().expect("Something went wrong");
    day3::print_results();
    day4::print_results();
}