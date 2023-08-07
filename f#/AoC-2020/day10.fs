module day10

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
    let jolts = getJolts lines |> Array.toList
    
    
    "Hello"
