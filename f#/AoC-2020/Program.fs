﻿open System.IO
open day1
open day2
open day3
open day4
open day5
open day6

let main (day: int, part: int, test: bool) =

    let partIndex = part - 1
    
    let days = Map [
        (1, [day1Part1; day1Part2])
        (2, [day2part1; day2part2])
        (3, [day3part1; day3part2])
        (4, [day4part1; day4part2])
        (5, [day5part1; day5part2])
        (6, [day6part1])
    ]

    let dayFunction = days[day][partIndex]
    
    let file = if test then "test" else "input"

    let lines =
        File.ReadLines($"/Users/louis/GitHub/personal/advent-of-code/f#/Input/day{day}/{file}.txt") |> Array.ofSeq

    let answer = dayFunction lines
    printfn $"The answer is: {answer}"

    0

[<EntryPoint>]
let programArgs argv = main (6, 1, false)