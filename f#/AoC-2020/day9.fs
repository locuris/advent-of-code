module day9

let getCorruptNumber (lines: string array) : int64 array * int64 =
    let xmasCode = lines |> Array.map int64
    let preambleLength = 25

    let result =
        xmasCode
        |> Array.mapi (fun i x ->
            if i < preambleLength then
                false
            else
                let preamble = xmasCode.[i - preambleLength .. i - 1]
                let mutable isValid = false

                for a in preamble do
                    for b in preamble do
                        if a <> b && a + b = x then
                            isValid <- true

                isValid)
        |> Array.removeManyAt 0 preambleLength
        |> Array.findIndex not

    xmasCode, xmasCode.[result + preambleLength]

let day9part1 (lines: string array) : string =
    let _, corruptNumber = getCorruptNumber lines
    string corruptNumber

let day9part2 (lines: string array) : string =
    let xmasCode, corruptNumber = getCorruptNumber lines
    let mutable rangeStart = 0

    let ranges =
        xmasCode
        |> Array.mapi (fun i x ->
            if i = rangeStart then
                None
            else
                let mutable rangeSum = Array.sum xmasCode.[rangeStart..i]

                if rangeSum = corruptNumber then
                    Option.Some(rangeStart, i)
                elif rangeSum > corruptNumber then
                    while rangeSum > corruptNumber do
                        rangeSum <- rangeSum - xmasCode.[rangeStart]
                        rangeStart <- rangeStart + 1

                    if rangeSum = corruptNumber then
                        Option.Some(rangeStart, i)
                    else
                        None
                else
                    None)

    let validRanges = ranges |> Array.filter Option.isSome |> Array.map Option.get
    let range = validRanges.[0]

    let rangeValues = xmasCode.[fst range .. snd range]

    let min = Array.min rangeValues
    let max = Array.max rangeValues

    min + max |> string
