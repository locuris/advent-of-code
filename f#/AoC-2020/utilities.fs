module utilities

let getLinesGroupedByNewLine (lines: string array): List<string array> =
    let finalList = ResizeArray<string array>()
    let currentList = ResizeArray<string>()
    for line in lines do
        if not (line = "") then
            currentList.Add(line)
        else
            finalList.Add(currentList.ToArray())
            currentList.Clear()
            
    finalList.Add(currentList.ToArray())
    finalList |> List.ofSeq