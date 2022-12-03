mod day1;
mod util;
mod day2;
mod day3;

fn main() {
    day1::print_result().expect("Something went wrong");
    day2::print_results().expect("Something went wrong");
    day3::print_results();
}