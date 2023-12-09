module day9


type history = int array list

let rec extrapolate (numbers: history) : history =
    if numbers |> List.head |> Array.forall (fun n -> n = 0) then
        numbers
    else
        numbers |> List.append [numbers |> List.head |> Array.pairwise |> Array.map (fun (first, second) -> second - first)] |> extrapolate
                
let getHistories (lines: string array) : history array =
    lines
    |> Array.map (fun line ->
        line.Split(' ')
        |> Array.map int)
    |> Array.map (fun seq -> extrapolate [seq])
    
let getAnswer (histories: history array) : int =
    histories |> Array.sumBy (fun histories -> histories |> List.sumBy (fun history -> history |> Array.last))


let part1 (lines: string array) : string =
    lines
    |> getHistories
    |> getAnswer
    |> string    
    
let changeDirections (histories: history array) : history array =
    histories |> Array.map (fun history -> [history |> List.last |> Array.rev])
    
let part2 (lines: string array) : string =
    let histories = lines |> getHistories    
    histories
    |> changeDirections
    |> Array.map extrapolate
    |> getAnswer
    |> string
