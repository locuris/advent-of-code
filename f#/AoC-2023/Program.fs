// For more information see https://aka.ms/fsharp-console-apps

open System.Diagnostics
open System.IO
open System.Threading
open day1
open day2
open day3
open day4
open day5
open Common.Input

printfn "Hello from F#"

let runForDay (day: int, part: int, test: bool, inputFilePath: string) =
    
    let partIndex = part - 1
    
    let days = Map [
        (1, [day1.part1; day1.part2])
        (2, [day2.part1; day2.part2])
        (3, [day3.part1; day3.part2])
        (4, [day4.part1; day4.part2])
        (5, [day5.part1; day5.part2])
    ]
    
    let dayFunction = days[day][partIndex]
    
    let file = if test then "test" else "input"
    
    let lines = File.ReadLines($"{inputFilePath}Input/2023/day{day}/{file}.txt") |> Array.ofSeq
    
    let stopwatch = Stopwatch.StartNew()
    let answer = dayFunction lines
    stopwatch.Stop()
    printfn $"Got the answer, {answer}, in {stopwatch.ElapsedMilliseconds}ms ❤️"
    0

[<EntryPoint>]
let main argv =
    match argv with
    | [| day; part; test; filePath; |] ->
        runForDay (int day, int part, test = "test", filePath)
    | _ ->
        let day, part, test = mainMenu()
        runForDay(day, part, test, "C:/Users/louis/Personal/advent-of-code/f#/")
            