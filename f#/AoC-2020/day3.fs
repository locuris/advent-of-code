module day3

type Point = { x: int; y: int }

// This is an AI assisted conversion of my original Python code - because I cba

let mutable width = 0
let mutable height = 0

let rideTheSlope slope area =
    let rec rideTheSlope' currentPoint trees =
        if currentPoint.y >= height then trees
        else
            let currentPoint = { x = currentPoint.x + slope.x; y = currentPoint.y + slope.y }
            if currentPoint.y >= height then trees
            else
                let currentPoint = 
                    if currentPoint.x >= width 
                    then { x = currentPoint.x - width; y = currentPoint.y }
                    else currentPoint
                let trees = 
                    if Map.find currentPoint area then trees + 1 
                    else trees
                rideTheSlope' currentPoint trees

    rideTheSlope' { x = 0; y = 0 } 0

let prepareArea (inputData: string array) =
    height <- inputData.Length
    width <- 0
    let mutable area = Map.empty
    for y = 0 to height - 1 do
        let line = inputData.[y]
        if width = 0 then width <- line.Length
        for x = 0 to line.Length - 1 do
            let point = { x = x; y = y }
            area <- area.Add(point, line.[x] = '#')
    area

let day3part1 (lines: string array) =
    let area = prepareArea lines
    let answer = rideTheSlope { x = 3; y = 1 } area
    answer.ToString()

let day3part2 (lines: string array) =
    let area = prepareArea lines
    let slopes = [| { x = 1; y = 1 }; { x = 3; y = 1 }; { x = 5; y = 1 }; { x = 7; y = 1 }; { x = 1; y = 2 } |]
    let mutable answer: int64 = 1
    for slope in slopes do
        let current_answer = rideTheSlope slope area
        printfn $"Current answer: {current_answer}"
        answer <- answer * (current_answer |> int64)
        printfn $"New answer: {answer}"
    answer.ToString()