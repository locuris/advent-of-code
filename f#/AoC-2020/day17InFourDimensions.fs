module day9InFourDimensions

let initialState (lines: string array) : bool array4d =
    let size = lines.Length
    let middle = (size - 1) / 2
    let cube: bool array4d = Array4D.init size size size size (fun _ _ _ _ -> false)

    for x in 0 .. size - 1 do
        for y in 0 .. size - 1 do
            if lines.[x].[y] = '#' then
                cube.[x, y, middle, middle] <- true

    cube

let checkNeighbours (cube: bool array4d) (x: int) (y: int) (z: int) (w: int) : int =
    let offsets = [ -1; 0; 1 ]

    let neighbours =
        [ for dx in offsets do
              for dy in offsets do
                  for dz in offsets do
                      for dw in offsets do
                          if dx <> 0 || dy <> 0 || dz <> 0 || dw <> 0 then
                              yield x + dx, y + dy, z + dz, w + dw ]

    let mutable active = 0

    for x, y, z, w in neighbours do
        if
            x >= 0
            && x < cube.GetLength(0)
            && y >= 0
            && y < cube.GetLength(1)
            && z >= 0
            && z < cube.GetLength(2)
            && w >= 0
            && w < cube.GetLength(3)
            && cube.[x, y, z, w]
        then
            active <- active + 1

    active

let executeCycle (cube: bool array4d) : bool array4d =
    let size = cube.GetLength(0)
    let mutable newCube = Array4D.init size size size size (fun _ _ _ _ -> false)

    for x in 0 .. cube.GetLength(0) - 1 do
        for y in 0 .. cube.GetLength(1) - 1 do
            for z in 0 .. cube.GetLength(2) - 1 do
                for w in 0 .. cube.GetLength(3) - 1 do
                    let activeNeighbours = checkNeighbours cube x y z w
                    let state = Array4D.get cube x y z w

                    Array4D.set
                        newCube
                        x
                        y
                        z
                        w
                        (if state then
                             activeNeighbours = 2 || activeNeighbours = 3
                         else
                             activeNeighbours = 3)
    newCube


let executeCycles (cube: bool array4d) (cycles: int) : bool array4d =
    let mutable cube = cube

    for _ in 1..cycles do
        let size = cube.GetLength(0)
        let newSize = size + 2

        let newCube: bool array4d =
            Array4D.init newSize newSize newSize newSize (fun _ _ _ _ -> false)

        for x in 0 .. size - 1 do
            for y in 0 .. size - 1 do
                for z in 0 .. size - 1 do
                    for w in 0 .. size - 1 do
                        Array4D.set newCube (x + 1) (y + 1) (z + 1) (w + 1) (Array4D.get cube x y z w)

        cube <- newCube
        cube <- executeCycle cube

    cube

let day17part2 (lines: string array) : string =
    let mutable cube = initialState lines
    cube <- executeCycles cube 6
    let mutable activeCubes = 0

    for x in 0 .. cube.GetLength(0) - 1 do
        for y in 0 .. cube.GetLength(1) - 1 do
            for z in 0 .. cube.GetLength(2) - 1 do
                for w in 0 .. cube.GetLength(3) - 1 do
                    if Array4D.get cube x y z w then
                        activeCubes <- activeCubes + 1

    activeCubes |> string
