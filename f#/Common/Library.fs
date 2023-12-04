namespace Common

module GridHelpers =
    let getSize(input: string array): int * int =
        input |> Array.item 0 |> String.length, input.Length