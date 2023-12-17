module day12

open Common


type Spring =
    | Operational of int
    | Damaged of int
    | Unknown of int
    | Any of int * int


let mapSprings (line: string) : Spring array =
    line.ToCharArray()
    |> Array.mapi (fun i spring ->
        match spring with
        | s when s = '#' -> Damaged(i)
        | s when s = '.' -> Operational(i)
        | s when s = '?' -> Unknown(i)
        | _ -> failwithf $"Unrecognised argument {spring}")

let mapToAny (operational: Spring) (gap: int) (pattern: int array) (maxLength: int) : Spring =
    match operational with
    | Operational index ->
        let patternOffset = (pattern[gap..] |> Array.sum) + (if gap = 1 then 1 else 0)

        Any(index, maxLength - index - patternOffset)
    | _ -> failwithf $"Can only handle Operational {operational}"



let mapPattern (maxLength: int) (line: string) : Spring array =
    let pattern = line.Split(',') |> Array.map int
    let mutable index = 0

    let springPattern =
        [| for length in pattern do
               let oldIndex = index
               let newIndex = index + length
               index <- newIndex + 1

               for i in oldIndex..newIndex do

                   if i = newIndex then Operational(i) else Damaged(i) |]

    let mutable gap = 0

    springPattern[.. (springPattern.Length - 2)]
    |> Array.map (fun spring ->
        match spring with
        | Operational _ ->
            gap <- gap + 1
            mapToAny spring gap pattern maxLength
        | _ -> spring)

let checkPossibility (springs: Spring array) (pattern: Spring array) (offset: int) : bool =
    not (
        pattern
        |> Array.mapi (fun i spring ->
            let otherSpring = springs[i + offset]

            match otherSpring with
            | os when os.GetType() = spring.GetType() -> false
            | Unknown _ -> false
            | _ -> true)
        |> Array.exists id
    )
    


let check (springs: Spring array) (pattern: Spring array) (offset: int) : int =
    pattern
    |> Array.map (fun spring ->
        match spring with
        | Any (start, end_) ->
            springs[]
        | Damaged index -> springs[index + offset].GetType() <> spring.GetType())
    |> Array.head


let checkPossibilities ((springs, pattern): Spring array * Spring array) : int =
    let answer =
        [| 0 .. (springs.Length - pattern.Length) |]
        |> Array.sumBy (fun offset -> if checkPossibility springs pattern offset then 1 else 0)

    printfn $"{answer}"
    answer

let part1 (lines: string array) : string =
    lines
    |> Array.map (fun line ->
        let lineComps = line.Split(' ')
        mapSprings (lineComps[0]), mapPattern lineComps[0].Length lineComps[1])
    |> Array.sumBy checkPossibilities
    |> string

let part2 (line: string array) : string = ""
