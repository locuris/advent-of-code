// For more information see https://aka.ms/fsharp-console-apps

open System.IO
open day1
open day2
open day3

printfn "Hello from F#"

let runForDay (day: int, part: int, test: bool, inputFilePath: string) =
    
    let partIndex = part - 1
    
    let days = Map [
        (1, [day1part1; day1part2])
        (2, [day2part1; day2part2])
        (3, [day3part1; day3part2])
    ]
    
    let dayFunction = days[day][partIndex]
    
    let file = if test then "test" else "input"
    
    let lines = File.ReadLines($"{inputFilePath}Input/2023/day{day}/{file}.txt") |> Array.ofSeq
    
    let answer = dayFunction lines
    printfn $"The answer is: {answer}"
    0

[<EntryPoint>]
let main argv =
    match argv with
    | [| day; part; test; filePath; |] ->
        runForDay (int day, int part, test = "test", filePath)
    | _ ->
        printfn "Two arguments please"
        1
            