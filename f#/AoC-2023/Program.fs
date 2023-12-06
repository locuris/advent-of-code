// For more information see https://aka.ms/fsharp-console-apps

open System.Diagnostics
open System.IO
open System.Threading
open day1
open day2
open day3
open day4
open day5
open day6
open Common.Input

printfn "Hello from F#"

let days = Map [
        (1, [day1.part1; day1.part2])
        (2, [day2.part1; day2.part2])
        (3, [day3.part1; day3.part2])
        (4, [day4.part1; day4.part2])
        (5, [day5.part1; day5.part2])
        (6, [day6.part1; day6.part2])
    ]

let getInputSpec(day: int) (file: string) =
    File.ReadLines($"C:/Users/louis/Personal/advent-of-code/f#/Input/{2023}/day{day}/{file}.txt") |> Array.ofSeq

let getInput(day: int) (test: bool) =
    let file = if test then "test" else "input"
    getInputSpec day file

let getAnswer(day: int) (part: int) (input: string array) =
    let stopwatch = Stopwatch.StartNew() 
    let answer = days[day][part - 1](input)
    stopwatch.Stop()
    printfn $"Got the answer, {answer} , in {stopwatch.ElapsedMilliseconds}ms ❤️"
    
    

[<EntryPoint>]
let main argv =    
    let test = false
    let day = 6
    let part = 2
    let file = getInput day test
    getAnswer day part file
    0
            