module day10

open System.Collections.Generic

let getJolts (lines: string array): int array =
    lines |> Array.map int |> Array.append [|0|] |> Array.sort

let day10part1 (lines: string array): string =
    let jolts = getJolts lines
    let diffs = jolts |> Array.mapi (fun i j -> if i = 0 then 0 else j - jolts.[i-1])
    
    let ones = diffs |> Array.filter ((=) 1) |> Array.length
    let threes = (diffs |> Array.filter ((=) 3) |> Array.length) + 1
    
    printfn $"{ones} difference(s) of 1 and {threes} difference(s) of 3"
    ones * threes |> string
    
let day10part2 (lines: string array): string =
    let mutable jolts = getJolts lines |> Array.toList
    let maxJolt = (jolts |> List.max) + 3
    jolts <- jolts |> List.append [maxJolt] |> List.sort
    
    let paths = Dictionary<int, int64>()
    paths.[0] <- 1L
    
    for jolt in jolts do
        for diff in 1..3 do
            if paths.ContainsKey(jolt - diff) then
                if paths.ContainsKey(jolt) then
                    paths.[jolt] <- paths.[jolt] + paths.[jolt - diff]
                else
                    paths.Add(jolt, paths.[jolt - diff])
    
    paths.[maxJolt] |> string