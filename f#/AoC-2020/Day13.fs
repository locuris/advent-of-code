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
    let _, busses = prepareNotes lines
    
    let busMap = busses |> Array.mapi (fun i bt -> (bt, i)) |> Array.filter (fun (bt, _) -> bt <> 1) |> Map.ofArray
    let busesSorted = busses |> Array.sortDescending
    
    let mutable found = false
    let mutable t = 0
    while not found do
        t <- t + 1
        let longestBus = busesSorted.[0]
        let longestBusTime = longestBus * t
        let longestBusIndex = busMap.[longestBus]
        found <- true
        
        for i = 1 to busesSorted.Length - 1 do
            let bus = busesSorted.[i]
            let busIndex = busMap.[bus]
            let diff = longestBusIndex + busIndex
            let busTime = longestBusTime - diff
            if found && busTime % bus <> 0 then
                found <- false
                t <- t + 1
            
    busses.[0] * t |> string
            