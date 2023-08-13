module day13

let prepareNotes (lines: string array) (includeX: bool): int * int array =
    let earliest = int lines.[0]
    let buses =
        if includeX then
            lines.[1].Split(',') |> Array.map (fun x -> if x = "x" then 1 else int x)
        else
            lines.[1].Split(',') |> Array.filter (fun x -> x <> "x") |> Array.map int
    (earliest, buses)

let day13part1 (lines: string array) : string =
    let earliest, buses = prepareNotes lines false
    let bus, wait = buses |> Array.map (fun bt -> (bt, bt - (earliest % bt))) |> Array.minBy snd
    bus * wait |> string
    
// Extended Euclidean Algorithm
let rec extendedGCD a b =
    if a = 0L then (b, 0L, 1L)
    else
        let (g, x, y) = extendedGCD (b % a) a
        (g, y - (b / a) * x, x)

// Function to find mod inverse of 'a' under modulo 'm'
let modInverse a m =
    let (_, x, _) = extendedGCD a m
    (x % m + m) % m

// Function to solve the Chinese Remainder Theorem problem
let chineseRemainder (num: int64 list) (rem: int64 list) =
    let prod = List.reduce (*) num
    let result =
        List.fold2 (fun acc a m ->
            let pp = prod / m
            acc + a * modInverse pp m * pp
        ) 0L rem num

    result % prod
    
let day13part2 (lines: string array) : string =
    let _, buses = prepareNotes lines true
    
    let num = buses |> Array.map int64
    let rem = buses |> Array.map int64 |> Array.mapi (fun i x -> x - int64 i)
    
    chineseRemainder (List.ofArray num) (List.ofArray rem) |> string