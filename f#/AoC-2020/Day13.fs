module day13

let prepareNotes (lines: string array) : int * int array =
    let earliest = int lines.[0]
    let buses = lines.[1].Split(',') |> Array.filter (fun x -> x <> "x") |> Array.map int
    (earliest, buses)

let day13part1 (lines: string array) : string =
    let earliest, buses = prepareNotes lines
    let bus, wait = buses |> Array.map (fun bt -> (bt, bt - (earliest % bt))) |> Array.minBy snd
    bus * wait |> string
    
let day13part2 (lines: string array) : string =
    "NOT DONE"
