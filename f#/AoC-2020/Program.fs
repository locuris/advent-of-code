open utilities
open day1
open day2
open day3
open day4
open day5

let main (day: int, part: int, test: bool) =

    let partIndex = part - 1
    
    let days = Map [
        (1, [day1Part1; day1Part2])
        (2, [day2part1; day2part2])
        (3, [day3part1; day3part2])
        (4, [day4part1; day4part2])
        (5, [day5part1; day5part2])
    ]

    let dayFunction = days[day][partIndex]
    
    let file = if test then "test" else "input"

    let lines =
        readFile $"/Users/louis/GitHub/personal/advent-of-code/f#/Input/day{day}/{file}.txt"

    let answer = dayFunction lines
    printfn $"The answer is: {answer}"

    0

[<EntryPoint>]
let programArgs argv = main (5, 2, false)
