// For more information see https://aka.ms/fsharp-console-apps

open System.Diagnostics
open System.IO

printfn "Hello from F#"

let days = Map [
        (1, [day1.part1; day1.part2])
        (2, [day2.part1; day2.part2])
        (3, [day3.part1; day3.part2])
        (4, [day4.part1; day4.part2])
        (5, [day5.part1; day5.part2])
        (6, [day6.part1; day6.part2])
        (7, [day7.part1; day7.part2])
        (8, [day8.part1; day8.part2])
        (9, [day9.part1; day9.part2])
        (10, [day10.part1; day10.part2])
        (11, [day11.part1; day11.part2])
        (12, [day12.part1; day12.part2])
        (13, [day13.part1; day13.part2])
        (14, [day14.part1; day14.part2])
        (15, [day15.part1; day15.part2])
        (16, [day16.part1; day16.part2])
        (17, [day17.part1; day17.part2])
        (18, [day18.part1; day18.part2])
        (19, [day19.part1; day19.part2])
        (20, [day20.part1; day20.part2])
        (21, [day21.part1; day21.part2])
        (22, [day22.part1; day22.part2])
        (23, [day23.part1; day23.part2])
        (24, [day24.part1; day24.part2])
        (25, [day25.part1; day25.part2])
    ]

let getInputSpec(day: int) (file: string) =
    File.ReadLines($"{Directory.GetCurrentDirectory()}/../../../../Input/{2023}/day{day}/{file}.txt") |> Array.ofSeq

let getInput(day: int) (test: bool) =
    let file = if test then "test" else "input"
    getInputSpec day file

let getAnswer(day: int) (part: int) (input: string array) =
    let stopwatch = Stopwatch.StartNew() 
    let answer = days[day][part - 1](input)
    stopwatch.Stop()
    printfn $"Answer: {answer}"
    printfn $"   in {stopwatch.ElapsedMilliseconds}ms"


let runAllDays() =
    days |> Map.iter (fun day _ ->        
        let file = getInput day false
        let test = getInput day true
        let stopwatch = Stopwatch.StartNew()
        days[day][0] test |> ignore
        days[day][0] file |> ignore
        days[day][1] test |> ignore
        days[day][1] file |> ignore
        stopwatch.Stop()
        printfn $"Day {day} ran in {stopwatch.ElapsedMilliseconds} ms")
    

[<EntryPoint>]
let main argv =    
    (*runAllDays()*)
    let day = 16
    let part = 1
    
    let input = getInput day true
    getAnswer day part input
    0