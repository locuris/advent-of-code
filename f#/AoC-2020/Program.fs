open System.IO
open day1
open day2
open day3
open day4
open day5
open day6
open day7
open day8
open day17
open day17InFourDimensions
open day9
open day10
    
let main (day: int, part: int, test: bool, inputFilepath: string) =

    let partIndex = part - 1
    
    let days = Map [
        (1, [day1Part1; day1Part2])
        (2, [day2part1; day2part2])
        (3, [day3part1; day3part2])
        (4, [day4part1; day4part2])
        (5, [day5part1; day5part2])
        (6, [day6part1; day6part2])
        (7, [day7part1; day7part2])
        (8, [day8part1; day8part2])
        (9, [day9part1; day9part2])
        (10, [day10part1; day10part2])
        (11, [])
        (12, [])
        (13, [])
        (14, [])
        (15, [])
        (16, [])
        (17, [day17part1; day17part2])
    ]

    let dayFunction = days[day][partIndex]
    
    let file = if test then "test" else "input"

    let lines =
        File.ReadLines($"{inputFilepath}Input/day{day}/{file}.txt") |> Array.ofSeq

    let answer = dayFunction lines
    printfn $"The answer is: {answer}"

    0

[<EntryPoint>]
let programArgs argv = main (int argv.[0], int argv.[1], argv.[2] = "test", argv.[3])